SET DATEFORMAT ymd
SET ARITHABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF


SET IDENTITY_INSERT PercurrentisDB.dbo.ServiceCompanyType ON

INSERT PercurrentisDB.dbo.ServiceCompanyType(Id, Name) VALUES (1, N'Accommodation')
INSERT PercurrentisDB.dbo.ServiceCompanyType(Id, Name) VALUES (2, N'Airline')
INSERT PercurrentisDB.dbo.ServiceCompanyType(Id, Name) VALUES (3, N'Eurotunnel')
INSERT PercurrentisDB.dbo.ServiceCompanyType(Id, Name) VALUES (4, N'Ferry')
INSERT PercurrentisDB.dbo.ServiceCompanyType(Id, Name) VALUES (5, N'Rental Car')
INSERT PercurrentisDB.dbo.ServiceCompanyType(Id, Name) VALUES (6, N'Taxi')

SET IDENTITY_INSERT PercurrentisDB.dbo.ServiceCompanyType OFF
