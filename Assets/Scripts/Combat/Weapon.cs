using RPG.Core;
using System;
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

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equipedPrefab != null)
            {
                Transform handTransform = GetTransformHand(rightHand, leftHand);
                Instantiate(equipedPrefab, handTransform);
            }

            if (animationOverride != null)
            {
                animator.runtimeAnimatorController = animationOverride;
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

        private Transform GetTransformHand(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHand) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }
    }
}

