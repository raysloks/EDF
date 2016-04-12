using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TraitSelectScript : MonoBehaviour {

    TraitList traits;

    public TurnManagerScript tm;

    public GameObject tsgo;

	void Start()
    {
        traits = new TraitList();

        ReconstructGUI();
    }

    public void OnTraitSelected(string trait_name)
    {
        var tf = traits.traits[trait_name];
        if (tm != null)
        {
            if (tf != null)
            {
                if (tm.current_turnholder != null)
                {
                    var trait = tf.Instantiate();
                    trait.Attach(tm.current_turnholder);
                    tm.current_turnholder.status.Add(trait);
                }
            }
        }
    }

    void ReconstructGUI()
    {
        var nume = traits.traits.GetEnumerator();
        float y = -20.0f;
        while (nume.MoveNext())
        {
            GameObject test = Instantiate(Resources.Load("Prefabs/Trait")) as GameObject;
            test.transform.SetParent(tsgo.transform);
            test.transform.localPosition = new Vector3(100.0f, y, 0.0f);
            var text = test.GetComponentsInChildren<Text>();
            text[0].text = nume.Current.Key;
            text[1].text = nume.Current.Value.GetDescription();
            var button = test.GetComponentInChildren<Button>();
            string key = nume.Current.Key;
            button.onClick.AddListener(delegate { OnTraitSelected(key); });
            y -= 40.0f;
        }
        ((RectTransform)tsgo.transform).sizeDelta = new Vector2(0.0f, -y - 20.0f);
    }

    void OnGUI()
    {
        //var nume = traits.traits.GetEnumerator();
        //float y = 20.0f;
        //while (nume.MoveNext())
        //{
        //    if (GUI.Button(new Rect(30.0f, y, 200.0f, 20.0f), new GUIContent(nume.Current.Value.GetName(), nume.Current.Value.GetDescription())))
        //    {
        //        if (tm != null)
        //            if (tm.current_turnholder != null)
        //            {
        //                var trait = nume.Current.Value.Instantiate();
        //                trait.Attach(tm.current_turnholder);
        //                tm.current_turnholder.status.Add(trait);
        //            }
        //    }
        //    y += 30.0f;
        //}
        //GUI.Label(new Rect(260.0f, 20.0f, 200.0f, 100.0f), GUI.tooltip);

        if (tm != null)
        {
            if (tm.current_turnholder != null)
                GUI.Label(new Rect(500.0f, 20.0f, 100.0f, 20.0f), tm.current_turnholder.hp.current.ToString() + " / " + tm.current_turnholder.hp.max.ToString());
            if (GUI.Button(new Rect(800.0f, 20.0f, 100.0f, 20.0f), "Save"))
                tm.save_manager.Save(tm);
            if (GUI.Button(new Rect(800.0f, 60.0f, 100.0f, 20.0f), "Load"))
                tm.save_manager.Load(tm);
            if (GUI.Button(new Rect(800.0f, 100.0f, 100.0f, 20.0f), "New Game"))
                tm.TempNewGame();
        }
    }
}
