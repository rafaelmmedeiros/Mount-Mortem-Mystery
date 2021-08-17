using RPG.Stats.Enums;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int characterLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return characterLevel;

            float currentExperience = experience.GetPoints();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float experienceToLevelUP = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (experienceToLevelUP > currentExperience)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}