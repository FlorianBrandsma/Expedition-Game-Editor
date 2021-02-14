using UnityEngine;
using System.Collections.Generic;

public class DataRequest
{
    public Enums.RequestType requestType;
    public bool includeDependencies;
    public List<NotificationElementData> notificationList = new List<NotificationElementData>();

    public void AddNotification(Enums.NotificationType notificationType, string message)
    {
        var notificationElementData = new NotificationElementData()
        {
            Id = notificationList.Count + 1,
            NotificationType = notificationType,
            Message = message
        };

        notificationList.Add(notificationElementData);
    }
}
