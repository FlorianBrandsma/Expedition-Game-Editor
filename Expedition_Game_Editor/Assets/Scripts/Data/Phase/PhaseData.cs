using UnityEngine;

public class PhaseData : PhaseBaseData
{
    public int TerrainTileId        { get; set; }

    public int PartyMemberId        { get; set; }

    public int ModelId              { get; set; }
    public string ModelPath         { get; set; }

    public string ModelIconPath     { get; set; }

    public float Height             { get; set; }
    public float Width              { get; set; }
    public float Depth              { get; set; }

    public float Scale              { get; set; }

    public string InteractableName  { get; set; }
    public string LocationName      { get; set; }

    public override void GetOriginalValues(PhaseData originalData)
    {
        TerrainTileId       = originalData.TerrainTileId;

        PartyMemberId       = originalData.PartyMemberId;

        ModelId             = originalData.ModelId;
        ModelPath           = originalData.ModelPath;

        ModelIconPath       = originalData.ModelIconPath;

        Height              = originalData.Height;
        Width               = originalData.Width;
        Depth               = originalData.Depth;

        Scale               = originalData.Scale;

        InteractableName    = originalData.InteractableName;
        LocationName        = originalData.LocationName;

        base.GetOriginalValues(originalData);
    }

    public PhaseData Clone()
    {
        var data = new PhaseData();
        
        data.TerrainTileId      = TerrainTileId;

        data.PartyMemberId      = PartyMemberId;

        data.ModelId            = ModelId;
        data.ModelPath          = ModelPath;

        data.ModelIconPath      = ModelIconPath;

        data.Height             = Height;
        data.Width              = Width;
        data.Depth              = Depth;

        data.Scale              = Scale;

        data.InteractableName   = InteractableName;
        data.LocationName       = LocationName;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(PhaseElementData elementData)
    {
        elementData.TerrainTileId       = TerrainTileId;

        elementData.PartyMemberId       = PartyMemberId;

        elementData.ModelId             = ModelId;
        elementData.ModelPath           = ModelPath;

        elementData.ModelIconPath       = ModelIconPath;

        elementData.Height              = Height;
        elementData.Width               = Width;
        elementData.Depth               = Depth;

        elementData.Scale               = Scale;

        elementData.InteractableName    = InteractableName;
        elementData.LocationName        = LocationName;

        base.Clone(elementData);
    }
}
