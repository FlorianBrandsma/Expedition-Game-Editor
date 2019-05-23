using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicController : MonoBehaviour, IDataController
{
    public Search.ObjectGraphic searchParameters;

    private ObjectGraphicDataManager objectGraphicDataManager = new ObjectGraphicDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.ObjectGraphic; } }
    public ICollection DataList                 { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.ObjectGraphic>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        objectGraphicDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = objectGraphicDataManager.GetObjectGraphicDataElements(searchParameters);

        var objectGraphicDataElements = DataList.Cast<ObjectGraphicDataElement>();

        //Debug.Log(segmentController.editorController.pathController.route.data.element.Cast<ElementDataElement>().FirstOrDefault().object_graphic_id);
        //objectGraphicDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //objectGraphicDataElements[0].Update();
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = searchElement.route.data.ElementData.Cast<ObjectGraphicDataElement>().FirstOrDefault();

        var objectGraphicDataElement = DataList.Cast<ObjectGraphicDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var resultElementData = resultData.ElementData.Cast<ObjectGraphicDataElement>().FirstOrDefault();

                objectGraphicDataElement.id = resultElementData.id;
                objectGraphicDataElement.Icon = resultElementData.Icon;
                objectGraphicDataElement.Path = resultElementData.Path;
                objectGraphicDataElement.Name = resultElementData.Name;
             
                break;
        }

        searchElement.route.data.ElementData = new[] { objectGraphicDataElement };
    }
}
