using UnityEngine;
using System.Linq;

public class InteractionCore : GeneralData
{
    private int terrainInteractableId;
    private int regionId;

    private string description;

    private float positionX;
    private float positionY;
    private float positionZ;

    private int rotationX;
    private int rotationY;
    private int rotationZ;

    private float scaleMultiplier;

    private int animation;


    public int originalIndex;
    public string originalDescription;

    public float originalPositionX;
    public float originalPositionY;
    public float originalPositionZ;

    public int originalRotationX;
    public int originalRotationY;
    public int originalRotationZ;

    public float originalScaleMultiplier;

    public int originalAnimation;


    private bool changedIndex;
    private bool changedDescription;

    private bool changedPositionX;
    private bool changedPositionY;
    private bool changedPositionZ;

    private bool changedRotationX;
    private bool changedRotationY;
    private bool changedRotationZ;

    private bool changedScaleMultiplier;

    private bool changedAnimation;

    public bool Changed
    {
        get
        {
            return  changedDescription  || changedPositionX || changedPositionY         || changedPositionZ || changedRotationX || 
                    changedRotationY    || changedRotationZ || changedScaleMultiplier   || changedAnimation;
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

    public int TerrainInteractableId
    {
        get { return terrainInteractableId; }
        set { terrainInteractableId = value; }
    }

    public int RegionId
    {
        get { return regionId; }
        set { regionId = value; }
    }

    public string Description
    {
        get { return description; }
        set
        {
            if (value == description) return;

            changedDescription = (value != originalDescription);

            description = value;
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

    public int Animation
    {
        get { return animation; }
        set
        {
            if (value == animation) return;

            changedAnimation = (value != originalAnimation);

            animation = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update()
    {
        var interactionData = Fixtures.interactionList.Where(x => x.id == id).FirstOrDefault();

        if (changedDescription)
            interactionData.description = description;

        if (changedPositionX)
            interactionData.positionX = positionX;

        if (changedPositionY)
            interactionData.positionY = positionY;

        if (changedPositionZ)
            interactionData.positionZ = positionZ;

        if (changedRotationX)
            interactionData.rotationX = rotationX;

        if (changedRotationY)
            interactionData.rotationY = rotationY;

        if (changedRotationZ)
            interactionData.rotationZ = rotationZ;

        if (changedScaleMultiplier)
            interactionData.scaleMultiplier = scaleMultiplier;

        if (changedAnimation)
            interactionData.animation = animation;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        var interactionData = Fixtures.interactionList.Where(x => x.id == id).FirstOrDefault();

        if (changedIndex)
        {
            interactionData.index = index;

            changedIndex = false;
        }
    }

    public virtual void SetOriginalValues()
    {
        originalDescription = description;

        originalPositionX = positionX;
        originalPositionY = positionY;
        originalPositionZ = positionZ;

        originalRotationX = rotationX;
        originalRotationY = rotationY;
        originalRotationZ = rotationZ;

        originalScaleMultiplier = scaleMultiplier;

        originalAnimation = animation;
    }

    public void GetOriginalValues()
    {
        description = originalDescription;

        positionX = originalPositionX;
        positionY = originalPositionY;
        positionZ = originalPositionZ;

        rotationX = originalRotationX;
        rotationY = originalRotationY;
        rotationZ = originalRotationZ;

        scaleMultiplier = originalScaleMultiplier;

        animation = originalAnimation;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedIndex = false;

        changedDescription = false;

        changedPositionX = false;
        changedPositionY = false;
        changedPositionZ = false;

        changedRotationX = false;
        changedRotationY = false;
        changedRotationZ = false;

        changedScaleMultiplier = false;

        changedAnimation = false;
    }

    public void Delete()
    {

    }

    #endregion
}
