using RPG.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "n/a";
            }
            else
            {
                Health health = fighter.GetTarget();
                GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercentageHealthPoints());
            }
        }
    }
}