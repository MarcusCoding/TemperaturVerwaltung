SELECT * FROM temperaturen left join sensoren on sensoren.sensorNr = temperaturen.sensorNr left join hersteller on sensoren.herstellerNr = hersteller.herstellerNr