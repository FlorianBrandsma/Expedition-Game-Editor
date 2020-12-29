using UnityEngine;
using System;

public class InteractableElementData : InteractableData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public InteractableData OriginalData            { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Interactable; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

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

    public bool ChangedScale
    {
        get { return !Mathf.Approximately(Scale, OriginalData.Scale); }
    }

    public bool ChangedHealth
    {
        get { return Health != OriginalData.Health; }
    }

    public bool ChangedHunger
    {
        get { return Hunger != OriginalData.Hunger; }
    }

    public bool ChangedThirst
    {
        get { return Thirst != OriginalData.Thirst; }
    }

    public bool ChangedWeight
    {
        get { return !Mathf.Approximately(Weight, OriginalData.Weight); }
    }

    public bool ChangedSpeed
    {
        get { return !Mathf.Approximately(Speed, OriginalData.Speed); }
    }

    public bool ChangedStamina
    {
        get { return !Mathf.Approximately(Stamina, OriginalData.Stamina); }
    }

    public bool Changed
    {
        get
        {
            return  ChangedModelId  || ChangedName      || ChangedScale     ||
                    ChangedHealth   || ChangedHunger    || ChangedThirst    ||
                    ChangedWeight   || ChangedSpeed     || ChangedStamina;
        }
    }
    #endregion

    public void Add(DataRequest dataRequest)
    {
        InteractableDataManager.AddData(this, dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            SetOriginalValues();
    }

    public void Update(DataRequest dataRequest)
    {
        if (!Changed) return;

        InteractableDataManager.UpdateData(this, dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            SetOriginalValues();
    }

    public void UpdateIndex()
    {
        if (!ChangedIndex) return;
        
        InteractableDataManager.UpdateIndex(this);

        OriginalData.Index = Index;
    }

    public void UpdateSearch() { }

    public void Remove(DataRequest dataRequest)
    {
        InteractableDataManager.RemoveData(this, dataRequest);
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
        var data = new InteractableElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
