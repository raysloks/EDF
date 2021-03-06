﻿using UnityEngine;
using System.Collections.Generic;

public class TerrainScript : MonoBehaviour {

    List<List<GameObject>> grid;
    Vector2 origin, size;
    
	public void Init () {
        origin = new Vector2(-5.0f, -5.0f);
        size = new Vector2(10.0f, 10.0f);

        grid = new List<List<GameObject>>();
        for (int x=0;x<size.x;++x)
        {
            grid.Add(new List<GameObject>());
            for (int y=0;y<size.y;++y)
            {
                grid[x].Add(null);
            }
        }
    }

    public void SetCell(Vector2 position, GameObject value)
    {
        KeyValuePair<int, int> cpos = GetCellPosition(position);
        grid[cpos.Key][cpos.Value] = value;
    }

    public GameObject GetCell(Vector2 position)
    {
        KeyValuePair<int, int> cpos = GetCellPosition(position);
        return grid[cpos.Key][cpos.Value];
    }

    public KeyValuePair<int, int> GetCellPosition(Vector2 position)
    {
        position -= origin;

        position.x = Mathf.Max(Mathf.Min(position.x, size.x), 0.0f);
        position.y = Mathf.Max(Mathf.Min(position.y, size.y), 0.0f);

        return new KeyValuePair<int, int>(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
    }

    public void GetPath(TargetData td)
    {
        KeyValuePair<int, int> source = GetCellPosition(td.start);
        KeyValuePair<int, int> target = GetCellPosition(td.end);

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
            List<KeyValuePair<int, int>> lowest = new List<KeyValuePair<int, int>>();
            foreach (KeyValuePair<int, int> node in open)
            {
                float fdist = f_score[node.Key][node.Value];
                if (fdist < current_f)
                {
                    current_f = fdist;
                    current = node;
                    lowest.Clear();
                    lowest.Add(node);
                }
                else
                {
                    if (fdist == current_f)
                    {
                        lowest.Add(node);
                    }
                }
            }
            if (td.random)
            {
                current = lowest[RandomManager.ai.Next(lowest.Count)];
            }

            open.Remove(current);
            closed.Add(current);

            bool finished = false;
            if (td.use_end)
                finished = current.Key == target.Key && current.Value == target.Value;
            else
            {
                GameObject go = grid[current.Key][current.Value];
                if (go != null)
                {
                    ClickScript cs = go.GetComponent<ClickScript>();
                    if (cs != null)
                        finished = cs.team != td.searcher.team;
                }
            }
            if (finished)
            {
                List<Vector2> total_path = new List<Vector2>();
                while (current.Key >= 0)
                {
                    total_path.Add(new Vector2(current.Key + 0.5f, current.Value + 0.5f) + origin);
                    current = path[current.Key][current.Value];
                }
                td.paths.Add(total_path);
                if (td.use_end)
                    return;
                continue;
            }

            for (int i=0;i<8;++i)
            {
                KeyValuePair<int, int> next = new KeyValuePair<int, int>(current.Key + nb[i].Key, current.Value + nb[i].Value);
                if (next.Key >= 0 && next.Key < size.x && next.Value >= 0 && next.Value < size.y && !closed.Contains(next))
                {
                    GameObject go = grid[next.Key][next.Value];
                    bool can_advance = go == null;
                    if (go != null)
                    {
                        if (td.use_end)
                            can_advance |= next.Value == target.Value && next.Key == target.Key;
                        else
                        {
                            ClickScript cs = go.GetComponent<ClickScript>();
                            if (cs != null)
                                can_advance |= cs.team != td.searcher.team && cs.hp.current > 0;
                        }
                    }
                    if (can_advance)
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
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
