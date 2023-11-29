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
            if (segments.OfType<DynamicFrameSegment>().Where(d => d.Data == null).Count() > 0)
                return null;
            var r = segments.OfType<AutoFrameSegment>().Sum(s => s.BytesCount) +
            segments.Where(s => s is not AutoFrameSegment).Sum(s => s.Data.Length);
            return [(byte)(r >> 8), (byte)r];
        }
    }
}
