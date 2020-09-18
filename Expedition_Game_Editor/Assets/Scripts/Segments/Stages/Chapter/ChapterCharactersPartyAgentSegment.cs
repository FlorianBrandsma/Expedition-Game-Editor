using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterCharactersPartyAgentSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private ChapterEditor ChapterEditor         { get { return (ChapterEditor)DataEditor; } }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.PartyMember);

        var searchParameters = searchProperties.searchParameters.Cast<Search.PartyMember>().First();
        searchParameters.chapterId = new List<int>() { ChapterEditor.Id };

        SegmentController.DataController.GetData(searchProperties);

        var partyMemberList = SegmentController.DataController.Data.dataList.Cast<PartyMemberElementData>().ToList();
        partyMemberList.ForEach(x => ChapterEditor.partyMemberElementDataList.Add(x));
    }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        SetSearchParameters();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    private void SetSearchParameters()
    {
        var searchProperties = SegmentController.DataController.SearchProperties;

        var searchParameters = searchProperties.searchParameters.Cast<Search.Interactable>().First();

        var idList = ChapterEditor.partyMemberElementDataList.Select(x => x.InteractableId).Union(
                     ChapterEditor.chapterInteractableElementDataList.Select(x => x.InteractableId)).Distinct().ToList();

        //Find interactables where id is not in the list
        var list = DataManager.GetInteractableData().Where(x => !idList.Contains(x.Id)).Select(x => x.Id).Distinct().ToList();

        searchParameters.id = list;
    }

    public void SetSearchResult(IElementData elementData)
    {
        SetSearchPartyMember((PartyMemberElementData)elementData);

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }

    private void SetSearchPartyMember(PartyMemberElementData resultElementData)
    {
        var elementData = ChapterEditor.partyMemberElementDataList.Where(x => x.Id == resultElementData.Id).First();

        elementData.InteractableId      = resultElementData.InteractableId;
        elementData.InteractableName    = resultElementData.InteractableName;
        elementData.ModelIconPath       = resultElementData.ModelIconPath;
    }

    public void CloseSegment() { }
}
