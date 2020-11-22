using UnityEngine;
using System;

public class ItemElementData : ItemData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public ItemData OriginalData                    { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Item; } }
    
    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedModelId
    {
        get { return ModelId != OriginalData.ModelId; }
    }

    public bool ChangedIndex
    {
        get { return Index != OriginalData.Index; }
    }

    public bool ChangedName
    {
        get { return Name != OriginalData.Name; }
    }
    
    public bool Changed
    {
        get { return ChangedModelId || ChangedName; }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;
        
        ItemDataManager.UpdateData(this);

        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        if (!ChangedIndex) return;

        ItemDataManager.UpdateIndex(this);

        OriginalData.Index = Index;
    }

    public void UpdateSearch() { }

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
        var data = new ItemElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        //itemElementData.SelectionStatus = SelectionStatus;

        base.Clone(data);

        return data;
    }
}
