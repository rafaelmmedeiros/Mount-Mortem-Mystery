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
            SaveFile(saveFile, CaptureState());
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        // PRIVATES
        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathfromSaveFile(saveFile);
            print("Saving to: " + path);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathfromSaveFile(saveFile);
            print("Loading from: " + path);

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private Dictionary<string, object> CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
            {
                state[saveableEntity.GetUniqueIdentifier()] = saveableEntity.CaptureState();
            }
            return state;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
            {
                saveableEntity.RestoreState(state[saveableEntity.GetUniqueIdentifier()]);
            }
        }

        private string GetPathfromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}