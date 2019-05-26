using UnityEngine;
using System.Linq;

public class TerrainElementCore : GeneralData
{
    private int chapterId;
    private int elementId;

    public int originalChapterId;
    public int originalElementId;

    public bool changed;
    private bool changedChapterId;
    private bool changedElementId;

    #region Properties

    public int ChapterId
    {
        get { return chapterId; }
        set
        {
            if (value == chapterId) return;

            changed = true;
            changedChapterId = true;

            chapterId = value;
        }
    }

    public int ElementId
    {
        get { return elementId; }
        set
        {
            if (value == elementId) return;

            changed = true;
            changedElementId = true;

            elementId = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update()
    {
        var terrainElementData = Fixtures.terrainElementList.Where(x => x.id == id).FirstOrDefault();

        if (changedElementId)
            terrainElementData.elementId = elementId;
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalChapterId = ChapterId;
        originalElementId = ElementId;

        //ClearChanges();
    }

    public void GetOriginalValues()
    {
        ChapterId = originalChapterId;
        ElementId = originalElementId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changedChapterId = false;
        changedElementId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
