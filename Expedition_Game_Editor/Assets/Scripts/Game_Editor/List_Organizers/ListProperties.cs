using UnityEngine;
using System.Collections;

public class ListProperties : MonoBehaviour
{
    public RectTransform main_list;

    public Vector2 min_anchor;
    public Vector2 max_anchor;

    //Main editors create the select/edit delegates
    public bool main_editor;

    public bool horizontal, vertical;

    public bool auto_select;

    public RowManager rowManager;

    public void SetList()
    {
        //1. Change the overall size of the list parent by changing it's Anchors
        //2. Determine this list's organizer
        //3. Set up rows to determine the size of the list
        //4. Set the size of the list children
        //5. Add list children with the organizer

        //1. ListProperties
        //2. ListProperties > RowManager > ListManager > Organizer
        //3. ListProperties > ListManager > Organizer

        SetListAnchors();

        rowManager.SetupRows(main_editor);

        main_list.GetComponent<ListManager>().SetAxis(horizontal, vertical);
        main_list.GetComponent<ListManager>().SetListSize(max_anchor.x);

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
