using UnityEngine;
using System;

public class SceneElementData : SceneData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public SceneData OriginalData                   { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Scene; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedRegionId
    {
        get { return RegionId != OriginalData.RegionId; }
    }

    public bool ChangedIndex
    {
        get { return Index != OriginalData.Index; }
    }

    public bool ChangedName
    {
        get { return Name != OriginalData.Name; }
    }

    public bool ChangedFreezeTime
    {
        get { return FreezeTime != OriginalData.FreezeTime; }
    }

    public bool ChangedFreezeMovement
    {
        get { return FreezeMovement != OriginalData.FreezeMovement; }
    }

    public bool ChangedAutoContinue
    {
        get { return AutoContinue != OriginalData.AutoContinue; }
    }

    public bool ChangedSceneDuration
    {
        get { return !Mathf.Approximately(SceneDuration, OriginalData.SceneDuration); }
    }

    public bool ChangedShotDuration
    {
        get { return !Mathf.Approximately(ShotDuration, OriginalData.ShotDuration); }
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
            return  ChangedRegionId         || ChangedName          || ChangedFreezeTime    || ChangedFreezeMovement || ChangedAutoContinue ||
                    ChangedSceneDuration    || ChangedShotDuration  || ChangedPublicNotes   || ChangedPrivateNotes;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        SceneDataManager.UpdateData(this);

        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        if (!ChangedIndex) return;

        SceneDataManager.UpdateIndex(this);

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
        var data = new SceneElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
