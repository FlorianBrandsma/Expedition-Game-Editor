using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class AssetHeaderSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public IndexSwitch indexSwitch;
    public SelectionElement selectionElement;
    public InputField inputField;
    public Text idText;
    #endregion

    #region Data Variables
    private int id;
    private int index;
    private int objectGraphicId;
    private string assetName;
    private string objectGraphicPath;
    private string objectGraphicIconPath;
    #endregion

    #region Properties
    public string Name
    {
        get { return assetName; }
        set
        {
            assetName = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemDataList = DataEditor.DataList.Cast<ItemDataElement>().ToList();
                    itemDataList.ForEach(itemData =>
                    {
                        itemData.Name = value;
                    });

                    break;

                case Enums.DataType.Interactable:

                    var interactableDataList = DataEditor.DataList.Cast<InteractableDataElement>().ToList();
                    interactableDataList.ForEach(interactableData =>
                    {
                        interactableData.Name = value;
                    });

                    break;
            }
        }
    }

    public ObjectGraphicDataElement ObjectGraphic
    {
        set
        {
            objectGraphicId    = value.Id;
            objectGraphicPath  = value.Path;
            objectGraphicIconPath  = value.iconPath;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemDataList = DataEditor.DataList.Cast<ItemDataElement>().ToList();
                    itemDataList.ForEach(itemData => 
                    {
                        itemData.ObjectGraphicId = value.Id;
                        itemData.objectGraphicPath = value.Path;
                        itemData.objectGraphicIconPath = value.iconPath;
                    });
                    
                    break;

                case Enums.DataType.Interactable:

                    var interactableDataList = DataEditor.DataList.Cast<InteractableDataElement>().ToList();
                    interactableDataList.ForEach(interactableData =>
                    {
                        interactableData.ObjectGraphicId = value.Id;
                        interactableData.objectGraphicPath = value.Path;
                        interactableData.objectGraphicIconPath = value.iconPath;
                    });

                    break;
            }
        }
    }

    public int ObjectGraphicId
    {
        get { return objectGraphicId; }
    }
    #endregion

    #region Methods
    public void Awake()
    {
        SelectionElementManager.Add(selectionElement);
    }

    public void UpdateName()
    {
        Name = inputField.text;
        DataEditor.UpdateEditor();
    }

    public void UpdateObjectGraphic(ObjectGraphicDataElement objectGraphicDataElement)
    {
        ObjectGraphic = objectGraphicDataElement;
        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        InitializeData();

        selectionElement.InitializeElement(selectionElement.GetComponent<IDataController>());
    }
    
    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Item:           InitializeItemData();           break;
            case Enums.DataType.Interactable:   InitializeInteractableData();   break;
        }

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, index, DataEditor.Data.dataController.DataList.Count - 1);
    }

    private void InitializeItemData()
    {
        var itemData        = (ItemDataElement)DataEditor.Data.dataElement;

        id                  = itemData.Id;
        index               = itemData.Index;
        assetName           = itemData.Name;

        objectGraphicId     = itemData.ObjectGraphicId;
        objectGraphicPath   = itemData.objectGraphicPath;
        objectGraphicIconPath   = itemData.objectGraphicIconPath;

        GetComponent<ObjectProperties>().castShadow = false;
    }

    private void InitializeInteractableData()
    {
        var interactableData = (InteractableDataElement)DataEditor.Data.dataElement;

        id                  = interactableData.Id;
        index               = interactableData.Index;
        assetName           = interactableData.Name;

        objectGraphicId     = interactableData.ObjectGraphicId;
        objectGraphicPath   = interactableData.objectGraphicPath;
        objectGraphicIconPath   = interactableData.objectGraphicIconPath;

        GetComponent<ObjectProperties>().castShadow = true;
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        idText.text = id.ToString();

        inputField.text = assetName;

        var objectGraphicDataElement    = new ObjectGraphicDataElement();

        objectGraphicDataElement.Id     = objectGraphicId;
        objectGraphicDataElement.Path   = objectGraphicPath;
        objectGraphicDataElement.iconPath   = objectGraphicIconPath;

        objectGraphicDataElement.SelectionElement = selectionElement;

        selectionElement.data.dataController.DataList = new List<IDataElement>() { objectGraphicDataElement };
        selectionElement.data.dataElement = objectGraphicDataElement;

        selectionElement.SetElement();

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        switch(selectionElement.data.dataController.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var objectGraphicDataElement = (ObjectGraphicDataElement)selectionElement.data.dataElement;

                UpdateObjectGraphic(objectGraphicDataElement);

                break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }
    #endregion
}
