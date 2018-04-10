using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class OverworldScript : MonoBehaviour
{
    string levelString;

    private int activeGrid = 0;

    private List<List<int[]>> baseGrid = new List<List<int[]>>();

    private List<List<List<int[]>>> availableGrids = new List<List<List<int[]>>>();
    private List<List<int[]>>[] assignedGrids = new List<List<int[]>>[] { new List<List<int[]>>(), new List<List<int[]>>(), new List<List<int[]>>(), new List<List<int[]>>() };

    private List<List<GameObject>>[] objectPool = { new List<List<GameObject>>(), new List<List<GameObject>>(), new List<List<GameObject>>() };
    private List<List<GameObject>>  gridObjects =   new List<List<GameObject>>();

    private Vector2 gridSize;
    private List<Vector2> startPos = new List<Vector2>();

    public RawImage sandstorm;
    public RawImage sand;
    //Color stormColor;
    float stormIntensity = 0;

    public float distMeter;
    public Vector2 distPoint;
    bool baseActive;

    public GameObject tileParent, tempParent, camParent;

    float tileSize = 31.75f;
    
    void Start()
    {
        tempParent.SetActive(false);

        LoadLevel(1);
    }

    void LoadLevel(int newLevel)
    {
        Read(newLevel);

        SetCoordinates(levelString, assignedGrids[0]);

        gridSize = new Vector2(baseGrid[0].Count, baseGrid.Count);

        distPoint = new Vector2(0,0);

        SpawnGrid(new Vector2(-(((gridSize.x - (gridSize.x % 2)) / 2) * tileSize), (((gridSize.y - (gridSize.y % 2)) / 2) * tileSize)), 0);

        CheckGrids();
    }

    void Read(int newLevel)
    {
        //Werkt met inladen bestand op mobiel, maar je kunt niets aanpassen
        TextAsset levelFile = Resources.Load<TextAsset>("Levels/Level" + newLevel);

        using (StreamReader reader = new StreamReader(new MemoryStream(levelFile.bytes)))
        {
            levelString = reader.ReadToEnd();
            
            reader.Close();
        }
    }

    void SetCoordinates(string newString, List<List<int[]>> newList)
    {
        newList.Clear();

        string[] gridRows = newString.Split('\n');

        for (int y = 0; y < gridRows.Length; y++)
        {
            //Add Y
            newList.Add(new List<int[]>());

            string[] tiles = gridRows[y].Split('|');

            for (int x = 0; x < tiles.Length; x++)
            {
                string[] tileElems = tiles[x].Split(',');
                //Add X
                newList[y].Add(new int[tileElems.Length]);

                for (int i = 0; i < tileElems.Length; i++)
                    newList[y][x][i] = int.Parse(tileElems[i]);
            }
        }

        baseGrid = newList;
    }

    void Update()
    {
        Vector3 oldPos = camParent.transform.position;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            camParent.transform.Translate(Vector3.left);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            camParent.transform.Translate(Vector3.right);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            camParent.transform.Translate(Vector3.forward);

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            camParent.transform.Translate(Vector3.back);
        
        if (camParent.transform.position != oldPos)
            UpdateGrid();
        
        UpdateStorm();  
    }

    public void UpdateGrid()
    {
        ResetObjects(gridObjects);
        gridObjects.Clear();

        //Reset Grids buiten scherm
        for (int i = 0; i < startPos.Count; i++)
        {
            //In volgorde: Te ver rechts || te ver links || te ver omhoog || te ver omlaag
            if (camParent.transform.position.x < startPos[i].x - ((gridSize.x - 2.67f) * tileSize) || camParent.transform.position.x > startPos[i].x + ((gridSize.x + 1.33f) * tileSize) ||
                camParent.transform.position.z > startPos[i].y + ((gridSize.y - 2.67f) * tileSize) || camParent.transform.position.z < startPos[i].y - ((gridSize.y + 1.33f) * tileSize))
            {
                assignedGrids[i].Clear();
                
                break;
            }
        }

        //vind het actieve grid (waar je op het moment op staat)
        for (int i = 0; i < startPos.Count; i++)
        {
            if (startPos[i] == new Vector2(0, 0))
                i++;

            if (camParent.transform.position.x > (startPos[i].x - (tileSize / 2)) && camParent.transform.position.x < (startPos[i].x + ((gridSize.x - 0.5f) * tileSize)) &&
                camParent.transform.position.z < (startPos[i].y + (tileSize / 2)) && camParent.transform.position.z > (startPos[i].y - ((gridSize.y - 0.5f) * tileSize)))
            {
                activeGrid = i;

                break;
            }
        }

        SpawnGrid(new Vector2(startPos[activeGrid].x, startPos[activeGrid].y), activeGrid);

        //Als je de grens enkel rechts ziet
        if (camParent.transform.position.x > (startPos[activeGrid].x + ((gridSize.x - 2.33f) * tileSize)))
            SpawnGrid(new Vector2(startPos[activeGrid].x + (gridSize.x * tileSize), startPos[activeGrid].y), 1);
        //Als je de grens enkel links ziet
        if (camParent.transform.position.x < (startPos[activeGrid].x + (tileSize * 1.33f)))
            SpawnGrid(new Vector2(startPos[activeGrid].x - (gridSize.x * tileSize), startPos[activeGrid].y), 1);
        //Als je de grens boven ziet
        if (camParent.transform.position.z > (startPos[activeGrid].y - (tileSize * 1.33f)))
        {
            SpawnGrid(new Vector2(startPos[activeGrid].x, startPos[activeGrid].y + (gridSize.y * tileSize)), 2);

            //Rechts
            if (camParent.transform.position.x > (startPos[activeGrid].x + ((gridSize.x - 2.33f) * tileSize)))
                SpawnGrid(new Vector2(startPos[activeGrid].x + (gridSize.x * tileSize), startPos[activeGrid].y + (gridSize.y * tileSize)), 3);

            //Links
            if (camParent.transform.position.x < (startPos[activeGrid].x + (tileSize * 1.33f)))
                SpawnGrid(new Vector2(startPos[activeGrid].x - (gridSize.x * tileSize), startPos[activeGrid].y + (gridSize.y * tileSize)), 3);
        }

        //Als je de grens beneden ziet
        if (camParent.transform.position.z < (startPos[activeGrid].y - ((gridSize.y - 2.33f) * tileSize)))
        {
            SpawnGrid(new Vector2(startPos[activeGrid].x, startPos[activeGrid].y - (gridSize.y * tileSize)), 2);

            //Rechts
            if (camParent.transform.position.x > (startPos[activeGrid].x + (gridSize.x - 2.33f) * tileSize))
                SpawnGrid(new Vector2(startPos[activeGrid].x + (gridSize.x * tileSize), startPos[activeGrid].y - (gridSize.y * tileSize)), 3);

            //Links
            if (camParent.transform.position.x < startPos[activeGrid].x + (tileSize * 1.33f))
                SpawnGrid(new Vector2(startPos[activeGrid].x - (gridSize.x * tileSize), startPos[activeGrid].y - (gridSize.y * tileSize)), 3);
        }

        CheckDistance();

        //Check of startzone binnen bereik is
        //Bereken afstand tussen middelpunt startzone en huidige positie.
        //Enter bufferzone op bepaalde afstand
        //Stop teller in bufferzone
        //Start teller wanneer je de bufferzone verlaat 
    }

    void ResetObjects(List<List<GameObject>> objectPool)
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            for (int j = 0; j < objectPool[i].Count; j++)
                objectPool[i][j].SetActive(false);        
        }
    }

    void CheckDistance()
    {
        //Alleen als startzone in assignedGrids staat
        //Gemaakt met vierkante grids in gedachten
        float dist = Mathf.Max(Mathf.Abs(distPoint.x - camParent.transform.position.x), Mathf.Abs(distPoint.y - camParent.transform.position.z - 10));

        if (baseActive)
        {
            if (dist > (gridSize.x * tileSize / 2))
            {
                //Debug.Log("You have are in the Bufferzone " + dist);
                stormIntensity = 0.35f;
            }
            else
            {
                //Reset counter
                distMeter = 0;
                stormIntensity = 0;
                //Debug.Log("You are in the Startzone " + dist);
            }
        } else
        {
            if (dist > 50)
                SetCheckpoint(dist);
        }
    }

    void SetCheckpoint(float dist)
    {
        //Add to total
        distMeter += dist;
        //Create new checkpoint (later character position)
        distPoint = new Vector2(camParent.transform.position.x, camParent.transform.position.z + 10);

        if(distMeter < 1000)
            stormIntensity = 0.35f + (distMeter / 4000);
    }

    void UpdateStorm()
    {
        sand.uvRect = new Rect(sand.uvRect.x - (0.25f + (stormIntensity * 3f)) * Time.deltaTime, sand.uvRect.y + (0.25f + (stormIntensity * 1.5f)) * Time.deltaTime, sand.uvRect.width, sand.uvRect.height);
        Color stormColor = Color.Lerp(sandstorm.material.color, new Color(RenderSettings.fogColor.r, RenderSettings.fogColor.g, RenderSettings.fogColor.b, stormIntensity), 0.025f);
        sandstorm.material.color = stormColor;
    }

    //Vast Array ipv. List, zodat grids altijd op dezelfde plek blijven
    void SpawnGrid(Vector2 newPos, int direction)
    {
        int gridNumber = activeGrid;
        
        if (gridObjects.Count > 0)
        {
            //0
            if (activeGrid == 0)
                gridNumber = direction;

            //INDIEN MOGELIJK: hier een slimme berekening voor maken a-la //testNumber = (((activeGrid + 1) % 3) == 1) ? activeGrid + gridNumber : activeGrid - gridNumber;
            //Theorie op "coding train"
            //1
            if (activeGrid == 1)
            {
                if (direction == 2)
                    gridNumber = 3;
                if (direction == 1)
                    gridNumber = 0;
                if (direction == 3)
                    gridNumber = 2;
            }
            //2
            if (activeGrid == 2)
            {
                if (direction == 1)
                    gridNumber = 3;
                if (direction == 2)
                    gridNumber = 0;
                if (direction == 3)
                    gridNumber = 1;
            }
            //3
            if (activeGrid == 3)
            {
                if (direction == 1)
                    gridNumber = 2;
                if (direction == 2)
                    gridNumber = 1;
                if (direction == 3)
                    gridNumber = 0;
            }
        }
        
        //Voegt grid posities toe aan lijst
        while (startPos.Count <= gridNumber)
            startPos.Add(new Vector2(0, 0));

        startPos[gridNumber] = newPos;

        if (assignedGrids[gridNumber].Count == 0)
        {
            int randomGrid = Random.Range(0, availableGrids.Count);

            //Assign Grid
            assignedGrids[gridNumber] = availableGrids[randomGrid];

            availableGrids.RemoveAt(randomGrid);

            CheckGrids(); //Stond eerst hoger. Naar beneden geschoven voor bufferzone reset
        }

        gridObjects.Add(new List<GameObject>());

        SetGrid(startPos[gridNumber], assignedGrids[gridNumber]);    
    }

    void CheckGrids()
    {
        bool gridFound = false;

        Debug.Log("Check grids...");

        //Check AvailableGrids en AssignedGrids voor OriginalGrid
        if (!gridFound)
        {
            Debug.Log("Searching for OriginalGrid...");

            for (int i = 0; i < assignedGrids.Length; i++)
            {
                if (assignedGrids[i] == baseGrid)
                {
                    gridFound = true;
                    Debug.Log("Found original grid in AssignedGrids: " + i);

                    distPoint = new Vector2(startPos[i].x + (((gridSize.x - (gridSize.x % 2)) / 2) * tileSize), startPos[i].y - (((gridSize.y - (gridSize.y % 2)) / 2) * tileSize));
                    baseActive = true;

                    break;
                }
            }
        }

        if (!gridFound)
        {
            for (int i = 0; i < availableGrids.Count; i++)
            {
                if (availableGrids[i] == baseGrid)
                {
                    gridFound = true;
                    Debug.Log("Found original grid in AvailableGrids");
                    break;
                }
            }
        }
        
        if (!gridFound)
        {
            baseActive = false;

            Debug.Log("Couldn't find grid. Add OriginalGrid");

            availableGrids.Add(new List<List<int[]>>());
            SetCoordinates(levelString, availableGrids[availableGrids.Count - 1]);
        }
        
        while(availableGrids.Count < 4)
            availableGrids.Add(RandomGrid(availableGrids.Count));    
    }

    List<List<int[]>> RandomGrid(int grid)
    {
        int[] objectCount = new int[] { 1, 0, 0 };

        int[] newInfo = new int[objectCount.Length];

        List<List<int[]>> randomGrid = new List<List<int[]>>();

        for (int y = 0; y < baseGrid.Count; y++)
        {
            randomGrid.Add(new List<int[]>());

            for (int x = 0; x < baseGrid[y].Count; x++)
            {
                randomGrid[y].Add(new int[newInfo.Length]);

                for (int i = 0; i < newInfo.Length; i++)
                    randomGrid[y][x][i] = Random.Range(0, objectCount[i]);
            }
        }

        return randomGrid;
    }

    void SetGrid(Vector2 newPos, List<List<int[]>> newGrid)
    {
        for (int y = 0; y < newGrid.Count; y++)
        {
            for (int x = 0; x < newGrid[y].Count; x++)
            {
                //Grid grens: hoger dan Z 2.33 = tevoorschijn, lager dan Z 0.5f = tevoorschijn

                //Hoger dan Z 2.33 = weg                                                                //Lager dan Z 0.5f = weg
                if (newPos.y - (y * tileSize) > (Camera.main.transform.position.z + (tileSize * 2.33f)) || newPos.y - (y * tileSize) < (Camera.main.transform.position.z - (tileSize * 0.5f)) ||
                    (((newPos.x + (x * tileSize)) - Camera.main.transform.position.x) / tileSize) - 1.25f > (((newPos.y -(y * tileSize)) - Camera.main.transform.position.z) / tileSize) )
                    break;

                //Afstand tussen Tile X en Cam X, gedeeld door grootte van Tile, plus 1.2 (voor speling) kleiner dan -Tile Z en Cam Z, gedeeld door grootte van Tile
                while ((((newPos.x + (x * tileSize)) - Camera.main.transform.position.x) / tileSize) + 1.25f <  -(((newPos.y - (y * tileSize)) - Camera.main.transform.position.z) / tileSize))
                    x++;

                if (x >= newGrid[y].Count)
                    break;

                int[] tileInfo = newGrid[y][x];

                GameObject newTile = SpawnObject(tileInfo[0], 0);
                gridObjects[gridObjects.Count - 1].Add(newTile);

                newTile.transform.parent = tileParent.transform;
                newTile.transform.localPosition = new Vector3(newPos.x + (x * tileSize), newTile.transform.position.y, newPos.y - (y * tileSize));

                for(int i = 1; i < objectPool.Length; i++)
                {
                    if (tileInfo[i] == 0)
                        break;

                    GameObject newObject = SpawnObject(tileInfo[i], i);

                    newObject.transform.position = newTile.transform.position + newTile.GetComponent<TileScript>().objectPos[i - 1];
                    gridObjects[gridObjects.Count - 1].Add(newObject);
                }
            }
        }
    }
    
    GameObject SpawnObject(int objectNumber, int newType)
    {
        while (objectPool[newType].Count <= objectNumber)
            objectPool[newType].Add(new List<GameObject>());
      
        for (int i = 0; i < objectPool[newType][objectNumber].Count; i++)
        {
            if (!objectPool[newType][objectNumber][i].activeInHierarchy)
            {
                objectPool[newType][objectNumber][i].SetActive(true);
                
                return objectPool[newType][objectNumber][i];   
            }
        }

        string[] types = new string[] { "Tile", "Hotspot", "Decor" };

        GameObject newObject = Instantiate(Resources.Load<GameObject>("Objects/" + types[newType] + "/" + objectNumber));

        objectPool[newType][objectNumber].Add(newObject);

        return newObject;
    }
}
