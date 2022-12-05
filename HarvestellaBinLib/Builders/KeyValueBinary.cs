using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarvestellaBinTextLib;

namespace HarvestellaBinTextLib.Builder
{
    public class KeyValueBinary
    {
        public String[] keys { private set; get; }
        public String[] values { private set; get; }

        private String ApplyPadding(string text)
        {
            int LengthNeedsPadding = 4 - (text.Length % 4);
            if (LengthNeedsPadding != 0)
            {
                for (int i = LengthNeedsPadding; i > 0; i--)
                {
                    text += '\0';
                }
            }
            return text;
        }

        public void fromDictionary(Dictionary<string, string> dict)
        {
            keys = new String[dict.Count];
            values = new String[dict.Count];
            uint i = 0;

            foreach(KeyValuePair<string, string> kvp in dict)
            {
                keys[i] = ApplyPadding(kvp.Key);
                values[i] = ApplyPadding(kvp.Value);
                i++;
            }
        }

        public byte[] getData()
        {
            uint items = (uint)keys.Length;
            byte[] length = BitConverter.GetBytes(items);
            uint totalBytes = 4 + (4 * items);

            byte[][] keysData = new byte[items][];
            for(int i = 0; i < keys.Length; i++) 
            {
                keysData[i] = Utils.ConvertStringToBytes(keys[i]);
                totalBytes += (uint)keysData[i].Length+4;
            }
            byte[][] valsData = new byte[items][];
            for (int i = 0; i < keys.Length; i++)
            {
                valsData[i] = Utils.ConvertStringToBytes(values[i]);
                totalBytes += (uint)valsData[i].Length+4;
            }

            byte[] ret = new byte[totalBytes];
            length.CopyTo(ret,0);
            uint currentPos = 4;
            // skip over value start Positions
            currentPos += (items) * 4;
            foreach(byte[] key in keysData)
            {
                BitConverter.GetBytes(key.Length).CopyTo(ret, currentPos);
                currentPos += 4;
                key.CopyTo(ret, currentPos);
                currentPos += (uint)key.Length;
            }

            int NextValInsertPosCount = 4;
            foreach (byte[] val in valsData)
            {
                BitConverter.GetBytes(val.Length).CopyTo(ret, currentPos);
                currentPos += 4;
                BitConverter.GetBytes(currentPos-4).CopyTo(ret, NextValInsertPosCount);
                NextValInsertPosCount += 4;
                val.CopyTo(ret, currentPos);
                currentPos += (uint)val.Length;
            }

            return ret;
        }

        public void saveDataToFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            byte[] data = getData();
            fs.Write(data, 0, data.Length);
            fs.Close();
        }
    }
}
