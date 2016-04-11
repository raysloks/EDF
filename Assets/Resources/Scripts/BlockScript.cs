using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {

    Vector3 previous;
    TerrainScript ts;
    
	void Start()
    {
        ts = FindObjectOfType<TerrainScript>();
        previous = transform.position;
	}
	
	void Update()
    {
        ts.SetCell(previous, null);
        ts.SetCell(transform.position, gameObject);
        previous = transform.position;
	}
}
