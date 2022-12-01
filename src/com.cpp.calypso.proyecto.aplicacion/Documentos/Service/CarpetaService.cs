using Abp.Runtime.Security;
using AutoMapper;
using Azure;
using Azure.Data.Tables;
//using Azure.Data.Tables;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Interface;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Models;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Documentos;
using Microsoft.Office.Interop.Word;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Service
{
    public class CarpetaAsyncBaseCrudAppService : AsyncBaseCrudAppService<Carpeta, CarpetaDto, PagedAndFilteredResultRequestDto>, ICarpetaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Documento> _documentoRepository;
        private readonly IBaseRepository<Usuario> _userRepository;
        private readonly AspUserManager<Rol, Usuario, Modulo> _userManager;
        private readonly IBaseRepository<UsuarioAutorizado> _usuarioautorizadoRepository;

        private readonly IBaseRepository<Carpeta> _carpetaRepository;
        private readonly IBaseRepository<Seccion> _seccionRepository;
        private readonly IBaseRepository<ImagenSeccion> _imagenseccionRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<ParametroSistema> _parametroRepository;

        const string AES_IV = "00f74597de203655";//16 bits  
        string url = "https://techintpmdisqas.table.core.windows.net";
        string server = "techintpmdisqas";
        string key = "FEBwOmcLhdWut8/vNMQnXeiNqUDBW3petxvLsXEkd9uIaUhh5dMsdxLOgc5L5P4reiUXbu5K0dasLWkETOTTFA==";
        public CarpetaAsyncBaseCrudAppService(
            IBaseRepository<Carpeta> repository,
            IBaseRepository<Documento> documentoRepository,
            IBaseRepository<Usuario> userRepository,
            AspUserManager<Rol, Usuario, Modulo> userManager,
            IBaseRepository<UsuarioAutorizado> usuarioautorizadoRepository,
             IBaseRepository<Seccion> seccionRepository,
             IBaseRepository<Carpeta> carpetaRepository,
              IBaseRepository<ImagenSeccion> imagenseccionRepository,
              IBaseRepository<Catalogo> catalogoRepository,
              IBaseRepository<ParametroSistema> parametroRepository
            ) : base(repository)
        {
            _documentoRepository = documentoRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _usuarioautorizadoRepository = usuarioautorizadoRepository;
            _carpetaRepository = carpetaRepository;
            _seccionRepository = seccionRepository;
            _imagenseccionRepository = imagenseccionRepository;
            _catalogoRepository = catalogoRepository;
            _parametroRepository = parametroRepository;
        }


        public CarpetaDto ObtenerCarpetaPorId(int id)
        {
            var carpeta = Repository.Get(id);
            return Mapper.Map<CarpetaDto>(carpeta);
        }


        public async Task<bool> CrearCarpetaAsync(CarpetaDto dto)
        {
            var entity = Mapper.Map<Carpeta>(dto);
            await Repository.InsertAsync(entity);
            return true;
        }

        public List<CarpetaDto> ObtenerCarpetas()
        {
            var carpetas = Repository.GetAll().Include(o => o.Estado).Include(o => o.Documentos).ToList();
            var dtos = Mapper.Map<List<CarpetaDto>>(carpetas);
            return dtos;
        }

        public bool EditarCarpetaAsync(CarpetaDto dto)
        {
            var carpeta = Mapper.Map<Carpeta>(dto);
            Repository.Update(carpeta);
            return true;
        }

        public ResultadoEliminacionResponse Eliminar(int id)
        {
            var tieneDocumentos = _documentoRepository.GetAll().Where(o => o.CarpetaId == id).Count() > 0;

            var carpeta = Repository.Get(id);

            if (tieneDocumentos)
            {
                return new ResultadoEliminacionResponse
                {
                    Eliminado = false,
                    Error = $"El contrato {carpeta.NombreCorto} no puede ser eliminado debido a que tiene documentos registrados."
                };
            }
            Repository.Delete(id);
            return new ResultadoEliminacionResponse
            {
                Eliminado = true,
                Error = ""
            };

        }

        public List<EstructuraArbol> ObtenerCarpetasAutorizadas()
        {
            var userId = Int32.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId().ToString());

            var carpetas = _usuarioautorizadoRepository.GetAll()
                .Include(o => o.Carpeta)
                .Where(o => o.UsuarioId == userId)
                .Where(o => o.Carpeta.Publicado == true)
                .Select(o => o.Carpeta)
                .ToList();

            var lista = new List<EstructuraArbol>();

            foreach (var carpeta in carpetas)
            {
                var nodo = new EstructuraArbol()
                {
                    key = carpeta.Id,
                    label = carpeta.NombreCorto,
                    data = carpeta.NombreCompleto,
                    expandedIcon = "fa fa-fw fa-folder-open",
                    collapsedIcon = "fa fa-fw fa-folder",
                    tipo = "padre",
                    children = new List<EstructuraArbol>(),
                    selectable = true,
                    draggable = true,
                    droppable = true,
                };
                lista.Add(nodo);
            }

            return lista;
        }


        public string ActualizarDocumentos()
        {
            int count = 0;
            var keys = this.GetKey();

            List<String> Errores = new List<string>();

            string url = "https://techintpmdisqas.table.core.windows.net";
            string server = "techintpmdisqas";
            string key = "FEBwOmcLhdWut8/vNMQnXeiNqUDBW3petxvLsXEkd9uIaUhh5dMsdxLOgc5L5P4reiUXbu5K0dasLWkETOTTFA==";


            var serviceClient = new TableServiceClient(new Uri(url), new TableSharedKeyCredential(server, key));

            //CARPETAS
            var tablaAzure = "Carpetas";
            var tableClient = new TableClient(new Uri(url), tablaAzure, new TableSharedKeyCredential(server, key));
            var resultsAzure = tableClient.Query<TableEntity>().ToList();

            string queryCarpetas = "select id,codigo,nombre_corto,nombre_completo,descripcion,publicado,vigente,usuario_creacion,fecha_creacion,usuario_actualizacion,fecha_actualizacion,usuario_eliminacion,fecha_eliminacion,catalogo_estado_id from SCH_DOCUMENTOS.carpetas";

            var carpetasOnpremise = new List<Carpeta>();
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(
                 connectionString))
            {
                SqlCommand command = new SqlCommand(queryCarpetas, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Carpeta s = new Carpeta();

                        if (!reader.IsDBNull(0))
                        {
                            s.Id = reader.GetInt32(0);
                        }
                        if (!reader.IsDBNull(1))
                        {
                            s.Codigo = reader.GetString(1);
                        }
                        if (!reader.IsDBNull(2))
                        {
                            s.NombreCorto = reader.GetString(2);
                        }
                        if (!reader.IsDBNull(3))
                        {
                            s.NombreCompleto = reader.GetString(3);
                        }
                        if (!reader.IsDBNull(4))
                        {
                            s.Descripcion = reader.GetString(4);
                        }
                        if (!reader.IsDBNull(5))
                        {
                            s.Publicado = reader.GetBoolean(5);
                        }
                        if (!reader.IsDBNull(6))
                        {
                            s.IsDeleted = reader.GetBoolean(6);
                        }
                        if (!reader.IsDBNull(7))
                        {
                            var creatorUser = reader[7].ToString();
                            s.CreatorUserId = Int32.Parse(creatorUser);
                        }
                        if (!reader.IsDBNull(8))
                        {
                            s.CreationTime = reader.GetDateTime(8);
                        }
                        if (!reader.IsDBNull(9))
                        {
                            var User = reader[9].ToString();
                            s.LastModifierUserId = Int32.Parse(User);
                        }
                        if (!reader.IsDBNull(10))
                        {
                            s.LastModificationTime = reader.GetDateTime(10);
                        }
                        if (!reader.IsDBNull(11))
                        {
                            var User = reader[11].ToString();
                            s.DeleterUserId = Int32.Parse(User);
                        }
                        if (!reader.IsDBNull(12))
                        {
                            s.DeletionTime = reader.GetDateTime(12);
                        }
                        if (!reader.IsDBNull(13))
                        {
                            s.EstadoId = reader.GetInt32(13);
                        }

                        carpetasOnpremise.Add(s);


                    }
                }
                connection.Close();
            }


            foreach (var c in carpetasOnpremise)
            {
                string partitionKey = c.Id.ToString();
                string rowKey = c.Id.ToString();
                if (!c.IsDeleted)
                {
                    c.Estado = Repository.GetAllIncluding(x => x.Estado).Where(x => x.Id == x.Id).FirstOrDefault().Estado;
                }
                var entity = new TableEntity(partitionKey, rowKey)
                                        {
                                            { "Id", c.Id },
                                            { "Codigo", c.Codigo },
                                            { "NombreCorto", c.NombreCorto },
                                            { "NombreCompleto", c.NombreCompleto },
                                            { "Descripcion", c.Descripcion },
                                            { "EstadoId", c.EstadoId },
                                            { "Publicado", c.Publicado },
                                            { "IsDeleted", c.IsDeleted },
                                            { "CreatorUserId", c.CreatorUserId },
                                            { "CreationTime", c.CreationTime },
                                            { "LastModifierUserId", c.LastModifierUserId },
                                            { "LastModificationTime", c.LastModificationTime },
                                            { "DeleterUserId", c.DeleterUserId },
                                            { "DeletionTime", c.DeletionTime },
                                            { "catalogo_estado",c.Estado!=null? c.Estado.nombre :""},
                                        };

                var carpetaAzure = (from a in resultsAzure
                                    where c.Id == a.GetInt32("Id")
                                    select a).FirstOrDefault();

                if (carpetaAzure != null)
                {
                    bool debeActualizar = false;
                    var fechaActualizacionString = carpetaAzure.GetDateTimeOffset("LastModificationTime");
                    var fechaEliminacionString = carpetaAzure.GetDateTimeOffset("DeletionTime");
                    if (c.DeletionTime.HasValue)//Valor Fecha Eliminacion Onpremise
                    {
                        debeActualizar = true;
                    }
                    else if (c.LastModificationTime.HasValue)
                    {
                        if (!fechaActualizacionString.HasValue)
                        {
                            debeActualizar = true;
                        }
                        else
                        {
                            var fechaActualizacion = fechaActualizacionString.Value;
                            var locaTime = fechaActualizacionString.Value.LocalDateTime;
                            if (fechaActualizacion != null
                                && c.LastModificationTime.Value > locaTime
                                )
                            {
                                debeActualizar = true;
                            }
                        }
                    }


                    if (debeActualizar)
                    {
                        carpetaAzure = entity;
                        try
                        {
                            tableClient.UpdateEntity(carpetaAzure, ETag.All, TableUpdateMode.Replace);
                        }
                        catch (Exception e)
                        {
                            string error = "CARP UPD ID: " + c.Id + " " + e.Message;
                            Errores.Add(error);
                        }
                    }
                }

                else
                {
                    try
                    {
                        tableClient.AddEntity(entity);
                    }
                    catch (Exception e)
                    {
                        string error = "CARPETA INSERT ID: " + c.Id + " " + e.Message;
                        Errores.Add(error);
                    }
                }



            }

            //DOCUMENTOS
            tablaAzure = "Documentos";
            tableClient = new TableClient(new Uri(url), tablaAzure, new TableSharedKeyCredential(server, key));
            resultsAzure = tableClient.Query<TableEntity>(filter: $"IsDeleted eq false").ToList();

            // var documentosOnpremise = _documentoRepository.GetAllIncluding(c => c.TipoDocumento).Where(c => c.IsDeleted || !c.IsDeleted).ToList();


            string queryDocumentos = "select id,codigo,nombre,carpeta_id,cantidad_paginas,tipo_documento,es_imagen,imagen,vigente,usuario_creacion,fecha_creacion,usuario_actualizacion,fecha_actualizacion,usuario_eliminacion,fecha_eliminacion,DocumentoPadreId from SCH_DOCUMENTOS.documentos";

            var documentosOnpremise = new List<Documento>();

            using (SqlConnection connection = new SqlConnection(
                 connectionString))
            {
                SqlCommand command = new SqlCommand(queryDocumentos, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Documento s = new Documento();

                        if (!reader.IsDBNull(0))
                        {
                            s.Id = reader.GetInt32(0);
                        }
                        if (!reader.IsDBNull(1))
                        {
                            s.Codigo = reader.GetString(1);
                        }
                        if (!reader.IsDBNull(2))
                        {
                            s.Nombre = reader.GetString(2);
                        }
                        if (!reader.IsDBNull(3))
                        {
                            s.CarpetaId = reader.GetInt32(3);
                        }
                        if (!reader.IsDBNull(4))
                        {
                            s.CantidadPaginas = reader.GetInt32(4);
                        }
                        if (!reader.IsDBNull(5))
                        {
                            s.TipoDocumentoId = reader.GetInt32(5);
                        }
                        if (!reader.IsDBNull(6))
                        {
                            s.EsImagen = reader.GetBoolean(6);
                        }
                        if (!reader.IsDBNull(7))
                        {
                            s.Imagen = reader.GetString(7);
                        }
                        if (!reader.IsDBNull(8))
                        {
                            s.IsDeleted = reader.GetBoolean(8);
                        }

                        if (!reader.IsDBNull(9))
                        {
                            var creatorUser = reader[9].ToString();
                            s.CreatorUserId = Int32.Parse(creatorUser);
                        }
                        if (!reader.IsDBNull(10))
                        {
                            s.CreationTime = reader.GetDateTime(10);
                        }
                        if (!reader.IsDBNull(11))
                        {
                            var User = reader[11].ToString();
                            s.LastModifierUserId = Int32.Parse(User);
                        }
                        if (!reader.IsDBNull(12))
                        {
                            s.LastModificationTime = reader.GetDateTime(12);
                        }
                        if (!reader.IsDBNull(13))
                        {
                            var User = reader[13].ToString();
                            s.DeleterUserId = Int32.Parse(User);
                        }
                        if (!reader.IsDBNull(14))
                        {
                            s.DeletionTime = reader.GetDateTime(14);
                        }
                        if (!reader.IsDBNull(15))
                        {
                            s.DocumentoPadreId = reader.GetInt32(15);
                        }

                        documentosOnpremise.Add(s);
                    }
                }
                connection.Close();
            }


            foreach (var c in documentosOnpremise)
            {
                string partitionKey = c.Id.ToString();
                string rowKey = c.Id.ToString();

                string tipoDocumento = _catalogoRepository.Get(c.TipoDocumentoId).nombre;

                var entity = new TableEntity(partitionKey, rowKey)
                                           {
                                                           { "Id", c.Id },
                                               { "Codigo", c.Codigo },
                                               { "Nombre", c.Nombre },
                                               { "CarpetaId", c.CarpetaId },
                                               { "CantidadPaginas", c.CantidadPaginas },
                                                           { "TipoDocumentoId", c.TipoDocumentoId },
                                               { "EsImagen", c.EsImagen },
                                                           { "Imagen", c.Imagen },
                                               { "DocumentoPadreId", c.DocumentoPadreId },
                                               { "IsDeleted", c.IsDeleted },
                                               { "CreatorUserId", c.CreatorUserId },
                                               { "CreationTime", c.CreationTime },
                                                           { "LastModifierUserId", c.LastModifierUserId },
                                               { "LastModificationTime", c.LastModificationTime },
                                               { "DeleterUserId", c.DeleterUserId },
                                               { "DeletionTime", c.DeletionTime },
                                               { "tipo_documento",tipoDocumento },
                                                { "orden",c.orden },


                                           };

                var docAzure = (from a in resultsAzure
                                where c.Id == a.GetInt32("Id")
                                select a).FirstOrDefault();

                if (docAzure != null)
                {
                    bool debeActualizar = false;
                    var fechaActualizacionString = docAzure.GetDateTimeOffset("LastModificationTime");
                    var fechaEliminacionString = docAzure.GetDateTimeOffset("DeletionTime");
                    if (c.DeletionTime.HasValue)//Valor Fecha Eliminacion Onpremise
                    {
                        debeActualizar = true;
                    }
                    else if (c.LastModificationTime.HasValue)
                    {
                        if (!fechaActualizacionString.HasValue)
                        {
                            debeActualizar = true;
                        }
                        else
                        {
                            var fechaActualizacion = fechaActualizacionString.Value;
                            var locaTime = fechaActualizacionString.Value.LocalDateTime;
                            if (fechaActualizacion != null
                                && c.LastModificationTime.Value > locaTime
                                )
                            {
                                debeActualizar = true;
                            }
                        }
                    }


                    if (debeActualizar)
                    {
                        docAzure = entity;
                        try
                        {
                            tableClient.UpdateEntity(docAzure, ETag.All, TableUpdateMode.Replace);
                        }
                        catch (Exception e)
                        {
                            string error = "DOC UPD ID: " + c.Id + " " + e.Message;
                            Errores.Add(error);
                        }
                    }


                }

                else
                {
                    try
                    {
                        tableClient.AddEntity(entity);
                    }
                    catch (Exception e)
                    {
                        string error = "DOC INSERT ID: " + c.Id + " " + e.Message;
                        Errores.Add(error);
                    }
                }
            }


            //UsuariosAutorizados

            tablaAzure = "UsuariosAutorizados";
            tableClient = new TableClient(new Uri(url), tablaAzure, new TableSharedKeyCredential(server, key));
            resultsAzure = tableClient.Query<TableEntity>(filter: $"IsDeleted eq false").ToList();





            string queryUsuarios = "select id, carpeta_id, usuario_id, vigente, usuario_creacion, fecha_creacion, usuario_actualizacion, fecha_actualizacion,usuario_eliminacion, fecha_eliminacion from SCH_DOCUMENTOS.usuarios_autorizados ";

            var UsuariosAutorizadosOnpremise = new List<UsuarioAutorizado>();

            using (SqlConnection connection = new SqlConnection(
                 connectionString))
            {
                SqlCommand command = new SqlCommand(queryUsuarios, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UsuarioAutorizado s = new UsuarioAutorizado();

                        if (!reader.IsDBNull(0))
                        {
                            s.Id = reader.GetInt32(0);
                        }
                        if (!reader.IsDBNull(1))
                        {
                            s.CarpetaId = reader.GetInt32(1);
                        }
                        if (!reader.IsDBNull(2))
                        {
                            s.UsuarioId = reader.GetInt32(2);
                        }

                        if (!reader.IsDBNull(3))
                        {
                            s.IsDeleted = reader.GetBoolean(3);
                        }

                        if (!reader.IsDBNull(4))
                        {
                            var creatorUser = reader[4].ToString();
                            s.CreatorUserId = Int32.Parse(creatorUser);
                        }
                        if (!reader.IsDBNull(5))
                        {
                            s.CreationTime = reader.GetDateTime(5);
                        }
                        if (!reader.IsDBNull(6))
                        {
                            var User = reader[6].ToString();
                            s.LastModifierUserId = Int32.Parse(User);
                        }
                        if (!reader.IsDBNull(7))
                        {
                            s.LastModificationTime = reader.GetDateTime(7);
                        }
                        if (!reader.IsDBNull(8))
                        {
                            var User = reader[8].ToString();
                            s.DeleterUserId = Int32.Parse(User);
                        }
                        if (!reader.IsDBNull(9))
                        {
                            s.DeletionTime = reader.GetDateTime(9);
                        }


                        UsuariosAutorizadosOnpremise.Add(s);
                    }
                }
                connection.Close();
            }
            int numParticion = 1;

            foreach (var c in UsuariosAutorizadosOnpremise)
            {
                string partitionKey = c.Id.ToString();
                string rowKey = c.Id.ToString();
                if (!c.IsDeleted)
                {
                    c.Usuario = _userRepository.Get(c.UsuarioId);
                }
                var entity = new TableEntity(partitionKey, rowKey)
                                           {
                                                 { "Id", c.Id },
                                               { "CarpetaId", c.CarpetaId },
                                               { "UsuarioId", c.UsuarioId },
                                                 { "IsDeleted", c.IsDeleted },
                                               { "CreatorUserId", c.CreatorUserId },
                                               { "CreationTime", c.CreationTime },
                                                 { "LastModifierUserId", c.LastModifierUserId },
                                               { "LastModificationTime", c.LastModificationTime },
                                               { "DeleterUserId", c.DeleterUserId },
                                               { "DeletionTime", c.DeletionTime },
                                               { "usuario",c.Usuario!=null? c.Usuario.Correo:""},

                                           };

                var imagenseccionAzure = (from a in resultsAzure
                                          where c.Id == a.GetInt32("Id")
                                          select a).FirstOrDefault();

                if (imagenseccionAzure != null)
                {
                    bool debeActualizar = false;
                    var fechaActualizacionString = imagenseccionAzure.GetDateTimeOffset("LastModificationTime");
                    var fechaEliminacionString = imagenseccionAzure.GetDateTimeOffset("DeletionTime");
                    if (c.DeletionTime.HasValue)//Valor Fecha Eliminacion Onpremise
                    {
                        debeActualizar = true;
                    }
                    else if (c.LastModificationTime.HasValue)
                    {
                        if (!fechaActualizacionString.HasValue)
                        {
                            debeActualizar = true;
                        }
                        else
                        {
                            var fechaActualizacion = fechaActualizacionString.Value;
                            var locaTime = fechaActualizacionString.Value.LocalDateTime;
                            if (fechaActualizacion != null
                                && c.LastModificationTime.Value > locaTime
                                )
                            {
                                debeActualizar = true;
                            }
                        }
                    }


                    if (debeActualizar)
                    {
                        imagenseccionAzure = entity;
                        try
                        {
                            tableClient.UpdateEntity(imagenseccionAzure, ETag.All, TableUpdateMode.Replace);
                        }
                        catch (Exception e)
                        {
                            string error = "USUARIO UPDATE ID: " + c.Id + " " + e.Message;
                            Errores.Add(error);
                        }
                    }

                }
                else
                {
                    try
                    {


                        tableClient.AddEntity(entity);
                        numParticion++;
                    }
                    catch (Exception e)
                    {
                        string error = "USUARIO INSERT ID: " + c.Id + " " + e.Message;
                        Errores.Add(error);
                    }
                }
            }



            if (Errores.Count > 0)
            {
                return JsonSerializer.Serialize(Errores);
            }
            else
            {
                return "OK";
            }

        }


        public List<string> GetKey()
        {
            var list = new List<String>();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            var PublicKey = rsa.ToXmlString(false);
            var PrivateKey = rsa.ToXmlString(true);
            list.Add(PublicKey);
            list.Add(PrivateKey);
            return list;
        }
        private static TripleDES GetInstance(string strKey)
        {
            TripleDES objProvider = new TripleDESCryptoServiceProvider();

            // Inicializa el proveedor
            objProvider.Key = Encoding.Unicode.GetBytes(strKey);
            objProvider.IV = new byte[objProvider.BlockSize / 8];
            // Devuelve el proveedor
            return objProvider;
        }


        public string EncryptByAES(string input, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        byte[] bytes = msEncrypt.ToArray();
                        return ByteArrayToHexString(bytes);
                    }
                }
            }
        }

        public string DecryptByAES(string input, string key)
        {
            byte[] inputBytes = HexStringToByteArray(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Convert the specified hex string to a byte array
        /// </summary>
        /// <param name="s">hexadecimal string (eg "7F 2C 4A" or "7F2C4A")</param>
        /// <returns>byte array corresponding to hexadecimal string</returns>
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// Convert a byte array into a formatted hex string
        /// </summary>
        /// <param name="data">byte array</param>
        /// <returns> formatted hexadecimal string</returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                //hexadecimal number
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                //16 digits separated by spaces
                //sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return sb.ToString().ToUpper();
        }




        public List<SeccionDto> SeccionesDES()
        {
            var seccionesOnpremise = _seccionRepository.GetAll().ToList();
            return Mapper.Map<List<SeccionDto>>(seccionesOnpremise);
        }


        public List<ImagenSeccionDto> ImagenesDES()
        {
            var ImagenesOnpremise = _imagenseccionRepository.GetAll().ToList();
            return Mapper.Map<List<ImagenSeccionDto>>(ImagenesOnpremise);
        }

        public int EncryptTest(string text)
        {
            return this.EncryptByAES(text, CatalogosCodigos.keycryt).Count();
        }

        public List<TableEntity> Contador()
        {
            var serviceClient = new TableServiceClient(new Uri(url), new TableSharedKeyCredential(server, key));
            var tablaAzure = "Contadores";
            var tableClient = new TableClient(new Uri(url), tablaAzure, new TableSharedKeyCredential(server, key));
            var resultsAzure = tableClient.Query<TableEntity>().ToList();
            return resultsAzure;
        }
        public string SyncCarpetas(string azureTableName)
        {

            string result = "";
            string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/Sincronizacion/Carpetas" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Second + ".txt");
            using (StreamWriter sw = File.CreateText(fileName))
            {
                List<String> Errores = new List<string>();
                var fechaUltimaSincronizacion = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_CARPETAS).FirstOrDefault();
                var serviceClient = new TableServiceClient(new Uri(url), new TableSharedKeyCredential(server, key));

                var fechaLocal = DateTime.Now;
                //CARPETAS
                var tablaAzure = azureTableName;
                var tableClient = new TableClient(new Uri(url), tablaAzure, new TableSharedKeyCredential(server, key));

                var resultsAzure = tableClient.Query<TableEntity>().ToList();

                //Contador
                var tableClientContador = new TableClient(new Uri(url), "Contadores", new TableSharedKeyCredential(server, key));
                int PartitionKeyMax = 100;

                var contadores = this.Contador();
                var resultContador = (from c in contadores where c.GetString("Tabla") == "carpetas" select c).FirstOrDefault();
                if (resultContador != null)
                {
                    PartitionKeyMax = resultContador.GetInt32("PartitionKeyMax").Value;
                }



                string queryCarpetas = "select id,codigo,nombre_corto,nombre_completo,descripcion,publicado,vigente,usuario_creacion,fecha_creacion,usuario_actualizacion,fecha_actualizacion,usuario_eliminacion,fecha_eliminacion,catalogo_estado_id from SCH_DOCUMENTOS.carpetas " +
                    "where  fecha_creacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_CARPETAS + "' ) OR " +
                           " fecha_actualizacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_CARPETAS + "' ) OR " +
                          " fecha_eliminacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_CARPETAS + "' ) ";


                var carpetasOnpremise = new List<Carpeta>();
                string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(
                     connectionString))
                {
                    SqlCommand command = new SqlCommand(queryCarpetas, connection);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Carpeta s = new Carpeta();

                            if (!reader.IsDBNull(0))
                            {
                                s.Id = reader.GetInt32(0);
                            }
                            if (!reader.IsDBNull(1))
                            {
                                s.Codigo = reader.GetString(1);
                            }
                            if (!reader.IsDBNull(2))
                            {
                                s.NombreCorto = reader.GetString(2);
                            }
                            if (!reader.IsDBNull(3))
                            {
                                s.NombreCompleto = reader.GetString(3);
                            }
                            if (!reader.IsDBNull(4))
                            {
                                s.Descripcion = reader.GetString(4);
                            }
                            if (!reader.IsDBNull(5))
                            {
                                s.Publicado = reader.GetBoolean(5);
                            }
                            if (!reader.IsDBNull(6))
                            {
                                s.IsDeleted = reader.GetBoolean(6);
                            }
                            if (!reader.IsDBNull(7))
                            {
                                var creatorUser = reader[7].ToString();
                                s.CreatorUserId = Int32.Parse(creatorUser);
                            }
                            if (!reader.IsDBNull(8))
                            {
                                s.CreationTime = reader.GetDateTime(8);
                            }
                            if (!reader.IsDBNull(9))
                            {
                                var User = reader[9].ToString();
                                s.LastModifierUserId = Int32.Parse(User);
                            }
                            if (!reader.IsDBNull(10))
                            {
                                s.LastModificationTime = reader.GetDateTime(10);
                            }
                            if (!reader.IsDBNull(11))
                            {
                                var User = reader[11].ToString();
                                s.DeleterUserId = Int32.Parse(User);
                            }
                            if (!reader.IsDBNull(12))
                            {
                                s.DeletionTime = reader.GetDateTime(12);
                            }
                            if (!reader.IsDBNull(13))
                            {
                                s.EstadoId = reader.GetInt32(13);
                            }

                            carpetasOnpremise.Add(s);


                        }
                    }
                    connection.Close();
                }


                foreach (var c in carpetasOnpremise)
                {
                    string partitionKey = PartitionKeyMax + "";
                    string rowKey = c.Id.ToString();
                    if (!c.IsDeleted)
                    {
                        c.Estado = Repository.GetAllIncluding(x => x.Estado).Where(x => x.Id == x.Id).FirstOrDefault().Estado;
                    }
                    var entity = new TableEntity(partitionKey, rowKey)
                                        {
                                            { "Id", c.Id },
                                            { "Codigo", c.Codigo },
                                            { "NombreCorto", c.NombreCorto },
                                            { "NombreCompleto", c.NombreCompleto },
                                            { "Descripcion", c.Descripcion },
                                            { "EstadoId", c.EstadoId },
                                            { "Publicado", c.Publicado },
                                            { "IsDeleted", c.IsDeleted },
                                            { "CreatorUserId", c.CreatorUserId },
                                            { "CreationTime", c.CreationTime },
                                            { "LastModifierUserId", c.LastModifierUserId },
                                            { "LastModificationTime", c.LastModificationTime },
                                            { "DeleterUserId", c.DeleterUserId },
                                            { "DeletionTime", c.DeletionTime },
                                            { "catalogo_estado",c.Estado!=null? c.Estado.nombre :""},
                                        };

                    var carpetaAzure = (from a in resultsAzure
                                        where c.Id == a.GetInt32("Id")
                                        select a).FirstOrDefault();

                    if (carpetaAzure != null)
                    {
                         partitionKey = carpetaAzure.GetString("PartitionKey");
                         rowKey = c.Id.ToString();
                        entity = new TableEntity(partitionKey, rowKey)
                                        {
                                            { "Id", c.Id },
                                            { "Codigo", c.Codigo },
                                            { "NombreCorto", c.NombreCorto },
                                            { "NombreCompleto", c.NombreCompleto },
                                            { "Descripcion", c.Descripcion },
                                            { "EstadoId", c.EstadoId },
                                            { "Publicado", c.Publicado },
                                            { "IsDeleted", c.IsDeleted },
                                            { "CreatorUserId", c.CreatorUserId },
                                            { "CreationTime", c.CreationTime },
                                            { "LastModifierUserId", c.LastModifierUserId },
                                            { "LastModificationTime", c.LastModificationTime },
                                            { "DeleterUserId", c.DeleterUserId },
                                            { "DeletionTime", c.DeletionTime },
                                            { "catalogo_estado",c.Estado!=null? c.Estado.nombre :""},
                                        };
                        carpetaAzure = entity;
                        try
                        {
                            tableClient.UpdateEntity(carpetaAzure, ETag.All, TableUpdateMode.Replace);
                        }
                        catch (Exception e)
                        {
                            string error = "CARP UPD ID: " + c.Id + " " + e.Message;
                            Errores.Add(error);
                        }
                    }

                    else
                    {
                        try
                        {
                            tableClient.AddEntity(entity);
                        }
                        catch (Exception e)
                        {
                            string error = "CARPETA INSERT ID: " + c.Id + " " + e.Message;
                            Errores.Add(error);
                        }
                    }
                }

                if (Errores.Count > 0)
                {
                    return JsonSerializer.Serialize(Errores);
                }
                else
                {

                    if (fechaUltimaSincronizacion != null)
                    {
                        var fechaSincronizacion = _parametroRepository.Get(fechaUltimaSincronizacion.Id);
                        fechaSincronizacion.Valor = fechaLocal.ToString("yyyy-MM-dd HH:mm:ss");
                        _parametroRepository.Update(fechaSincronizacion);

                        /*Actualizacion Contadores*/
                        var contadorpartitionKey = resultContador.GetString("PartitionKey").ToString();
                        var contadorrowKey = resultContador.GetString("RowKey").ToString();
                        var contador = new TableEntity(contadorpartitionKey, contadorrowKey)
                                           {
                                               { "Finalizado", resultContador.GetBoolean("Finalizado").Value},
                                               { "PartitionKeyActual", resultContador.GetInt32("PartitionKeyActual").Value },
                                               { "PartitionKeyMax",PartitionKeyMax},
                                               { "Tabla",resultContador.GetString("Tabla").ToString()}
                                           };
                        tableClientContador.UpdateEntity(contador, ETag.All, TableUpdateMode.Replace);
                    }
                    result = "OK";

                }


            }
            return result;
        }
        public string SyncDocumentos(string azureTableName)
        {

            string result = "";
            string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/Sincronizacion/Documentos" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Second + ".txt");
            using (StreamWriter sw = File.CreateText(fileName))
            {
                List<String> Errores = new List<string>();
                var fechaUltimaSincronizacion = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_DOCUMENTOS).FirstOrDefault();
                var serviceClient = new TableServiceClient(new Uri(url), new TableSharedKeyCredential(server, key));

                var fechaLocal = DateTime.Now;

                //DOCUMENTOS
                var tablaAzure = azureTableName;
                var tableClient = new TableClient(new Uri(url), tablaAzure, new TableSharedKeyCredential(server, key));
                var resultsAzure = tableClient.Query<TableEntity>().ToList();



                //Contador
                var tableClientContador = new TableClient(new Uri(url), "Contadores", new TableSharedKeyCredential(server, key));
                int PartitionKeyMax = 100;

                var contadores = this.Contador();
                var resultContador = (from c in contadores where c.GetString("Tabla") == "documentos" select c).FirstOrDefault();
                if (resultContador != null)
                {
                    PartitionKeyMax = resultContador.GetInt32("PartitionKeyMax").Value;
                }


                string queryDocumentos = "select id,codigo,nombre,carpeta_id,cantidad_paginas,tipo_documento,es_imagen,imagen,vigente,usuario_creacion,fecha_creacion,usuario_actualizacion,fecha_actualizacion,usuario_eliminacion,fecha_eliminacion,DocumentoPadreId from SCH_DOCUMENTOS.documentos " +
                        "where  fecha_creacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_DOCUMENTOS + "' ) OR " +
                           " fecha_actualizacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_DOCUMENTOS + "' ) OR " +
                          " fecha_eliminacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_DOCUMENTOS + "' ) ";

                var documentosOnpremise = new List<Documento>();
                string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(
                     connectionString))
                {
                    SqlCommand command = new SqlCommand(queryDocumentos, connection);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Documento s = new Documento();

                            if (!reader.IsDBNull(0))
                            {
                                s.Id = reader.GetInt32(0);
                            }
                            if (!reader.IsDBNull(1))
                            {
                                s.Codigo = reader.GetString(1);
                            }
                            if (!reader.IsDBNull(2))
                            {
                                s.Nombre = reader.GetString(2);
                            }
                            if (!reader.IsDBNull(3))
                            {
                                s.CarpetaId = reader.GetInt32(3);
                            }
                            if (!reader.IsDBNull(4))
                            {
                                s.CantidadPaginas = reader.GetInt32(4);
                            }
                            if (!reader.IsDBNull(5))
                            {
                                s.TipoDocumentoId = reader.GetInt32(5);
                            }
                            if (!reader.IsDBNull(6))
                            {
                                s.EsImagen = reader.GetBoolean(6);
                            }
                            if (!reader.IsDBNull(7))
                            {
                                s.Imagen = reader.GetString(7);
                            }
                            if (!reader.IsDBNull(8))
                            {
                                s.IsDeleted = reader.GetBoolean(8);
                            }

                            if (!reader.IsDBNull(9))
                            {
                                var creatorUser = reader[9].ToString();
                                s.CreatorUserId = Int32.Parse(creatorUser);
                            }
                            if (!reader.IsDBNull(10))
                            {
                                s.CreationTime = reader.GetDateTime(10);
                            }
                            if (!reader.IsDBNull(11))
                            {
                                var User = reader[11].ToString();
                                s.LastModifierUserId = Int32.Parse(User);
                            }
                            if (!reader.IsDBNull(12))
                            {
                                s.LastModificationTime = reader.GetDateTime(12);
                            }
                            if (!reader.IsDBNull(13))
                            {
                                var User = reader[13].ToString();
                                s.DeleterUserId = Int32.Parse(User);
                            }
                            if (!reader.IsDBNull(14))
                            {
                                s.DeletionTime = reader.GetDateTime(14);
                            }
                            if (!reader.IsDBNull(15))
                            {
                                s.DocumentoPadreId = reader.GetInt32(15);
                            }

                            documentosOnpremise.Add(s);
                        }
                    }
                    connection.Close();
                }


                foreach (var c in documentosOnpremise)
                {
                    string partitionKey = PartitionKeyMax + "";
                    string rowKey = c.Id.ToString();

                    string tipoDocumento = _catalogoRepository.Get(c.TipoDocumentoId).nombre;

                    var entity = new TableEntity(partitionKey, rowKey)
                                           {
                                                           { "Id", c.Id },
                                               { "Codigo", c.Codigo },
                                               { "Nombre", c.Nombre },
                                               { "CarpetaId", c.CarpetaId },
                                               { "CantidadPaginas", c.CantidadPaginas },
                                                           { "TipoDocumentoId", c.TipoDocumentoId },
                                               { "EsImagen", c.EsImagen },
                                                           { "Imagen", c.Imagen },
                                               { "DocumentoPadreId", c.DocumentoPadreId },
                                               { "IsDeleted", c.IsDeleted },
                                               { "CreatorUserId", c.CreatorUserId },
                                               { "CreationTime", c.CreationTime },
                                                           { "LastModifierUserId", c.LastModifierUserId },
                                               { "LastModificationTime", c.LastModificationTime },
                                               { "DeleterUserId", c.DeleterUserId },
                                               { "DeletionTime", c.DeletionTime },
                                               { "tipo_documento",tipoDocumento },
                                                { "orden",c.orden },


                                           };

                    var docAzure = (from a in resultsAzure
                                    where c.Id == a.GetInt32("Id")
                                    select a).FirstOrDefault();

                    if (docAzure != null)
                    {
                        partitionKey = docAzure.GetString("PartitionKey");
                        rowKey = c.Id.ToString();
                         entity = new TableEntity(partitionKey, rowKey)
                                           {
                                                           { "Id", c.Id },
                                               { "Codigo", c.Codigo },
                                               { "Nombre", c.Nombre },
                                               { "CarpetaId", c.CarpetaId },
                                               { "CantidadPaginas", c.CantidadPaginas },
                                                           { "TipoDocumentoId", c.TipoDocumentoId },
                                               { "EsImagen", c.EsImagen },
                                                           { "Imagen", c.Imagen },
                                               { "DocumentoPadreId", c.DocumentoPadreId },
                                               { "IsDeleted", c.IsDeleted },
                                               { "CreatorUserId", c.CreatorUserId },
                                               { "CreationTime", c.CreationTime },
                                                           { "LastModifierUserId", c.LastModifierUserId },
                                               { "LastModificationTime", c.LastModificationTime },
                                               { "DeleterUserId", c.DeleterUserId },
                                               { "DeletionTime", c.DeletionTime },
                                               { "tipo_documento",tipoDocumento },
                                                { "orden",c.orden },


                                           };

                        docAzure = entity;
                        try
                        {
                            tableClient.UpdateEntity(docAzure, ETag.All, TableUpdateMode.Replace);
                        }
                        catch (Exception e)
                        {
                            string error = "DOC UPD ID: " + c.Id + " " + e.Message;
                            Errores.Add(error);
                        }

                    }

                    else
                    {
                        try
                        {
                            tableClient.AddEntity(entity);
                        }
                        catch (Exception e)
                        {
                            string error = "DOC INSERT ID: " + c.Id + " " + e.Message;
                            Errores.Add(error);
                        }
                    }
                }



                if (Errores.Count > 0)
                {
                    return JsonSerializer.Serialize(Errores);
                }
                else
                {

                    if (fechaUltimaSincronizacion != null)
                    {
                        var fechaSincronizacion = _parametroRepository.Get(fechaUltimaSincronizacion.Id);
                        fechaSincronizacion.Valor = fechaLocal.ToString("yyyy-MM-dd HH:mm:ss");
                        _parametroRepository.Update(fechaSincronizacion);

                        /*Actualizacion Contadores*/
                        var contadorpartitionKey = resultContador.GetString("PartitionKey").ToString();
                        var contadorrowKey = resultContador.GetString("RowKey").ToString();
                        var contador = new TableEntity(contadorpartitionKey, contadorrowKey)
                                           {
                                               { "Finalizado", resultContador.GetBoolean("Finalizado").Value},
                                               { "PartitionKeyActual", resultContador.GetInt32("PartitionKeyActual").Value },
                                               { "PartitionKeyMax",PartitionKeyMax},
                                               { "Tabla",resultContador.GetString("Tabla").ToString()}
                                           };
                        tableClientContador.UpdateEntity(contador, ETag.All, TableUpdateMode.Replace);



                    }
                    result = "OK";

                }

            }
            return result;
        }
        public String SyncUsuarios(string azureTableName)
        {
            string result = "";
            string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/Sincronizacion/Usuarios" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Second + ".txt");
            using (StreamWriter sw = File.CreateText(fileName))
            {
                List<String> Errores = new List<string>();
                var fechaUltimaSincronizacion = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_USUARIOS).FirstOrDefault();
                var serviceClient = new TableServiceClient(new Uri(url), new TableSharedKeyCredential(server, key));

                var fechaLocal = DateTime.Now;


                //UsuariosAutorizados

                var tablaAzure = azureTableName;
                var tableClient = new TableClient(new Uri(url), tablaAzure, new TableSharedKeyCredential(server, key));
                var resultsAzure = tableClient.Query<TableEntity>().ToList();

                //Contador
                var tableClientContador = new TableClient(new Uri(url), "Contadores", new TableSharedKeyCredential(server, key));
                int PartitionKeyMax = 100;

                var contadores = this.Contador();
                var resultContador = (from c in contadores where c.GetString("Tabla") == "usuarios_autorizados" select c).FirstOrDefault();
                if (resultContador != null)
                {
                    PartitionKeyMax = resultContador.GetInt32("PartitionKeyMax").Value;
                }

                string queryUsuarios = "select id, carpeta_id, usuario_id, vigente, usuario_creacion, fecha_creacion, usuario_actualizacion, fecha_actualizacion,usuario_eliminacion, fecha_eliminacion from SCH_DOCUMENTOS.usuarios_autorizados " +
                "where  fecha_creacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_USUARIOS + "' ) OR " +
                       "fecha_actualizacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_USUARIOS + "' ) OR " +
                       "fecha_eliminacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_USUARIOS + "' ) ";


                var UsuariosAutorizadosOnpremise = new List<UsuarioAutorizado>();
                string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(
                     connectionString))
                {
                    SqlCommand command = new SqlCommand(queryUsuarios, connection);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UsuarioAutorizado s = new UsuarioAutorizado();

                            if (!reader.IsDBNull(0))
                            {
                                s.Id = reader.GetInt32(0);
                            }
                            if (!reader.IsDBNull(1))
                            {
                                s.CarpetaId = reader.GetInt32(1);
                            }
                            if (!reader.IsDBNull(2))
                            {
                                s.UsuarioId = reader.GetInt32(2);
                            }

                            if (!reader.IsDBNull(3))
                            {
                                s.IsDeleted = reader.GetBoolean(3);
                            }

                            if (!reader.IsDBNull(4))
                            {
                                var creatorUser = reader[4].ToString();
                                s.CreatorUserId = Int32.Parse(creatorUser);
                            }
                            if (!reader.IsDBNull(5))
                            {
                                s.CreationTime = reader.GetDateTime(5);
                            }
                            if (!reader.IsDBNull(6))
                            {
                                var User = reader[6].ToString();
                                s.LastModifierUserId = Int32.Parse(User);
                            }
                            if (!reader.IsDBNull(7))
                            {
                                s.LastModificationTime = reader.GetDateTime(7);
                            }
                            if (!reader.IsDBNull(8))
                            {
                                var User = reader[8].ToString();
                                s.DeleterUserId = Int32.Parse(User);
                            }
                            if (!reader.IsDBNull(9))
                            {
                                s.DeletionTime = reader.GetDateTime(9);
                            }


                            UsuariosAutorizadosOnpremise.Add(s);
                        }
                    }
                    connection.Close();
                }

                foreach (var c in UsuariosAutorizadosOnpremise)
                {
                    string partitionKey = PartitionKeyMax + "";
                    string rowKey = c.Id.ToString();
                    if (!c.IsDeleted)
                    {
                        c.Usuario = _userRepository.Get(c.UsuarioId);
                    }
                    var entity = new TableEntity(partitionKey, rowKey)
                                           {
                                                 { "Id", c.Id },
                                               { "CarpetaId", c.CarpetaId },
                                               { "UsuarioId", c.UsuarioId },
                                                 { "IsDeleted", c.IsDeleted },
                                               { "CreatorUserId", c.CreatorUserId },
                                               { "CreationTime", c.CreationTime },
                                                 { "LastModifierUserId", c.LastModifierUserId },
                                               { "LastModificationTime", c.LastModificationTime },
                                               { "DeleterUserId", c.DeleterUserId },
                                               { "DeletionTime", c.DeletionTime },
                                               { "usuario",c.Usuario!=null? c.Usuario.Correo:""},

                                           };

                    var usuarioAzure = (from a in resultsAzure
                                        where c.Id == a.GetInt32("Id")
                                        select a).FirstOrDefault();

                    if (usuarioAzure != null)
                    {
                        partitionKey = usuarioAzure.GetString("PartitionKey");
                        rowKey = c.Id.ToString();
                        entity = new TableEntity(partitionKey, rowKey)
                                           {
                                                 { "Id", c.Id },
                                               { "CarpetaId", c.CarpetaId },
                                               { "UsuarioId", c.UsuarioId },
                                                 { "IsDeleted", c.IsDeleted },
                                               { "CreatorUserId", c.CreatorUserId },
                                               { "CreationTime", c.CreationTime },
                                                 { "LastModifierUserId", c.LastModifierUserId },
                                               { "LastModificationTime", c.LastModificationTime },
                                               { "DeleterUserId", c.DeleterUserId },
                                               { "DeletionTime", c.DeletionTime },
                                               { "usuario",c.Usuario!=null? c.Usuario.Correo:""},

                                           };


                        usuarioAzure = entity;
                        try
                        {
                            tableClient.UpdateEntity(usuarioAzure, ETag.All, TableUpdateMode.Replace);
                        }
                        catch (Exception e)
                        {
                            string error = "USUARIO UPDATE ID: " + c.Id + " " + e.Message;
                            Errores.Add(error);
                        }
                    }
                    else
                    {
                        try
                        {

                            tableClient.AddEntity(entity);

                        }
                        catch (Exception e)
                        {
                            string error = "USUARIO INSERT ID: " + c.Id + " " + e.Message;
                            Errores.Add(error);
                        }
                    }
                }

                if (Errores.Count > 0)
                {
                    return JsonSerializer.Serialize(Errores);
                }
                else
                {

                    if (fechaUltimaSincronizacion != null)
                    {
                        var fechaSincronizacion = _parametroRepository.Get(fechaUltimaSincronizacion.Id);
                        fechaSincronizacion.Valor = fechaLocal.ToString("yyyy-MM-dd HH:mm:ss");
                        _parametroRepository.Update(fechaSincronizacion);

                        /*Actualizacion Contadores*/
                        var contadorpartitionKey = resultContador.GetString("PartitionKey").ToString();
                        var contadorrowKey = resultContador.GetString("RowKey").ToString();
                        var contador = new TableEntity(contadorpartitionKey, contadorrowKey)
                                           {
                                               { "Finalizado", resultContador.GetBoolean("Finalizado").Value},
                                               { "PartitionKeyActual", resultContador.GetInt32("PartitionKeyActual").Value },
                                               { "PartitionKeyMax",PartitionKeyMax},
                                               { "Tabla",resultContador.GetString("Tabla").ToString()}
                                           };
                        tableClientContador.UpdateEntity(contador, ETag.All, TableUpdateMode.Replace);


                    }
                    result = "OK";

                }



            }
            return result;
        }



        public string SyncSecciones(string azureTableName)
        {

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/Sincronizacion/Secciones" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Second + ".txt");

            using (StreamWriter sw = File.CreateText(fileName))
            {
                List<String> Errores = new List<string>();
                var fechaUltimaSincronizacion = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_SECCIONES).FirstOrDefault();
                var serviceClient = new TableServiceClient(new Uri(url), new TableSharedKeyCredential(server, key));

                // 1.Obtener la fecha Actual del Sistema
                var fechaLocal = DateTime.Now;
                //2.Consultar todos los registros modificados




                //Contador
                var tableClientContador = new TableClient(new Uri(url), "Contadores", new TableSharedKeyCredential(server, key));
                int PartitionKeyMax = 100;
                int MaximoRegistros = 200;

                var contadores = this.Contador();
                var resultContador = (from c in contadores where c.GetString("Tabla") == "secciones" select c).FirstOrDefault();
                if (resultContador != null)
                {
                    PartitionKeyMax = resultContador.GetInt32("PartitionKeyMax").Value;
                }




                string querySecciones =

                    "SELECT id,seccion,contenido,documento_id,seccion_id,numero_pagina,vigente,usuario_creacion,fecha_creacion,usuario_actualizacion," +
                    "fecha_actualizacion,usuario_eliminacion,fecha_eliminacion,Codigo,ordinal,contenido_plano FROM SCH_DOCUMENTOS.secciones " +
                     "where  fecha_creacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_SECCIONES + "' ) OR " +
                           "fecha_actualizacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_SECCIONES + "' ) OR " +
                          "fecha_eliminacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_SECCIONES + "' ) ";



                // 3.Subir los registros al azure  SI EL ID existe actualizo SINO creo la entidad

                var seccionesOnpremise = new List<Seccion>();
                string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(
                     connectionString))
                {
                    SqlCommand command = new SqlCommand(querySecciones, connection);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Seccion s = new Seccion();
                            if (!reader.IsDBNull(0))
                            {
                                s.Id = reader.GetInt32(0);
                            }
                            if (!reader.IsDBNull(1))
                            {
                                s.NombreSeccion = reader.GetString(1);
                            }
                            if (!reader.IsDBNull(2))
                            {
                                s.Contenido = reader.GetString(2);
                            }
                            if (!reader.IsDBNull(3))
                            {
                                s.DocumentoId = reader.GetInt32(3);
                            }
                            if (!reader.IsDBNull(4))
                            {
                                s.SeccionPadreId = reader.GetInt32(4);
                            }
                            if (!reader.IsDBNull(5))
                            {
                                s.NumeroPagina = reader.GetString(5);
                            }
                            if (!reader.IsDBNull(6))
                            {
                                s.IsDeleted = reader.GetBoolean(6);
                            }
                            if (!reader.IsDBNull(7))
                            {
                                var creatorUser = reader[7].ToString();
                                s.CreatorUserId = Int32.Parse(creatorUser);
                            }
                            if (!reader.IsDBNull(8))
                            {
                                s.CreationTime = reader.GetDateTime(8);
                            }
                            if (!reader.IsDBNull(9))
                            {
                                var User = reader[9].ToString();
                                s.LastModifierUserId = Int32.Parse(User);
                            }
                            if (!reader.IsDBNull(10))
                            {
                                s.LastModificationTime = reader.GetDateTime(10);
                            }
                            if (!reader.IsDBNull(11))
                            {
                                var User = reader[11].ToString();
                                s.DeleterUserId = Int32.Parse(User);
                            }
                            if (!reader.IsDBNull(12))
                            {
                                s.DeletionTime = reader.GetDateTime(12);
                            }
                            if (!reader.IsDBNull(13))
                            {
                                s.Codigo = reader.GetString(13);
                            }
                            if (!reader.IsDBNull(14))
                            {
                                s.Ordinal = reader.GetInt32(14);
                            }
                            if (!reader.IsDBNull(15))
                            {
                                s.Contenido_Plano = reader.GetString(15);
                            }

                            seccionesOnpremise.Add(s);


                        }
                    }
                    connection.Close();
                }

                var data = seccionesOnpremise;

                //SECCIONES
                var tablaAzure = azureTableName;
                var tableClient = new TableClient(new Uri(url), tablaAzure, new TableSharedKeyCredential(server, key));
                var resultsAzure = tableClient.Query<TableEntity>().ToList();

                int iteraciones = 1;
                var countRegistrosParticion = (from m in resultsAzure
                                               where m.GetString("PartitionKey").ToString() == "" + PartitionKeyMax
                                               select m).ToList();
                if (countRegistrosParticion.Count >= MaximoRegistros)
                {
                    PartitionKeyMax = PartitionKeyMax + 1;
                }
                else
                {
                    iteraciones = countRegistrosParticion.Count;
                }



                foreach (var c in data)
                {
                    int caracteresEncryptados = this.EncryptByAES(c.Contenido, CatalogosCodigos.keycryt).Length;
                    var seccionAzure = (from a in resultsAzure where c.Id == a.GetInt32("Id") select a).FirstOrDefault();

                    if (seccionAzure != null)
                    {
                        string partitionKey = seccionAzure.GetString("PartitionKey");
                        string rowKey = seccionAzure.GetString("RowKey");
                        var entity = new TableEntity(partitionKey, rowKey)
                                        {
                                            { "Id", c.Id },
                                            { "NombreSeccion",this.EncryptByAES(c.NombreSeccion,CatalogosCodigos.keycryt)},
                                            { "Codigo", c.Codigo },
                                            { "Contenido",this.EncryptByAES(c.Contenido,CatalogosCodigos.keycryt)},
                                            { "Contenido_Plano",this.EncryptByAES(c.Contenido_Plano,CatalogosCodigos.keycryt) },
                                            { "DocumentoId", c.DocumentoId },
                                            { "SeccionPadreId", c.SeccionPadreId },
                                            { "NumeroPagina", c.NumeroPagina },
                                            { "Ordinal", c.Ordinal },
                                            { "IsDeleted", c.IsDeleted },
                                            { "CreatorUserId", c.CreatorUserId },
                                            { "CreationTime", c.CreationTime },
                                            { "LastModifierUserId", c.LastModifierUserId },
                                            { "LastModificationTime", c.LastModificationTime },
                                            { "DeleterUserId", c.DeleterUserId },
                                            { "DeletionTime", c.DeletionTime },
                                        };

                        seccionAzure = entity;
                        try
                        {
                            tableClient.UpdateEntity(seccionAzure, ETag.All, TableUpdateMode.Replace);
                        }
                        catch (RequestFailedException e)
                        {
                            string error = "UPD ID: " + c.Id + " " + e.Message;
                            sw.WriteLine(error);
                            Errores.Add(error);
                        }
                    }
                    else
                    {
                        string partitionKey = PartitionKeyMax + "";
                        string rowKey = c.Id.ToString();
                        var entity = new TableEntity(partitionKey, rowKey)
                                       {
                                           { "Id", c.Id },
                                           { "NombreSeccion",this.EncryptByAES(c.NombreSeccion,CatalogosCodigos.keycryt)},
                                           { "Codigo", c.Codigo },
                                           { "Contenido",this.EncryptByAES(c.Contenido,CatalogosCodigos.keycryt)},
                                           { "Contenido_Plano",this.EncryptByAES(c.Contenido_Plano,CatalogosCodigos.keycryt) },
                                           { "DocumentoId", c.DocumentoId },
                                           { "SeccionPadreId", c.SeccionPadreId },
                                           { "NumeroPagina", c.NumeroPagina },
                                           { "Ordinal", c.Ordinal },
                                           { "IsDeleted", c.IsDeleted },
                                           { "CreatorUserId", c.CreatorUserId },
                                           { "CreationTime", c.CreationTime },
                                           { "LastModifierUserId", c.LastModifierUserId },
                                           { "LastModificationTime", c.LastModificationTime },
                                           { "DeleterUserId", c.DeleterUserId },
                                           { "DeletionTime", c.DeletionTime },
                                       };
                        try
                        {
                            tableClient.AddEntity(entity);
                            iteraciones++;
                        }
                        catch (RequestFailedException e)
                        {
                            if (caracteresEncryptados > 32000)
                            {
                                sw.WriteLine("Caracteres Encrytpados ", caracteresEncryptados);
                            }
                            string error = "INSERT ID: " + c.Id + " " + e.Message;
                            sw.WriteLine(error);
                            Errores.Add(error);
                        }

                        if (iteraciones % MaximoRegistros == 0)
                        {
                            iteraciones = 1;
                            PartitionKeyMax = PartitionKeyMax + 1;
                        }

                    }



                }


                if (Errores.Count > 0)
                {
                    return JsonSerializer.Serialize(Errores);
                }
                else
                {
                    if (fechaUltimaSincronizacion != null)
                    {
                        var fechaSincronizacion = _parametroRepository.Get(fechaUltimaSincronizacion.Id);
                        fechaSincronizacion.Valor = fechaLocal.ToString("yyyy-MM-dd HH:mm:ss");
                        _parametroRepository.Update(fechaSincronizacion);


                        /*Actualizacion Contadores*/
                        var contadorpartitionKey = resultContador.GetString("PartitionKey").ToString();
                        var contadorrowKey = resultContador.GetString("RowKey").ToString();
                        var contador = new TableEntity(contadorpartitionKey, contadorrowKey)
                                           {
                                               { "Finalizado", resultContador.GetBoolean("Finalizado").Value},
                                               { "PartitionKeyActual", resultContador.GetInt32("PartitionKeyActual").Value },
                                               { "PartitionKeyMax",PartitionKeyMax},
                                               { "Tabla",resultContador.GetString("Tabla").ToString()}
                                           };
                        tableClientContador.UpdateEntity(contador, ETag.All, TableUpdateMode.Replace);



                    }
                    return "OK";
                }

            }
        }

        public string SyncImagenesSecciones(string azureTableName)
        {

            string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/Sincronizacion/Imagenes" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Second + ".txt");


            using (StreamWriter sw = File.CreateText(fileName))
            {
                List<int> IdsInserts = new List<int>();
                List<String> Errores = new List<string>();

                var fechaUltimaSincronizacion = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_IMAGENES).FirstOrDefault();
                var serviceClient = new TableServiceClient(new Uri(url), new TableSharedKeyCredential(server, key));

                // 1.Obtener la fecha Actual del Sistema
                var fechaLocal = DateTime.Now;
                //2.Consultar todos los registros modificados


                //Contador
                var tableClientContador = new TableClient(new Uri(url), "Contadores", new TableSharedKeyCredential(server, key));
                int PartitionKeyMax = 101;
                int MaximoRegistros = 5;

                var contadores = this.Contador();
                var resultContador = (from c in contadores where c.GetString("Tabla") == "imagenes_seccion" select c).FirstOrDefault();
                if (resultContador != null)
                {
                    PartitionKeyMax = resultContador.GetInt32("PartitionKeyMax").Value;
                }



                //IMAGENES SECCION
                var tablaAzure = azureTableName;
                var tableClient = new TableClient(new Uri(url), tablaAzure, new TableSharedKeyCredential(server, key));
                //  var resultsAzure = tableClient.Query<TableEntity>(filter: $"IsDeleted eq false").ToList();
                //var resultsAzure = tableClient.Query<TableEntity>(select: new string[] { "PartitionKey", "RowKey", "Id", "LastModificationTime", "DeletionTime" }).ToList();
              //  var resultsAzure = tableClient.Query<TableEntity>().ToList();


                string queryImagenes =

                    "select id,seccion_id,imagen_base_64,nombre_imagen,vigente,usuario_creacion,fecha_creacion,usuario_actualizacion,fecha_actualizacion," +
                    "usuario_eliminacion,fecha_eliminacion,Sincronizado from SCH_DOCUMENTOS.imagenes_seccion " +// where Sincronizado=0"+
                    " where  fecha_creacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_IMAGENES + "' ) OR " +
                    " fecha_actualizacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_IMAGENES + "' ) OR " +
                    " fecha_eliminacion >= (SELECT CAST(valor AS Datetime) FROM SCH_USUARIOS.parametros WHERE codigo = '" + ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_IMAGENES + "' ) ";

                var data = new List<ImagenSeccion>();
                string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(
                     connectionString))
                {
                    SqlCommand command = new SqlCommand(queryImagenes, connection);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ImagenSeccion s = new ImagenSeccion();
                            if (!reader.IsDBNull(0))
                            {
                                s.Id = reader.GetInt32(0);
                            }
                            if (!reader.IsDBNull(1))
                            {
                                s.SeccionId = reader.GetInt32(1);
                            }
                            if (!reader.IsDBNull(2))
                            {
                                s.ImagenBase64 = reader.GetString(2);
                            }
                            if (!reader.IsDBNull(3))
                            {
                                s.NombreImagen = reader.GetString(3);
                            }

                            if (!reader.IsDBNull(4))
                            {
                                s.IsDeleted = reader.GetBoolean(4);
                            }
                            if (!reader.IsDBNull(5))
                            {
                                var creatorUser = reader[5].ToString();
                                s.CreatorUserId = Int32.Parse(creatorUser);
                            }
                            if (!reader.IsDBNull(6))
                            {
                                s.CreationTime = reader.GetDateTime(6);
                            }
                            if (!reader.IsDBNull(7))
                            {
                                var User = reader[7].ToString();
                                s.LastModifierUserId = Int32.Parse(User);
                            }
                            if (!reader.IsDBNull(8))
                            {
                                s.LastModificationTime = reader.GetDateTime(8);
                            }
                            if (!reader.IsDBNull(9))
                            {
                                var User = reader[9].ToString();
                                s.DeleterUserId = Int32.Parse(User);
                            }
                            if (!reader.IsDBNull(10))
                            {
                                s.DeletionTime = reader.GetDateTime(10);
                            }
                            if (!reader.IsDBNull(11))
                            {
                                s.Sincronizado = reader.GetBoolean(11);
                            }


                            data.Add(s);


                        }
                    }
                    connection.Close();
                }



                int iteraciones = 1;
                var resultsAzure = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '"+ PartitionKeyMax + "'").ToList();
                var countRegistrosParticion = (from m in resultsAzure
                                               where m.GetString("PartitionKey").ToString() == "" + PartitionKeyMax
                                               select m).ToList();
                if (countRegistrosParticion.Count >= MaximoRegistros)
                {
                    PartitionKeyMax = PartitionKeyMax + 1;
                }
                else
                {
                    iteraciones = countRegistrosParticion.Count;
                }

                foreach (var c in data)
                {



                    var split = this.GetChunks(this.EncryptByAES(c.ImagenBase64, CatalogosCodigos.keycryt), 32000);
                    if (split.Count > 20)
                    {
                        string error = "IMG LONG " + split.Count + " INSERT ID: " + c.Id + "  PARTICIONES MAYORES A 20 ";
                        Errores.Add(error);
                    }
                    else
                    {

                        string parte1 = "";
                        try
                        {
                            if (split.Count >= 0)
                            {
                                parte1 = split[0];
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }

                        string parte2 = "";
                        try
                        {
                            if (split.Count >= 2)
                            {
                                parte2 = split[1];
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }

                        string parte3 = "";
                        try
                        {
                            if (split.Count >= 3)
                            {
                                parte3 = split[2];
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }
                        string parte4 = "";
                        try
                        {
                            if (split.Count >= 4)
                            {
                                parte4 = split[3];
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }

                        string parte5 = "";
                        try
                        {
                            if (split.Count >= 5)
                            {
                                parte5 = split[4];
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }
                        string parte6 = "";
                        try
                        {
                            if (split.Count >= 6)
                            {
                                parte6 = split[5];
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }
                        string parte7 = "";
                        try
                        {
                            if (split.Count >= 7)
                            {
                                parte7 = split[6];
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }
                        string parte8 = "";
                        try
                        {
                            if (split.Count >= 8)
                            {
                                parte8 = split[7];
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }
                        string parte9 = "";
                        try
                        {
                            if (split.Count >= 9)
                            {
                                parte9 = split[8];
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                        }
                        string parte10 = "";
                        try
                        {
                            if (split.Count >= 10)
                            {
                                parte10 = split[9];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }

                        string parte11 = "";
                        try
                        {
                            if (split.Count >= 11)
                            {
                                parte11 = split[10];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }

                        string parte12 = "";
                        try
                        {
                            if (split.Count >= 12)
                            {
                                parte12 = split[11];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }
                        string parte13 = "";
                        try
                        {
                            if (split.Count >= 13)
                            {
                                parte13 = split[12];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }

                        string parte14 = "";
                        try
                        {
                            if (split.Count >= 14)
                            {
                                parte14 = split[13];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }

                        string parte15 = "";
                        try
                        {
                            if (split.Count >= 15)
                            {
                                parte15 = split[14];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }
                        string parte16 = "";
                        try
                        {
                            if (split.Count >= 16)
                            {
                                parte16 = split[15];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }

                        string parte17 = "";
                        try
                        {
                            if (split.Count >= 17)
                            {
                                parte17 = split[16];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }

                        string parte18 = "";
                        try
                        {
                            if (split.Count >= 18)
                            {
                                parte18 = split[17];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }
                        string parte19 = "";
                        try
                        {
                            if (split.Count >= 19)
                            {
                                parte19 = split[18];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }
                        string parte20 = "";
                        try
                        {
                            if (split.Count >= 20)
                            {
                                parte20 = split[19];
                            }
                        }
                        catch (IndexOutOfRangeException e) { }


                        var imagenseccionAzure= tableClient.Query<TableEntity>(filter: $"Id eq "+c.Id+"").FirstOrDefault();
                        /*imagenseccionAzure = (from a in resultsAzure
                                                  where c.Id == a.GetInt32("Id")
                                                  select a).FirstOrDefault();*/

                        if (imagenseccionAzure != null)
                        {
                            string partitionKey = imagenseccionAzure.GetString("PartitionKey");
                            string rowKey = imagenseccionAzure.GetString("RowKey");
                            var entity = new TableEntity(partitionKey, rowKey)
                                        {
                                            { "Id", c.Id },
                                            { "SeccionId",c.SeccionId },
                                            { "NombreImagen", c.NombreImagen },
                                            { "IsDeleted", c.IsDeleted },
                                            { "CreatorUserId", c.CreatorUserId },
                                            { "CreationTime", c.CreationTime },
                                            { "LastModifierUserId", c.LastModifierUserId },
                                            { "LastModificationTime", c.LastModificationTime },
                                            { "DeleterUserId", c.DeleterUserId },
                                            { "DeletionTime", c.DeletionTime },
                                            { "ImagenParte1",parte1 },
                                            { "ImagenParte2",parte2},
                                            { "ImagenParte3",parte3},
                                            { "ImagenParte4",parte4},
                                            { "ImagenParte5",parte5 },
                                            { "ImagenParte6",parte6},
                                            { "ImagenParte7",parte7 },
                                            { "ImagenParte8",parte8 },
                                            { "ImagenParte9",parte9},
                                            { "ImagenParte10",parte10 },
                                               { "ImagenParte11",parte11 },
                                               { "ImagenParte12",parte12 },
                                               { "ImagenParte13",parte13 },
                                               { "ImagenParte14",parte14 },
                                               { "ImagenParte15",parte15 },
                                               { "ImagenParte16",parte16 },
                                               { "ImagenParte17",parte17 },
                                               { "ImagenParte18",parte18 },
                                               { "ImagenParte19",parte19 },
                                               { "ImagenParte20",parte20 },

                                        };

                            try
                            {

                                tableClient.UpdateEntity(imagenseccionAzure, ETag.All, TableUpdateMode.Replace);
                                if (!c.IsDeleted)
                                {
                                    var img = _imagenseccionRepository.Get(c.Id);
                                    img.Sincronizado = true;
                                }

                            }
                            catch (Exception e)
                            {

                                string error = "IMG UPD ID: " + c.Id + " " + e.Message;
                                IdsInserts.Add(c.Id);
                                Errores.Add(error);
                            }

                        }

                        else
                        {
                            string partitionKey = PartitionKeyMax + "";
                            string rowKey = c.Id.ToString();
                            var entity = new TableEntity(partitionKey, rowKey)
                                        {
                                            { "Id", c.Id },
                                            { "SeccionId",c.SeccionId },
                                            { "NombreImagen", c.NombreImagen },
                                            { "IsDeleted", c.IsDeleted },
                                            { "CreatorUserId", c.CreatorUserId },
                                            { "CreationTime", c.CreationTime },
                                            { "LastModifierUserId", c.LastModifierUserId },
                                            { "LastModificationTime", c.LastModificationTime },
                                            { "DeleterUserId", c.DeleterUserId },
                                            { "DeletionTime", c.DeletionTime },
                                            { "ImagenParte1",parte1 },
                                            { "ImagenParte2",parte2},
                                            { "ImagenParte3",parte3},
                                            { "ImagenParte4",parte4},
                                            { "ImagenParte5",parte5 },
                                            { "ImagenParte6",parte6},
                                            { "ImagenParte7",parte7 },
                                            { "ImagenParte8",parte8 },
                                            { "ImagenParte9",parte9},
                                            { "ImagenParte10",parte10 },
                                             { "ImagenParte11",parte11 },
                                               { "ImagenParte12",parte12 },
                                               { "ImagenParte13",parte13 },
                                               { "ImagenParte14",parte14 },
                                               { "ImagenParte15",parte15 },
                                               { "ImagenParte16",parte16 },
                                               { "ImagenParte17",parte17 },
                                               { "ImagenParte18",parte18 },
                                               { "ImagenParte19",parte19 },
                                               { "ImagenParte20",parte20 },

                                        };

                            try
                            {
                                tableClient.AddEntity(entity);
                                if (!c.IsDeleted)
                                {
                                    var img = _imagenseccionRepository.Get(c.Id);
                                    img.Sincronizado = true;
                                }
                                iteraciones++;
                            }
                            catch (Exception e)
                            {
                                string error = "IMG INSERT ID: " + c.Id + " " + e.Message;
                                IdsInserts.Add(c.Id);
                                Errores.Add(error);
                            }

                            if (iteraciones % MaximoRegistros == 0)
                            {
                                iteraciones = 1;
                                PartitionKeyMax = PartitionKeyMax + 1;
                            }



                        }

                    }


                }





                sw.WriteLine(".....ERRORES IDS");
                sw.WriteLine(String.Join(",", IdsInserts));
                sw.WriteLine(".....");

                if (Errores.Count > 0)
                {
                    sw.WriteLine("TOTAL ERROES " + Errores.Count);
                    foreach (var e in Errores)
                    {
                        sw.WriteLine(e);
                        sw.WriteLine(".....");

                    }

                    return JsonSerializer.Serialize(Errores);
                }
                else
                {

                    if (fechaUltimaSincronizacion != null)
                    {
                        var fechaSincronizacion = _parametroRepository.Get(fechaUltimaSincronizacion.Id);
                        fechaSincronizacion.Valor = fechaLocal.ToString("yyyy-MM-dd HH:mm:ss");
                        _parametroRepository.Update(fechaSincronizacion);
                    }

                        /*Actualizacion Contadores*/
                        var contadorpartitionKey = resultContador.GetString("PartitionKey").ToString();
                        var contadorrowKey = resultContador.GetString("RowKey").ToString();
                        var contador = new TableEntity(contadorpartitionKey, contadorrowKey)
                                           {
                                               { "Finalizado", resultContador.GetBoolean("Finalizado").Value},
                                               { "PartitionKeyActual", resultContador.GetInt32("PartitionKeyActual").Value },
                                               { "PartitionKeyMax",PartitionKeyMax},
                                               { "Tabla",resultContador.GetString("Tabla").ToString()}
                                           };
                        tableClientContador.UpdateEntity(contador, ETag.All, TableUpdateMode.Replace);




                        return "OK";
                    }
                }
            }

            public string SyncImagenesSeccionesLista(List<int> id)
            {
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/Sincronizacion/Imagenes" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "" + DateTime.Now.Second + ".txt");

                using (StreamWriter sw = File.CreateText(fileName))
                {

                    List<String> Errores = new List<string>();

                    string url = "https://techintpmdisqas.table.core.windows.net";
                    string server = "techintpmdisqas";
                    string key = "FEBwOmcLhdWut8/vNMQnXeiNqUDBW3petxvLsXEkd9uIaUhh5dMsdxLOgc5L5P4reiUXbu5K0dasLWkETOTTFA==";


                    var serviceClient = new TableServiceClient(new Uri(url), new TableSharedKeyCredential(server, key));


                    //IMAGENES SECCION
                    var tablaAzure = "ImagenesSeccionTest";
                    var tableClient = new TableClient(new Uri(url), tablaAzure, new TableSharedKeyCredential(server, key));
                    //  var resultsAzure = tableClient.Query<TableEntity>(filter: $"IsDeleted eq false").ToList();
                    //var resultsAzure = tableClient.Query<TableEntity>(select: new string[] { "PartitionKey", "RowKey", "Id", "LastModificationTime", "DeletionTime" }).ToList();
                    var resultsAzure = tableClient.Query<TableEntity>().ToList();
                string queryImagenes =

                        "select id,seccion_id,imagen_base_64,nombre_imagen,vigente,usuario_creacion,fecha_creacion,usuario_actualizacion,fecha_actualizacion," +
                        "usuario_eliminacion,fecha_eliminacion from SCH_DOCUMENTOS.imagenes_seccion";

                    if (id.Count > 0)
                    {

                        queryImagenes =

                           "select id,seccion_id,imagen_base_64,nombre_imagen,vigente,usuario_creacion,fecha_creacion,usuario_actualizacion,fecha_actualizacion," +
                           "usuario_eliminacion,fecha_eliminacion from SCH_DOCUMENTOS.imagenes_seccion  where id in (" + String.Join(",", id) + ")";
                    }

                    var data = new List<ImagenSeccion>();
                    string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(
                         connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryImagenes, connection);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ImagenSeccion s = new ImagenSeccion();
                                if (!reader.IsDBNull(0))
                                {
                                    s.Id = reader.GetInt32(0);
                                }
                                if (!reader.IsDBNull(1))
                                {
                                    s.SeccionId = reader.GetInt32(1);
                                }
                                if (!reader.IsDBNull(2))
                                {
                                    s.ImagenBase64 = reader.GetString(2);
                                }
                                if (!reader.IsDBNull(3))
                                {
                                    s.NombreImagen = reader.GetString(3);
                                }

                                if (!reader.IsDBNull(4))
                                {
                                    s.IsDeleted = reader.GetBoolean(4);
                                }
                                if (!reader.IsDBNull(5))
                                {
                                    var creatorUser = reader[5].ToString();
                                    s.CreatorUserId = Int32.Parse(creatorUser);
                                }
                                if (!reader.IsDBNull(6))
                                {
                                    s.CreationTime = reader.GetDateTime(6);
                                }
                                if (!reader.IsDBNull(7))
                                {
                                    var User = reader[7].ToString();
                                    s.LastModifierUserId = Int32.Parse(User);
                                }
                                if (!reader.IsDBNull(8))
                                {
                                    s.LastModificationTime = reader.GetDateTime(8);
                                }
                                if (!reader.IsDBNull(9))
                                {
                                    var User = reader[9].ToString();
                                    s.DeleterUserId = Int32.Parse(User);
                                }
                                if (!reader.IsDBNull(10))
                                {
                                    s.DeletionTime = reader.GetDateTime(10);
                                }


                                data.Add(s);


                            }
                        }
                        connection.Close();
                    }



                    int countOnpremise = data.Count();
                    int particionesOnpremise = (countOnpremise / 10);

                    /*Agrupado por 10*/
                    int i = 0;
                    var dataOnpremise = data.GroupBy(x => i++ % (particionesOnpremise == 0 ? 1 : particionesOnpremise));


                    int inicialpartitionKey = 100;
                    /*var maxPartitionKey = (from max in resultsAzure
                                           select max.GetInt32("PartitionKey")).ToList().Max();
                    if (maxPartitionKey.HasValue)
                    {
                        inicialpartitionKey = maxPartitionKey.Value;
                    }*/

                    int numParticion = 1;

                    foreach (var particionado in dataOnpremise)
                    {

                        int valpartitionKey = numParticion + inicialpartitionKey;

                        foreach (var c in particionado)
                        {



                            var split = this.GetChunks(this.EncryptByAES(c.ImagenBase64, CatalogosCodigos.keycryt), 32000);
                            if (split.Count > 20)
                            {

                                sw.WriteLine("LONG " + split.Count);
                                string error = "IMG INSERT ID: " + c.Id + "  PARTICIONES MAYORES A 20";
                                sw.WriteLine(error);
                                sw.WriteLine(".....");

                            }
                            else
                            {

                                string parte1 = "";
                                try
                                {
                                    if (split.Count >= 0)
                                    {
                                        parte1 = split[0];
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                }

                                string parte2 = "";
                                try
                                {
                                    if (split.Count >= 2)
                                    {
                                        parte2 = split[1];
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                }

                                string parte3 = "";
                                try
                                {
                                    if (split.Count >= 3)
                                    {
                                        parte3 = split[2];
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                }
                                string parte4 = "";
                                try
                                {
                                    if (split.Count >= 4)
                                    {
                                        parte4 = split[3];
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                }

                                string parte5 = "";
                                try
                                {
                                    if (split.Count >= 5)
                                    {
                                        parte5 = split[4];
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                }
                                string parte6 = "";
                                try
                                {
                                    if (split.Count >= 6)
                                    {
                                        parte6 = split[5];
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                }
                                string parte7 = "";
                                try
                                {
                                    if (split.Count >= 7)
                                    {
                                        parte7 = split[6];
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                }
                                string parte8 = "";
                                try
                                {
                                    if (split.Count >= 8)
                                    {
                                        parte8 = split[7];
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                }
                                string parte9 = "";
                                try
                                {
                                    if (split.Count >= 9)
                                    {
                                        parte9 = split[8];
                                    }
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                }
                                string parte10 = "";
                                try
                                {
                                    if (split.Count >= 10)
                                    {
                                        parte10 = split[9];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }

                                string parte11 = "";
                                try
                                {
                                    if (split.Count >= 11)
                                    {
                                        parte11 = split[10];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }

                                string parte12 = "";
                                try
                                {
                                    if (split.Count >= 12)
                                    {
                                        parte12 = split[11];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }
                                string parte13 = "";
                                try
                                {
                                    if (split.Count >= 13)
                                    {
                                        parte13 = split[12];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }

                                string parte14 = "";
                                try
                                {
                                    if (split.Count >= 14)
                                    {
                                        parte14 = split[13];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }

                                string parte15 = "";
                                try
                                {
                                    if (split.Count >= 15)
                                    {
                                        parte15 = split[14];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }
                                string parte16 = "";
                                try
                                {
                                    if (split.Count >= 16)
                                    {
                                        parte16 = split[15];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }

                                string parte17 = "";
                                try
                                {
                                    if (split.Count >= 17)
                                    {
                                        parte17 = split[16];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }

                                string parte18 = "";
                                try
                                {
                                    if (split.Count >= 18)
                                    {
                                        parte18 = split[17];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }
                                string parte19 = "";
                                try
                                {
                                    if (split.Count >= 19)
                                    {
                                        parte19 = split[18];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }
                                string parte20 = "";
                                try
                                {
                                    if (split.Count >= 20)
                                    {
                                        parte20 = split[19];
                                    }
                                }
                                catch (IndexOutOfRangeException e) { }


                                var imagenseccionAzure = (from a in resultsAzure
                                                          where c.Id == a.GetInt32("Id")
                                                          select a).FirstOrDefault();



                                if (imagenseccionAzure != null)
                                {
                                    bool debeActualizar = false;
                                    var fechaActualizacionString = imagenseccionAzure.GetDateTimeOffset("LastModificationTime");
                                    var fechaEliminacionString = imagenseccionAzure.GetDateTimeOffset("DeletionTime");
                                    if (c.DeletionTime.HasValue)//Valor Fecha Eliminacion Onpremise
                                    {
                                        debeActualizar = true;
                                    }
                                    else if (c.LastModificationTime.HasValue)
                                    {
                                        if (!fechaActualizacionString.HasValue)
                                        {
                                            debeActualizar = true;
                                        }
                                        else
                                        {
                                            var fechaActualizacion = fechaActualizacionString.Value;
                                            var locaTime = fechaActualizacionString.Value.LocalDateTime;
                                            if (fechaActualizacion != null
                                                && c.LastModificationTime.Value > locaTime
                                                )
                                            {
                                                debeActualizar = true;
                                            }
                                        }
                                    }



                                    if (debeActualizar)
                                    {
                                        string partitionKey = imagenseccionAzure.GetString("PartitionKey");
                                        string rowKey = imagenseccionAzure.GetString("RowKey");
                                        var entity = new TableEntity(partitionKey, rowKey)
                                        {
                                            { "Id", c.Id },
                                            { "SeccionId",c.SeccionId },
                                            { "NombreImagen", c.NombreImagen },
                                            { "IsDeleted", c.IsDeleted },
                                            { "CreatorUserId", c.CreatorUserId },
                                            { "CreationTime", c.CreationTime },
                                            { "LastModifierUserId", c.LastModifierUserId },
                                            { "LastModificationTime", c.LastModificationTime },
                                            { "DeleterUserId", c.DeleterUserId },
                                            { "DeletionTime", c.DeletionTime },
                                            { "ImagenParte1",parte1 },
                                            { "ImagenParte2",parte2},
                                            { "ImagenParte3",parte3},
                                            { "ImagenParte4",parte4},
                                            { "ImagenParte5",parte5 },
                                            { "ImagenParte6",parte6},
                                            { "ImagenParte7",parte7 },
                                            { "ImagenParte8",parte8 },
                                            { "ImagenParte9",parte9},
                                            { "ImagenParte10",parte10 },
                                               { "ImagenParte11",parte11 },
                                               { "ImagenParte12",parte12 },
                                               { "ImagenParte13",parte13 },
                                               { "ImagenParte14",parte14 },
                                               { "ImagenParte15",parte15 },
                                               { "ImagenParte16",parte16 },
                                               { "ImagenParte17",parte17 },
                                               { "ImagenParte18",parte18 },
                                               { "ImagenParte19",parte19 },
                                               { "ImagenParte20",parte20 },

                                        };


                                        try
                                        {
                                            tableClient.UpdateEntity(imagenseccionAzure, ETag.All, TableUpdateMode.Replace);
                                        }
                                        catch (Exception e)
                                        {
                                            string error = "IMG UPD ID: " + c.Id + " " + e.Message;
                                            Errores.Add(error);
                                            sw.WriteLine(error);
                                        }
                                    }


                                }

                                else
                                {
                                    string partitionKey = valpartitionKey.ToString();
                                    string rowKey = c.Id.ToString();
                                    var entity = new TableEntity(partitionKey, rowKey)
                                        {
                                            { "Id", c.Id },
                                            { "SeccionId",c.SeccionId },
                                            { "NombreImagen", c.NombreImagen },
                                            { "IsDeleted", c.IsDeleted },
                                            { "CreatorUserId", c.CreatorUserId },
                                            { "CreationTime", c.CreationTime },
                                            { "LastModifierUserId", c.LastModifierUserId },
                                            { "LastModificationTime", c.LastModificationTime },
                                            { "DeleterUserId", c.DeleterUserId },
                                            { "DeletionTime", c.DeletionTime },
                                            { "ImagenParte1",parte1 },
                                            { "ImagenParte2",parte2},
                                            { "ImagenParte3",parte3},
                                            { "ImagenParte4",parte4},
                                            { "ImagenParte5",parte5 },
                                            { "ImagenParte6",parte6},
                                            { "ImagenParte7",parte7 },
                                            { "ImagenParte8",parte8 },
                                            { "ImagenParte9",parte9},
                                            { "ImagenParte10",parte10 },
                                               { "ImagenParte11",parte11 },
                                               { "ImagenParte12",parte12 },
                                               { "ImagenParte13",parte13 },
                                               { "ImagenParte14",parte14 },
                                               { "ImagenParte15",parte15 },
                                               { "ImagenParte16",parte16 },
                                               { "ImagenParte17",parte17 },
                                               { "ImagenParte18",parte18 },
                                               { "ImagenParte19",parte19 },
                                               { "ImagenParte20",parte20 },

                                        };


                                    try
                                    {
                                        tableClient.AddEntity(entity);
                                    }
                                    catch (Exception e)
                                    {
                                        string error = "IMG INSERT ID: " + c.Id + " " + e.Message;
                                        sw.WriteLine(error);
                                        Errores.Add(error);
                                    }
                                }

                            }


                        }
                        numParticion++;
                    }







                    if (Errores.Count > 0)
                    {
                        sw.WriteLine("TOTAL ERROES" + Errores.Count);
                        foreach (var e in Errores)
                        {
                            sw.WriteLine(e);
                            sw.WriteLine(".....");

                        }

                        return JsonSerializer.Serialize(Errores);
                    }
                    else
                    {
                        return "OK";
                    }
                }
            }

            public List<string> GetChunks(string value, int chunkSize)
            {
                List<string> triplets = new List<string>();
                while (value.Length > chunkSize)
                {
                    triplets.Add(value.Substring(0, chunkSize));
                    value = value.Substring(chunkSize);
                }

                if (value != "")
                    triplets.Add(value);
                return triplets;
            }

            public int GetOriginalLengthInBytes(string base64string)
            {
                if (string.IsNullOrEmpty(base64string)) { return 0; }

                var characterCount = base64string.Length;
                var paddingCount = base64string.Substring(characterCount - 2, 2)
                                               .Count(c => c == '=');
                return (3 * (characterCount / 4)) - paddingCount;
            }


            public ExcelPackage SyncImagenesExcel()
            {

                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/Sincronizacion/ImageneExcels" + ".txt");

                using (StreamWriter sw = File.CreateText(fileName))
                {

                    ExcelPackage excel = new ExcelPackage();
                    var h = excel.Workbook.Worksheets.Add("Secciones");

                    string queryImagenes =

                        "select id,seccion_id,imagen_base_64,nombre_imagen,vigente,usuario_creacion,fecha_creacion,usuario_actualizacion,fecha_actualizacion," +
                        "usuario_eliminacion,fecha_eliminacion from SCH_DOCUMENTOS.imagenes_seccion";
                    //					where seccion_id not in (51,983,391,403,414,417,451,878,879,902,1240,1226,1287,1416,1417,1418,1547,397,1558,1829,1322,2721);";
                    /*"select i.id,i.seccion_id,i.imagen_base_64,i.nombre_imagen,i.vigente,i.usuario_creacion,i.fecha_creacion,i.usuario_actualizacion,i.fecha_actualizacion,i.usuario_eliminacion,i.fecha_eliminacion from SCH_DOCUMENTOS.imagenes_seccion i," +
                    "SCH_DOCUMENTOS.secciones s,SCH_DOCUMENTOS.documentos d where i.seccion_id = s.id  and s.documento_id = d.id  and s.documento_id = 26 and d.carpeta_id = 20 ";*/



                    var data = new List<ImagenSeccion>();
                    string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(
                         connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryImagenes, connection);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ImagenSeccion s = new ImagenSeccion();
                                if (!reader.IsDBNull(0))
                                {
                                    s.Id = reader.GetInt32(0);
                                }
                                if (!reader.IsDBNull(1))
                                {
                                    s.SeccionId = reader.GetInt32(1);
                                }
                                if (!reader.IsDBNull(2))
                                {
                                    s.ImagenBase64 = reader.GetString(2);
                                }
                                if (!reader.IsDBNull(3))
                                {
                                    s.NombreImagen = reader.GetString(3);
                                }

                                if (!reader.IsDBNull(4))
                                {
                                    s.IsDeleted = reader.GetBoolean(4);
                                }
                                if (!reader.IsDBNull(5))
                                {
                                    var creatorUser = reader[5].ToString();
                                    s.CreatorUserId = Int32.Parse(creatorUser);
                                }
                                if (!reader.IsDBNull(6))
                                {
                                    s.CreationTime = reader.GetDateTime(6);
                                }
                                if (!reader.IsDBNull(7))
                                {
                                    var User = reader[7].ToString();
                                    s.LastModifierUserId = Int32.Parse(User);
                                }
                                if (!reader.IsDBNull(8))
                                {
                                    s.LastModificationTime = reader.GetDateTime(8);
                                }
                                if (!reader.IsDBNull(9))
                                {
                                    var User = reader[9].ToString();
                                    s.DeleterUserId = Int32.Parse(User);
                                }
                                if (!reader.IsDBNull(10))
                                {
                                    s.DeletionTime = reader.GetDateTime(10);
                                }


                                data.Add(s);


                            }
                        }
                        connection.Close();
                    }


                    h.Cells["A1"].Value = "CARPETA";
                    h.Cells["B1"].Value = "DOCUMENTO";
                    h.Cells["C1"].Value = "SECCION";
                    h.Cells["D1"].Value = "TAMANO DE PARTES DESENCRYPT";
                    h.Cells["E1"].Value = "TAMANO DE PARTES ENCRYPT";
                    h.Cells["F1"].Value = "KB";
                    int count = 2;
                    foreach (var c in data)
                    {
                        try
                        {
                            var splitdes = this.GetChunks(c.ImagenBase64, 32000);
                            var split = this.GetChunks(this.EncryptByAES(c.ImagenBase64, CatalogosCodigos.keycryt), 32000);
                            // var split = this.GetChunks(this.EncryptByAES(c.ImagenBase64, CatalogosCodigos.keycryt), 32000);
                            var bytes = this.GetOriginalLengthInBytes(c.ImagenBase64) / 1024;

                            var i = _imagenseccionRepository.GetAll().Where(x => x.Id == c.Id).FirstOrDefault();
                            var seccion = _seccionRepository.GetAll().Where(x => x.Id == i.SeccionId).FirstOrDefault();
                            var documento = _documentoRepository.GetAll().Where(x => x.Id == seccion.DocumentoId).FirstOrDefault();
                            var carpeta = _carpetaRepository.GetAll().Where(x => x.Id == documento.CarpetaId).FirstOrDefault();
                            if (i != null)
                            {

                                h.Cells["A" + count].Value = carpeta.NombreCorto;
                                h.Cells["B" + count].Value = documento.Nombre;
                                h.Cells["C" + count].Value = seccion.NombreSeccion;
                                h.Cells["D" + count].Value = splitdes.Count;
                                h.Cells["E" + count].Value = split.Count;
                                h.Cells["F" + count].Value = bytes;

                                count++;

                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }

                    return excel;
                }



            }

            public List<List<int>> ParticionadaImagenes()
            {
                List<List<int>> result = new List<List<int>>();

                string queryImagenes = "select id from SCH_DOCUMENTOS.imagenes_seccion ORDER BY id asc";



                var data = new List<int>();
                string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(
                     connectionString))
                {
                    SqlCommand command = new SqlCommand(queryImagenes, connection);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            if (!reader.IsDBNull(0))
                            {
                                data.Add(reader.GetInt32(0));
                            }

                        }
                    }
                    connection.Close();
                }

                int countOnpremise = data.Count();
                int particionesOnpremise = (countOnpremise / 5);

                int i = 0;
                var dataOnpremise = data.GroupBy(x => i++ % particionesOnpremise);

                foreach (var particionado in dataOnpremise)
                {
                    result.Add(particionado.ToList());

                }


                return result;
            }

            public FechasSincronizacion UltimasFechasSincronizacion()
            {
                var fechaUsuario = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_USUARIOS).FirstOrDefault();

                var fechaCarpetas = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_CARPETAS).FirstOrDefault();

                var fechaDocumentos = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_DOCUMENTOS).FirstOrDefault();

                var fechaSecciones = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_SECCIONES).FirstOrDefault();

                var fechaImagenes = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.FECHA_ULTIMA_SINCRONIZACION_IMAGENES).FirstOrDefault();


                FechasSincronizacion r = new FechasSincronizacion();
                r.fechaUsuarios = fechaUsuario != null ? fechaUsuario.Valor : "";
                r.fechaCarpetas = fechaCarpetas != null ? fechaCarpetas.Valor : "";
                r.fechaDocumentos = fechaDocumentos != null ? fechaDocumentos.Valor : "";
                r.fechaSecciones = fechaSecciones != null ? fechaSecciones.Valor : "";
                r.fechaImagenes = fechaImagenes != null ? fechaImagenes.Valor : "";

                return r;
            }

        public string VerificarContraseña(string pass)
        {
            

                 var clave = _parametroRepository.GetAll().Where(c => c.Codigo == ProyectoCodigos.SINCRONIZAR_NUBE_CONTRATOS).FirstOrDefault();

            if (clave != null)
            {
                if (clave.Valor.ToUpper() == pass.ToUpper())
                {
                    return "OK";
                }
                else
                {
                    return "INCORRECTO";
                }
            }
            else {
                return "INCORRECTO";
            }
        }

        public int superaCaracteres(string contenido)
        {
            int caracteresEncryptados = this.EncryptByAES(contenido, CatalogosCodigos.keycryt).Length;
            return caracteresEncryptados;
        }
    }
    }


