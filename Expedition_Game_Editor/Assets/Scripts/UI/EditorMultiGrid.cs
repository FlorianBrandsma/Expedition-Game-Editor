using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class EditorMultiGrid : MonoBehaviour, IElement
{
    private List<SelectionElement> elementList = new List<SelectionElement>();

    public Enums.ElementType elementType;

    public RectTransform innerGrid;

    public RectTransform iconParent;
    public RawImage icon;
    public RawImage iconBase;

    public Text idText;
    public Text[] headerText;

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

        switch (Element.route.data.DataController.DataType)
        {
            case Enums.DataType.Terrain: SetTerrainElement(); break;
            default: Debug.Log("CASE MISSING: " + Element.route.data.DataController.DataType); break;
        }
    }

    private void SetTerrainElement()
    {
        Data data = Element.route.data;
        var dataElement = (TerrainDataElement)data.DataElement;

        if (elementType == Enums.ElementType.MultiGrid)
        {
            switch(multiGridProperties.SecondaryDataController.DataType)
            {
                case Enums.DataType.TerrainTile: SetTerrainTileData(dataElement); break;
                default: Debug.Log("MISSING CASE: " + multiGridProperties.SecondaryDataController.DataType); break;
            }
        }

        if (icon != null)
            icon.texture = Resources.Load<Texture2D>(dataElement.iconPath);
    }

    private void SetTerrainTileData(TerrainDataElement terrainData)
    {
        dataList = multiGridProperties.SecondaryDataController.DataList.Cast<TerrainTileDataElement>().Where(x => x.TerrainId == terrainData.id).Distinct().Cast<IDataElement>().ToList();

        SetData(dataList);
    }

    public void SetData(List<IDataElement> list)
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
            element.route.data = new Data(multiGridProperties.SecondaryDataController, data);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.table + generalData.id;
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
