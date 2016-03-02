﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TurnManagerScript : MonoBehaviour {

    public CameraFollowScript cam;

    Dictionary<float, List<ClickScript>> order;
    Dictionary<float, List<ClickScript>> new_order;

    public ClickScript current_turnholder;

    public SaveManager save_manager;

	// Use this for initialization
	void Start () {
        order = new Dictionary<float, List<ClickScript>>();
        new_order = new Dictionary<float, List<ClickScript>>();

        save_manager = new SaveManager();

        save_manager.Load(this);
    }

    public void TempNewGame()
    {
        var go = Instantiate(Resources.Load("Prefabs/Character"), new Vector3(0.5f, 0.5f, 0.0f), Quaternion.AngleAxis(-45.0f, new Vector3(1.0f, 0.0f, 0.0f))) as GameObject;
        var cs = go.GetComponent<ClickScript>();
        if (!order.ContainsKey(0.0f))
            order.Add(0.0f, new List<ClickScript>());
        order[0.0f].Add(cs);

        go = Instantiate(Resources.Load("Prefabs/Character"), new Vector3(-0.5f, 0.5f, 0.0f), Quaternion.AngleAxis(-45.0f, new Vector3(1.0f, 0.0f, 0.0f))) as GameObject;
        cs = go.GetComponent<ClickScript>();
        if (!order.ContainsKey(0.5f))
            order.Add(0.5f, new List<ClickScript>());
        order[0.5f].Add(cs);
    }

    public void Save(Stream stream)
    {
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(stream, order.Count);
        var nume = order.GetEnumerator();
        while (nume.MoveNext())
        {
            bf.Serialize(stream, nume.Current.Key);
            bf.Serialize(stream, nume.Current.Value.Count);
            var nume2 = nume.Current.Value.GetEnumerator();
            while (nume2.MoveNext())
            {
                Vector3 position = nume2.Current.transform.position;
                bf.Serialize(stream, position.x);
                bf.Serialize(stream, position.y);
                bf.Serialize(stream, position.z);
                nume2.Current.Save(stream);
            }
        }
    }

    public void Load(Stream stream)
    {
        BinaryFormatter bf = new BinaryFormatter();

        order.Clear();

        int count = (int)bf.Deserialize(stream);
        for (int i=0;i<count;++i)
        {
            float key = (float)bf.Deserialize(stream);
            List<ClickScript> list = new List<ClickScript>();
            int count2 = (int)bf.Deserialize(stream);
            for (int j=0;j<count2;++j)
            {
                Vector3 position = new Vector3((float)bf.Deserialize(stream), (float)bf.Deserialize(stream), (float)bf.Deserialize(stream));
                var go = Instantiate(Resources.Load("Prefabs/Character"), position, Quaternion.AngleAxis(-45.0f, new Vector3(1.0f, 0.0f, 0.0f))) as GameObject;
                var cs = go.GetComponent<ClickScript>();
                cs.Load(stream);
                list.Add(cs);
            }
            order.Add(key, list);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (current_turnholder == null)
        {
            var nume = order.GetEnumerator();
            while (nume.MoveNext())
            {
                var nume2 = nume.Current.Value.GetEnumerator();
                while (nume2.MoveNext())
                {
                    if (nume2.Current.Hold())
                    {

                    }
                    else
                    {
                        current_turnholder = nume2.Current;
                        float time_to_advance = nume.Current.Key;
                        while (nume2.MoveNext())
                        {
                            if (!new_order.ContainsKey(0.0f))
                                new_order.Add(0.0f, new List<ClickScript>());
                            new_order[0.0f].Add(nume2.Current);
                        }
                        while (nume.MoveNext())
                            new_order.Add(nume.Current.Key - time_to_advance, nume.Current.Value);
                        current_turnholder.my_turn = true;
                        current_turnholder.advance = 0.0f;
                    }
                }
            }
        }
        if (current_turnholder != null)
        {
            cam.target = current_turnholder;
            while (current_turnholder.advance>0.0f)
            {
                TurnData data = new TurnData();
                data.time = current_turnholder.advance;
                data.type = TurnType.battle;
                current_turnholder.OnTurnEnd(data);
                if (current_turnholder.advance>0.0f)
                {
                    current_turnholder.my_turn = false;
                    if (!new_order.ContainsKey(current_turnholder.advance))
                        new_order.Add(current_turnholder.advance, new List<ClickScript>());
                    new_order[current_turnholder.advance].Add(current_turnholder);
                    order = new_order;
                    new_order = new Dictionary<float, List<ClickScript>>();
                    current_turnholder = null;
                    break;
                }
            }
        }
	}
}
