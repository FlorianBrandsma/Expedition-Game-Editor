using UnityEngine;
using System;

public class WorldInteractableElementData : WorldInteractableData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public WorldInteractableData OriginalData       { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.WorldInteractable; } }
    
    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }
    
    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedInteractableId
    {
        get { return InteractableId != OriginalData.InteractableId; }
    }

    public bool ChangedQuestId
    {
        get { return QuestId != OriginalData.QuestId; }
    }

    public bool Changed
    {
        get
        {
            return ChangedInteractableId || ChangedQuestId;
        }
    }
    #endregion

    public WorldInteractableElementData() { }

    public void Add(DataRequest dataRequest)
    {
        WorldInteractableDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        WorldInteractableDataManager.UpdateData(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        WorldInteractableDataManager.RemoveData(this, dataRequest);
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
        var data = new WorldInteractableElementData();
        
        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
