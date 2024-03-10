namespace Core.Extentions;

public static class StringExtentions
{
    public static void ToLover(this List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = list[i].ToLower();
        }
    }

    public static string UppercaseFirstLetter(this string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return s;
        }
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    public static double ComputeCyrillicPercentage(this string s)
    {
        int cyrillicCount = 0;
        foreach (char c in s)
        {
            if ((c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я'))
                cyrillicCount++;
        }

        return (double)cyrillicCount / s.Length;
    }
}
