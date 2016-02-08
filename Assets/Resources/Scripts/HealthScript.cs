using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    public int current, max;

	// Use this for initialization
	void Start () {
        max = 4;
        current = 4;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Damage () {
        current -= 1;
        if (current <= 0)
        {
            var anim = GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("death");
            }
        }
    }
}
