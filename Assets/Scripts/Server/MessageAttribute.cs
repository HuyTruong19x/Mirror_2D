using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class MessageAttribute : Attribute
{
    private byte _code;

    public byte Code => _code;

    public MessageAttribute(byte code)
    {
        _code = code;
    }
}
