using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperaturAdmin.Models
{
    public class Log
    {
        public int logNr { get; set; }

        public int sensorNr { get; set; }

        public int benutzerNr { get; set; }

        public string datum { get; set; }

        public string nachricht { get; set; }
    }
}
