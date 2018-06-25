﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SegmentManager : MonoBehaviour, IEditor
{
	public GameObject[] 	segments;

	public RectTransform 	background, 
							mainList,
							listParent;

	public Slider 			slider;

    bool changed;
	
    public void OpenEditor()
    {
        gameObject.SetActive(true);
        InitializeSegments();
    }

    void InitializeSegments()
    {
        foreach (GameObject segment in segments)
            segment.GetComponent<EditorSegment>().InitializeSegment();
    }

    public void PlaceSegments(int newSegment)
    {
        float newPos = 0;

        for (int i = 0; i < segments.Length; i++)
        {
			ObjectiveSegmentFormat segment = segments[i].GetComponent<ObjectiveSegmentFormat>();

			segments[i].transform.localPosition = new Vector2(segments[i].transform.localPosition.x, newPos + (listParent.rect.height / 2));

            if (!segment.collapsed)
                newPos -= segment.segmentSize;
            else
                newPos -= 30;   
        }

		float scrollRectSize = background.sizeDelta.y + mainList.offsetMax.y - mainList.offsetMin.y;

		listParent.sizeDelta = new Vector2(listParent.sizeDelta.x, - newPos - scrollRectSize);

		AdjustListHeight();
    }

    public void AdjustListHeight()
    {
        listParent.transform.localPosition = new Vector2(listParent.transform.localPosition.x, -listParent.sizeDelta.y / 2);

		slider.gameObject.SetActive(listParent.sizeDelta.y > mainList.rect.max.y / 4);
    }

    public void SaveEdit()
    {

    }

    public void ApplyEdit()
    {

    }

    public void CancelEdit()
    {

    }

    public void CloseEditor()
    {
        CloseSegments();

        gameObject.SetActive(false);
    }

    public void CloseSegments()
    {   
        foreach (GameObject segment in segments)
            segment.GetComponent<EditorSegment>().CloseSegment();    
    }
}