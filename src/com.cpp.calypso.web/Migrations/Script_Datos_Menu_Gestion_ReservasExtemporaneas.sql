
INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (NULL, N'/Proveedor/ReservaHotel/CrearReservasExtemporaneas', 1, 2, 2, N'fa fa-user', 5, (select Id from SCH_USUARIOS.menuitems where codigo='HOSPEDAJE'), NULL, N'RESERVACOMTEPOR', N'Crear Reserva Extempor�nea')
INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (NULL, N'/Proveedor/ReservaHotel/GestionarReservasExtemporaneas', 1, 2, 2, N'fa fa-gear', 5, (select Id from SCH_USUARIOS.menuitems where codigo='HOSPEDAJE'), NULL, N'GESTIONEXTEMPO', N'Gesti�n de Reservas Extempor�neas')




