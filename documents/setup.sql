--SQL Setup

-- => SensorTabelle
CREATE TABLE `sensoren` (
  `sensorNr` int(11) NOT NULL,
  `serverschrank` int(11) NOT NULL,
  `adresse` varchar(20) NOT NULL,
  `herstellerNr` int(11) NOT NULL,
  `maxtemperatur` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indizes für die Tabelle `sensoren`
--
ALTER TABLE `sensoren`
  ADD PRIMARY KEY (`sensorNr`),

-- AUTO_INCREMENT für Tabelle `sensoren`
--
ALTER TABLE `sensoren`
  MODIFY `sensorNr` int(11) NOT NULL AUTO_INCREMENT;



--- => TemperaturTabelle


CREATE TABLE `temperaturen` (
  `temperaturNr` int(11) NOT NULL,
  `sensorNr` int(11) NOT NULL,
  `zeit` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `temperatur` double NOT NULL,
  `datum` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indizes für die Tabelle `temperaturen`
--
ALTER TABLE `temperaturen`
  ADD PRIMARY KEY (`temperaturNr`);

--
-- AUTO_INCREMENT für Tabelle `temperaturen`
--
ALTER TABLE `temperaturen`
  MODIFY `temperaturNr` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;



--HerstellerTabelle


CREATE TABLE `hersteller` (
  `herstellerNr` int(11) NOT NULL,
  `name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indizes für die Tabelle `sensoren`
--
ALTER TABLE `hersteller`
  ADD PRIMARY KEY (`herstellerNr`);

--
-- AUTO_INCREMENT für Tabelle `hersteller`
--
ALTER TABLE `hersteller`
  MODIFY `herstellerNr` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;



-- => BenutzerTabelle


CREATE TABLE `benutzer` (
  `benutzerNr` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  `anmeldename` varchar(50) NOT NULL,
  `telefonnr` int(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indizes für die Tabelle `benutzer`
--
ALTER TABLE `benutzer`
  ADD PRIMARY KEY (`benutzerNr`);

--
-- AUTO_INCREMENT für Tabelle `benutzer`
--
ALTER TABLE `benutzer`
  MODIFY `benutzerNr` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;



-- => LogTabelle


CREATE TABLE `log` (
  `logNr` int(11) NOT NULL,
  `sensorNr` int(11) NOT NULL,
  `benutzerNr` int(11) NOT NULL,
  `datum` varchar(10) NOT NULL,
  `nachricht` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indizes für die Tabelle `log`
--
ALTER TABLE `log`
  ADD PRIMARY KEY (`logNr`);

--
-- AUTO_INCREMENT für Tabelle `log`
--
ALTER TABLE `log`
  MODIFY `logNr` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;



--Fremdschlüssel setzen
ALTER TABLE `sensoren` ADD CONSTRAINT `hersteller` FOREIGN KEY (`herstellerNr`) REFERENCES `hersteller`(`herstellerNr`) ON DELETE RESTRICT ON UPDATE RESTRICT; 

ALTER TABLE `log` ADD CONSTRAINT `sensor` FOREIGN KEY (`sensorNr`) REFERENCES `sensoren`(`sensorNr`) ON DELETE RESTRICT ON UPDATE RESTRICT; 

ALTER TABLE `log` ADD CONSTRAINT `nutzer` FOREIGN KEY (`benutzerNr`) REFERENCES `benutzer`(`benutzerNr`) ON DELETE RESTRICT ON UPDATE RESTRICT; 

ALTER TABLE `temperaturen` ADD CONSTRAINT `sensoren` FOREIGN KEY (`sensorNr`) REFERENCES `sensoren`(`sensorNr`) ON DELETE RESTRICT ON UPDATE RESTRICT; 

