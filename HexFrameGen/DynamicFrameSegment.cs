using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen
{
    public class DynamicFrameSegment : FrameSegment
    {
        private byte[] _data;

        public override byte[] Data => _data;

        public void SetData(IEnumerable<byte> data) => _data = data.ToArray();

        public void SetData(string data) => _data = data.Split(' ', '-').Select(s => Convert.ToByte(s, 16)).ToArray();
    }
}
