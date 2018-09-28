using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// For saving to file
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Vuforia;

[Serializable]
public class TargetStorer {

    public List<GameObject> CustomMarkers = new List<GameObject>(); 

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        // Accesseses a standard location on each device with persistentDataPath
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);
        // TargetStorer data = new TargetStorer();

        bf.Serialize(file, this);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            TargetStorer data = (TargetStorer)bf.Deserialize(file);
            file.Close();
        }

    }
}
