﻿SET DATEFORMAT ymd
SET ARITHABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF


SET IDENTITY_INSERT PercurrentisDB.dbo.ApprovalRole ON

INSERT PercurrentisDB.dbo.ApprovalRole(Id, Role) VALUES (1, N'Manager')
INSERT PercurrentisDB.dbo.ApprovalRole(Id, Role) VALUES (2, N'ProjectManager')
INSERT PercurrentisDB.dbo.ApprovalRole(Id, Role) VALUES (3, N'COO')

SET IDENTITY_INSERT PercurrentisDB.dbo.ApprovalRole OFF
