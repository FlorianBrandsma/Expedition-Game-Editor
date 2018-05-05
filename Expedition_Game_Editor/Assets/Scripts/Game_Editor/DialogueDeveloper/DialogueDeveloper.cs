using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//OLD SCRIPT - DELETE

public class DialogueDeveloper : MonoBehaviour
{
    static public string file;
    static public string path;

    static public List<string>  scenes       = new List<string>();

    static public List<string>  characters   = new List<string>();
    static public List<string>  rotations    = new List<string>();
    static public List<string>  animations   = new List<string>();

    static public List<int>     speaker = new List<int>();

    static public List<string>  dialogue = new List<string>();

    static public bool addScene;

    public Dropdown languageSelection;

    public Text dialogueName;

    public RectTransform displaySelection;

    string selectedDialogue;
    
    public RectTransform    myList,
                            listParent,
                            numberParent;

    int[] listSize = new int[] { 50, 150 };

    public Slider slider;

    public GameObject editScreen;

    int[] textDisplaySize   = new int[] { 75, 50 };
    int[] textDisplayHorz   = new int[] { 40, -5 };

    private List<int>     sizeList   = new List<int>();
    private List<Vector2> posList    = new List<Vector2>();

    Vector3 listMin, listMax;

    void Awake()
    {
        OldEditorManager.SetLanguages(languageSelection);
    }

    void OnEnable()
    {
        OldEditorManager.menu.SetupTabs(   new string[] { "Dialogues", "Characters", "Animations" }, 
                                        new    int[] { 2, 1, 1 }, 
                                        new    int[] { 1, 2, 0 }, 
                                        new   bool[] {true, false, false });

        EditorMenu.listButtons[0].onClick.Invoke();

        OldEditorManager.ActivateLanguage(languageSelection);
    }

    public void SelectDialogue(Button selectedButton)
    {
        OldEditorManager.menu.listSelection.transform.position = selectedButton.transform.position;

        string[] dialoguenames = new string[] { "Test Dialogue 1", "Test Dialogue 2" };

        selectedDialogue = dialoguenames[selectedButton.GetComponent<ButtonFormat>().dialogueNumber];

        //selectedDialogue = Path.GetFileNameWithoutExtension(Directory.GetFiles(Application.dataPath + "/Resources/Dialogues")[selectedButton.GetComponent<ButtonFormat>().dialogueNumber]);

        dialogueName.text = selectedDialogue;

        InitializeDialogue();      
    }

    public void InitializeDialogue()
    {
        //Close Editor
        editScreen.GetComponent<SceneEditor>().trashToggle.isOn = false;
        displaySelection.gameObject.SetActive(false);
        editScreen.SetActive(false);

        myList.offsetMin = new Vector2(myList.offsetMin.x, listSize[0]);

        Read();
       
        scenes.Clear();

        string[] newScenes = file.Split('$');

        for (int i = 0; i < newScenes.Length; i++)
        {
            if (newScenes[i].Length == 0)
                break;

            scenes.Add(newScenes[i]);
        }

        SetDialogue();    
    }

    public void SetDialogue()
    {
        characters.Clear();
        rotations.Clear();
        animations.Clear();

        speaker.Clear();

        dialogue.Clear();

        for (int i = 0; i < scenes.Count; i++)
        {
            characters.Add(GetStringBetweenChars(scenes[i], "", "&"));
            rotations.Add(GetStringBetweenChars(scenes[i], "&", "@"));
            animations.Add(GetStringBetweenChars(scenes[i], "@", "*"));

            speaker.Add(int.Parse(GetStringBetweenChars(scenes[i], "*", "+")));

            dialogue.Add(scenes[i].Split('+')[1]);
        }

        SetupScenes();
    }

    public void SetupScenes()
    {
        sizeList.Clear();
        posList.Clear();
        int lastSpeaker = -1;

        int[] newPos = new int[] { textDisplayHorz[0], 0 };

        for (int i = 0; i < scenes.Count; i++)
        {    
            if (i > 0)
                lastSpeaker = speaker[i - 1];

            int newSize = textDisplaySize[speaker[i] != lastSpeaker ? 0 : 1];
            sizeList.Add(newSize);

            newPos[0] = (speaker[i] != lastSpeaker ? SwapPosition(newPos[0]) : newPos[0]);
            newPos[1] += (newSize);
            
            posList.Add(new Vector2(newPos[0], newPos[1] - (newSize / 2f)));                       
        }
        
        if (posList.Count > 0)
        {
            listParent.sizeDelta = new Vector2(listParent.sizeDelta.x, posList[posList.Count - 1].y + textDisplaySize[0]);

            listMin = myList.TransformPoint(new Vector2(0, myList.rect.min.y));
            listMax = myList.TransformPoint(new Vector2(0, myList.rect.max.y));
        } else {
            listParent.sizeDelta = Vector2.zero;
        }

        slider.gameObject.SetActive(listParent.sizeDelta.y > myList.rect.max.y * 2);

        SetSceneDisplay();
    }

