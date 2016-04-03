using UnityEngine;
using System.Collections;

public class TextParticleScript : MonoBehaviour {

	SpriteRenderer sr;

	public float alpha_speed = 0.25f;
	public float up_speed = 0.5f;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3 (0.0f, up_speed * Time.deltaTime, 0.0f);
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - alpha_speed * Time.deltaTime);
		if (sr.color.a <= 0.0f)
			Destroy (gameObject);
	}
}
