using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen
{
    public class HexFrame : FrameSegment
    {
        private byte[] _data;

        public HexFrame(byte[] data) => _data = data;

        public override byte[] Data => _data;
    }
}
