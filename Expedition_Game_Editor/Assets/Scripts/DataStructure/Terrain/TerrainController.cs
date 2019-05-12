﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainController : MonoBehaviour//, IDataController
{
    public int temp_id_count;

    public SearchParameters searchParameters;

    private TerrainManager terrainManager       = new TerrainManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Terrain; } }
    public ICollection DataList                 { get; set; }

    public SearchParameters SearchParameters
    {
        get { return searchParameters; }
        set { searchParameters = value; }
    }

    public void InitializeController()
    {
        //GetData(new List<int>());
    }

    public void GetData(SearchParameters searchParameters)
    {
        DataList = terrainManager.GetTerrainDataElements(this);

        var terrainDataElements = DataList.Cast<TerrainDataElement>();

        //terrainDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //terrainDataElements[0].Update();
    }

    public void ReplaceData(IEnumerable dataElement)
    {

    }
}