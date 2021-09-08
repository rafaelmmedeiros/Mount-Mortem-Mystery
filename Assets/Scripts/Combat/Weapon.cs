using UnityEngine;


namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        public void Onhit()
        {
            print("gameObject.name: " + gameObject.name);
        }

    }
}