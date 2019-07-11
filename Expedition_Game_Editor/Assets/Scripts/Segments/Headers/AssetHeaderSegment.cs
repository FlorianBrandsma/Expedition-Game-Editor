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

            switch (DataEditor.Data.DataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemData    = (ItemDataElement)DataEditor.Data.DataElement;
                    itemData.Name   = value;

                    break;

                case Enums.DataType.Interactable:

                    var elementData     = (InteractableDataElement)DataEditor.Data.DataElement;
                    elementData.Name    = value;

                    break;
            }
        }
    }

    public ObjectGraphicDataElement ObjectGraphic
    {
        set
        {
            objectGraphicId    = value.id;
            objectGraphicPath  = value.Path;
            objectGraphicIconPath  = value.iconPath;

            switch (DataEditor.Data.DataController.DataType)
            {
                case Enums.DataType.Item:

                    var itemData                = (ItemDataElement)DataEditor.Data.DataElement;
                    itemData.ObjectGraphicId    = value.id;
                    itemData.objectGraphicPath  = value.Path;
                    itemData.objectGraphicIconPath  = value.iconPath;

                    break;

                case Enums.DataType.Interactable:

                    var elementData                 = (InteractableDataElement)DataEditor.Data.DataElement;
                    elementData.ObjectGraphicId     = value.id;
                    elementData.objectGraphicPath   = value.Path;
                    elementData.objectGraphicIconPath   = value.iconPath;

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

    public void UpdateObjectGraphic(ObjectGraphicDataElement objectGraphicDataElement)
    {
        ObjectGraphic = objectGraphicDataElement;
        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment

    public void InitializeSegment()
    {
        InitializeDependencies();

        InitializeData();

        selectionElement.InitializeElement(selectionElement.GetComponent<IDataController>());

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, index, DataEditor.Data.DataController.DataList.Count - 1);
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    public void InitializeData()
    {
        switch (DataEditor.Data.DataController.DataType)
        {
            case Enums.DataType.Item: InitializeItemData(); break;
            case Enums.DataType.Interactable: InitializeElementData(); break;
        }
    }

    private void InitializeItemData()
    {
        var itemData        = (ItemDataElement)DataEditor.Data.DataElement;

        id                  = itemData.id;
        index               = itemData.Index;
        assetName           = itemData.Name;

        objectGraphicId     = itemData.ObjectGraphicId;
        objectGraphicPath   = itemData.objectGraphicPath;
        objectGraphicIconPath   = itemData.objectGraphicIconPath;

        GetComponent<ObjectProperties>().castShadow = false;
    }

    private void InitializeElementData()
    {
        var elementData     = (InteractableDataElement)DataEditor.Data.DataElement;

        id                  = elementData.id;
        index               = elementData.Index;
        assetName           = elementData.Name;

        objectGraphicId     = elementData.ObjectGraphicId;
        objectGraphicPath   = elementData.objectGraphicPath;
        objectGraphicIconPath   = elementData.objectGraphicIconPath;

        GetComponent<ObjectProperties>().castShadow = true;
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        idText.text = id.ToString();

        inputField.text = assetName;

        var objectGraphicDataElement    = new ObjectGraphicDataElement();

        objectGraphicDataElement.id     = objectGraphicId;
        objectGraphicDataElement.Path   = objectGraphicPath;
        objectGraphicDataElement.iconPath   = objectGraphicIconPath;

        objectGraphicDataElement.SelectionElement = selectionElement;

        selectionElement.route.data.DataController.DataList = new List<IDataElement>() { objectGraphicDataElement };
        selectionElement.route.data.DataElement = objectGraphicDataElement;

        selectionElement.SetElement();

        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        switch(selectionElement.route.data.DataController.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var objectGraphicDataElement = (ObjectGraphicDataElement)selectionElement.route.data.DataElement;

                UpdateObjectGraphic(objectGraphicDataElement);

                break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }

    #endregion
}
