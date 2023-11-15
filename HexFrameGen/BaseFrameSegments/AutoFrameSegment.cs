using HexFrameGen.AutoCalculator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen.BaseFrameSegments
{
    /// <summary>
    /// 自动帧段
    /// The children's instance will calculate its data automatically
    /// u ought to define its length at first, and u shouldn't change it
    /// </summary>
    public class AutoFrameSegment : BaseFrameSegment
    {
        private List<BaseFrameSegment> _segments = new();
        public int BytesCount;
        public IAutoCalculator Calculator;

        public AutoFrameSegment(int bytesCount) => BytesCount = bytesCount;

        public AutoFrameSegment(AutoFrameSegment segment)
        {
            _segments = new(segment._segments);
            BytesCount = segment.BytesCount;
            Calculator = segment.Calculator;
        }

        public void Register(params BaseFrameSegment[] segments) => _segments.AddRange(segments);

        public void ExchangeDynamic(DynamicFrameSegment dest)
        {
            var temp = _segments.OfType<DynamicFrameSegment>().Where(s => s.Name.Equals(dest.Name));
            if (temp.Count() < 1) return;
            var index = _segments.IndexOf(temp.First());
            _segments[index] = dest;
        }

        public void ExchangeAuto(AutoFrameSegment ori, AutoFrameSegment dest)
        {
            if (ori == dest || !_segments.Contains(ori)) return;
            _segments[_segments.IndexOf(ori)] = dest;
        }

        public AutoFrameSegment Clone() => new(this);

        public override byte[] Data => Calculator?.Calculate(_segments);
    }
}
