using UnityEngine;
using System;

public class WorldInteractableElementData : WorldInteractableData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public WorldInteractableData OriginalData       { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.WorldInteractable; } }

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

    public void Update()
    {
        if (!Changed) return;

        WorldInteractableDataManager.UpdateData(this);

        SetOriginalValues();
    }

    public void UpdateSearch()
    {
        if (!Changed) return;

        WorldInteractableDataManager.UpdateSearch(this);

        OriginalData.InteractableId = InteractableId;

        OriginalData.InteractableName = InteractableName;

        OriginalData.ModelPath = ModelPath;
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
        var data = new WorldInteractableElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
