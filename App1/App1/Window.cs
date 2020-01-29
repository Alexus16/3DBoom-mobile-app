using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    class Window
    {
        public int minValueOfMotor;
        public int maxValueOfMotor;
        public int curValueOfMotor;

        public int id;
        public string Name;

        public bool isAuto;
        public Configurator conf;
        public Window()
        {

        }
        public Window(int _id, string _name, bool _isAuto)
        {
            id = _id;
            Name = _name;
            isAuto = _isAuto;
        }
    }
}
