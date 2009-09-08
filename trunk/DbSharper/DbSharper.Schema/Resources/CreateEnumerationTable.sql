SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

IF NOT EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Ref_Enumeration')
	BEGIN
		CREATE TABLE [dbo].[Ref_Enumeration](
			[Name] [varchar](64) NOT NULL,
			[BaseType] [varchar](32) NOT NULL CONSTRAINT [DF_Ref_Enumeration_BaseType]  DEFAULT ('int'),
			[HasFlagsAttribute] [bit] NOT NULL,
			[Description] [nvarchar](128) NULL,
		 CONSTRAINT [PK_Ref_Enumeration] PRIMARY KEY CLUSTERED 
		(
			[Name] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]

		SET ANSI_PADDING OFF
		
		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of enum.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ref_Enumeration', @level2type=N'COLUMN',@level2name=N'Name'
		
		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Base class of enum.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ref_Enumeration', @level2type=N'COLUMN',@level2name=N'BaseType'
		
		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If enum has FlagsAttribute.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ref_Enumeration', @level2type=N'COLUMN',@level2name=N'HasFlagsAttribute'
		
		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ref_Enumeration', @level2type=N'COLUMN',@level2name=N'Description'
		
		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enum type data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ref_Enumeration'
	END