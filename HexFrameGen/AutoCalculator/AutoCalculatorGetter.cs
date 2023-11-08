using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen.AutoCalculator
{
    public static class AutoCalculatorGetter
    {
        public static Dictionary<string, IAutoCalculator> GetAutoCalculatorByDll(string path)
        {
            Dictionary<string, IAutoCalculator> calculators = new();
            foreach (var type in Assembly.LoadFrom(path).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IAutoCalculator))))
                calculators.Add(type.Name, (IAutoCalculator)Activator.CreateInstance(type));
            return calculators;
        }

        public static Dictionary<string, IAutoCalculator> GetAutoCalculatorByDllFromDirectory(string path)
        {
            var dir = new DirectoryInfo(path);
            if (!dir.Exists) throw new FileNotFoundException();
            Dictionary<string, IAutoCalculator> calculators = new Dictionary<string, IAutoCalculator>();
            foreach (var dict in dir.GetFiles("*.dll").Select(f => GetAutoCalculatorByDllFromDirectory(f.FullName)))
                calculators = calculators.Concat(dict).ToDictionary(kv => kv.Key, kv => kv.Value);
            return calculators;
        }
    }
}