    int SwapPosition(int oldPos)
    {
        if (oldPos == textDisplayHorz[0])
            return    textDisplayHorz[1];
        else
            return    textDisplayHorz[0];
    }

    void Read()
    {
        //Werkt met inladen bestand op mobiel, maar je kunt niets aanpassen
        TextAsset myFile = Resources.Load<TextAsset>("Dialogues/" + selectedDialogue + "/" + OldEditorManager.languages[OldEditorManager.activeLanguage]);

        using (StreamReader reader = new StreamReader(new MemoryStream(myFile.bytes)))
        {
            file = reader.ReadToEnd();

            reader.Close();
        }
        /*
        path = Application.dataPath + "/Resources/Dialogues/" + selectedDialogue + "/" + EditorManager.languages[EditorManager.activeLanguage] + ".txt";

        StreamReader reader = new StreamReader(path);

        file = reader.ReadToEnd();

        reader.Close();
        */
    }

    public void SetSceneDisplay()
    {
        SetSlider();

        OldEditorManager.ResetButtons(OldEditorManager.textDisplayList);
        OldEditorManager.ResetText(OldEditorManager.textList);

        int lastSpeaker = -1;

        for (int i = 0; i < scenes.Count; i++)
        {
            if (scenes[i].Length == 0)
                break;

            while (ListPosition(i) < listMax.y)
                i++;

            if (ListPosition(i) > listMin.y)
                break;               
                
            Button newDisplay = OldEditorManager.SpawnTextDisplay(OldEditorManager.textDisplayList);

            newDisplay.transform.SetParent(listParent, false);

            newDisplay.transform.localPosition = new Vector2(posList[i].x, listParent.sizeDelta.y / 2f - (posList[i].y));

            int tempInt = i;

            //newDisplay.group = listParent.GetComponent<ToggleGroup>();
            newDisplay.onClick.AddListener(delegate { SelectDisplay(newDisplay, tempInt); });

            ListElement editor_option = newDisplay.GetComponent<ListElement>();

            if (i > 0)
                lastSpeaker = speaker[i - 1];

            //Name = CharacterSlot > Character > Name
            //string newSpeaker = Resources.Load<GameObject>("Objects/Characters/" + characters[i].Split(',')[speaker[i]]).GetComponent<CharacterInfo>().charName[OldEditorManager.activeLanguage];
            /*
            editor_option.header.text = speaker[i] != lastSpeaker ? newSpeaker : "";
            editor_option.GetComponent<RectTransform>() = new Vector2(editor_option.sizeDelta.x, sizeList[i]);

            editor_option.displayText.text = dialogue[i];
            */
            SetNumbers(i);
        }       
    }

    float ListPosition(int i)
    {
        return listParent.TransformPoint(new Vector2(0, (listParent.sizeDelta.y / 2.222f) - posList[i].y)).y;
    }

    string GetStringBetweenChars(string input, string start, string end)
    {
        int from = input.IndexOf(start) + start.Length;
        int to = input.LastIndexOf(end);

        return input.Substring(from, to - from);
    }

    void SetNumbers(int i)
    {
        Text newDigit = OldEditorManager.SpawnText();
        newDigit.text = (i + 1).ToString();
        newDigit.transform.SetParent(numberParent, false);

        newDigit.transform.localPosition = new Vector2(0, listParent.sizeDelta.y / 2f - (posList[i].y));
    }

    void SetSlider()
    {
        if (slider.gameObject.activeInHierarchy)
            slider.value = Mathf.Clamp(myList.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
    }
    
    public void AddScene()
    {
        if(!addScene)
            ActivateEditor(scenes.Count, true);
        
        //Voeg scene toe aan de lijst in textdisplay. kopie van de laatste (als die er is?)
    }

    public void StartPreview()
    {
        Debug.Log("Start Preview");
    }

    public void SelectDisplay(Button newDisplay, int newScene)
    {
        if (addScene)
        {
            editScreen.GetComponent<SceneEditor>().CloseEditor();
            
            if (newScene >= editScreen.GetComponent<SceneEditor>().curScene)
                newScene--;
        }

        ActivateEditor(newScene, false);  

        SetSelection(newScene);
    }

    public void SetSelection(int newScene)
    {
        displaySelection.transform.localPosition = new Vector2(posList[newScene].x, listParent.sizeDelta.y / 2f - (posList[newScene].y));
        displaySelection.sizeDelta = new Vector2(displaySelection.sizeDelta.x, sizeList[newScene]);
        displaySelection.gameObject.SetActive(true);
    }

    void ActivateEditor(int newScene, bool addNew)
    {
        editScreen.SetActive(true);
        editScreen.GetComponent<SceneEditor>().SetEditor(newScene, addNew);

        myList.offsetMin = new Vector2(myList.offsetMin.x, listSize[1]);

        if (addNew)
            myList.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }
}
