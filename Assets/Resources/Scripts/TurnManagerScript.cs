using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManagerScript : MonoBehaviour {

    public CameraFollowScript cam;

    Dictionary<float, ClickScript> order;
    Dictionary<float, ClickScript> new_order;

    ClickScript current_turnholder;

	// Use this for initialization
	void Start () {
        order = new Dictionary<float, ClickScript>();
        new_order = new Dictionary<float, ClickScript>();

        order.Add(0.0f, (Instantiate(Resources.Load("Prefabs/Character")) as GameObject).GetComponent<ClickScript>());
        order.Add(0.5f, (Instantiate(Resources.Load("Prefabs/Character")) as GameObject).GetComponent<ClickScript>());
    }
	
	// Update is called once per frame
	void Update () {
        if (current_turnholder == null)
        {
            var nume = order.GetEnumerator();
            if (nume.MoveNext())
            {
                var next = nume.Current;
                current_turnholder = next.Value;
                float time_to_advance = next.Key;
                while (nume.MoveNext())
                    new_order.Add(nume.Current.Key - time_to_advance, nume.Current.Value);
                current_turnholder.my_turn = true;
                current_turnholder.advance = 0.0f;
            }
        }
        if (current_turnholder != null)
        {
            cam.target = current_turnholder;
            if (current_turnholder.advance>0.0f)
            {
                current_turnholder.my_turn = false;
                new_order.Add(current_turnholder.advance, current_turnholder);
                order = new_order;
                new_order = new Dictionary<float, ClickScript>();
                current_turnholder = null;
            }
        }
	}
}
