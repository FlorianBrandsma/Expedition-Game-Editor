using UnityEngine;
using System.Collections;

public class ListOptions : MonoBehaviour
{
    public RectTransform option_list;

    public Vector2 min_anchor;
    public Vector2 max_anchor;

    public bool auto_select;

    public void SetList()
    {
        SetListAnchors();

        option_list.GetComponent<ListManager>().SetList(max_anchor.x);

        //Automatically selects an element(id) on startup
        if (auto_select)
            SelectElement(GetComponent<SubEditor>().id);
    }

    void SetListAnchors()
    {
        option_list.anchorMin = min_anchor;
        option_list.anchorMax = max_anchor;

        option_list.gameObject.SetActive(true);
    }
    
    void SelectElement(int id)
    {
        option_list.GetComponent<ListManager>().SelectElement(id, false);
    }  
}
