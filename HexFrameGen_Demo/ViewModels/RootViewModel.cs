using HandyControl.Controls;
using HandyControl.Tools.Command;
using HexFrameGen;
using HexFrameGen.BaseFrameSegments;
using HexFrameGen.BaseFrameSegments.AutoFrameSegments;
using HexFrameGen.ComplexFrameSegments;
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
            
        }

        public string ClickBtnText => "Click Me!";

        public RelayCommand Click => new(s =>
        {
            HexFrame frame = new();
            StaticFrameSegment header = new("AA 55");
            AutoLengthFrameSegment length = new(2);
            FixedFrameSegment command = new(1);
            DynamicFrameSegment data = new();
            AutoCheckSumFrameSegment crc = new(1);
            command.SetData("20");
            data.SetData("01 2c");
            ComplexFrameSegment ncs = new() { length, command, data };
            ComplexFrameSegment nc = new() { ncs, crc };
            length.Register(nc);
            crc.Register(ncs);
            frame.Add(header);
            frame.Add(nc);
            Console.WriteLine(frame);
        });
    }
}
