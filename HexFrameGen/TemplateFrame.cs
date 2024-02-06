using HexFrameGen.AutoCalculator;
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
        public List<BaseFrameSegment> Segments;
        public Dictionary<string, DynamicFrameSegment> Dynamic =>
            Segments.Where(s => s is DynamicFrameSegment).Select(s => s as DynamicFrameSegment).Where(s => s.Name != null).ToDictionary(s => s.Name, s => s);

        public TemplateFrame(params BaseFrameSegment[] segments) => Segments = segments.ToList();

        public TemplateFrame(IEnumerable<BaseFrameSegment> segments) => Segments = segments.ToList();

        public TemplateFrame(TemplateFrame frame)
        {
            var autodict = frame.Segments.OfType<AutoFrameSegment>().ToDictionary(s => s, s => s.Clone());
            Segments = frame.Segments.Select(s => s is DynamicFrameSegment dfs ? dfs.Clone() : (s is AutoFrameSegment afs ? autodict[afs] : s)).ToList();
            foreach (var auto in Segments.OfType<AutoFrameSegment>())
            {
                foreach (var dy in Segments.OfType<DynamicFrameSegment>())
                    auto.ExchangeDynamic(dy);
                foreach (var kv in autodict)
                    auto.ExchangeAuto(kv.Key, kv.Value);
            }
        }

        public TemplateFrame(string str, Dictionary<string, IAutoCalculator> calculatorDict)
        {
            Segments = [];
            var autos = new Dictionary<AutoFrameSegment, List<int>>();
            foreach (var s in str.Split(' '))
            {
                if (s.First() == '(') // (2, BytesCount2B, 2-3-4-5-6) (2, BytesCount2B, 2>6) (2, BytesCount2B, 2-4>6-8)
                {
                    var temp = s.Substring(1, s.Length - 2).Split(',');
                    var auto = new AutoFrameSegment(int.Parse(temp[0].Trim())) { Calculator = calculatorDict[temp[1].Trim()] };
                    Segments.Add(auto);
                    autos.Add(auto, []);
                    foreach (var seg in temp[2].Trim().Split('-'))
                    {
                        if (seg.Contains(">"))
                        {
                            var t = seg.Split('>').Select(int.Parse).ToArray();
                            autos[auto].AddRange(Enumerable.Range(t[0], t[1] - t[0] + 1));
                        }
                        else autos[auto].Add(int.Parse(seg));
                    }
                }
                else if (s.First() == '[')
                {
                    Segments.Add(new DynamicFrameSegment(s.Substring(1, s.Length - 2)));
                }
                else
                {
                    Segments.Add(new StaticFrameSegment(s));
                }
            }
            foreach (var kv in autos)
            {
                for (var i = 0; i < Segments.Count; i++)
                {
                    if (kv.Value.Contains(i))
                        kv.Key.Register(Segments[i]);
                }
            }
        }

        public TemplateFrame Clone() => new(this);

        public void AddSegment(BaseFrameSegment segment) => Segments.Add(segment);

        public HexFrame Gen() => new(Data);

        public bool CanGen() => Dynamic.Count(ds => ds.Value.Data == null) < 1;

        public override byte[] Data
        {
            get
            {
                if (!CanGen())
                    throw new InvalidOperationException("There are some DynamicFrames without data: " + string.Concat(Dynamic.Where(kv => kv.Value.Data == null).Select(kv => kv.Key)));
                List<byte> bytes = new();
                foreach (var data in Segments.Select(s => s.Data))
                    bytes.AddRange(data);
                return bytes.ToArray();
            }
        }

        public override string ToString() => 
            string.Join(" ", Segments.Select(s => s is DynamicFrameSegment dfs && s.Data == null ? $"[{dfs.Name}]" : (s is AutoFrameSegment afs && s.Data == null ? $"({afs.Calculator.GetType().Name})" : s.ToString())).ToArray());
    }
}
