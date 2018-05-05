using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//OLD SCRIPT - DELETE

public class MapGrid : MonoBehaviour
{
    string filePath;

    public Dropdown levelSelection;

    private List<Button> gridTiles = new List<Button>();

    private List<List<int[]>> coordinates = new List<List<int[]>>();

    public RectTransform gridParent;
    public RectTransform grid;

    Vector2 gridSize;

    public RawImage selection;

    private Button selectedTile;
    Vector2 selectedPosition;

    int tileSize = 75;

    Vector2 gridMin, gridMax,
            tileMin, tileMax;

    private void Start()
    {
        OldEditorManager.SetLevels(levelSelection);
    }

    void OnEnable ()
    {
        OldEditorManager.menu.SetupTabs(   new string[] { "Tiles", "Hotspots", "Decors" }, 
                                        new int[] { 1, 1, 1 }, 
                                        new int[] { 0, 1, 2 },
                                        new bool[3]);

        LoadGrid();
    }

    public void LoadGrid()
    {
        Read();

        gridSize = new Vector2(coordinates[0].Count, coordinates.Count);

        gridParent.sizeDelta = new Vector2(gridSize.x * tileSize, gridSize.y * tileSize);
        gridParent.transform.localPosition = new Vector2(gridParent.sizeDelta.x / 2, -gridParent.sizeDelta.y / 2);

        selection.rectTransform.sizeDelta = new Vector2(tileSize, tileSize);

        selectedPosition = Vector2.zero;

        SetGrid();
    }

    void Read()
    {
        /*
        filePath = Application.dataPath + "/Resources/Levels/Level" + EditorManager.activeLevel + ".txt";

        StreamReader reader = new StreamReader(filePath);

        SetCoordinates(reader.ReadToEnd());

        reader.Close();
        */

        //Werkt met inladen bestand op mobiel, maar je kunt niets aanpassen
        TextAsset myFile = Resources.Load<TextAsset>("Levels/Level" + OldEditorManager.activeLevel);

        using (StreamReader reader = new StreamReader(new MemoryStream(myFile.bytes)))
        {
            SetCoordinates(reader.ReadToEnd());

            reader.Close();
        }
    }

    void SetCoordinates(string newLevel)
    {
        coordinates.Clear();

        string[] levelRows = newLevel.Split('\n');

        for (int y = 0; y < levelRows.Length; y++)
        {
            //Add Y
            coordinates.Add(new List<int[]>());

            string[] tiles = levelRows[y].Split('|');

            for (int x = 0; x < tiles.Length; x++)
            {
                string[] tileElems = tiles[x].Split(',');
                //Add X
                coordinates[y].Add(new int[tileElems.Length]);

                for (int i = 0; i < tileElems.Length; i++)
                    coordinates[y][x][i] = int.Parse(tileElems[i]);
            }           
        }
    }

    public void SetGrid()
    {
        OldEditorManager.ResetButtons(gridTiles);
        
        gridMin = grid.TransformPoint(new Vector2(grid.rect.min.x - tileSize * 0.5f, grid.rect.min.y - tileSize * 0.5f));
        gridMax = grid.TransformPoint(new Vector2(grid.rect.max.x + tileSize * 0.5f, grid.rect.max.y + tileSize * 0.5f));

        for (int y = 0; y < coordinates.Count; y++)
        {
            for (int x = 0; x < coordinates[y].Count; x++)
            {
                while (GridPosition(x, y).x < gridMin.x)
                    x++;

                if (GridPosition(x, y).y < gridMax.y ||
                    GridPosition(x, y).y > gridMin.y ||
                    GridPosition(x, y).x > gridMax.x)
                    break;
  
                Button newTile = OldEditorManager.SpawnTile(gridTiles);
                OldEditorManager.SetTile(newTile, coordinates[y][x], new Vector2(x,y));

                newTile.onClick.AddListener(delegate { SelectTile(newTile); });

                newTile.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(tileSize, tileSize);

                newTile.transform.SetParent(gridParent.transform, false);
                newTile.transform.localPosition = new Vector2((tileSize * 0.5f) - (gridParent.sizeDelta.x / 2f) + (x * tileSize), -(tileSize * 0.5f) + (gridParent.sizeDelta.y / 2f) - (y * tileSize));

                //Behoud de eerder gekozen Tile op de juiste positie gebasseerd op gridNumber. Oorspronkelijke Tile staat al lang ergens anders
                if (newTile.GetComponent<TileFormat>().tilePos == selectedPosition)
                    SelectTile(newTile);
            }
        }
        SetDigits();
    }

    Vector2 GridPosition(float x, float y)
    {
        return gridParent.TransformPoint(new Vector2(0 - (gridParent.sizeDelta.x / 2.222f) + x * tileSize, 0 + (gridParent.sizeDelta.y / 2.222f) - y * tileSize));
    }

    void SetDigits()
    {
        OldEditorManager.ResetText(OldEditorManager.textList);

        for (int x = 0; x < gridSize.x; x++)
        {
            while (GridPosition(x, 0).x < gridMin.x)
                x++;
            if (GridPosition(x, 0).x > gridMax.x)
                break;

            Text newDigit = OldEditorManager.SpawnText();
            newDigit.text = (x).ToString();
            newDigit.transform.SetParent(OldEditorManager.manager.numberParent[0].transform, false);

            newDigit.transform.localPosition = new Vector2((tileSize * 0.5f) - (gridParent.sizeDelta.x / 2f) + (x * tileSize), 0);
        }
        OldEditorManager.manager.numberParent[0].transform.localPosition = new Vector2(gridParent.transform.localPosition.x, 0);

        for (int y = 0; y < gridSize.y; y++)
        {
            while (GridPosition(0, y).y < gridMax.y)
                y++;
            if (GridPosition(0, y).y > gridMin.y)
                break;

            Text newDigit = OldEditorManager.SpawnText();
            newDigit.text = (y).ToString();
            newDigit.transform.SetParent(OldEditorManager.manager.numberParent[1].transform, false);

            newDigit.transform.localPosition = new Vector2(0, -(tileSize * 0.5f) + (gridParent.sizeDelta.y / 2f) - (y * tileSize));
        }
        OldEditorManager.manager.numberParent[1].transform.localPosition = new Vector2(0, gridParent.transform.localPosition.y);
    }

    public void SelectTile(Button newTile)
    {
        selectedTile = newTile;
        selectedPosition = newTile.GetComponent<TileFormat>().tilePos;

        selection.transform.position = selectedTile.transform.position;
        selection.transform.SetParent(selectedTile.transform);
    }

    public void SaveTile(Button newTile)
    {
        selectedTile.GetComponent<TileFormat>().tileInfo[EditorMenu.activeTab] = newTile.GetComponent<TileFormat>().tileInfo[EditorMenu.activeTab];

        string newString = "";

        for (int y = 0; y < coordinates.Count; y++)
        {
            for (int x = 0; x < coordinates[y].Count; x++)
            {
                if (x > 0)
                    newString += "|";

                for(int i = 0; i < coordinates[y][x].Length; i++)
                {
                    if (i > 0)
                        newString += ",";

                    newString += coordinates[y][x][i];
                }
            }

            if (y < coordinates.Count - 1)
                newString += "\n";
        }

        //File.WriteAllText(filePath, newString);

        SetGrid();       
    }
}
