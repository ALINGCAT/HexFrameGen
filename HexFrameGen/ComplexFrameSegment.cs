using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen
{
    /// <summary>
    /// 复合帧段
    /// </summary>
    public class ComplexFrameSegment : FrameSegment, IEnumerable<FrameSegment>
    {
        protected readonly List<FrameSegment> _segments = new();
        public void AddSegment(FrameSegment segment) => _segments.Add(segment);

        public IEnumerator<FrameSegment> GetEnumerator() => _segments.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _segments.GetEnumerator();

        public void Add(FrameSegment segment) => _segments.Add(segment);

        public override int Length => _segments.Sum(r => r.Length);

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
