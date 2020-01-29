using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    class ConfiguratorItem
    {
        public delegate int valOfSensor(int _id);
        public delegate string nameOfSensor(int _id);
        /* types 0-math 1-const 2-sensor 3-time
         * 
         * 
         * 
         */
        public bool isDaughter = false;
        public valOfSensor getValueOfSensor;
        public nameOfSensor getNameOfSensor;
        public int id;
        public int type;
        public int typeOfMath;
        public ConfiguratorItem it1;
        public ConfiguratorItem it2;
        public DateTime time;
        public int valueOfConst;
        public int idOfSensor;
        public ConfiguratorItem()
        {

        }
        public void setFunc(valOfSensor _func)
        {
           getValueOfSensor = _func;
        
        }
        public void setFunc(nameOfSensor _func)
        {
            getNameOfSensor = _func;

        }
        public void setType(int _type)
        {
            type = _type;
        }
        public void setValue(int val)
        {
            if (type == 1) valueOfConst = val;
            else if (type == 2) idOfSensor = val;
        }
        public void setValue(DateTime dt)
        {
            if (type == 3) time = dt;
        }
        public void setTypeOfMath(int _type)
        {
            if (type == 0) typeOfMath = _type;
        }
        public void setIT(int n, ConfiguratorItem _it)
        {
            if (n == 0)
            {
                it1 = _it;
            }
            else if (n == 1)
            {
                it2 = _it;
            }
        }
        public int getValue()
        {
            switch (type)
            {
                case 0:
                    switch (typeOfMath)
                    {
                        case 0:
                            return Convert.ToInt32(Convert.ToBoolean(it1.getValue()) && Convert.ToBoolean(it2.getValue()));
                            break;
                        case 1:
                            return Convert.ToInt32(Convert.ToBoolean(it1.getValue()) || Convert.ToBoolean(it2.getValue()));
                            break;
                        case 2:
                            return Convert.ToInt32(!Convert.ToBoolean(it1.getValue()));
                            break;
                        case 3:
                            return it1.getValue() + it2.getValue();
                            break;
                        case 4:
                            return it1.getValue() - it2.getValue();
                            break;
                        case 5:
                            return it1.getValue() / it2.getValue();
                            break;
                        case 6:
                            return it1.getValue() * it2.getValue();
                            break;
                        case 7:
                            return Convert.ToInt32(it1.getValue() == it2.getValue());
                            break;
                        case 8:
                            return Convert.ToInt32(it1.getValue() > it2.getValue());
                            break;
                        case 9:
                            return Convert.ToInt32(it1.getValue() < it2.getValue());
                            break;
                        case 10:
                            return Convert.ToInt32(it1.getValue() >= it2.getValue());
                            break;
                        case 11:
                            return Convert.ToInt32(it1.getValue() <= it2.getValue());
                            break;
                    }   
                    break;
                case 1:
                    return valueOfConst;
                    break;
                case 2:
                    try
                    {
                        return getValueOfSensor(idOfSensor);
                    }
                    catch { return -1; }
                    break;
                case 3:
                    return -1;
                    break;
                default:
                    return -1;
                    break;
            }
            return -1;
        }
        public string getName()
        {
            switch (type)
            {
                case 0:
                    switch (typeOfMath)
                    {
                        case 0:
                            return "(" + it1.getName() + " И " + it2.getName() + ")";
                            break;
                        case 1:
                            return "(" + it1.getName() + " ИЛИ " + it2.getName() + ")";
                            break;
                        case 2:
                            return "НЕ(" + it1.getName() + ")";
                            break;
                        case 3:
                            return "(" + it1.getName() + " + " + it2.getName() + ")";
                            break;
                        case 4:
                            return "(" + it1.getName() + " - " + it2.getName() + ")";
                            break;
                        case 5:
                            return "(" + it1.getName() + " / " + it2.getName() + ")";
                            break;
                        case 6:
                            return "(" + it1.getName() + " * " + it2.getName() + ")";
                            break;
                        case 7:
                            return "(" + it1.getName() + "=" + it2.getName() + ")";
                            break;
                        case 8:
                            return "(" + it1.getName() + ">" + it2.getName() + ")";
                            break;
                        case 9:
                            return "(" + it1.getName() + "<" + it2.getName() + ")";
                            break;
                        case 10:
                            return "(" + it1.getName() + ">=" + it2.getName() + ")";
                            break;
                        case 11:
                            return "(" + it1.getName() + "<=" + it2.getName() + ")";
                            break;
                    }
                    break;
                case 1:
                    return valueOfConst.ToString();
                    break;
                case 2:
                    try
                    {
                        return getNameOfSensor(idOfSensor);
                    }
                    catch { return "Fatal"; }
                    break;
                case 3:
                    return time.ToString();
                    break;
                default:
                    return "";
                    break;
            }
            return "";
        }
    }
}
