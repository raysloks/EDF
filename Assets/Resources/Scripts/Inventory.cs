using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Inventory
{
    public List<Item> item;
    public int gold;

    public Inventory()
    {
        item = new List<Item>();
    }

    public void Save(Stream stream)
    {
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(stream, item.Count);
    }

    public void Load(Stream stream)
    {
        BinaryFormatter bf = new BinaryFormatter();

        int count = (int)bf.Deserialize(stream);
    }
}