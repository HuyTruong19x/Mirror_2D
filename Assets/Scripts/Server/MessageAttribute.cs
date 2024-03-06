using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class MessageAttribute : Attribute
{
    private MessageCode _code;

    public MessageCode Code => _code;

    public MessageAttribute(MessageCode code)
    {
        _code = code;
    }
}
