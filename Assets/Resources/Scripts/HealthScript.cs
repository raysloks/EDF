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

    public void Damage(int damage) {
        current -= damage;
        if (current > max)
            current = max;
    }
}
