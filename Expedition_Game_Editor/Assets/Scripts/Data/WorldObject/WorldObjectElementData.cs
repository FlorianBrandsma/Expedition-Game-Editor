using UnityEngine;
using System;

public class WorldObjectElementData : WorldObjectData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public WorldObjectData OriginalData             { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.WorldObject; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedModelId
    {
        get { return ModelId != OriginalData.ModelId; }
    }

    public bool ChangedRegionId
    {
        get { return RegionId != OriginalData.RegionId; }
    }

    public bool ChangedTerrainId
    {
        get { return TerrainId != OriginalData.TerrainId; }
    }

    public bool ChangedTerrainTileId
    {
        get { return TerrainTileId != OriginalData.TerrainTileId; }
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

    public bool ChangedScale
    {
        get { return !Mathf.Approximately(Scale, OriginalData.Scale); }
    }

    public bool ChangedAnimation
    {
        get { return Animation != OriginalData.Animation; }
    }

    public bool Changed
    {
        get
        {
            return  ChangedRegionId     || ChangedTerrainId || ChangedTerrainTileId || 
                    ChangedPositionX    || ChangedPositionY || ChangedPositionZ     || 
                    ChangedRotationX    || ChangedRotationY || ChangedRotationZ     || 
                    ChangedScale        || ChangedAnimation;
        }
    }
    #endregion

    public void Update()
    {
        if (!Changed) return;

        WorldObjectDataManager.UpdateData(this);

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
        var data = new WorldObjectElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
