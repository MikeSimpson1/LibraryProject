namespace LibraryApplication.Data;

public static class StringUtils
{
    public static bool IsNullOrEmpty(string s)
    {
        if (s.Equals(null) || s.Equals(""))
        {
            return true;
        }
        return false;
    }
}
