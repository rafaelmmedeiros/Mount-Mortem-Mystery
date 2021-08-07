using System.Collections;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        //System.Guid.NewGuid().ToString();

        public string GetUniqueIdentifier()
        {
            return "Id";
        }

        public object CaptureState()
        {
            print("Capturing state for: " + GetUniqueIdentifier());
            return null;
        }

        public void RestoreState(object state)
        {
            print("Restoring state for: " + GetUniqueIdentifier());
        }

        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;

            print("Updating:");
        }
    }
}