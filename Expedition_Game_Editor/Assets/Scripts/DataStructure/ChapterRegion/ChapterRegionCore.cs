using UnityEngine;
using System.Linq;

public class ChapterRegionCore : GeneralData
{
    private int chapterId;
    private int regionId;

    public int originalIndex;
    public int originalRegionId;

    private bool changedIndex;
    private bool changedRegionId;

    public bool Changed
    {
        get
        {
            return changedRegionId;
        }
    }

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

    public void Create()
    {

    }

    public void Update()
    {
        if (!Changed) return;

        var regionData = Fixtures.chapterRegionList.Where(x => x.id == id).FirstOrDefault();

        if (changedRegionId)
            regionData.regionId = regionId;

        SetOriginalValues();
    }

    public void UpdateIndex()
    {
        var regionData = Fixtures.chapterList.Where(x => x.id == id).FirstOrDefault();

        if (changedIndex)
        {
            regionData.index = index;

            changedIndex = false;
        }
    }

    public void SetOriginalValues()
    {
        originalRegionId = regionId;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        regionId = originalRegionId;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changedIndex = false;
        changedRegionId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
