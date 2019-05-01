using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class AssetHeader : MonoBehaviour, ISegment
{
    private SegmentController segmentController;
    public IEditor dataEditor { get; set; }

    #region UI
    public IndexSwitch indexSwitch;
    public SearchElement searchElement;
    public InputField inputField;
    public Text id;
    #endregion

    #region Data Variables
    private int _id;
    private int _index;
    private int _objectGraphicId;
    private string _name;
    private string _objectGraphicName;
    private string _objectGraphicIcon;
    #endregion

    #region Properties
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;

            switch (dataEditor.data.controller.DataType)
            {
                case Enums.DataType.Item:

                    ItemDataElement itemData = dataEditor.data.element.Cast<ItemDataElement>().FirstOrDefault();
                    itemData.Name = value;

                    break;

                case Enums.DataType.Element:

                    ElementDataElement elementData = dataEditor.data.element.Cast<ElementDataElement>().FirstOrDefault();
                    elementData.Name = value;

                    break;
            }
        }
    }

    public ObjectGraphicDataElement ObjectGraphic
    {
        set
        {
            _objectGraphicId    = value.id;
            _objectGraphicName  = value.name;
            _objectGraphicIcon  = value.icon;

            switch (dataEditor.data.controller.DataType)
            {
                case Enums.DataType.Item:

                    ItemDataElement itemData    = dataEditor.data.element.Cast<ItemDataElement>().FirstOrDefault();
                    itemData.ObjectGraphicId    = value.id;
                    itemData.objectGraphicName  = value.name;
                    itemData.objectGraphicIcon  = value.icon;

                    break;

                case Enums.DataType.Element:

                    ElementDataElement elementData  = dataEditor.data.element.Cast<ElementDataElement>().FirstOrDefault();
                    elementData.ObjectGraphicId     = value.id;
                    elementData.objectGraphicName   = value.name;
                    elementData.objectGraphicIcon   = value.icon;

                    break;
            }
        }
    }

    public int ObjectGraphicId
    {
        get { return _objectGraphicId; }
    }
    #endregion

    #region Methods
    public void UpdateName()
    {
        Name = inputField.text;
        dataEditor.UpdateEditor();
    }

    public void UpdateObjectGraphic(ObjectGraphicDataElement objectGraphicDataElement)
    {
        ObjectGraphic = objectGraphicDataElement;
        dataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeSegment()
    {
        segmentController = GetComponent<SegmentController>();
        dataEditor = segmentController.editorController.pathController.dataEditor;

        switch (dataEditor.data.controller.DataType)
        {
            case Enums.DataType.Item:       InitializeItemData();       break;
            case Enums.DataType.Element:    InitializeElementData();    break;
        }

        searchElement.InitializeElement(dataEditor);

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, _index, dataEditor.data.controller.DataList.Count - 1);
    }

    private void InitializeItemData()
    {
        ItemDataElement itemData = dataEditor.data.element.Cast<ItemDataElement>().FirstOrDefault();

        _id                 = itemData.id;
        _index              = itemData.Index;
        _name               = itemData.Name;

        _objectGraphicId    = itemData.ObjectGraphicId;
        _objectGraphicName  = itemData.objectGraphicName;
        _objectGraphicIcon  = itemData.objectGraphicIcon;

        GetComponent<ObjectProperties>().castShadow = false;
    }

    private void InitializeElementData()
    {
        ElementDataElement elementData = dataEditor.data.element.Cast<ElementDataElement>().FirstOrDefault();

        _id                 = elementData.id;
        _index              = elementData.Index;
        _name               = elementData.Name;

        _objectGraphicId    = elementData.ObjectGraphicId;
        _objectGraphicName  = elementData.objectGraphicName;
        _objectGraphicIcon  = elementData.objectGraphicIcon;

        GetComponent<ObjectProperties>().castShadow = true;
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        id.text = _id.ToString();

        inputField.text = _name;

        var objectGraphicDataElement    = new ObjectGraphicDataElement();

        objectGraphicDataElement.id     = _objectGraphicId;
        objectGraphicDataElement.name   = _objectGraphicName;
        objectGraphicDataElement.icon   = _objectGraphicIcon;

        searchElement.SetElement(new[] { objectGraphicDataElement });

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

    public void SetSearchResult(SearchElement searchElement)
    {
        switch(searchElement.route.data.controller.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var objectGraphicDataElement = searchElement.route.data.element.Cast<ObjectGraphicDataElement>().FirstOrDefault();

                UpdateObjectGraphic(objectGraphicDataElement);

                break;

            default: Debug.Log("CASE MISSING"); break;
        }
    }

    #endregion
}
