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
using TemperaturClient.Helpers;
using TemperaturClient.Models;

namespace TemperaturClient.ViewModels
{
    public class MainWindowViewModel
    {
        #region Globale Variablen

        public ObservableCollection<Temperaturen> TemperaturItems { get; set; }

        public ObservableCollection<Sensoren> SensorItems { get; set; }

        public ObservableCollection<Temperaturen> Last10Temperaturen { get; set; }

        public ObservableCollection<Temperaturen> Last10TemperaturenDESC { get; set; }

        public ObservableCollection<Sensoren> SensorTemperaturenItems { get; set; }

        public ObservableCollection<SensorTemperatur> SelectAllItems { get; set; }

        public MySQLHelper sql;

        public Logger log;

        #endregion

        //Konstruktor
        public MainWindowViewModel()
        {
            //ConnectionString abrufen
            string connectionString = string.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};", 
                Properties.Settings.Default.dbserver, Properties.Settings.Default.dbdatabase, Properties.Settings.Default.dbuser, Properties.Settings.Default.dbpw);

            //Logger init
            log = new Logger();

            //SQL Helper initialisieren
            sql = new MySQLHelper(connectionString);

            if(sql != null)
            {
                TemperaturItems = new ObservableCollection<Temperaturen>();
                SensorItems = new ObservableCollection<Sensoren>();
                SensorTemperaturenItems = new ObservableCollection<Sensoren>();
                Last10Temperaturen = new ObservableCollection<Temperaturen>();
                Last10TemperaturenDESC = new ObservableCollection<Temperaturen>();
                SelectAllItems = new ObservableCollection<SensorTemperatur>();

                FillTables();
            }

        }

        #region Init

        public bool LoadTemperaturen()
        {
            try
            {
                //Sqls laden
                string sSQL = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "SQL", "SelectTemp.sql"));

                //Temperaturen laden
                TemperaturItems.Clear();
                List<Temperaturen> retVal = sql.getTemperaturen(sSQL);
                if (retVal != null)
                {
                    foreach (Temperaturen el in retVal)
                    {
                        TemperaturItems.Add(el);
                    }
                    //ZusatzAuswertunen zu Temperaturen
                    if (TemperaturItems.Count > 10)
                    {
                        Last10Temperaturen.Clear();
                        Last10TemperaturenDESC.Clear();
                        //es ergibt keinen Sinn bei weniger als 10 Temperaturwerten dieses Sortierung durchzuführen
                        List<Temperaturen> lastRows = TemperaturItems.OrderByDescending(y => y.temperaturID).Take(10).OrderBy(y => y.temperaturID).ToList();
                        List<Temperaturen> lastRowsDesc = TemperaturItems.OrderByDescending(y => y.temperaturID).OrderByDescending(y => y.zeit).Take(10).ToList();

                        foreach (Temperaturen el in lastRows)
                        {
                            Last10Temperaturen.Add(el);
                        }
                        foreach (Temperaturen el in lastRowsDesc)
                        {
                            Last10TemperaturenDESC.Add(el);
                        }
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Laden der Temperaturen", ex);
                return false;
            }

        }

        public bool LoadSensoren()
        {
            try
            {
                //Sqls laden
                string sSQL = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "SQL", "SelectSensor.sql"));

                //Sensoren laden
                SensorItems.Clear();
                List<Sensoren> retValSensoren = sql.getSensoren(sSQL, true, false);
                if (retValSensoren != null)
                {
                    foreach (Sensoren el in retValSensoren)
                    {
                        SensorItems.Add(el);
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

        public bool LoadSensorenTemps()
        {
            try
            {
                //Sqls laden
                string sSQL = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "SQL", "SelectSensorTemp.sql"));

                //Sensoren & Temperaturen laden
                SensorTemperaturenItems.Clear();
                List<Sensoren> retValSensorenT = sql.getSensoren(sSQL, false, true);
                if (retValSensorenT != null)
                {
                    foreach (Sensoren el in retValSensorenT)
                    {
                        SensorTemperaturenItems.Add(el);
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
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Laden der Sensoren+Temperaturen", ex);
                return false;
            }
        }

        public bool LoadAllTables()
        {
            try
            {
                //Sqls laden
                string sSQL = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "SQL", "SelectAll.sql"));

                //Sensoren  & Temperaturen laden
                SensorTemperaturenItems.Clear();
                List<SensorTemperatur> retValSensorenT = sql.getAllData(sSQL);
                if (retValSensorenT != null)
                {
                    foreach (SensorTemperatur el in retValSensorenT)
                    {
                        SelectAllItems.Add(el);
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
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Laden der Sensoren+Temperaturen", ex);
                return false;
            }
        }


        public bool FillTables()
        {
            try
            {
                int retVal = 0;

                if (!LoadSensoren())
                {
                    retVal++;
                }
                if (!LoadTemperaturen())
                {
                    retVal++;
                }
                if (!LoadAllTables())
                {
                    retVal++;
                }
                if (!LoadSensorenTemps())
                {
                    retVal++;
                }

                if (retVal > 0)
                {
                    MessageBox.Show(String.Format("{0} {1} fehlerhaft, bitte Log prüfen!", retVal, retVal == 1 ? " Abfrage war " : "Abfragen waren "));
                }

                return true;
            }
            catch(Exception ex)
            {
                log.writeLog(LogType.ERROR, MethodBase.GetCurrentMethod().Name + ": Fehler beim Füllen der DataGrids", ex);
                return false;
            }

        }

        #endregion

        #region Befehle

        public ICommand UpdateTempCommand
        {
            get { return new DelegateCommand<object>(UpdateTemps); }
        }

        private void UpdateTemps(object context)
        {
            if(sql == null)
            {
                string connectionString = string.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};",
                    Properties.Settings.Default.dbserver, Properties.Settings.Default.dbdatabase, Properties.Settings.Default.dbuser, Properties.Settings.Default.dbpw);

                //SQL Helper initialisieren
                sql = new MySQLHelper(connectionString);
            }

            LoadTemperaturen();
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

        public ICommand UpdateSensorTempCommand
        {
            get { return new DelegateCommand<object>(UpdateSensorTemp); }
        }

        private void UpdateSensorTemp(object context)
        {
            if (sql == null)
            {
                string connectionString = string.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};",
                    Properties.Settings.Default.dbserver, Properties.Settings.Default.dbdatabase, Properties.Settings.Default.dbuser, Properties.Settings.Default.dbpw);


                //SQL Helper initialisieren
                sql = new MySQLHelper(connectionString);
            }
            LoadSensorenTemps();
        }

        public ICommand UpdateAllTempCommand
        {
            get { return new DelegateCommand<object>(UpdateAllSelect); }
        }

        private void UpdateAllSelect(object context)
        {
            if (sql == null)
            {
                string connectionString = string.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};",
                    Properties.Settings.Default.dbserver, Properties.Settings.Default.dbdatabase, Properties.Settings.Default.dbuser, Properties.Settings.Default.dbpw);


                //SQL Helper initialisieren
                sql = new MySQLHelper(connectionString);
            }
            LoadAllTables();
        }


        public ICommand CustomTempCommand
        {
            get { return new DelegateCommand<object>(CustomTemps); }
        }

        private void CustomTemps(object context)
        {
            RandomGenerator randomG = new RandomGenerator();

            randomG.generateRandomTemperaturen(1, 4, 25);
        }

        #endregion
    }
}
