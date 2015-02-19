USE [master]
GO

/****** Object:  Database [PercurrentisDB]    Script Date: 04/14/2014 16:15:33 ******/
CREATE DATABASE [PercurrentisDB] ON  PRIMARY 
( NAME = N'PercurrentisDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\PercurrentisDB.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PercurrentisDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\PercurrentisDB_log.LDF' , SIZE = 768KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [PercurrentisDB] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PercurrentisDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [PercurrentisDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [PercurrentisDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [PercurrentisDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [PercurrentisDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [PercurrentisDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [PercurrentisDB] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [PercurrentisDB] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [PercurrentisDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [PercurrentisDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [PercurrentisDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [PercurrentisDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [PercurrentisDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [PercurrentisDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [PercurrentisDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [PercurrentisDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [PercurrentisDB] SET  ENABLE_BROKER 
GO

ALTER DATABASE [PercurrentisDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [PercurrentisDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [PercurrentisDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [PercurrentisDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [PercurrentisDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [PercurrentisDB] SET READ_COMMITTED_SNAPSHOT ON 
GO

ALTER DATABASE [PercurrentisDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [PercurrentisDB] SET  READ_WRITE 
GO

ALTER DATABASE [PercurrentisDB] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [PercurrentisDB] SET  MULTI_USER 
GO

ALTER DATABASE [PercurrentisDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [PercurrentisDB] SET DB_CHAINING OFF 
GO

