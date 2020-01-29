using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Npgsql;

namespace App1
{
    class DataBase
    {
        private static Mutex mutexObj= new Mutex();
        public static bool isConnected = false;
        public static string __ip;
        public static string __user;
        public static string __password;
        public static NpgsqlConnection __conn;
        public static List<Window> windows;
        public static List<Lamp> lamps;
        public static List<Executor> executors;
        public static List<Sensor> sensors;
        public async static Task<bool> connect()
        {
            try
            {
                isConnected = true;
                await __conn.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async static Task<bool> disconnect()
        {
            try
            { 
                isConnected = false;
                await __conn.CloseAsync();
                return true;
            }
            catch
            { 
                return false;
            }
        }
        public async static Task<bool> init()
        {
            mutexObj.WaitOne();
            try
            {
                __ip = App.Current.Properties["ipServer"].ToString();
                __user = App.Current.Properties["user"].ToString();
                __password = App.Current.Properties["password"].ToString();
                string test = $"Host={__ip};Port=5432;UserName={__user};Password={__password};Database=SmartLab";
                __conn = new NpgsqlConnection($"Host={__ip};Port=5432;UserName={__user};Password={__password};Database=SmartLab");
                await __conn.OpenAsync();
                await __conn.CloseAsync();
                mutexObj.ReleaseMutex();
                return true;
            }
            catch
            {
                mutexObj.ReleaseMutex();
                return false;
            }
            
        }
        public async static Task<bool> refreshData()
         {
            mutexObj.WaitOne();
            try
            {
                NpgsqlDataReader reader;
                NpgsqlCommand com;
                lamps = new List<Lamp>();
                executors = new List<Executor>();
                windows = new List<Window>();
                sensors = new List<Sensor>();
                if (isConnected) await disconnect();
                await connect();
                com = new NpgsqlCommand("SELECT \"id\", \"Name\", \"Value\", \"Conf\", \"Auto\" FROM \"Lamps\"", __conn);
                reader = await com.ExecuteReaderAsync();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int _id = Convert.ToInt32(reader.GetValue(0));
                        string _name = Convert.ToString(reader.GetValue(1));
                        int _val = Convert.ToInt32(reader.GetValue(2));
                        string _conf = Convert.ToString(reader.GetValue(3));
                        bool _isAuto = Convert.ToBoolean(reader.GetValue(4));
                        Lamp temp = new Lamp(_id, _name, _isAuto);
                        temp.conf = new Configurator(_conf);
                        temp.curValue = _val;
                        lamps.Add(temp);
                    }
                }
                com.Cancel();
            await disconnect();
            await connect();
                com = new NpgsqlCommand("SELECT \"id\", \"Name\", \"Value\" FROM \"Sensors\"", __conn);
                reader = await com.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        int _id = Convert.ToInt32(reader.GetValue(0));
                        string _name = Convert.ToString(reader.GetValue(1));
                        int _val = Convert.ToInt32(reader.GetValue(2));
                        Sensor temp = new Sensor(_val, _name);
                        temp.id = _id;
                        sensors.Add(temp);
                    }
                }
                com.Cancel();
            await disconnect();
            await connect();
            com = new NpgsqlCommand("SELECT \"id\", \"Name\", \"Value\", \"Conf\", \"Auto\" FROM \"Executors\"", __conn);
                reader = await com.ExecuteReaderAsync(); 
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int _id = Convert.ToInt32(reader.GetValue(0));
                        string _name = Convert.ToString(reader.GetValue(1));
                        int _val = Convert.ToInt32(reader.GetValue(2));
                        string _conf = Convert.ToString(reader.GetValue(3));
                        bool _isAuto = Convert.ToBoolean(reader.GetValue(4));
                        Executor temp = new Executor(_id, _name, _isAuto);
                        temp.conf = new Configurator(_conf);
                        temp.curValue = _val;
                        executors.Add(temp);
                    }
                }
                com.Cancel();
            await disconnect();
            await connect();
            com = new NpgsqlCommand("SELECT \"id\", \"Name\", \"Conf\", \"Auto\", \"Value\", \"minValue\", \"maxValue\" FROM \"Windows\"", __conn);
                reader = await com.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int _id = Convert.ToInt32(reader.GetValue(0));
                        string _name = Convert.ToString(reader.GetValue(1));
                        string _conf = Convert.ToString(reader.GetValue(2));
                        bool _isAuto = Convert.ToBoolean(reader.GetValue(3));
                        int _val1 = Convert.ToInt32(reader.GetValue(4));
                        int _minValue1 = Convert.ToInt32(reader.GetValue(5));
                        int _maxValue1 = Convert.ToInt32(reader.GetValue(6));

                        Window temp = new Window(_id, _name, _isAuto);
                        temp.conf = new Configurator(_conf);
                        temp.curValueOfMotor = _val1;
                        temp.minValueOfMotor = _minValue1;
                        temp.maxValueOfMotor = _maxValue1;
                        windows.Add(temp);
                    }
                }
                await disconnect();
                setFunctions();
                mutexObj.ReleaseMutex();
                return true;
        }
            catch
           {
                mutexObj.ReleaseMutex();
                return false;
          }
        }
        public async static Task<bool> updateData(int id, string tableForUpdate, string nameOfProperty, int valueForUpdate)
        {
            mutexObj.WaitOne();
            try
            {
                if (isConnected) await disconnect();
                await connect();
                NpgsqlCommand com = new NpgsqlCommand($"update \"{tableForUpdate}\" set \"{nameOfProperty}\"={valueForUpdate} where \"id\"={id}", __conn);
                bool res = Convert.ToBoolean(await com.ExecuteNonQueryAsync());
                await disconnect();
                mutexObj.ReleaseMutex();
                return res;
            }
            catch
            {
                mutexObj.ReleaseMutex();
                return false;
            }
        }
        public async static Task<bool> updateData(int id, string tableForUpdate, string nameOfProperty, string valueForUpdate)
        {
            mutexObj.WaitOne();
            try
            {
                if (isConnected) await disconnect();
                await connect();
                NpgsqlCommand com = new NpgsqlCommand($"update \"{tableForUpdate}\" set \"{nameOfProperty}\"={valueForUpdate} where \"id\"={id}", __conn);
                bool res = Convert.ToBoolean(await com.ExecuteNonQueryAsync());
                await disconnect();
                mutexObj.ReleaseMutex();
                return res;
            }
            catch
            {
                mutexObj.ReleaseMutex();
                return false;
            }
        }
        public static Lamp findLampById(int _id)
        {
            for(int i = 0; i < lamps.Count; i++)
            {
                if (lamps[i].id == _id) return lamps[i];
            }
            return null;
        }
        public static Executor findExecutorById(int _id)
        {
            for (int i = 0; i < executors.Count; i++)
            {
                if (executors[i].id == _id) return executors[i];
            }
            return null;
        }
        public static Window findWindowById(int _id)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].id == _id) return windows[i];
            }
            return null;
        }
        public static Sensor findSensorById(int _id)
        {
            for (int i = 0; i < sensors.Count; i++)
            {
                if (sensors[i].id == _id) return sensors[i];
            }
            return null;
        }
        public static int getValueOfSensorById(int _id)
        {
            Sensor tempSensor = findSensorById(_id);
            if (tempSensor != null)
            {
                return tempSensor.Value;
            }
            else
            {
                return -1;
            }
        }
        public static string getNameOfSensorById(int _id)
        {
            Sensor tempSensor = findSensorById(_id);
            if (tempSensor != null)
            {
                return tempSensor.Name;
            }
            else
            {
                return "";
            }
        }
        public static void setFunctions()
        {
            for (int i = 0; i < lamps.Count; i++)
            {
                for (int j = 0; j < lamps[i].conf.items.Count; j++)
                {
                    lamps[i].conf.items[j].setFunc(getNameOfSensorById);
                    lamps[i].conf.items[j].setFunc(getValueOfSensorById);
                }
            }
            for (int i = 0; i < executors.Count; i++)
            {
                for (int j = 0; j < executors[i].conf.items.Count; j++)
                {
                    executors[i].conf.items[j].setFunc(getNameOfSensorById);
                    executors[i].conf.items[j].setFunc(getValueOfSensorById);
                }
            }
            for (int i = 0; i < windows.Count; i++)
            {
                for (int j = 0; j < windows[i].conf.items.Count; j++)
                {
                    windows[i].conf.items[j].setFunc(getNameOfSensorById);
                    windows[i].conf.items[j].setFunc(getValueOfSensorById);
                }
            }
        }
    }
}
