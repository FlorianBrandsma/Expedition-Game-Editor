using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemController : MonoBehaviour, IDataController
{
    public int temp_id_count;
    public Enums.ItemType itemType;

    public Search.Item searchParameters;

    private ItemManager itemManager             = new ItemManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Item; } }
    public ICollection DataList                 { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Item>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        itemManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = itemManager.GetItemDataElements(searchParameters);

        var itemDataElements = DataList.Cast<ItemDataElement>();

        //itemDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //itemDataElements[0].Update();
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }
}
