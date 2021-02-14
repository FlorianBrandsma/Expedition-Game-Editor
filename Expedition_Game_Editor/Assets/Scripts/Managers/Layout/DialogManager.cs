using UnityEngine;
using System;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    public bool? confirmRequest = null;

    private void Awake()
    {
        instance = this;
    }

    public void RequestConfirmation(Action<bool?> callback, Enums.DialogType dialogType)
    {
        StartCoroutine(AwaitConfirmation(callback, dialogType));
    }

    public IEnumerator AwaitConfirmation(Action<bool?> callback, Enums.DialogType dialogType)
    {
        RenderManager.Render(new PathManager.Dialog(dialogType).Initialize());

        yield return new WaitWhile(() => confirmRequest == null);

        callback(confirmRequest);

        confirmRequest = null;
    }
}
