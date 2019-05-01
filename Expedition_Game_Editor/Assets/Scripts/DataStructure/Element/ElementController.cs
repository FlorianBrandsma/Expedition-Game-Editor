using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ElementController : MonoBehaviour, IDataController
{
    public bool searchById;
    public int temp_id_count;

    private ElementManager elementManager       = new ElementManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.Element; } }
    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = elementManager.GetElementDataElements(this);

        var elementDataElements = DataList.Cast<ElementDataElement>();

        //elementDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //elementDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}
