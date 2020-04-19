using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.WorldObject);

        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldObject>().First();
        searchParameters.regionId = new List<int>() { EditorManager.editorManager.forms.First().activePath.FindLastRoute(Enums.DataType.Region).GeneralData.Id };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, searchProperties);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
