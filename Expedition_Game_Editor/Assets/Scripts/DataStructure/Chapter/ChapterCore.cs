using UnityEngine;
using System.Linq;

public class ChapterCore : GeneralData
{
    private int elementId;
    private string name;
    private string notes;

    public int originalIndex;
    public int originalElementId;
    public string originalName;
    public string originalNotes;

    public bool changed;
    private bool changedIndex;
    private bool changedElementId;
    private bool changedName;
    private bool changedNotes;

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

    public string Name
    {
        get { return name; }
        set
        {
            if (value == name) return;

            changed = true;
            changedName = true;

            name = value;
        }
    }

    public string Notes
    {
        get { return notes; }
        set
        {
            if (value == notes) return;

            changed = true;
            changedNotes = true;

            notes = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {
        
    }

    public virtual void Update()
    {
        var chapterData = Fixtures.chapterList.Where(x => x.id == id).FirstOrDefault();

        if (changedElementId)
            chapterData.elementId = elementId;

        if (changedName)
            chapterData.name = name;

        if (changedNotes)
            chapterData.notes = notes;
    }

    public void UpdateIndex()
    {
        var chapterData = Fixtures.chapterList.Where(x => x.id == id).FirstOrDefault();

        if (changedIndex)
        {
            chapterData.index = index;

            changedIndex = false;
        }
    }

    public virtual void SetOriginalValues()
    {
        originalElementId = elementId;
        originalName = name;
        originalNotes = notes;
    }

    public void GetOriginalValues()
    {
        elementId = originalElementId;
        name = originalName;
        notes = originalNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changed = false;
        changedIndex = false;
        changedElementId = false;
        changedName = false;
        changedNotes = false;
    }

    public void Delete()
    {

    }

    #endregion
}
