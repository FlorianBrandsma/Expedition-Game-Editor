using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SubController : MonoBehaviour, IEditor
{
	public EditorController controller { get; set; }

	public EditorHeader     header;
	public EditorSegment[] 	segments;

	bool changed;
	
	public void OpenEditor()
	{
		controller = GetComponent<EditorController>();

        header.Activate();

		InitializeSegments();
	}

	void InitializeSegments()
	{ 
		foreach (EditorSegment segment in segments)
			segment.InitializeSegment(this);       
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
        header.Deactivate();

		CloseSegments();
	}

	public void CloseSegments()
	{        
		foreach (EditorSegment segment in segments)
			segment.CloseSegment(); 
	}
}
