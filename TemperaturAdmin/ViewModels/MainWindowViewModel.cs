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
                SelectedSensor = new Sensoren();
                SelectedUser = new Benutzer();
                SelectedLog = new Log();

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

        public bool UserExists(int benutzerNr)
        {
            try
            {
                string checkSql = $"SELECT * from benutzer where benutzerNr = \"{benutzerNr}\"";
                if (sql != null)
                {
                    if (sql.getBenutzerNr(checkSql))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Auslesen des Nutzers", ex);
                return false;
            }
        }

        public bool HerstellerExists(int herstellerNr)
        {
            try
            {
                string checkSql = $"SELECT * from hersteller where herstellerNr = \"{herstellerNr}\"";
                if (sql != null)
                {
                    if (sql.getBenutzerNr(checkSql))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Auslesen der Hersteller", ex);
                return false;
            }
        }

        public bool SensorExits(int sensorNr)
        {
            try
            {
                string checkSql = $"SELECT * from sensoren where sensorNr = \"{sensorNr}\"";
                if (sql != null)
                {
                    if (sql.getBenutzerNr(checkSql))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Auslesen der Hersteller", ex);
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
            if (sql != null)
            {
                if (SelectedUser != null)
                {
                    if (string.IsNullOrEmpty(SelectedUser.anzeigeName))
                    {
                        MessageBox.Show($"AnzeigeName wurde nicht gefüllt!");
                        return;
                    }
                    if (string.IsNullOrEmpty(SelectedUser.name))
                    {
                        MessageBox.Show($"Name wurde nicht gefüllt!");
                        return;
                    }
                    if (SelectedUser.telefonNr < 1)
                    {
                        MessageBox.Show($"Telefonr wurde nicht gefüllt!");
                        return;
                    }

                    string insertSSQL = $"INSERT INTO benutzer (name, anmeldename, telefonnr) VALUES (\"{SelectedUser.name}\", \"{SelectedUser.anzeigeName}\", \"{SelectedUser.telefonNr}\")";
                    if (sql.executeSQL(insertSSQL))
                    {
                        MessageBox.Show($"Erfolgreich den Benutzer angelegt!");
                        LoadBenutzer();
                    }
                    else
                    {
                        MessageBox.Show($"Fehler beim Anlegen des Benutzer!");
                    }

                    LoadSensoren();
                }
            }
        }

        public ICommand AddLogCommand
        {
            get { return new DelegateCommand<object>(AddLog); }
        }

        private void AddLog(object context)
        {
            if (sql != null)
            {
                if (SelectedLog != null)
                {
                    if (string.IsNullOrEmpty(SelectedLog.datum))
                    {
                        MessageBox.Show($"Datum wurde nicht gefüllt!");
                        return;
                    }
                    if (string.IsNullOrEmpty(SelectedLog.nachricht))
                    {
                        MessageBox.Show($"Nachricht wurde nicht gefüllt!");
                        return;
                    }
                    if (SelectedLog.sensorNr < 1)
                    {
                        MessageBox.Show($"SensorNr wurde nicht gefüllt!");
                        return;
                    }
                    if (SelectedLog.benutzerNr < 1)
                    {
                        MessageBox.Show($"BenutzerNr wurde nicht gefüllt!");
                        return;
                    }
                    if (!UserExists(SelectedLog.benutzerNr))
                    {
                        MessageBox.Show($"Der Benutzer mit der ID {SelectedLog.benutzerNr} gibt es nicht!");
                        return;
                    }
                    if (!SensorExits(SelectedLog.sensorNr))
                    {
                        MessageBox.Show($"Der Sensor mit der ID {SelectedLog.sensorNr} gibt es nicht!");
                        return;
                    }

                    string insertSSQL = $"INSERT INTO log (sensorNr, benutzerNr, datum, nachricht) VALUES (\"{SelectedLog.sensorNr}\", \"{SelectedLog.benutzerNr}\", \"{SelectedLog.datum}\", \"{SelectedLog.nachricht}\")";
                    if (sql.executeSQL(insertSSQL))
                    {
                        MessageBox.Show($"Erfolgreich den Logeintrag angelegt!");
                        LoadLogs();
                    }
                    else
                    {
                        MessageBox.Show($"Fehler beim Logeintrag des Benutzer!");
                    }

                    LoadSensoren();
                }
            }
        }

        public ICommand AddSensorCommand
        {
            get { return new DelegateCommand<object>(AddSensor); }
        }

        private void AddSensor(object context)
        {
            if(sql != null)
            {
                if(SelectedSensor != null)
                {
                    if (SelectedSensor.benutzerNr < 1)
                    {
                        MessageBox.Show($"BenutzerNr wurde nicht gefüllt!");
                        return;
                    }
                    if (!UserExists(SelectedSensor.benutzerNr))
                    {
                        MessageBox.Show($"Der Benutzer mit der ID {SelectedSensor.benutzerNr} gibt es nicht!");
                        return;
                    }
                    if (!HerstellerExists(SelectedSensor.herstellerNr))
                    {
                        MessageBox.Show($"Der Hersteller mit der ID {SelectedSensor.herstellerNr} gibt es nicht!");
                        return;
                    }

                    string insertSSQL = $"INSERT INTO sensoren (serverschrank, adresse, herstellerNr, maxtemperatur) VALUES (\"{SelectedSensor.serverschrank}\", \"{SelectedSensor.adresse}\", \"{SelectedSensor.herstellerNr}\", \"{SelectedSensor.maxTemperatur}\")";
                    if (sql.executeSQL(insertSSQL))
                    {
                        string insertSQL = String.Format("INSERT INTO log (sensorNr, benutzerNr, datum, nachricht) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\")", SelectedSensor.sensorNr, SelectedSensor.benutzerNr, DateTime.Now.ToString("dd.MM.yyyy H:mm"), "Neuer Sensor wurde angelegt!");
                        sql.executeSQL(insertSQL);
                        MessageBox.Show($"Erfolgreich den Sensor angelegt!");
                        LoadSensoren();
                    }
                    else
                    {
                        MessageBox.Show($"Fehler beim Anlegen des Sensors!");
                    }
                    
                    LoadSensoren();
                }
            }
        }

        public ICommand EditSensorCommand
        {
            get { return new DelegateCommand<object>(EditSensor); }
        }

        private void EditSensor(object context)
        {
            if (sql != null)
            {
                if (SelectedSensor != null && SelectedSensor.sensorNr > 0)
                {
                    //Überprüfungen
                    if(SelectedSensor.benutzerNr < 1)
                    {
                        MessageBox.Show($"BenutzerNr wurde nicht gefüllt!");
                        return;
                    }
                    if (!UserExists(SelectedSensor.benutzerNr))
                    {
                        MessageBox.Show($"Der Benutzer mit der ID {SelectedSensor.benutzerNr} gibt es nicht!");
                        return;
                    }
                    if (!HerstellerExists(SelectedSensor.herstellerNr))
                    {
                        MessageBox.Show($"Der Hersteller mit der ID {SelectedSensor.herstellerNr} gibt es nicht!");
                        return;
                    }

                    string updateSQL = $"UPDATE sensoren SET serverschrank = \"{SelectedSensor.serverschrank}\", adresse = \"{SelectedSensor.adresse}\"," +
                        $" herstellerNr = \"{SelectedSensor.herstellerNr}\", maxTemperatur = \"{SelectedSensor.maxTemperatur}\" WHERE sensorNr = {SelectedSensor.sensorNr}";
                    if (sql.executeSQL(updateSQL))
                    {
                        string insertSQL = String.Format("INSERT INTO log (sensorNr, benutzerNr, datum, nachricht) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\")", SelectedSensor.sensorNr, SelectedSensor.benutzerNr, DateTime.Now.ToString("dd.MM.yyyy H:mm"), $"Sensor mit ID {SelectedSensor.sensorNr} wurde geändert! Neue MaxTemp: {SelectedSensor.maxTemperatur}");
                        sql.executeSQL(insertSQL);
                        MessageBox.Show($"Erfolgreich den Sensor mit ID {SelectedSensor.sensorNr} geändert!");
                        LoadSensoren();
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Ändern des Sensors!");
                    }

                }
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
            if (SelectedSensor != null && SelectedSensor.sensorNr > 0)
            {
                if (sql != null)
                {
                    string sSQL = String.Format("DELETE FROM sensoren WHERE sensorNr = \"{0}\"", SelectedSensor.sensorNr);
                    if (sql.executeSQL(sSQL))
                    {
                        string insertSQL = String.Format("INSERT INTO log (sensorNr, benutzerNr, datum, nachricht) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\")", SelectedSensor.sensorNr, 1, DateTime.Now.ToString("dd.MM.yyyy H:mm"), $"Sensor mit ID {SelectedSensor.sensorNr} wurde gelöscht!");
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
            if (SelectedUser != null && SelectedUser.benutzerNr > 0)
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
