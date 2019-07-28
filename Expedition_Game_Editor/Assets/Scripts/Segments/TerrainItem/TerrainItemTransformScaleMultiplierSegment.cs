using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class TerrainItemTransformScaleMultiplierSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    private InteractableController ElementController { get { return (InteractableController)SegmentController.DataController; } }

    #region UI

    public EditorInputNumber inputField;

    #endregion

    #region Data Variables

    private float scaleMultiplier;

    #endregion

    #region Data Properties

    public float ScaleMultiplier
    {
        get { return scaleMultiplier; }
        set
        {
            scaleMultiplier = value;

            switch (DataEditor.Data.DataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionData = (InteractionDataElement)DataEditor.Data.DataElement;
                    interactionData.ScaleMultiplier = value;

                    break;

                case Enums.DataType.TerrainObject:

                    var terrainObjectData = (TerrainObjectDataElement)DataEditor.Data.DataElement;
                    terrainObjectData.ScaleMultiplier = value;

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }

    #endregion

    public void UpdateScaleMultiplier()
    {
        ScaleMultiplier = inputField.Value;

        DataEditor.UpdateEditor();
    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

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
            case Enums.DataType.Interaction: InitializeInteractionData(); break;
            case Enums.DataType.TerrainObject: InitializeTerrainObjectData(); break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.DataElement;

        scaleMultiplier = interactionData.ScaleMultiplier;
    }

    private void InitializeTerrainObjectData()
    {
        var terrainObjectData = (TerrainObjectDataElement)DataEditor.Data.DataElement;

        scaleMultiplier = terrainObjectData.ScaleMultiplier;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        inputField.Value = ScaleMultiplier;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
