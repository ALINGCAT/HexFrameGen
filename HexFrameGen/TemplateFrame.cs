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
            var autodict = frame._segments.OfType<AutoFrameSegment>().ToDictionary(s => s, s => s.Clone());
            _segments = frame._segments.Select(s => s is DynamicFrameSegment dfs ? dfs.Clone() : (s is AutoFrameSegment afs ? autodict[afs] : s)).ToList();
            foreach (var auto in _segments.OfType<AutoFrameSegment>())
            {
                foreach (var dy in _segments.OfType<DynamicFrameSegment>())
                    auto.ExchangeDynamic(dy);
                foreach (var kv in autodict)
                    auto.ExchangeAuto(kv.Key, kv.Value);
            }
        }

        public TemplateFrame Clone() => new(this);

        public void AddSegment(BaseFrameSegment segment) => _segments.Add(segment);

        public HexFrame Gen() => new(Data);

        public bool CanGen() => Dynamic.Count(ds => ds.Value.Data == null) < 1;

        public override byte[] Data
        {
            get
            {
                if (!CanGen())
                    throw new InvalidOperationException("There are some DynamicFrames without data: " + string.Concat(Dynamic.Where(kv => kv.Value.Data == null).Select(kv => kv.Key)));
                List<byte> bytes = new();
                foreach (var data in _segments.Select(s => s.Data))
                    bytes.AddRange(data);
                return bytes.ToArray();
            }
        }

        public override string ToString() => 
            string.Join(" ", _segments.Select(s => s is DynamicFrameSegment dfs && s.Data == null ? $"[{dfs.Name}]" : (s is AutoFrameSegment afs && s.Data == null ? $"({afs.Calculator.GetType().Name})" : s.ToString())).ToArray());
    }
}
