using UnityEngine;
using System;
using System.Collections.Generic;

public class EditorWorldElementData : EditorWorldData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public EditorWorldData OriginalData             { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.EditorWorld; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }
    
    public List<TerrainElementData> TerrainDataList { get; set; } = new List<TerrainElementData>();
    public List<PhaseElementData> PhaseDataList     { get; set; } = new List<PhaseElementData>();

    #region Changed
    public bool Changed { get; set; }
    #endregion

    public void Add(DataRequest dataRequest) { }

    public void Update(DataRequest dataRequest) { }

    public void Remove(DataRequest dataRequest) { }

    public void SetOriginalValues()
    {
        OriginalData = base.Clone();

        TerrainDataList.ForEach(x => x.SetOriginalValues());
        PhaseDataList.ForEach(x => x.SetOriginalValues());

        ClearChanges();
    }
    
    public void ClearChanges()
    {
        if (!Changed) return;

        GetOriginalValues();
    }

    public void GetOriginalValues()
    {
        base.GetOriginalValues(OriginalData);
    }

    public new IElementData Clone()
    {
        var data = new EditorWorldElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();
        
        base.Clone(data);

        return data;
    }
}
