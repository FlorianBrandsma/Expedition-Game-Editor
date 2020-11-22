using UnityEngine;
using System;

public class AtmosphereElementData : AtmosphereData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public AtmosphereData OriginalData              { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Atmosphere; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedStartTime
    {
        get { return StartTime != OriginalData.StartTime; }
    }

    public bool ChangedEndTime
    {
        get { return EndTime != OriginalData.EndTime; }
    }

    public bool ChangedPublicNotes
    {
        get { return PublicNotes != OriginalData.PublicNotes; }
    }

    public bool ChangedPrivateNotes
    {
        get { return PrivateNotes != OriginalData.PrivateNotes; }
    }

    public bool Changed
    {
        get
        {
            return ChangedStartTime || ChangedEndTime || ChangedPublicNotes || ChangedPrivateNotes;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        AtmosphereDataManager.UpdateData(this);

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
        var data = new AtmosphereElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
