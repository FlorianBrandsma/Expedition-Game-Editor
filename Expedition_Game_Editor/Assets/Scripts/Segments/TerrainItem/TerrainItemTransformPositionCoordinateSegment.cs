using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class TerrainItemTransformPositionCoordinateSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController     { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    private InteractableController ElementController{ get { return (InteractableController)SegmentController.DataController; } }

    #region UI

    public EditorInputNumber xInputField, yInputField, zInputField;

    #endregion

    #region Data Variables

    private float positionX, positionY, positionZ;

    #endregion

    #region Data Properties

    public float PositionX
    {
        get { return positionX; }
        set
        {
            positionX = value;

            switch (DataEditor.Data.DataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = (InteractionDataElement)DataEditor.Data.DataElement;
                    interactionData.PositionX = value;

                    break;

                case Enums.DataType.TerrainObject:

                    var terrainObjectData = (TerrainObjectDataElement)DataEditor.Data.DataElement;
                    terrainObjectData.PositionX = value;

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }

    public float PositionY
    {
        get { return positionY; }
        set
        {
            positionY = value;

            switch (DataEditor.Data.DataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = (InteractionDataElement)DataEditor.Data.DataElement;
                    interactionData.PositionY = value;

                    break;

                case Enums.DataType.TerrainObject:

                    var terrainObjectData = (TerrainObjectDataElement)DataEditor.Data.DataElement;
                    terrainObjectData.PositionY = value;

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }

    public float PositionZ
    {
        get { return positionZ; }
        set
        {
            positionZ = value;

            switch (DataEditor.Data.DataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = (InteractionDataElement)DataEditor.Data.DataElement;
                    interactionData.PositionZ = value;

                    break;

                case Enums.DataType.TerrainObject:

                    var terrainObjectData = (TerrainObjectDataElement)DataEditor.Data.DataElement;
                    terrainObjectData.PositionZ = value;

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }
    #endregion

    public void UpdatePositionX()
    {
        PositionX = xInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdatePositionY()
    {
        PositionY = yInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdatePositionZ()
    {
        PositionZ = zInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void InitializeSegment()
    {
        InitializeDependencies();

        InitializeData();
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeData()
    {
        switch (DataEditor.Data.DataController.DataType)
        {
            case Enums.DataType.Interaction:    InitializeInteractionData();    break;
            case Enums.DataType.TerrainObject:  InitializeTerrainObjectData();  break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.DataElement;

        positionX = interactionData.PositionX;
        positionY = interactionData.PositionY;
        positionZ = interactionData.PositionZ;
    }

    private void InitializeTerrainObjectData()
    {
        var terrainObjectData = (TerrainObjectDataElement)DataEditor.Data.DataElement;

        positionX = terrainObjectData.PositionX;
        positionY = terrainObjectData.PositionY;
        positionZ = terrainObjectData.PositionZ;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        xInputField.Value = PositionX;
        yInputField.Value = PositionY;
        zInputField.Value = PositionZ;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
