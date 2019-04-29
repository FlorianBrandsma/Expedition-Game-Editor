using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ElementController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Element; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    ElementManager elementManager = new ElementManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = elementManager.GetElementDataElements(this);

        var elementDataElements = dataList.Cast<ElementDataElement>();

        //elementDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //elementDataElements[0].Update();
    }
}
