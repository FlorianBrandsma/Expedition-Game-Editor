﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ExTile : MonoBehaviour, IElement, IPoolable
{
    public Enums.ElementType elementType;

    public RawImage icon;
    public RawImage iconBase;

    private string iconPath;

    public SelectionElement Element         { get { return GetComponent<SelectionElement>(); } }

    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return elementType; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        SelectionElementManager.Add(newElement.Element);

        return newElement;
    }

    public void InitializeElement() { }

    public void SetElement()
    {
        switch (Element.data.dataController.DataType)
        {
            case Enums.DataType.Icon:               SetIconElement();               break;
            case Enums.DataType.Interactable:       SetInteractableElement();       break;
            case Enums.DataType.Terrain:            SetTerrainElement();            break;
            case Enums.DataType.Tile:               SetTileElement();               break;
            case Enums.DataType.TerrainTile:        SetTerrainTileElement();        break;
            case Enums.DataType.ObjectGraphic:      SetObjectGraphicElement();      break;
            case Enums.DataType.WorldInteractable:  SetWorldInteractableElement();  break;

            default: Debug.Log("CASE MISSING: " + Element.data.dataController.DataType); break;
        }
    }

    private void SetIconElement()
    {
        var dataElement = (IconDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.Path;
        else
            iconPath = dataElement.originalPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);

        if(dataElement.baseIconPath != "")
            iconBase.texture = Resources.Load<Texture2D>(dataElement.baseIconPath);
    }

    private void SetInteractableElement()
    {
        var dataElement = (InteractableDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.objectGraphicIconPath;
        else
            iconPath = dataElement.originalObjectGraphicIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainElement()
    {
        var dataElement = (TerrainDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.iconPath;
        else
            iconPath = dataElement.originalIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTileElement()
    {
        var dataElement = (TileDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.icon;
        else
            iconPath = dataElement.icon;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainTileElement()
    {
        var dataElement = (TerrainTileDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.iconPath;
        else
            iconPath = dataElement.originalIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetObjectGraphicElement()
    {
        var dataElement = (ObjectGraphicDataElement)Element.data.dataElement;
        
        iconPath = dataElement.iconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetWorldInteractableElement()
    {
        var dataElement = (WorldInteractableDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.objectGraphicIconPath;
        else
            iconPath = dataElement.originalObjectGraphicIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    public void CloseElement() { }

    public void ClosePoolable()
    {
        //gameObject.SetActive(false);
    }
}
