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

    private int rotationX;
    private int rotationY;
    private int rotationZ;

    private float scaleMultiplier;
    
    private int playedSeconds;

    //Original
    public int originalRegionId;
    public int originalPartyMemberId;
    
    public float originalPositionX;
    public float originalPositionY;
    public float originalPositionZ;

    public int originalRotationX;
    public int originalRotationY;
    public int originalRotationZ;

    public float originalScaleMultiplier;

    private int originalPlayedSeconds;

    //Changed
    private bool changedRegionId;
    private bool changedPartyMemberId;
    
    private bool changedPositionX;
    private bool changedPositionY;
    private bool changedPositionZ;

    private bool changedRotationX;
    private bool changedRotationY;
    private bool changedRotationZ;

    private bool changedScaleMultiplier;

    private bool changedPlayedSeconds;
    
    public bool Changed
    {
        get
        {
            return  changedRegionId         || changedPartyMemberId || 
                    changedPositionX        || changedPositionY     || changedPositionZ     ||
                    changedRotationX        || changedRotationY     || changedRotationZ     ||
                    changedScaleMultiplier  || changedPlayedSeconds;
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
        set
        {
            if (value == positionX) return;

            changedPositionX = (value != originalPositionX);

            positionX = value;
        }
    }

    public float PositionY
    {
        get { return positionY; }
        set
        {
            if (value == positionY) return;

            changedPositionY = (value != originalPositionY);

            positionY = value;
        }
    }

    public float PositionZ
    {
        get { return positionZ; }
        set
        {
            if (value == positionZ) return;

            changedPositionZ = (value != originalPositionZ);

            positionZ = value;
        }
    }

    public int RotationX
    {
        get { return rotationX; }
        set
        {
            if (value == rotationX) return;

            changedRotationX = (value != originalRotationX);

            rotationX = value;
        }
    }

    public int RotationY
    {
        get { return rotationY; }
        set
        {
            if (value == rotationY) return;

            changedRotationY = (value != originalRotationY);

            rotationY = value;
        }
    }

    public int RotationZ
    {
        get { return rotationZ; }
        set
        {
            if (value == rotationZ) return;

            changedRotationZ = (value != originalRotationZ);

            rotationZ = value;
        }
    }

    public float ScaleMultiplier
    {
        get { return scaleMultiplier; }
        set
        {
            if (value == scaleMultiplier) return;

            changedScaleMultiplier = (value != originalScaleMultiplier);

            scaleMultiplier = value;
        }
    }

    public int PlayedSeconds
    {
        get { return playedSeconds; }
        set
        {
            if (value == playedSeconds) return;

            changedPlayedSeconds = (value != originalPlayedSeconds);

            playedSeconds = value;
        }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var playerSaveData = Fixtures.playerSaveList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedRegionId)
            playerSaveData.regionId = regionId;

        if (changedPartyMemberId)
            playerSaveData.partyMemberId = partyMemberId;
        
        if (changedPositionX)
            playerSaveData.positionX = positionX;

        if (changedPositionY)
            playerSaveData.positionY = positionY;

        if (changedPositionZ)
            playerSaveData.positionZ = positionZ;

        if (changedRotationX)
            playerSaveData.rotationX = rotationX;

        if (changedRotationY)
            playerSaveData.rotationY = rotationY;

        if (changedRotationZ)
            playerSaveData.rotationZ = rotationZ;

        if (changedScaleMultiplier)
            playerSaveData.scaleMultiplier = scaleMultiplier;

        if (changedPlayedSeconds)
            playerSaveData.playedSeconds = playedSeconds;
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
        originalRegionId = regionId;
        originalPartyMemberId = partyMemberId;
        
        originalPositionX = positionX;
        originalPositionY = positionY;
        originalPositionZ = positionZ;

        originalRotationX = rotationX;
        originalRotationY = rotationY;
        originalRotationZ = rotationZ;

        originalScaleMultiplier = scaleMultiplier;

        originalPlayedSeconds = playedSeconds;
    }

    public void GetOriginalValues()
    {
        regionId = originalRegionId;
        partyMemberId = originalPartyMemberId;
        
        positionX = originalPositionX;
        positionY = originalPositionY;
        positionZ = originalPositionZ;

        rotationX = originalRotationX;
        rotationY = originalRotationY;
        rotationZ = originalRotationZ;

        scaleMultiplier = originalScaleMultiplier;

        playedSeconds = originalPlayedSeconds;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedRegionId = false;
        changedPartyMemberId = false;
        
        changedPositionX = false;
        changedPositionY = false;
        changedPositionZ = false;

        changedRotationX = false;
        changedRotationY = false;
        changedRotationZ = false;

        changedScaleMultiplier = false;

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
