using UnityEngine;
using System.Linq;

public class ChapterRegionCore : GeneralData
{
    private int chapterId;
    private int regionId;

    //Original
    public int originalRegionId;

    //Changed
    private bool changedRegionId;

    public bool Changed
    {
        get
        {
            return changedRegionId;
        }
    }

    #region Properties
    public int ChapterId
    {
        get { return chapterId; }
        set { chapterId = value; }
    }

    public int RegionId
    {
        get { return regionId; }
        set
        {
            if (value == regionId) return;

            changedRegionId = (value != originalRegionId);

            regionId = value;
        }
    }
    #endregion

    #region Methods

    public void Create() { }

    public virtual void Update()
    {
        var regionData = Fixtures.chapterRegionList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedRegionId)
            regionData.regionId = regionId;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var regionData = Fixtures.chapterList.Where(x => x.Id == Id).FirstOrDefault();

        regionData.Index = Index;

        changedIndex = false; 
    }

    public virtual void SetOriginalValues()
    {
        originalRegionId = regionId;
    }

    public void GetOriginalValues()
    {
        regionId = originalRegionId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedRegionId = false;
    }

    public void Delete() { }

    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var chapterRegionDataSource = (ChapterRegionDataElement)dataSource;

        chapterId = chapterRegionDataSource.chapterId;
        regionId = chapterRegionDataSource.regionId;
    }
}
