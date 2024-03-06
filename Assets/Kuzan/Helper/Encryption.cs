using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Encryption
{
    public static bool WriteToFile(string key, string path, string value)
    {
        try
        {
            File.WriteAllBytes(path, Encrypt(key, value));
            return true;
        }
        catch
        {
            return false;
        }
    }   

    private static byte[] Encrypt(string key, string value)
    {
        var keyByte = Encoding.UTF8.GetBytes(key);
        var valueByte = Encoding.UTF8.GetBytes(value);

        for(int i = 0; i < valueByte.Length; i++)
        {
            valueByte[i] = (byte)(valueByte[i] + keyByte[i % keyByte.Length]);
        }

        return valueByte;
    }

    private static string Decrypt(string key, byte[] value)
    {
        var keyByte = Encoding.UTF8.GetBytes(key);
        var valueByte = value;

        for (int i = 0; i < valueByte.Length; i++)
        {
            valueByte[i] = (byte)(valueByte[i] - keyByte[i % keyByte.Length]);
        }

        return Encoding.UTF8.GetString(valueByte);
    }

    public string ReadFile(string key, string path)
    {
        try
        {
            var value = File.ReadAllBytes(path);
            return Decrypt(key, value);
        }
        catch
        {
            return null;
        }
    }    

    public static string MD5Hash(string value)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        md5.ComputeHash(Encoding.UTF8.GetBytes(value));

        byte[] result = md5.Hash;

        StringBuilder strBuilder = new StringBuilder();

        for(int i = 0;i<result.Length;i++)
        {
            strBuilder.Append(result[i].ToString("x2"));
        }
        return strBuilder.ToString();
    }    

    public static string Base64Encode(string text)
    {
        var byteText = Encoding.UTF8.GetBytes(text);
        return System.Convert.ToBase64String(byteText);
    }    

    public static string Base64Decode(string text)
    {
        var byteText = System.Convert.FromBase64String(text);
        return Encoding.UTF8.GetString(byteText);
    }    
}
