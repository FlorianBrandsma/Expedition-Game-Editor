﻿using UnityEngine;
using System.Collections.Generic;

public class Data
{
    public IDataController dataController;
    public List<IElementData> dataList;
    public SearchProperties searchProperties;

    public Data() { }

    public Data(IDataController dataController)
    {
        this.dataController = dataController;
        searchProperties = dataController.SearchProperties;
    }

    public Data Clone()
    {
        var data = new Data();

        data.dataController = dataController;
        data.dataList = new List<IElementData>();

        dataList.ForEach(x => data.dataList.Add(x.Clone()));

        data.searchProperties = searchProperties;

        //Disabled because of issues with editors opened from a different form. Data was cloned and given to
        //the data controller where it would otherwise be copied
        //dataController.Data = data;

        return data;
    }
}
