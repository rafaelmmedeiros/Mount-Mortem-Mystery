using RPG.Core;
using RPG.Saving.Interfaces;
using RPG.Stats;
using RPG.Stats.Enums;
using RPG.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] bool IsImmortal = false;
        [SerializeField] float regenerationPercentage = 70;

        [SerializeField] UnityEvent<float> onTakeDamage; // Chance to Pascal Case It´s a Method
        [SerializeField] UnityEvent onDie; // Same Above

        // Unity does not reconize a Generic Type, need to be created a subclass from that event
        // But It Works on unity 2020+

        //[System.Serializable]
        //public class TakedamageEvent : UnityEvent<float> { }

        LazyValue<float> healthPoints;
        private bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;

        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            if (IsImmortal) return;

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            onTakeDamage.Invoke(damage);

            if (healthPoints.value == 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
        }

        public void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }

        public float GetPercentageHealthPoints()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        //  INTERFACES
        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value <= 0)
            {
                Die();
            }
        }
    }
}