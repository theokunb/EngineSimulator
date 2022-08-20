using EngineSimulator.Services;
using System.Globalization;

namespace EngineSimulator.Models
{
    public class FileParser : IFileParser
    {
        public FileParser(string path) 
        {
            objects = new Dictionary<string, object>();
            this.path = path;
        }


        private string path;
        private Dictionary<string, object> objects;



        public void AddParams(params string[] names)
        {
            foreach(var element in names)
                objects.Add(element, null);
        }
        public Dictionary<string,object> Parse()
        {
            if (objects.Count == 0)
                return new Dictionary<string, object>();
            using (StreamReader reader = new StreamReader(path))
            {
                foreach(var key in objects.Keys)
                {
                    string? line = reader.ReadLine();
                    if (TryParseFloat(line, out float? floatval))
                    {
                        objects[key] = floatval;
                    }
                    else if(TryParseArray(line,out int[]? arrval))
                    {
                        objects[key] = arrval;
                    }
                }
            }
            return objects;
        }

        private bool TryParseFloat(string line,out float? result)
        {
            if (float.TryParse(line, NumberStyles.Any, CultureInfo.InvariantCulture, out float _))
            {
                result = float.Parse(line, CultureInfo.InvariantCulture);
                return true;
            }
            else
            {
                result=null;
                return false;
            }
        }

        private bool TryParseArray(string line,out int[]? result)
        {
            if (line != null)
            {
                var parts = line.Split(' ');
                int[] M = new int[parts.Length];
                for (int i = 0; i < parts.Length; i++)
                {
                    if (int.TryParse(parts[i], out int _))
                        M[i] = int.Parse(parts[i]);
                }
                result = M;
                return true;
            }
            result = null;
            return false;
        }

    }
}
