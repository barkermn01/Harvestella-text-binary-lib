using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarvestellaBinTextLib;

namespace HarvestellaBinTextLib.Parser
{
    public class KeyValueBinary
    {
        private byte[] data;

        public void setData(byte[] data)
        {
            this.data = data;
        }

        public void readFile(String filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            long fileLength = fs.Length;
            this.data = new byte[fileLength];
            fs.Read(data, 0, Convert.ToInt32(fileLength));
            fs.Close();
        }

        public String[] keys { private set; get; }
        public String[] values { private set; get; }

        public void ParseData()
        {
            uint NoOfItems = BitConverter.ToUInt32(data, 0);
            keys = new string[NoOfItems];
            values = new string[NoOfItems];

            List<String> lines = new List<string>();
            uint currentPossition = 4;
            currentPossition += NoOfItems * 4;

            for (uint i = 0; i < NoOfItems; i++)
            {
                uint nextLength = BitConverter.ToUInt32(data, (int)currentPossition);
                currentPossition += 4;
                String key = Utils.ConvertBytesToString(data, currentPossition, nextLength);
                currentPossition += nextLength;
                keys[i] = key.TrimEnd('\0');
            }

            for (uint i = 0; i < NoOfItems; i++)
            {
                uint nextLength = BitConverter.ToUInt32(data, (int)currentPossition);
                currentPossition += 4;
                String val = Utils.ConvertBytesToString(data, currentPossition, nextLength);
                currentPossition += nextLength;
                values[i] = val.TrimEnd('\0');
            }
        }

        public Dictionary<String, String> getAsDictionary()
        {
            var dict = new Dictionary<String, String>();
            for(int i = 0; i < keys.Length; i++)
            {
                dict.Add(keys[i], values[i]);
            }
            return dict;
        }
    }
}
