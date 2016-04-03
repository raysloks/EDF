using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManagerScript : MonoBehaviour {

    public List<GameObject> lines;
    Sprite[] sprites;

    // Use this for initialization
    void Start () {
        lines = new List<GameObject>();
        sprites = Resources.LoadAll<Sprite>("Sprites/directions");

        for (int i=0;i<256;++i)
            lines.Add(Instantiate(Resources.Load("Prefabs/Line")) as GameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Construct(List<Vector2> path, Vector3 start)
    {
        Clear();
        Vector2 current = start;
        var nume = path.GetEnumerator();
        int i = 0;
        if (nume.MoveNext())
            while (nume.MoveNext())
            {
                GameObject line = lines[i++];
                line.SetActive(true);
                line.transform.position = current;
                SpriteRenderer rend = line.GetComponent<SpriteRenderer>();
                Vector2 delta = nume.Current - current;
                rend.sprite = rend.sprite = sprites[Mathf.RoundToInt(delta.x + 4.0f - delta.y * 3.0f)];
                current = nume.Current;
                if (i >= lines.Count)
                    break;
            }
    }

    public void Clear()
    {
        foreach (GameObject line in lines)
            line.SetActive(false);
    }
}
