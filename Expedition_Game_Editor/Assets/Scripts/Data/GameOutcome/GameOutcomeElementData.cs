using UnityEngine;
using System;

public class GameOutcomeElementData : GameOutcomeData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameOutcomeData OriginalData             { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameOutcome; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public float CurrentSceneDuration { get; set; }

    private int sceneIndex = -1;

    public int SceneIndex
    {
        get { return sceneIndex; }
        set
        {
            sceneIndex = value;

            if (sceneIndex < 0) return;

            CurrentSceneDuration = ActiveScene.SceneDuration;
        }
    }

    public GameSceneElementData ActiveScene
    {
        get { return SceneDataList[SceneIndex]; }
    }

    #region Changed
    public bool Changed { get { return false; } }
    #endregion

    public void Add(DataRequest dataRequest) { }

    public void Update(DataRequest dataRequest) { }

    public void UpdateSearch() { }

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
        var data = new GameOutcomeElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
