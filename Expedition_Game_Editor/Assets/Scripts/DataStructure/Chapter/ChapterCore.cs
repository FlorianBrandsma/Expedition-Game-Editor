using UnityEngine;
using System.Linq;

public class ChapterCore : GeneralData
{
    private int interactableId;

    private string name;
    private string publicNotes;
    private string privateNotes;

    //Original
    public int originalInteractableId;

    public string originalName;
    public string originalPublicNotes;
    public string originalPrivateNotes;

    //Changed
    private bool changedInteractableId;

    private bool changedName;
    private bool changedPublicNotes;
    private bool changedPrivateNotes;

    public bool Changed
    {
        get
        {
            return changedInteractableId || changedName || changedPublicNotes || changedPrivateNotes;
        }
    }

    #region Properties
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

    public string PublicNotes
    {
        get { return publicNotes; }
        set
        {
            if (value == publicNotes) return;

            changedPublicNotes = (value != originalPublicNotes);

            publicNotes = value;
        }
    }

    public string PrivateNotes
    {
        get { return privateNotes; }
        set
        {
            if (value == privateNotes) return;

            changedPrivateNotes = (value != originalPrivateNotes);

            privateNotes = value;
        }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var chapterData = Fixtures.chapterList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedInteractableId)
            chapterData.interactableId = interactableId;

        if (changedName)
            chapterData.name = name;

        if (changedPublicNotes)
            chapterData.publicNotes = publicNotes;

        if (changedPrivateNotes)
            chapterData.privateNotes = privateNotes;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;
        
        var chapterData = Fixtures.chapterList.Where(x => x.Id == Id).FirstOrDefault();

        chapterData.Index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalInteractableId = interactableId;

        originalName = name;
        originalPublicNotes = publicNotes;
        originalPrivateNotes = privateNotes;
    }

    public void GetOriginalValues()
    {
        interactableId = originalInteractableId;

        name = originalName;
        publicNotes = originalPublicNotes;
        privateNotes = originalPrivateNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedInteractableId = false;
        changedName = false;
        changedPublicNotes = false;
        changedPrivateNotes = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var chapterDataSource = (ChapterDataElement)dataSource;

        interactableId = chapterDataSource.interactableId;

        name = chapterDataSource.name;
        publicNotes = chapterDataSource.publicNotes;
        privateNotes = chapterDataSource.privateNotes;
    }
}
