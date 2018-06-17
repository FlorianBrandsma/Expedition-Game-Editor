using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderManager : MonoBehaviour
{
    public Slider   horizontal_slider,
                    vertical_slider;

    public float    slider_offset;

    RectTransform   main_list, list_parent;

    ListManager listManager;

    public void SetSliders(ListManager new_listManager, RectTransform new_main_list, RectTransform new_list_parent)
    {
        listManager = new_listManager;
        main_list = new_main_list;
        list_parent = new_list_parent;

        if (list_parent.sizeDelta.y > main_list.rect.max.y * 2)
        {
            main_list.offsetMax = new Vector2(-vertical_slider.GetComponent<RectTransform>().rect.width, main_list.offsetMax.y);

            listManager.horizontal_offset = vertical_slider.GetComponent<RectTransform>().rect.width / 2f;

            vertical_slider.gameObject.SetActive(true);
        }

        if ((list_parent.sizeDelta.x + main_list.rect.width) > main_list.rect.max.x * 2)
        {
            main_list.offsetMin = new Vector2(main_list.offsetMin.x, horizontal_slider.GetComponent<RectTransform>().rect.height);

            list_parent.sizeDelta = new Vector2(list_parent.sizeDelta.x + horizontal_slider.GetComponent<RectTransform>().rect.height, list_parent.sizeDelta.y);

            listManager.vertical_offset = horizontal_slider.GetComponent<RectTransform>().rect.height / 2f;

            horizontal_slider.gameObject.SetActive(true);
        }

        if(horizontal_slider != null && vertical_slider != null)
        {
            if (vertical_slider.gameObject.activeInHierarchy)
                horizontal_slider.GetComponent<RectTransform>().offsetMax = new Vector2(-slider_offset, horizontal_slider.GetComponent<RectTransform>().offsetMax.y);

            if (horizontal_slider.gameObject.activeInHierarchy)
                vertical_slider.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_slider.GetComponent<RectTransform>().offsetMin.x, slider_offset);
        }
    }

    public void UpdateSliders()
    {
        if (vertical_slider != null && vertical_slider.gameObject.activeInHierarchy)
            vertical_slider.value = Mathf.Clamp(main_list.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
        if (horizontal_slider != null && horizontal_slider.gameObject.activeInHierarchy)
            horizontal_slider.value = Mathf.Clamp(main_list.GetComponent<ScrollRect>().horizontalNormalizedPosition, 0, 1);
    }

    public void CloseSliders()
    {
        main_list.offsetMin = new Vector2(main_list.offsetMin.x, 0);
        main_list.offsetMax = new Vector2(0, main_list.offsetMax.y);

        if (horizontal_slider != null)
        {
            listManager.vertical_offset = 0;
            horizontal_slider.GetComponent<RectTransform>().offsetMax = new Vector2(0, horizontal_slider.GetComponent<RectTransform>().offsetMax.y);
            horizontal_slider.gameObject.SetActive(false);
        }

        if (vertical_slider != null)
        {
            listManager.horizontal_offset = 0;
            vertical_slider.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_slider.GetComponent<RectTransform>().offsetMin.x, 0);
            vertical_slider.gameObject.SetActive(false);
        } 
    }
}
