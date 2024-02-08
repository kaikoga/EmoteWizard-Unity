namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class StringExtension
    {
        public static string Nowrap(this string str) => str.Replace(" ", " ");

#if UNITY_2022_3_OR_NEWER

        public static string[] SplitCompat(this string str, string separator) => str.Split(separator);

#else

        public static string[] SplitCompat(this string str, string separator) => str.Split(new [] { separator }, System.StringSplitOptions.None);

#endif
    }
}