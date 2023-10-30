using HexFrameGen;
using HexFrameGen.AutoFrameSegments;
using Stylet;
using System;

namespace HexFrameGen_Demo.ViewModels
{
    public class RootViewModel : PropertyChangedBase
    {
        private string _title = "HexFrameGen-Demo";
        public string Title
        {
            get { return _title; }
            set { SetAndNotify(ref _title, value); }
        }

        public RootViewModel()
        {
            HexFrame setLd = new();
            StaticFrameSegment header = new("AA 55");
            AutoLengthFrameSegment length = new(2);
            StaticFrameSegment command = new("20");
            DynamicFrameSegment data = new();
            AutoCheckSumFrameSegment crc = new(1);
            data.SetData("01 2c");
            length.Register(length);
            length.Register(command);
            length.Register(data);
            length.Register(crc);
            crc.Register(length);
            crc.Register(command);
            crc.Register(data);
            setLd.AddSegment(header);
            setLd.AddSegment(length);
            setLd.AddSegment(command);
            setLd.AddSegment(data);
            setLd.AddSegment(crc);
        }
    }
}
