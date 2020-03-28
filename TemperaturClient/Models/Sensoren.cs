using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperaturClient.Models
{
    public class Sensoren
    {

        public int sensorNr { get; set; }

        public int serverschrank { get; set; }

        public string adresse { get; set; }

        public int herstellerNr { get; set; }

        public double maxTemperatur { get; set; }

        //Optionale Daten

        public string herstellerName { get; set; }

        public int TemperaturID { get; set; }

        public double temperatur { get; set; }

        public string zeit { get; set; }

        public double hoechsteTemperatur { get; set; }

        public double durchschnittsTemperatur { get; set; }
    }
}
