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
    public class AutoFrameSegment : BaseFrameSegment, INewable<AutoFrameSegment>
    {
        protected List<BaseFrameSegment> _segments = new();
        public int BytesCount;
        public IAutoCalculator Calculator;

        public AutoFrameSegment(int bytesCount) => BytesCount = bytesCount;

        public void Register(params BaseFrameSegment[] segments) => _segments.AddRange(segments);

        public AutoFrameSegment New() => new(BytesCount) { _segments = new(_segments) };

        public override byte[] Data => Calculator?.Calculate(_segments);
    }
}
