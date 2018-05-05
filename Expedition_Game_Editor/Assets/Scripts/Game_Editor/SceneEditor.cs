using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//OLD SCRIPT - DELETE

public class SceneEditor : MonoBehaviour
{
    public int curScene;
    int selectedCharacter;
    int speaker;

    public RectTransform selection, speakerIcon, rotIcon;
    public Text speakerName, displayText, sceneNumber;

    public InputField inputField;

    public Toggle trashToggle;

    public GameObject characterSelection;

    public Button[] characterSlots;

    int iconSize = 60;

    public GameObject addStepGlow;

    public void SetEditor(int newScene, bool addScene)
    {
        curScene = newScene;

        //Check om te zien of je een scene toevoegt. Zo ja, geef de juiste knoppen weer.
        trashToggle.gameObject.SetActive(!addScene);

        DialogueDeveloper.addScene = addScene;

        if (addScene)
        {
            addStepGlow.SetActive(true);
            //Voeg een tijdelijke scene toe om te bewerken
            //Kopiëer scene als die al bestaat. Maak anders een Default
            if (DialogueDeveloper.scenes.Count > 0)
                DialogueDeveloper.scenes.Add(DialogueDeveloper.scenes[DialogueDeveloper.scenes.Count - 1]);
            else
                DialogueDeveloper.scenes.Add("0,0,0,0,0,0&1,1,1,1,1,1@0,0,0,0,0,0*0+");

            OldEditorManager.dialogueDeveloper.SetDialogue();

            //"Verander Scene" als truc om de nieuwe scene te selecteren. Buitenom zou dit geen toegevoegde scene worden.
            ChangeScene(0);
        }

        SetText();
        SetNumber();
        SetCharacters();
    }

    void SetText()
    {
        inputField.text = DialogueDeveloper.dialogue[curScene];
    }

    public void ChangeScene(int value)
    {
        //Onthoud de laatste scene om te verwisselen
        int prevScene = curScene;
        string tempScene = DialogueDeveloper.scenes[prevScene];

        curScene += value;

        if (curScene < 0)
            curScene = DialogueDeveloper.scenes.Count - 1;
        if (curScene > DialogueDeveloper.scenes.Count - 1)
            curScene = 0;

        //Verwissel scenes
        DialogueDeveloper.scenes.RemoveAt(prevScene);
        DialogueDeveloper.scenes.Insert(curScene, tempScene);

        OldEditorManager.dialogueDeveloper.SetDialogue();
        OldEditorManager.dialogueDeveloper.SetSelection(curScene);

        SetNumber();
    }

    public void SetNumber()
    {
        sceneNumber.text = (curScene + 1).ToString();
    }

    void SetCharacters()
    {
        for (int i = 0; i < characterSlots.Length; i++)
        {
            characterSlots[i].onClick.RemoveAllListeners();

            characterSlots[i].GetComponent<RawImage>().material = new Material(Shader.Find("UI/MultipleTextures"));

            OldEditorManager.SetTile(characterSlots[i], new int[] { int.Parse(DialogueDeveloper.rotations[curScene].Split(',')[i]), int.Parse(DialogueDeveloper.characters[curScene].Split(',')[i]), int.Parse(DialogueDeveloper.animations[curScene].Split(',')[i]) }, Vector2.zero);

            int tempInt = i;

            characterSlots[i].onClick.AddListener(delegate { SelectCharacter(tempInt); });
        }

        SelectCharacter(0);

        speaker = DialogueDeveloper.speaker[curScene];

        SetSpeakerIcon();
    }

    public void SelectCharacter(int newCharacter)
    {
        selectedCharacter = newCharacter;

        SetSelection();

        SetRotation();

        //Open de volgende tab als 0 open staat
        if (EditorMenu.activeTab == 0)
            OldEditorManager.menu.tabs[1].onClick.Invoke();
    }

    void SetSelection()
    {
        characterSelection.transform.localPosition = characterSlots[selectedCharacter].transform.localPosition;
    }

