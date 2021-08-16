using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Core.Interfaces;
using RPG.Saving.Interfaces;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1.21f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform lefttHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Weapon currentWeapon = null;

        Health target;
        Mover mover;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!IsInRange())
            {
                mover.MoveTo(target.transform.position, 1);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        public Health GetTarget()
        {
            return target;
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            currentWeapon.Spawn(rightHandTransform, lefttHandTransform, animator);
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
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

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTeste = combatTarget.GetComponent<Health>();

            return targetToTeste != null && !targetToTeste.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        //  PRIVATES
        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetRange();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        //  ANIMATION EVENTS
        public void Hit()
        {
            if (target == null) return;

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LauchProjectile(rightHandTransform, lefttHandTransform, target, gameObject);
            }
            else
            {
                target.TakeDamage(gameObject, currentWeapon.GetDamage());
            }
        }

        public void Shoot()
        {
            Hit();
        }

        //  INTERFACES
        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}