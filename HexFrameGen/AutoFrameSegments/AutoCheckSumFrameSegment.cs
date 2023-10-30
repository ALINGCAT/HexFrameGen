using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen.AutoFrameSegments
{
    public class AutoCheckSumFrameSegment : AutoFrameSegment
    {
        private readonly List<FrameSegment> _segments = new();
        public AutoCheckSumFrameSegment(int length) : base(length)
        {
        }

        public void Register(FrameSegment segment) => _segments.Add(segment);

        public override byte[] Data
        {
            get
            {
                var data = new byte[Length];
                var lens = _segments.Sum(s => s.CheckSum);
                for (int i = Length - 1; i >= 0; i--)
                {
                    data[i] = (byte)(lens & 0xff);
                    lens >>= 8;
                }
                return data;
            }
        }
    }
}
