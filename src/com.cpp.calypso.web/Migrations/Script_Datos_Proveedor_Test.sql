 
SET IDENTITY_INSERT [SCH_SERVICIOS].[paises] ON 

INSERT [SCH_SERVICIOS].[paises] ([Id], [codigo], [nombre], [vigente]) VALUES (1, N'ec', N'Ecuador', 1)
INSERT [SCH_SERVICIOS].[paises] ([Id], [codigo], [nombre], [vigente]) VALUES (2, N'co', N'Colombia', 1)
INSERT [SCH_SERVICIOS].[paises] ([Id], [codigo], [nombre], [vigente]) VALUES (3, N'pe', N'Peru', 1)
SET IDENTITY_INSERT [SCH_SERVICIOS].[paises] OFF


SET IDENTITY_INSERT [SCH_SERVICIOS].[provincias] ON 

INSERT [SCH_SERVICIOS].[provincias] ([Id], [PaisId], [codigo], [nombre], [vigente]) VALUES (1, 1, N'loja', N'loja', 1)
INSERT [SCH_SERVICIOS].[provincias] ([Id], [PaisId], [codigo], [nombre], [vigente]) VALUES (5, 1, N'cuen', N'cuenca', 1)
INSERT [SCH_SERVICIOS].[provincias] ([Id], [PaisId], [codigo], [nombre], [vigente]) VALUES (6, 1, N'quit', N'quito', 1)
SET IDENTITY_INSERT [SCH_SERVICIOS].[provincias] OFF

SET IDENTITY_INSERT [SCH_SERVICIOS].[ciudades] ON
INSERT INTO [SCH_SERVICIOS].[ciudades] ([Id], [ProvinciaId], [codigo], [nombre], [vigente]) VALUES (1, 1, N'loja', N'loja', 1)
INSERT INTO [SCH_SERVICIOS].[ciudades] ([Id], [ProvinciaId], [codigo], [nombre], [vigente]) VALUES (5, 1, N'cuen', N'cuenca', 1)
SET IDENTITY_INSERT [SCH_SERVICIOS].[ciudades] OFF

 

SET IDENTITY_INSERT [SCH_SERVICIOS].[zonas] ON
INSERT INTO [SCH_SERVICIOS].[zonas] ([Id], [codigo], [nombre], [descripcion], [vigente]) VALUES (1, N'z1', N'zona 1', N'zona1 ', 1)
INSERT INTO [SCH_SERVICIOS].[zonas] ([Id], [codigo], [nombre], [descripcion], [vigente]) VALUES (2, N'z2', N'zona 2 - info', N'zona 2', 1)
SET IDENTITY_INSERT [SCH_SERVICIOS].[zonas] OFF



SET IDENTITY_INSERT [SCH_SERVICIOS].[locaciones] ON
INSERT INTO [SCH_SERVICIOS].[locaciones] ([Id], [codigo], [ZonaId], [nombre], [vigente]) VALUES (3, 1, 1, N'locacion 1', 1)
INSERT INTO [SCH_SERVICIOS].[locaciones] ([Id], [codigo], [ZonaId], [nombre], [vigente]) VALUES (4, 2, 2, N'locacion 2', 1)
INSERT INTO [SCH_SERVICIOS].[locaciones] ([Id], [codigo], [ZonaId], [nombre], [vigente]) VALUES (5, 3, 1, N'locacion 3', 1)
SET IDENTITY_INSERT [SCH_SERVICIOS].[locaciones] OFF

