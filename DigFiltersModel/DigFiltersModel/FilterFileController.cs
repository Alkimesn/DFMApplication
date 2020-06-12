using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DigFiltersModel
{
    class FilterFileController:IFilterController
    {
        string FilterDir = "SavedFilters";
        public List<DFMFilter> savedFilters { get; private set; } = new List<DFMFilter>();
        List<string> GetSavedFilters()
        {
            savedFilters.Clear();
            List<string> errors = new List<string>();
            foreach(var path in Directory.GetFiles(FilterDir))
            {
                try
                {
                    savedFilters.Add(LoadFilterFromFile(path));
                }
                catch(Exception e)
                {
                    errors.Add("Path: " + path + ": exception" + e.ToString());
                }
            }
            return errors;
        }
        DFMFilter LoadFilterFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            if (!lines[0].ToLower().StartsWith("filter ")) throw new FileFormatException("File is not formatted as a filter file");
            string[] namelines = lines[0].Split(' ');
            string name = namelines[1];
            string[] uppernums = lines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            DFMCoefList upper = new DFMCoefList(uppernums.Select(x => double.Parse(x)).ToArray());
            string[] lowernums = lines[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            DFMCoefList lower = new DFMCoefList(lowernums.Select(x => double.Parse(x)).ToArray());
            if (lower[0] == 0) throw new ArgumentException("First denominator coefficient can't be 0");
            DFMFilter filter = new DFMFilter(upper, lower, name);
            return filter;
        }
        void SaveFilter(DFMFilter filter)
        {
            string[] lines = new string[3];
            lines[0] = "Filter " + filter.Name;
            lines[1] = filter.UpperCoefsString;
            lines[2] = filter.LowerCoefsString;
            File.WriteAllLines(FilterDir + @"\" + filter.Name, lines);
        }
        public void LoadFilter(string path, Controller controller)
        {
            DFMFilter filter = LoadFilterFromFile(path);
            SaveFilter(filter);
            GetSavedFilters();
            controller.CurFilter = filter;
        }
    }
}
