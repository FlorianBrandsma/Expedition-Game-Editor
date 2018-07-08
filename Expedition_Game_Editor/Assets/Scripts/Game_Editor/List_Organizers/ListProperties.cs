﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RowManager))]

public class ListProperties : MonoBehaviour
{
    public Vector2 grid_size;

    public RectTransform list_area;
    public RectTransform main_list;
    /*
    //Main editors create the select/edit delegates
    public bool main_editor;
    //Was false in SecondaryWindow
    */
    public bool auto_select;

    public bool get_select, set_select;

    //Only spawn visible elements
    public bool visible_only;

    public bool zigzag;
    //Spawn tiles in rect without altering size
    public bool fit_axis;

    public float base_size;

    public bool horizontal, vertical;

    public bool enable_sliders;
    public bool enable_numbers;
    public bool enable_slideshow;

    public IController controller { get; set; }

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

        //3.7.2018 ListProperties and RowManager are now interwoven

        GetComponent<RowManager>().SetRows();

        controller = GetComponent<IController>();
        main_list.GetComponent<ListManager>().SetProperties(this);

        main_list.GetComponent<ListManager>().SetListSize(base_size);

        //Automatically selects and highlights an element(id) on startup
        if (auto_select)
            SelectElement(GetComponent<EditorController>().id);
    }
       
    void SelectElement(int id)
    {
        main_list.GetComponent<ListManager>().SelectElement(id, false);
    }  
}
