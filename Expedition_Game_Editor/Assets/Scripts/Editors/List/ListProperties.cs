using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ListData))]

public class ListProperties : MonoBehaviour
{
    public enum Type
    {
        None,
        List,
        Grid,
        Panel,
    }

    public ListProperties.Type listType;
    public bool flexible_type;

    public Vector2 grid_size;

    public RectTransform list_area;
    public RectTransform main_list;

    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;
    public bool always_on;

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

    public void InitializeProperties(Route route)
    {
        if(flexible_type)
        {
            listType = route.origin.listType;
            
            GetComponent<ListData>().data = route.data;
        }     
    }

    public void SetList()
    {
        controller = GetComponent<IController>();
        main_list.GetComponent<ListManager>().SetProperties(this);

        main_list.GetComponent<ListManager>().SetListSize();  
    }
       
    public void AutoSelectElement()
    {
        main_list.GetComponent<ListManager>().AutoSelectElement();
    }

    public void ResetList()
    {
        main_list.GetComponent<ListManager>().ResetRows();
    } 
}
