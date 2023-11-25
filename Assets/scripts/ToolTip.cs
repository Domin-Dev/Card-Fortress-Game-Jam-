using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : EventTrigger
{
    Transform infoTransform;

    private void Start()
    {
        infoTransform = transform.GetChild(0).transform;
        infoTransform.gameObject.SetActive(false);
    }
    public void SetActive(bool value)
    {
        infoTransform.gameObject.SetActive(value);          
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        SetActive(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        SetActive(false);
    }
}
