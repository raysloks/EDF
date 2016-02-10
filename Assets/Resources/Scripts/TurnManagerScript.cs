using UnityEngine;
using System.Collections.Generic;

public class TurnManagerScript : MonoBehaviour {

    public CameraFollowScript cam;

    Dictionary<float, List<ClickScript>> order;
    Dictionary<float, List<ClickScript>> new_order;

    public ClickScript current_turnholder;

	// Use this for initialization
	void Start () {
        order = new Dictionary<float, List<ClickScript>>();
        new_order = new Dictionary<float, List<ClickScript>>();

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
            if (current_turnholder.advance>0.0f)
            {
                current_turnholder.my_turn = false;
                if (!new_order.ContainsKey(current_turnholder.advance))
                    new_order.Add(current_turnholder.advance, new List<ClickScript>());
                new_order[current_turnholder.advance].Add(current_turnholder);
                order = new_order;
                new_order = new Dictionary<float, List<ClickScript>>();
                current_turnholder = null;
            }
        }
	}
}
