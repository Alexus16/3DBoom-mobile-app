using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    class Executor
    {
        public int id;
        public string Name;

        public int curValue;
        public bool isAuto;
        public Configurator conf;
        public Executor()
        {
            
        }
        public Executor(int _id, string _Name, bool _isAuto)
        {
            id = _id;
            Name = _Name;
            isAuto = _isAuto;
        }
    }
}
