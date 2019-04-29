using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    public ICollection dataList { get; set; }

    public Type type;
    public bool search_by_id;
    public int temp_id_count;

    ItemManager itemManager = new ItemManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = itemManager.GetItemDataElements(this);

        var itemDataElements = dataList.Cast<ItemDataElement>();

        //itemDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //itemDataElements[0].Update();
    }
}
