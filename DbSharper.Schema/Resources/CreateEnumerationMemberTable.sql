SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

IF NOT EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Ref_EnumerationMember')
	BEGIN
		CREATE TABLE [dbo].[Ref_EnumerationMember](
			[EnumerationName] [varchar](64) NOT NULL,
			[Name] [varchar](64) NOT NULL,
			[Value] [bigint] NOT NULL,
			[Description] [nvarchar](128) NOT NULL,
		 CONSTRAINT [PK_Ref_EnumerationMember] PRIMARY KEY CLUSTERED 
		(
			[EnumerationName] ASC,
			[Name] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]

		SET ANSI_PADDING OFF

		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of enum.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ref_EnumerationMember', @level2type=N'COLUMN',@level2name=N'EnumerationName'

		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of enum member.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ref_EnumerationMember', @level2type=N'COLUMN',@level2name=N'Name'

		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Value of enum member.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ref_EnumerationMember', @level2type=N'COLUMN',@level2name=N'Value'

		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of enum member.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ref_EnumerationMember', @level2type=N'COLUMN',@level2name=N'Description'

		EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Enmu member data.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ref_EnumerationMember'

		ALTER TABLE [dbo].[Ref_EnumerationMember]  WITH CHECK ADD  CONSTRAINT [FK_Ref_EnumerationMember_Ref_Enumeration] FOREIGN KEY([EnumerationName])
		REFERENCES [dbo].[Ref_Enumeration] ([Name])

		ALTER TABLE [dbo].[Ref_EnumerationMember] CHECK CONSTRAINT [FK_Ref_EnumerationMember_Ref_Enumeration]
	END