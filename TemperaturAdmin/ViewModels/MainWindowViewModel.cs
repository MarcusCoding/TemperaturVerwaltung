using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TemperaturAdmin.Helpers;
using TemperaturAdmin.Models;

namespace TemperaturAdmin.ViewModels
{
    public class MainWindowViewModel
    {
        #region Globale Variablen

        Logger log ;

        MySQLHelper sql;

        public ObservableCollection<Benutzer> userItems { get; set; }

        public ObservableCollection<Log> logItems { get; set; }

        public ObservableCollection<Sensoren> sensorItems { get; set; }

        public ObservableCollection<Temperaturen> tempItems { get; set; }

        public Temperaturen SelectedTemp { get; set; }

        public Sensoren SelectedSensor { get; set; }

        public Log SelectedLog { get; set; }

        public Benutzer SelectedUser { get; set; }

        #endregion

        public MainWindowViewModel()
        {
            //ConnectionString abrufen
            string connectionString = string.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};",
                Properties.Settings.Default.dbserver, Properties.Settings.Default.dbdatabase, Properties.Settings.Default.dbuser, Properties.Settings.Default.dbpw);

            //Logger init
            log = new Logger();

            //SQL Helper initialisieren
            sql = new MySQLHelper(connectionString);

            if (sql != null)
            {
                userItems = new ObservableCollection<Benutzer>();
                logItems = new ObservableCollection<Log>();
                sensorItems = new ObservableCollection<Sensoren>();
                tempItems = new ObservableCollection<Temperaturen>();

                FillTables();
            }

        }

        #region Init

        public bool LoadBenutzer()
        {
            try
            {
                //Sqls laden
                string sSQL = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "SQL", "SelectUser.sql"));

                //Sensoren  & Temperaturen laden
                userItems.Clear();
                List<Benutzer> retValUser = sql.getBenutzer(sSQL);
                if (retValUser != null)
                {
                    foreach (Benutzer el in retValUser)
                    {
                        userItems.Add(el);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Laden der Benutzer", ex);
                return false;
            }
        }

        public bool LoadLogs()
        {
            try
            {
                //Sqls laden
                string sSQL = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "SQL", "SelectLogs.sql"));

                //Sensoren  & Temperaturen laden
                logItems.Clear();
                List<Log> retValUser = sql.getLogs(sSQL);
                if (retValUser != null)
                {
                    foreach (Log el in retValUser)
                    {
                        logItems.Add(el);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Laden der Logs", ex);
                return false;
            }
        }

        public bool LoadSensoren()
        {
            try
            {
                //Sqls laden
                string sSQL = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "SQL", "SelectSensor.sql"));

                //Sensoren  & Temperaturen laden
                sensorItems.Clear();
                List<Sensoren> retValUser = sql.getSensoren(sSQL);
                if (retValUser != null)
                {
                    foreach (Sensoren el in retValUser)
                    {
                        sensorItems.Add(el);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Laden der Sensoren", ex);
                return false;
            }
        }

        public bool Loadtemperaturen()
        {
            try
            {
                //Sqls laden
                string sSQL = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "SQL", "SelectTemps.sql"));

                //Sensoren  & Temperaturen laden
                tempItems.Clear();
                List<Temperaturen> retValUser = sql.getTemperaturen(sSQL);
                if (retValUser != null)
                {
                    foreach (Temperaturen el in retValUser)
                    {
                        tempItems.Add(el);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Laden der Temperaturen", ex);
                return false;
            }
        }

        public bool FillTables()
        {
            try
            {
                int retVal = 0;

                if (!LoadBenutzer())
                {
                    retVal++;
                }
                if (!LoadLogs())
                {
                    retVal++;
                }
                if (!LoadSensoren())
                {
                    retVal++;
                }
                if (!Loadtemperaturen())
                {
                    retVal++;
                }

                if (retVal > 0)
                {
                    MessageBox.Show(String.Format("{0} {1} fehlerhaft, bitte Log prüfen!", retVal, retVal == 1 ? " Abfrage war " : "Abfragen waren "));
                }

                return true;
            }
            catch (Exception ex)
            {
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Füllen der DataGrids", ex);
                return false;
            }

        }

        #endregion

        #region Befehle


        public ICommand UpdateUserCommand
        {
            get { return new DelegateCommand<object>(UpdateUser); }
        }

        private void UpdateUser(object context)
        {
            if (sql == null)
            {
                string connectionString = string.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};",
                    Properties.Settings.Default.dbserver, Properties.Settings.Default.dbdatabase, Properties.Settings.Default.dbuser, Properties.Settings.Default.dbpw);

                //SQL Helper initialisieren
                sql = new MySQLHelper(connectionString);
            }

            LoadBenutzer();
        }

        public ICommand UpdateTempCommand
        {
            get { return new DelegateCommand<object>(UpdateTemp); }
        }

        private void UpdateTemp(object context)
        {
            if (sql == null)
            {
                string connectionString = string.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};",
                    Properties.Settings.Default.dbserver, Properties.Settings.Default.dbdatabase, Properties.Settings.Default.dbuser, Properties.Settings.Default.dbpw);

                //SQL Helper initialisieren
                sql = new MySQLHelper(connectionString);
            }

            Loadtemperaturen();
        }

        public ICommand UpdateLogCommand
        {
            get { return new DelegateCommand<object>(UpdateLog); }
        }

        private void UpdateLog(object context)
        {
            if (sql == null)
            {
                string connectionString = string.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};",
                    Properties.Settings.Default.dbserver, Properties.Settings.Default.dbdatabase, Properties.Settings.Default.dbuser, Properties.Settings.Default.dbpw);

                //SQL Helper initialisieren
                sql = new MySQLHelper(connectionString);
            }

            LoadLogs();
        }

        public ICommand UpdateSensorCommand
        {
            get { return new DelegateCommand<object>(UpdateSensor); }
        }

        private void UpdateSensor(object context)
        {
            if (sql == null)
            {
                string connectionString = string.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};",
                    Properties.Settings.Default.dbserver, Properties.Settings.Default.dbdatabase, Properties.Settings.Default.dbuser, Properties.Settings.Default.dbpw);

                //SQL Helper initialisieren
                sql = new MySQLHelper(connectionString);
            }

            LoadSensoren();
        }

        public ICommand AddUserCommand
        {
            get { return new DelegateCommand<object>(AddUser); }
        }

        private void AddUser(object context)
        {
            if(sql != null)
            {

            }
        }


        public ICommand RemoveTempCommand
        {
            get { return new DelegateCommand<object>(RemoveTemp); }
        }

        private void RemoveTemp(object context)
        {
            if(SelectedTemp != null)
            {
                if (sql != null)
                {
                    string sSQL = String.Format("DELETE FROM temperaturen WHERE temperaturNr = \"{0}\"", SelectedTemp.temperaturID);
                    if (sql.executeSQL(sSQL))
                    {
                        string insertSQL = String.Format("INSERT INTO log (sensorNr, benutzerNr, datum, nachricht) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\")", SelectedTemp.sensorID, 1, DateTime.Now.ToString("dd.MM.yyyy H:mm"), $"Temperaturwert mit ID {SelectedTemp.temperaturID} wurde gelöscht!");
                        sql.executeSQL(insertSQL);
                        Loadtemperaturen();
                    }
                }
            }

        }

        public ICommand RemoveSensorCommand
        {
            get { return new DelegateCommand<object>(RemoveSensor); }
        }

        private void RemoveSensor(object context)
        {
            if (SelectedSensor != null)
            {
                if (sql != null)
                {
                    string sSQL = String.Format("DELETE FROM sensoren WHERE sensorNr = \"{0}\"", SelectedSensor.sensorNr);
                    if (sql.executeSQL(sSQL))
                    {
                        string insertSQL = String.Format("INSERT INTO log (sensorNr, benutzerNr, datum, nachricht) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\")", SelectedSensor.sensorNr, 1, DateTime.Now.ToString("dd.MM.yyyy H:mm"), $"Sensor mit ID {SelectedTemp.sensorID} wurde gelöscht!");
                        sql.executeSQL(insertSQL);
                        LoadSensoren();
                    }
                }
            }

        }


        public ICommand RemoveUserCommand
        {
            get { return new DelegateCommand<object>(RemoveUser); }
        }

        private void RemoveUser(object context)
        {
            if (SelectedUser != null)
            {
                if (sql != null)
                {
                    string sSQL = String.Format("DELETE FROM benutzer WHERE benutzerNr = \"{0}\"", SelectedUser.benutzerNr);
                    if (sql.executeSQL(sSQL))
                    {
                        string insertSQL = String.Format("INSERT INTO log (sensorNr, benutzerNr, datum, nachricht) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\")", 0, 1, DateTime.Now.ToString("dd.MM.yyyy H:mm"), $"Nutzer mit ID {SelectedUser.benutzerNr} wurde gelöscht!");
                        sql.executeSQL(insertSQL);
                        LoadSensoren();
                    }
                }
            }

        }

        #endregion
    }
}
