using UnityEngine;
using System.Collections;

public class ClickScript : MonoBehaviour {

    Animator anim;
    HealthScript hp;

    public Vector2 target;
    float speed = 4.0f;

    float transition;

    public bool my_turn;
    public float advance;

    public int facing;

	// Use this for initialization
	void Start()
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Shadow")) as GameObject;
        ShadowScript ss = obj.GetComponent<ShadowScript>();
        ss.follow = transform;

        target = transform.position;
        anim = GetComponentInChildren<Animator>();
        hp = GetComponent<HealthScript>();
    }

    public bool Hold()
    {
        bool hold = false;
        if (hp != null)
            hold = hp.current <= 0;
        return hold;
    }

    // Update is called once per frame
    void Update()
    {
        bool stunned = false;
        if (hp != null)
            stunned = hp.current <= 0;
        if (!stunned)
        {
            if (my_turn)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    var rh = Physics.RaycastAll(ray);
                    int rh_final = -1;
                    for (int i = 0; i < rh.Length; ++i)
                    {
                        ClickScript other = rh[i].transform.GetComponentInParent<ClickScript>();
                        if (other != null && other != this)
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
                        if (other != null && other != this)
                        {
                            if (other.transform.position.x > transform.position.x)
                                facing = 1;
                            if (other.transform.position.x < transform.position.x)
                                facing = -1;
                            other.facing = -facing;

                            HealthScript hs = other.GetComponent<HealthScript>();
                            hs.Damage();

                            other.anim.SetTrigger("hit");
                            anim.SetTrigger("attack");
                            transition = 0.5f;

                            my_turn = false;
                        }
                        else
                        {
                            target = rh[rh_final].point;

                            target.x = Mathf.Round(target.x - 0.5f) + 0.5f;
                            target.y = Mathf.Round(target.y - 0.5f) + 0.5f;

                            Vector2 dif = target - new Vector2(transform.position.x, transform.position.y);
                            if (dif.magnitude > 5)
                                target = transform.position;
                            else
                                my_turn = false;
                        }
                    }
                }
            }

            Vector2 delta = target - new Vector2(transform.position.x, transform.position.y);

            anim.SetBool("walking", delta != new Vector2());

            transition -= Time.deltaTime;
            if (transition < 0.0f)
                transition = 0.0f;
            if (delta == new Vector2() && !my_turn && transition <= 0.0f)
                advance = 1.0f;

            if (delta.x > 0.0f)
                facing = 1;
            if (delta.x < 0.0f)
                facing = -1;

            if (facing > 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            if (facing < 0)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            facing = 0;

            float max_len = speed * Time.deltaTime;
            if (delta.magnitude > max_len)
                delta = delta.normalized * max_len;
            transform.Translate(delta, Space.World);
        }
        else
        {
            advance = 1.0f;
        }
    }
}
