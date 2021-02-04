﻿CREATE TABLE [dbo].[Region]
(
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_Region_Id DEFAULT NEWSEQUENTIALID(),
	[Top] INT NOT NULL,
	[Left] INT NOT NULL,
	[Width] INT NOT NULL,
	[Height] INT NOT NULL,
	[Label] VARCHAR(255) NULL,
	[ImageId] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT PK_Region_Id PRIMARY KEY([Id])
)
