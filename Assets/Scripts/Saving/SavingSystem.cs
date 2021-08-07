using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathfromSaveFile(saveFile);
            print("Saving to: " + path);
            FileStream fileStream = File.Open(path, FileMode.Create);
            byte[] bytes = Encoding.UTF8.GetBytes("We are doomed");
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        public void Load(string saveFile)
        {
            print("Loading from: " + GetPathfromSaveFile(saveFile));
        }

        private string GetPathfromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}