﻿use LogDB

SET ANSI_NULLS ON
 SET QUOTED_IDENTIFIER ON
 CREATE TABLE [dbo].[NlogDBLog] (
     [Id] [int] IDENTITY(1,1) NOT NULL,
     [Application] [nvarchar](50) NOT NULL,
     [Logged] [datetime] NOT NULL,
     [Level] [nvarchar](50) NOT NULL,
     [Message] [nvarchar](max) NOT NULL,
     [Logger] [nvarchar](250) NULL,
     [Callsite] [nvarchar](max) NULL,
     [Exception] [nvarchar](max) NULL,
   CONSTRAINT [PK_dbo.NlogDBLog] PRIMARY KEY CLUSTERED ([Id] ASC)
     WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 ) ON [PRIMARY]