 
    

--MODULOS Y FUNCIONALIDADES
--SET IDENTITY_INSERT [SCH_USUARIOS].[modulos] ON
--INSERT INTO [SCH_USUARIOS].[modulos] ([Id], [Codigo], [Descripcion], [Nombre]) VALUES (1, N'mod_usuarios', N'Usuarios', N'Usuarios')
--SET IDENTITY_INSERT [SCH_USUARIOS].[modulos] OFF
    

SET IDENTITY_INSERT [SCH_USUARIOS].[funcionalidades] ON

INSERT INTO [SCH_USUARIOS].[funcionalidades] ([Id], [Codigo], [Nombre], [Descripcion], [Controlador], [Estado], [ModuloId]) VALUES (4000, N'fun_proveedores', N'Gestion de Proveedores', N'Gestion de Proveedores', N'proveedor', 1, 2)

SET IDENTITY_INSERT [SCH_USUARIOS].[funcionalidades] OFF


       
        
SET IDENTITY_INSERT [SCH_USUARIOS].[acciones] ON
INSERT INTO [SCH_USUARIOS].[acciones] ([Id], [Codigo], [Nombre], [FuncionalidadId]) VALUES (4001, N'GET', N'Visualizar Proveedores', 4000)
INSERT INTO [SCH_USUARIOS].[acciones] ([Id], [Codigo], [Nombre], [FuncionalidadId]) VALUES (4002, N'CREATE', N'Crear Proveedores',4000)
INSERT INTO [SCH_USUARIOS].[acciones] ([Id], [Codigo], [Nombre], [FuncionalidadId]) VALUES (4003, N'UPDATE', N'Editar Proveedores', 4000)
INSERT INTO [SCH_USUARIOS].[acciones] ([Id], [Codigo], [Nombre], [FuncionalidadId]) VALUES (4004, N'DELETE', N'Eliminar Proveedores', 4000)
INSERT INTO [SCH_USUARIOS].[acciones] ([Id], [Codigo], [Nombre], [FuncionalidadId]) VALUES (4005, N'EXPORT', N'Exportar Datos (Excel,PDF)', 4000)

SET IDENTITY_INSERT [SCH_USUARIOS].[acciones] OFF

            

--PARAMETROS
---SET IDENTITY_INSERT [SCH_USUARIOS].[parametros] ON
--INSERT INTO [SCH_USUARIOS].[parametros] ([Id], [Codigo], [Nombre], [Descripcion], [Categoria], [Tipo], [Valor], [EsEditable], [TieneOpciones], [LastModificationTime], [LastModifierUserId], [CreationTime], [CreatorUserId]) VALUES (1, N'UI.PAGE_SIZE', N'Tamaño de la página en las listas', N'Tamaño de las grillas en los listados', 1, 1, N'20', 1, 0, N'2018-06-12 23:00:30', 1, N'2018-06-12 23:00:30', NULL)
--SET IDENTITY_INSERT [SCH_USUARIOS].[parametros] OFF


             
-- MENUS
--SET IDENTITY_INSERT [SCH_USUARIOS].[menus] ON
--INSERT INTO [SCH_USUARIOS].[menus] ([Id], [Descripcion], [Codigo], [Nombre], [ModuloId]) VALUES (1, N'Principal', N'MAIN', N'Menu Principal',1)
--SET IDENTITY_INSERT [SCH_USUARIOS].[menus] OFF


--SET IDENTITY_INSERT [SCH_USUARIOS].[menuitems] ON
--INSERT INTO [SCH_USUARIOS].[menuitems] ([Id], [Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (2, NULL, N'/ParametroSistema', 1, 2, 1, N'fa fa-gear', 1, NULL, 3, N'PARAMETRO', N'Parámetros')
--INSERT INTO [SCH_USUARIOS].[menuitems] ([Id], [Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (3, NULL, N'/Seguridad/Rol', 1, 2, 2, N'fa fa-unlock', 1, NULL, 2, N'ROLES', N'Roles y Permisos')
--INSERT INTO [SCH_USUARIOS].[menuitems] ([Id], [Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (4, NULL, N'/Seguridad/Usuario', 1, 2, 3, N'fa fa-users', 1, NULL, 1, N'USUARIOS', N'Usuarios')
--INSERT INTO [SCH_USUARIOS].[menuitems] ([Id], [Descripcion], [Url], [Estado], [TipoId], [Orden], [Icono], [MenuId], [PadreId], [FuncionalidadId], [Codigo], [Nombre]) VALUES (5, NULL, N'/Sesion', 1, 2, 4, N'fa fa-exchange', 1, NULL, 4, N'SESION', N'Sesiones')
--SET IDENTITY_INSERT [SCH_USUARIOS].[menuitems] OFF


--VIEW


---CATALOGOS
--Tipos Comida / Opciones 
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] ON
INSERT INTO [SCH_CATALOGOS].[tipos_catalogos] ([Id], [nombre], [codigo], [vigente], [tipo_ordenamiento], [editable]) VALUES (3014, N'Tipo de Comida', N'TIPOCOMIDA', 1, N'ORD', 1)
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] OFF

SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] ON
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4284, 3014, N'Desayuno', N'Desayuno', N'DESAYNO', 0, 1, 0)
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4285, 3014, N'Almuerzo', N'Almuerzo', N'ALMUERZO', 0, 1, 0)
SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] OFF

