using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ExMultiGrid : MonoBehaviour, IElement, IPoolable
{
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

    private List<EditorElement> elementList = new List<EditorElement>();
    private List<IElementData> dataList     = new List<IElementData>();
    private List<GeneralData> generalDataList;

    public EditorElement EditorElement      { get { return GetComponent<EditorElement>(); } }
    
    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return elementType; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        SelectionElementManager.Add(newElement.EditorElement);

        return newElement;
    }
    
    public void InitializeElement()
    {
        multiGridProperties = (MultiGridProperties)EditorElement.DataElement.DisplayManager.Display.Properties;
    }

    public void UpdateElement()
    {
        SetElement();
    }

    public void SetElement()
    {
        innerGrid.sizeDelta = new Vector2(  EditorElement.RectTransform.sizeDelta.x - multiGridProperties.margin, 
                                            EditorElement.RectTransform.sizeDelta.y - multiGridProperties.margin);

        switch (EditorElement.DataElement.data.dataController.DataType)
        {
            case Enums.DataType.Terrain: SetTerrain(); break;

            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.data.dataController.DataType); break;
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

    private void SetTerrain()
    {
        var elementData = (TerrainElementData)EditorElement.DataElement.data.elementData;

        if(multiGridProperties.SecondaryDataController != null)
        {
            switch (multiGridProperties.SecondaryDataController.DataType)
            {
                case Enums.DataType.TerrainTile: SetTerrainTileData(elementData); break;
                default: Debug.Log("CASE MISSING: " + multiGridProperties.SecondaryDataController.DataType); break;
            }
        }

        if (icon != null)
        {
            if (EditorElement.selectionProperty == SelectionManager.Property.Get)
                iconPath = elementData.iconPath;
            else
                iconPath = elementData.originalIconPath; 
        }

        id = elementData.Id;
        header = elementData.Name;
        baseTilePath = elementData.baseTilePath;
    }

    private void SetTerrainTileData(TerrainElementData terrainData)
    {
        dataList = multiGridProperties.SecondaryDataController.DataList.Cast<TerrainTileElementData>().Where(x => x.TerrainId == terrainData.Id).Distinct().Cast<IElementData>().ToList();

        var searchProperties = multiGridProperties.SecondaryDataController.SearchProperties;
        searchProperties.elementType = Enums.ElementType.CompactTile;

        var searchParameters = searchProperties.searchParameters.Cast<Search.Tile>().First();
        searchParameters.tileSetId = new List<int>() { terrainData.tileSetId };

        SetData(dataList, searchProperties);
    }

    public void SetData(List<IElementData> list, SearchProperties searchProperties)
    {
        generalDataList = list.Cast<GeneralData>().ToList();

        string elementType = Enum.GetName(typeof(Enums.ElementType), multiGridProperties.innerElementType);

        //It's always tile so far
        var prefab = Resources.Load<ExTile>("Elements/UI/" + elementType);

        foreach (IElementData elementData in list)
        {
            var innerElement = (ExTile)PoolManager.SpawnObject(prefab);

            SelectionElementManager.InitializeElement(  innerElement.EditorElement.DataElement, innerGrid,
                                                        EditorElement.DataElement.DisplayManager,
                                                        multiGridProperties.innerSelectionType,
                                                        multiGridProperties.innerSelectionProperty);

            innerElement.EditorElement.parent = EditorElement;

            elementList.Add(innerElement.EditorElement);

            elementData.DataElement = innerElement.EditorElement.DataElement;
            innerElement.EditorElement.DataElement.data = new DataElement.Data(multiGridProperties.SecondaryDataController, elementData, searchProperties);

            //Overwrites dataController set by initialization
            innerElement.EditorElement.DataElement.data.dataController = multiGridProperties.SecondaryDataController;

            //Debugging
            GeneralData generalData = (GeneralData)elementData;
            innerElement.name = generalData.DebugName + generalData.Id;
            //

            SetElement(innerElement.EditorElement);
        }
    }

    void SetElement(EditorElement element)
    {
        element.RectTransform.sizeDelta = multiGridProperties.elementSize;

        int index = generalDataList.FindIndex(x => x.Id == element.DataElement.GeneralData.Id);
        element.transform.localPosition = new Vector2( -((multiGridProperties.elementSize.x * 0.5f) * (Mathf.Sqrt(dataList.Count) - 1)) + (index % Mathf.Sqrt(dataList.Count) * multiGridProperties.elementSize.x),
                                                        -(multiGridProperties.elementSize.y * 0.5f) + (innerGrid.sizeDelta.y / 2f) - (Mathf.Floor(index / Mathf.Sqrt(dataList.Count)) * multiGridProperties.elementSize.y));

        element.gameObject.SetActive(true);

        element.DataElement.SetElement();
        element.SetOverlay();
    }

    public void CloseElement()
    {
        elementList.ForEach(x => PoolManager.ClosePoolObject(x.DataElement.Poolable));
        SelectionElementManager.CloseElement(elementList);
    }

    public void ClosePoolable()
    {
        //gameObject.SetActive(false);
    }
}
