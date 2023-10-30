using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen
{
    /// <summary>
    /// 自动帧段
    /// The children's instance will calculate its data automatically
    /// u ought to define its length at first, and u shouldn't change it
    /// </summary>
    public abstract class AutoFrameSegment : FrameSegment
    {
        protected int _length;
        public override int Length => _length;
        public AutoFrameSegment(int length) => _length = length;
    }
}
