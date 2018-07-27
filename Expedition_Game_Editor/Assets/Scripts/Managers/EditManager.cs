﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(RowManager))]

public class EditManager : MonoBehaviour
{
    public void SetEdit()
    {
        RowManager rowManager = GetComponent<RowManager>();

        if(rowManager.edit_index.Count > 0)
        {
            Button add_button = GetComponent<EditorController>().actionManager.AddButton();

            add_button.GetComponentInChildren<Text>().text = "Add " + rowManager.table;

            //TEMPORARY ID! Create placeholder and display in the future
            add_button.onClick.AddListener(delegate { EditorManager.editorManager.windows[0].OpenPath(NewPath(rowManager.edit_path, 1)); });
        }
    }

    public Path NewPath(Path path, int id)
    {
        
        Path new_path = new Path(new List<int>(), new List<int>());
        
        for (int i = 0; i < path.editor.Count; i++)
            new_path.editor.Add(path.editor[i]);

        for (int i = 0; i < path.id.Count; i++)
            new_path.id.Add(path.id[i]);

        //Trouble (edit: is it?)
        if (new_path.id.Count < new_path.editor.Count)
            new_path.id.Add(id);
        else
            new_path.id[new_path.id.Count - 1] = id;

        return new_path;
        
    }
}
