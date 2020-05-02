USE [master]
GO
/****** Object:  Database [BLUSENSE]    Script Date: 4/29/2020 2:52:21 PM ******/
CREATE DATABASE [BLUSENSE]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BLUSENSE', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\BLUSENSE.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BLUSENSE_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\BLUSENSE_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [BLUSENSE] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BLUSENSE].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BLUSENSE] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BLUSENSE] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BLUSENSE] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BLUSENSE] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BLUSENSE] SET ARITHABORT OFF 
GO
ALTER DATABASE [BLUSENSE] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BLUSENSE] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BLUSENSE] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BLUSENSE] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BLUSENSE] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BLUSENSE] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BLUSENSE] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BLUSENSE] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BLUSENSE] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BLUSENSE] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BLUSENSE] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BLUSENSE] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BLUSENSE] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BLUSENSE] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BLUSENSE] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BLUSENSE] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BLUSENSE] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BLUSENSE] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BLUSENSE] SET  MULTI_USER 
GO
ALTER DATABASE [BLUSENSE] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BLUSENSE] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BLUSENSE] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BLUSENSE] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BLUSENSE] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BLUSENSE] SET QUERY_STORE = OFF
GO
USE [BLUSENSE]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetContinentBluBoxData]    Script Date: 4/29/2020 2:52:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_GetContinentBluBoxData]
(
	@Continent			VARCHAR(50)			--查詢條件(全查就提供 %%)
)
RETURNS @Statistic TABLE 
( 
	ColumnName			VARCHAR(50),		--欄位名稱
	ParameterName		VARCHAR(50),		--參數名稱
	ParameterValue		INT					--參數值
)
AS
BEGIN
	WITH 
		CountryBlusBoxs as
		(
			SELECT CountryCode, CountryName, CertifiedProduct, Usage, 'R' + IIF(SUBSTRING(SwVersion,1,1) NOT IN (6, 7), '?', SUBSTRING(SwVersion,1,1)) as SWVersion 
			FROM vwBluBoxs
			WHERE StateCode LIKE @Continent
		)
		INSERT INTO @Statistic (ColumnName, ParameterName, ParameterValue)

		SELECT 'CertifiedProduct' as ColumnName, CertifiedProduct as ParameterName, COUNT(*) as ParameterValue 
		FROM CountryBlusBoxs 
		GROUP BY CertifiedProduct

		UNION ALL

		SELECT 'Usage' as ColumnName, Usage as ParameterName, COUNT(*) as ParameterValue
		FROM CountryBlusBoxs
		GROUP BY Usage

		UNION ALL

		SELECT 'SwVersion' as ColumnName, SWVersion as ParameterName, COUNT(*) as ParameterValue
		FROM CountryBlusBoxs
		GROUP BY SWVersion
	RETURN

END;
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetCountryBluBoxData]    Script Date: 4/29/2020 2:52:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_GetCountryBluBoxData] (@CountryCode CHAR(2))
RETURNS @Statistic TABLE 
(
	ColumnName			VARCHAR(50),		--欄位名稱
	ParameterName		VARCHAR(50),		--參數名稱
	ParameterValue		INT					--參數值
)
AS
BEGIN

	WITH 
		CountryBlusBoxs as
		(
			SELECT CountryCode, CountryName, UseStatus, Usage, 'R' + IIF(SUBSTRING(SwVersion,1,1) NOT IN (6, 7), '?', SUBSTRING(SwVersion,1,1)) as SWVersion 
			FROM vwBluBoxs
			WHERE CountryCode = @CountryCode
		)
		INSERT INTO @Statistic (ColumnName, ParameterName, ParameterValue)

		SELECT 'CertifiedProduct' as ColumnName, UseStatus as ParameterName, COUNT(*) as ParameterValue 
		FROM CountryBlusBoxs 
		GROUP BY UseStatus

		UNION ALL

		SELECT 'Usage' as ColumnName, Usage as ParameterName, COUNT(*) as ParameterValue
		FROM CountryBlusBoxs
		GROUP BY Usage

		UNION ALL

		SELECT 'SwVersion' as ColumnName, SWVersion as ParameterName, COUNT(*) as ParameterValue
		FROM CountryBlusBoxs
		GROUP BY SWVersion

	RETURN;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetSwVersion]    Script Date: 4/29/2020 2:52:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_GetSwVersion](
  @StateCode int    
  )
