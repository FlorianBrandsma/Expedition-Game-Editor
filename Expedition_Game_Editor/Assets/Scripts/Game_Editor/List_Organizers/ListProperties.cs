using UnityEngine;
using System.Collections;

public class ListProperties : MonoBehaviour
{
    public RectTransform main_list;

    public Vector2 min_anchor;
    public Vector2 max_anchor;

    public bool auto_select;

    public void SetList()
    {
        SetListAnchors();

        main_list.GetComponent<ListManager>().SetList(max_anchor.x);

        //Automatically selects and highlights an element(id) on startup
        if (auto_select)
            SelectElement(GetComponent<SubEditor>().id);
    }

    void SetListAnchors()
    {
        main_list.anchorMin = min_anchor;
        main_list.anchorMax = max_anchor;

        main_list.gameObject.SetActive(true);
    }
    
    void SelectElement(int id)
    {
        main_list.GetComponent<ListManager>().SelectElement(id, false);
    }  
}
