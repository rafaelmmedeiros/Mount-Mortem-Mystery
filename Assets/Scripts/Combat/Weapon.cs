using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent OnHit;

        public void Onhit()
        {
            OnHit.Invoke();
        }
    }
}