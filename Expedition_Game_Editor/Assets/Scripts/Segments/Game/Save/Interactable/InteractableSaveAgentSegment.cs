using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InteractableSaveAgentSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.InteractableSave);

        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractableSave>().First();
        searchParameters.type = new List<int>() { (int)Enums.InteractableType.Agent };

        SegmentController.DataController.GetData(searchProperties);
    }

    public void InitializeSegment()
    {
        InitializeData();
    }
    
    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment() { }
}
