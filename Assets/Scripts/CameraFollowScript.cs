using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {

    public ClickScript target;
    Vector3 view_offset;

	// Use this for initialization
	void Start () {
        view_offset = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if (target != null)
        {
            transform.Translate(-target.target / 4.0f, Space.World);
            transform.Translate(-view_offset, Space.World);

            float len = transform.position.magnitude;
            if (len>0.0f)
            {
                float nlen = Mathf.Max(0.0f, len /*Mathf.Exp(Mathf.Log(0.5f) * Time.deltaTime)*/ - 1.0f * Time.deltaTime);
                transform.Translate(transform.position * nlen / len - transform.position, Space.World);
            }

            transform.Translate(view_offset, Space.World);
            transform.Translate(target.target / 4.0f, Space.World);
        }
	}
}
