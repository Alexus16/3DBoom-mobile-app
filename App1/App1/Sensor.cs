using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    class Sensor
    {
        public int id;
        public string Name;
        public int Value;
        public Sensor()
        {
            Name = "default";
            Value = 0;
        }
        public Sensor( int _val, string _name)
        {
            Name = _name;
            Value = _val;
        }
    }
}
