using UnityEngine;
using System;

public class GameElementData : GameData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameData OriginalData                    { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Game; } }
    
    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedRating
    {
        get { return Rating != OriginalData.Rating; }
    }

    public bool Changed
    {
        get { return ChangedRating; }
    }
    #endregion

    public GameElementData() { }

    public void Add(DataRequest dataRequest)
    {
        GameDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        GameDataManager.UpdateData(this, dataRequest);
    }
    
    public void Remove(DataRequest dataRequest)
    {
        GameDataManager.RemoveData(this, dataRequest);
    }
    
    public void SetOriginalValues()
    {
        OriginalData = base.Clone();
        
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
        var data = new GameElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
