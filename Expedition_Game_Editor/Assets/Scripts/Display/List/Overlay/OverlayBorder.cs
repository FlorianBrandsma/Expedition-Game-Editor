using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverlayBorder : MonoBehaviour
{
    static public List<RectTransform> scroll_parent_list = new List<RectTransform>();

    public bool     always_active;

    //public bool     active { get; set; }

    public bool     horizontal, 
                    vertical;

    public Vector2 offset_min;
    public Vector2 offset_max;

    public RectTransform scroll_parent { get; set; }

    public void Activate()
    {
        GetComponent<RectTransform>().offsetMin = offset_min;
        GetComponent<RectTransform>().offsetMax = offset_max;

        if (always_active)
            gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        

        if(scroll_parent != null)
            ResetScrollParent();

        gameObject.SetActive(false);
    }

    public RectTransform ScrollParent()
    {
        if(scroll_parent == null)
        {
            foreach (RectTransform new_parent in scroll_parent_list)
            {
                if (!new_parent.gameObject.activeInHierarchy)
                {
                    scroll_parent = new_parent;
                    scroll_parent.SetParent(gameObject.transform, false);

                    scroll_parent.gameObject.SetActive(true);

                    return scroll_parent;
                }      
            }

            scroll_parent = Instantiate(Resources.Load<RectTransform>("Editor/Overlay/Scroll_Parent"));

            scroll_parent_list.Add(scroll_parent);

            scroll_parent.SetParent(gameObject.transform, false);
        }

        return scroll_parent;
    }

    void ResetScrollParent()
    {
        scroll_parent.gameObject.SetActive(false);
        scroll_parent = null;
    }
}
