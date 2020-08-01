using UnityEngine;
using System.Linq;

public class InteractableCore : GeneralData
{
    private int type;

    private int objectGraphicId;

    private string name;

    private float scaleMultiplier;

    private int health;
    private int hunger;
    private int thirst;

    private float weight;
    private float speed;
    private float stamina;

    //Original
    public int originalObjectGraphicId;

    public string originalName;

    public float originalScaleMultiplier;

    public int originalHealth;
    public int originalHunger;
    public int originalThirst;

    public float originalWeight;
    public float originalSpeed;
    public float originalStamina;

    //Changed
    private bool changedObjectGraphicId;

    private bool changedName;

    private bool changedScaleMultiplier;

    private bool changedHealth;
    private bool changedHunger;
    private bool changedThirst;

    private bool changedWeight;
    private bool changedSpeed;
    private bool changedStamina;

    public bool Changed
    {
        get
        {
            return  changedObjectGraphicId  || changedName      || changedScaleMultiplier   ||
                    changedHealth           || changedHunger    || changedThirst            ||
                    changedWeight           || changedSpeed     || changedStamina;
        }
    }

    #region Properties
    public int Type
    {
        get { return type; }
        set { type = value; }
    }

    public int ObjectGraphicId
    {
        get { return objectGraphicId; }
        set
        {
            if (value == objectGraphicId) return;

            changedObjectGraphicId = (value != originalObjectGraphicId);

            objectGraphicId = value;
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

    public int Health
    {
        get { return health; }
        set
        {
            if (value == health) return;

            changedHealth = (value != originalHealth);

            health = value;
        }
    }

    public int Hunger
    {
        get { return hunger; }
        set
        {
            if (value == hunger) return;

            changedHunger = (value != originalHunger);

            hunger = value;
        }
    }

    public int Thirst
    {
        get { return thirst; }
        set
        {
            if (value == thirst) return;

            changedThirst = (value != originalThirst);

            thirst = value;
        }
    }

    public float Weight
    {
        get { return weight; }
        set
        {
            if (value == weight) return;

            changedWeight = (value != originalWeight);

            weight = value;
        }
    }

    public float Speed
    {
        get { return speed; }
        set
        {
            if (value == speed) return;

            changedSpeed = (value != originalSpeed);

            speed = value;
        }
    }

    public float Stamina
    {
        get { return stamina; }
        set
        {
            if (value == stamina) return;

            changedStamina = (value != originalStamina);

            stamina = value;
        }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var interactableData = Fixtures.interactableList.Where(x => x.Id == Id).FirstOrDefault();
        
        if (changedObjectGraphicId)
            interactableData.objectGraphicId = objectGraphicId;

        if (changedName)
            interactableData.name = name;

        if (changedScaleMultiplier)
            interactableData.scaleMultiplier = scaleMultiplier;

        if (changedHealth)
            interactableData.health = health;

        if (changedHunger)
            interactableData.hunger = hunger;

        if (changedThirst)
            interactableData.thirst = thirst;

        if (changedWeight)
            interactableData.weight = weight;

        if (changedSpeed)
            interactableData.speed = speed;

        if (changedStamina)
            interactableData.stamina = stamina;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        var interactableData = Fixtures.interactableList.Where(x => x.Id == Id).FirstOrDefault();

        interactableData.Index = Index;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalObjectGraphicId = ObjectGraphicId;

        originalName = Name;

        originalScaleMultiplier = scaleMultiplier;

        originalHealth = health;
        originalHunger = hunger;
        originalThirst = thirst;

        originalWeight = weight;
        originalSpeed = speed;
        originalStamina = stamina;
    }

    public void GetOriginalValues()
    {
        objectGraphicId = originalObjectGraphicId;

        name = originalName;

        scaleMultiplier = originalScaleMultiplier;

        health = originalHealth;
        hunger = originalHunger;
        thirst = originalThirst;

        weight = originalWeight;
        speed = originalSpeed;
        stamina = originalStamina;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedObjectGraphicId = false;

        changedName = false;

        changedScaleMultiplier = false;

        changedHealth = false;
        changedHunger = false;
        changedThirst = false;

        changedWeight = false;
        changedSpeed = false;
        changedStamina = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var interactableDataSource = (InteractableElementData)dataSource;

        objectGraphicId = interactableDataSource.objectGraphicId;

        name = interactableDataSource.name;

        scaleMultiplier = interactableDataSource.scaleMultiplier;

        health = interactableDataSource.health;
        hunger = interactableDataSource.hunger;
        thirst = interactableDataSource.thirst;

        weight = interactableDataSource.weight;
        speed = interactableDataSource.speed;
        stamina = interactableDataSource.stamina;
    }
}
