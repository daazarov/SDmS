SET IDENTITY_INSERT [dbo].[device_parameters] ON
INSERT INTO [dbo].[device_parameters] ([parameter_id], [description], [default_value], [processing_flag]) VALUES (1, N'DESIRED_TEMPERATURE', N'24', N'X')
INSERT INTO [dbo].[device_parameters] ([parameter_id], [description], [default_value], [processing_flag]) VALUES (2, N'TEMPERATURE_DATA', N'', N'X')
INSERT INTO [dbo].[device_parameters] ([parameter_id], [description], [default_value], [processing_flag]) VALUES (3, N'DESIRED_ENABLED', N'false', N'X')
INSERT INTO [dbo].[device_parameters] ([parameter_id], [description], [default_value], [processing_flag]) VALUES (4, N'LED_ENABLED', N'fasle', N'X')
INSERT INTO [dbo].[device_parameters] ([parameter_id], [description], [default_value], [processing_flag]) VALUES (5, N'LED_INTENSITY', N'100', N'X')
INSERT INTO [dbo].[device_parameters] ([parameter_id], [description], [default_value], [processing_flag]) VALUES (6, N'LED_POWER', N'', NULL)
INSERT INTO [dbo].[device_parameters] ([parameter_id], [description], [default_value], [processing_flag]) VALUES (7, N'LED_MIN_VOLTAGE', N'0', NULL)
INSERT INTO [dbo].[device_parameters] ([parameter_id], [description], [default_value], [processing_flag]) VALUES (8, N'LED_MAX_VOLTAGE', N'0', NULL)
SET IDENTITY_INSERT [dbo].[device_parameters] OFF
