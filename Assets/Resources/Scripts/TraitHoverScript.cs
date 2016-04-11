using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class TraitHoverScript : MonoBehaviour, UnityEngine.EventSystems.IPointerEnterHandler, UnityEngine.EventSystems.IPointerExitHandler {

    public RectTransform desc;
    Image image;

    float target_w, target_h;
    float min_w, min_h;
    float max_w, max_h;

    void Start()
    {
        target_w = -50.0f;
        target_h = -50.0f;

        min_w = 0.0f;
        min_h = 0.0f;

        max_w = 200.0f;
        max_h = 200.0f;

        image = desc.GetComponent<Image>();
    }

    void Update()
    {
        float weight = Mathf.Exp(Mathf.Log(0.01f) * Time.deltaTime);
        desc.sizeDelta = new Vector2(
            Mathf.Min(Mathf.Max(desc.sizeDelta.x * weight + target_w * (1.0f-weight), min_w), max_w),
            Mathf.Min(Mathf.Max(desc.sizeDelta.y * weight + target_h * (1.0f-weight), min_h), max_h));
        desc.gameObject.SetActive(desc.sizeDelta.x > 40.0f && desc.sizeDelta.y > 40.0f);
        image.color = new Color(image.color.r, image.color.g, image.color.b, Math.Max(Mathf.Min((desc.sizeDelta.x - 40.0f) / 40.0f, 1.0f), 0.0f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        target_w = 300.0f;
        target_h = 300.0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        target_w = -50.0f;
        target_h = -50.0f;
    }
}
