USE [game_DB]
GO
/****** Object:  Table [dbo].[Pc_Keno_Sample]    Script Date: 2016/2/26 下午 03:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pc_Keno_Sample](
	[SID] [int] IDENTITY(1,1) NOT NULL,
	[Location] [varchar](10) NOT NULL,
	[UserID] [varchar](20) NOT NULL,
	[Dish] [varchar](10) NOT NULL,
	[BallSmall] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_BallSmall]  DEFAULT ('{"Enable":1,"Name":"小","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":2,"BetMax":500,"SingelMax":5000,"DishMax":12345}'),
	[BallMid] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_BallMid]  DEFAULT ('{"Enable":1,"Name":"中","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":2,"BetMax":500,"SingelMax":5000,"DishMax":12345}'),
	[BallBig] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_BallBig]  DEFAULT ('{"Enable":1,"Name":"大","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":500,"SingelMax":5000,"DishMax":12345}'),
	[BallOdd] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_BallOdd]  DEFAULT ('{"Enable":1,"Name":"奇","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[BallSam] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_BallSam]  DEFAULT ('{"Enable":1,"Name":"和","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[BallEven] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_BallEven]  DEFAULT ('{"Enable":1,"Name":"偶","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[SumBig] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_SumBig]  DEFAULT ('{"Enable":1,"Name":"大","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[SumSmall] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_SumSmall]  DEFAULT ('{"Enable":1,"Name":"小","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[SumOdd] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_SumOdd]  DEFAULT ('{"Enable":1,"Name":"單","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[SumEven] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_SumEven]  DEFAULT ('{"Enable":1,"Name":"雙","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[SumBigOdd] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_SumBigOdd]  DEFAULT ('{"Enable":0,"Name":"大單","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[SumSmallOdd] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_SumSmallOdd]  DEFAULT ('{"Enable":0,"Name":"小單","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":0,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[SumBigEven] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_SumBigEven]  DEFAULT ('{"Enable":0,"Name":"大雙","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":0,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[SumSmallEven] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_SumSmallEven]  DEFAULT ('{"Enable":0,"Name":"小雙","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":0,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[Range_1] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_Range_1]  DEFAULT ('{"Enable":0,"Name":"金","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[Range_2] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_Range_2]  DEFAULT ('{"Enable":0,"Name":"木","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[Range_3] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_Range_3]  DEFAULT ('{"Enable":0,"Name":"水","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[Range_4] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_Range_4]  DEFAULT ('{"Enable":0,"Name":"火","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[Range_5] [varchar](255) NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_Range_5]  DEFAULT ('{"Enable":0,"Name":"土","Odds":1.97,"Water":1.1,"WaterBack":0.05,"BetMin":1,"BetMax":1,"SingelMax":5000,"DishMax":10000}'),
	[BallMaxMonney] [int] NOT NULL CONSTRAINT [DF_Pc_Keno_Sample_BallMaxMonney]  DEFAULT ((100000)),
	[UpdateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](20) NOT NULL,
	[CompanyID] [int] NOT NULL,
 CONSTRAINT [PK_Pc_Keno_Sample] PRIMARY KEY CLUSTERED 
(
	[SID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'BallSmall'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'BallMid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'BallBig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奇' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'BallOdd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'和' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'BallSam'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'偶' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'BallEven'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總和大' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'SumBig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總和小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'SumSmall'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總和單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'SumOdd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總和雙' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'SumEven'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總和大單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'SumBigOdd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總和小單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'SumSmallOdd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總和大雙' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'SumBigEven'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總和小雙' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'SumSmallEven'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'五行-金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'Range_1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'五行-木' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'Range_2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'五行-水' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'Range_3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'五行-火' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'Range_4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'五行-土' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'Range_5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'彩票最大收注額:
BallLocationMax_1 +
BallLocationMax_2 +
BallLocationMax_3 +
BallLocationMax_4 +
BallLocationMax_5 +
BallLocationMax_6 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pc_Keno_Sample', @level2type=N'COLUMN',@level2name=N'BallMaxMonney'
GO
