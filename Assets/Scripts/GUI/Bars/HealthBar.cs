
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.GUI.Bars
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;

        void Update()
        {
            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
        }
    }
}