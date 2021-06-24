using System.Collections.Generic;
using System.IO;


namespace GetContactVKBot.Core
{
    public class ExcelGenerator
    {
        public string Generate(string filename, List<string> data)
        {
            File.WriteAllLines($"{filename}.csv", data);
            return $"{filename}.csv";
        }
        
        
    }
}