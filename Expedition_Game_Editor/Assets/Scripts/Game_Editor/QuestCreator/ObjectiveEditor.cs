using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ObjectiveEditor : MonoBehaviour
{
    public ScrollRect mainList;
    public RectTransform listParent;

    public int curStep;

    //Objective info. Dubbele array. Time[0] = Day, Time[1] = Night
    private int times = 2;

    private List<int> interactables = new List<int>();
    private int intTrigger;
    private List<string> tasks      = new List<string>();

    private  List<string>[] segments        = { new List<string>(), new List<string>() };

    //Yield
    private List<Vector3>[] intPositions    = { new List<Vector3>(),new List<Vector3>() };
    private     List<int>[] intRotation     = { new List<int>(),    new List<int>() };
    private     List<int>[] intAnim         = { new List<int>(),    new List<int>() };

    private     List<int>[] delayTime       = { new List<int>(),     new List<int>() };
    private     List<int>[] delayYield      = { new List<int>(),     new List<int>() };
    private     List<int>[] delayAnim       = { new List<int>(),     new List<int>() };

    private List<List<string>>[] monologue  = { new List<List<string>>(), new List<List<string>>() };
    private List<List<int>>[] monoAnim      = { new List<List<int>>(),    new List<List<int>>() };
    private List<List<int>>[] monoTurn      = { new List<List<int>>(),    new List<List<int>>() };

    private           int[] eventType       = new int[2];
    private           int[] eventNumber     = new int[2];
    private           int[] collectAmount   = new int[2];

    private     List<int>[] rewards         = { new List<int>(),    new List<int>() };
    private     List<int>[] rewardAmount    = { new List<int>(),    new List<int>() };



    public Slider mainSlider;

    public RectTransform editorParent;

    //General Editor, Task Editor
    public GameObject[] editors;
    private int activeEditor;

    float[] editorWidth = new float[] { -115f, -7.5f };

    //Journal, Interactables
    public Image[] subEditorTabs;
    public GameObject[] subEditors;
    private int activeSubEditor = 0;
    
    //Journal
    //public RectTransform selection, speakerIcon, rotIcon;
    public Text journalText, stepNumber;

    public InputField inputField;
    
    //Interactables

    public Toggle trashToggle;

    public GameObject addStepGlow;

    public void SetSlider()
    {
        mainSlider.value = Mathf.Clamp(mainList.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
    }

    //Translate txt info to variables
    public void SetEditor(int newStep, bool addStep)
    {
        curStep = newStep;

        trashToggle.gameObject.SetActive(!addStep);

        QuestCreator.addStep = addStep;

        if (addStep)
        {
            addStepGlow.SetActive(true);
            //Voeg een tijdelijke scene toe om te bewerken
            //Kopiëer scene als die al bestaat. Maak anders een Default
            if (QuestCreator.steps.Count > 0)
            {
                QuestCreator.steps.Add(QuestCreator.steps[QuestCreator.steps.Count - 1]);

                for (int time = 0; time < times; time++) 
                    QuestCreator.objectives[time].Add(QuestCreator.objectives[time][QuestCreator.objectives[time].Count - 1]);

            } else {

                QuestCreator.steps.Add("New part of the quest.");

                for (int time = 0; time < times; time++)
                    QuestCreator.objectives[time].Add("0|0|0|0|0@0,0,0@Empty|0,0|0,0@0|0|0@0,0|0,0");
            }
            OldEditorManager.questCreator.SetQuest();

            //"Verander Scene" als truc om de nieuwe scene te selecteren. Buitenom zou dit geen toegevoegde scene worden.
            ChangeStep(0);
        }

        LoadInteractables();

        //Day/Night tabs
        SelectEditor(0);
        //Journal
        SetText();
        SetNumber();
    }

    void LoadInteractables()
    {
        int index = QuestCreator.objectives[0][curStep].IndexOf('<');
        string[] elements = QuestCreator.objectives[0][curStep].Substring(0, index).Split('|');

        interactables.Clear();
        /*
        intPositions[time].Clear();
        intRotation[time].Clear();
        intAnim[time].Clear();
        */
        intTrigger = int.Parse(elements[1]);

        int myInteractables = elements[0].Split(',').Length;

        for (int interactable = 0; interactable < myInteractables; interactable++)
        {
            interactables.Add(int.Parse(elements[0].Split(',')[interactable]));

            Debug.Log(interactables[interactable] + ":" + intTrigger);
            
            LoadTasks(interactable);
        }          
    }

    void LoadTasks(int interactable)
    {

    }

    string GetStringBetweenChars(string input, string start, string end)
    {
        int from = input.IndexOf(start) + start.Length;
        int to = input.LastIndexOf(end);

        return input.Substring(from, to - from);
    }
    
    public void SelectEditor(int newEditor)
    {
        subEditors[activeSubEditor].SetActive(false);

        activeSubEditor = newEditor;

        for (int i = 0; i < subEditorTabs.Length; i++)
        {
            if (i != newEditor)
                subEditorTabs[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_S2_O");
            else
                subEditorTabs[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Buttons/Tab_S2_A");
        }

        subEditors[activeSubEditor].SetActive(true);

        SetSegments();
    }
    
    //Set segments according to variables
    void SetSegments()
    {

    }
    //Start Journal
    void SetText()
    {
        inputField.text = QuestCreator.steps[curStep];
    }

    public void ChangeStep(int value)
    {
        //Onthoud de laatste scene om te verwisselen
        int prevStep = curStep;
        
        curStep += value;

        if (curStep < 0)
            curStep = QuestCreator.steps.Count - 1;
        if (curStep > QuestCreator.steps.Count - 1)
            curStep = 0;

        //Verwissel Journal Entries
        string tempStep = QuestCreator.steps[prevStep];

        QuestCreator.steps.RemoveAt(prevStep);
        QuestCreator.steps.Insert(curStep, tempStep);

        //Verwissel Objectives
        for (int time = 0; time < times; time++) 
        {
            tempStep = QuestCreator.objectives[time][prevStep];

            QuestCreator.objectives[time].RemoveAt(prevStep);
            QuestCreator.objectives[time].Insert(curStep, tempStep);
        }

        OldEditorManager.questCreator.SetQuest();
        OldEditorManager.questCreator.SetSelection(curStep);

        SetNumber();
    }

    public void SetNumber()
    {
        stepNumber.text = (curStep + 1).ToString();
    }
    //End Journal
    
    public void ActivateEditor(int newEditor)
    {
        editors[activeEditor].SetActive(false);

        activeEditor = newEditor;

        editorParent.offsetMin = new Vector2(editorWidth[activeEditor], editorParent.offsetMin.y);

        editors[activeEditor].SetActive(true);

        //SetQuest();
    }
    
    public void CloseEditor()
    {
        QuestCreator.addStep = false;
        addStepGlow.SetActive(false);
        OldEditorManager.questCreator.InitializeQuest();
    }
}
