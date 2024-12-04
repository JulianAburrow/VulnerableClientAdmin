namespace VulnerableClientAdminUI.Shared.Methods;

public static class SharedMethods
{
    public static string SplitPascalCaseStrings(string stringToSplit)
    {
        if (stringToSplit == null)
            return string.Empty;

        return Regex.Replace(stringToSplit, "(\\B[A-Z])", " $1");
    }

    public static byte[] GetUTF8Bytes(string bytesFromString) =>
        Encoding.UTF8.GetBytes(bytesFromString);

    public static string GetBase64String(byte[] fileBytes) =>
        Convert.ToBase64String(fileBytes);
}
