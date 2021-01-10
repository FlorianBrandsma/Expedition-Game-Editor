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
        var dataRequest = new DataRequest()
        {
            requestType = Enums.RequestType.Execute
        };

        dataList = segment.DataEditor.Data.dataController.Data.dataList;
        
        var elementData = segment.DataEditor.EditData;
        var elementIndex = dataList.IndexOf(dataList.Where(x => x.Id == elementData.Id).First());

        dataList.RemoveAt(elementIndex);
        dataList.Insert(index + extraRow, elementData);

        for (int i = extraRow; i < dataList.Count; i++)
        {
            switch (elementData.DataType)
            {
                case Enums.DataType.Item:           UpdateItemIndex(i, dataRequest);            break;
                case Enums.DataType.Interactable:   UpdateInteractableIndex(i, dataRequest);    break;
                case Enums.DataType.Region:         UpdateRegionIndex(i, dataRequest);          break;

                case Enums.DataType.Chapter:        UpdateChapterIndex(i, dataRequest);         break;
                case Enums.DataType.Phase:          UpdatePhaseIndex(i, dataRequest);           break;
                case Enums.DataType.Quest:          UpdateQuestIndex(i, dataRequest);           break;
                case Enums.DataType.Objective:      UpdateObjectiveIndex(i, dataRequest);       break;
                case Enums.DataType.Task:           UpdateTaskIndex(i, dataRequest);            break;

                case Enums.DataType.Scene:          UpdateSceneIndex(i, dataRequest);           break;

                default: Debug.Log("CASE MISSING " + elementData.DataType); break;
            }
        }
        
        SelectionElementManager.UpdateElements(elementData);
        SelectionManager.ResetSelection(dataList);

        SetIndex(); 
    }

    private void UpdateItemIndex(int index, DataRequest dataRequest)
    {
        var elementData = (ItemElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex(dataRequest);
    }

    private void UpdateInteractableIndex(int index, DataRequest dataRequest)
    {
        var elementData = (InteractableElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex(dataRequest);
    }

    private void UpdateRegionIndex(int index, DataRequest dataRequest)
    {
        var elementData = (RegionElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex(dataRequest);
    }

    private void UpdateChapterIndex(int index, DataRequest dataRequest)
    {
        var elementData = (ChapterElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex(dataRequest);
    }

    private void UpdatePhaseIndex(int index, DataRequest dataRequest)
    {
        var elementData = (PhaseElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex(dataRequest);
    }

    private void UpdateQuestIndex(int index, DataRequest dataRequest)
    {
        var elementData = (QuestElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex(dataRequest);
    }

    private void UpdateObjectiveIndex(int index, DataRequest dataRequest)
    {
        var elementData = (ObjectiveElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex(dataRequest);
    }

    private void UpdateTaskIndex(int index, DataRequest dataRequest)
    {
        var elementData = (TaskElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex(dataRequest);
    }

    private void UpdateSceneIndex(int index, DataRequest dataRequest)
    {
        var elementData = (SceneElementData)dataList[index];

        elementData.Index = index - extraRow;

        elementData.UpdateIndex(dataRequest);
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
