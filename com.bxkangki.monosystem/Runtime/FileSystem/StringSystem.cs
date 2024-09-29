using System.Text;

namespace FileSystem
{
    public readonly struct StringSystem
    {
        // Static readonly string builder for save memeory and prevent allocated garbages.
        // If you call Clear() method, earlier contents are going to all removed.
        private static readonly StringBuilder sb = new StringBuilder();
        public static void Clear()
        {
            sb.Clear();
        }

        /// <summary>
        /// Append object(string) to current String System.
        /// </summary>
        /// <param name="str"></param>
        public static void Append(object str)
        {
            sb.Append(str);
        }

        public static string Format(object a, object b)
        {
            sb.Clear();
            sb.Append(a);
            sb.Append(b);
            return Value();
        }

        public static string Format(object a, object b, object c)
        {
            sb.Clear();
            sb.Append(a);
            sb.Append(b);
            sb.Append(c);
            return Value();
        }


        public static string Format(object a, object b, object c, object d)
        {
            sb.Clear();
            sb.Append(a);
            sb.Append(b);
            sb.Append(c);
            sb.Append(d);
            return Value();
        }


        public static string Format(object a, object b, object c, object d, object e)
        {
            sb.Clear();
            sb.Append(a);
            sb.Append(b);
            sb.Append(c);
            sb.Append(d);
            sb.Append(e);
            return Value();
        }

        public static string Format(object a, object b, object c, object d, object e, object g)
        {
            sb.Clear();
            sb.Append(a);
            sb.Append(b);
            sb.Append(c);
            sb.Append(d);
            sb.Append(e);
            sb.Append(g);
            return Value();
        }

        public static string Value()
        {
            return sb.ToString();
        }
    }
}
