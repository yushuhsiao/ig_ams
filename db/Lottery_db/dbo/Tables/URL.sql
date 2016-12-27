USE [game_DB]
GO
/****** Object:  Table [dbo].[URL]    Script Date: 2016/2/26 下午 03:37:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[URL](
	[SID] [int] IDENTITY(1,1) NOT NULL,
	[GameType] [varchar](10) NOT NULL,
	[Location] [varchar](10) NOT NULL,
	[Url] [varchar](255) NOT NULL,
	[Status] [int] NOT NULL,
	[Sort] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](20) NOT NULL,
	[LoadTime] [int] NOT NULL,
	[FailCount] [int] NOT NULL,
 CONSTRAINT [PK_URL] PRIMARY KEY CLUSTERED 
(
	[SID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'URL', @level2type=N'COLUMN',@level2name=N'SID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'球種' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'URL', @level2type=N'COLUMN',@level2name=N'GameType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地區' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'URL', @level2type=N'COLUMN',@level2name=N'Location'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'連結' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'URL', @level2type=N'COLUMN',@level2name=N'Url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'URL', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'URL', @level2type=N'COLUMN',@level2name=N'Sort'
GO
