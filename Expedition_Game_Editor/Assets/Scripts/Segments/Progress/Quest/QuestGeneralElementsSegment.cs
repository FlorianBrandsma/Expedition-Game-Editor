using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestGeneralElementsSegment : MonoBehaviour, ISegment
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

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        var questEditor = (QuestEditor)DataEditor;

        if (questEditor.questElementDataList.Count > 0) return;

        var questData = (QuestDataElement)DataEditor.Data.DataElement;

        var searchParameters = new Search.PhaseElement();

        searchParameters.requestType = Search.PhaseElement.RequestType.Custom;
        searchParameters.phaseId = new List<int>() { questData.PhaseId };

        SegmentController.DataController.GetData(new[] { searchParameters });

        var questElementList = SegmentController.DataController.DataList.Cast<PhaseElementDataElement>().ToList();

        questElementList.ForEach(x => questEditor.questElementDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        //QuestDataElement questData = DataEditor.Data.ElementData.Cast<QuestDataElement>().FirstOrDefault();
        //var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Element>().FirstOrDefault();

        ////Out of all the elements, select only those that are not in this list
        //var idList = SegmentController.DataController.DataList.Cast<QuestElementDataElement>().Select(x => x.ElementId).ToList();
        //var list = dataManager.GetElementData().Where(x => !idList.Contains(x.id)).Select(x => x.id).Distinct().ToList();

        //searchParameters.id = list;
    }

    public void OpenSegment()
    {
        SetSearchParameters();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        var questData = (QuestDataElement)DataEditor.Data.DataElement;

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
