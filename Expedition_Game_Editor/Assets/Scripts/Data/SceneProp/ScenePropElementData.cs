﻿using UnityEngine;
using System;

public class ScenePropElementData : ScenePropData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public ScenePropData OriginalData               { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.SceneProp; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedModelId
    {
        get { return ModelId != OriginalData.ModelId; }
    }

    public bool ChangedPositionX
    {
        get { return !Mathf.Approximately(PositionX, OriginalData.PositionX); }
    }

    public bool ChangedPositionY
    {
        get { return !Mathf.Approximately(PositionY, OriginalData.PositionY); }
    }

    public bool ChangedPositionZ
    {
        get { return !Mathf.Approximately(PositionZ, OriginalData.PositionZ); }
    }

    public bool ChangedRotationX
    {
        get { return RotationX != OriginalData.RotationX; }
    }

    public bool ChangedRotationY
    {
        get { return RotationY != OriginalData.RotationY; }
    }

    public bool ChangedRotationZ
    {
        get { return RotationZ != OriginalData.RotationZ; }
    }
    
    public bool Changed
    {
        get
        {
            return  ChangedModelId      || 
                    ChangedPositionX    || ChangedPositionY || ChangedPositionZ ||
                    ChangedRotationX    || ChangedRotationY || ChangedRotationZ;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        ScenePropDataManager.UpdateData(this);

        SetOriginalValues();
    }

    public void UpdateSearch()
    {
        if (!Changed) return;

        ScenePropDataManager.UpdateSearch(this);

        OriginalData.ModelId = ModelId;

        OriginalData.ModelName = ModelName;
        OriginalData.ModelIconPath = ModelIconPath;
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
        var data = new ScenePropElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
