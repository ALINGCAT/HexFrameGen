using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen.BaseFrameSegments
{
    public class FixedFrameSegment : BaseFrameSegment, ISettable
    {
        private byte[] _data;
        private int _length;
        public override int Length => _length;
        public override byte[] Data => _data;

        public FixedFrameSegment(int length)
        {
            _length = length;
            _data = new byte[length];
        }

        public void SetData(IEnumerable<byte> data)
        {
            if (data.Count() != _length) throw new Exception("Length isn't matched.");
            _data = data.ToArray();
        }

        public void SetData(string data)
        {
            var pieces = data.Split(' ', '-');
            if (pieces.Count() != _length) throw new Exception("Length isn't matched.");
            _data = pieces.Select(s => Convert.ToByte(s, 16)).ToArray();
        }
    }
}
