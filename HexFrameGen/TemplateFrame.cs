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
    public class TemplateFrame : FrameSegment, INewable<HexFrame>
    {
        private readonly List<BaseFrameSegment> _segments;
        private readonly Dictionary<string, DynamicFrameSegment> _dynamic = new();

        public TemplateFrame(params BaseFrameSegment[] segments)
        {
            _segments = segments.ToList();
            _dynamic = segments.Where(s => s is DynamicFrameSegment).Select(s => s as DynamicFrameSegment).Where(s => s.Name != null).ToDictionary(s => s.Name, s => s);
        }

        public void AddSegment(BaseFrameSegment segment)
        {
            if (segment is DynamicFrameSegment)
            {
                var ds = segment as DynamicFrameSegment;
                _segments.Add(segment);
                _dynamic.Add(ds.Name, ds);
            }
            else _segments.Add(segment);
        }

        public DynamicFrameSegment GetDynamic(string name) => _dynamic[name];

        public void FixDynamic(string name, IEnumerable<byte> data)
        {
            var dy = _dynamic[name];
            _dynamic.Remove(name);
            var index = _segments.IndexOf(dy);
            _segments[index] = new StaticFrameSegment(data);
        }

        public HexFrame New() => new(Data);

        public override byte[] Data
        {
            get
            {
                if (_dynamic.Count(ds => ds.Value.Data == null) > 0) throw new InvalidOperationException();
                List<byte> bytes = new();
                foreach (var data in _segments.Select(s => s.Data))
                    bytes.AddRange(data);
                return bytes.ToArray();
            }
        }
    }
}
