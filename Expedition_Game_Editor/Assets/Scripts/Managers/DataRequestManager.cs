using UnityEngine;
using System.Linq;

public static class DataRequestManager
{
    private static IEditor dataEditor;

    public static DataRequest dataRequest;

    public static void CreateDataRequest(IEditor newDataEditor, Enums.RequestType requestType)
    {
        dataEditor = newDataEditor;

        if (dataEditor == null) return;
        
        dataRequest = new DataRequest()
        {
            requestType = requestType
        };
        
        ValidateRequest();
    }

    private static void ValidateRequest()
    {
        dataEditor.ApplyChanges(dataRequest);
        
        if(dataRequest.notificationList.Count > 0)
        {
            var dialogType = Enums.DialogType.Main;

            var errorCount      = dataRequest.notificationList.Where(x => x.NotificationType == Enums.NotificationType.Error).Count();
            var warningCount    = dataRequest.notificationList.Where(x => x.NotificationType == Enums.NotificationType.Warning).Count();

            if(errorCount + warningCount == 1)
            {
                //Show a regular notification dialog when there is only one notification
                dialogType = Enums.DialogType.Notification;

            } else if(errorCount > 0 && warningCount > 0) {

                //Show both errors and warnings
                dialogType = Enums.DialogType.Main;

            } else if (errorCount > 0 && warningCount == 0) {

                //Show only errors
                dialogType = Enums.DialogType.Error;

            } else if (warningCount > 0 && errorCount == 0) {

                //Show only warnings
                dialogType = Enums.DialogType.Warning;
            }
            
            DialogManager.instance.RequestConfirmation(ConfirmRequest, dialogType);

        } else {

            ExecuteRequest();
        }
    }

    private static void ConfirmRequest(bool? confirmed)
    {
        //Rather make it not confirmable
        if (confirmed == true && dataRequest.notificationList.Where(x => x.NotificationType == Enums.NotificationType.Error).Count() == 0)
            ExecuteRequest();

        RenderManager.PreviousPath();
    }

    private static void ExecuteRequest()
    {
        dataRequest.requestType = Enums.RequestType.Execute;

        //Apply changes when there are no combined errors 
        dataEditor.ApplyChanges(dataRequest);

        dataEditor.ElementDataList.ForEach(x =>
        {
            if (SelectionElementManager.SelectionActive(x.DataElement))
            {
                var editorElement = (EditorElement)x.DataElement.SelectionElement;

                x.DataElement.Id = x.Id;

                if (editorElement.child != null)
                    editorElement.child.DataElement.Id = x.Id;

                x.DataElement.UpdateElement();
            }
        });

        dataEditor.FinalizeChanges();
    }
}
