using UnityEngine;
using System.Collections;

public class ShadowScript : MonoBehaviour {

    public Transform follow;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (follow != null)
        {
            Vector2 off = follow.position;
            off *= 32.0f;
            off.x = Mathf.Round(off.x);
            off.y = Mathf.Round(off.y);
            off /= 32.0f;
            transform.position = off;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
