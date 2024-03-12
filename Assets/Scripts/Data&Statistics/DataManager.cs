using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour {

    public Data data;
    public string fileName = "data.dat";
    private string dataPath;
    public static DataManager instance;

    private void Awake() {

        if (!instance) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        dataPath = $"{Application.persistentDataPath}/{fileName}";
        Debug.Log(Application.persistentDataPath);
        Load();
    }

    public void Save() {
        BinaryFormatter binf = new BinaryFormatter();
        FileStream file = File.Create(dataPath);
        binf.Serialize(file, data);
        file.Close();
    }

    public void Load() {
        if (!File.Exists(dataPath)) {
            return;
        }

        BinaryFormatter binf = new BinaryFormatter();
        FileStream file = File.Open(dataPath, FileMode.Open);
        data = (Data)binf.Deserialize(file);
        file.Close();
    }
}