RETURNS @TABLE TABLE 
(
[SW Version]  nvarchar(10),
Number  INT
) 
AS 
BEGIN
--宣告建立接收資料暫存資料表
DECLARE @TB TABLE ([SW Version]  nvarchar(10) ,Number  INT
) 	
	BEGIN
		IF  @StateCode = 99
		BEGIN
			INSERT INTO @TB([SW Version] ,Number )
			SELECT  'R' + IIF(SUBSTRING(SwVersion,1,1) NOT IN (6, 7), '?', SUBSTRING(SwVersion,1,1)) as [SW Version], COUNT(*) as Number 
			FROM vwBluBoxs 
			GROUP BY 'R' + IIF(SUBSTRING(SwVersion,1,1) NOT IN (6, 7), '?', SUBSTRING(SwVersion,1,1))
		END 
		ELSE
		BEGIN
			INSERT INTO @TB([SW Version] ,Number )
			SELECT  'R' + IIF(SUBSTRING(SwVersion,1,1) NOT IN (6, 7), '?', SUBSTRING(SwVersion,1,1)) as [SW Version], COUNT(*) as Number 
			FROM vwBluBoxs 
			WHERE StateCode = @StateCode
			GROUP BY 'R' + IIF(SUBSTRING(SwVersion,1,1) NOT IN (6, 7), '?', SUBSTRING(SwVersion,1,1))
		END
	END
	--寫入要回傳資料的資料表
	INSERT INTO @TABLE([SW Version] ,Number)
	SELECT * FROM @TB
	
 RETURN 
END 

