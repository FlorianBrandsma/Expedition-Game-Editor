using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EditorMultiGrid : MonoBehaviour, IElement
{
    private List<SelectionElement> elementList = new List<SelectionElement>();

    public Enums.ElementType elementType;

    public RectTransform innerGrid;

    public RectTransform iconParent;
    public RawImage icon;
    public RawImage iconBase;

    public Text idText;
    public Text headerText;

    private int id;
    private string header;
    private string iconPath;
    private string baseTilePath;

    private MultiGridProperties multiGridProperties;

    private List<IDataElement> dataList = new List<IDataElement>();
    private List<GeneralData> generalDataList;

    private SelectionElement Element { get { return GetComponent<SelectionElement>(); } }

    public void InitializeElement()
    {
        multiGridProperties = Element.ListManager.listProperties.GetComponent<MultiGridProperties>();
    }

    public void SetElement()
    {
        innerGrid.sizeDelta = new Vector2(  Element.RectTransform.sizeDelta.x - multiGridProperties.margin, 
                                            Element.RectTransform.sizeDelta.y - multiGridProperties.margin);

        switch (Element.data.dataController.DataType)
        {
            case Enums.DataType.Terrain: SetTerrainInteractable(); break;
            default: Debug.Log("CASE MISSING: " + Element.data.dataController.DataType); break;
        }

        if (idText != null)
            idText.text = id.ToString();

        if (headerText != null)
            headerText.text = header;

        if (icon != null)
            icon.texture = Resources.Load<Texture2D>(iconPath);

        if (iconBase != null)
            iconBase.texture = Resources.Load<Texture2D>(baseTilePath);
    }

    private void SetTerrainInteractable()
    {
        var data = Element.data;
        var dataElement = (TerrainDataElement)data.dataElement;

        if(multiGridProperties.SecondaryDataController != null)
        {
            switch (multiGridProperties.SecondaryDataController.DataType)
            {
                case Enums.DataType.TerrainTile: SetTerrainTileData(dataElement); break;
                default: Debug.Log("CASE MISSING: " + multiGridProperties.SecondaryDataController.DataType); break;
            }
        }

        if (icon != null)
        {
            if (Element.selectionProperty == SelectionManager.Property.Get)
                iconPath = dataElement.iconPath;
            else
                iconPath = dataElement.originalIconPath; 
        }

        id = dataElement.id;
        header = dataElement.Name;
        baseTilePath = dataElement.baseTilePath;
    }

    private void SetTerrainTileData(TerrainDataElement terrainData)
    {
        dataList = multiGridProperties.SecondaryDataController.DataList.Cast<TerrainTileDataElement>().Where(x => x.TerrainId == terrainData.id).Distinct().Cast<IDataElement>().ToList();

        var searchParameters = multiGridProperties.SecondaryDataController.SearchParameters.Cast<Search.Tile>().FirstOrDefault();
        searchParameters.elementType = Enums.ElementType.CompactTile;

        searchParameters.tileSetId = new List<int>() { terrainData.tileSetId };

        SetData(dataList, new[] { searchParameters });
    }

    public void SetData(List<IDataElement> list, IEnumerable searchParameters)
    {
        generalDataList = list.Cast<GeneralData>().ToList();

        string elementType = Enum.GetName(typeof(Enums.ElementType), multiGridProperties.innerElementType);

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/" + elementType);

        foreach (IDataElement data in list)
        {
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, multiGridProperties.innerElementType,
                                                                            Element.ListManager, multiGridProperties.innerSelectionType, multiGridProperties.innerSelectionProperty, innerGrid);

            element.parent = Element;

            elementList.Add(element);

            data.SelectionElement = element;
            element.data = new SelectionElement.Data(multiGridProperties.SecondaryDataController, data, searchParameters);

            //Overwrites dataController set by initialization
            element.dataController = multiGridProperties.SecondaryDataController;

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.DebugName + generalData.id;
            //

            SetElement(element);
        }
    }

    void SetElement(SelectionElement element)
    {
        RectTransform rect = element.GetComponent<RectTransform>();

        int index = generalDataList.FindIndex(x => x.id == element.GeneralData().id);

        rect.sizeDelta = multiGridProperties.elementSize;

        rect.transform.localPosition = new Vector2(-((multiGridProperties.elementSize.x * 0.5f) * (Mathf.Sqrt(dataList.Count) - 1)) + (index % Mathf.Sqrt(dataList.Count) * multiGridProperties.elementSize.x),
                                                    -(multiGridProperties.elementSize.y * 0.5f) + (innerGrid.sizeDelta.y / 2f) - (Mathf.Floor(index / Mathf.Sqrt(dataList.Count)) * multiGridProperties.elementSize.y));

        rect.gameObject.SetActive(true);

        element.SetElement();
    }

    public void CloseElement()
    {
        SelectionElementManager.CloseElement(elementList);
        //content.offsetMin = new Vector2(10, content.offsetMin.y);
        //content.offsetMax = new Vector2(-10, content.offsetMax.y);

        //headerText.text = string.Empty;
        //idText.text = string.Empty;

        //if (descriptionText != null)
        //    descriptionText.text = string.Empty;

        //if (multiGridProperties.icon)
        //    icon.gameObject.SetActive(false);

        //if (multiGridProperties.edit)
        //    EditButton.gameObject.SetActive(false);
    }
}
