using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager
{
    public void Save(TurnManagerScript tm)
    {
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Create);

        tm.Save(file);

        file.Close();
    }

    public void Load(TurnManagerScript tm)
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);

            tm.Load(file);

            file.Close();
        }
    }
}