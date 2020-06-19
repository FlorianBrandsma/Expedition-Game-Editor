using UnityEngine;
using System.Linq;

public class AtmosphereCore : GeneralData
{
    private int terrainId;

    private bool isDefault;

    private int startTime;
    private int endTime;

    private string publicNotes;
    private string privateNotes;

    //Original
    public int originalStartTime;
    public int originalEndTime;

    public string originalPublicNotes;
    public string originalPrivateNotes;

    //Changed
    public bool changedStartTime;
    public bool changedEndTime;

    private bool changedPublicNotes;
    private bool changedPrivateNotes;

    public bool Changed
    {
        get
        {
            return changedStartTime || changedEndTime || changedPublicNotes || changedPrivateNotes;
        }
    }

    #region Properties
    public int TerrainId
    {
        get { return terrainId; }
        set { terrainId = value; }
    }

    public bool Default
    {
        get { return isDefault; }
        set { isDefault = value; }
    }

    public int StartTime
    {
        get { return startTime; }
        set
        {
            if (value == startTime) return;

            changedStartTime = (value != originalStartTime);

            startTime = value;
        }
    }

    public int EndTime
    {
        get { return endTime; }
        set
        {
            if (value == endTime) return;

            changedEndTime = (value != originalEndTime);

            endTime = value;
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
        var atmosphereData = Fixtures.atmosphereList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedStartTime)
            atmosphereData.startTime = startTime;

        if (changedEndTime)
            atmosphereData.endTime = endTime;

        if (changedPublicNotes)
            atmosphereData.publicNotes = publicNotes;

        if (changedPrivateNotes)
            atmosphereData.privateNotes = privateNotes;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var interactionData = Fixtures.interactionList.Where(x => x.Id == Id).FirstOrDefault();

        interactionData.Index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalStartTime = startTime;
        originalEndTime = endTime;

        originalPublicNotes = publicNotes;
        originalPrivateNotes = privateNotes;
    }

    public void GetOriginalValues()
    {
        startTime = originalStartTime;
        endTime = originalEndTime;

        publicNotes = originalPublicNotes;
        privateNotes = originalPrivateNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedStartTime = false;
        changedEndTime = false;

        changedPublicNotes = false;
        changedPrivateNotes = false;
    }

    public void Delete() { }

    public void CloneCore(AtmosphereDataElement dataElement)
    {
        CloneGeneralData(dataElement);

        dataElement.terrainId = terrainId;
        
        dataElement.isDefault = isDefault;

        dataElement.startTime = startTime;
        dataElement.endTime = endTime;

        dataElement.publicNotes = publicNotes;
        dataElement.privateNotes = privateNotes;


        dataElement.originalStartTime = originalStartTime;
        dataElement.originalEndTime = originalEndTime;

        dataElement.originalPublicNotes = originalPublicNotes;
        dataElement.originalPrivateNotes = originalPrivateNotes;
    }
    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var atmosphereDataSource = (AtmosphereDataElement)dataSource;

        terrainId = atmosphereDataSource.terrainId;

        isDefault = atmosphereDataSource.isDefault;

        startTime = atmosphereDataSource.startTime;
        endTime = atmosphereDataSource.endTime;

        publicNotes = atmosphereDataSource.publicNotes;
        privateNotes = atmosphereDataSource.privateNotes;
    }
}
