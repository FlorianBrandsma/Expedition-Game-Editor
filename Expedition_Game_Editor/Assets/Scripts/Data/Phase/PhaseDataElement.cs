using UnityEngine;
using System.Collections;

public class PhaseDataElement : PhaseCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public PhaseDataElement() : base()
    {
        DataType = Enums.DataType.Phase;
    }

    public int terrainTileId;

    public int partyMemberId;

    public int objectGraphicId;
    public string objectGraphicPath;

    public string objectGraphicIconPath;
    
    public float height;
    public float width;
    public float depth;

    public string interactableName;

    public string locationName;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IDataElement Clone()
    {
        var dataElement = new PhaseDataElement();

        dataElement.SelectionElement = SelectionElement;

        dataElement.terrainTileId = terrainTileId;

        dataElement.partyMemberId = partyMemberId;

        dataElement.objectGraphicId = objectGraphicId;
        dataElement.objectGraphicPath = objectGraphicPath;

        dataElement.objectGraphicIconPath = objectGraphicIconPath;

        dataElement.height = height;
        dataElement.width = width;
        dataElement.depth = depth;

        dataElement.interactableName = interactableName;

        dataElement.locationName = locationName;

        CloneCore(dataElement);
        
        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var phaseDataSource = (PhaseDataElement)dataSource;

        terrainTileId = phaseDataSource.terrainTileId;

        partyMemberId = phaseDataSource.partyMemberId;

        objectGraphicId = phaseDataSource.objectGraphicId;
        objectGraphicPath = phaseDataSource.objectGraphicPath;

        objectGraphicIconPath = phaseDataSource.objectGraphicIconPath;

        height = phaseDataSource.height;
        width = phaseDataSource.width;
        depth = phaseDataSource.depth;

        interactableName = phaseDataSource.interactableName;

        locationName = phaseDataSource.locationName;

        SetOriginalValues();
    }
}
