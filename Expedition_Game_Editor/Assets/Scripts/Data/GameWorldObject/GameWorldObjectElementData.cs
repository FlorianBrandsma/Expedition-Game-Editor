using UnityEngine;

public class GameWorldObjectElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GameWorldObjectElementData() : base()
    {
        DataType = Enums.DataType.GameWorldObject;
    }

    public int terrainTileId;
    public int objectGraphicId;

    public string objectGraphicPath;

    public string objectGraphicName;
    public string objectGraphicIconPath;

    public int animation;

    public float positionX;
    public float positionY;
    public float positionZ;

    public float rotationX;
    public float rotationY;
    public float rotationZ;

    public float scaleMultiplier;

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
        var elementData = new GameWorldObjectElementData();

        CloneGeneralData(elementData);

        return elementData;
    }
    #endregion
}
