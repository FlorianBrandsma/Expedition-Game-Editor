using UnityEngine;
using System;
using System.Linq;

public class GameSceneActorElementData : GameSceneActorData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameSceneActorData OriginalData          { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameSceneActor; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public GameWorldInteractableElementData WorldInteractable
    {
        get { return GameManager.instance.gameWorldData.WorldInteractableDataList.Where(x => x.Id == WorldInteractableId).FirstOrDefault(); }
    }

    public GameWorldInteractableElementData TargetWorldInteractable
    {
        get { return GameManager.instance.gameWorldData.WorldInteractableDataList.Where(x => x.Id == TargetWorldInteractableId).FirstOrDefault(); }
    }

    #region Changed
    public bool Changed { get { return false; } }
    #endregion

    public void Add(DataRequest dataRequest) { }

    public void Update(DataRequest dataRequest) { }

    public void Remove(DataRequest dataRequest) { }

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
        var data = new GameSceneActorElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
