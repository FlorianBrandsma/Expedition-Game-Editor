using UnityEngine;
using System.Collections;

public class PreviewManager : MonoBehaviour
{
    public Camera cam;

    public void SetPreview(PreviewProperties previewProperties)
    {
        cam.rect = new Rect(new Vector2(cam.rect.x, cam.rect.y), new Vector2((previewProperties.field_width / EditorManager.UI.rect.width) - cam.rect.x, 1));

        gameObject.SetActive(true);
    }

    public void ClosePreview()
    {
        gameObject.SetActive(false);
    }
}
