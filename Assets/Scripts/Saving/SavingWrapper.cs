using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                GetComponent<SavingSystem>().Save(defaultSaveFile);

            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                GetComponent<SavingSystem>().Load(defaultSaveFile);

            }
        }

    }
}