GO
/****** Object:  Table [dbo].[Countries]    Script Date: 4/29/2020 2:52:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryCode] [varchar](5) NOT NULL,
	[CountryName] [nvarchar](max) NULL,
	[ContinentCode] [varchar](5) NULL,
 CONSTRAINT [PK_COUNTRIES] PRIMARY KEY CLUSTERED 
(
	[CountryCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GlobalParameters]    Script Date: 4/29/2020 2:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GlobalParameters](
	[ParameterName] [varchar](20) NOT NULL,
	[ParameterValue] [varchar](200) NOT NULL,
	[ParameterDesc] [nvarchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BluBoxs]    Script Date: 4/29/2020 2:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BluBoxs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Model] [varchar](4) NOT NULL,
	[SN] [varchar](4) NOT NULL,
	[CountryCode] [varchar](2) NULL,
	[SwVersion] [varchar](10) NULL,
	[UseStatus] [varchar](5) NULL,
	[UseType] [varchar](2) NULL,
	[UpdateTime] [datetime] NOT NULL,
	[ShippingDate] [date] NULL,
 CONSTRAINT [PK_BLUE_BOXS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwBluBoxs]    Script Date: 4/29/2020 2:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







--UseType is Usage
CREATE VIEW [dbo].[vwBluBoxs] 
AS
SELECT a.Id, a.Model + a.SN as Instrument, a.Model, a.SN, IIF(b.ContinentCode IS NULL, '', b.ContinentCode) as ContinentCode, a.CountryCode, 
		IIF(b.CountryName IS NULL, 'Unknown', b.CountryName) as CountryName, 
		IIF(a.UseStatus IS NULL, '', a.UseStatus) as UseStatus, 
		IIF(c.ParameterDesc IS NULL, '', c.ParameterDesc) as Usage, 
		IIF(a.SwVersion IS NULL, '', a.SwVersion) as SwVersion, 
		a.UpdateTime,
		a.ShippingDate
FROM BluBoxs a LEFT JOIN
	 Countries b ON (a.CountryCode = b.CountryCode) LEFT JOIN
	 GlobalParameters c ON (c.ParameterName = 'UseType' AND a.UseType = c.ParameterValue)
GO
/****** Object:  Table [dbo].[RepFiles]    Script Date: 4/29/2020 2:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RepFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Model] [varchar](4) NOT NULL,
	[SN] [varchar](4) NOT NULL,
	[LogDate] [varchar](20) NOT NULL,
	[LogTime] [varchar](20) NOT NULL,
	[TestItem] [varchar](100) NULL,
	[Result] [varchar](50) NULL,
	[SWVersion] [varchar](10) NULL,
	[REPFile] [varchar](max) NULL,
	[LOGFile] [varchar](max) NULL,
	[IMGFile] [varchar](max) NULL,
	[UseStatus] [varchar](10) NULL,
	[LogDateTime] [datetime] NULL,
 CONSTRAINT [PK_RepFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwRepFile]    Script Date: 4/29/2020 2:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[vwRepFile] 
AS
WITH 
	SortRepFile as
	(
		SELECT Id, Model, SN, Model + SN as Instrument, LogDate, LogTime, 
				CASE TestItem
					WHEN 'ViroTrack Acute Dengue NS1 Ag' THEN 'Acute Dengue NS1 Ag'
					WHEN 'CoV2 Ig Ab' THEN 'COVID-19 IgA+M/IgG Ig Ab'
					WHEN 'ViroTrack Duo Dengue' THEN 'Sero Dengue IgG/IgM Ab'
					WHEN 'ViroTrack Sero Dengue Ab' THEN 'Sero Dengue IgG/IgM Ab' 
					ELSE TestItem END as TestItem,
				Result, SWVersion, REPFile, LOGFile, IMGFile, UseStatus, LogDateTime
		FROM dbo.RepFiles
		WHERE TestItem != ''
	)
	SELECT a.Id, a.Model, a.SN, Instrument, a.LogDate, a.LogTime, a.TestItem, a.Result, 
			ISNULL(a.SWVersion, '') as SWVersion, 
			a.REPFile, a.LOGFile, a.IMGFile, a.UseStatus, a.LogDateTime,
			b.CountryCode, c.CountryName, c.ContinentCode, ISNULL(b.UseType, '') as UseType, 
			IIF(ISNULL(d.ParameterDesc, '') NOT IN ('University', 'Hospital', 'Research'), 'Unknown', d.ParameterDesc)  as Usage
	FROM SortRepFile a INNER JOIN
		 BluBoxs b ON (a.Model = b.Model AND a.SN = b.SN) LEFT JOIN
		 Countries c ON (b.CountryCode = c.CountryCode) LEFT JOIN
		 GlobalParameters d ON (d.ParameterName = 'UseType' AND b.UseType = d.ParameterValue)
GO
/****** Object:  Table [dbo].[Measurements]    Script Date: 4/29/2020 2:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Measurements](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SN] [varchar](10) NULL,
	[Model] [varchar](10) NULL,
	[Test_Item] [varchar](max) NOT NULL,
	[Lot_Number] [varchar](max) NULL,
	[Patient_ID] [varchar](max) NULL,
	[Note] [varchar](max) NULL,
	[Sample_Type] [varchar](max) NULL,
	[Date] [date] NOT NULL,
	[Time] [time](7) NOT NULL,
	[Test_1_Type] [varchar](max) NULL,
	[Test_1_Result] [varchar](max) NULL,
	[Test_1_Value] [decimal](18, 0) NULL,
	[Test_1_Lower_CutOff_Value] [decimal](18, 0) NULL,
	[Test_1_Higher_CutOff_Value] [decimal](18, 0) NULL,
	[Test_1_QC_Passed] [varchar](10) NULL,
	[Test_1_QC_Code] [int] NULL,
	[Test_2_Type] [varchar](max) NULL,
	[Test_2_Result] [varchar](max) NULL,
	[Test_2_Value] [decimal](18, 0) NULL,
	[Test_2_Lower_CutOff_Value] [decimal](18, 0) NULL,
	[Test_2_Higher_CutOff_Value] [decimal](18, 0) NULL,
	[Test_2_QC_Passed] [varchar](10) NULL,
	[Test_2_QC_Code] [int] NULL,
	[Test_3_Type] [varchar](max) NULL,
	[Test_3_Result] [varchar](max) NULL,
	[Test_3_Value] [decimal](18, 0) NULL,
	[Test_3_Lower_CutOff_Value] [decimal](18, 0) NULL,
	[Test_3_Higher_CutOff_Value] [decimal](18, 0) NULL,
	[Test_3_QC_Passed] [varchar](10) NULL,
	[Test_3_QC_Code] [int] NULL,
	[Test_4_Type] [varchar](max) NULL,
	[Test_4_Result] [varchar](max) NULL,
	[Test_4_Value] [decimal](18, 0) NULL,
	[Test_4_Lower_CutOff_Value] [decimal](18, 0) NULL,
	[Test_4_Higher_CutOff_Value] [decimal](18, 0) NULL,
	[Test_4_QC_Passed] [varchar](10) NULL,
	[Test_4_QC_Code] [int] NULL,
	[Internal_QC_Passed] [bit] NULL,
	[Camera_Check_Passed] [bit] NULL,
	[Electronics_Check_Passed] [bit] NULL,
	[Mechanics_Check_Passed] [bit] NULL,
	[Optics_Check_Passed] [bit] NULL,
	[Optics_Check_Value] [int] NULL,
	[Error_Code] [varchar](max) NULL,
	[Cartridge_SN] [int] NULL,
	[Test_Item_ID] [int] NOT NULL,
	[SW_Version] [varchar](32) NULL,
 CONSTRAINT [PK_Measurements] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeasurementTests]    Script Date: 4/29/2020 2:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeasurementTests](
	[Patient_ID] [varchar](max) NULL,
	[Model] [varchar](4) NULL,
	[SN] [varchar](4) NULL,
	[No] [int] NULL,
	[Test_Type] [varchar](max) NULL,
	[Test_Result] [varchar](max) NULL,
	[Test_Value] [decimal](18, 0) NULL,
	[Test_Lower_CutOff_Value] [decimal](18, 0) NULL,
	[Test_Higher_CutOff_Value] [decimal](18, 0) NULL,
	[Test_QC_Passed] [varchar](10) NULL,
	[Test_QC_Code] [int] NULL,
	[TestDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sheet1]    Script Date: 4/29/2020 2:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sheet1](
	[Model] [nvarchar](255) NULL,
	[SN] [nvarchar](255) NULL,
	[Date] [datetime] NULL,
	[Location] [nvarchar](255) NULL,
	[Use for] [nvarchar](255) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BluBoxs] ADD  CONSTRAINT [DF_BlueBoxs_UpdateTime]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[RepFiles] ADD  CONSTRAINT [DF_RepFiles_CreateTime]  DEFAULT (getdate()) FOR [LogDateTime]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetBluBoxGroupData]    Script Date: 4/29/2020 2:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GetBluBoxGroupData]
(
	@QueryCondition			VARCHAR(50)			--查詢條件
)
AS
BEGIN
	DECLARE @Sql				NVARCHAR(MAX);
	CREATE TABLE #BluBoxData (GroupName VARCHAR(50), Number INT);

	SET @Sql = N'SELECT ' + @QueryCondition + ', COUNT(*) as Number FROM vwBluBoxs GROUP BY ' + @QueryCondition + ';';

	INSERT INTO #BluBoxData (GroupName, Number)
	EXEC sp_executesql @Sql;
END;
GO
USE [master]
GO
ALTER DATABASE [BLUSENSE] SET  READ_WRITE 
GO
