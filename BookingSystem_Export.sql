USE [master]
GO
/****** Object:  Database [bookingsystem]    Script Date: 3/17/2023 8:15:58 PM ******/
CREATE DATABASE [bookingsystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'bookingsystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\bookingsystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'bookingsystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\bookingsystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [bookingsystem] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [bookingsystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [bookingsystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [bookingsystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [bookingsystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [bookingsystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [bookingsystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [bookingsystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [bookingsystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [bookingsystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [bookingsystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [bookingsystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [bookingsystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [bookingsystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [bookingsystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [bookingsystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [bookingsystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [bookingsystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [bookingsystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [bookingsystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [bookingsystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [bookingsystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [bookingsystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [bookingsystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [bookingsystem] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [bookingsystem] SET  MULTI_USER 
GO
ALTER DATABASE [bookingsystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [bookingsystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [bookingsystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [bookingsystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [bookingsystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [bookingsystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [bookingsystem] SET QUERY_STORE = ON
GO
ALTER DATABASE [bookingsystem] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [bookingsystem]
GO
/****** Object:  Table [dbo].[admin]    Script Date: 3/17/2023 8:15:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[admin](
	[AdminID] [int] NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Phone] [varchar](10) NOT NULL,
	[Email] [varchar](60) NOT NULL,
 CONSTRAINT [PK_admin] PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[customer]    Script Date: 3/17/2023 8:15:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[customer](
	[CustomerID] [int] NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[Phone] [varchar](10) NOT NULL,
	[Email] [varchar](60) NOT NULL,
	[Password] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[employee]    Script Date: 3/17/2023 8:15:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[employee](
	[EmployeeID] [int] NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[Phone] [varchar](10) NOT NULL,
	[Email] [varchar](60) NOT NULL,
	[Services] [text] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[service]    Script Date: 3/17/2023 8:15:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[service](
	[ServiceID] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Description] [text] NOT NULL,
	[Price] [decimal](6, 2) NOT NULL,
	[Established] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[stores]    Script Date: 3/17/2023 8:15:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[stores](
	[StoreID] [int] NOT NULL,
	[Name] [varchar](60) NOT NULL,
	[Address] [varchar](255) NOT NULL,
	[Phone] [varchar](10) NOT NULL,
	[Email] [varchar](60) NOT NULL,
	[EmployeeID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StoreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[transactions]    Script Date: 3/17/2023 8:15:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[transactions](
	[TransactionID] [int] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
	[StoreID] [int] NOT NULL,
	[Date] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [bookingsystem] SET  READ_WRITE 
GO
