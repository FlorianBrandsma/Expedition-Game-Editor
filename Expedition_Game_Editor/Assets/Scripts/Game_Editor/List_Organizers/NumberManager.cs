using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NumberManager : MonoBehaviour, IOverlay
{
    static public List<RectTransform> number_parent_list = new List<RectTransform>();
    private List<RectTransform> number_parent_list_local = new List<RectTransform>();

    static public List<Text> number_list = new List<Text>();
    private List<Text> number_list_local = new List<Text>();

    private RectTransform   main_list, 
                            list_parent;

    private RectTransform   horizontal_parent,
                            vertical_parent;

    OverlayManager          overlayManager;

    public void InitializeOverlay(RectTransform new_main_list, RectTransform new_list_parent)
    {
        overlayManager = GetComponent<OverlayManager>();

        main_list = new_main_list;
        list_parent = new_list_parent;
    }

    public void ActivateOverlay(IOrganizer organizer)
    {
        Vector2 list_size = organizer.GetListSize(overlayManager.listManager.id_list, false);

        if (list_size.x > 0)
            overlayManager.horizontal_min.gameObject.SetActive(true);

        if (list_size.y > 0)
            overlayManager.vertical_min.gameObject.SetActive(true);
    }

    public void SetOverlay()
    {
        Vector2 list_size = overlayManager.listManager.list_size;

        Vector2 base_size = new Vector2(list_parent.rect.width / list_size.x, list_parent.rect.height / list_size.y);

        if(overlayManager.horizontal_min.gameObject.activeInHierarchy)
        {
            horizontal_parent = overlayManager.horizontal_min.GetComponent<OverlayBorder>().ScrollParent();

            for (int x = 0; x < list_size.x; x++)
                SetNumbers(overlayManager.horizontal_min, x, -((base_size.x * 0.5f) * (list_size.x - 1)) + (x * base_size.x));
        }

        if(overlayManager.vertical_min.gameObject.activeInHierarchy)
        {
            vertical_parent = overlayManager.vertical_min.GetComponent<OverlayBorder>().ScrollParent();

            for (int y = 0; y < list_size.y; y++)
                SetNumbers(overlayManager.vertical_min, y, -(base_size.y * 0.5f) + (list_parent.sizeDelta.y / 2f) - (y * base_size.y));
        }
    }

    public void UpdateOverlay()
    {
        if (vertical_parent != null)
            vertical_parent.transform.localPosition = new Vector2(0, list_parent.transform.localPosition.y );

        if (horizontal_parent != null)
            horizontal_parent.transform.localPosition = new Vector2(list_parent.transform.localPosition.x, 0);
    }

    public void CloseOverlay()
    {
        ResetText(number_list_local);

        DestroyImmediate(this);
    }

    public void SetNumbers(RectTransform number_parent, int index, float new_position)
    {
        OverlayBorder overlayBorder = number_parent.GetComponent<OverlayBorder>();
        
        Text new_text = SpawnText(number_list);
        number_list_local.Add(new_text);

        new_text.text = (index + 1).ToString();
        new_text.transform.SetParent(overlayBorder.scroll_parent, false);

        if (overlayBorder.vertical)
            new_text.transform.localPosition = new Vector2(0, new_position);
        
        if(overlayBorder.horizontal)
            new_text.transform.localPosition = new Vector2(new_position, 0);  
    }

    static public Text SpawnText(List<Text> list)
    {
        foreach(Text element in list)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                element.gameObject.SetActive(true);
                return element;
            }
        }

        Text new_text = Instantiate(Resources.Load<Text>("Editor/Text"));
        list.Add(new_text);

        return new_text;
    }

    public void ResetText(List<Text> list)
    {
        foreach(Text element in list)
            element.gameObject.SetActive(false);   
    }

    //Spawn pages
}
