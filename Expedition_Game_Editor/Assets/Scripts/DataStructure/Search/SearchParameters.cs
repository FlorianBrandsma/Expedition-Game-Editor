using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SearchParameters
{
    [HideInInspector]
    public List<int> includedIdList = new List<int>();
    [HideInInspector]
    public List<int> exclusedIdList = new List<int>();

    public bool searching;

    public bool unique;
    public bool displayNullId;

    public SearchParameters Copy()
    {
        SearchParameters searchParameters = new SearchParameters();

        searchParameters.includedIdList = includedIdList;
        searchParameters.exclusedIdList = exclusedIdList;

        searchParameters.unique = unique;
        searchParameters.displayNullId = displayNullId;

        return searchParameters;
    }
}
