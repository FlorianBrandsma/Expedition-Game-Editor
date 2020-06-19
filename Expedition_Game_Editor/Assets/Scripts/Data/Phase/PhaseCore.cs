﻿using UnityEngine;
using System.Linq;

public class PhaseCore : GeneralData
{
    private int chapterId;

    private string name;

    private int defaultRegionId;

    private float defaultPositionX;
    private float defaultPositionY;
    private float defaultPositionZ;

    private int defaultRotationX;
    private int defaultRotationY;
    private int defaultRotationZ;

    private float defaultScaleMultiplier;

    private string publicNotes;
    private string privateNotes;

    //Original
    public string originalName;

    private int originalDefaultRegionId;

    private float originalDefaultPositionX;
    private float originalDefaultPositionY;
    private float originalDefaultPositionZ;

    private int originalDefaultRotationX;
    private int originalDefaultRotationY;
    private int originalDefaultRotationZ;

    private float originalDefaultScaleMultiplier;

    public string originalPublicNotes;
    public string originalPrivateNotes;

    //Changed
    private bool changedName;

    private bool changedDefaultRegionId;

    private bool changedDefaultPositionX;
    private bool changedDefaultPositionY;
    private bool changedDefaultPositionZ;

    private bool changedDefaultRotationX;
    private bool changedDefaultRotationY;
    private bool changedDefaultRotationZ;

    private bool changedDefaultScaleMultiplier;

    private bool changedPublicNotes;
    private bool changedPrivateNotes;

