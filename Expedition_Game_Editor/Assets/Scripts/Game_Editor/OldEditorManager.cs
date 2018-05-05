using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//OLD SCRIPT - DELETE

public class OldEditorManager : MonoBehaviour
{
    static public List<Button> textDisplayList = new List<Button>();
    static public List<Text> textList = new List<Text>();

    public Dropdown editorSelection;
    public GameObject[] editors;
    static public int activeEditor;

    public GameObject buttonSelection;

    static public int activeLevel = 1;

    static public string[] languages = new string[] { "EN", "NL" };
    static public int activeLanguage;

    //Geef aan MapMaker
    public RectTransform[] numberParent;

    public GameObject editorMenu;

    static public OldEditorManager manager;

    static public EditorMenu menu;

    static public MapGrid mapMaker;
    static public QuestCreator questCreator;
    static public DialogueDeveloper dialogueDeveloper;

    void Awake()
    {
        manager = GetComponent<OldEditorManager>();

        menu = editorMenu.GetComponent<EditorMenu>();

        //mapMaker = editors[0].GetComponent<MapGrid>();
        //questCreator = editors[1].GetComponent<QuestCreator>();
        //dialogueDeveloper = editors[2].GetComponent<DialogueDeveloper>();
    }

    void Start()
    {
        LoadEditors();
        //SetEditor(0);
    }
    
    public void LoadEditors()
    {
        for(int editor = 0; editor < editors.Length; editor++)
        {
            editorSelection.options.Add(new Dropdown.OptionData(editors[editor].name));
        }
            
        //editorSelection.captionText.text = editorSelection.options[activeEditor - 1].text;
        //editorSelection.value = activeEditor - 1;
    }

    public void ChangeEditor()
    {
        SetEditor(editorSelection.value);
    }

    public void SetEditor(int newEditor)
    {
        //Kan later misschien weg als elke editor tekst gebruikt
        ResetText(textList);

        editors[activeEditor].SetActive(false);

        activeEditor = newEditor;

        editorSelection.captionText.text = editorSelection.options[activeEditor].text;
        editorSelection.value = activeEditor;

        editors[activeEditor].SetActive(true);     
    }

    static public void SetLevels(Dropdown dropdown)
    {
        int fileCount = 3;
        //int fileCount = (Directory.GetFiles(Application.persistentDataPath + "/Resources/Levels").Length / 2);

        for (int i = 1; i <= fileCount; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData("Level " + i));
        }

        dropdown.captionText.text = dropdown.options[activeLevel - 1].text;
        dropdown.value = activeLevel - 1;
    }

    //Verplaats later naar MapMaker
    public void LoadLevel(Dropdown dropdown)
    {
        activeLevel = dropdown.value + 1;

        if (mapMaker.isActiveAndEnabled)
            mapMaker.LoadGrid();
    }

    public static void SetLanguages(Dropdown dropdown)
    {
        for (int i = 0; i < languages.Length; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData(languages[i]));
        }
    }

    public void SelectLanguage(Dropdown languageSelection)
    {
        activeLanguage = languageSelection.value;

        if(questCreator.isActiveAndEnabled)
            questCreator.InitializeQuest();
        if(dialogueDeveloper.isActiveAndEnabled)
            dialogueDeveloper.InitializeDialogue();
    }

    public static void ActivateLanguage(Dropdown dropdown)
    {
        dropdown.captionText.text = dropdown.options[activeLanguage].text;
        dropdown.value = activeLanguage;
    }

    static public Button SpawnTile(List<Button> tilePool)
    {
        for (int i = 0; i < tilePool.Count; i++)
        {
            if (!tilePool[i].gameObject.activeInHierarchy)
            {
                tilePool[i].gameObject.SetActive(true);

                return tilePool[i];
            }
        }

        Button newTile = Instantiate(Resources.Load<Button>("Editor/Tile"));
        newTile.GetComponent<RawImage>().material = new Material(Shader.Find("UI/MultipleTextures"));

        tilePool.Add(newTile);

        return newTile;
    }

    static public void SetTile(Button newTile, int[] tileInfo, Vector2 tilePos)
    {
        TileFormat tileFormat = newTile.GetComponent<TileFormat>();

        tileFormat.tileInfo = tileInfo;
        tileFormat.tilePos = tilePos;

        SetIcon(newTile);
    }

    static public void SetIcon(Button newTile)
    {
        Material material = newTile.GetComponent<RawImage>().material;

        TileFormat tileFormat = newTile.GetComponent<TileFormat>();

        for (int i = 0; i < 3; i++)
            material.SetTexture("_Tex" + i, Resources.Load<Texture2D>("Textures/" + EditorMenu.activeTabs[EditorMenu.texPath[i]] + "/" + tileFormat.tileInfo[EditorMenu.texPath[i]]));
    }

    static public Button SpawnTextDisplay(List<Button> displayPool)
    {
        for (int i = 0; i < displayPool.Count; i++)
        {
            if (!displayPool[i].gameObject.activeInHierarchy)
            {
                displayPool[i].gameObject.SetActive(true);

                return displayPool[i];
            }
        }

        Button newDisplay = Instantiate(Resources.Load<Button>("Editor/TextDisplay"));
        displayPool.Add(newDisplay);

        return newDisplay;
    }

    static public void InvokeToggle(Toggle toggle)
    {
        toggle.isOn = !toggle.isOn;
    }

    static public void ResetButtons(List<Button> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(false);
            list[i].onClick.RemoveAllListeners();
        }
    }

    static public Text SpawnText()
    {
        for (int i = 0; i < textList.Count; i++)
        {
            if (!textList[i].gameObject.activeInHierarchy)
            {
                textList[i].gameObject.SetActive(true);
                return textList[i];
            }
        }

        Text newText = Instantiate(Resources.Load<Text>("Editor/Text"));
        textList.Add(newText);

        return newText;
    }

    static public void ResetText(List<Text> list)
    {
        for (int i = 0; i < list.Count; i++)
            list[i].gameObject.SetActive(false);
    }
}
