using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SegmentOrganizer : MonoBehaviour 
{
	public RectTransform[] 	segments;

	public RectTransform 	background, 
							mainList,
							listParent;

	public Slider 			slider;

	void Start ()
    {
        PlaceSegments(0);
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
}
