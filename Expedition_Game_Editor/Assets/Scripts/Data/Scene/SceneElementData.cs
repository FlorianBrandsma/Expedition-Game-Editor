using UnityEngine;
using System;

public class SceneElementData : SceneData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public SceneData OriginalData                   { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Scene; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

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

    public bool ChangedAutoContinue
    {
        get { return AutoContinue != OriginalData.AutoContinue; }
    }

    public bool ChangedSetActorsInstantly
    {
        get { return SetActorsInstantly != OriginalData.SetActorsInstantly; }
    }

    public bool ChangedSceneDuration
    {
        get { return !Mathf.Approximately(SceneDuration, OriginalData.SceneDuration); }
    }

    public bool ChangedShotDuration
    {
        get { return !Mathf.Approximately(ShotDuration, OriginalData.ShotDuration); }
    }

    public bool ChangedEditorNotes
    {
        get { return EditorNotes != OriginalData.EditorNotes; }
    }

    public bool ChangedGameNotes
    {
        get { return GameNotes != OriginalData.GameNotes; }
    }

    public bool Changed
    {
        get
        {
            return  ChangedRegionId         || ChangedName          || ChangedFreezeTime    || ChangedAutoContinue || ChangedSetActorsInstantly ||
                    ChangedSceneDuration    || ChangedShotDuration  || ChangedEditorNotes   || ChangedGameNotes;
        }
    }
    #endregion

    public SceneElementData() { }

    public void Add(DataRequest dataRequest)
    {
        SceneDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        SceneDataManager.UpdateData(this, dataRequest);
    }

    public void UpdateIndex(DataRequest dataRequest)
    {
        SceneDataManager.UpdateIndex(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        SceneDataManager.RemoveData(this, dataRequest);
    }

    public void RemoveIndex(DataRequest dataRequest)
    {
        SceneDataManager.RemoveIndex(this, dataRequest);
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
        var data = new SceneElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
