using System.Collections;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Core.Interfaces;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1.21f;
        [SerializeField] float weaponDamage = 5f;

        Health targetHealth;
        Mover mover;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (targetHealth == null) return;
            if (targetHealth.IsDead()) return;

            if (!GetIsInRange())
            {
                mover.MoveTo(targetHealth.transform.position, 1);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(targetHealth.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //  This will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;

            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //  Animation Event
        public void Hit()
        {
            if (targetHealth == null) return;
            targetHealth.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetHealth.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTeste = combatTarget.GetComponent<Health>();

            return targetToTeste != null && !targetToTeste.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            targetHealth = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            targetHealth = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}