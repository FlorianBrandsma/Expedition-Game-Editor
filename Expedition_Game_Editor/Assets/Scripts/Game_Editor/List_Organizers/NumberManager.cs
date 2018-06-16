using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NumberManager : MonoBehaviour
{
    private List<Text>      number_list = new List<Text>();

    public RectTransform    horizontal_number_parent,
                            vertical_number_parent;

    RectTransform main_list, list_parent;

    ListManager listManager;

    public void InitializeNumbers(ListManager new_listManager, RectTransform new_main_list, RectTransform new_list_parent)
    {
        listManager = new_listManager;
        main_list = new_main_list;
        list_parent = new_list_parent;
    }

    public void SetNumbers(RectTransform parent, int index, Vector2 new_position)
    {
        Text newDigit = SpawnText(number_list);
        newDigit.text = (index + 1).ToString();
        newDigit.transform.SetParent(parent, false);

        if (parent == vertical_number_parent)
            newDigit.transform.localPosition = new Vector2(new_position.x, new_position.y - listManager.vertical_offset);
        else
            newDigit.transform.localPosition = new Vector2(new_position.x + listManager.horizontal_offset, new_position.y);
    }

    public void UpdateNumberPositions()
    {
        vertical_number_parent.transform.localPosition = new Vector2(0, list_parent.transform.localPosition.y + main_list.offsetMin.y);
        horizontal_number_parent.transform.localPosition = new Vector2(list_parent.transform.localPosition.x + main_list.offsetMax.x, 0);
    }

    public void CloseNumbers()
    {
        ResetText();
    }

    static public Text SpawnText(List<Text> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].gameObject.activeInHierarchy)
            {
                list[i].gameObject.SetActive(true);
                return list[i];
            }
        }

        Text new_text = Instantiate(Resources.Load<Text>("Editor/Text"));
        list.Add(new_text);

        return new_text;
    }

    public void ResetText()
    {
        for (int i = 0; i < number_list.Count; i++)
        {
            number_list[i].gameObject.SetActive(false);
        }
    }
}
