using UnityEngine;
using System;

public class PartyMemberElementData : PartyMemberData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public PartyMemberData OriginalData             { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.PartyMember; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedChapterId
    {
        get { return ChapterId != OriginalData.ChapterId; }
    }

    public bool ChangedInteractableId
    {
        get { return InteractableId != OriginalData.InteractableId; }
    }

    public bool Changed
    {
        get
        {
            return ChangedChapterId || ChangedInteractableId;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        PartyMemberDataManager.UpdateData(this);

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
        var data = new PartyMemberElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
