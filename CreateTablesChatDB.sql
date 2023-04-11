use ChatDB
CREATE TABLE [dbo].[Users](
	[user_id] UNIQUEIDENTIFIER PRIMARY KEY,
	[login] [nvarchar](10) NOT NULL,
	[name_user] [nvarchar](30) NOT NULL,
	[password] [nvarchar](50) NOT NULL
	)
GO
CREATE TABLE [dbo].[Chats](
	[chat_id] UNIQUEIDENTIFIER PRIMARY KEY,
	[date_create] [datetime] NOT NULL
	)
GO
CREATE TABLE [dbo].[Chat_Users](
	[chat_users_id] UNIQUEIDENTIFIER PRIMARY KEY,
	[chat_id] UNIQUEIDENTIFIER NOT NULL,
	[user_id] UNIQUEIDENTIFIER NOT NULL,
	[last_view_date] [datetime] NULL
	)
GO
CREATE TABLE [dbo].[Messages](
	[message_id] UNIQUEIDENTIFIER PRIMARY KEY,
	[chat_id] UNIQUEIDENTIFIER NOT NULL,
	[sender_id] UNIQUEIDENTIFIER NOT NULL,
	[textmessage] [nvarchar](1000) NOT NULL,
	[timesend] [datetime] NOT NULL
	)
GO

ALTER TABLE [dbo].[Chat_Users] ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Chat_Users] ADD FOREIGN KEY([chat_id])
REFERENCES [dbo].[Chats] ([chat_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Messages] ADD FOREIGN KEY([chat_id])
REFERENCES [dbo].[Chats] ([chat_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Messages] ADD FOREIGN KEY([sender_id])
REFERENCES [dbo].[Users] ([user_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
INSERT [dbo].[Users] ([user_id], [login], [name_user], [password]) VALUES ('C1F79FE5-7F5F-44F7-AD32-79AE7E891214', 'amusin', N'Мусин, Антон', 'C7o2c7aAhyNntSps39GIJKxZhhoIY1nQ') --Cux00921
GO
INSERT [dbo].[Users] ([user_id], [login], [name_user], [password]) VALUES ('7816D8E9-72BF-44A6-B14B-A4B15D7B8428', 'buja89', N'Бугарев, Евгений', '5Tch5j1Ng/OA8/zDRmsv76sq99Jf9eSS') --Jessie89
GO
INSERT [dbo].[Users] ([user_id], [login], [name_user], [password]) VALUES ('135B49E7-F537-4170-9DB6-980F71BFBDBA', 'vikula', N'Титова, Виктория', '7P4f1CNHzSdlGQu4IGPb5aDPDYjfZjgP') --TR3eWa3y
GO
INSERT [dbo].[Chats] ([chat_id], [date_create]) VALUES ('96430667-E54C-4762-A85B-E17225FB4F82', '2023-03-10 21:21:35.997')
GO
INSERT [dbo].[Chat_Users] ([chat_users_id], [chat_id], [user_id], [last_view_date]) VALUES ('9AF282A9-667E-4756-BB28-25D49BB77D05', '96430667-E54C-4762-A85B-E17225FB4F82', 'C1F79FE5-7F5F-44F7-AD32-79AE7E891214', '2023-03-10 21:25:25.997')
GO
INSERT [dbo].[Chat_Users] ([chat_users_id], [chat_id], [user_id], [last_view_date]) VALUES ('033BA774-BC91-4F2A-9541-CAAAB7E6F95D', '96430667-E54C-4762-A85B-E17225FB4F82', '7816D8E9-72BF-44A6-B14B-A4B15D7B8428', '2023-03-10 21:21:36.997')
GO
INSERT [dbo].[Messages] ([message_id], [chat_id], [sender_id], [textmessage], [timesend]) VALUES ('5255BA3F-57D2-4C6B-A13D-733313406871', '96430667-E54C-4762-A85B-E17225FB4F82', 'C1F79FE5-7F5F-44F7-AD32-79AE7E891214', N'Евгений, привет!', '2023-03-10 21:21:36.997')
GO
INSERT [dbo].[Messages] ([message_id], [chat_id], [sender_id], [textmessage], [timesend]) VALUES ('F5552CA8-7C37-41C1-A0EB-BDFAB8188914', '96430667-E54C-4762-A85B-E17225FB4F82', '7816D8E9-72BF-44A6-B14B-A4B15D7B8428', N'Привет!', '2023-03-10 21:23:30.997')
GO
INSERT [dbo].[Messages] ([message_id], [chat_id], [sender_id], [textmessage], [timesend]) VALUES ('999AC115-CB73-4814-8917-90871A078B3F', '96430667-E54C-4762-A85B-E17225FB4F82', '7816D8E9-72BF-44A6-B14B-A4B15D7B8428', N'Купил билеты?', '2023-03-10 21:23:35.997')
GO
INSERT [dbo].[Messages] ([message_id], [chat_id], [sender_id], [textmessage], [timesend]) VALUES ('6141A9AD-21B3-4C69-AA48-AF2771A70380', '96430667-E54C-4762-A85B-E17225FB4F82', 'C1F79FE5-7F5F-44F7-AD32-79AE7E8912148', N'Пока нет', '2023-03-10 21:25:25.997')
GO