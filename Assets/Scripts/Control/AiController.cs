using RPG.Combat;
using RPG.Core;
using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using RPG.Utils;
using System;

namespace RPG.Control
{
    public class AiController : MonoBehaviour
    {
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float chaseDistance = 5f;

        [SerializeField] float suspiciousTime = 5f;
        [SerializeField] float aggreveteCoolTime = 5f;

        [SerializeField] float wayPointTolerance = 1f;
        [SerializeField] float wayPointDwellTime = 3f;

        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        LazyValue<Vector3> guardPosition;
        int currentWayPointIndex = 0;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWayPoint = Mathf.Infinity;
        float timeSinceAggreveted = Mathf.Infinity;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (IsAggreveted() && fighter.CanAttack(player))
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspiciousTime)
            {
                SuspiciousBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();
        }

        private bool IsAggreveted()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggreveted < aggreveteCoolTime;
        }

        public void Aggrevete()
        {
            timeSinceAggreveted = 0;
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private void SuspiciousBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWayPoint = 0;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }
            if (timeSinceArrivedAtWayPoint > wayPointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWayPoint < wayPointTolerance;
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWaypoint(currentWayPointIndex);
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWayPoint += Time.deltaTime;
            timeSinceAggreveted += Time.deltaTime;
        }

        //  DEBUG
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

