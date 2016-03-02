﻿using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ClickScript : MonoBehaviour {

    Animator anim;
    public HealthScript hp;

    public Vector2 target;
    float speed = 4.0f;

    List<Vector2> path;

    float transition;

    public bool my_turn;
    public float advance;

    public int facing;

    public List<Status> status;

    public delegate void OnRollDelegate(ClickScript cs, RollData data);
    public delegate void OnHitDelegate(ClickScript cs, HitData data);
    public delegate void OnHealthChangedDelegate(ClickScript cs, int difference);
    public delegate void OnTurnEndDelegate(ClickScript cs, TurnData data);
    public delegate void OnRecalculateStats(ClickScript cs, CharacterData data);

    public class DelegateDictionary<TValue> : Dictionary<float, TValue>
    {
        public new TValue this[float key]
        {
            get
            {
                TValue value;
                TryGetValue(key, out value);
                return value;
            }
            set
            {
                base[key] = value;
            }
        }
    }

    public DelegateDictionary<OnRollDelegate> onRoll;
    public DelegateDictionary<OnHitDelegate> onHit;
    public DelegateDictionary<OnHealthChangedDelegate> onHealthChanged;
    public DelegateDictionary<OnTurnEndDelegate> onTurnEnd;
    public DelegateDictionary<OnRecalculateStats> onRecalculateStats;

    // Use this for initialization
    void Start()
    {
        onRoll = new DelegateDictionary<OnRollDelegate>();
        onHit = new DelegateDictionary<OnHitDelegate>();
        onHealthChanged = new DelegateDictionary<OnHealthChangedDelegate>();
        onTurnEnd = new DelegateDictionary<OnTurnEndDelegate>();
        onRecalculateStats = new DelegateDictionary<OnRecalculateStats>();

        status = new List<Status>();

        GameObject obj = Instantiate(Resources.Load("Prefabs/Shadow")) as GameObject;
        ShadowScript ss = obj.GetComponent<ShadowScript>();
        ss.follow = transform;

        target = transform.position;
        anim = GetComponentInChildren<Animator>();
    }

    public void Save(Stream stream)
    {
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(stream, hp.current);

        bf.Serialize(stream, status.Count);

        var nume = status.GetEnumerator();
        while (nume.MoveNext())
        {
            bf.Serialize(stream, nume.Current);
        }
    }

    public void Load(Stream stream)
    {
        BinaryFormatter bf = new BinaryFormatter();

        hp.current = (int)bf.Deserialize(stream);

        int count = (int)bf.Deserialize(stream);
        for (int i=0;i<count;++i)
        {
            Status nstatus = (Status)bf.Deserialize(stream);
            nstatus.Attach(this);
            status.Add(nstatus);
        }
    }

    public bool Hold()
    {
        bool hold = false;
        if (hp != null)
            hold = hp.current <= 0;
        return hold;
    }

    public int Relation(ClickScript cs)
    {
        if (cs != null)
        {
            if (cs == this)
            {
                return -1;
            }
            return 1;
        }
        return 0;
    }

    public void OnHit(HitData data)
    {
        {
            List<float> empty = new List<float>();
            var nume = onHit.GetEnumerator();
            while (nume.MoveNext())
                if (nume.Current.Value != null)
                    nume.Current.Value(this, data);
            var nume2 = empty.GetEnumerator();
            while (nume.MoveNext())
                onHit.Remove(nume2.Current);
        }
        
        anim.SetTrigger("hit");

        if (!Hold())
        {
            if (data.source.transform.position.x > transform.position.x)
                facing = 1;
            if (data.source.transform.position.x < transform.position.x)
                facing = -1;
        }

        {
            int total_damage = 0;
            var nume = data.damage.GetEnumerator();
            while (nume.MoveNext())
            {
                total_damage += nume.Current.Value;
            }
            hp.Damage(total_damage);
            OnHealthChanged(-total_damage);
        }

        GameObject obj = Instantiate(Resources.Load("Prefabs/Blood Spurt")) as GameObject;
        SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
        rend.color = new Color(0.278f, 0.071f, 0.071f);
        obj.transform.position = transform.position;
        obj.transform.localScale = new Vector3(-1.0f * facing, 1.0f, 1.0f);
    }

    public void OnHealthChanged(int difference)
    {
        {
            List<float> empty = new List<float>();
            var nume = onHealthChanged.GetEnumerator();
            while (nume.MoveNext())
                if (nume.Current.Value != null)
                    nume.Current.Value(this, difference);
            var nume2 = empty.GetEnumerator();
            while (nume.MoveNext())
                onHealthChanged.Remove(nume2.Current);
        }

        if (hp.current <= 0)
        {
            var anim = GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("death");
            }
        }
    }

    public void OnTurnEnd(TurnData data)
    {
        {
            List<float> empty = new List<float>();
            var nume = onTurnEnd.GetEnumerator();
            while (nume.MoveNext())
                if (nume.Current.Value != null)
                    nume.Current.Value(this, data);
            var nume2 = empty.GetEnumerator();
            while (nume.MoveNext())
                onTurnEnd.Remove(nume2.Current);
        }
    }

    void Update()
    {
        bool stunned = false;
        if (hp != null)
            stunned = hp.current <= 0;
        if (!stunned)
        {
            if (my_turn)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    var rh = Physics.RaycastAll(ray);
                    int rh_final = -1;
                    for (int i = 0; i < rh.Length; ++i)
                    {
                        ClickScript other = rh[i].transform.GetComponentInParent<ClickScript>();
                        if (other != null && other != this)
                        {
                            rh_final = i;
                        }
                        else
                        {
                            if (rh_final < 0)
                                rh_final = i;
                        }
                    }
                    if (rh_final >= 0)
                    {
                        ClickScript other = rh[rh_final].transform.GetComponentInParent<ClickScript>();
                        if (other != null && other != this)
                        {
                            if (other.transform.position.x > transform.position.x)
                                facing = 1;
                            if (other.transform.position.x < transform.position.x)
                                facing = -1;

                            HitData hd = new HitData(this, other);
                            hd.damage.Add(new KeyValuePair<List<string>, int>(new List<string>(), 1));

                            other.OnHit(hd);

                            anim.SetTrigger("attack");
                            transition = 0.5f;

                            my_turn = false;
                        }
                        else
                        {
                            Vector2 ntarget = rh[rh_final].point;

                            ntarget.x = Mathf.Round(ntarget.x - 0.5f) + 0.5f;
                            ntarget.y = Mathf.Round(ntarget.y - 0.5f) + 0.5f;

                            TerrainScript ts = rh[rh_final].transform.GetComponent<TerrainScript>();
                            if (ts != null)
                                path = ts.GetPath(transform.position, ntarget);

                            my_turn = false;
                        }
                    }
                }
            }

            if (path != null)
            {
                if (path.Count > 0)
                {
                    if (path[path.Count - 1] - new Vector2(transform.position.x, transform.position.y) == new Vector2())
                        path.RemoveAt(path.Count - 1);
                    if (path.Count > 0)
                        target = path[path.Count - 1];
                }
            }

            Vector2 delta = target - new Vector2(transform.position.x, transform.position.y);

            anim.SetBool("walking", delta != new Vector2());

            transition -= Time.deltaTime;
            if (transition < 0.0f)
                transition = 0.0f;
            if (delta == new Vector2() && !my_turn && transition <= 0.0f)
                advance = 1.0f;

            if (delta.x > 0.0f)
                facing = 1;
            if (delta.x < 0.0f)
                facing = -1;

            if (facing > 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            if (facing < 0)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            float max_len = speed * Time.deltaTime;
            if (delta.magnitude > max_len)
                delta = delta.normalized * max_len;
            transform.Translate(delta, Space.World);
        }
        else
        {
            advance = 1.0f;
        }
    }
}
