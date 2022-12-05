using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public static class Utils
    {
        public static String ConvertBytesToString(byte[] bytes, uint startPos = 0, uint length = 0)
        {
            if (length == 0)
            {
                length = (uint)bytes.Length - startPos;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                char c = (char)bytes[startPos + i];
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static Byte[] ConvertBytesToString(String str)
        {
            char[] chars = str.ToCharArray();
            byte[] bytes = new byte[chars.Length];
            for(int i =0; i < chars.Length; i++)
            {
                bytes[i] = (byte)chars[i];
            }
            return bytes;
        }
    }
}
