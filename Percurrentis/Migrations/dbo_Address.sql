SET DATEFORMAT ymd
SET ARITHABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF

SET IDENTITY_INSERT PercurrentisDB.dbo.Address ON

INSERT PercurrentisDB.dbo.Address(Id, AddressName, Street, City, PostalCode, StateProvince, CountryRegionID, Longitude, Latitude) VALUES (1, N'CSi Industries', N'Lissenveld 41', N'Raamsdonksveer', NULL, NULL, 147, N'4.885749', N'51.708474')
INSERT PercurrentisDB.dbo.Address(Id, AddressName, Street, City, PostalCode, StateProvince, CountryRegionID, Longitude, Latitude) VALUES (2, N'CSi Romania', N'Bulevardul Muncii 12', N'Cluj-Napoca', NULL, NULL, 173, N'23.637461', N'46.796105')
INSERT PercurrentisDB.dbo.Address(Id, AddressName, Street, City, PostalCode, StateProvince, CountryRegionID, Longitude, Latitude) VALUES (3, N'CSi China', NULL, NULL, NULL, NULL, 45, NULL, NULL)
INSERT PercurrentisDB.dbo.Address(Id, AddressName, Street, City, PostalCode, StateProvince, CountryRegionID, Longitude, Latitude) VALUES (4, N'Eurotunnel Folkstone', N'Ashford Road', N'Folkstone', N'CT18 8XX', NULL, 224, N'1.12207', N'51.09559')
INSERT PercurrentisDB.dbo.Address(Id, AddressName, Street, City, PostalCode, StateProvince, CountryRegionID, Longitude, Latitude) VALUES (5, N'Eurotunnel Coquelles', N'Eurotunnel', N'Coquelles', NULL, NULL, 72, N'1.81446', N'50.93906')

SET IDENTITY_INSERT PercurrentisDB.dbo.Address OFF
