using UnityEngine;
using System.Linq;

public class TaskCore : GeneralData
{
    private int terrainElementId;
    private string description;

    public int originalIndex;
    public string originalDescription;

    private bool changedIndex;
    private bool changedDescription;

    public bool Changed
    {
        get
        {
            return changedDescription;
        }
    }

    #region Properties

    public int TerrainElementId
    {
        get { return terrainElementId; }
        set { terrainElementId = value; }
    }

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

    public string Description
    {
        get { return description; }
        set
        {
            if (value == description) return;

            changedDescription = (value != originalDescription);

            description = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public void Update()
    {
        if (!Changed) return;

        var taskData = Fixtures.taskList.Where(x => x.id == id).FirstOrDefault();

        if (changedDescription)
            taskData.description = description;
        
        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        var taskData = Fixtures.taskList.Where(x => x.id == id).FirstOrDefault();

        if (changedIndex)
        {
            taskData.index = index;

            changedIndex = false;
        }
    }

    public void SetOriginalValues()
    {
        originalDescription = description;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        description = originalDescription;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changedIndex = false;
        changedDescription = false;
    }

    public void Delete()
    {

    }

    #endregion
}
