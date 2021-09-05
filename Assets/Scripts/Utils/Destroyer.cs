using UnityEngine;

namespace RPG.Utils
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;
        public void DestroyTarget()
        {
            Destroy(targetToDestroy);
        }
    }
}