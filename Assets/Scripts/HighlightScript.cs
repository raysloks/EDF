using UnityEngine;
using System.Collections;

public class HighlightScript : MonoBehaviour {

    Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rh = new RaycastHit();
        if (Physics.Raycast(ray, out rh))
        {
            Vector2 target = rh.point;
            target.x = Mathf.Round(target.x - 0.5f) + 0.5f;
            target.y = Mathf.Round(target.y - 0.5f) + 0.5f;

            transform.position = target;

            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }
    }
}
