using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Combat/New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] Weapon equipedPrefab = null;
        [SerializeField] AnimatorOverrideController animationOverride = null;
        [SerializeField] float damage = 0;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] float range = 0;
        [SerializeField] bool isRightHand = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (equipedPrefab != null)
            {
                Transform handTransform = GetTransformHand(rightHand, leftHand);
                Weapon weapon = Instantiate(equipedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }

            //  Casting to animation return to default
            var overrideCOntroller = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animationOverride != null)
            {
                animator.runtimeAnimatorController = animationOverride;
            }
            else if (overrideCOntroller != null)
            {
                animator.runtimeAnimatorController = overrideCOntroller.runtimeAnimatorController;
            }
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LauchProjectile(Transform righHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileToLauch = Instantiate(projectile, GetTransformHand(righHand, leftHand).position, Quaternion.identity);
            projectileToLauch.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public float GetRange()
        {
            return range;
        }

        //  PRIVATES
        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROY"; //  Added to avoid conflicts
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransformHand(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHand) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }
    }
}

