using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperaturAdmin.Models
{
    public class Sensoren
    {

        public int sensorNr { get; set; }

        public int serverschrank { get; set; }

        public string adresse { get; set; }

        public int herstellerNr { get; set; }

        public double maxTemperatur { get; set; }

        public int benutzerNr { get; set; }

    }
}
