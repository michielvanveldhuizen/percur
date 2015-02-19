SET DATEFORMAT ymd
SET ARITHABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF


SET IDENTITY_INSERT PercurrentisDB.dbo.Company ON

INSERT PercurrentisDB.dbo.Company(Id, DefaultCompany, Name, AddressID) VALUES (1, CONVERT(bit, 'True'), N'CSi Industries', 1)
INSERT PercurrentisDB.dbo.Company(Id, DefaultCompany, Name, AddressID) VALUES (2, CONVERT(bit, 'True'), N'CSi Romania', 2)
INSERT PercurrentisDB.dbo.Company(Id, DefaultCompany, Name, AddressID) VALUES (3, CONVERT(bit, 'True'), N'CSi China', 3)
INSERT PercurrentisDB.dbo.Company(Id, DefaultCompany, Name, AddressID) VALUES (4, CONVERT(bit, 'True'), N'Guest', NULL)

SET IDENTITY_INSERT PercurrentisDB.dbo.Company OFF
