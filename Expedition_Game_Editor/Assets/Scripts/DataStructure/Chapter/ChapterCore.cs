using UnityEngine;
using System.Linq;

public class ChapterCore : GeneralData
{
    private int interactableId;
    private string name;
    private string notes;

    public int originalIndex;
    public int originalInteractableId;
    public string originalName;
    public string originalNotes;

    private bool changedIndex;
    private bool changedInteractableId;
    private bool changedName;
    private bool changedNotes;

    public bool Changed
    {
        get
        {
            return changedInteractableId || changedName || changedNotes;
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

    public int InteractableId
    {
        get { return interactableId; }
        set
        {
            if (value == interactableId) return;

            changedInteractableId = (value != originalInteractableId);

            interactableId = value;
        }
    }

    public string Name
    {
        get { return name; }
        set
        {
            if (value == name) return;

            changedName = (value != originalName);

            name = value;
        }
    }

    public string Notes
    {
        get { return notes; }
        set
        {
            if (value == notes) return;

            changedNotes = (value != originalNotes);

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

        if (changedInteractableId)
            chapterData.interactableId = interactableId;

        if (changedName)
            chapterData.name = name;

        if (changedNotes)
            chapterData.notes = notes;
    }

    public void UpdateSearch() { }

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
        originalInteractableId = interactableId;
        originalName = name;
        originalNotes = notes;
    }

    public void GetOriginalValues()
    {
        interactableId = originalInteractableId;
        name = originalName;
        notes = originalNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedIndex = false;
        changedInteractableId = false;
        changedName = false;
        changedNotes = false;
    }

    public void Delete()
    {

    }

    #endregion
}
