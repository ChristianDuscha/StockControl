USE master
GO

IF DB_ID(N'StockControl') IS NULL
BEGIN
	CREATE DATABASE StockControl;
END
GO

USE StockControl
GO

IF OBJECT_ID('Lieferanten_Ware') IS NOT NULL
BEGIN
	DROP TABLE Lieferanten_Ware;
END

IF OBJECT_ID('Lager') IS NOT NULL
BEGIN
	DROP TABLE Lager;
END

IF OBJECT_ID('Benutzer') IS NOT NULL
BEGIN
	DROP TABLE Benutzer;
END

IF OBJECT_ID('Waren') IS NOT NULL
BEGIN
	DROP TABLE Waren;
END

IF OBJECT_ID('Lieferant') IS NOT NULL
BEGIN
	DROP TABLE Lieferant;
END

CREATE TABLE Benutzer (
	BenutzerID INT IDENTITY(1,1) PRIMARY KEY,
	Rolle NVARCHAR(255),
	Name NVARCHAR(255),
	Adresse NVARCHAR(255),
	Telefon NVARCHAR(255),
	EMail NVARCHAR(255),
	Passwort NVARCHAR(255)
);

CREATE TABLE Lager (
	LagerID INT IDENTITY(1,1) PRIMARY KEY,
	BenutzerID INT,
	Lagername NVARCHAR(255),
	Standort NVARCHAR(255),
	Bestand DECIMAL,
	FOREIGN KEY (BenutzerID) REFERENCES Benutzer(BenutzerID)
);

CREATE TABLE Waren (
	WarenID INT IDENTITY(1,1) PRIMARY KEY,
	Warennamen NVARCHAR(255),
	Warentyp NVARCHAR(255)
);

CREATE TABLE Lieferant (
	LieferantenID INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(255),
	Adresse NVARCHAR(255),
	Telefon NVARCHAR(50)
);

CREATE TABLE Lieferanten_Ware (
	LieferantenID INT,
	WarenID INT,
	Lieferdatum DATE,
	Preis DECIMAL,
	Stückzahl INT,
	PRIMARY KEY (LieferantenID, WarenID),
	FOREIGN KEY (LieferantenID) REFERENCES Lieferant(LieferantenID),
	FOREIGN KEY (WarenID) REFERENCES Waren(WarenID)
);
