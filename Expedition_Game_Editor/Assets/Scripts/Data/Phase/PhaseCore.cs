using UnityEngine;
using System.Linq;

public class PhaseCore : GeneralData
{
    private int chapterId;

    private string name;

    private string publicNotes;
    private string privateNotes;

    //Original
    public string originalName;

    public string originalPublicNotes;
    public string originalPrivateNotes;

    //Changed
    private bool changedName;

    private bool changedPublicNotes;
    private bool changedPrivateNotes;

    public bool Changed
    {
        get
        {
            return changedName || changedPublicNotes || changedPrivateNotes;
        }
    }

    #region Properties
    public int ChapterId
    {
        get { return chapterId; }
        set { chapterId = value; }
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
    public virtual void Create() { }

    public virtual void Update()
    {
        var phaseData = Fixtures.phaseList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedName)
            phaseData.name = name;

        if (changedPublicNotes)
            phaseData.publicNotes = publicNotes;

        if (changedPrivateNotes)
            phaseData.privateNotes = privateNotes;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var phaseData = Fixtures.phaseList.Where(x => x.Id == Id).FirstOrDefault();

        phaseData.Index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalName = name;

        originalPublicNotes = publicNotes;
        originalPrivateNotes = privateNotes;
    }

    public void GetOriginalValues()
    {
        name = originalName;

        publicNotes = originalPublicNotes;
        privateNotes = originalPrivateNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedName = false;

        changedPublicNotes = false;
        changedPrivateNotes = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var phaseDataSource = (PhaseDataElement)dataSource;

        chapterId = phaseDataSource.chapterId;

        name = phaseDataSource.name;

        publicNotes = phaseDataSource.publicNotes;
        privateNotes = phaseDataSource.privateNotes;
    }
}
