using System.ComponentModel;

namespace PMSCore;

public enum UserProperty
{
    [Description ("Gives a permission to a user")]
    AddPermissions,
    [Description ("Removes a permission from a user")]
    RemovePermissions,
    [Description ("Changes a given user id")]
    ChangeUserId,
}