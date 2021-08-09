using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Core.Interfaces;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBetweenAttacks = 1.21f;
        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon weapon = null;

        Health targetHealth;
        Mover mover;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            SpawnWeapon();
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
        private void SpawnWeapon()
        {
            if (weapon == null) return;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(handTransform, animator);
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
            targetHealth.TakeDamage(weapon.GetDamage());
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetHealth.transform.position) < weapon.GetRange();
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