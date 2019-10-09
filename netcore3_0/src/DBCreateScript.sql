USE [master]
GO

/****** Object:  Database [pyprflowlocaldb]    Script Date: 9/4/2019 10:28:39 AM ******/
CREATE DATABASE [pyprflowlocaldb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'pyprflowlocaldb', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.DEV2014\MSSQL\DATA\pyprflowlocaldb.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'pyprflowlocaldb_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.DEV2014\MSSQL\DATA\pyprflowlocaldb_log.ldf' , SIZE = 816KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [pyprflowlocaldb] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [pyprflowlocaldb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [pyprflowlocaldb] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET ARITHABORT OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [pyprflowlocaldb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [pyprflowlocaldb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET  ENABLE_BROKER 
GO

ALTER DATABASE [pyprflowlocaldb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [pyprflowlocaldb] SET READ_COMMITTED_SNAPSHOT ON 
GO

ALTER DATABASE [pyprflowlocaldb] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET RECOVERY FULL 
GO

ALTER DATABASE [pyprflowlocaldb] SET  MULTI_USER 
GO

ALTER DATABASE [pyprflowlocaldb] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [pyprflowlocaldb] SET DB_CHAINING OFF 
GO

ALTER DATABASE [pyprflowlocaldb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [pyprflowlocaldb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [pyprflowlocaldb] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [pyprflowlocaldb] SET  READ_WRITE 
GO


