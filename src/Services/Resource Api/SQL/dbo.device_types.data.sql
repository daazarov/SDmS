SET IDENTITY_INSERT [dbo].[device_types] ON
INSERT INTO [dbo].[device_types] ([device_type_id], [description], [device_category_id]) VALUES (1, N'temperature', 2)
INSERT INTO [dbo].[device_types] ([device_type_id], [description], [device_category_id]) VALUES (2, N'climate controle', 2)
INSERT INTO [dbo].[device_types] ([device_type_id], [description], [device_category_id]) VALUES (3, N'led', 1)
SET IDENTITY_INSERT [dbo].[device_types] OFF
