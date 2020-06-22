using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class AssetHeaderSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExIndexSwitch indexSwitch;
    public EditorElement editorElement;
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

                    var itemData = (ItemElementData)DataEditor.Data.elementData;

                    itemData.Name = value;

                    break;

                case Enums.DataType.Interactable:

                    var interactableData = (InteractableElementData)DataEditor.Data.elementData;

                    interactableData.Name = value;

                    break;
            }
        }
    }

    public ObjectGraphicElementData ObjectGraphic
    {
        set
        {
            objectGraphicId         = value.Id;
            objectGraphicPath       = value.Path;
            objectGraphicIconPath   = value.iconPath;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemData = (ItemElementData)DataEditor.Data.elementData;

                    itemData.ObjectGraphicId        = value.Id;
                    itemData.objectGraphicPath      = value.Path;
                    itemData.objectGraphicIconPath  = value.iconPath;

                    break;

                case Enums.DataType.Interactable:

                    var interactableData = (InteractableElementData)DataEditor.Data.elementData;

                    interactableData.ObjectGraphicId = value.Id;
                    interactableData.objectGraphicPath = value.Path;
                    interactableData.objectGraphicIconPath = value.iconPath;
                    
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
    public void UpdateName()
    {
        Name = inputField.text;
        DataEditor.UpdateEditor();
    }

    public void UpdateObjectGraphic(ObjectGraphicElementData objectGraphicElementData)
    {
        ObjectGraphic = objectGraphicElementData;
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
        editorElement.DataElement.InitializeElement(editorElement.GetComponent<IDataController>());

        var objectGraphicElementData = new ObjectGraphicElementData();
        
        objectGraphicElementData.DataElement = editorElement.DataElement;

        editorElement.DataElement.data.dataController.DataList = new List<IElementData>() { objectGraphicElementData };
        editorElement.DataElement.data.elementData = objectGraphicElementData;
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
            indexSwitch.InitializeSwitch(this, index);
    }

    private void InitializeItemData()
    {
        var itemData        = (ItemElementData)DataEditor.Data.elementData;

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
        var interactableData = (InteractableElementData)DataEditor.Data.elementData;

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

        var objectGraphicElementData = (ObjectGraphicElementData)editorElement.DataElement.data.elementData;

        objectGraphicElementData.Id         = objectGraphicId;
        objectGraphicElementData.Path       = objectGraphicPath;
        objectGraphicElementData.iconPath   = objectGraphicIconPath;

        SelectionElementManager.Add(editorElement);
        SelectionManager.SelectData(editorElement.DataElement.data.dataController.DataList);

        editorElement.DataElement.SetElement();
        editorElement.SetOverlay();

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        SelectionElementManager.elementPool.Remove(editorElement);

        gameObject.SetActive(false);
    }

    public void SetSearchResult(DataElement dataElement)
    {
        switch(dataElement.data.dataController.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var objectGraphicElementData = (ObjectGraphicElementData)dataElement.data.elementData;

                UpdateObjectGraphic(objectGraphicElementData);

                break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }
    #endregion
}
