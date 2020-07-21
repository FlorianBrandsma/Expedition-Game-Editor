using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldElementTransformSizeScaleSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExInputNumber inputField;

    public Text heightText, widthText, depthText;
    #endregion

    #region Data Variables
    private float height;
    private float width;
    private float depth;
    private float scaleMultiplier;
    #endregion

    #region Properties
    public float ScaleMultiplier
    {
        get { return scaleMultiplier; }
        set
        {
            scaleMultiplier = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Interaction:

                    var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
                    interactionDataList.ForEach(interactionData =>
                    {
                        interactionData.ScaleMultiplier = value;
                    });

                    break;

                case Enums.DataType.WorldObject:

                    var worldObjectDataList = DataEditor.DataList.Cast<WorldObjectElementData>().ToList();
                    worldObjectDataList.ForEach(worldObjectData =>
                    {
                        worldObjectData.ScaleMultiplier = value;
                    });

                    break;

                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.DefaultScaleMultiplier = value;
                    });

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    public int Time
    {
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseElementData>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.DefaultTime = value;
                    });

                    break;
            }
        }
    }
    #endregion

    #region Methods
    public void UpdateScaleMultiplier()
    {
        ScaleMultiplier = inputField.Value;

        SetSizeValues();

        DataEditor.UpdateEditor();
    }

    public void SetSizeValues()
    {
        heightText.text = (height * scaleMultiplier).ToString();
        widthText.text = (width * scaleMultiplier).ToString();
        depthText.text = (depth * scaleMultiplier).ToString();
    }

    public void UpdateTime()
    {
        Time = TimeManager.instance.ActiveTime;

        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        SetSizeValues();
    }
    
    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Interaction:    InitializeInteractionData();    break;
            case Enums.DataType.WorldObject:    InitializeWorldObjectData();    break;
            case Enums.DataType.Phase:          InitializePhaseData();          break;

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
        }
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionElementData)DataEditor.Data.elementData;

        height = interactionData.height;
        width = interactionData.width;
        depth = interactionData.depth;

        scaleMultiplier = interactionData.ScaleMultiplier;
    }

    private void InitializeWorldObjectData()
    {
        var worldObjectData = (WorldObjectElementData)DataEditor.Data.elementData;

        height = worldObjectData.height;
        width = worldObjectData.width;
        depth = worldObjectData.depth;

        scaleMultiplier = worldObjectData.ScaleMultiplier;
    }

    private void InitializePhaseData()
    {
        var phaseData = (PhaseElementData)DataEditor.Data.elementData;

        height = phaseData.height;
        width = phaseData.width;
        depth = phaseData.depth;

        scaleMultiplier = phaseData.DefaultScaleMultiplier;

        TimeManager.instance.ActiveTime = phaseData.DefaultTime;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        SetSizeValues();

        inputField.Value = ScaleMultiplier;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
