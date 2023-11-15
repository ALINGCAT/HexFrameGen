using HexFrameGen;
using HexFrameGen.AutoCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultAutoCalculator
{
    public class CheckSum1B : IAutoCalculator
    {
        public byte[] Calculate(IEnumerable<BaseFrameSegment> segments)
        {
            try { return new byte[1] { (byte)segments.Sum(s => s.Data.Sum(d => d)) }; }
            catch { return null; }
        }
    }
}
