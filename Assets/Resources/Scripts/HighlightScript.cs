using UnityEngine;
using System.Collections;

public class HighlightScript : MonoBehaviour {

    SpriteRenderer rend;

    public TurnManagerScript tm;

	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var rh = Physics.RaycastAll(ray);
        rend.enabled = false;
        bool no_input = false;
        if (tm.current_turnholder != null)
            no_input = !tm.current_turnholder.my_turn;
        if (!no_input)
        {
            rend.material.color = new Color(1.0f, 1.0f, 1.0f);
            for (int i = 0; i < rh.Length; ++i)
            {
                Vector2 target = rh[i].point;
                target.x = Mathf.Round(target.x - 0.5f) + 0.5f;
                target.y = Mathf.Round(target.y - 0.5f) + 0.5f;

                transform.position = target;

                ClickScript other = rh[i].transform.GetComponentInParent<ClickScript>();
                if (other != null && other != tm.current_turnholder)
                    rend.material.color = new Color(1.0f, 0.0f, 0.0f);

                rend.enabled = true;
            }
        }
    }
}
