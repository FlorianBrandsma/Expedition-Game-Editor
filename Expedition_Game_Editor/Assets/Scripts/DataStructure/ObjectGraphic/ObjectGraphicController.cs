using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicController : MonoBehaviour, IDataController
{
    [HideInInspector]
    public List<int> idList = new List<int>();
    public int temp_id_count;

    public SearchParameters searchParameters;

    private ObjectGraphicManager objectGraphicManager = new ObjectGraphicManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.ObjectGraphic; } }
    public ICollection DataList                 { get; set; }

    public SearchParameters SearchParameters
    {
        get { return searchParameters; }
        set { searchParameters = value; }
    }

    public void InitializeController()
    {
        for (int id = 0; id < temp_id_count; id++)
            idList.Add(id);

        GetData(idList);
    }

    public void GetData(List<int> idList)
    {
        DataList = objectGraphicManager.GetObjectGraphicDataElements(idList);

        var objectGraphicDataElements = DataList.Cast<ObjectGraphicDataElement>();

        //Debug.Log(segmentController.editorController.pathController.route.data.element.Cast<ElementDataElement>().FirstOrDefault().object_graphic_id);
        //objectGraphicDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //objectGraphicDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }

    public void ReplaceData(IEnumerable dataElement)
    {

    }
}
