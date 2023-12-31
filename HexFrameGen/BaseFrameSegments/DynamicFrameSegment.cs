﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFrameGen.BaseFrameSegments
{
    /// <summary>
    /// 动态帧段 不定长帧段
    /// </summary>
    public class DynamicFrameSegment : BaseFrameSegment
    {
        public string Name;

        private byte[] _data;

        public DynamicFrameSegment(string name) => Name = name;

        public override byte[] Data => _data;

        public DynamicFrameSegment Clone()
        {
            var segment = new DynamicFrameSegment(Name);
            if (_data != null)
            {
                segment._data = new byte[_data.Length];
                Array.Copy(_data, segment._data, _data.Length);
            }
            return segment;
        }

        public void SetData(byte data) => _data = [data];

        public void SetData(IEnumerable<byte> data) => _data = data.ToArray();

        public void SetData(string data) => _data = data.Split(' ', '-', '_').Select(s => Convert.ToByte(s, 16)).ToArray();
    }
}
