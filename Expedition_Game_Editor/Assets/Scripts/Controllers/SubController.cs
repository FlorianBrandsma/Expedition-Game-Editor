using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SubController : MonoBehaviour, IEditor
{
    public EditorController controller { get; set; }

    public GameObject       header_field;
    private IHeader         header;
    public EditorSegment[] 	segments;

    bool changed;

    public void Awake()
    {
        if (header_field != null)
            header = header_field.GetComponent<IHeader>();
    }

    public void OpenEditor()
    {
        controller = GetComponent<EditorController>();

        if(header != null)
            header.Activate(this);

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
        if (header_field != null)
            header.Deactivate();

        CloseSegments();
    }

    public void CloseSegments()
    {        
        foreach (EditorSegment segment in segments)
            segment.CloseSegment(); 
    }
}
