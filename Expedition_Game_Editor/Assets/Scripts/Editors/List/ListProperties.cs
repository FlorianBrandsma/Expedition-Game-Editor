using UnityEngine;

[RequireComponent(typeof(ListData))]

public class ListProperties : MonoBehaviour
{
    public enum Type
    {
        None,
        Button,
        Tile,
        Panel,
        PanelTile,
    }

    public Type listType { get; set; }

    private Route route;

    //public ListProperties.Type listType;
    public bool flexible_type;

    public ListManager listManager;

    public SelectionManager.Type selectionType;
    public SelectionManager.Property selectionProperty;
    public bool always_on;

    //Only spawn visible elements
    public bool visible_only;

    public Vector2 element_size;

    public bool horizontal, vertical;

    public bool enable_sliders;
    public bool enable_numbers;
    public bool enable_paging;

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
                case Type.PanelTile:
                    gameObject.AddComponent<PanelTileProperties>();
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
        listManager.SetProperties(this);

        listManager.SetListSize();  
    }
       
    public void AutoSelectElement()
    {
        listManager.AutoSelectElement();
    }

    public void ResetList()
    {
        listManager.ResetRows();
    } 

    public void CloseList()
    {
        //Bandaid fix
        if (flexible_type)
        {
            route.origin.listType = Type.None;
            DestroyImmediate(GetComponent<IProperties>() as Object);
        }

        listManager.CloseList();
    }
}
