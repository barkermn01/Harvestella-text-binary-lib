using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml;

namespace CLI_App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0) { showHelp(); }
            switch (args[0] ?? "")
            {
                case "decipher":
                    {
                        try
                        {
                            string t1 = args[1];
                            if (!File.Exists(t1)) { throw new Exception("File could not be found"); }

                        }catch(Exception ex) { 

                            Console.WriteLine("Input file was not supplied or could not be found");
                            Environment.Exit(2); 

                        }

                        try
                        {

                            string t1 = args[2];

                        } catch(Exception ex)
                        {
                            Console.WriteLine("output file name was not supplied");
                            Environment.Exit(3);
                        }

                        string inputName = args[1];
                        string outputName = args[2];

                        HarvestellaBinTextLib.Parser.KeyValueBinary kvbParser = new HarvestellaBinTextLib.Parser.KeyValueBinary();
                        kvbParser.setData(File.ReadAllBytes(inputName));
                        kvbParser.ParseData();
                        Dictionary<String, String> dict = kvbParser.getAsDictionary();
                        String doc = JsonSerializer.Serialize(dict);
                        File.WriteAllText(outputName, doc);
                        break;
                    }
                case "cipher":
                    {

                        try
                        {
                            string t1 = args[1];
                            if (!File.Exists(t1)) { throw new Exception("File could not be found"); }

                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine("Input file was not supplied or could not be found");
                            Environment.Exit(2);

                        }

                        try
                        {

                            string t1 = args[2];

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("output file name was not supplied");
                            Environment.Exit(3);
                        }

                        string inputName = args[1];
                        string outputName = args[2];

                        HarvestellaBinTextLib.Builder.KeyValueBinary kvbBuilder = new HarvestellaBinTextLib.Builder.KeyValueBinary();
                        string jsonFileContents = File.ReadAllText(inputName);
                        kvbBuilder.fromDictionary((Dictionary<String, String>)JsonSerializer.Deserialize(jsonFileContents, typeof(Dictionary<String, String>)));
                        kvbBuilder.saveDataToFile(outputName);
                        break;
                    }
                default:
                    {
                        showHelp();
                        break;
                    }
            }
        }

        private static void showHelp()
        {
            Console.WriteLine("Usage binlib.exe <operation> <inputFile> <outputFile>");
            Console.WriteLine("Operation 'decipher' input_file_name.pak output_file_name.json");
            Console.WriteLine("Operation 'cipher' input_file_name.json output_file_name.pak");
            Environment.Exit(1);
        }
    }
}
