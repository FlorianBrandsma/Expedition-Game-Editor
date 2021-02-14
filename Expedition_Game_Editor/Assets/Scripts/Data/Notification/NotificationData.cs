using UnityEngine;

public class NotificationData
{
    public int Id                                   { get; set; }

    public Enums.NotificationType NotificationType  { get; set; }

    public string Message                           { get; set; }

    public void GetOriginalValues(NotificationData originalData)
    {
        Id                  = originalData.Id;

        NotificationType    = originalData.NotificationType;

        Message             = originalData.Message;
    }

    public NotificationData Clone()
    {
        var data = new NotificationData();

        data.Id                 = Id;

        data.NotificationType   = NotificationType;

        data.Message            = Message;

        return data;
    }

    public virtual void Clone(NotificationElementData elementData)
    {
        elementData.Id                  = Id;

        elementData.NotificationType    = NotificationType;

        elementData.Message             = Message;
    }
}
