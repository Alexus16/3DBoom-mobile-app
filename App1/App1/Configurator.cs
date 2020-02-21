using System.Collections.Generic;
using System.Text;
using System;

namespace App1
{
    class Configurator
    {
        public List<ConfiguratorItem> items;
        int resultId;
        int currentId = 0;
        public Configurator()
        {
            items = new List<ConfiguratorItem>();
            resultId = 0;
        }
        public void addItemConst(int _val)
        {
            ConfiguratorItem temp = new ConfiguratorItem();
            temp.valueOfConst = _val;
            temp.getNameOfSensor = DataBase.getNameOfSensorById;
            temp.getValueOfSensor = DataBase.getValueOfSensorById;
            temp.isDaughter = false;
            temp.type = 1;
            temp.id = currentId++;
            items.Add(temp);
        }
        public void addItemSensor(int _str)
        {
            ConfiguratorItem temp = new ConfiguratorItem();
            temp.idOfSensor = _str;
            temp.isDaughter = false;
            temp.type = 2;
            temp.getNameOfSensor = DataBase.getNameOfSensorById;
            temp.getValueOfSensor = DataBase.getValueOfSensorById;
            temp.id = currentId++;
            items.Add(temp);
        }
        public void addItemLink(int id1, int id2, int _typeOfMath)
        {
            ConfiguratorItem temp = new ConfiguratorItem();
            temp.getNameOfSensor = DataBase.getNameOfSensorById;
            temp.getValueOfSensor = DataBase.getValueOfSensorById;
            temp.type = 0;
            temp.it1 = findConfigutaorItemById(id1);
            temp.it2 = findConfigutaorItemById(id2);
            findConfigutaorItemById(id1).isDaughter = true;
            findConfigutaorItemById(id2).isDaughter = true;
            temp.typeOfMath = _typeOfMath;
            temp.id = currentId++;
            items.Add(temp);
        }
        public void addItemLink(int id1, int _typeOfMath)
        {
            if (_typeOfMath == 2)
            {
                ConfiguratorItem temp = new ConfiguratorItem();
                temp.type = 0;
                temp.it1 = findConfigutaorItemById(id1);
                findConfigutaorItemById(id1).isDaughter = true;
                temp.typeOfMath = _typeOfMath;
                temp.id = currentId++;
                items.Add(temp);
            }
        }
        public void setResultId(int _id)
        {
            resultId = _id;
        }
        public ConfiguratorItem findConfigutaorItemById(int _id)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].id == _id) return items[i];
            }
            return null;
        }
        public int getValue()
        {
            ConfiguratorItem temp = findConfigutaorItemById(resultId);
            return temp.getValue() == 0 ? 0 : 1;
        }
        public string getName()
        {
            ConfiguratorItem temp = findConfigutaorItemById(resultId);
            return temp.getName();
        }
        public string getConfString()
        {
            string res = "";
            StringBuilder str = new StringBuilder();
            str.AppendFormat("'<Conf>;resId=[{0}];count=[{1}];curId=[{2}];", resultId, items.Count, currentId);
            str.Append("<List>;");
            int id1;
            int id2;
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].it1 != null)
                {
                    id1 = items[i].it1.id;
                }
                else
                {
                    id1 = -1;
                }
                if(items[i].it2 != null)
                {
                    id2 = items[i].it2.id;
                }
                else
                {
                    id2 = -1;
                }
                str.Append("<Item>;");
                str.AppendFormat("id=[{0}];it1id=[{1}];it2id=[{2}];val=[{3}];nameOfSensor=[{4}];type=[{5}];typeOfMath=[{6}];isDaughter=[{7}];", items[i].id, id1, id2, items[i].valueOfConst, items[i].idOfSensor, items[i].type, items[i].typeOfMath, items[i].isDaughter);
                str.Append("</Item>;");
            }
            str.Append("</List>;");
            str.Append("</Conf>'");
            res = str.ToString();
            return res;
        }
        public Configurator(string confStr)
        {
            if (confStr != "")
            {
                string[] components = confStr.Split(';');
                int confBegin = -1;
                int confEnd = -1;
                int listBegin = -1;
                int listEnd = -1;
                int _count = -1;
                int _resId = -1;
                int _curId = -1;
                for (int i = 0; i < components.Length; i++)
                {
                    switch (components[i])
                    {
                        case "<Conf>": confBegin = i; break;
                        case "</Conf>": confEnd = i; break;
                        case "<List>": listBegin = i; break;
                        case "</List>": listEnd = i; break;
                    }
                }
                for (int j = confBegin + 1; j < confEnd; j++)
                {
                    char[] chars = { '[', ']' };
                    if (components[j].Contains("count")) _count = Convert.ToInt32(components[j].Split(chars)[1]);
                    else if (components[j].Contains("resId")) _resId = Convert.ToInt32(components[j].Split(chars)[1]);
                    else if (components[j].Contains("curId")) _curId = Convert.ToInt32(components[j].Split(chars)[1]);
                }
                items = new List<ConfiguratorItem>();
                for (int i = 0; i < _count; i++) items.Add(new ConfiguratorItem());
                currentId = _curId;
                resultId = _resId;
                int curIndex = 0;
                for (int j = listBegin + 1; j < listEnd; j++)
                {
                    char[] chars = { '[', ']' };
                    bool isItem = false;
                    int itemBegin = -1;
                    int itemEnd = -1;
                    int _id = -1;
                    if (components[j].Contains("<Item>"))
                    {
                        isItem = true;
                        itemBegin = j;
                        for (int i = j + 1; i < listEnd; i++)
                        {
                            if (components[i].Contains("</Item>"))
                            {
                                itemEnd = i;
                                break;
                            }
                        }
                        for (int i = itemBegin + 1; i < itemEnd; i++)
                        {
                            if (components[i].Contains("id")) { _id = Convert.ToInt32(components[i].Split(chars)[1]); break; }
                        }
                        if (_id != -1) items[curIndex].id = _id;
                        curIndex++;
                    }

                }
                curIndex = 0;
                for (int j = listBegin + 1; j < listEnd; j++)
                {
                    char[] chars = { '[', ']' };
                    bool isItem = false;
                    bool _isDaughter = false;
                    int itemBegin = -1;
                    int itemEnd = -1;
                    int _it1id = -1;
                    int _it2id = -1;
                    int _val = -1;
                    int _valueOfSensor = -1;
                    int _type = -1;
                    int _typeOfMath = -1;
                    if (components[j].Contains("<Item>"))
                    {
                        isItem = true;
                        itemBegin = j;
                        for (int i = j + 1; i < listEnd; i++)
                        {
                            if (components[i].Contains("</Item>"))
                            {
                                itemEnd = i;
                                break;
                            }
                        }
                        for (int i = itemBegin + 1; i < itemEnd; i++)
                        {
                            if (components[i].Contains("it1id")) _it1id = Convert.ToInt32(components[i].Split(chars)[1]);
                            else if (components[i].Contains("it2id")) _it2id = Convert.ToInt32(components[i].Split(chars)[1]);
                            else if (components[i].Contains("val")) _val = Convert.ToInt32(components[i].Split(chars)[1]);
                            else if (components[i].Contains("nameOfSensor")) _valueOfSensor = Convert.ToInt32(components[i].Split(chars)[1]);
                            else if (components[i].Contains("typeOfMath")) _typeOfMath = Convert.ToInt32(components[i].Split(chars)[1]);
                            else if (components[i].Contains("type")) _type = Convert.ToInt32(components[i].Split(chars)[1]);
                            else if (components[i].Contains("isDaughter")) _isDaughter = Convert.ToBoolean(components[i].Split(chars)[1]);
                        }
                        if (_it1id != -1) items[curIndex].it1 = findConfigutaorItemById(_it1id);
                        if (_it2id != -1) items[curIndex].it2 = findConfigutaorItemById(_it2id);
                        items[curIndex].valueOfConst = _val;
                        items[curIndex].idOfSensor = _valueOfSensor;
                        items[curIndex].type = _type;
                        items[curIndex].typeOfMath = _typeOfMath;
                        items[curIndex].isDaughter = _isDaughter;
                        curIndex++;
                    }
                }
            }
            else
            {
                items = new List<ConfiguratorItem>();
                resultId = 0;
            }
        }
        public void deleteItemWithID(int _id)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].id == _id)
                {
                    if (items[i].it1 != null) items[i].it1.isDaughter = false;
                    if (items[i].it2 != null) items[i].it2.isDaughter = false;
                    items.RemoveAt(i);
                    break;
                }
            }
        }
        public void deleteItemWithDaughtersWithID(int _id)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].id == _id)
                {
                    if (items[i].it1 != null) deleteItemWithDaughtersWithID(items[i].it1.id);
                    //items[i].it1 = null;
                    //items[i].it2 = null;
                    break;
                }
            }
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].id == _id)
                {
                    if (items[i].it2 != null) deleteItemWithDaughtersWithID(items[i].it2.id);
                    //items[i].it1 = null;
                    //items[i].it2 = null;
                    break;
                }
            }
            deleteItemWithID(_id);
        }
    }
}
