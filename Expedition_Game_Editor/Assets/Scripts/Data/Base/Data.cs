using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data
{
    public IDataController dataController;
    public List<IElementData> dataList;
    public SearchProperties searchProperties;

    public Data() { }

    public Data Clone()
    {
        var data = new Data();

        data.dataController = dataController;
        data.dataList = new List<IElementData>();

        dataList.ForEach(x => data.dataList.Add(x.Clone()));

        data.searchProperties = searchProperties;

        dataController.Data = data;

        return data;
    }
}