    public void SelectTile(Button newTile)
    {
        int[] newInfo = new int[3];

        if (EditorMenu.activeTab == 1 && newTile.GetComponent<TileFormat>().tileInfo[EditorMenu.activeTab] == 0)
        {
            //Reset Tile als je een leeg karakter selecteerd
        }
        else
        {
            for (int i = 0; i < newInfo.Length; i++)
                newInfo[i] = i == EditorMenu.activeTab ? newTile.GetComponent<TileFormat>().tileInfo[EditorMenu.activeTab] : characterSlots[selectedCharacter].GetComponent<TileFormat>().tileInfo[i];
        }

        OldEditorManager.SetTile(characterSlots[selectedCharacter], newInfo, Vector2.zero);

        if (selectedCharacter == speaker)
            SetSpeakerIcon();
    }


    public void ChangeRotation(int value)
    {
        TileFormat tileFormat = characterSlots[selectedCharacter].GetComponent<TileFormat>();

        tileFormat.tileInfo[0] += value;

        if (tileFormat.tileInfo[0] < 1)
            tileFormat.tileInfo[0] = 8;
        if (tileFormat.tileInfo[0] > 8)
            tileFormat.tileInfo[0] = 1;

        OldEditorManager.SetTile(characterSlots[selectedCharacter], tileFormat.tileInfo, Vector2.zero);

        SetRotation();
    }

    public void SetRotation()
    {
        rotIcon.transform.localEulerAngles = new Vector3(0, 0, characterSlots[selectedCharacter].GetComponent<TileFormat>().tileInfo[0] * 45 - 225);
    }

    public void AssignSpeaker()
    {
        speaker = selectedCharacter;

        SetSpeakerIcon();
    }

    void SetSpeakerIcon()
    {
        speakerIcon.transform.localPosition = new Vector2(characterSlots[speaker].transform.localPosition.x + (iconSize / 4), characterSlots[speaker].transform.localPosition.y + (iconSize / 4));

        SetName();
    }

    void SetName()
    {
        //speakerName.text = Resources.Load<GameObject>("Objects/Characters/" + characterSlots[speaker].GetComponent<TileFormat>().tileInfo[1]).GetComponent<CharacterInfo>().charName[OldEditorManager.activeLanguage];
    }

    public void RemoveScene()
    {
        selection.GetComponent<Image>().color = trashToggle.isOn ? Color.red : Color.green;
    }

    public void SaveText()
    {
        string newString = "";

        if (trashToggle.isOn)
            DialogueDeveloper.scenes.RemoveAt(curScene);

        for (int i = 0; i < DialogueDeveloper.scenes.Count; i++)
        {
            //Add Scene
            if (i > 0)
                newString += "$";

            //Verander volgorde volgens tabs? Rotation, Character, Animation. Scheelt wat regels.
            //Splitters = string[&,@,*]

            //Add changed scene
            if (i == curScene && !trashToggle.isOn)
            {
                //Add Characters
                for (int j = 0; j < characterSlots.Length; j++)
                {
                    if (j > 0)
                        newString += ",";

                    newString += characterSlots[j].GetComponent<TileFormat>().tileInfo[1];
                }

                //Add Rotations
                newString += "&";

                for (int j = 0; j < characterSlots.Length; j++)
                {
                    if (j > 0)
                        newString += ",";

                    newString += characterSlots[j].GetComponent<TileFormat>().tileInfo[0];
                }

                //Add Animations
                newString += "@";

                for (int j = 0; j < characterSlots.Length; j++)
                {
                    if (j > 0)
                        newString += ",";

                    newString += characterSlots[j].GetComponent<TileFormat>().tileInfo[2];
                }
                //Add Speaker
                newString += "*" + speaker.ToString();
                //Add Dialogue
                newString += "+" + displayText.text;

            }
            else
            {
                newString += DialogueDeveloper.scenes[i];
            }
        }

        //File.WriteAllText(DialogueDeveloper.path, newString);

        CloseEditor();
    }

    public void CloseEditor()
    {
        OldEditorManager.menu.tabs[0].onClick.Invoke();

        DialogueDeveloper.addScene = false;
        addStepGlow.SetActive(false);
        OldEditorManager.dialogueDeveloper.InitializeDialogue();
    }
}
