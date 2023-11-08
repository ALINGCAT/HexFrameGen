using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen
{
    /// <summary>
    /// 帧段
    /// </summary>
    public abstract class FrameSegment
    {
        public virtual byte[] Data { get; }
        public override string ToString() => BitConverter.ToString(Data).Replace("-", " ");
    }
}
