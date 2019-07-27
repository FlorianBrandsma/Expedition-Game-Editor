using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterGeneralPartyMemberSegment : MonoBehaviour, ISegment
{
    private DataManager dataManager = new DataManager();

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    private InteractableController ElementController { get { return (InteractableController)SegmentController.DataController; } }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        if (chapterEditor.partyMemberDataList.Count > 0) return;

        var chapterData = (ChapterDataElement)DataEditor.Data.DataElement;

        var searchParameters = new Search.PartyMember();

        searchParameters.requestType = Search.PartyMember.RequestType.Custom;
        searchParameters.chapterId = new List<int>() { chapterData.id };

        SegmentController.DataController.GetData(new[] { searchParameters });

        var partyMemberList = SegmentController.DataController.DataList.Cast<PartyMemberDataElement>().ToList();
        partyMemberList.ForEach(x => chapterEditor.partyMemberDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Interactable>().FirstOrDefault();

        List<int> idList = new List<int>();
        chapterEditor.partyMemberDataList.ForEach(x => idList.Add(x.InteractableId));
        chapterEditor.terrainInteractableDataList.ForEach(x => idList.Add(x.InteractableId));

        var list = dataManager.GetInteractableData().Where(x => !idList.Contains(x.id)).Select(x => x.id).Distinct().ToList();

        searchParameters.id = list;
    }

    public void OpenSegment()
    {
        SetSearchParameters();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
