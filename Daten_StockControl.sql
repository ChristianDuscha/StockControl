USE StockControl
GO

INSERT INTO Benutzer (Rolle, Name, Adresse, Telefon, EMail, Passwort) VALUES
('Admin', 'ADMINISTRATOR', 'Bärstraße 10, 66113 Saarbrücken', '0681164975', 'admin@einzha.de', 'rootroot'),
('Admin', 'Dieter Michels', 'Bergstraße 2, 66113 Saarbrücken', '0681987654', 'd.michels@einzha.de', 'SehrGutesPasswort'),
('Mitarbeiter', 'Hans Müller', 'Rehweg 2, 66113 Saarbrücken', '068154321', 'h.müller@einzha.de', 'hamü'),
('Mitarbeiter', 'Lisa Bosch', 'Talstraße 4, 66113 Saarbrücken', '0681612345', 'lisa.bosch@einzha.de', '123'),
('Mitarbeiter', 'Eva Weber', 'Seeweg 5, 66113 Saarbrücken', '0681498765', 'weber.e@einzha.de', '321');

INSERT INTO Lager (BenutzerID, Lagername, Standort, Bestand) VALUES
(1, 'Zentrallager', 'Mainstraße 1, 66113 Saarbrücken', 1000),
(2, 'Nebenlager Nord', 'Nordstraße 42, 66113 Saarbrücken', 500),
(1, 'Nebenlager Süd', 'Suedstraße 17, 66113 Saarbrücken', 700),
(3, 'Nebenlager Ost', 'Ost-Weg 21, 66113 Saarbrücken', 2000),
(4, 'Nebenlager West', 'Westallee 27, 66113 Saarbrücken', 300);

INSERT INTO Waren (Warennamen, Warentyp) VALUES
('Tisch', 'Möbel'),
('Toastbrot', 'Lebensmittel'),
('Fußball', 'Spielzeug'),
('Monitor', 'Elektronik'),
('Pullover', 'Kleidung');

INSERT INTO Lieferant (Name, Adresse, Telefon) VALUES
('Möbel GmbH', 'Möbelstr. 1, 12345 Musterstadt', '012341234'),
('Elektronik AG', 'Elektronikstr. 2, 12345 Musterstadt', '098769876'),
('Zubehör KG', 'Zubehörweg 3, 12345 Musterstadt', '012398123'),
('Bürobedarf GmbH', 'Bürostr. 4, 12345 Musterstadt', '098761234'),
('Tech Supplies', 'Techstr. 5, 12345 Musterstadt', '012349123');

INSERT INTO Lieferanten_Ware (LieferantenID, WarenID, Lieferdatum, Preis, Stückzahl) VALUES
(1, 1, '2024-01-10', 200.00, 50),
(2, 3, '2024-01-15', 3.50, 30),
(3, 5, '2024-01-20', 5.00, 100),
(4, 4, '2024-01-25', 150.00, 20),
(5, 2, '2024-01-30', 65.00, 70);