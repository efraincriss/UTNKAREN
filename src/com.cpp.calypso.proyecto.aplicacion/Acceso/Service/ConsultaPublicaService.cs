using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Abp.Extensions;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Accesos;
using com.cpp.calypso.proyecto.dominio.Constantes;
using Newtonsoft.Json;
using OfficeOpenXml;
using Xceed.Words.NET;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Service
{
    public class ConsultaPublicaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ConsultaPublica, ConsultaPublicaDto, PagedAndFilteredResultRequestDto>, IConsultaPublicaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Ciudad> _repositoryCiudad;
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<Colaboradores> _colaboradoresRepository;
        private readonly IBaseRepository<CorreoLista> _correosListaRepository;
        private readonly IBaseRepository<ListaDistribucion> _listaDistribucionRepository;
        private readonly IBaseRepository<Catalogo> _catalogorepository;
        private readonly IBaseRepository<Usuario> _usuarioRepository;

        public ConsultaPublicaAsyncBaseCrudAppService(
            IBaseRepository<ConsultaPublica> repository,
            IBaseRepository<Ciudad> repositoryCiudad,
            IBaseRepository<Archivo> archivoRepository,
            IBaseRepository<Colaboradores> colaboradoresRepository,
            IBaseRepository<CorreoLista> correosListaRepository,
            IBaseRepository<ListaDistribucion> listaDistribucionRepository,
             IBaseRepository<Catalogo> catalogorepository,
              IBaseRepository<Usuario> usuarioRepository
            ) : base(repository)
        {
            _repositoryCiudad = repositoryCiudad;
            _archivoRepository = archivoRepository;
            _colaboradoresRepository = colaboradoresRepository;
            _correosListaRepository = correosListaRepository;
            _listaDistribucionRepository = listaDistribucionRepository;
            _catalogorepository = catalogorepository;
            _usuarioRepository = usuarioRepository;
        }


        public List<ConsultaPublicaDto> BuscarPorIdentificacionNombre(string identificacion = "", string nombre = "")
        {
            var query = Repository.GetAll().Include(o => o.TipoIdentificacion);

            if (!identificacion.IsNullOrEmpty())
            {
                query = query.Where(o => o.identificacion.StartsWith(identificacion));
            }

            if (!nombre.IsNullOrEmpty())
            {
                query = query.Where(o =>
                    o.nombres_completos.Contains(nombre));
            }

            var entities = query.ToList();
            var dto = Mapper.Map<List<ConsultaPublica>, List<ConsultaPublicaDto>>(entities);
            return dto;
        }

        public string GenerarWord(int consultaPublicaId)
        {
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuarioencontrado = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
            if (usuarioencontrado != null && usuarioencontrado.Id > 0)
            {


                var consulta = Repository.Get(consultaPublicaId);
                var proyecto = _catalogorepository.Get(consulta.ProyectoId);

                // Path Plantilla
                string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/Accesos/Anexo10.docx");

                if (File.Exists((string)filename))
                {
                    Random a = new Random();
                    var valor = a.Next(1, 100000);
                    string salida = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/Accesos/DocumentosGenerados/" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + "_" + consulta.nombres_completos + ".docx");

                    using (var plantilla = DocX.Load(filename))
                    {
                        var document = DocX.Create(salida);
                        document.InsertDocument(plantilla);

                        MemoryStream ms = new MemoryStream(usuarioencontrado.Firma);
                        System.Drawing.Image Imagen = System.Drawing.Image.FromStream(ms);
                        var addresses = document.FindAll("{firma}").FirstOrDefault();
                        using (MemoryStream imgStream = new MemoryStream())
                        {
                            Imagen.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
                            imgStream.Seek(0, SeekOrigin.Begin);
                            var image = document.AddImage(imgStream);
                            Picture picture = image.CreatePicture(89, 89);

                            // Insert a new Paragraph into the document.
                            Paragraph p1 = document.Paragraphs.Where(x => x.Text.Contains("{firma}")).FirstOrDefault();

                            p1.AppendPicture(picture);
                        }
                        Imagen.Dispose();
                        document.ReplaceText("{fecha_generacion}", consulta.fecha_consulta.HasValue ? consulta.fecha_consulta.GetValueOrDefault().ToShortDateString() : DateTime.Now.ToShortDateString());
                        document.ReplaceText("{proyecto}", proyecto.nombre);
                        document.ReplaceText("{nombre_completo}", consulta.nombres_completos);
                        document.ReplaceText("{identificacion}", consulta.identificacion);
                        document.ReplaceText("{condicion}", consulta.condicion_cedulado);
                        document.ReplaceText("{firma}", "");
                        document.ReplaceText("{USERNAME}", usuarioencontrado.NombresCompletos.ToUpper());


                        document.Save();
                        return salida;
                    }

                }
                else
                {
                    return null;
                }


            }
            else
            {
                return null;
            }
        }


        public void SubirPdf(int consultaPublicaId, ArchivoDto archivo)
        {
            var consulta = Repository.Get(consultaPublicaId);
            if (consulta.ArchivoPdfId > 0)
            {
                var archivoId = consulta.ArchivoPdfId;
                var file = consulta.ArchivoPdf;

                consulta.ArchivoPdfId = null;
                Repository.Update(consulta);
                _archivoRepository.Delete(file);

            }
            var entity = _archivoRepository.Insert(Mapper.Map<ArchivoDto, Archivo>(archivo));


            consulta.ArchivoPdf = entity;
            Repository.Update(consulta);
        }



        public ConsultaPublicaDto ExisteCandidato(string identificacion)
        {
            var count = Repository.GetAll()
                .Count(o => o.identificacion == identificacion);


            if (count > 0)
            {
                var entity = Repository
                    .GetAll()
                    .Include(o => o.Proyecto)
                    .FirstOrDefault(o => o.identificacion == identificacion);
                return Mapper.Map<ConsultaPublica, ConsultaPublicaDto>(entity);
            }
            return new ConsultaPublicaDto();
        }

        public bool EnviarCorreoElectronico(int id, string asunto, string cuerpo, bool IsBodyHtml, HttpPostedFileBase UploadedFile, string ListaDistribucionId)
        {
            var consulta = Repository.Get(id);

            try
            {
                //Usuario

                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com");
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress("pmdis_cpp@outlook.com", "PMDIS", Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = consulta.identificacion + " - " + consulta.nombres_completos;
                //Aquí ponemos el mensaje que incluirá el correo
                mail.Body = "Se envía información de consulta pública de " + consulta.nombres_completos + ".";
                mail.IsBodyHtml = true;
                //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor

                var listacorreos = new List<String>();
                var Listas = ListaDistribucionId.Split(",");
                foreach (var ListaId in Listas)

                {
                    var Id = Convert.ToInt32(ListaId);
                    var correos = _correosListaRepository.GetAll()
                                                       .Where(c => c.ListaDistribucionId == Id)
                                                       .Where(c => c.ListaDistribucion.vigente)
                                                       .Where(c => c.vigente).Select(c => c.correo).ToList();
                    listacorreos.AddRange(correos);
                }


                if (listacorreos.Count > 0)
                {
                    foreach (var correo in listacorreos)
                    {
                        mail.To.Add(correo);
                    }

                }

                //Si queremos enviar archivos adjuntos tenemos que especificar la ruta en donde se encuentran
                string url = "";
                if (UploadedFile != null)
                {



                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    Random a = new Random();
                    var extra = a.Next(1, 100000);
                    UploadedFile.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosConsultaPublica/" + extra + "_" + fileName));

                    url = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosConsultaPublica/" + extra + "_" + fileName);
                    if (File.Exists((string)url))
                    {
                        mail.Attachments.Add(new Attachment(url));
                    }

                }


                //Configuracion del SMTP
                SmtpServer.Port = 587; //Puerto que utiliza Gmail para sus servicios
                                       //Especificamos las credenciales con las que enviaremos el mail

                SmtpServer.Credentials = new System.Net.NetworkCredential("pmdis_cpp@outlook.com", "pmdis.2019");
                SmtpServer.Port = 587;

                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                if (File.Exists((string)url))
                {
                    File.Delete(url);
                }

                return true;

            }
            catch (Exception ex)
            {
                Logger.Debug(ex.Message);
                Logger.Error(ex.Message);
                Logger.Warn(ex.Message);
                ElmahExtension.LogToElmah(ex);
                return false;

            }

        }

        public List<ListaDistribucion> ListaDistribucion(int Tipo = 0)
        {
            var listadistribucion = _listaDistribucionRepository.GetAll()
                                                                .Where(c => c.vigente)
                                                                .ToList();
            return listadistribucion;
        }


        public string RealizarConsulta(string queryString)
        {

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CatalogosCodigos.DEFAULT_NAME_CONNECTION_STRING].ConnectionString;
                {
                    using (SqlConnection connection = new SqlConnection(
                               connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        command.Connection.Open();
                        //command.ExecuteNonQuery();

                        List<Dictionary<string, object>> Listado = new List<Dictionary<string, object>>();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> dato = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {

                                    dato.Add(reader.GetName(i), reader.GetValue(i));

                                }
                                Listado.Add(dato);
                            }
                        }
                        var JsonResult = JsonConvert.SerializeObject(Listado);
                        return Listado.Count > 0 ? JsonResult : reader.RecordsAffected + " fila(s) afectadas";

                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        public byte[] ConsultarFotoUsuario()
        {
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuarioencontrado = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
            if (usuarioencontrado != null && usuarioencontrado.Id > 0)
            {
                return usuarioencontrado.Firma != null ? usuarioencontrado.Firma : null;
            }
            else
            {
                return null;
            }

        }

        public byte[] GuardarFotoUsuario(HttpPostedFileBase UploadedFile)
        {
            byte[] a = null;
            if (UploadedFile != null)
            {
                using (Bitmap bitMap = new Bitmap(UploadedFile.InputStream))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        a = ms.ToArray();

                    }
                }

                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

                var usuarioencontrado = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
                if (usuarioencontrado != null && usuarioencontrado.Id > 0)
                {
                    usuarioencontrado.Firma = a;
                    _usuarioRepository.Update(usuarioencontrado);
                    return usuarioencontrado.Firma != null ? usuarioencontrado.Firma : null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public List<Object> RealizarMultiplesConsultas(string query_list)
        {
            List<Object> Errores = new List<Object>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CatalogosCodigos.DEFAULT_NAME_CONNECTION_STRING].ConnectionString;
                {
                    using (SqlConnection connection = new SqlConnection(
                               connectionString))
                    {

                        var listado_consultas = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(query_list);

                        foreach (var c in listado_consultas)
                        {
                            SqlCommand command = new SqlCommand(c.FirstOrDefault().Value.ToString(), connection);
                            try
                            {

                                command.Connection.Open();
                                var reader = command.ExecuteReader();
                                object dato = new { Id = c.FirstOrDefault().Key.ToString(), result = reader.RecordsAffected + " fila(s) afectadas" };

                                Errores.Add(dato);
                                command.Connection.Close();
                            }
                            catch (Exception ex)
                            {
                                object dato = new { Id = c.FirstOrDefault().Key, result = ex.Message };
                                Errores.Add(dato);
                                command.Connection.Close();
                            }
                        }


                        var JsonResult = JsonConvert.SerializeObject(Errores);

                        return Errores;
                    }
                }
            }
            catch (Exception ex)
            {

                object dato = new { Id = "message", result = ex.Message };
                Errores.Add(dato);
                return Errores;
            }
        }

        public ExcelPackage Reporte(DateTime? desde, DateTime? hasta)
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/ReporteRC.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("ConsutaRegistroCivil", pck.Workbook.Worksheets[1]);
            }
            ExcelWorksheet h = excel.Workbook.Worksheets[1];

            var result = this.BuscarPorIdentificacionNombre("", "").OrderByDescending(c=>c.fecha_consulta).ToList();
           
         
            if (desde.HasValue && hasta.HasValue)
            {
                result = (from r in result
                          where r.fecha_consulta.HasValue
                          where r.fecha_consulta.Value.Date >= desde.Value.Date
                          where r.fecha_consulta.Value.Date <= hasta.Value.Date
                          select r).ToList();
            }


            int count = 8;
            int rowInicial = 7;
            int rowFinal = 0;
            string cell = "";

            cell = "D" + 3;
            if (desde.HasValue && hasta.HasValue)
            {
                h.Cells[cell].Value = desde.Value.ToShortDateString() + " - "+ hasta.Value.ToShortDateString();
            }
            
  

            foreach (var d in result)
            {
                cell = "A" + count;
                h.Cells[cell].Value = "1709564007";
              //  h.Cells[cell].Style.Font.Bold = true;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                cell = "B" + count;
                h.Cells[cell].Value = d.fecha_consulta.HasValue?d.fecha_consulta.Value.ToString("dd/MM/yyyy HH:mm:ss"):"";
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                cell = "C" + count;
                h.Cells[cell].Value = d.identificacion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                cell = "D" + count;
                h.Cells[cell].Value = d.nombres_completos;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;
                cell = "E" + count;
                h.Cells[cell].Value =String.IsNullOrEmpty(d.tipoRC)?"":d.tipoRC.ToUpper();
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Justify;

                count++;
                rowFinal++;
            }

      //      h.Cells[8,1,h.Dimension.End.Row, 5].AutoFilter = true;

            return excel;
        }


    }
}

