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
    private string objectGraphicIcon;
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

                    ItemDataElement itemData = DataEditor.Data.ElementData.Cast<ItemDataElement>().FirstOrDefault();
                    itemData.Name = value;

                    break;

                case Enums.DataType.Element:

                    ElementDataElement elementData = DataEditor.Data.ElementData.Cast<ElementDataElement>().FirstOrDefault();
                    elementData.Name = value;

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
            objectGraphicIcon  = value.Icon;

            switch (DataEditor.Data.DataController.DataType)
            {
                case Enums.DataType.Item:

                    ItemDataElement itemData    = DataEditor.Data.ElementData.Cast<ItemDataElement>().FirstOrDefault();
                    itemData.ObjectGraphicId    = value.id;
                    itemData.objectGraphicPath  = value.Path;
                    itemData.objectGraphicIcon  = value.Icon;

                    break;

                case Enums.DataType.Element:

                    ElementDataElement elementData  = DataEditor.Data.ElementData.Cast<ElementDataElement>().FirstOrDefault();
                    elementData.ObjectGraphicId     = value.id;
                    elementData.objectGraphicPath   = value.Path;
                    elementData.objectGraphicIcon   = value.Icon;

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
        DataEditor = SegmentController.editorController.pathController.dataEditor;

        switch (DataEditor.Data.DataController.DataType)
        {
            case Enums.DataType.Item:       InitializeItemData();       break;
            case Enums.DataType.Element:    InitializeElementData();    break;
        }

        selectionElement.InitializeElement();

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, index, DataEditor.Data.DataController.DataList.Count - 1);
    }

    private void InitializeItemData()
    {
        ItemDataElement itemData = DataEditor.Data.ElementData.Cast<ItemDataElement>().FirstOrDefault();

        id                  = itemData.id;
        index               = itemData.Index;
        assetName           = itemData.Name;

        objectGraphicId     = itemData.ObjectGraphicId;
        objectGraphicPath   = itemData.objectGraphicPath;
        objectGraphicIcon   = itemData.objectGraphicIcon;

        GetComponent<ObjectProperties>().castShadow = false;
    }

    private void InitializeElementData()
    {
        ElementDataElement elementData = DataEditor.Data.ElementData.Cast<ElementDataElement>().FirstOrDefault();

        id                  = elementData.id;
        index               = elementData.Index;
        assetName           = elementData.Name;

        objectGraphicId     = elementData.ObjectGraphicId;
        objectGraphicPath   = elementData.objectGraphicPath;
        objectGraphicIcon   = elementData.objectGraphicIcon;

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
        objectGraphicDataElement.Icon   = objectGraphicIcon;

        selectionElement.SetElement(new[] { objectGraphicDataElement });

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

                var objectGraphicDataElement = selectionElement.route.data.ElementData.Cast<ObjectGraphicDataElement>().FirstOrDefault();

                UpdateObjectGraphic(objectGraphicDataElement);

                break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }

    #endregion
}
