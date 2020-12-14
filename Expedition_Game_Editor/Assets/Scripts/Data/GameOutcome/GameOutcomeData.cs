using UnityEngine;
using System.Collections.Generic;

public class GameOutcomeData
{
    public int Id                           { get; set; }

    public int Type                         { get; set; }

    public bool CompleteTask                { get; set; }
    public bool ResetObjective              { get; set; }

    public int CancelScenarioType           { get; set; }
    public bool CancelScenarioOnInteraction { get; set; }
    public bool CancelScenarioOnInput       { get; set; }
    public bool CancelScenarioOnRange       { get; set; }
    public bool CancelScenarioOnHit         { get; set; }

    public List<GameSceneElementData> SceneDataList { get; set; } = new List<GameSceneElementData>();

    public virtual void GetOriginalValues(GameOutcomeData originalData)
    {
        Id                          = originalData.Id;

        Type                        = originalData.Type;

        CompleteTask                = originalData.CompleteTask;
        ResetObjective              = originalData.ResetObjective;

        CancelScenarioType          = originalData.CancelScenarioType;
        CancelScenarioOnInteraction = originalData.CancelScenarioOnInteraction;
        CancelScenarioOnInput       = originalData.CancelScenarioOnInput;
        CancelScenarioOnRange       = originalData.CancelScenarioOnRange;
        CancelScenarioOnHit         = originalData.CancelScenarioOnHit;
    }

    public GameOutcomeData Clone()
    {
        var data = new GameOutcomeData();
        
        data.Id                             = Id;

        data.Type                           = Type;

        data.CompleteTask                   = CompleteTask;
        data.ResetObjective                 = ResetObjective;

        data.CancelScenarioType             = CancelScenarioType;
        data.CancelScenarioOnInteraction    = CancelScenarioOnInteraction;
        data.CancelScenarioOnInput          = CancelScenarioOnInput;
        data.CancelScenarioOnRange          = CancelScenarioOnRange;
        data.CancelScenarioOnHit            = CancelScenarioOnHit;

        SceneDataList.ForEach(x => x.SetOriginalValues());

        return data;
    }

    public virtual void Clone(GameOutcomeElementData elementData)
    {
        elementData.Id                          = Id;

        elementData.Type                        = Type;

        elementData.CompleteTask                = CompleteTask;
        elementData.ResetObjective              = ResetObjective;

        elementData.CancelScenarioType          = CancelScenarioType;
        elementData.CancelScenarioOnInteraction = CancelScenarioOnInteraction;
        elementData.CancelScenarioOnInput       = CancelScenarioOnInput;
        elementData.CancelScenarioOnRange       = CancelScenarioOnRange;
        elementData.CancelScenarioOnHit         = CancelScenarioOnHit;
    }
}
