using UnityEngine;
using System.Linq;

public class PhaseInteractableCore : GeneralData
{
    private int phaseId;
    private int questId;
    private int worldInteractableId;

    //Original
    public int originalPhaseId;
    public int originalQuestId;
    public int originalWorldInteractableId;

    //Changed
    private bool changedPhaseId;
    private bool changedQuestId;
    private bool changedWorldInteractableId;

    public bool Changed
    {
        get
        {
            return changedPhaseId || changedQuestId || changedWorldInteractableId;
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

    public int WorldInteractableId
    {
        get { return worldInteractableId; }
        set
        {
            if (value == worldInteractableId) return;

            changedWorldInteractableId = (value != originalWorldInteractableId);

            worldInteractableId = value;
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

            var taskList = Fixtures.taskList.Where(x => x.worldInteractableId == phaseElementData.worldInteractableId).Distinct().ToList();

            Fixtures.interactionList.Where(x => taskList.Select(y => y.Id).Contains(x.taskId)).Distinct().ToList().ForEach(x => Fixtures.interactionList.Remove(x));
            Fixtures.taskList.Where(x => taskList.Select(y => y.Id).Contains(x.Id)).Distinct().ToList().ForEach(x => Fixtures.taskList.Remove(x));
        }
    }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalPhaseId = phaseId;
        originalQuestId = questId;
        originalWorldInteractableId = worldInteractableId;
    }

    public void GetOriginalValues()
    {
        phaseId = originalPhaseId;
        questId = originalQuestId;
        worldInteractableId = originalWorldInteractableId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedPhaseId = false;
        changedQuestId = false;
        changedWorldInteractableId = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var phaseInteractableDataSource = (PhaseInteractableDataElement)dataSource;

        phaseId = phaseInteractableDataSource.phaseId;
        questId = phaseInteractableDataSource.questId;
        worldInteractableId = phaseInteractableDataSource.worldInteractableId;
    }
}
