SET DATEFORMAT ymd
SET ARITHABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF

SET IDENTITY_INSERT PercurrentisDB.dbo.AddressType ON

INSERT PercurrentisDB.dbo.AddressType(Id, Name) VALUES (1, N'Company')
INSERT PercurrentisDB.dbo.AddressType(Id, Name) VALUES (2, N'Residential')
INSERT PercurrentisDB.dbo.AddressType(Id, Name) VALUES (3, N'Airport')
INSERT PercurrentisDB.dbo.AddressType(Id, Name) VALUES (4, N'Harbor')
INSERT PercurrentisDB.dbo.AddressType(Id, Name) VALUES (5, N'Other')

SET IDENTITY_INSERT PercurrentisDB.dbo.AddressType OFF
