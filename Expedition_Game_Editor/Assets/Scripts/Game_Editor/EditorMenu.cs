using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//OLD SCRIPT - DELETE
public class EditorMenu : MonoBehaviour
{
    private List<Button> menuTiles = new List<Button>();
    static public List<Button> listButtons = new List<Button>();

    public GameObject listSelection;

    public Button[]   tabs;
    public Text[]     tabLabels;
    
    static public string[] activeTabs;
    static public int[] texPath;

    //0:Tiles, 1:Hotspots, 2:Decoration
    static public int activeTab; 
    //0:Empty, 1:Tiles, 2: List
    int activeSortType;

    public RectTransform myList;
    public RectTransform listParent;

    public Slider slider;

    int menuTileSize = 75;
    int buttonSize = 40;

    int maxWidth;

    Vector3 listMin, listMax;

    public GameObject editButtons;

    //private textureAmount[]

    public void SetupTabs(string[] newTabs, int[] sortType, int[] newTexPath, bool[] editable)
    {
        activeTabs = newTabs;
        texPath = newTexPath;
        
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].onClick.RemoveAllListeners();
            
            tabLabels[i].text = newTabs[i];

            int tempInt = i;
            tabs[i].onClick.AddListener(delegate { SetOptions(tempInt, sortType[tempInt], editable[tempInt]); }); 
        }

        tabs[0].onClick.Invoke();
    }

    //Eenmalig
    void SetOptions(int newTab, int sortType, bool editable)
    {
        //Waarom is dit ookalweer een toggle? Invoke is wel handig?
        OldEditorManager.ResetButtons(menuTiles);
        OldEditorManager.ResetButtons(listButtons);

        activeTab = newTab;
        activeSortType = sortType;

        for (int i = 0; i < tabs.Length; i++)
        {
            if (i != newTab)
                tabs[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_O");
            else
                tabs[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_A");
        }

        //Set options to support Grid
        if (sortType == 1)
        {
            int fileCount = 0;

            switch (activeTabs[activeTab])
            {
                case "Tiles":
                    fileCount = 10;
                    break;

                case "Hotspots":
                    fileCount = 4;
                    break;

                case "Decors":
                    fileCount = 4;
                    break;

                case "Characters":
                    fileCount = 4;
                    break;

                case "Animations":
                    fileCount = 6;
                    break;
            }

            //int fileCount = (Directory.GetFiles(Application.dataPath + "/Resources/Textures/" + activeTabs[newTab]).Length / 2);

            listMin = myList.TransformPoint(new Vector2(myList.rect.min.x - menuTileSize * 0.5f, myList.rect.min.y - menuTileSize * 0.5f));
            listMax = myList.TransformPoint(new Vector2(myList.rect.max.x + menuTileSize * 0.5f, myList.rect.max.y + menuTileSize * 0.5f));

            maxWidth = MaxWidth(menuTileSize);

            listParent.sizeDelta = new Vector2(maxWidth * menuTileSize, (fileCount / maxWidth) * menuTileSize);
            listParent.transform.localPosition = new Vector2(0, -listParent.sizeDelta.y / 2); 
        }
        //Set options to support listed Buttons
        if (sortType == 2)
        {
            int fileCount = 0;

            switch (activeTabs[activeTab])
            {
                case "Quests":
                    fileCount = 1;
                    break;

                case "Dialogues":
                    fileCount = 2;
                    break;
            }

            //int fileCount = (Directory.GetFiles(Application.dataPath + "/Resources/" + activeTabs[newTab]).Length);

            listMin = myList.TransformPoint(new Vector2(myList.rect.min.x, myList.rect.min.y - buttonSize * 0.5f));
            listMax = myList.TransformPoint(new Vector2(myList.rect.max.x, myList.rect.max.y + buttonSize * 0.5f));

            maxWidth = MaxWidth(buttonSize);

            listParent.sizeDelta = new Vector2(maxWidth * buttonSize, fileCount * buttonSize);
            listParent.transform.localPosition = new Vector2(0, -listParent.sizeDelta.y / 2);
        }

        listSelection.SetActive(sortType == 2);

        editButtons.SetActive(editable);

        slider.gameObject.SetActive(listParent.sizeDelta.y > myList.rect.max.y * 2);

        SetMenu();
    }

    int MaxWidth(float newSize)
    {
        int x = 0;

        while(-(x * newSize / 2f) + (x * newSize) < myList.rect.max.x)
            x++;
        
        return x - 1;
    }

    //Herhaal
    void SetMenu()
    {
        SetSlider();

        if(activeSortType == 0)
        {
            OldEditorManager.ResetButtons(menuTiles);
            OldEditorManager.ResetButtons(listButtons);
        }
        if(activeSortType == 1)
            SetGrid();
        if (activeSortType == 2)
            SetList();
    }

    void SetSlider()
    {
        if (slider.gameObject.activeInHierarchy)
            slider.value = Mathf.Clamp(myList.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
    }

    void SetGrid()
    {
        OldEditorManager.ResetButtons(menuTiles);

        int fileCount = 0;

        switch(activeTabs[activeTab])
        {
            case "Tiles":
                fileCount = 10;
                break;

            case "Hotspots":
                fileCount = 4;
                break;

            case "Decors":
                fileCount = 4;
                break;

            case "Characters":
                fileCount = 4;
                break;

            case "Animations":
                fileCount = 6;
                break;
        }

        //int fileCount = (Directory.GetFiles(Application.dataPath + "/Resources/Textures/" + activeTabs[activeTab]).Length / 2);
   
        int y = 0;

        for (int x = 0; x < fileCount; x++)
        {
            while (ListPosition(y) < listMax.y)
            {
                y++;
                x += maxWidth;
            }
            if (ListPosition(y) > listMin.y)
                break;

            Button newTile = OldEditorManager.SpawnTile(menuTiles);

            int[] newPath = new int[texPath.Length];
            newPath[activeTab] = x;
            OldEditorManager.SetTile(newTile, newPath, new Vector2(x,y));
            
            if(OldEditorManager.activeEditor == 0)
                newTile.onClick.AddListener(delegate { OldEditorManager.mapMaker.SaveTile(newTile); });

            if (OldEditorManager.activeEditor == 2 && OldEditorManager.dialogueDeveloper.editScreen.activeInHierarchy)
                newTile.onClick.AddListener(delegate { OldEditorManager.dialogueDeveloper.editScreen.GetComponent<SceneEditor>().SelectTile(newTile); });

            newTile.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(menuTileSize, menuTileSize);

            newTile.transform.SetParent(listParent.transform, false);
            newTile.transform.localPosition = new Vector2((menuTileSize * 0.5f) - (listParent.sizeDelta.x / 2f) + (x % maxWidth * menuTileSize), -(menuTileSize * 0.5f) + (listParent.sizeDelta.y / 2f) - (y * menuTileSize));

            if ((x + 1) % maxWidth == 0)
                y++;
        }
    }

    void SetList()
    {
        OldEditorManager.ResetButtons(listButtons);

        string currentTab = activeTabs[activeTab];

        int fileCount = 0;

        switch (activeTabs[activeTab])
        {
            case "Quests":
                fileCount = 1;
                break;

            case "Dialogues":
                fileCount = 2;
                break;
        }

        //int fileCount = (Directory.GetFiles(Application.dataPath + "/Resources/" + currentTab).Length);

        for (int y = 0; y < fileCount; y++)
        {         
            while (ListPosition(y) < listMax.y)
                y++;
            
            if (ListPosition(y) > listMin.y)
                break;
            
            Button newButton = SpawnButton(listButtons);

            string[] fileNames = new string[2];

            if (currentTab == "Quests")
                fileNames = new string[] { "Test Quest2", "" };

            if (currentTab == "Dialogues")
                fileNames = new string[] { "Test Dialogue 1", "Test Dialogue 2" };

            string fileName = fileNames[y];

            //string fileName = Path.GetFileNameWithoutExtension(Directory.GetFiles(Application.dataPath + "/Resources/" + currentTab)[y]);

            SetButton(newButton, fileName, y);
 
            if (OldEditorManager.manager.editors[1].activeInHierarchy)
                newButton.onClick.AddListener(delegate { OldEditorManager.questCreator.SelectQuest(newButton); });
            if (OldEditorManager.manager.editors[2].activeInHierarchy)
                newButton.onClick.AddListener(delegate { OldEditorManager.dialogueDeveloper.SelectDialogue(newButton); });

            newButton.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(newButton.GetComponent<Image>().rectTransform.sizeDelta.x, buttonSize);          
            
            newButton.transform.SetParent(listParent.transform, false);
            newButton.transform.localPosition = new Vector2(0, -(newButton.GetComponent<Image>().rectTransform.sizeDelta.y * 0.5f) + (listParent.sizeDelta.y / 2f) - (y * newButton.GetComponent<Image>().rectTransform.sizeDelta.y));          
        }
    }

    float ListPosition(float y)
    {
        return listParent.TransformPoint(new Vector2(0, (listParent.sizeDelta.y / 2.222f) - y * menuTileSize)).y;
    }

    static public Button SpawnButton(List<Button> buttonPool)
    {
        for (int i = 0; i < buttonPool.Count; i++)
        {
            if (!buttonPool[i].gameObject.activeInHierarchy)
            {
                buttonPool[i].gameObject.SetActive(true);

                return buttonPool[i];
            }
        }

        Button newButton = Instantiate(Resources.Load<Button>("Editor/ListButton"));
        buttonPool.Add(newButton);

        return newButton;
    }

    static public void SetButton(Button newButton, string label, int newNumber)
    {
        ButtonFormat buttonFormat = newButton.GetComponent<ButtonFormat>();

        buttonFormat.label.text = label;
        buttonFormat.dialogueNumber = newNumber;
    }
}
