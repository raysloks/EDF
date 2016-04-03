using UnityEngine;
using System.Collections;

public class HighlightScript : MonoBehaviour {

    SpriteRenderer rend;

    public TurnManagerScript tm;
    public PathManagerScript pm;

    bool pe;

	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 prev_pos = transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var rh = Physics.RaycastAll(ray);
        rend.enabled = false;
        bool no_input = false;
        if (tm.current_turnholder != null)
            no_input = !tm.current_turnholder.my_turn;
        no_input |= UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (!no_input)
        {
            rend.material.color = new Color(1.0f, 1.0f, 1.0f);
            int rh_final = -1;
            for (int i = 0; i < rh.Length; ++i)
            {
                ClickScript other = rh[i].transform.GetComponentInParent<ClickScript>();
                if (other != null && other != tm.current_turnholder)
                {
                    rh_final = i;
                }
                else
                {
                    if (rh_final < 0)
                        rh_final = i;
                }
            }
            if (rh_final >= 0)
            {
                ClickScript other = rh[rh_final].transform.GetComponentInParent<ClickScript>();
                if (other != null && other != tm.current_turnholder)
                {
                    rend.material.color = new Color(1.0f, 0.0f, 0.0f);
					if (other.team == tm.current_turnholder.team)
						rend.material.color = new Color(0.0f, 1.0f, 0.0f);
                    transform.position = other.transform.position;
                    transform.localScale = rh[rh_final].transform.localScale;
                }
                else
                {
                    Vector2 target = rh[rh_final].point;
                    target.x = Mathf.Round(target.x - 0.5f) + 0.5f;
                    target.y = Mathf.Round(target.y - 0.5f) + 0.5f;

                    transform.position = target;
                }
                rend.enabled = true;
            }
        }
        
        if (pm != null && tm != null)
        {
            if (!rend.enabled)
                pm.Clear();
            else
                if (prev_pos != transform.position || rend.enabled && !pe)
                    if (tm.current_turnholder != null)
                    {
                        Vector2 s = new Vector2(tm.current_turnholder.transform.position.x, tm.current_turnholder.transform.position.y);
                        Vector2 e = new Vector2(transform.position.x, transform.position.y);
                        pm.Construct(tm.terrain.GetPath(s, e), transform.position);
                    }
        }

        pe = rend.enabled;
    }
}
