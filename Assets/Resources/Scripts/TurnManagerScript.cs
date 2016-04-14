using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TurnManagerScript : MonoBehaviour {

    public CameraFollowScript cam;

    Dictionary<float, List<ClickScript>> order;
    Dictionary<float, List<ClickScript>> new_order;

    List<ClickScript> on_hold;

    public ClickScript current_turnholder;

    public TerrainScript terrain;

    public SaveManager save_manager;

	// Use this for initialization
	void Start () {
        terrain.Init();

        order = new Dictionary<float, List<ClickScript>>();
        new_order = new Dictionary<float, List<ClickScript>>();

        on_hold = new List<ClickScript>();

        save_manager = new SaveManager();

        save_manager.Load(this);
    }

    public void TempNewGame()
    {
        Clear();

        {
            ClickScript cs = NewCharacter(new Vector3(0.5f, -4.5f, 0.0f));
            var bs = new BaseStatsStatus();
            bs.Attach(cs);
            cs.status.Add(bs);
            cs.player = true;
            cs.RecalculateStats();
        }
        {
            ClickScript cs = NewCharacter(new Vector3(-0.5f, -4.5f, 0.0f));
            var bs = new BaseStatsStatus();
            bs.Attach(cs);
            cs.status.Add(bs);
            cs.player = true;
            cs.RecalculateStats();
        }

        {
            ClickScript cs = NewCharacter(new Vector3(0.5f, 4.5f, 0.0f));
            var bs = new BaseStatsStatus();
            bs.Attach(cs);
            cs.status.Add(bs);
            cs.player = false;
            cs.team = 1;
            cs.RecalculateStats();
        }
        {
            ClickScript cs = NewCharacter(new Vector3(-0.5f, 4.5f, 0.0f));
            var bs = new BaseStatsStatus();
            bs.Attach(cs);
            cs.status.Add(bs);
            cs.player = false;
            cs.team = 1;
            cs.RecalculateStats();
        }
    }

    public void Erase(ClickScript cs)
    {
        if (current_turnholder == cs)
            current_turnholder = null;

        var nume = order.GetEnumerator();
        while (nume.MoveNext())
            nume.Current.Value.Remove(cs);

        var nume2 = new_order.GetEnumerator();
        while (nume2.MoveNext())
            nume2.Current.Value.Remove(cs);

        on_hold.Remove(cs);

        Destroy(cs.gameObject);
    }

    public void Clear()
    {
        current_turnholder = null;

        var nume = order.GetEnumerator();
        while (nume.MoveNext())
        {
            var nume2 = nume.Current.Value.GetEnumerator();
            while (nume2.MoveNext())
            {
                Destroy(nume2.Current.gameObject);
            }
        }
        order.Clear();
        new_order.Clear();

        var on_hold_nume = on_hold.GetEnumerator();
        while (on_hold_nume.MoveNext())
        {
            Destroy(on_hold_nume.Current.gameObject);
        }
        on_hold.Clear();
    }

    public ClickScript NewCharacter(Vector3 position, bool add_to_order = true)
    {
        var go = Instantiate(Resources.Load("Prefabs/Character"), position, Quaternion.AngleAxis(-45.0f, new Vector3(1.0f, 0.0f, 0.0f))) as GameObject;
        var cs = go.GetComponent<ClickScript>();
        cs.Init();
        cs.tm = this;
        terrain.SetCell(cs.transform.position, go);
        if (add_to_order)
        {
            if (current_turnholder != null)
            {
                if (!new_order.ContainsKey(0.5f))
                    new_order.Add(0.5f, new List<ClickScript>());
                new_order[0.5f].Add(cs);
            }
            else
            {
                if (!order.ContainsKey(0.5f))
                    order.Add(0.5f, new List<ClickScript>());
                order[0.5f].Add(cs);
            }
        }
        return cs;
    }

    void SaveCS(Stream stream, ClickScript cs)
    {
        BinaryFormatter bf = new BinaryFormatter();

        Vector3 position = cs.transform.position;
        bf.Serialize(stream, position.x);
        bf.Serialize(stream, position.y);
        bf.Serialize(stream, position.z);
        cs.Save(stream);
    }

    ClickScript LoadCS(Stream stream)
    {
        BinaryFormatter bf = new BinaryFormatter();
        
        Vector3 position = new Vector3((float)bf.Deserialize(stream), (float)bf.Deserialize(stream), (float)bf.Deserialize(stream));
        var cs = NewCharacter(position, false);
        cs.Load(stream);

        return cs;
    }

    public void Save(Stream stream)
    {
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(stream, RandomManager.random);
        bf.Serialize(stream, RandomManager.ai);

        bf.Serialize(stream, order.Count);
        var nume = order.GetEnumerator();
        while (nume.MoveNext())
        {
            bf.Serialize(stream, nume.Current.Key);
            bf.Serialize(stream, nume.Current.Value.Count);
            var nume2 = nume.Current.Value.GetEnumerator();
            while (nume2.MoveNext())
                SaveCS(stream, nume2.Current);
        }

        {
            bf.Serialize(stream, on_hold.Count);
            var nume2 = on_hold.GetEnumerator();
            while (nume2.MoveNext())
                SaveCS(stream, nume2.Current);
        }
    }

    public void Load(Stream stream)
    {
        BinaryFormatter bf = new BinaryFormatter();

        RandomManager.random = (System.Random)bf.Deserialize(stream);
        RandomManager.ai = (System.Random)bf.Deserialize(stream);

        order.Clear();

        int count = (int)bf.Deserialize(stream);
        for (int i=0;i<count;++i)
        {
            float key = (float)bf.Deserialize(stream);
            List<ClickScript> list = new List<ClickScript>();
            int count2 = (int)bf.Deserialize(stream);
            for (int j=0;j<count2;++j)
                list.Add(LoadCS(stream));
            order.Add(key, list);
        }
        
        int on_hold_count = (int)bf.Deserialize(stream);
        for (int i=0;i<on_hold_count;++i)
            on_hold.Add(LoadCS(stream));
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
                        on_hold.Add(nume2.Current);
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
