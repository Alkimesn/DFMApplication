using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DigFiltersModel
{
    class SignalFileController:ISignalController
    {
        string SignalDir = "SavedSignals";
        public List<DFMSignal> savedSignals { get; private set; } = new List<DFMSignal>();
        List<string> GetSavedSignals()
        {
            savedSignals.Clear();
            List<string> errors = new List<string>();
            foreach(var path in Directory.GetFiles(SignalDir))
            {
                try
                {
                    savedSignals.Add(LoadSignalFromFile(path));
                }
                catch(Exception e)
                {
                    errors.Add("Path: " + path + ": exception" + e.ToString());
                }
            }
            return errors;
        }
        DFMSignal LoadSignalFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            if (!lines[0].ToLower().StartsWith("signal ")) throw new FileFormatException("File is not formatted as a signal file");
            string[] namelines = lines[0].Split(' ');
            string name = namelines[1];
            string[] nums = lines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> totalnumbers = new List<int>();
            if (nums[0].StartsWith("*"))
            {
                int repetitions = int.Parse(nums[0].Substring(1));
                var numbers = nums.Skip(1).Select(x => int.Parse(x));
                for (int i = 0; i < repetitions; i++)
                {
                    totalnumbers.AddRange(numbers);
                }
            }
            else
            {
                var numbers = nums.Select(x => int.Parse(x));
                totalnumbers.AddRange(numbers);
            }
            DFMSignal signal = new DFMSignal(name, totalnumbers.ToArray());
            return signal;
        }
        void SaveSignal(DFMSignal signal)
        {
            string[] lines = new string[2];
            lines[0] = "Signal " + signal.Name;
            lines[1] = signal.ValuesString;
            File.WriteAllLines(SignalDir + @"\" + signal.Name, lines);
        }
        public void LoadSignal(string path, Controller controller)
        {
            DFMSignal signal = LoadSignalFromFile(path);
            SaveSignal(signal);
            GetSavedSignals();
            controller.CurSignal = signal;
        }
    }
}
