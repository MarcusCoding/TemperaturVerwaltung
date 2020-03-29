using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TemperaturAdmin.Models;

namespace TemperaturAdmin.Helpers
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
                        using (MySqlDataReader reader = cmd.ExecuteReader())
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
                                };
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

        public List<Benutzer> getBenutzer(string sql)
        {
            Converter converter = new Converter();
            List<Benutzer> retVal = new List<Benutzer>();
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
                                Benutzer dataRow = new Benutzer()
                                {
                                    benutzerNr = converter.ConvertToInt(reader[0].ToString()),
                                    name =reader[1].ToString(),
                                    anzeigeName = reader[2].ToString(),
                                    telefonNr = converter.ConvertToInt(reader[3].ToString()),
                                };
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


        public List<Log> getLogs(string sql)
        {
            Converter converter = new Converter();
            List<Log> retVal = new List<Log>();
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
                                Log dataRow = new Log()
                                {
                                    logNr = converter.ConvertToInt(reader[0].ToString()),
                                    sensorNr = converter.ConvertToInt(reader[1].ToString()),
                                    benutzerNr = converter.ConvertToInt(reader[2].ToString()),
                                    datum = reader[3].ToString(),
                                    nachricht = reader[4].ToString(),
                                };
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


        public List<Sensoren> getSensoren(string sql)
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
                                };
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

        public bool getBenutzerNr(string sql)
        {
            Converter converter = new Converter();
            if (string.IsNullOrEmpty(sql))
            {
                return false;
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
                                return false;
                            }
                            while (reader.Read())
                            {
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + "Fehler beim Ausführen des Benutzer-Read-SQls", ex);
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + sql);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool getHerstellerNr(string sql)
        {
            Converter converter = new Converter();
            if (string.IsNullOrEmpty(sql))
            {
                return false;
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
                                return false;
                            }
                            while (reader.Read())
                            {
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + "Fehler beim Ausführen des Hersteller-Read-SQls", ex);
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + sql);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool genSensorNr(string sql)
        {
            Converter converter = new Converter();
            if (string.IsNullOrEmpty(sql))
            {
                return false;
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
                                return false;
                            }
                            while (reader.Read())
                            {
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + "Fehler beim Ausführen des Sensoren-Read-SQls", ex);
                    log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": " + sql);
                    return false;
                }
            }
            else
            {
                return false;
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
