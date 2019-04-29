using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class AssetHeader : MonoBehaviour, ISegment
{
    private IDataController dataController { get { return GetComponent<IDataController>(); } }

    private SegmentController segmentController;
    public IEditor dataEditor { get; set; }

    #region UI
    public IndexSwitch indexSwitch;
    public EditorTile iconTile;
    public InputField inputField;
    public Text id;
    #endregion

    #region Data Variables
    private int _id;
    private int _index;
    private int _objectGraphicId;
    private string _name;
    private string _icon;
    #endregion

    #region Properties
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;

            switch (dataEditor.data.controller.data_type)
            {
                case Enums.DataType.Item:

                    ItemDataElement itemData = dataEditor.data.element.Cast<ItemDataElement>().FirstOrDefault();
                    itemData.name = value;

                    break;

                case Enums.DataType.Element:

                    ElementDataElement elementData = dataEditor.data.element.Cast<ElementDataElement>().FirstOrDefault();
                    elementData.name = value;

                    break;
            }
        }
    }

    public int ObjectId
    {
        get { return _objectGraphicId; }
        set
        {
            _objectGraphicId = value;

            ObjectGraphicDataElement objectGraphicDataElement;

            switch (dataEditor.data.controller.data_type)
            {
                case Enums.DataType.Item:

                    ItemDataElement itemData = dataEditor.data.element.Cast<ItemDataElement>().FirstOrDefault();
                    itemData.objectGraphicId = value;

                    dataController.GetData(new List<int>() { itemData.objectGraphicId });

                    objectGraphicDataElement = dataController.dataList.Cast<ObjectGraphicDataElement>().FirstOrDefault();

                    itemData.objectName = objectGraphicDataElement.name;
                    itemData.icon = objectGraphicDataElement.icon;

                    _icon = itemData.icon;

                    SetObject();

                    break;

                case Enums.DataType.Element:

                    ElementDataElement elementData = dataEditor.data.element.Cast<ElementDataElement>().FirstOrDefault();
                    elementData.objectGraphicId = value;

                    dataController.GetData(new List<int>() { elementData.objectGraphicId });

                    objectGraphicDataElement = dataController.dataList.Cast<ObjectGraphicDataElement>().FirstOrDefault();

                    elementData.objectName = objectGraphicDataElement.name;
                    elementData.icon = objectGraphicDataElement.icon;

                    _icon = elementData.icon;

                    SetObject();

                    break;
            }
        }
    }

    #endregion

    #region Methods
    public void UpdateName()
    {
        Name = inputField.text;
        dataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeSegment()
    {
        segmentController = GetComponent<SegmentController>();
        dataEditor = segmentController.editorController.pathController.dataEditor;

        switch (dataEditor.data.controller.data_type)
        {
            case Enums.DataType.Item:       InitializeItemData();       break;
            case Enums.DataType.Element:    InitializeElementData();    break;
        }

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, _index, dataEditor.data.controller.dataList.Count - 1);
    }

    private void InitializeItemData()
    {
        ItemDataElement itemData = dataEditor.data.element.Cast<ItemDataElement>().FirstOrDefault();

        _id                 = itemData.id;
        _index              = itemData.index;
        _name               = itemData.name;
        _objectGraphicId    = itemData.objectGraphicId;

        GetComponent<ObjectProperties>().castShadow = false;
    }

    private void InitializeElementData()
    {
        ElementDataElement elementData = dataEditor.data.element.Cast<ElementDataElement>().FirstOrDefault();

        _id                 = elementData.id;
        _index              = elementData.index;
        _name               = elementData.name;
        _objectGraphicId    = elementData.objectGraphicId;

        GetComponent<ObjectProperties>().castShadow = true;
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        id.text = _id.ToString();

        inputField.text = _name;

        ObjectId = _objectGraphicId;

        gameObject.SetActive(true);
    }

    private void SetObject()
    {
        iconTile.icon.texture = Resources.Load<Texture2D>(_icon);

        GetComponent<IDisplay>().SetDisplay();
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
    #endregion
}
