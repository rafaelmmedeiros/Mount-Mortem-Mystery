using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Core.Interfaces;
using RPG.Saving.Interfaces;
using RPG.Attributes;
using RPG.Stats;
using RPG.Stats.Enums;
using RPG.Stats.Interfaces;
using System.Collections.Generic;
using RPG.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1.21f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        LazyValue<WeaponConfig> currentWeapon;

        Health target;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            currentWeapon = new LazyValue<WeaponConfig>(SetupDefaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public Health GetTarget()
        {
            return target;
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
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

        private void AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.value.GetRange();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        private WeaponConfig SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //  ANIMATION EVENTS
        public void Hit()
        {
            if (target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LauchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            //  It happen only when the stat requested is Damage
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
    }
}