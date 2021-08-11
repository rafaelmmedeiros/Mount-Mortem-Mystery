using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 15f;
        [SerializeField] bool IsHoming = true;
        float damage = 0;

        Health target = null;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (target == null) return;

            if (IsHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;

            target.TakeDamage(damage);
            Destroy(gameObject);
        }

        //  PRIVATES
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 1.5f;
        }
    }
}