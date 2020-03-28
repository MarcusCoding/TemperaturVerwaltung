using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TemperaturClient.Models;

namespace TemperaturClient.Helpers
{
    public class MySQLHelper
    {
        public MySqlConnection connection;

        public Logger log;

        public MySQLHelper(string ConnectionString)
        {
            log = new Logger();

            if (!string.IsNullOrEmpty(ConnectionString))
            {
                connection = new MySqlConnection(ConnectionString);
                connection.Open();
            }

        }

        public bool executeSQL(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return false;
            }
            if(connection.State == ConnectionState.Open)
            {
                try
                {
                    using(MySqlCommand cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch(Exception ex)
                {
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + "Fehler beim Ausführen des SQls", ex);
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + sql);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<Temperaturen> getTemperaturen(string sql)
        {
            Converter converter = new Converter();
            List<Temperaturen> retVal = new List<Temperaturen>();
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }
            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    {
                        using(MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return null;
                            }
                            while (reader.Read())
                            {
                                Temperaturen dataRow = new Temperaturen()
                                {
                                    sensorID = converter.ConvertToInt(reader[1].ToString()),
                                    temperaturID = converter.ConvertToInt(reader[0].ToString()),
                                    zeit = DateTime.Parse(reader[2].ToString()),
                                    temperatur = converter.ConvertToDouble(reader[3].ToString()),
                                    maxtemperatur = converter.ConvertToDouble(reader[4].ToString()),
                                };
                                if(dataRow.temperatur > dataRow.maxtemperatur)
                                {
                                    dataRow.temperaturUeberschritten = true;
                                }
                                else
                                {
                                    dataRow.temperaturUeberschritten = false;
                                }
                                retVal.Add(dataRow);
                            }
                        }
                    }
                    return retVal;
                }
                catch (Exception ex)
                {
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + "Fehler beim Ausführen des Read-SQls", ex);
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + sql);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<SensorTemperatur> getAllData(string sql)
        {
            Converter converter = new Converter();
            List<SensorTemperatur> retVal = new List<SensorTemperatur>();
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }
            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return null;
                            }
                            while (reader.Read())
                            {
                                SensorTemperatur dataRow = new SensorTemperatur()
                                {
                                    sensorID = converter.ConvertToInt(reader[1].ToString()),
                                    temperaturID = converter.ConvertToInt(reader[0].ToString()),
                                    zeit = DateTime.Parse(reader[2].ToString()),
                                    temperatur = converter.ConvertToDouble(reader[3].ToString()),

                                    sensorNr = converter.ConvertToInt(reader[4].ToString()),
                                    serverschrank = converter.ConvertToInt(reader[5].ToString()),
                                    adresse = reader[5].ToString(),
                                    herstellerNr = converter.ConvertToInt(reader[7].ToString()),
                                    maxTemperatur = converter.ConvertToDouble(reader[8].ToString()),

                                    herstellerID = converter.ConvertToInt(reader[9].ToString()),
                                    herstellerName = reader[10].ToString(),
                                };

                                retVal.Add(dataRow);
                            }
                        }
                    }
                    return retVal;
                }
                catch (Exception ex)
                {
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + "Fehler beim Ausführen des ReadAll-SQls", ex);
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + sql);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<Sensoren> getSensoren(string sql, bool herstellerDaten = false, bool temperaturDaten = false)
        {
            Converter converter = new Converter();
            List<Sensoren> retVal = new List<Sensoren>();
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }
            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return null;
                            }
                            while (reader.Read())
                            {
                                Sensoren dataRow = new Sensoren()
                                {
                                    sensorNr = converter.ConvertToInt(reader[0].ToString()),
                                    serverschrank = converter.ConvertToInt(reader[1].ToString()),
                                    adresse = reader[2].ToString(),
                                    herstellerNr = converter.ConvertToInt(reader[3].ToString()),
                                    maxTemperatur = converter.ConvertToDouble(reader[4].ToString()),
                                    hoechsteTemperatur = converter.ConvertToDouble(reader[5].ToString()),
                                    durchschnittsTemperatur = converter.ConvertToDouble(reader[6].ToString()),
                                };

                                if (herstellerDaten && temperaturDaten == false)
                                {
                                    dataRow.herstellerName = reader[7].ToString();
                                }
                                if (temperaturDaten && herstellerDaten == false)
                                {
                                    dataRow.TemperaturID = converter.ConvertToInt(reader[5].ToString());
                                    dataRow.temperatur = converter.ConvertToDouble(reader[6].ToString());
                                    dataRow.zeit = reader[7].ToString();
                                }

                                retVal.Add(dataRow);
                            }
                        }
                    }
                    return retVal;
                }
                catch (Exception ex)
                {
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + "Fehler beim Ausführen des Read-SQls", ex);
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + sql);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public bool closeConnection()
        {
            if(connection.State == ConnectionState.Open)
            {
                connection.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}
