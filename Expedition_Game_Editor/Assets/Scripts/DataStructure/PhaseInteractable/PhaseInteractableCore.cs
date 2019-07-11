using UnityEngine;
using System.Linq;

public class PhaseInteractableCore : GeneralData
{
    private int phaseId;
    private int questId;
    private int terrainInteractableId;

    public int originalIndex;
    public int originalPhaseId;
    public int originalQuestId;
    public int originalTerrainInteractableId;

    private bool changedIndex;
    private bool changedPhaseId;
    private bool changedQuestId;
    private bool changedTerrainInteractableId;

    public bool Changed
    {
        get
        {
            return changedPhaseId || changedQuestId || changedTerrainInteractableId;
        }
    }

    #region Properties

    public int Id { get { return id; } }

    public int Index
    {
        get { return index; }
        set
        {
            if (value == index) return;

            changedIndex = true;

            index = value;
        }
    }

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

    public int TerrainInteractableId
    {
        get { return terrainInteractableId; }
        set
        {
            if (value == terrainInteractableId) return;

            changedTerrainInteractableId = (value != originalTerrainInteractableId);

            terrainInteractableId = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update()
    {
        var phaseElementData = Fixtures.phaseInteractableList.Where(x => x.id == id).FirstOrDefault();

        if (changedQuestId)
        {
            phaseElementData.questId = questId;

            Fixtures.interactionList.Where(x => x.terrainInteractableId == phaseElementData.terrainInteractableId).Distinct().ToList().ForEach(x => Fixtures.interactionList.Remove(x));
        }
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalPhaseId = phaseId;
        originalQuestId = questId;
        originalTerrainInteractableId = terrainInteractableId;
    }

    public void GetOriginalValues()
    {
        phaseId = originalPhaseId;
        questId = originalQuestId;
        terrainInteractableId = originalTerrainInteractableId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedIndex = false;
        changedPhaseId = false;
        changedQuestId = false;
        changedTerrainInteractableId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
