﻿using UnityEngine;
using System.Collections.Generic;

public class GameWorldInteractableData
{
    public int Id                       { get; set; }
    
    public int TerrainTileId            { get; set; }
    public int ModelId                  { get; set; }

    public Enums.InteractableType Type  { get; set; }

    public string ModelPath             { get; set; }
    public string ModelIconPath         { get; set; }

    public string InteractableName      { get; set; }

    public int Health                   { get; set; }
    public int Hunger                   { get; set; }
    public int Thirst                   { get; set; }

    public float Weight                 { get; set; }
    public float Speed                  { get; set; }
    public float Stamina                { get; set; }

    public float Height                 { get; set; }
    public float Width                  { get; set; }
    public float Depth                  { get; set; }

    public float Scale                  { get; set; }
    
    public List<GameInteractionElementData> InteractionDataList { get; set; } = new List<GameInteractionElementData>();

    public GameInteractionElementData Interaction { get; set; }
    
    public virtual void GetOriginalValues(GameWorldInteractableData originalData)
    {
        Id                  = originalData.Id;

        TerrainTileId       = originalData.TerrainTileId;
        ModelId             = originalData.ModelId;

        Type                = originalData.Type;

        ModelPath           = originalData.ModelPath;
        ModelIconPath       = originalData.ModelIconPath;

        InteractableName    = originalData.InteractableName;

        Health              = originalData.Health;
        Hunger              = originalData.Hunger;
        Thirst              = originalData.Thirst;

        Weight              = originalData.Weight;
        Speed               = originalData.Speed;
        Stamina             = originalData.Stamina;

        Height              = originalData.Height;
        Width               = originalData.Width;
        Depth               = originalData.Depth;

        Scale               = originalData.Scale;

        Interaction         = originalData.Interaction;
    }

    public GameWorldInteractableData Clone()
    {
        var data = new GameWorldInteractableData();

        data.Id                 = Id;

        data.TerrainTileId      = TerrainTileId;
        data.ModelId            = ModelId;

        data.Type               = Type;

        data.ModelPath          = ModelPath;
        data.ModelIconPath      = ModelIconPath;

        data.InteractableName   = InteractableName;

        data.Health             = Health;
        data.Hunger             = Hunger;
        data.Thirst             = Thirst;

        data.Weight             = Weight;
        data.Speed              = Speed;
        data.Stamina            = Stamina;

        data.Height             = Height;
        data.Width              = Width;
        data.Depth              = Depth;

        data.Scale              = Scale;

        data.Interaction        = Interaction;

        InteractionDataList.ForEach(x => x.SetOriginalValues());

        return data;
    }

    public virtual void Clone(GameWorldInteractableElementData elementData)
    {
        elementData.Id                  = Id;

        elementData.TerrainTileId       = TerrainTileId;
        elementData.ModelId             = ModelId;

        elementData.Type                = Type;

        elementData.ModelPath           = ModelPath;
        elementData.ModelIconPath       = ModelIconPath;

        elementData.InteractableName    = InteractableName;

        elementData.Health              = Health;
        elementData.Hunger              = Hunger;
        elementData.Thirst              = Thirst;

        elementData.Weight              = Weight;
        elementData.Speed               = Speed;
        elementData.Stamina             = Stamina;

        elementData.Height              = Height;
        elementData.Width               = Width;
        elementData.Depth               = Depth;

        elementData.Scale               = Scale;

        elementData.Interaction         = Interaction;
    }
}
