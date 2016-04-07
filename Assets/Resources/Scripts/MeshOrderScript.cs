using UnityEngine;

[ExecuteInEditMode]
public class MeshOrderScript : MonoBehaviour {

    public string layerName;
    public int order;

    Renderer rend;
    
	void Start()
    {
        rend = GetComponent<Renderer>();
        rend.sortingLayerName = layerName;
        rend.sortingOrder = order;
	}
	
	void Update()
    {
        if (rend.sortingLayerName != layerName)
            rend.sortingLayerName = layerName;
        if (rend.sortingOrder != order)
            rend.sortingOrder = order;
    }

    void OnValidate()
    {
        rend = GetComponent<Renderer>();
        rend.sortingLayerName = layerName;
        rend.sortingOrder = order;
    }
}
