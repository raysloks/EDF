using UnityEngine;
using System.Collections.Generic;

public class TerrainScript : MonoBehaviour {

    List<List<bool>> grid;
    Vector2 origin, size;

	// Use this for initialization
	void Start () {

        origin = new Vector2(-5.0f, -5.0f);
        size = new Vector2(10.0f, 10.0f);

        grid = new List<List<bool>>();
        for (int x=0;x<size.x;++x)
        {
            grid.Add(new List<bool>());
            for (int y=0;y<size.y;++y)
            {
                grid[x].Add(true);
            }
        }
    }

    public List<Vector2> GetPath(Vector2 start, Vector2 end)
    {
        // adjust position
        start -= origin;
        end -= origin;

        // clamp position
        start.x = Mathf.Max(Mathf.Min(start.x, size.x), 0.0f);
        start.y = Mathf.Max(Mathf.Min(start.y, size.y), 0.0f);
        end.x = Mathf.Max(Mathf.Min(end.x, size.x), 0.0f);
        end.y = Mathf.Max(Mathf.Min(end.y, size.y), 0.0f);

        KeyValuePair<int, int> source = new KeyValuePair<int, int>(Mathf.FloorToInt(start.x), Mathf.FloorToInt(start.y));
        KeyValuePair<int, int> target = new KeyValuePair<int, int>(Mathf.FloorToInt(end.x), Mathf.FloorToInt(end.y));

        HashSet<KeyValuePair<int, int>> closed = new HashSet<KeyValuePair<int, int>>();
        HashSet<KeyValuePair<int, int>> open = new HashSet<KeyValuePair<int, int>>();
        open.Add(source);

        List<List<float>> g_score = new List<List<float>>();
        List<List<float>> f_score = new List<List<float>>();

        List<List<KeyValuePair<int, int>>> path = new List<List<KeyValuePair<int, int>>>();

        for (int x = 0; x < size.x; ++x)
        {
            g_score.Add(new List<float>());
            f_score.Add(new List<float>());
            path.Add(new List<KeyValuePair<int, int>>());
            for (int y = 0; y < size.y; ++y)
            {
                float gdist = float.PositiveInfinity;
                float fdist = float.PositiveInfinity;
                if (x == source.Key && y == source.Value)
                {
                    gdist = 0.0f;
                    fdist = Mathf.Sqrt((target.Key - source.Key) * (target.Key - source.Key) + (target.Value - source.Value) * (target.Value - source.Value));
                }
                g_score[x].Add(gdist);
                f_score[x].Add(fdist);
                path[x].Add(new KeyValuePair<int, int>(-1, -1));
            }
        }

        List<KeyValuePair<int, int>> nb = new List<KeyValuePair<int, int>>();
        nb.Add(new KeyValuePair<int, int>(1, 0));
        nb.Add(new KeyValuePair<int, int>(-1, 0));
        nb.Add(new KeyValuePair<int, int>(0, 1));
        nb.Add(new KeyValuePair<int, int>(0, -1));
        nb.Add(new KeyValuePair<int, int>(1, 1));
        nb.Add(new KeyValuePair<int, int>(-1, 1));
        nb.Add(new KeyValuePair<int, int>(1, -1));
        nb.Add(new KeyValuePair<int, int>(-1, -1));

        while (open.Count > 0)
        {
            KeyValuePair<int, int> current = new KeyValuePair<int, int>(-1, -1);
            float current_f = float.PositiveInfinity;
            foreach (KeyValuePair<int, int> node in open)
            {
                float fdist = f_score[node.Key][node.Value];
                if (fdist < current_f)
                {
                    current_f = fdist;
                    current = node;
                }
            }

            if (current.Key == target.Key && current.Value == target.Value)
            {
                List<Vector2> total_path = new List<Vector2>();
                while (current.Key >= 0)
                {
                    total_path.Add(new Vector2(current.Key + 0.5f, current.Value + 0.5f) + origin);
                    current = path[current.Key][current.Value];
                }
                return total_path;
            }

            open.Remove(current);
            closed.Add(current);

            for (int i=0;i<8;++i)
            {
                KeyValuePair<int, int> next = new KeyValuePair<int, int>(current.Key + nb[i].Key, current.Value + nb[i].Value);
                if (next.Key >= 0 && next.Key < size.x && next.Value >= 0 && next.Value < size.y && grid[next.Key][next.Value] && !closed.Contains(next))
                {
                    float gdist = g_score[current.Key][current.Value] + Mathf.Sqrt(nb[i].Key * nb[i].Key + nb[i].Value * nb[i].Value);
                    open.Add(next);
                    if (gdist < g_score[next.Key][next.Value])
                    {
                        path[next.Key][next.Value] = current;
                        g_score[next.Key][next.Value] = gdist;
                        f_score[next.Key][next.Value] = gdist + Mathf.Sqrt((target.Key - next.Key) * (target.Key - next.Key) + (target.Value - next.Value) * (target.Value - next.Value));
                    }
                }
            }
        }
        
        return new List<Vector2>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
