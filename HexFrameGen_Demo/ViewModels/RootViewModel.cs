using HandyControl.Controls;
using HandyControl.Tools.Command;
using HexFrameGen;
using HexFrameGen.AutoCalculator;
using HexFrameGen.BaseFrameSegments;
using Stylet;
using System;
using System.IO;

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
            Directory.CreateDirectory("AutoCalculators");
            var calculator = AutoCalculatorGetter.GetAutoCalculatorByDllFromDirectory("AutoCalculators");
            var header = new StaticFrameSegment("AA 55");
            var len = new AutoFrameSegment(2) { Calculator = calculator["BytesCount2B"] };
            var command = new DynamicFrameSegment("Command");
            var data = new DynamicFrameSegment("Data");
            var crc = new AutoFrameSegment(1) { Calculator = calculator["CheckSum1B"] };
            len.Register(len, command, data, crc);
            crc.Register(len, command, data);
            TemplateFrame template = new(header, len, command, data, crc);
            Console.WriteLine(template);
            var ct = new TemplateFrame(template);
            ct.Dynamic["Command"].SetData("20");
            ct.Dynamic["Data"].SetData("03 E8");
            Console.WriteLine(ct.Gen());
        }

        public string ClickBtnText => "Click Me!";

        public RelayCommand Click => new(s =>
        {
        });
    }
}