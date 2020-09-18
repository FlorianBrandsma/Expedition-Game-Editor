using UnityEngine;
using System;

public class PhaseElementData : PhaseData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public PhaseData OriginalData                   { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Phase; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedIndex
    {
        get { return Index != OriginalData.Index; }
    }

    public bool ChangedName
    {
        get { return Name != OriginalData.Name; }
    }

    public bool ChangedDefaultRegionId
    {
        get { return DefaultRegionId != OriginalData.DefaultRegionId; }
    }

    public bool ChangedDefaultPositionX
    {
        get { return !Mathf.Approximately(DefaultPositionX, OriginalData.DefaultPositionX); }
    }

    public bool ChangedDefaultPositionY
    {
        get { return !Mathf.Approximately(DefaultPositionY, OriginalData.DefaultPositionY); }
    }

    public bool ChangedDefaultPositionZ
    {
        get { return !Mathf.Approximately(DefaultPositionZ, OriginalData.DefaultPositionZ); }
    }

    public bool ChangedDefaultRotationX
    {
        get { return DefaultRotationX != OriginalData.DefaultRotationX; }
    }

    public bool ChangedDefaultRotationY
    {
        get { return DefaultRotationY != OriginalData.DefaultRotationY; }
    }

    public bool ChangedDefaultRotationZ
    {
        get { return DefaultRotationZ != OriginalData.DefaultRotationZ; }
    }

    public bool ChangedDefaultTime
    {
        get { return DefaultTime != OriginalData.DefaultTime; }
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
            return  ChangedName             || ChangedDefaultRegionId   ||
                    ChangedDefaultPositionX || ChangedDefaultPositionY  || ChangedDefaultPositionZ ||
                    ChangedDefaultRotationX || ChangedDefaultRotationY  || ChangedDefaultRotationZ ||
                    ChangedDefaultTime      || ChangedPublicNotes       || ChangedPrivateNotes;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        PhaseDataManager.UpdateData(this);

        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        if (!ChangedIndex) return;

        PhaseDataManager.UpdateIndex(this);

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
        var data = new PhaseElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
