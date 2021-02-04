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

    private SaveEditor SaveEditor { get { return (SaveEditor)DataEditor; } }

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
        //Assign data
        SetStartButtonData();

        startButton.DataElement.Path = SegmentController.EditorController.PathController.route.path;

        startButton.InitializeElement();
    }

    private void SetStartButtonData()
    {
        startButton.DataElement.Id = DataEditor.EditData.Id;
        startButton.DataElement.Data = DataEditor.Data.dataController.Data;
    }

    public void SelectButton()
    {
        switch(SaveEditor.SaveType)
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
        GameManager.instance.SaveData((SaveElementData)SaveEditor.EditData);

        RenderManager.PreviousPath();
    }

    public void OpenSegment()
    {
        phaseNameText.text = SaveEditor.PhaseName;
        phaseGameNotesText.text = SaveEditor.PhaseGameNotes;

        startButtonText.text = Enum.GetName(typeof(Enums.SaveType), SaveEditor.SaveType) + " Game";
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
