using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManagerScript : MonoBehaviour {

    public CameraFollowScript cam;

    Dictionary<float, ClickScript> order;
    Dictionary<float, ClickScript> new_order;

    public ClickScript current_turnholder;

	// Use this for initialization
	void Start () {
        order = new Dictionary<float, ClickScript>();
        new_order = new Dictionary<float, ClickScript>();

        var go = Instantiate(Resources.Load("Prefabs/Character"), new Vector3(0.5f, 0.5f, 0.0f), Quaternion.AngleAxis(-45.0f, new Vector3(1.0f, 0.0f, 0.0f))) as GameObject;
        var cs = go.GetComponent<ClickScript>();
        order.Add(0.0f, cs);

        go = Instantiate(Resources.Load("Prefabs/Character"), new Vector3(-0.5f, 0.5f, 0.0f), Quaternion.AngleAxis(-45.0f, new Vector3(1.0f, 0.0f, 0.0f))) as GameObject;
        cs = go.GetComponent<ClickScript>();
        order.Add(0.5f, cs);
    }
	
	// Update is called once per frame
	void Update () {
        if (current_turnholder == null)
        {
            var nume = order.GetEnumerator();
            while (nume.MoveNext())
            {
                if (nume.Current.Value.Hold())
                {

                }
                else
                {
                    var next = nume.Current;
                    current_turnholder = next.Value;
                    float time_to_advance = next.Key;
                    while (nume.MoveNext())
                        new_order.Add(nume.Current.Key - time_to_advance, nume.Current.Value);
                    current_turnholder.my_turn = true;
                    current_turnholder.advance = 0.0f;
                    break;
                }
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
