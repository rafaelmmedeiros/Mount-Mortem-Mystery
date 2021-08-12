using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject equipedPrefab = null;
        [SerializeField] AnimatorOverrideController animationOverride = null;
        [SerializeField] float damage = 0;
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
                GameObject weapon = Instantiate(equipedPrefab, handTransform);
                weapon.name = weaponName;
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

        public void LauchProjectile(Transform righHand, Transform leftHand, Health target)
        {
            Projectile projectileToLauch = Instantiate(projectile, GetTransformHand(righHand, leftHand).position, Quaternion.identity);
            projectileToLauch.SetTarget(target, damage);
        }

        public float GetDamage()
        {
            return damage;
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

