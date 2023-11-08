using HandyControl.Controls;
using HandyControl.Tools.Command;
using HexFrameGen;
using HexFrameGen.AutoCalculator;
using HexFrameGen.BaseFrameSegments;
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
            var calculators = AutoCalculatorGetter.GetAutoCalculatorByDll("AutoCalculators/DefaultAutoCalculator.dll");
            var header = new StaticFrameSegment("AA 55");
            var length = new AutoFrameSegment(2) { Calculator = calculators["BytesCount2B"] };
            var command = new DynamicFrameSegment("Command");
            var data = new DynamicFrameSegment("Data");
            var crc = new AutoFrameSegment(1) { Calculator = calculators["CheckSum1B"] };
            length.Register(length, command, data, crc);
            crc.Register(length, command, data);
            command.SetData("16");
            data.SetData("03 E8");
            var template = new TemplateFrame(header, length, command, data, crc);
            Console.WriteLine(template);
        }

        public string ClickBtnText => "Click Me!";

        public RelayCommand Click => new(s =>
        {
        });
    }
}
