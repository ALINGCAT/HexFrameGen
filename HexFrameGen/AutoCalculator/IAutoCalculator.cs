using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen.AutoCalculator
{
    public interface IAutoCalculator
    {
        byte[] Calculate(IEnumerable<BaseFrameSegment> segments);
    }
}
