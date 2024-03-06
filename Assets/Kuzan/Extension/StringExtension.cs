using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtension
{
    public static string MD5(this string text)
    {
        return Encryption.MD5Hash(text);
    }

    public static string Base64Encode(this string text)
    {
        return Encryption.Base64Encode(text);
    }

    public static string Base64Decode(this string text)
    {
        return Encryption.Base64Decode(text);
    }
}
