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

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equipedPrefab != null)
            {
                if (isRightHand)
                {
                    Instantiate(equipedPrefab, rightHand);
                }
                else
                {
                    Instantiate(equipedPrefab, leftHand);
                }
            }

            if (animationOverride != null)
            {
                animator.runtimeAnimatorController = animationOverride;
            }
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetRange()
        {
            return range;
        }
    }
}

