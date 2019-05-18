using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterElementController : MonoBehaviour, IDataController
{
    public Search.Element searchParameters;

    public ChapterElementDataManager chapterElementDataManager = new ChapterElementDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.ChapterElement; } }
    public ICollection DataList                 { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Element>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        chapterElementDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = chapterElementDataManager.GetChapterElementDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = searchElement.route.data.ElementData.Cast<ChapterElementDataElement>().FirstOrDefault();

        var chapterElementDataElement = DataList.Cast<ChapterElementDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch(resultData.DataController.DataType)
        {
            case Enums.DataType.Element:

                var resultElementData = resultData.ElementData.Cast<ElementDataElement>().FirstOrDefault();

                chapterElementDataElement.ElementId = resultElementData.id;
                chapterElementDataElement.objectGraphicIcon = resultElementData.objectGraphicIcon;

                break;
        }
        
        searchElement.route.data.ElementData = new[] { chapterElementDataElement };
    }
}
