using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ListData))]

public class ListProperties : MonoBehaviour
{
    public enum Type
    {
        None,
        Button,
        Tile,
        Panel,
    }

    public Type listType;

    private Route route;

    //public ListProperties.Type listType;
    public bool flexible_type;

    public RectTransform list_area;
    public RectTransform main_list;

    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;
    public bool always_on;

    //Only spawn visible elements
    public bool visible_only;

    public float base_size;

    public bool horizontal, vertical;

    public bool enable_sliders;
    public bool enable_numbers;
    public bool enable_slideshow;

    public IController controller { get; set; }

    public void InitializeProperties(Route new_route)
    {
        route = new_route;

        if(flexible_type)
        {
            listType = route.origin.listType;
            
            switch (listType)
            {
                case Type.Button:
                    gameObject.AddComponent<ButtonProperties>();
                    break;
                case Type.Tile:
                    gameObject.AddComponent<TileProperties>();
                    break;
                case Type.Panel:
                    gameObject.AddComponent<PanelProperties>();
                    break;
                default: break;
            }
            
            GetComponent<ListData>().data = route.data;
        } else {

            if (GetComponent<IProperties>() != null)
                listType = GetComponent<IProperties>().Type();
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

    public void CloseList()
    {
        //Bandaid fix
        if (flexible_type)
        {
            route.origin.listType = Type.None;
            DestroyImmediate(GetComponent<IProperties>() as Object);
        }

        main_list.GetComponent<ListManager>().CloseList();
    }
}
