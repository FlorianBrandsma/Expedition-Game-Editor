﻿using UnityEngine;
using System.Linq;

public class PlayerSaveCore : GeneralData
{
    private int saveId;
    private int regionId;
    private int partyMemberId;
    
    private float positionX;
    private float positionY;
    private float positionZ;

    private float scaleMultiplier;

    //Recorded in seconds
    private int gameTime;
    private int playedTime;

    //Original
    public int originalRegionId;
    public int originalPartyMemberId;

    public float originalScaleMultiplier;

    private int originalPlayedSeconds;

    //Changed
    private bool changedRegionId;
    private bool changedPartyMemberId;

    private bool changedScaleMultiplier;

    private bool changedPlayedSeconds;
    
    public bool Changed
    {
        get
        {
            return  changedRegionId || changedPartyMemberId || changedScaleMultiplier;
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
        var playerSaveData = Fixtures.playerSaveList.Where(x => x.Id == Id).FirstOrDefault();

        playerSaveData.positionX = positionX;
        playerSaveData.positionY = positionY;
        playerSaveData.positionZ = positionZ;

        playerSaveData.gameTime = gameTime;
        playerSaveData.playedTime = playedTime;

        if (!Changed) return;

        if (changedRegionId)
            playerSaveData.regionId = regionId;
        
        if (changedScaleMultiplier)
            playerSaveData.scaleMultiplier = scaleMultiplier;  
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
        
        originalScaleMultiplier = scaleMultiplier;

        originalPlayedSeconds = playedTime;
    }

    public void GetOriginalValues()
    {
        regionId = originalRegionId;
        partyMemberId = originalPartyMemberId;

        scaleMultiplier = originalScaleMultiplier;

        playedTime = originalPlayedSeconds;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedRegionId = false;
        changedPartyMemberId = false;

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