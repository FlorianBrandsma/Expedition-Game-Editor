using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TeamMemberDiscoverUserSegment : MonoBehaviour, ISegment
{
    public Enums.PortalGroupType portalGroupType;

    public ListProperties ListProperties        { get { return GetComponent<ListProperties>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.User);

        InitializeSearchParameters(searchProperties);
        
        SegmentController.DataController.GetData(searchProperties);
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.User>().First();

        searchParameters.teamId = SegmentController.Path.FindLastRoute(Enums.DataType.Team).ElementData.Id;

        searchParameters.requestType = Search.User.RequestType.GetPotentialTeamUsers;
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
