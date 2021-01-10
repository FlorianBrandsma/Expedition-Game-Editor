using UnityEngine;
using System;

public class InteractionDestinationElementData : InteractionDestinationData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public InteractionDestinationData OriginalData  { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.InteractionDestination; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
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

    public bool ChangedPositionVariance
    {
        get { return !Mathf.Approximately(PositionVariance, OriginalData.PositionVariance); }
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

    public bool ChangedChangeRotation
    {
        get { return ChangeRotation != OriginalData.ChangeRotation; }
    }

    public bool ChangedAnimation
    {
        get { return Animation != OriginalData.Animation; }
    }

    public bool ChangedPatience
    {
        get { return !Mathf.Approximately(Patience, OriginalData.Patience); }
    }

    public bool Changed
    {
        get
        {
            return  ChangedRegionId     || ChangedTerrainId || ChangedTerrainTileId ||
                    ChangedPositionX    || ChangedPositionY || ChangedPositionZ     || ChangedPositionVariance  ||
                    ChangedRotationX    || ChangedRotationY || ChangedRotationZ     || ChangedChangeRotation    ||
                    ChangedAnimation    || ChangedPatience;
        }
    }
    #endregion

    public InteractionDestinationElementData() { }

    public void Add(DataRequest dataRequest)
    {
        InteractionDestinationDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        InteractionDestinationDataManager.UpdateData(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        InteractionDestinationDataManager.RemoveData(this, dataRequest);
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
        var data = new InteractionDestinationElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
