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
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        // PRIVATES
        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathfromSaveFile(saveFile);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathfromSaveFile(saveFile);

            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
            {
                state[saveableEntity.GetUniqueIdentifier()] = saveableEntity.CaptureState();
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveableEntity.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    saveableEntity.RestoreState(state[id]);
                }
            }
        }

        private string GetPathfromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}