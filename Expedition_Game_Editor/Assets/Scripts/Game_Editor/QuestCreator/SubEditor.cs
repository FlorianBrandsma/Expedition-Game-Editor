using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SubEditor : MonoBehaviour, IEditor
{
    public EditorController controller { get; set; }

    public GameObject       header;
	public GameObject[] 	content;
    /*
	public RectTransform 	background, 
							mainList,
							listParent;

	public Slider 			slider;
    */
    bool changed;
	
    public void OpenEditor()
    {
        controller = GetComponent<EditorController>();

        InitializeSegments();
    }

    void InitializeSegments()
    {
        foreach (GameObject segment in content)
            segment.GetComponent<EditorSegment>().InitializeSegment(this);
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
    }

    public void CloseSegments()
    {   
        foreach (GameObject segment in content)
            segment.GetComponent<EditorSegment>().CloseSegment();    
    }
}
