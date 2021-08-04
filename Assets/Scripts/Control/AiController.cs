using RPG.Combat;
using RPG.Core;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    public class AiController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspiciousTime = 5f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        // Guard Behavior
        Vector3 guardPosition;
        float timeLastSawPlayer = Mathf.Infinity;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeToPlayer() && fighter.CanAttack(player))
            {
                timeLastSawPlayer = 0;
                AttackBehavior();
            }
            else if (timeLastSawPlayer < suspiciousTime)
            {
                SuspiciousBehavior();
            }
            else
            {
                GuardBehavior();
            }

            timeLastSawPlayer += Time.deltaTime;
        }

        private void GuardBehavior()
        {
            mover.StartMoveAction(guardPosition);
        }

        private void SuspiciousBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeToPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        //  UNITY ONLY
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