SET IDENTITY_INSERT [SCH_RRHH].[colaboradores] ON
INSERT INTO [SCH_RRHH].[colaboradores] ([Id], [fecha_ingreso], [idTipoIdentificacion], [nro_identificacion], [primer_apellido], [segundo_apellido], [nombres], [fecha_nacimiento], [idGenero], [PaisId], [pais_nacimiento], [idEtnia], [estado_civil], [fecha_matrimonio], [discapacidad], [tipo_discapacidad], [porcentaje_discapacidad], [codigo_siniestro], [formacion_educativa], [huella_digital], [fotografia], [usuario_creacion], [fecha_creacion], [usuario_actualizacion], [fecha_actualizacion], [vigente], [id_candidato], [meta4], [grupo_personal_id], [destino_estancia_id], [locacion_trabajo_id], [sitio_trabajo], [AdminRotacionId], [area_id], [cargo_id], [vinculo_laboral_id], [clase_id], [encuadre_id], [turno_id], [horario_desde], [horario_hasta], [plan_beneficios_id], [plan_salud_id], [cobertura_dependiente_id], [planes_beneficios_id], [asociacion_id], [apto_medico_id], [alimentacion], [nro_comidas], [alojamiento], [nombre_hotel_id], [tipo_habitacion_id], [movilizacion], [tipo_movilizacion_id], [ParroquiaId], [ComunidadId], [lavanderia], [empresa_id], [posicion], [division_personal_id], [subdivision_personal_id], [funcion_id], [tipo_contrato_id], [clase_contrato_id], [caducidad_contrato], [ejecutor_obra], [tipo_nomina_id], [periodo_nomina_id], [proyecto_id], [forma_pago_id], [grupo_id], [subgrupo_id], [remuneracion_mensual], [banco_id], [tipo_cuenta_id], [numero_cuenta], [nro_legajo]) VALUES (4, N'2018-10-10 00:00:00', 1, N'1', N'solicitante', N'dos', N'tres', N'2018-10-10 00:00:00', 1, 1, 1, 1, 1, N'2018-10-10 00:00:00', 1, 1, 1, 1, 1, N'1', <Binary Data>, N'1', N'2018-10-10 00:00:00', N'1', N'2018-10-10 00:00:00', 1, 1, 1, 1, 1, 1, N'1', 1, 1, 1, 1, 1, 1, 1, N'10:00:00', N'20:00:00', 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, N'1', 1, 1, 1, 1, 1, N'2019-01-08 20:00:00', 1, 1, 1, 1, 1, 1, 1, CAST(1.00 AS Decimal(18, 2)), 1, 1, N'1', N'1')
SET IDENTITY_INSERT [SCH_RRHH].[colaboradores] OFF

/*
SET IDENTITY_INSERT [SCH_SERVICIOS].[solicitudes_viandas] ON
INSERT INTO [SCH_SERVICIOS].[solicitudes_viandas] ([Id], [solicitante_id], [LocacionId], [tipo_comida_id], [disciplina_id], [fecha_solicitud], [fecha_alcancce], [pedido_viandas], [alcance_viandas], [total_pedido], [consumido], [consumo_justificado], [total_consumido], [por_justificar], [estado], [solicitud_original_id], [referencia_ubicacion]) VALUES (6, 1, 1, 1, 1, N'2018-10-10 00:00:00', N'2018-02-01 00:00:00', 20, 10, 30, 0, 0, 0, 0, 1, 1, N'referencia')
INSERT INTO [SCH_SERVICIOS].[solicitudes_viandas] ([Id], [solicitante_id], [LocacionId], [tipo_comida_id], [disciplina_id], [fecha_solicitud], [fecha_alcancce], [pedido_viandas], [alcance_viandas], [total_pedido], [consumido], [consumo_justificado], [total_consumido], [por_justificar], [estado], [solicitud_original_id], [referencia_ubicacion]) VALUES (7, 1, 2, 1, 1, N'2018-07-13 00:00:00', N'2018-02-01 00:00:00', 20, 20, 30, 0, 0, 0, 0, 1, 1, N'referencia 2')
SET IDENTITY_INSERT [SCH_SERVICIOS].[solicitudes_viandas] OFF
 */

SET IDENTITY_INSERT [SCH_SERVICIOS].[parroquias] ON
INSERT INTO [SCH_SERVICIOS].[parroquias] ([Id], [CiudadId], [codigo], [nombre], [vigente]) VALUES (3, 1, N'1', N'parroquia', 1)
SET IDENTITY_INSERT [SCH_SERVICIOS].[parroquias] OFF

/*
SET IDENTITY_INSERT [SCH_RRHH].[contactos] ON
INSERT INTO [SCH_RRHH].[contactos] ([Id], [ColaboradoresId], [calle_principal], [numero], [calle_secundaria], [referencia], [telefono_convencional], [celular], [correo_electronico], [ComunidadId], [comunidad_laboral_id], [detalle_comunidad], [ParroquiaId], [parroquia_laboral_id], [detalle_parroquia], [PaisId], [pais_lab], [ProvinciaId], [provincia_lab], [CiudadId], [canton_lab], [fotografia], [amazonica], [vigente]) VALUES (6, 1, N'calle principal', N'numero', N'secundari', N'referencia', N'telefon', N'celular', N'correo', 1, 1, N'det', 3, 1, N'1', 1, 1, 1, 1, 1, 1, 0, 1, 1)
SET IDENTITY_INSERT [SCH_RRHH].[contactos] OFF
*/

