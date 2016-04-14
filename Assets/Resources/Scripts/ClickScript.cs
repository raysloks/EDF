using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ClickScript : MonoBehaviour {

    public TurnManagerScript tm;

    Animator anim;
    public HealthScript hp;

    public Inventory inventory;

    public Vector2 target;
    float speed = 4.0f;

    List<Vector2> path;

    float transition;

    public bool my_turn;
    public float advance;

    public int facing;

	public int team;

	public bool player;

    public CharacterData stats;

    public List<Status> status;

    public delegate void OnRollDelegate(ClickScript cs, RollData data);
    public delegate void OnHitDelegate(ClickScript cs, HitData data);
    public delegate void OnHealthChangedDelegate(ClickScript cs, int difference);
    public delegate void OnTurnEndDelegate(ClickScript cs, TurnData data);
    public delegate void OnRecalculateStatsDelegate(ClickScript cs, CharacterData data);
    public delegate void OnGetTypeDelegate(ClickScript cs, TypeData data);
    public delegate void OnStatusGainedDelegate(ClickScript cs, Status status);
    public delegate void OnStatusLostDelegate(ClickScript cs, Status status);

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
    public DelegateDictionary<OnRecalculateStatsDelegate> onRecalculateStats;
    public DelegateDictionary<OnGetTypeDelegate> onGetType;
    public DelegateDictionary<OnStatusGainedDelegate> onStatusGained;
    public DelegateDictionary<OnStatusLostDelegate> onStatusLost;

    public void Init()
    {
        onRoll = new DelegateDictionary<OnRollDelegate>();
        onHit = new DelegateDictionary<OnHitDelegate>();
        onHealthChanged = new DelegateDictionary<OnHealthChangedDelegate>();
        onTurnEnd = new DelegateDictionary<OnTurnEndDelegate>();
        onRecalculateStats = new DelegateDictionary<OnRecalculateStatsDelegate>();
        onGetType = new DelegateDictionary<OnGetTypeDelegate>();
        onStatusGained = new DelegateDictionary<OnStatusGainedDelegate>();
        onStatusLost = new DelegateDictionary<OnStatusLostDelegate>();

        status = new List<Status>();

        inventory = new Inventory();

        GameObject obj = Instantiate(Resources.Load("Prefabs/Shadow")) as GameObject;
        ShadowScript ss = obj.GetComponent<ShadowScript>();
        ss.follow = transform;

        target = transform.position;
        anim = GetComponentInChildren<Animator>();

        path = new List<Vector2>();

        hp = new HealthScript();
        hp.max = 4;
        hp.current = 4;
    }

    public void Save(Stream stream)
    {
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(stream, hp.current);

        inventory.Save(stream);

        bf.Serialize(stream, facing);

		bf.Serialize(stream, team);
        
		bf.Serialize(stream, player);

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

        if (hp.current <= 0)
            anim.SetBool("dead", true);
        
        inventory.Load(stream);

		facing = (int)bf.Deserialize(stream);

		team = (int)bf.Deserialize(stream);

		player = (bool)bf.Deserialize(stream);
        
        int count = (int)bf.Deserialize(stream);
        for (int i=0;i<count;++i)
        {
            Status nstatus = (Status)bf.Deserialize(stream);
            nstatus.Attach(this);
            status.Add(nstatus);
        }

        RecalculateStats();
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

    public void OnRoll(RollData data)
    {
        List<float> empty = new List<float>();
        var nume = onRoll.GetEnumerator();
        while (nume.MoveNext())
            if (nume.Current.Value != null)
                nume.Current.Value(this, data);
            else
                empty.Add(nume.Current.Key);
        var nume2 = empty.GetEnumerator();
        while (nume.MoveNext())
            onRoll.Remove(nume2.Current);
    }

    public void OnHit(HitData data)
    {
        {
            List<float> empty = new List<float>();
            var nume = onHit.GetEnumerator();
            while (nume.MoveNext())
                if (nume.Current.Value != null)
                    nume.Current.Value(this, data);
                else
                    empty.Add(nume.Current.Key);
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
                else
                    empty.Add(nume.Current.Key);
            var nume2 = empty.GetEnumerator();
            while (nume.MoveNext())
                onHealthChanged.Remove(nume2.Current);
        }

        if (hp.current <= 0)
        {
            var anim = GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.SetBool("dead", true);
            }
        }

        if (hp.current <= -10)
        {
            tm.Erase(this);
        }

        RecalculateStats();
    }

    public void OnTurnEnd(TurnData data)
    {
        List<float> empty = new List<float>();
        var nume = onTurnEnd.GetEnumerator();
        while (nume.MoveNext())
            if (nume.Current.Value != null)
                nume.Current.Value(this, data);
            else
                empty.Add(nume.Current.Key);
        var nume2 = empty.GetEnumerator();
        while (nume.MoveNext())
            onTurnEnd.Remove(nume2.Current);
    }

    public void OnStatusGained(Status status)
    {
        List<float> empty = new List<float>();
        var nume = onStatusGained.GetEnumerator();
        while (nume.MoveNext())
            if (nume.Current.Value != null)
                nume.Current.Value(this, status);
            else
                empty.Add(nume.Current.Key);
        var nume2 = empty.GetEnumerator();
        while (nume.MoveNext())
            onStatusGained.Remove(nume2.Current);

        RecalculateStats();
    }

    public void OnStatusLost(Status status)
    {
        List<float> empty = new List<float>();
        var nume = onStatusLost.GetEnumerator();
        while (nume.MoveNext())
            if (nume.Current.Value != null)
                nume.Current.Value(this, status);
            else
                empty.Add(nume.Current.Key);
        var nume2 = empty.GetEnumerator();
        while (nume.MoveNext())
            onStatusLost.Remove(nume2.Current);

        RecalculateStats();
    }

    public void RecalculateStats()
    {
        stats = new CharacterData();
        List<float> empty = new List<float>();
        var nume = onRecalculateStats.GetEnumerator();
        while (nume.MoveNext())
            if (nume.Current.Value != null)
                nume.Current.Value(this, stats);
            else
                empty.Add(nume.Current.Key);
        var nume2 = empty.GetEnumerator();
        while (nume.MoveNext())
            onRecalculateStats.Remove(nume2.Current);
    }

    public List<string> GetTypes()
    {
        TypeData data = new TypeData();
        List<float> empty = new List<float>();
        var nume = onGetType.GetEnumerator();
        while (nume.MoveNext())
            if (nume.Current.Value != null)
                nume.Current.Value(this, data);
            else
                empty.Add(nume.Current.Key);
        var nume2 = empty.GetEnumerator();
        while (nume.MoveNext())
            onGetType.Remove(nume2.Current);
        return data.type;
    }

    void Attack(ClickScript other)
    {
        if (other.transform.position.x > transform.position.x)
            facing = 1;
        if (other.transform.position.x < transform.position.x)
            facing = -1;

        HitData hd = new HitData(this, other);
        hd.damage.Add(new KeyValuePair<List<string>, int>(new List<string>(), 1));

        RollData attacker = new RollData();

        attacker.source = this;
        attacker.target = other;
        attacker.type.Add("attack");
        attacker.type.Add("melee");

        RollData defender = new RollData(attacker);

        defender.bonus.Add(new KeyValuePair<List<string>, int>(new List<string>(), defender.target.stats.armor));

        attacker.roll.Add(RandomManager.d6());
        attacker.roll.Add(RandomManager.d6());

        OnRoll(attacker);
        other.OnRoll(defender);

        int result = attacker.GetBoth();

        Debug.Log(result);

        if (result >= 0)
        {
            other.OnHit(hd);
        }
        else
        {
            Instantiate(Resources.Load("Prefabs/MissMessage"), other.transform.position, Quaternion.AngleAxis(-45.0f, new Vector3(1.0f, 0.0f, 0.0f)));
        }

        anim.SetTrigger("attack");
        transition = 0.5f;
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
				if (player)
				{
	                if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
	                {
	                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	                    var rh = Physics.RaycastAll(ray);
	                    int rh_final = -1;
	                    for (int i = 0; i < rh.Length; ++i)
	                    {
	                        ClickScript other = rh[i].transform.GetComponentInParent<ClickScript>();
	                        if (other != null && other != this)
	                            rh_final = i;
	                        else
	                            if (rh_final < 0)
	                                rh_final = i;
	                    }
	                    if (rh_final >= 0)
	                    {
	                        ClickScript other = rh[rh_final].transform.GetComponentInParent<ClickScript>();
	                        if (other != null && other != this && other.team != team && (transform.position - other.transform.position).magnitude < 1.5f)
	                        {
                                Attack(other);
                            }
	                        else
	                        {
	                            Vector2 ntarget = rh[rh_final].point;

	                            ntarget.x = Mathf.Round(ntarget.x - 0.5f) + 0.5f;
	                            ntarget.y = Mathf.Round(ntarget.y - 0.5f) + 0.5f;

                                TargetData td = new TargetData();
                                td.start = transform.position;
                                td.end = ntarget;
                                td.searcher = this;
                                td.use_end = true;

                                tm.terrain.GetPath(td);
                                path = td.paths[0];
                                if (path.Count > 0)
                                    path.RemoveAt(path.Count - 1);
                            }

                            my_turn = false;
                        }
	                }
				}
				else
                {
                    TargetData td = new TargetData();
                    td.start = transform.position;
                    td.end = td.start;
                    td.searcher = this;
                    td.use_end = false;
                    td.random = true;

                    List<ClickScript> attacks = new List<ClickScript>();
                    List<List<Vector2>> moves = new List<List<Vector2>>();

                    tm.terrain.GetPath(td);
                    foreach (var npath in td.paths)
                    {
                        if (npath.Count > 0)
                        {
                            npath.RemoveAt(npath.Count - 1);
                            if (npath.Count > 0)
                            {
                                var go = tm.terrain.GetCell(npath[0]);
                                ClickScript other = go.GetComponent<ClickScript>();
                                if (other != null && other != this && other.team != team && (transform.position - other.transform.position).magnitude < 1.5f)
                                    attacks.Add(other);
                                else
                                    moves.Add(npath);
                            }
                        }
                    }
                    
                    if (attacks.Count > 0)
                    {
                        Attack(attacks[RandomManager.ai.Next(attacks.Count)]);
                    }
                    else
                    {
                        if (moves.Count > 0)
                        {
                            path = moves[RandomManager.ai.Next(moves.Count)];
                        }
                    }

                    my_turn = false;
                }
            }

            if (path != null)
            {
                if (path.Count > 0)
                {
                    if (target - new Vector2(transform.position.x, transform.position.y) == new Vector2())
                    {
                        target = path[path.Count - 1];
                        path.RemoveAt(path.Count - 1);
                        Debug.Log("Path node consumed " + target.x + " " + target.y);
                        GameObject go_at_target = tm.terrain.GetCell(target);
                        if (go_at_target != null && go_at_target != gameObject)
                        {
                            target = transform.position;
                            path.Clear();
                        }
                        else
                        {
                            tm.terrain.SetCell(transform.position, null);
                            tm.terrain.SetCell(target, gameObject);
                        }
                    }
                }
            }

            Vector2 delta = target - new Vector2(transform.position.x, transform.position.y);

            anim.SetBool("walking", delta != new Vector2());

            transition -= Time.deltaTime;
            if (transition < 0.0f)
                transition = 0.0f;
            if (delta == new Vector2() && !my_turn && transition <= 0.0f && path.Count == 0)
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
