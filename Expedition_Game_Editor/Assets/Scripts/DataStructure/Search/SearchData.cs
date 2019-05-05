using UnityEngine;
using System.Collections;

public class SearchData : GeneralData
{
    public SearchParameters searchParameters;

    public SearchData(SearchParameters searchParameters, GeneralData generalData)
    {
        id = generalData.id;
        table = generalData.table;
        type = generalData.type;
        index = generalData.index;

        this.searchParameters = searchParameters;
    }
}
