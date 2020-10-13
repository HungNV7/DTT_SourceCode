USE [master]
GO
/****** Object:  Database [DTT]    Script Date: 10/10/2020 11:45:09 AM ******/
CREATE DATABASE [DTT]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DTT', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\DTT.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DTT_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\DTT_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DTT] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DTT].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DTT] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DTT] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DTT] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DTT] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DTT] SET ARITHABORT OFF 
GO
ALTER DATABASE [DTT] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DTT] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DTT] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DTT] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DTT] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DTT] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DTT] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DTT] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DTT] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DTT] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DTT] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DTT] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DTT] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DTT] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DTT] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DTT] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DTT] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DTT] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DTT] SET  MULTI_USER 
GO
ALTER DATABASE [DTT] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DTT] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DTT] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DTT] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [DTT] SET DELAYED_DURABILITY = DISABLED 
GO
USE [DTT]
GO
/****** Object:  Table [dbo].[tblDecodeQuestion]    Script Date: 10/10/2020 11:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDecodeQuestion](
	[questionID] [int] NOT NULL,
	[row] [int] NOT NULL,
	[col] [int] NOT NULL,
	[detail] [nvarchar](max) NOT NULL,
	[questionImageName] [nvarchar](max) NULL,
	[questionVideoName] [nvarchar](max) NULL,
	[answer] [nvarchar](max) NOT NULL,
	[questionTypeID] [nvarchar](50) NOT NULL,
	[matchID] [nvarchar](50) NOT NULL,
	[isBackup] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[questionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblDetailMatch]    Script Date: 10/10/2020 11:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDetailMatch](
	[No] [int] NOT NULL,
	[studentID] [int] NOT NULL,
	[matchID] [nvarchar](50) NOT NULL,
	[point] [int] NOT NULL,
	[position] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblMatch]    Script Date: 10/10/2020 11:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMatch](
	[matchID] [nvarchar](50) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[matchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblQuestion]    Script Date: 10/10/2020 11:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblQuestion](
	[questionID] [int] NOT NULL,
	[detail] [nvarchar](max) NOT NULL,
	[questionImageName] [nvarchar](max) NULL,
	[questionVideoName] [nvarchar](max) NULL,
	[answer] [nvarchar](max) NOT NULL,
	[answerImageName] [nvarchar](max) NULL,
	[answerVideoName] [nvarchar](max) NULL,
	[questionTypeID] [nvarchar](50) NOT NULL,
	[position] [int] NULL,
	[matchID] [nvarchar](50) NOT NULL,
	[isBackup] [bit] NOT NULL,
	[note] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[questionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblQuestionType]    Script Date: 10/10/2020 11:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblQuestionType](
	[questionTypeID] [nvarchar](50) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[questionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblStudent]    Script Date: 10/10/2020 11:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblStudent](
	[studentID] [int] NOT NULL,
	[firstName] [nvarchar](50) NULL,
	[lastName] [nvarchar](50) NOT NULL,
	[class] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[studentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[tblDecodeQuestion]  WITH CHECK ADD FOREIGN KEY([matchID])
REFERENCES [dbo].[tblMatch] ([matchID])
GO
ALTER TABLE [dbo].[tblDecodeQuestion]  WITH CHECK ADD FOREIGN KEY([questionTypeID])
REFERENCES [dbo].[tblQuestionType] ([questionTypeID])
GO
ALTER TABLE [dbo].[tblDetailMatch]  WITH CHECK ADD FOREIGN KEY([matchID])
REFERENCES [dbo].[tblMatch] ([matchID])
GO
ALTER TABLE [dbo].[tblDetailMatch]  WITH CHECK ADD FOREIGN KEY([studentID])
REFERENCES [dbo].[tblStudent] ([studentID])
GO
ALTER TABLE [dbo].[tblQuestion]  WITH CHECK ADD FOREIGN KEY([matchID])
REFERENCES [dbo].[tblMatch] ([matchID])
GO
ALTER TABLE [dbo].[tblQuestion]  WITH CHECK ADD FOREIGN KEY([questionTypeID])
REFERENCES [dbo].[tblQuestionType] ([questionTypeID])
GO

INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'0', N'Câu hỏi chính vòng giải mã')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'02', N'Câu hỏi chính vòng vượt chướng ngại vật')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'1', N'Câu hỏi vòng khởi động')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'10', N'Gợi ý ô màu xanh vòng giải mã')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'11', N'Câu hỏi ô màu xanh vòng giải mã')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'2', N'Câu hỏi gợi ý vòng vượt chướng ngại vật')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'20', N'Gợi ý ô màu vàng vòng giải mã')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'21', N'Câu hỏi ô màu vàng vòng giải mã')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'3', N'Câu hỏi vòng tăng tốc')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'31', N'Câu hỏi ô màu đỏ vòng giải mã')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'41', N'Câu hỏi vòng về đích 10đ')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'42', N'Câu hỏi vòng về đích 20đ')
INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'43', N'Câu hỏi vòng về đích 30đ')
USE [master]
GO
ALTER DATABASE [DTT] SET  READ_WRITE 
GO
