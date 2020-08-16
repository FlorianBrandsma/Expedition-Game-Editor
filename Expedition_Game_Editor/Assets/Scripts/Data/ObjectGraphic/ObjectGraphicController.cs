using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectGraphicController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public IDataManager DataManager             { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.ObjectGraphic; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IElementData> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public ObjectGraphicController()
    {
        DataManager = new ObjectGraphicDataManager(this);
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var objectGraphicData = (ObjectGraphicElementData)searchElement.data.elementData;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var resultElementData = (ObjectGraphicElementData)resultData;

                objectGraphicData.Id = resultElementData.Id;
                objectGraphicData.IconId = resultElementData.IconId;
                objectGraphicData.Path = resultElementData.Path;
                objectGraphicData.Name = resultElementData.Name;

                objectGraphicData.Height = resultElementData.Height;
                objectGraphicData.Width = resultElementData.Width;
                objectGraphicData.Depth = resultElementData.Depth;

                objectGraphicData.iconPath = resultElementData.iconPath;
                
                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
