using CardStorageService.Domain;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CardStorageService.API;

public static class CacheProvider
{
    private static string _path = $"{AppDomain.CurrentDomain.BaseDirectory}data.protected";
    private static byte[] _additionalEntropy = { 5, 3, 7, 1, 5 };

    public static void CacheConnection(ConnectionString connection)
    {
        try
        {
            string data = JsonSerializer.Serialize(connection);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            File.WriteAllBytes(_path, Protect(dataBytes));
        }
        catch (Exception e)
        {
            Console.WriteLine("Serialize data error.");
        }
    }

    public static ConnectionString GetConnectionFromCache()
    {
        try
        {
            var protectedDataBytes = File.ReadAllBytes(_path);
            byte[] dataBytes = Unprotect(protectedDataBytes);
            return JsonSerializer.Deserialize<ConnectionString>(Encoding.UTF8.GetString(dataBytes));
        }
        catch (Exception e)
        {
            Console.WriteLine("Deserialize data error.");
            return null;
        }
    }

    private static byte[] Protect(byte[] data)
    {
        try
        {
            return ProtectedData.Protect(data, _additionalEntropy, DataProtectionScope.LocalMachine);
        }
        catch (CryptographicException e)
        {
            Console.WriteLine("Protect error.");
            return null;
        }
    }

    private static byte[] Unprotect(byte[] data)
    {
        try
        {
            return ProtectedData.Unprotect(data, _additionalEntropy, DataProtectionScope.LocalMachine);
        }
        catch (CryptographicException e)
        {
            Console.WriteLine($"Unprotect error.\n{e.Message}");
            return null;
        }
    }
}