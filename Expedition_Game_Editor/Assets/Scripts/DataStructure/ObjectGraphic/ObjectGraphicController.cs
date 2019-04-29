using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.ObjectGraphic; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    ObjectGraphicManager objectGraphicManager = new ObjectGraphicManager();

    public void InitializeController()
    {
        List<int> id_list = new List<int>();

        for (int id = 1; id <= temp_id_count; id++)
            id_list.Add(id);

        GetData(id_list);
    }

    public void GetData(List<int> id_list)
    {
        dataList = objectGraphicManager.GetObjectGraphicDataElements(id_list);

        var objectGraphicDataElements = dataList.Cast<ObjectGraphicDataElement>();

        //Debug.Log(segmentController.editorController.pathController.route.data.element.Cast<ElementDataElement>().FirstOrDefault().object_graphic_id);
        //objectGraphicDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //objectGraphicDataElements[0].Update();
    }
}
