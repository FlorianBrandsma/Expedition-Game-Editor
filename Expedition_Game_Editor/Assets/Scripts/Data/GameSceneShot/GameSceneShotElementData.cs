using UnityEngine;
using System;
using System.Linq;

public class GameSceneShotElementData : GameSceneShotData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameSceneShotData OriginalData          { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameSceneShot; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public GameWorldInteractableElementData PositionTargetWorldInteractable
    {
        get { return GameManager.instance.gameWorldData.WorldInteractableDataList.Where(x => x.Id == PositionTargetWorldInteractableId).FirstOrDefault(); }
    }

    public GameWorldInteractableElementData RotationTargetWorldInteractable
    {
        get { return GameManager.instance.gameWorldData.WorldInteractableDataList.Where(x => x.Id == RotationTargetWorldInteractableId).FirstOrDefault(); }
    }

    #region Changed
    public bool Changed { get { return false; } }
    #endregion

    public void Update() { }

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
        var data = new GameSceneShotElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
