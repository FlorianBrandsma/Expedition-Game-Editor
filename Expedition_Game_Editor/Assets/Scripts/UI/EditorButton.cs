﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorButton : MonoBehaviour, IElement
{
    private SelectionElement element { get { return GetComponent<SelectionElement>(); } }
    private ButtonProperties properties;

    public Text label;
    public RawImage icon;

    public void InitializeElement()
    {
        //properties = element.ListManager.listProperties.GetComponent<ButtonProperties>();

        icon.texture = Resources.Load<Texture2D>("Textures/Icons/UI/" + element.selectionProperty.ToString());
    }

    public void SetElement()
    {
        switch (element.data.dataController.DataType)
        {
            case Enums.DataType.Item:           SetItemElement();           break;
            case Enums.DataType.ChapterRegion:  SetChapterRegionElement();  break;
            default: Debug.Log("CASE MISSING");                             break;
        }
    }

    private void SetItemElement()
    {
        var data = (ItemDataElement)element.data.dataElement;

        label.text = data.originalName;
    }

    private void SetChapterRegionElement()
    {
        var data = (ChapterRegionDataElement)element.data.dataElement;

        label.text = data.name;
    }

    public void CloseElement()
    {

    }
}
