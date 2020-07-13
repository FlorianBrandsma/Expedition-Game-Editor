using UnityEngine;

public class GameAtmosphereElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GameAtmosphereElementData() : base()
    {
        DataType = Enums.DataType.GameAtmosphere;
    }

    public int terrainId;

    public bool isDefault;

    public int startTime;
    public int endTime;

    #region ElementData
    public bool Changed { get { return false; } }
    public void Create() { }
    public void Update() { }
    public void UpdateSearch() { }
    public void UpdateIndex() { }
    public virtual void SetOriginalValues() { }
    public void GetOriginalValues() { }
    public virtual void ClearChanges() { }
    public void Delete() { }
    public IElementData Clone()
    {
        var elementData = new GameAtmosphereElementData();

        CloneGeneralData(elementData);

        return elementData;
    }
    #endregion
}
