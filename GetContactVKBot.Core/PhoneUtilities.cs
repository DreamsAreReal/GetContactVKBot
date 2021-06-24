namespace GetContactVKBot.Core
{
    public static class PhoneUtilities
    {
        public static string FormatPhone(this string text)
        {
            return text.Replace("+", string.Empty).Replace(" ", string.Empty).Trim();
        }
    }
}