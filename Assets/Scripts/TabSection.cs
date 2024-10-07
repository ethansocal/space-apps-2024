using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabSection : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabScript tabScript;
    public Image background;
    void Start()
    {
        // background = GetComponent<Image>();
        if (tabScript == null)
        {
            Debug.LogError("TabScript is not assigned in the Inspector!");
            return;
        }
        tabScript.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (tabScript != null)
        {
            Debug.Log("Clicked on:" + this.name);
            tabScript.OnTabSelected(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tabScript != null)
        {
            tabScript.OnTabEnter(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tabScript != null)
        {
            tabScript.OnTabExit(this);
        }
    }
}