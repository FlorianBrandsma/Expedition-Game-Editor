using UnityEngine;
using System.Linq;

public class TeamSegment : MonoBehaviour, ISegment
{
    public Enums.PortalGroupType portalGroupType;

    public ListProperties ListProperties        { get { return GetComponent<ListProperties>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.Team);

        InitializeSearchParameters(searchProperties);
        
        SegmentController.DataController.GetData(searchProperties);
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Team>().First();

        searchParameters.includeAddElement = ListProperties.AddProperty != SelectionManager.Property.None;

        searchParameters.userId = SegmentController.Path.FindLastRoute(Enums.DataType.User).ElementData.Id;

        //Look for users that are associated with the team
        searchParameters.associated = portalGroupType != Enums.PortalGroupType.Discover;

        //Look for users that are actually part of the team
        searchParameters.joined = portalGroupType == Enums.PortalGroupType.Current;

        searchParameters.requestType = Search.Team.RequestType.GetUserTeams;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }
    
    public void OpenSegment()
    {
        if(ListProperties != null)
            ListProperties.DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
