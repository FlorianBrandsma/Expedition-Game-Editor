using UnityEngine;
using System.Collections;
using System.Linq;

public class ItemController : MonoBehaviour, IDataController
{
    public enum Type
    {
        Supplies,
        Gear,
        Spoils
    }

    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Item; } }

    public ICollection data_list { get; set; }

    public Type type;
    public bool search_by_id;
    public int temp_id_count;

    ItemManager itemManager = new ItemManager();

    public void InitializeController()
    {
        GetData(); 
    }

    public void GetData()
    {
        data_list = itemManager.GetItemDataElements(this);

        var itemDataElements = data_list.Cast<ItemDataElement>();
    }
}
