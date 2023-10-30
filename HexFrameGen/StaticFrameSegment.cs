using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen
{
    /// <summary>
    /// 静态帧段
    /// The data is immutable
    /// </summary>
    public class StaticFrameSegment : FrameSegment
    {
        private readonly byte[] _data;

        public override byte[] Data => _data;

        public StaticFrameSegment(IEnumerable<byte> data) => _data = data.ToArray();

        public StaticFrameSegment(string data) => _data = data.Split(' ', '-').Select(s => Convert.ToByte(s, 16)).ToArray();
    }
}
