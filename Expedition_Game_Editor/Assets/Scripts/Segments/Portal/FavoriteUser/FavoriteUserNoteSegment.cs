using UnityEngine;
using UnityEngine.UI;

public class FavoriteUserNoteSegment : MonoBehaviour, ISegment
{
    public InputField inputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    public string Note
    {
        get { return ((FavoriteUserEditor)DataEditor).Note; }
        set
        {
            var favoriteUserEditor = (FavoriteUserEditor)DataEditor;
            favoriteUserEditor.Note = value;
        }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if(!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;
    }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        inputField.text = Note;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateNote()
    {
        Note = inputField.text;
        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
