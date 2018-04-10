using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TaskEditor : MonoBehaviour, IEditor
{
    public void OpenEditor()
    {
        this.gameObject.SetActive(true);
    }

    public void CloseEditor()
    {
        this.gameObject.SetActive(false);
    }

    private int curStep;

    private  List<string>[] segments        = { new List<string>(), new List<string>() };

    private     List<int>[] interactables   = { new List<int>(),    new List<int>() };
    private           int[] intTrigger      = new int[2];
    
    private List<Vector3>[] intPositions    = { new List<Vector3>(),new List<Vector3>() };
    private     List<int>[] intRotation     = { new List<int>(),    new List<int>() };
    private     List<int>[] intAnim         = { new List<int>(),    new List<int>() };

    private     List<int>[] delayTime       = { new List<int>(),    new List<int>() };
    private     List<int>[] delayYield      = { new List<int>(),    new List<int>() };
    private     List<int>[] delayAnim       = { new List<int>(),    new List<int>() };

    private List<List<string>>[] monologue  = { new List<List<string>>(), new List<List<string>>() };
    private List<List<int>>[] monoAnim      = { new List<List<int>>(), new List<List<int>>() };
    private List<List<int>>[] monoTurn      = { new List<List<int>>(), new List<List<int>>() };

    private           int[] eventType       = new int[2];
    private           int[] eventNumber     = new int[2];
    private           int[] collectAmount   = new int[2];

    private     List<int>[] rewards         = { new List<int>(),    new List<int>() };
    private     List<int>[] rewardAmount    = { new List<int>(),    new List<int>() };

    //public GameObject objectiveEditor;
    public ObjectiveEditor objectiveEditor;
    /*
    private void OnEnable()
    {
        SetEditor();
    }
    */
    public void SetEditor()
    {
        curStep = objectiveEditor.curStep;

        LoadSegments();
    }

    void LoadSegments()
    {
        for (int time = 0; time < QuestCreator.times.Length; time++)
        {
            segments[time].Clear();

            string[] newSegments = QuestCreator.objectives[time][curStep].Split('@');

            for (int segment = 0; segment < newSegments.Length; segment++)
            {
                if (newSegments[segment].Length == 0)
                    break;

                segments[time].Add(newSegments[segment].Replace("\n", ""));
                //Debug.Log(newSegments[segment].Replace("\n", ""));
            }

            LoadGeneral(time);
            //LoadDelay(time);
            //LoadMonologue(time);
            //LoadEvent(time);
            //LoadReward(time);
        }
    }

    void LoadGeneral(int time)
    {
        Debug.Log(time);
        /*
        string[] vectors = elements[2].Split(',')[interactable].Split(':');
        intPositions[time].Add(new Vector3(float.Parse(vectors[0]), float.Parse(vectors[1]), float.Parse(vectors[2])));

        intRotation[time].Add(int.Parse(elements[3].Split(',')[interactable]));

        intAnim[time].Add(int.Parse(elements[4].Split(',')[interactable]));
        */
    }

    /*
    void LoadDelay(int time)
    {
        delayTime[time].Clear();
        delayYield[time].Clear();
        delayAnim[time].Clear();

        int myInteractables = interactables[time].Count;

        for (int interactable = 0; interactable < myInteractables; interactable++)
        {
            string[] elements = segments[time][1].Split('|');

            delayTime[time].Add(int.Parse(elements[0].Split(',')[interactable]));

            delayYield[time].Add(int.Parse(elements[1].Split(',')[interactable]));

            delayAnim[time].Add(int.Parse(elements[2].Split(',')[interactable]));
        }
    }
    void LoadMonologue(int time)
    {
        monologue[time].Clear();
        monoAnim[time].Clear();
        monoTurn[time].Clear();

        //Tel hoeveel interactables er zijn
        int myInteractables = interactables[time].Count;

        for (int interactable = 0; interactable < myInteractables; interactable++)
        {
            //Geef elke interactable een lijst om elementen in te stoppen
            monologue[time].Add(new List<string>());
            monoAnim[time].Add(new List<int>());
            monoTurn[time].Add(new List<int>());

            //Verkrijg de elementen door de monologue segment te splitten
            string[] elements = segments[time][2].Split('|');

            for (int element = 0; element < elements.Length; element++)
            {
                //Split de eerder verkregen element te splitten met interactables
                //Split dit vervolgens om de scenes per element te verkrijgen
                string[] scenes = elements[element].Split(',')[interactable].Split('+');

                for (int scene = 0; scene < scenes.Length; scene++)
                {
                    //Elk element kent zijn eigen lijst
                    //Geef de individuele elementen (verdeeld in scenes) aan een lijst
                    if (element == 0)
                        monologue[time][interactable].Add(scenes[scene]);

                    if (element == 1)
                        monoAnim[time][interactable].Add(int.Parse(scenes[scene]));

                    if (element == 2)
                        monoTurn[time][interactable].Add(int.Parse(scenes[scene]));
                }
            }
        }
    }
    void LoadEvent(int time)
    {
        string[] elements = segments[time][3].Split('|');

        eventType[time] = int.Parse(elements[0]);
        eventNumber[time] = int.Parse(elements[1]);
        collectAmount[time] = int.Parse(elements[2]);
    }
    void LoadReward(int time)
    {
        rewards[time].Clear();
        rewardAmount[time].Clear();

        string[] elements = segments[time][4].Split('|');

        string[] myRewards = elements[0].Split(',');
        string[] myAmounts = elements[1].Split(',');

        for (int reward = 0; reward < myRewards.Length; reward++)
        {
            rewards[time].Add(int.Parse(myRewards[reward]));
            rewardAmount[time].Add(int.Parse(myAmounts[reward]));
        }
    }
    */
}
