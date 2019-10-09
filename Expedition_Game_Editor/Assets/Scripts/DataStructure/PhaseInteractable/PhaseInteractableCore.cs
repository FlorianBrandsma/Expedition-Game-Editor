using UnityEngine;
using System.Linq;

public class PhaseInteractableCore : GeneralData
{
    private int phaseId;
    private int questId;
    private int sceneInteractableId;

    public int originalPhaseId;
    public int originalQuestId;
    public int originalSceneInteractableId;

    private bool changedPhaseId;
    private bool changedQuestId;
    private bool changedSceneInteractableId;

    public bool Changed
    {
        get
        {
            return changedPhaseId || changedQuestId || changedSceneInteractableId;
        }
    }

    #region Properties

    public int PhaseId
    {
        get { return phaseId; }
        set
        {
            if (value == phaseId) return;

            changedPhaseId = (value != originalPhaseId);

            phaseId = value;
        }
    }

    public int QuestId
    {
        get { return questId; }
        set
        {
            if (value == questId) return;

            changedQuestId = (value != originalQuestId);

            questId = value;
        }
    }

    public int SceneInteractableId
    {
        get { return sceneInteractableId; }
        set
        {
            if (value == sceneInteractableId) return;

            changedSceneInteractableId = (value != originalSceneInteractableId);

            sceneInteractableId = value;
        }
    }

    #endregion

    #region Methods

    public void Create() { }

    public virtual void Update()
    {
        var phaseElementData = Fixtures.phaseInteractableList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedQuestId)
        {
            phaseElementData.questId = questId;

            Fixtures.interactionList.Where(x => x.sceneInteractableId == phaseElementData.sceneInteractableId).Distinct().ToList().ForEach(x => Fixtures.interactionList.Remove(x));
        }
    }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalPhaseId = phaseId;
        originalQuestId = questId;
        originalSceneInteractableId = sceneInteractableId;
    }

    public void GetOriginalValues()
    {
        phaseId = originalPhaseId;
        questId = originalQuestId;
        sceneInteractableId = originalSceneInteractableId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedPhaseId = false;
        changedQuestId = false;
        changedSceneInteractableId = false;
    }

    public void Delete() { }

    #endregion
}
