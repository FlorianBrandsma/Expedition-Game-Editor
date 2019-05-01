using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicController : MonoBehaviour, IDataController
{
    public bool searchById;
    public int temp_id_count;

    private ObjectGraphicManager objectGraphicManager = new ObjectGraphicManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.ObjectGraphic; } }
    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        List<int> id_list = new List<int>();

        for (int id = 1; id <= temp_id_count; id++)
            id_list.Add(id);

        GetData(id_list);
    }

    public void GetData(List<int> id_list)
    {
        DataList = objectGraphicManager.GetObjectGraphicDataElements(id_list);

        var objectGraphicDataElements = DataList.Cast<ObjectGraphicDataElement>();

        //Debug.Log(segmentController.editorController.pathController.route.data.element.Cast<ElementDataElement>().FirstOrDefault().object_graphic_id);
        //objectGraphicDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //objectGraphicDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}
