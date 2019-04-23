using UnityEngine;
using System.Collections;
using System.Linq;

public class ElementController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type             { get { return Enums.DataType.Element; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    ElementManager elementManager = new ElementManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = elementManager.GetElementDataElements(this);

        var elementDataElements = data_list.Cast<ElementDataElement>();

        //elementDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
    }
}