SET IDENTITY_INSERT [SCH_SERVICIOS].[requisito] ON
INSERT INTO [SCH_SERVICIOS].[requisito] ([Id], [cat_requisito_id], [codigo], [nombre], [descripcion], [responsable_id], [caducidad], [tiempo_vigencia], [usuario_creacion], [fecha_creacion], [usuario_actualizacion], [fecha_actualizacion], [vigente]) VALUES (1, 1, 1, N'Req1', N'Req1', 1, 1, 1, N'1', N'2018-10-10 00:00:00', N'1', N'2018-10-10 00:00:00', 1)
INSERT INTO [SCH_SERVICIOS].[requisito] ([Id], [cat_requisito_id], [codigo], [nombre], [descripcion], [responsable_id], [caducidad], [tiempo_vigencia], [usuario_creacion], [fecha_creacion], [usuario_actualizacion], [fecha_actualizacion], [vigente]) VALUES (2, 1, 2, N'Req2', N'Req2', 1, 1, 1, N'1', N'2018-10-10 00:00:00', N'1', N'2018-10-10 00:00:00', 1)
INSERT INTO [SCH_SERVICIOS].[requisito] ([Id], [cat_requisito_id], [codigo], [nombre], [descripcion], [responsable_id], [caducidad], [tiempo_vigencia], [usuario_creacion], [fecha_creacion], [usuario_actualizacion], [fecha_actualizacion], [vigente]) VALUES (11, 1, 3, N'req tres', N'req tres', 1, 0, 2, N'1', N'2018-10-10 00:00:00', N'3', N'2018-10-10 00:00:00', 1)

SET IDENTITY_INSERT [SCH_SERVICIOS].[requisito] OFF


SET IDENTITY_INSERT [SCH_SERVICIOS].[horarios] ON
INSERT INTO [SCH_SERVICIOS].[horarios] ([Id], [des], [codigo], [nombre], [hora_inicio], [hora_fin], [usuario_creacion], [fecha_creacion], [usuario_actualizacion], [fecha_actualizacion], [vigente]) VALUES (1, 1, 1, N'horario 1', N'10:00:00', N'20:00:00', N'1', N'2018-10-10 00:00:00', N'1', N'2018-10-10 00:00:00', 1)
INSERT INTO [SCH_SERVICIOS].[horarios] ([Id], [des], [codigo], [nombre], [hora_inicio], [hora_fin], [usuario_creacion], [fecha_creacion], [usuario_actualizacion], [fecha_actualizacion], [vigente]) VALUES (2, 2, 2, N'horario 2', N'10:00:00', N'20:00:00', N'1', N'2018-10-10 00:00:00', N'1', N'2018-10-10 00:00:00', 1)
SET IDENTITY_INSERT [SCH_SERVICIOS].[horarios] OFF


SET IDENTITY_INSERT [SCH_SERVICIOS].[requisito_servicios] ON
INSERT INTO [SCH_SERVICIOS].[requisito_servicios] ([Id], [cat_usuario_id], [cat_estancia_id], [HorarioId], [ServicioId], [RequisitosId], [descripcion], [obligatorio], [usuario_creacion], [fecha_creacion], [usuario_actualizacion], [fecha_actualizacion], [vigente]) VALUES (3, 1, 1, 1, 4288, 11, N'ok', 1, N'1', N'2018-10-10 00:00:00', N'2', N'2018-10-10 00:00:00', 1)
SET IDENTITY_INSERT [SCH_SERVICIOS].[requisito_servicios] OFF


SET IDENTITY_INSERT [SCH_PROVEEDORES].[menus_proveedor] ON
INSERT INTO [SCH_PROVEEDORES].[menus_proveedor] ([Id], [ProveedorId], [fecha_inicial], [fecha_final], [aprobado], [fecha_aprobacion], [descripcion], [documentacion_id]) VALUES (4, 1, N'2018-10-10 00:00:00', N'2018-10-11 00:00:00', 0, N'2018-10-10 00:00:00', N'ok', 1)
INSERT INTO [SCH_PROVEEDORES].[menus_proveedor] ([Id], [ProveedorId], [fecha_inicial], [fecha_final], [aprobado], [fecha_aprobacion], [descripcion], [documentacion_id]) VALUES (5, 1, N'2018-10-10 00:00:00', N'2018-10-11 00:00:00', 0, N'2018-11-10 00:00:00', N'otro', 2)
SET IDENTITY_INSERT [SCH_PROVEEDORES].[menus_proveedor] OFF
