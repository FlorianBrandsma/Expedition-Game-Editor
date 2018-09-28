using UnityEngine;
using System.Collections;

public class PreviewProperties : MonoBehaviour
{
    public PreviewManager previewManager;

    public float field_width { get; set; }

    public void SetPreview()
    {
        field_width = GetComponent<EditorController>().editorField.GetComponent<RectTransform>().rect.width;

        previewManager.SetPreview(this);
    }
    public void ClosePreview()
    {
        previewManager.ClosePreview();
    }
}
