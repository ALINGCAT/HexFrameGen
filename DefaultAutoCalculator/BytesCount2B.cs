using HexFrameGen;
using HexFrameGen.AutoCalculator;
using HexFrameGen.BaseFrameSegments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultAutoCalculator
{
    public class BytesCount2B : IAutoCalculator
    {
        public byte[] Calculate(IEnumerable<BaseFrameSegment> segments)
        {
            var r = segments.Where(s => s is AutoFrameSegment).Sum(s => (s as AutoFrameSegment).BytesCount) +
                segments.Where(s => s is not AutoFrameSegment).Sum(s => s.Data.Length);
            return new byte[2] { (byte)(r >> 8), (byte)r };
        }
    }
}
