using UnityEngine;
using System;

public class ChapterInteractableElementData : ChapterInteractableData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public ChapterInteractableData OriginalData     { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.ChapterInteractable; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedInteractableId
    {
        get { return InteractableId != OriginalData.InteractableId; }
    }

    public bool Changed
    {
        get
        {
            return ChangedInteractableId;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        ChapterInteractableDataManager.UpdateData(this);

        SetOriginalValues();
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
        var data = new ChapterInteractableElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();
        
        base.Clone(data);

        return data;
    }
}
