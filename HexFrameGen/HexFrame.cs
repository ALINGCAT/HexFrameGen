using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen
{
    public class HexFrame : FrameSegment
    {
        private readonly List<FrameSegment> _segments = new();
        public void AddSegment(FrameSegment segment) => _segments.Add(segment);

        public override byte[] Data
        {
            get
            {
                var r = new List<byte>();
                foreach (var segment in _segments) r.AddRange(segment.Data);
                return r.ToArray();
            }
        }
    }
}
