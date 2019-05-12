using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchParameters
{
    public Enums.DataType dataType;

    [HideInInspector]
    public List<int> idList;
    [HideInInspector]
    public List<int> includedIdList = new List<int>();
    [HideInInspector]
    public List<int> exclusedIdList = new List<int>();
    
    [HideInInspector]
    public bool displayNullId;

    public int temp_id_count;

    public bool unique;
    
    public string value;

    public SearchParameters Copy()
    {
        SearchParameters searchParameters = new SearchParameters();

        searchParameters.includedIdList = includedIdList;
        searchParameters.exclusedIdList = exclusedIdList;

        searchParameters.unique = unique;
        //searchParameters.displayNullId = displayNullId;

        return searchParameters;
    }
}
