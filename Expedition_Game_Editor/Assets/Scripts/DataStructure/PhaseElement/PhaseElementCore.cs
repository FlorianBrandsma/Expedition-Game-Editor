using UnityEngine;
using System.Collections;

public class PhaseElementCore : GeneralData
{
    private int phaseId;
    private int questId;
    private int terrainElementId;

    public int originalIndex;
    public int originalPhaseId;
    public int originalQuestId;
    public int originalTerrainElementId;

    public bool changed;
    private bool changedIndex;
    private bool changedPhaseId;
    private bool changedQuestId;
    private bool changedTerrainElementId;

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

            changedPhaseId = true;

            phaseId = value;
        }
    }

    public int QuestId
    {
        get { return questId; }
        set
        {
            if (value == questId) return;

            changedQuestId = true;

            questId = value;
        }
    }

    public int TerrainElementId
    {
        get { return terrainElementId; }
        set
        {
            if (value == terrainElementId) return;

            changedTerrainElementId = true;

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
        //if (!changed) return;

        //Debug.Log("Updated " + name);

        //if (changed_id)             return;
        //if (changed_table)          return;
        //if (changed_type)           return;
        //if (changed_index)          return;
        //if (changed_name)           return;
        //if (changed_description)    return;

        //SetOriginalValues();
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalPhaseId = phaseId;
        originalQuestId = questId;
        originalTerrainElementId = terrainElementId;

        //ClearChanges();
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

        changed = false;
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
