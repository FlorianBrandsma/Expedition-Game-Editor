using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SceneInteractableDataElement : SceneInteractableCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public SceneInteractableDataElement() : base() { }

    public int terrainTileId;

    public int objectGraphicId;
    public string objectGraphicPath;

    public string interactableName;
    public string objectGraphicIconPath;

    public string originalInteractableName;
    public string originalObjectGraphicIconPath;

    public float positionX;
    public float positionY;
    public float positionZ;

    public int rotationX;
    public int rotationY;
    public int rotationZ;

    public float scaleMultiplier;

    public int animation;

    public Vector2 startPosition;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalInteractableName = interactableName;
        originalObjectGraphicIconPath = objectGraphicIconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        interactableName = originalInteractableName;
        objectGraphicIconPath = originalObjectGraphicIconPath;
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}
