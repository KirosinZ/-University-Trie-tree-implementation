using System.Linq;

namespace STRIALG_TRIE
{
    static class WordsExtension
    {
        public static string RemoveAny(this string arg, string chars)
        {
            if (arg == "") return arg;
            string buf = "";
            foreach (char c in arg)
            {
                if (chars.IndexOf(c) == -1) buf += c;
            }
            return buf;
        }

        public static string ToWord(this string arg)
        {
            if (arg == "") return arg;
            int tmp = arg.IndexOf(' ');
            if (tmp != -1) arg = arg.Remove(tmp);
            return arg.RemoveAny(",.;:/?!\'\"[]{}()<>@#$%^&*-_=+").ToLower();
        }

        public static bool IsLetter(this string arg) => arg.Length == 1;

        public static bool IsWord(this string arg) => arg!="" && arg.IndexOfAny(" \n".ToCharArray()) == -1;

        public static string[] ToWords(this string arg) => arg.Split(' ').Select(x => x.ToWord()).Where(x => x != "").ToArray();
    }
}
