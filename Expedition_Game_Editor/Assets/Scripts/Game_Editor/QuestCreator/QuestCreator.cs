using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class QuestCreator : MonoBehaviour
{
    private string    journalFile;
    
    private string objectiveFile;

    static public            string[]   times       = new string[] { "Day", "Night" };

    static public       List<string>    steps       =   new List<string>();
    static public       List<string>[]  objectives  = { new List<string>(), new List<string>() };
    
    static public bool      addStep;

    static public string    selectedQuest;

    public RectTransform    displaySelection;

    public Text             questName;

    public Dropdown         languageSelection;

    public RectTransform    myList,
                            listParent,
                            numberParent;

    int[] listSize = new int[] { 50, 330 };
   
    public Slider           slider;

    public GameObject       questEditor,
                            objectiveEditor;

    Vector2                 listMin, listMax;

    float                   textDisplaySize = 50;

    void Awake()
    {
        OldEditorManager.SetLanguages(languageSelection);
    }

    void OnEnable()
    {
        OldEditorManager.menu.SetupTabs(   new string[] { "Quests", "Characters", "Items" }, 
                                        new int[] { 2, 0, 0 }, 
                                        new int[0],
                                        new bool[] { true, false, false });

        EditorMenu.listButtons[0].onClick.Invoke();

        OldEditorManager.ActivateLanguage(languageSelection);
    }

    public void SelectQuest(Button selectedButton)
    {
        OldEditorManager.menu.listSelection.transform.position = selectedButton.transform.position;

        string[] questnames = new string[] { "Test Quest2" };

        selectedQuest = questnames[selectedButton.GetComponent<ButtonFormat>().dialogueNumber];

        //selectedQuest = Path.GetFileNameWithoutExtension(Directory.GetFiles(Application.dataPath + "/Resources/Quests")[selectedButton.GetComponent<ButtonFormat>().dialogueNumber]);

        questName.text = selectedQuest;

        InitializeQuest();  
    }

    public void InitializeQuest()
    {
        //Close editors
        questEditor.SetActive(false);
        objectiveEditor.SetActive(false);

        displaySelection.gameObject.SetActive(false);

        myList.offsetMin = new Vector2(myList.offsetMin.x, listSize[0]);

        LoadJournal();
        LoadObjectives();

        SetQuest();
    }

    void LoadJournal()
    {
        steps.Clear();

        ReadJournal();
        
        string[] newEntries = journalFile.Split('$');

        for (int entry = 0; entry < newEntries.Length; entry++)
        {
            if (newEntries[entry].Length == 0)
                break;

            steps.Add(newEntries[entry].Replace("\n", ""));
        }
    }

    void ReadJournal()
    {
        TextAsset myFile = Resources.Load<TextAsset>("Quests/" + selectedQuest + "/" + OldEditorManager.languages[OldEditorManager.activeLanguage] + "_Journal");

        using (StreamReader reader = new StreamReader(new MemoryStream(myFile.bytes)))
        {
            journalFile = reader.ReadToEnd();

            reader.Close();
        }
        /*
        string path = Application.dataPath + "/Resources/Quests/" + selectedQuest + "/" + EditorManager.languages[EditorManager.activeLanguage] + "_Journal.txt";

        StreamReader reader = new StreamReader(path);

        journalFile = reader.ReadToEnd();

        reader.Close();
        */
    }

    void LoadObjectives()
    {
        for (int time = 0; time < times.Length; time++)
        {
            objectives[time].Clear();

            ReadObjectives(times[time]);

            string[] newObjectives = objectiveFile.Split('$');

            for (int objective = 0; objective < newObjectives.Length; objective++)
            {
                if (newObjectives[objective].Length == 0)
                    break;

                objectives[time].Add(newObjectives[objective]);
            }
        }
    }

    void ReadObjectives(string time)
    {
        TextAsset myFile = Resources.Load<TextAsset>("Quests/" + selectedQuest + "/" + time + "/" + OldEditorManager.languages[OldEditorManager.activeLanguage]);

        using (StreamReader reader = new StreamReader(new MemoryStream(myFile.bytes)))
        {
            objectiveFile = reader.ReadToEnd();

            reader.Close();
        }
        /*
        string path = Application.dataPath + "/Resources/Quests/" + selectedQuest + "/" + time + "/" + EditorManager.languages[EditorManager.activeLanguage] + ".txt";

        StreamReader reader = new StreamReader(path);

        objectiveFile = reader.ReadToEnd();

        reader.Close();
        */
    }

    public void SetQuest()
    {
        /*
        characters.Clear();
        rotations.Clear();
        animations.Clear();
        
        speaker.Clear();

        dialogue.Clear();
        */

        for (int i = 0; i < steps.Count; i++)
        {
            /*
            characters.Add(GetStringBetweenChars(scenes[i], "", "&"));
            rotations.Add(GetStringBetweenChars(scenes[i], "&", "@"));
            animations.Add(GetStringBetweenChars(scenes[i], "@", "*"));

            speaker.Add(int.Parse(GetStringBetweenChars(scenes[i], "*", "+")));

            dialogue.Add(scenes[i].Split('+')[1]);
            */
        }

        SetupObjectives();
    }
    
    public void SetupObjectives()
    {
        listParent.sizeDelta = new Vector2(listParent.sizeDelta.x, textDisplaySize * steps.Count);

        listMin = myList.TransformPoint(new Vector2(0, myList.rect.min.y));
        listMax = myList.TransformPoint(new Vector2(0, myList.rect.max.y));

        slider.gameObject.SetActive(listParent.sizeDelta.y > myList.rect.max.y * 2);

        SetQuestDisplay();
    }
    
    public void SetQuestDisplay()
    {
        SetSlider();

        OldEditorManager.ResetButtons(OldEditorManager.textDisplayList);
        OldEditorManager.ResetText(OldEditorManager.textList);

        for (int i = 0; i < steps.Count; i++)
        {
            if (steps[i].Length == 0)
                break;
   
            while (ListPosition(i) < listMax.y)
                i++;

            if (ListPosition(i) > listMin.y)
                break;
    
            Button newDisplay = OldEditorManager.SpawnTextDisplay(OldEditorManager.textDisplayList);
            newDisplay.transform.SetParent(listParent, false);
            newDisplay.transform.localPosition = new Vector2(0, (listParent.sizeDelta.y / 2f) - (textDisplaySize / 2f) - (textDisplaySize * i));

            int tempInt = i;

            newDisplay.onClick.AddListener(delegate { SelectDisplay(newDisplay, tempInt); });

            ListElement textDisplay = newDisplay.GetComponent<ListElement>();
            /*
            textDisplay.bg.sizeDelta = new Vector2(textDisplay.bg.sizeDelta.x, textDisplaySize);

            textDisplay.speakerName.text = "";

            textDisplay.displayText.text = steps[i];
            */
            SetNumbers(i, newDisplay);  
        }       
    }
    
    float ListPosition(int i)
    {
        return listParent.TransformPoint(new Vector2(0, (listParent.sizeDelta.y / 2.25f) - (textDisplaySize * i))).y;
    }

    void SetNumbers(int i, Button newParent)
    {
        Text newDigit = OldEditorManager.SpawnText();
        newDigit.text = (i + 1).ToString();
        newDigit.transform.SetParent(numberParent, false);

        newDigit.transform.localPosition = new Vector2(0, newParent.transform.localPosition.y);
    }
    
    void SetSlider()
    {
        if (slider.gameObject.activeInHierarchy)
            slider.value = Mathf.Clamp(myList.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
    }

    public void SelectDisplay(Button newDisplay, int newObjective)
    {
        if (addStep)
        {
            objectiveEditor.GetComponent<ObjectiveEditor>().CloseEditor();

            if (newObjective >= objectiveEditor.GetComponent<ObjectiveEditor>().curStep)
                newObjective--;
        }
        
        ActivateEditor(newObjective, false);

        SetSelection(newObjective);
    }

    public void SetSelection(int newObjective)
    {
        displaySelection.transform.localPosition = new Vector2(0, (listParent.sizeDelta.y / 2f) - (textDisplaySize / 2f) - (textDisplaySize * newObjective));
        displaySelection.sizeDelta = new Vector2(displaySelection.sizeDelta.x, textDisplaySize);
        displaySelection.gameObject.SetActive(true);
    }

    public void EditQuest(bool isNew)
    {
        
    }
    
    public void AddStep()
    {
        if(!addStep)
            ActivateEditor(steps.Count, true);
    }

    void ActivateEditor(int newObjective, bool addNew)
    {
        objectiveEditor.SetActive(true);
        objectiveEditor.GetComponent<ObjectiveEditor>().SetEditor(newObjective, addNew);

        myList.offsetMin = new Vector2(myList.offsetMin.x, listSize[1]);

        if(addNew)
            myList.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

        SetQuest();
    }
}
