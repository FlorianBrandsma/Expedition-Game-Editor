using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemController : MonoBehaviour, IDataController
{
    public Enums.ItemType itemType;
    public bool searchById;
    public int temp_id_count;

    private ItemManager itemManager             = new ItemManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.Item; } }
    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = itemManager.GetItemDataElements(this);

        var itemDataElements = DataList.Cast<ItemDataElement>();

        //itemDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //itemDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}