    public bool Changed
    {
        get
        {
            return  changedName                     || changedDefaultRegionId   || 
                    changedDefaultPositionX         || changedDefaultPositionY  || changedDefaultPositionZ ||
                    changedDefaultRotationX         || changedDefaultRotationY  || changedDefaultRotationZ || 
                    changedDefaultScaleMultiplier   || changedPublicNotes       || changedPrivateNotes;
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

    public int DefaultRegionId
    {
        get { return defaultRegionId; }
        set
        {
            if (value == defaultRegionId) return;

            changedDefaultRegionId = (value != originalDefaultRegionId);

            defaultRegionId = value;
        }
    }

    public float DefaultPositionX
    {
        get { return defaultPositionX; }
        set
        {
            if (value == defaultPositionX) return;

            changedDefaultPositionX = (value != originalDefaultPositionX);

            defaultPositionX = value;
        }
    }

    public float DefaultPositionY
    {
        get { return defaultPositionY; }
        set
        {
            if (value == defaultPositionY) return;

            changedDefaultPositionY = (value != originalDefaultPositionY);

            defaultPositionY = value;
        }
    }

    public float DefaultPositionZ
    {
        get { return defaultPositionZ; }
        set
        {
            if (value == defaultPositionZ) return;

            changedDefaultPositionZ = (value != originalDefaultPositionZ);

            defaultPositionZ = value;
        }
    }

    public int DefaultRotationX
    {
        get { return defaultRotationX; }
        set
        {
            if (value == defaultRotationX) return;

            changedDefaultRotationX = (value != originalDefaultRotationX);

            defaultRotationX = value;
        }
    }

    public int DefaultRotationY
    {
        get { return defaultRotationY; }
        set
        {
            if (value == defaultRotationY) return;

            changedDefaultRotationY = (value != originalDefaultRotationY);

            defaultRotationY = value;
        }
    }

    public int DefaultRotationZ
    {
        get { return defaultRotationZ; }
        set
        {
            if (value == defaultRotationZ) return;

            changedDefaultRotationZ = (value != originalDefaultRotationZ);

            defaultRotationZ = value;
        }
    }

    public float DefaultScaleMultiplier
    {
        get { return defaultScaleMultiplier; }
        set
        {
            if (value == defaultScaleMultiplier) return;

            changedDefaultScaleMultiplier = (value != originalDefaultScaleMultiplier);

            defaultScaleMultiplier = value;
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

        if (changedDefaultRegionId)
            phaseData.defaultRegionId = defaultRegionId;

        if (changedDefaultPositionX)
            phaseData.defaultPositionX = defaultPositionX;

        if (changedDefaultPositionY)
            phaseData.defaultPositionY = defaultPositionY;

        if (changedDefaultPositionZ)
            phaseData.defaultPositionZ = defaultPositionZ;

        if (changedDefaultRotationX)
            phaseData.defaultRotationX = defaultRotationX;

        if (changedDefaultRotationY)
            phaseData.defaultRotationY = defaultRotationY;

        if (changedDefaultRotationZ)
            phaseData.defaultRotationZ = defaultRotationZ;

        if (changedDefaultScaleMultiplier)
            phaseData.defaultScaleMultiplier = defaultScaleMultiplier;

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

        originalDefaultRegionId = defaultRegionId;

        originalDefaultPositionX = defaultPositionX;
        originalDefaultPositionY = defaultPositionY;
        originalDefaultPositionZ = defaultPositionZ;

        originalDefaultRotationX = defaultRotationX;
        originalDefaultRotationY = defaultRotationY;
        originalDefaultRotationZ = defaultRotationZ;

        originalDefaultScaleMultiplier = defaultScaleMultiplier;

        originalPublicNotes = publicNotes;
        originalPrivateNotes = privateNotes;
    }

    public void GetOriginalValues()
    {
        name = originalName;

        defaultRegionId = originalDefaultRegionId;

        defaultPositionX = originalDefaultPositionX;
        defaultPositionY = originalDefaultPositionY;
        defaultPositionZ = originalDefaultPositionZ;

        defaultRotationX = originalDefaultRotationX;
        defaultRotationY = originalDefaultRotationY;
        defaultRotationZ = originalDefaultRotationZ;

        defaultScaleMultiplier = originalDefaultScaleMultiplier;

        publicNotes = originalPublicNotes;
        privateNotes = originalPrivateNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedName = false;

        changedDefaultRegionId = false;

        changedDefaultPositionX = false;
        changedDefaultPositionY = false;
        changedDefaultPositionZ = false;

        changedDefaultRotationX = false;
        changedDefaultRotationY = false;
        changedDefaultRotationZ = false;

        changedDefaultScaleMultiplier = false;

        changedPublicNotes = false;
        changedPrivateNotes = false;
    }

    public void Delete() { }

    public void CloneCore(PhaseDataElement dataElement)
    {
        CloneGeneralData(dataElement);

        dataElement.defaultPositionX = defaultPositionX;

        dataElement.name = name;

        dataElement.defaultRegionId = defaultRegionId;

        dataElement.defaultPositionX = defaultPositionX;
        dataElement.defaultPositionY = defaultPositionY;
        dataElement.defaultPositionZ = defaultPositionZ;

        dataElement.defaultRotationX = defaultRotationX;
        dataElement.defaultRotationY = defaultRotationY;
        dataElement.defaultRotationZ = defaultRotationZ;

        dataElement.defaultScaleMultiplier = defaultScaleMultiplier;

        dataElement.publicNotes = publicNotes;
        dataElement.privateNotes = privateNotes;

        //Original
        dataElement.originalName = originalName;

        dataElement.originalDefaultRegionId = originalDefaultRegionId;

        dataElement.originalDefaultPositionX = originalDefaultPositionX;
        dataElement.originalDefaultPositionY = originalDefaultPositionY;
        dataElement.originalDefaultPositionZ = originalDefaultPositionZ;

        dataElement.originalDefaultRotationX = originalDefaultRotationX;
        dataElement.originalDefaultRotationY = originalDefaultRotationY;
        dataElement.originalDefaultRotationZ = originalDefaultRotationZ;

        dataElement.originalDefaultScaleMultiplier = originalDefaultScaleMultiplier;

        dataElement.originalPublicNotes = originalPublicNotes;
        dataElement.originalPrivateNotes = originalPrivateNotes;
    }
    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var phaseDataSource = (PhaseDataElement)dataSource;

        chapterId = phaseDataSource.chapterId;

        name = phaseDataSource.name;

        defaultRegionId = phaseDataSource.defaultRegionId;

        defaultPositionX = phaseDataSource.defaultPositionX;
        defaultPositionY = phaseDataSource.defaultPositionY;
        defaultPositionZ = phaseDataSource.defaultPositionZ;

        defaultRotationX = phaseDataSource.defaultRotationX;
        defaultRotationY = phaseDataSource.defaultRotationY;
        defaultRotationZ = phaseDataSource.defaultRotationZ;

        defaultScaleMultiplier = phaseDataSource.defaultScaleMultiplier;

        publicNotes = phaseDataSource.publicNotes;
        privateNotes = phaseDataSource.privateNotes;
    }
}
