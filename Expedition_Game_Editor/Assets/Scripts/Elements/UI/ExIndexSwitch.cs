using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ExIndexSwitch : MonoBehaviour, IEditorElement
{
    private ISegment segment;

    private List<IElementData> dataList;

    public Button minusbutton;
    public Button plusButton;

    public Text numberText;

    private int index;
    private int indexLimit;

    private int extraRow;

    public void EnableElement(bool enable)
    {
        minusbutton.interactable = enable;
        plusButton.interactable = enable;
    }

    public void InitializeSwitch(ISegment segment, int index)
    {
        this.segment = segment;

        extraRow = segment.DataEditor.Data.dataController.Data.dataList.Any(x => x.Id == 0) ? 1 : 0;

        indexLimit = segment.DataEditor.Data.dataList.Count - 1;

        this.index = index;

        SetSwitch();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    private void SetSwitch()
    {
        SetIndex();
    }

    public void ChangeIndex(int value)
    {
        index += value;

        if (index + extraRow > indexLimit)
            index = 0;

        if (index + extraRow < extraRow)
            index = indexLimit - extraRow;

        UpdateIndex();
    }

    private void UpdateIndex()
    {
        dataList = segment.DataEditor.Data.dataController.Data.dataList;
        
        var elementData = segment.DataEditor.EditData;
        var elementIndex = dataList.IndexOf(dataList.Where(x => x.Id == elementData.Id).First());

        dataList.RemoveAt(elementIndex);
        dataList.Insert(index + extraRow, elementData);

        for (int i = extraRow; i < dataList.Count; i++)
        {
            switch (elementData.DataType)
            {
                case Enums.DataType.Item:           UpdateItemIndex(i);          break;
                case Enums.DataType.Interactable:   UpdateInteractableIndex(i);  break;
                case Enums.DataType.Region:         UpdateRegionIndex(i);        break;

                case Enums.DataType.Chapter:        UpdateChapterIndex(i);       break;
                case Enums.DataType.Phase:          UpdatePhaseIndex(i);         break;
                case Enums.DataType.Quest:          UpdateQuestIndex(i);         break;
                case Enums.DataType.Objective:      UpdateObjectiveIndex(i);     break;
                case Enums.DataType.Task:           UpdateTaskIndex(i);          break;

                case Enums.DataType.Scene:          UpdateSceneIndex(i);         break;

                default: Debug.Log("CASE MISSING " + elementData.DataType); break;
            }
        }
        
        SelectionElementManager.UpdateElements(elementData);
        SelectionManager.ResetSelection(dataList);

        SetIndex(); 
    }

    private void UpdateItemIndex(int index)
    {
        var elementData = (ItemElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex();
    }

    private void UpdateInteractableIndex(int index)
    {
        var elementData = (InteractableElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex();
    }

    private void UpdateRegionIndex(int index)
    {
        var elementData = (RegionElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex();
    }

    private void UpdateChapterIndex(int index)
    {
        var elementData = (ChapterElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex();
    }

    private void UpdatePhaseIndex(int index)
    {
        var elementData = (PhaseElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex();
    }

    private void UpdateQuestIndex(int index)
    {
        var elementData = (QuestElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex();
    }

    private void UpdateObjectiveIndex(int index)
    {
        var elementData = (ObjectiveElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex();
    }

    private void UpdateTaskIndex(int index)
    {
        var elementData = (TaskElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex();
    }

    private void UpdateSceneIndex(int index)
    {
        var elementData = (SceneElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex();
    }

    public void SetIndex()
    {
        numberText.text = (index + 1).ToString();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