-- Opciones de comida
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] ON
INSERT INTO [SCH_CATALOGOS].[tipos_catalogos] ([Id], [nombre], [codigo], [vigente], [tipo_ordenamiento], [editable]) VALUES (3015, N'Opcion de Comida', N'OPCIONCOMIDA', 1, N'ORD', 1)
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] OFF

SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] ON
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4286, 3015, N'Completo', N'Completo', N'COMPLETO', 0, 1, 0)
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4287, 3015, N'Sin Sopa', N'Sin Sopa', N'SINSOPA', 0, 1, 0)
SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] OFF


--Servicio
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] ON
INSERT INTO [SCH_CATALOGOS].[tipos_catalogos] ([Id], [nombre], [codigo], [vigente], [tipo_ordenamiento], [editable]) VALUES (3016, N'Servicio', N'SERVICIO', 1, N'ORD', 1)
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] OFF

SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] ON
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4288, 3016, N'Servicio Almuersos', N'Almuerzos', N'SALMUERZO', 0, 1, 0)
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4289, 3016, N'Servicio Transporte', N'Trasporte', N'STRASPORTE', 0, 1, 0)
SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] OFF

--Disciplina
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] ON
INSERT INTO [SCH_CATALOGOS].[tipos_catalogos] ([Id], [nombre], [codigo], [vigente], [tipo_ordenamiento], [editable]) VALUES (3017, N'Disciplina', N'DISCIPLINA', 1, N'ORD', 1)
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] OFF

SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] ON
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4290, 3017, N'Disciplina1', N'Disciplina1', N'DISCIPLINA1', 0, 1, 0)
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4291, 3017, N'Disciplina2', N'Disciplina2', N'DISCIPLINA2', 0, 1, 0)
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4292, 3017, N'Disciplina3', N'Disciplina3', N'DISCIPLINA3', 0, 1, 0)
SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] OFF


--Area
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] ON
INSERT INTO [SCH_CATALOGOS].[tipos_catalogos] ([Id], [nombre], [codigo], [vigente], [tipo_ordenamiento], [editable]) VALUES (3018, N'Area', N'AREA', 1, N'ORD', 1)
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] OFF

SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] ON
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4293, 3018, N'civil', N'civil', N'CIVIL', 0, 1, 0)
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4294, 3018, N'eléctrica', N'eléctrica', N'ELECTRICA', 0, 1, 0)
SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] OFF
 

--Accion
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] ON
INSERT INTO [SCH_CATALOGOS].[tipos_catalogos] ([Id], [nombre], [codigo], [vigente], [tipo_ordenamiento], [editable]) VALUES (3019, N'Acción', N'ACCION', 1, N'ORD', 1)
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] OFF

SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] ON
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4300, 3019, N'Solicitudes', N'Solicitudes', N'SOLICITUDES', 0, 1, 0)
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4301, 3019, N'Aprobar', N'Aprobar', N'APROBAR', 0, 1, 0)
SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] OFF


--Tipo Proveedor
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] ON
INSERT INTO [SCH_CATALOGOS].[tipos_catalogos] ([Id], [nombre], [codigo], [vigente], [tipo_ordenamiento], [editable]) VALUES (3020, N'Tipo Proveedor', N'TIPOPROVEEDOR', 1, N'ORD', 1)
SET IDENTITY_INSERT [SCH_CATALOGOS].[tipos_catalogos] OFF

SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] ON
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4320, 3020, N'Tipo Proveedor 1', N'Tipo Proveedor 1', N'TPRO1', 0, 1, 0)
INSERT INTO [SCH_CATALOGOS].[catalogos] ([Id], [TipoCatalogoId], [nombre], [descripcion], [codigo], [predeterminado], [vigente], [ordinal]) VALUES (4321, 3020, N'Tipo Proveedor 2', N'Tipo Proveedor 2', N'TPRO2', 0, 1, 0)
SET IDENTITY_INSERT [SCH_CATALOGOS].[catalogos] OFF


 