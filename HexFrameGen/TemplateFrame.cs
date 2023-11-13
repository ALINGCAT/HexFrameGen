using HexFrameGen.BaseFrameSegments;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HexFrameGen
{
    public class TemplateFrame : FrameSegment
    {
        private List<BaseFrameSegment> _segments;
        public Dictionary<string, DynamicFrameSegment> Dynamic =>
            _segments.Where(s => s is DynamicFrameSegment).Select(s => s as DynamicFrameSegment).Where(s => s.Name != null).ToDictionary(s => s.Name, s => s);

        public TemplateFrame(params BaseFrameSegment[] segments) => _segments = segments.ToList();

        public TemplateFrame(TemplateFrame frame)
        {
            _segments = frame._segments.Select(s => s is DynamicFrameSegment dfs ? dfs.Clone() : s).ToList();
            foreach (var auto in _segments.OfType<AutoFrameSegment>())
                foreach (var dy in _segments.OfType<DynamicFrameSegment>())
                    auto.ExchangeDynamic(dy);
        }

        public TemplateFrame Clone() => new(this);

        public void AddSegment(BaseFrameSegment segment) => _segments.Add(segment);

        public HexFrame Gen() => new(Data);

        public override byte[] Data
        {
            get
            {
                if (Dynamic.Count(ds => ds.Value.Data == null) > 0)
                    throw new InvalidOperationException("There are some DynamicFrames without data: " + string.Concat(Dynamic.Where(kv => kv.Value.Data == null).Select(kv => kv.Key)));
                List<byte> bytes = new();
                foreach (var data in _segments.Select(s => s.Data))
                    bytes.AddRange(data);
                return bytes.ToArray();
            }
        }
    }
}
