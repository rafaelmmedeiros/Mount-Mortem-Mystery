using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathfromSaveFile(saveFile);
            print("Saving to: " + path);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CaptureState());
            }
        }

        public void Load(string saveFile)
        {
            string path = GetPathfromSaveFile(saveFile);
            print("Loading from: " + path);

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(stream));
            }
        }

        // PRIVATES
        private object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
            {
                state[saveableEntity.GetUniqueIdentifier()] = saveableEntity.CaptureState();
            }
            return state;
        }

        private void RestoreState(object state)
        {
            Dictionary<string, object> stateToRestore = (Dictionary<string, object>)state;
            foreach (SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
            {
                saveableEntity.RestoreState(stateToRestore[saveableEntity.GetUniqueIdentifier()]);
            }
        }

        private string GetPathfromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}