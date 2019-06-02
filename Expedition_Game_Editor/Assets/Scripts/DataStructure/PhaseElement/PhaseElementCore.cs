using UnityEngine;
using System.Linq;

public class PhaseElementCore : GeneralData
{
    private int phaseId;
    private int questId;
    private int terrainElementId;

    public int originalIndex;
    public int originalPhaseId;
    public int originalQuestId;
    public int originalTerrainElementId;

    private bool changedIndex;
    private bool changedPhaseId;
    private bool changedQuestId;
    private bool changedTerrainElementId;

    public bool Changed
    {
        get
        {
            return changedPhaseId || changedQuestId || changedTerrainElementId;
        }
    }

    #region Properties

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

    public int TerrainElementId
    {
        get { return terrainElementId; }
        set
        {
            if (value == terrainElementId) return;

            changedTerrainElementId = (value != originalTerrainElementId);

            terrainElementId = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update()
    {
        var phaseElementData = Fixtures.phaseElementList.Where(x => x.id == id).FirstOrDefault();

        if (changedQuestId)
        {
            phaseElementData.questId = questId;

            Fixtures.taskList.Where(x => x.terrainElementId == phaseElementData.terrainElementId).Distinct().ToList().ForEach(x => Fixtures.taskList.Remove(x));
        }
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalPhaseId = phaseId;
        originalQuestId = questId;
        originalTerrainElementId = terrainElementId;
    }

    public void GetOriginalValues()
    {
        phaseId = originalPhaseId;
        questId = originalQuestId;
        terrainElementId = originalTerrainElementId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedIndex = false;
        changedPhaseId = false;
        changedQuestId = false;
        changedTerrainElementId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
