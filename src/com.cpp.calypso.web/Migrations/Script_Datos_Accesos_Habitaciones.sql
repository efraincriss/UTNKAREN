

-- TIPO CATALOGO
insert 
SCH_CATALOGOS.tipos_catalogos(codigo,nombre,editable,tipo_ordenamiento,vigente) 
values 
('TIPOHABITACION','TIPO DE HABITACION',1,'ORD',1);



-- CATALOGO HOSPEDAJE
insert SCH_CATALOGOS.catalogos
(TipoCatalogoId,codigo,nombre,descripcion,predeterminado,ordinal,vigente) 
values
((select id from SCH_CATALOGOS.tipos_catalogos where codigo='TIPOHABITACION'),
'TH_SIMPLE',
N'Habitaci�n Simple',
N'Habitaci�n Simple',0,1,1);

insert SCH_CATALOGOS.catalogos
(TipoCatalogoId,codigo,nombre,descripcion,predeterminado,ordinal,vigente) 
values
((select id from SCH_CATALOGOS.tipos_catalogos where codigo='TIPOHABITACION'),
'TH_DOBLE',
N'Habitaci�n Doble',
N'Habitaci�n Doble',0,1,1);

insert SCH_CATALOGOS.catalogos
(TipoCatalogoId,codigo,nombre,descripcion,predeterminado,ordinal,vigente) 
values
((select id from SCH_CATALOGOS.tipos_catalogos where codigo='TIPOHABITACION'),
'TH_TRPLE',
N'Habitaci�n Triple',
N'Habitaci�n Triple',0,1,1);

insert SCH_CATALOGOS.catalogos
(TipoCatalogoId,codigo,nombre,descripcion,predeterminado,ordinal,vigente) 
values
((select id from SCH_CATALOGOS.tipos_catalogos where codigo='ACCIONCOL'),
'ACCESO',
N'Requisito de Acceso',
N'Requisito de Acceso',0,1,1);

-- Modulo
INSERT [SCH_USUARIOS].[modulos] ([Codigo], [Nombre], [Descripcion]) VALUES ('mod_accesos', 'Accesos', 'Accesos')
INSERT [SCH_USUARIOS].[modulos] ([Codigo], [Nombre], [Descripcion]) VALUES ('mod_transporte', 'Transporte', 'Transporte')

-- Menus
INSERT [SCH_USUARIOS].[menus] ([Descripcion], [ModuloId], [Codigo], [Nombre]) 
VALUES ('Accesos', (select id from SCH_USUARIOS.modulos where codigo='mod_accesos'), 'MACCESOS', 'Men� de Accesos')

INSERT [SCH_USUARIOS].[menus] ([Descripcion], [ModuloId], [Codigo], [Nombre]) 
VALUES ('Transportes', (select id from SCH_USUARIOS.modulos where codigo='mod_transporte'), 'MTRANSPORTE', 'Men� de Transporte')

-- Menu Items
INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (NULL, NULL, 1, 1, 6, N'fa fa-building', 5, NULL, NULL, N'HOSPEDAJE', N'Gesti�n de Hospedaje')
INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (NULL, N'/Proveedor/Habitacion/IndexProveedores', 1, 2, 2, N'fa fa-hotel', 5, (select Id from SCH_USUARIOS.menuitems where codigo='HOSPEDAJE'), NULL, N'SER_HOSPEDAJE', N'Gesti�n de Habitaciones')
INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (NULL, N'/Proveedor/ReservaHotel/CrearReservas', 1, 2, 1, N'fa fa-calendar-plus-o', 5, (select Id from SCH_USUARIOS.menuitems where codigo='HOSPEDAJE'), NULL, N'CREAR_RESER', N'Crear Reserva')
INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (NULL, N'/Proveedor/ReservaHotel/GestionarReservas', 1, 2, 2, N'fa fa-calendar', 5, (select Id from SCH_USUARIOS.menuitems where codigo='HOSPEDAJE'), NULL, N'GEST_RESER', N'Gesti�n de Reservas')
INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (NULL, N'/Proveedor/ReservaHotel/PanelControl', 1, 2, 4, N'fa fa-calendar', 5, (select Id from SCH_USUARIOS.menuitems where codigo='HOSPEDAJE'), NULL, N'PANEL_RESER', N'Panel de Control de Reservas')

INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (NULL, NULL, 1, 1, 1, N'fa fa-gears', (select id from SCH_USUARIOS.menus where codigo='MACCESOS'), NULL, NULL, N'SEGURIDAD_PATR', N'Seguridad Patrimonial')
INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (NULL, N'/Accesos/ConsultaPublica/', 1, 2, 1, N'fa fa-gears', (select id from SCH_USUARIOS.menus where codigo='MACCESOS'), (select Id from SCH_USUARIOS.menuitems where codigo='SEGURIDAD_PATR'), NULL, N'CONSU_PUBLICA', N'Consulta P�blica')
INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (NULL, N'/Accesos/TarjetaAcceso/BuscarColaborador?source=tarjetas', 1, 2, 2, N'fa fa-gears', (select id from SCH_USUARIOS.menus where codigo='MACCESOS'), (select Id from SCH_USUARIOS.menuitems where codigo='SEGURIDAD_PATR'), NULL, N'GESTION_TARJET', N'Gesti�n de Tarjetas')
INSERT [SCH_USUARIOS].[menuitems] ([Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES ( NULL, N'/RRHH/ColaboradorRequisito', 1, 2, 2, N'fa fa-user', 6,  (select Id from SCH_USUARIOS.menuitems where codigo='SEGURIDAD_PATR'), NULL, N'VALIDREQUISITO', N'Validaci�n de Requisitos')

