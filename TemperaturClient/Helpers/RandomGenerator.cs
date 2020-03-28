using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperaturClient.Helpers
{
    public class RandomGenerator
    {
        /// <summary>
        /// Generiert zufällig Daten für die Temperaturen Tabelle
        /// </summary>
        /// <param name="sensorNrV"></param>
        /// <param name="sensorNrB"></param>
        public void generateRandomTemperaturen(int sensorNrV, int sensorNrB, int anzahl)
        {
            string connectionString = string.Format("Server = {0}; Database = {1}; Uid = {2}; Pwd = {3};",
                Properties.Settings.Default.dbserver, Properties.Settings.Default.dbdatabase, Properties.Settings.Default.dbuser, Properties.Settings.Default.dbpw);

            MySQLHelper sql = new MySQLHelper(connectionString);

            Random rnd = new Random();

            for (int i = 0; i <= anzahl; i++)
            {
                int sensorNr = rnd.Next(sensorNrV, sensorNrB);

                int temperatur = rnd.Next(20, 100);

                string insertSQL = String.Format("INSERT INTO `temperaturen`(`sensorNr`, `temperatur`) VALUES ({0}, {1})", sensorNr, temperatur);

                sql.executeSQL(insertSQL);

                System.Threading.Thread.Sleep(15);
            }

         
        }

    }
}
