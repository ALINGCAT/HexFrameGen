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
        public List<BaseFrameSegment> Segments = new();
        public int BytesCount;
        public IAutoCalculator Calculator;

        public AutoFrameSegment(int bytesCount) => BytesCount = bytesCount;

        public AutoFrameSegment(AutoFrameSegment segment)
        {
            Segments = new(segment.Segments);
            BytesCount = segment.BytesCount;
            Calculator = segment.Calculator;
        }

        public void Register(params BaseFrameSegment[] segments) => Segments.AddRange(segments);

        public void Register(IEnumerable<BaseFrameSegment> segments) => Segments.AddRange(segments);

        public void ExchangeDynamic(DynamicFrameSegment dest)
        {
            var temp = Segments.OfType<DynamicFrameSegment>().Where(s => s.Name.Equals(dest.Name));
            if (temp.Count() < 1) return;
            var index = Segments.IndexOf(temp.First());
            Segments[index] = dest;
        }

        public void ExchangeAuto(AutoFrameSegment ori, AutoFrameSegment dest)
        {
            if (ori == dest || !Segments.Contains(ori)) return;
            Segments[Segments.IndexOf(ori)] = dest;
        }

        public AutoFrameSegment Clone() => new(this);

        public override byte[] Data => Calculator?.Calculate(Segments);
    }
}
