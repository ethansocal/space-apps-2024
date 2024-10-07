using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabScript : MonoBehaviour
{
    public List<TabSection> tabSections;
    // public Sprite tabIdle;
    // public Sprite tabHover;
    // public Sprite tabActive;
    public Color tabIdleColor;
    public Color tabHoverColor;
    public Color tabActiveColor;
    public List<GameObject> objectsToSwap;

    public TabSection selectedTab;

    public void Subscribe(TabSection section)
    {
        if (tabSections == null)
        {
            tabSections = new List<TabSection>();
        }

        tabSections.Add(section);
    }

    public void OnTabEnter(TabSection section)
    {
        ResetTabs();
        if (selectedTab == null || section != selectedTab)
        {
            // section.background.sprite = tabHover;
            section.GetComponent<Image>().color = tabHoverColor;
        }
    }

    public void OnTabExit(TabSection section)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabSection section)
    {
        selectedTab = section;
        ResetTabs();
        // section.background.sprite = tabActive;
        section.GetComponent<Image>().color = tabActiveColor; 
        int index = section.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            objectsToSwap[i].SetActive(i == index);
        }
    }

    public void ResetTabs()
    {
        foreach (TabSection section in tabSections)
        {
            if (selectedTab != null && section == selectedTab)
            {
                continue;
            }
            // section.background.sprite = tabIdle;
            section.GetComponent<Image>().color = tabIdleColor;
        }
    }
}