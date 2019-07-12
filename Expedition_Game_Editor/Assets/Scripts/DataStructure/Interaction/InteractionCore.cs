using UnityEngine;
using System.Linq;

public class InteractionCore : GeneralData
{
    private int terrainInteractableId;
    private int regionId;
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

    public int TerrainInteractableId
    {
        get { return terrainInteractableId; }
        set { terrainInteractableId = value; }
    }

    public int RegionId
    {
        get { return regionId; }
        set { regionId = value; }
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

        var interactionData = Fixtures.interactionList.Where(x => x.id == id).FirstOrDefault();

        if (changedDescription)
            interactionData.description = description;
        
        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        var interactionData = Fixtures.interactionList.Where(x => x.id == id).FirstOrDefault();

        if (changedIndex)
        {
            interactionData.index = index;

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
