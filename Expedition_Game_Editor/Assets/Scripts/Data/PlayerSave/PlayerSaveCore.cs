using UnityEngine;
using System.Linq;

public class PlayerSaveCore : GeneralData
{
    private int saveId;
    private int regionId;
    private int partyMemberId;
    
    private float positionX;
    private float positionY;
    private float positionZ;

    //Recorded in seconds
    private int gameTime;
    private int playedTime;

    //Original
    public int originalRegionId;
    public int originalPartyMemberId;

    private int originalPlayedSeconds;

    //Changed
    private bool changedRegionId;
    private bool changedPartyMemberId;

    private bool changedPlayedSeconds;
    
    public bool Changed
    {
        get
        {
            return  changedRegionId || changedPartyMemberId;
        }
    }

    #region Properties
    public int SaveId
    {
        get { return saveId; }
        set { saveId = value; }
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

    public int PartyMemberId
    {
        get { return partyMemberId; }
        set
        {
            if (value == partyMemberId) return;

            changedPartyMemberId = (value != originalPartyMemberId);

            partyMemberId = value;
        }
    }
    
    public float PositionX
    {
        get { return positionX; }
        set { positionX = value; }
    }

    public float PositionY
    {
        get { return positionY; }
        set { positionY = value; }
    }

    public float PositionZ
    {
        get { return positionZ; }
        set { positionZ = value; }
    }

    public int GameTime
    {
        get { return gameTime; }
        set
        {
            if (value == gameTime) return;

            gameTime = value;
        }
    }

    public int PlayedTime
    {
        get { return playedTime; }
        set
        {
            if (value == playedTime) return;

            playedTime = value;
        }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var playerSaveData = Fixtures.playerSaveList.Where(x => x.id == Id).FirstOrDefault();

        playerSaveData.positionX = positionX;
        playerSaveData.positionY = positionY;
        playerSaveData.positionZ = positionZ;

        playerSaveData.gameTime = gameTime;
        playerSaveData.playedTime = playedTime;

        if (!Changed) return;

        if (changedRegionId)
            playerSaveData.regionId = regionId;
    }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalRegionId = regionId;
        originalPartyMemberId = partyMemberId;

        originalPlayedSeconds = playedTime;
    }

    public void GetOriginalValues()
    {
        regionId = originalRegionId;
        partyMemberId = originalPartyMemberId;

        playedTime = originalPlayedSeconds;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedRegionId = false;
        changedPartyMemberId = false;

        changedPlayedSeconds = false;
    }

    public void Delete() { }

    public void CloneCore(PlayerSaveElementData elementData)
    {
        CloneGeneralData(elementData);
    }
    #endregion

    new public virtual void Copy(IElementData dataSource) { }
}
