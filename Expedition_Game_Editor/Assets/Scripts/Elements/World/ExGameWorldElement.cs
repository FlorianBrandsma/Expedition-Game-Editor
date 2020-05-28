using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExGameWorldElement : MonoBehaviour, IElement, IPoolable
{
    private ObjectGraphic objectGraphic;

    private Vector3 startPosition = Vector3.zero;

    private Vector3 position;
    private Vector3 rotation;

    private float scaleMultiplier;
    
    public GameElement Element              { get { return GetComponent<GameElement>(); } }

    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return Enums.ElementType.GameWorldElement; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        return newElement;
    }

    public void InitializeElement() { }

    public void SetElement()
    {
        if (objectGraphic != null)
            objectGraphic.gameObject.SetActive(false);

        switch (Element.GeneralData.DataType)
        {
            case Enums.DataType.WorldObject: SetWorldObjectElement(); break;

            default: Debug.Log("CASE MISSING: " + Element.GeneralData.DataType); break;
        }

        transform.localPosition     = new Vector3(startPosition.x + position.x, startPosition.y - position.y, -position.z);
        transform.localEulerAngles  = new Vector3(rotation.x, rotation.y, rotation.z);
        transform.localScale        = new Vector3(1 * scaleMultiplier, 1 * scaleMultiplier, 1 * scaleMultiplier);
    }

    private void SetWorldObjectElement()
    {
        var dataElement = (WorldObjectDataElement)Element.DataElement;

        var prefab = Resources.Load<ObjectGraphic>(dataElement.objectGraphicPath);
        objectGraphic = (ObjectGraphic)PoolManager.SpawnObject(dataElement.ObjectGraphicId, prefab);

        startPosition = dataElement.startPosition;

        position = new Vector3(dataElement.PositionX, dataElement.PositionY, dataElement.PositionZ);
        rotation = new Vector3(dataElement.RotationX, dataElement.RotationY, dataElement.RotationZ);

        scaleMultiplier = dataElement.ScaleMultiplier;

        SetObjectGraphic();
    }

    private void SetObjectGraphic()
    {
        //objectGraphic.selectionElement = Element;

        objectGraphic.transform.SetParent(transform, false);

        objectGraphic.gameObject.SetActive(true);
    }

    public void CloseElement() { }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
