using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class AssetHeader : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    #region UI
    public IndexSwitch indexSwitch;
    public SelectionElement selectionElement;
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

            switch (DataEditor.data.DataController.DataType)
            {
                case Enums.DataType.Item:

                    ItemDataElement itemData = DataEditor.data.ElementData.Cast<ItemDataElement>().FirstOrDefault();
                    itemData.Name = value;

                    break;

                case Enums.DataType.Element:

                    ElementDataElement elementData = DataEditor.data.ElementData.Cast<ElementDataElement>().FirstOrDefault();
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
            _objectGraphicName  = value.Name;
            _objectGraphicIcon  = value.Icon;

            switch (DataEditor.data.DataController.DataType)
            {
                case Enums.DataType.Item:

                    ItemDataElement itemData    = DataEditor.data.ElementData.Cast<ItemDataElement>().FirstOrDefault();
                    itemData.ObjectGraphicId    = value.id;
                    itemData.objectGraphicName  = value.Name;
                    itemData.objectGraphicIcon  = value.Icon;

                    break;

                case Enums.DataType.Element:

                    ElementDataElement elementData  = DataEditor.data.ElementData.Cast<ElementDataElement>().FirstOrDefault();
                    elementData.ObjectGraphicId     = value.id;
                    elementData.objectGraphicName   = value.Name;
                    elementData.objectGraphicIcon   = value.Icon;

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

        switch (DataEditor.data.DataController.DataType)
        {
            case Enums.DataType.Item:       InitializeItemData();       break;
            case Enums.DataType.Element:    InitializeElementData();    break;
        }

        selectionElement.InitializeElement();

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, _index, DataEditor.data.DataController.DataList.Count - 1);
    }

    private void InitializeItemData()
    {
        ItemDataElement itemData = DataEditor.data.ElementData.Cast<ItemDataElement>().FirstOrDefault();

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
        ElementDataElement elementData = DataEditor.data.ElementData.Cast<ElementDataElement>().FirstOrDefault();

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
        objectGraphicDataElement.Name   = _objectGraphicName;
        objectGraphicDataElement.Icon   = _objectGraphicIcon;

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
