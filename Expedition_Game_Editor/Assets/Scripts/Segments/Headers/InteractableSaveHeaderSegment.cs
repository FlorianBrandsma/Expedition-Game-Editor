using UnityEngine;
using UnityEngine.UI;

public class InteractableSaveHeaderSegment : MonoBehaviour, ISegment
{
    public RawImage icon;
    public Text headerText;
    public Text idText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public InteractableSaveEditor InteractableSaveEditor { get { return (InteractableSaveEditor)DataEditor; } }

    #region Data properties
    private int Id
    {
        get { return InteractableSaveEditor.Id; }
    }

    private string InteractableName
    {
        get { return InteractableSaveEditor.InteractableName; }
    }

    private string ModelIconPath
    {
        get { return InteractableSaveEditor.ModelIconPath; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
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
        icon.texture = Resources.Load<Texture2D>(ModelIconPath);
        headerText.text = InteractableName;
        idText.text = Id.ToString();

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}
