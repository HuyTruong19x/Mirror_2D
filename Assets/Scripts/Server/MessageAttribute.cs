using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class MessageAttribute : Attribute
{
    private ActionChannel _code;

    public ActionChannel Code => _code;

    public MessageAttribute(ActionChannel code)
    {
        _code = code;
    }
}
