using UnityEngine;
using System;

public class FavoriteUserElementData : FavoriteUserData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public FavoriteUserData OriginalData            { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.FavoriteUser; } }
    
    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedNote
    {
        get { return Note != OriginalData.Note; }
    }
    
    public bool Changed
    {
        get { return ChangedNote; }
    }
    #endregion

    public FavoriteUserElementData() { }

    public void Add(DataRequest dataRequest)
    {
        FavoriteUserDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        FavoriteUserDataManager.UpdateData(this, dataRequest);
    }
    
    public void Remove(DataRequest dataRequest)
    {
        FavoriteUserDataManager.RemoveData(this, dataRequest);
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
        var data = new FavoriteUserElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
