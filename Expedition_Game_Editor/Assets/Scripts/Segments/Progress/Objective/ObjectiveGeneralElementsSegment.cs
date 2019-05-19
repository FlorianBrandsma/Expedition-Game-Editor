using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveGeneralElementsSegment : MonoBehaviour, ISegment
{
    private DataManager dataManager = new DataManager();

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    private ElementController ElementController { get { return (ElementController)SegmentController.DataController; } }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void InitializeSegment()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;

        InitializeElementData();

        SetSearchParameters();
    }

    private void InitializeElementData()
    {
        var objectiveEditor = (ObjectiveEditor)DataEditor;

        if (objectiveEditor.objectiveElementDataList.Count > 0) return;

        ObjectiveDataElement objectiveData = DataEditor.Data.ElementData.Cast<ObjectiveDataElement>().FirstOrDefault();

        var searchParameters = new Search.ObjectiveElement();

        searchParameters.requestType = Search.ObjectiveElement.RequestType.Custom;
        //searchParameters.objectiveId = new List<int>() { objectiveData.id };
        searchParameters.temp_id_count = 4;

        SegmentController.DataController.GetData(new[] { searchParameters });

        var objectiveElementList = SegmentController.DataController.DataList.Cast<ObjectiveElementDataElement>().ToList();

        objectiveElementList.ForEach(x => objectiveEditor.objectiveElementDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        //ObjectiveDataElement objectiveData = DataEditor.Data.ElementData.Cast<ObjectiveDataElement>().FirstOrDefault();
        //var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Element>().FirstOrDefault();

        ////Out of all the elements, select only those that are not in this list
        //var idList = SegmentController.DataController.DataList.Cast<ObjectiveElementDataElement>().Select(x => x.ElementId).ToList();
        //var list = dataManager.GetElementData().Where(x => !idList.Contains(x.id)).Select(x => x.id).Distinct().ToList();

        //searchParameters.id = list;
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        ObjectiveDataElement objectiveData = DataEditor.Data.ElementData.Cast<ObjectiveDataElement>().FirstOrDefault();

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
