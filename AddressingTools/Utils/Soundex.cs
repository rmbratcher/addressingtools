using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AddressingTools
{
        public static class Soundex
        {
            private static Regex isDigits = new Regex(@"^\d+$");
            private static Regex split = new Regex(@"[\s,;_.-]");

            public static string EncodeString(string s,int l=5)
            {
                string[] parts = split.Split(s);
                List<string> rets = new List<string>();
                foreach (string part in parts)
                {
                    if(!String.IsNullOrEmpty(part))
                        rets.Add(_encodeString(part, l));
                }
                return string.Join("-", rets.ToArray());
            }

            private static string _encodeString(string s, int l)
            {
                if (isDigits.IsMatch(s))
                    return s;
                if (s.Length < 2)
                    return s;
                string encoded = String.Empty;
                string sl = s.ToLower();
                string previousValue = null;
                for (int i = 0; i < sl.Length; i++)
                {
                    string x = EncodeChar(sl[i]);
                    if (x != previousValue)
                    {
                        encoded += EncodeChar(sl[i]);
                        previousValue = x;
                    }
                }

                string f = s[0].ToString();
                string g = "0";

                if(!String.IsNullOrEmpty(encoded))
                    g = encoded.Substring(1);

                if (g.Length < l)
                    f += g.PadRight(l, '0');
                else
                    f += g.Substring(0, l);

                string ret = f.ToUpper();
                return ret;
            }

            private static string EncodeChar(char c)
            {
                // C# will re-order this list and produce a look-up list from it

                // C# will do all the work we would otherwise do by building arrays of values

                switch (Char.ToLower(c))
                {
                    case 'b':
                    case 'f':
                    case 'p':
                    case 'v':
                        return "1";
                    case 'c':
                    case 'g':
                    case 'j':
                    case 'k':
                    case 'q':
                    case 's':
                    case 'x':
                    case 'z':
                        return "2";
                    case 'd':
                    case 't':
                        return "3";
                    case 'l':
                        return "4";
                    case 'm':
                    case 'n':
                        return "5";
                    case 'r':
                        return "6";
                    default:
                        return string.Empty;
                }
            }
        }
}
