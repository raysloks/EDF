using UnityEngine;
using System.Collections;

public class ClickScript : MonoBehaviour {

    public Animator anim;

    public Vector2 target;
    float speed = 4.0f;

	// Use this for initialization
	void Start()
    {
	
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rh = new RaycastHit();
            if (Physics.Raycast(ray, out rh))
                target = rh.point;
        }

        target.x = Mathf.Round(target.x-0.5f)+0.5f;
        target.y = Mathf.Round(target.y-0.5f)+0.5f;

        Vector2 delta = target - new Vector2(transform.position.x, transform.position.y);

        anim.SetBool("walking", delta != new Vector2());

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
