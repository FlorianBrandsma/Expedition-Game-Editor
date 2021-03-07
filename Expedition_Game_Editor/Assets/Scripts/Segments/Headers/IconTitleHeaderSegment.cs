using UnityEngine;
using UnityEngine.UI;

public class IconTitleHeaderSegment : MonoBehaviour, ISegment
{
    public ExIndexSwitch indexSwitch;
    public RawImage icon;
    public Text headerText;
    public Text descriptionText;
    public Text idText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    private int Id
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.User:
                    return ((UserEditor)DataEditor).Id;

                case Enums.DataType.FavoriteUser:
                    return ((FavoriteUserEditor)DataEditor).Id;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).Id;

                case Enums.DataType.InteractionDestination:
                    return ((InteractionDestinationEditor)DataEditor).Id;

                case Enums.DataType.Save:
                    return ((SaveEditor)DataEditor).Id;

                case Enums.DataType.InteractableSave:
                    return ((InteractableSaveEditor)DataEditor).Id;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    public int Index
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Save:
                    return ((SaveEditor)DataEditor).Index;
                    
                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    private string Title
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.User:
                    return ((UserEditor)DataEditor).Username;

                case Enums.DataType.FavoriteUser:
                    return ((FavoriteUserEditor)DataEditor).Username;

                case Enums.DataType.Team:
                    return ((TeamEditor)DataEditor).Name;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).InteractableName;

                case Enums.DataType.InteractionDestination:
                    return ((InteractionDestinationEditor)DataEditor).InteractableName;

                case Enums.DataType.Save:
                    return ((SaveEditor)DataEditor).InteractableName;

                case Enums.DataType.InteractableSave:
                    return ((InteractableSaveEditor)DataEditor).InteractableName;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
    }

    private string Description
    {
        get
        {
            //Starting to look pretty useless, huh
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.User:
                    return string.Empty;

                case Enums.DataType.FavoriteUser:
                    return string.Empty;

                case Enums.DataType.Team:
                    return string.Empty;
                    //return ((TeamEditor)DataEditor).Description;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
    }

    private string IconPath
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.User:
                    return ((UserEditor)DataEditor).IconPath;

                case Enums.DataType.FavoriteUser:
                    return ((FavoriteUserEditor)DataEditor).IconPath;

                case Enums.DataType.Team:
                    return ((TeamEditor)DataEditor).IconPath;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).ModelIconPath;

                case Enums.DataType.InteractionDestination:
                    return ((InteractionDestinationEditor)DataEditor).ModelIconPath;

                case Enums.DataType.Save:
                    return ((SaveEditor)DataEditor).ModelIconPath;

                case Enums.DataType.InteractableSave:
                    return ((InteractableSaveEditor)DataEditor).ModelIconPath;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
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

        if (indexSwitch != null)
        {
            var enabled = Id > 0;

            indexSwitch.EnableElement(enabled);
            indexSwitch.InitializeSwitch(this, enabled ? Index : DataEditor.Data.dataList.Count - 1);
        }
    }

    public void InitializeSegment() { }

    public void OpenSegment()
    {
        if(idText != null)
        {
            if (Id > 0)
                idText.text = Id.ToString();
            else
                idText.text = "New";
        }
        
        icon.texture = Resources.Load<Texture2D>(IconPath);
        headerText.text = Title;

        if(descriptionText != null)
            descriptionText.text = Description;
        
        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}
