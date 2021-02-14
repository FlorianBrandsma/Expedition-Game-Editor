using UnityEngine;
using System;

public class GameSaveElementData : GameSaveData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameSaveData OriginalData                { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameSave; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }
    
    #region Changed
    public bool Changed { get; set; }
    #endregion

    public GameSaveElementData() { }

    public void Add(DataRequest dataRequest) { }

    public void Update(DataRequest dataRequest)
    {
        SaveData.Update(dataRequest);

        ChapterSaveDataList.ForEach(x => x.Update(dataRequest));
        PhaseSaveDataList.ForEach(x => x.Update(dataRequest));
        QuestSaveDataList.ForEach(x => x.Update(dataRequest));
        ObjectiveSaveDataList.ForEach(x => x.Update(dataRequest));
        TaskSaveDataList.ForEach(x => x.Update(dataRequest));
        InteractionSaveDataList.ForEach(x => x.Update(dataRequest));
    }

    public void Remove(DataRequest dataRequest) { }

    public void SetOriginalValues()
    {
        OriginalData = base.Clone();

        SaveData.SetOriginalValues();

        ChapterSaveDataList.ForEach(x => x.SetOriginalValues());
        PhaseSaveDataList.ForEach(x => x.SetOriginalValues());
        QuestSaveDataList.ForEach(x => x.SetOriginalValues());
        ObjectiveSaveDataList.ForEach(x => x.SetOriginalValues());
        TaskSaveDataList.ForEach(x => x.SetOriginalValues());
        InteractionSaveDataList.ForEach(x => x.SetOriginalValues());

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
        var data = new GameSaveElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
