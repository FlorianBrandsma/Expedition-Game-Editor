using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(SelectionElement))]
public class MiniButtonManager : MonoBehaviour
{
    public Texture2D texture;

    public void SetButtons()
    {
        SelectionElement new_element = GetComponent<EditorController>().actionManager.AddMiniButton();

        SelectionElement element = GetComponent<SelectionElement>();

        new_element.data = element.data;
        new_element.selectionType = element.selectionType;
        new_element.listType = element.listType;

        new_element.icon.texture = texture;

        new_element.GetComponent<Button>().onClick.AddListener(delegate { new_element.SelectElement();});
    }
}
