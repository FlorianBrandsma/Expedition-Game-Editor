using UnityEngine;
using UnityEngine.UI;
using System;

public class SaveGameSegment : MonoBehaviour, ISegment
{
    public Text phaseNameText;
    public Text phaseGameNotesText;
    
    public EditorElement startButton;

    public Text startButtonText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    private Enums.SaveType SaveType
    {
        get { return ((SaveEditor)DataEditor).SaveType; }
    }

    private string PhaseName
    {
        get { return ((SaveEditor)DataEditor).PhaseName; }
    }

    private string PhaseGameNotes
    {
        get { return ((SaveEditor)DataEditor).PhaseGameNotes; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        InitializeStartButton();
    }

    private void InitializeStartButton()
    {
        startButton.DataElement.Id      = DataEditor.EditData.Id;
        startButton.DataElement.Data    = DataEditor.Data.dataController.Data;
        startButton.DataElement.Path    = SegmentController.EditorController.PathController.route.path;

        startButton.InitializeElement();
    }

    public void SelectButton()
    {
        switch(SaveType)
        {
            case Enums.SaveType.Save: SaveGame(); break;
            case Enums.SaveType.Load: LoadGame(); break;
        }
    }

    private void LoadGame()
    {
        startButton.SelectElement();
    }

    private void SaveGame()
    {
        GameManager.instance.SaveData((SaveElementData)DataEditor.EditData);

        RenderManager.PreviousPath();
    }

    public void OpenSegment()
    {
        phaseNameText.text = PhaseName;
        phaseGameNotesText.text = PhaseGameNotes;

        startButtonText.text = Enum.GetName(typeof(Enums.SaveType), SaveType) + " Game";
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
