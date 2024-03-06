using System;
using System.Text;
using System.Security.Cryptography;

public static class StringGuidExtension
{
    public static Guid ToGuid(this string id)

    {

        var provider = new MD5CryptoServiceProvider();

        byte[] inputBytes = Encoding.Default.GetBytes(id);

        byte[] hashBytes = provider.ComputeHash(inputBytes);



        return new Guid(hashBytes);

    }
}
