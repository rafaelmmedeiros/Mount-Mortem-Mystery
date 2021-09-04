using System.Collections;
using System.Collections.Generic;
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