namespace PMSCore;

public enum EntityState : byte
{
    Ok = 0,
    NotFound = 1,
    AlreadyExists = 2,
    Forbidden = 3,
}