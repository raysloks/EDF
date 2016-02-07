using UnityEngine;
using System.Collections;

public class ClickScript : MonoBehaviour {

    public Animator anim;

    public Vector2 target;
    float speed = 4.0f;

    public bool my_turn;
    public float advance;

	// Use this for initialization
	void Start()
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Shadow")) as GameObject;
        ShadowScript ss = obj.GetComponent<ShadowScript>();
        ss.follow = transform;
	}

    // Update is called once per frame
    void Update()
    {
        if (my_turn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rh = new RaycastHit();
                if (Physics.Raycast(ray, out rh))
                {
                    target = rh.point;

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

        Vector2 delta = target - new Vector2(transform.position.x, transform.position.y);

        anim.SetBool("walking", delta != new Vector2());

        if (delta == new Vector2() && !my_turn)
            advance = 1.0f;

        if (delta.x > 0.0f)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        if (delta.x < 0.0f)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        float max_len = speed * Time.deltaTime;
        if (delta.magnitude > max_len)
            delta = delta.normalized * max_len;
        transform.Translate(delta, Space.World);

        if (Input.GetKeyDown(KeyCode.Space))
            transform.Translate(new Vector3(1.0f, 0.0f, 0.0f));
    }
}
