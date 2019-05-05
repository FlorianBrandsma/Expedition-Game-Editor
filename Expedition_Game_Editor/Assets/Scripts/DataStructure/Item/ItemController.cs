using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemController : MonoBehaviour, IDataController
{
    public int temp_id_count;
    public Enums.ItemType itemType;

    public SearchParameters searchParameters;

    private ItemManager itemManager             = new ItemManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Item; } }
    public ICollection DataList                 { get; set; }

    public SearchParameters SearchParameters
    {
        get { return searchParameters; }
        set { searchParameters = value; }
    }

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

    public void ReplaceData(IEnumerable dataElement)
    {

    }
}
