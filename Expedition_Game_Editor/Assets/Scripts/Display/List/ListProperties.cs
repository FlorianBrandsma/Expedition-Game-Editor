using UnityEngine;
using System.Collections.Generic;

public class ListProperties : MonoBehaviour, IDisplay
{
    //public GeneralData listData;

    //public List<GeneralData> dataList { get; set; }

    //public DataList dataList; //{ get; set; }

    public DisplayManager.Type displayType { get; set; }

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
    
    public IDataController dataController { get { return GetComponent<IDataController>(); } }

    public void InitializeProperties()
    {
        if(flexible_type)
        {
            //displayType = route.origin.displayType;
            
            //switch (displayType)
            //{
            //    case DisplayManager.Type.Button:
            //        gameObject.AddComponent<ButtonProperties>();
            //        break;
            //    case DisplayManager.Type.Tile:
            //        gameObject.AddComponent<TileProperties>();
            //        break;
            //    case DisplayManager.Type.Panel:
            //        gameObject.AddComponent<PanelProperties>();
            //        break;
            //    case DisplayManager.Type.PanelTile:
            //        gameObject.AddComponent<PanelTileProperties>();
            //        break;
            //    default: break;
            //}

            //dataList.data = route.data;

        } else {

            if (GetComponent<IProperties>() != null)
                displayType = GetComponent<IProperties>().Type();
        }
    }

    public void SetDisplay()
    {
        listManager.InitializeList(this);

        listManager.SetListSize();  
    }
       
    public void AutoSelectElement()
    {
        listManager.AutoSelectElement();
    }

    public void ResetList()
    {
        listManager.ResetData();
    } 

    public void CloseDisplay()
    {
        //Bandaid fix
        if (flexible_type)
        {
            //route.origin.displayType = DisplayManager.Type.None;
            //DestroyImmediate(GetComponent<IProperties>() as Object);
        }

        listManager.CloseList();
    }
}
