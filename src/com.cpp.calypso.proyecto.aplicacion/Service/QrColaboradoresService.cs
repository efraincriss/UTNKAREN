using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class QrColaboradoresAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<QrColaboradores, QrColaboradoresDto, PagedAndFilteredResultRequestDto>, IQrColaboradoresAsyncBaseCrudAppService

    {
        private readonly IBaseRepository<Colaboradores> _colaboradoresRepository;
        private readonly IBaseRepository<ColaboradorBaja> _colaboradoresBajaRepository;
        private readonly IBaseRepository<Archivo> _archivo;
        private readonly IBaseRepository<ColaboradoresFotografia> _colaboradoresFotografia;
        public QrColaboradoresAsyncBaseCrudAppService(IBaseRepository<QrColaboradores> repository, IBaseRepository<Colaboradores> colaboradoresRepository, IBaseRepository<ColaboradoresFotografia> colaboradoresFotografia, IBaseRepository<Archivo> archivo,
            IBaseRepository<ColaboradorBaja> colaboradoresBajaRepository) : base(repository)
        {
            _colaboradoresRepository = colaboradoresRepository;
            _colaboradoresBajaRepository = colaboradoresBajaRepository;
            _colaboradoresFotografia = colaboradoresFotografia;
            _archivo = archivo;
        }
        public bool DarDeBajaQr(QrColaboradorModel row)
        {
            try
            {
                Random x = new Random();
                var xunique = x.Next(9999, 999999);

                var qr = Repository.Get(row.Id);
                qr.FechaEntrega = null; //Validacion
                Repository.Update(qr);

                var col = _colaboradoresRepository.Get(row.Id);
                col.codigo_seguridad_qr = xunique.ToString(); //Validacion
                _colaboradoresRepository.Update(col);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string EntregarQr(QrColaboradorModel row)
        {
            /*if (row.FechaEntrega.HasValue)
            {
                return "ENTREGADO";
            }
            else
            {
                var n = new QrColaboradores()
                {
                    ColaboradorId = row.Id,
                    FechaEntrega = row.FechaEntrega,
                    NumeroQrGenerados = 1,
                    vigente = true
                };
                Repository.InsertAndGetId(n);
                return "OK";
            }*/
            return "OK";

        }

        public bool GenerarQrIndividual(QrColaboradorModel row)
        {
            Random x = new Random();
            var xunique = x.Next(9999, 999999);
            //COLABORADOR DATOS
            var colaborador = _colaboradoresRepository.Get(row.Id);
            if (colaborador != null)
            {
                colaborador.codigo_seguridad_qr = xunique.ToString();
                _colaboradoresRepository.Update(colaborador);

                /*
                if (row.QrId > 0)
                {
                    var qr = Repository.Get(row.QrId);
                    qr.NumeroQrGenerados = qr.NumeroQrGenerados + 1;
                    Repository.Update(qr);
                }
*/
            }


            return true;
        }

        public bool GenerarQrMasiva(List<QrColaboradorModel> list)
        {

            foreach (var row in list)
            {
                Random x = new Random();
                var xunique = x.Next(9999, 999999);
                var colaborador = _colaboradoresRepository.Get(row.Id);
                if (colaborador != null)
                {
                    colaborador.codigo_seguridad_qr = xunique.ToString();
                    _colaboradoresRepository.Update(colaborador);

                    /*
                    if (row.QrId > 0)
                    {
                        var qr = Repository.Get(row.QrId);
                        qr.NumeroQrGenerados = qr.NumeroQrGenerados + 1;
                        Repository.Update(qr);
                    }*/
                }
            }
            return true;
        }

        public string GenerarTarjeta(List<QrColaboradorModel> colaboradores)

        {
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/PlantillaTarjeta.docx");
            Random a = new Random();
            var valor = a.Next(1, 100);
            string nombre = "~/Views/PlantillaWord/Tarjetas/GeneracionTarjetas";
            if (colaboradores.Count == 0)
            {
                nombre = nombre + "_" + colaboradores.FirstOrDefault().Identificacion + "_" + DateTime.Now.Second.ToString() + ".docx";
            }
            else
            {
                nombre = nombre + "_" + DateTime.Now.Year + "" + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Second.ToString() + ".docx";
            }

            string salida = System.Web.HttpContext.Current.Server.MapPath(nombre);
            if (File.Exists((string)filename))
            {

                using (var plantilla = DocX.Load(filename))
                {


                    var document = DocX.Create(salida);
                    foreach (var col in colaboradores)
                    {
                        document.InsertDocument(plantilla);
                        string qr_id = "{qr" + "_" + col.Identificacion + "}";
                        string foto_id = "{foto" + "_" + col.Identificacion + "}";

                        document.ReplaceText("{qr}", qr_id);
                        document.ReplaceText("{foto}", foto_id);

                        var firtsname = col.Nombres.Split(' ').Length > 0 ? col.Nombres.Split(' ')[0] : col.Nombres;

                        document.ReplaceText("{nombres}", firtsname + " " + col.PrimerApellido);
                        document.ReplaceText("{identificacion}", "ID "+col.Identificacion);
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(col.CodigoSapLocal, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                        //imgBarCode.Height = 150;
                        //imgBarCode.Width = 150;
                        using (Bitmap bitMap = qrCode.GetGraphic(20))
                        {
                            using (MemoryStream img = new MemoryStream())
                            {
                                bitMap.Save(img, System.Drawing.Imaging.ImageFormat.Png);
                                var imagen = img.ToArray();
                                MemoryStream ms = new MemoryStream(imagen);
                                System.Drawing.Image Imagen = System.Drawing.Image.FromStream(ms);
                                using (MemoryStream imgStream = new MemoryStream())
                                {
                                    Imagen.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
                                    imgStream.Seek(0, SeekOrigin.Begin);
                                    var image = document.AddImage(imgStream);

                                    Picture picture = image.CreatePicture(85, 85);// 77 77
                                    // Insert a new Paragraph into the document.
                                    Paragraph p1 = document.Paragraphs.Where(x => x.Text.Contains(qr_id)).FirstOrDefault();

                                    p1.AppendPicture(picture);


                                }
                                Imagen.Dispose();



                            }
                        }


                        var archivo = _colaboradoresFotografia.GetAll()
                                                          .Where(c => c.colaborador_id == col.Id)
                                                          .Where(c => c.vigente)
                                                          .FirstOrDefault();


                        if (archivo != null && archivo.Id > 0)
                        {

                            var foto = _archivo.GetAll()
                                                             .Where(c => c.Id == archivo.archivo_id)
                                                             .Where(c => c.vigente)
                                                             .FirstOrDefault();
                            MemoryStream ms = new MemoryStream(foto.hash);
                            System.Drawing.Image Imagen = System.Drawing.Image.FromStream(ms);

                            using (MemoryStream imgStream = new MemoryStream())
                            {
                                Imagen.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
                                imgStream.Seek(0, SeekOrigin.Begin);
                                var image = document.AddImage(imgStream);
                                Picture picture = image.CreatePicture(91, 93);


                                // Insert a new Paragraph into the document.
                                Paragraph p1 = document.Paragraphs.Where(x => x.Text.Contains(foto_id)).FirstOrDefault();

                                p1.AppendPicture(picture);
                            }
                            Imagen.Dispose();

                            document.ReplaceText(foto_id, "");
                        }
                        else
                        {
                            document.ReplaceText(foto_id, "\n\n\n\n\n");
                        }


                        document.ReplaceText(qr_id, "");

                    }






                    document.Save();
                    return salida;
                }
            }
            return "";
        }
        public List<QrColaboradorModel> ListColaboradores(string search)
        {
            var query = _colaboradoresRepository.GetAllIncluding(c => c.Area, c => c.Cargo)
                                              .Where(c => c.vigente)
                                              // .Where(c => c.es_externo == false)
                                              .Where(c => c.estado == RRHHCodigos.ESTADO_ACTIVO)
                                              .Where(c => c.nombres_apellidos.Contains(search) || c.numero_identificacion.StartsWith(search))
              .OrderBy(c => c.primer_apellido)
                                              .ToList();
            var list = new List<QrColaboradorModel>();
            foreach (var q in query)
            {
                var data = new QrColaboradorModel()
                {
                    Area = q.catalogo_area_id.HasValue ? q.Area.nombre : "",
                    Cargo = q.catalogo_cargo_id.HasValue ? q.Cargo.nombre : "",
                    CodigoSap = q.es_externo.HasValue && q.es_externo.Value ? q.numero_identificacion : q.empleado_id_sap.HasValue ? q.empleado_id_sap.Value.ToString() : "",
                    CodigoSapLocal = q.es_externo.HasValue && q.es_externo.Value ? q.numero_identificacion : q.empleado_id_sap_local.HasValue ? q.empleado_id_sap_local.Value.ToString() : "",
                    Id = q.Id,
                    Identificacion = q.numero_identificacion,
                    NombreCompleto = q.nombres_apellidos,
                    TipoUsuario = q.es_externo.HasValue ? q.es_externo.Value ? "EXTERNO" : "INTERNO" : "",
                    PrimerApellido = q.primer_apellido,
                    SegundoApellido = q.segundo_apellido,
                    Nombres = q.nombres,
                    esExterno = q.es_externo.HasValue ? q.es_externo.Value : false
                };


                list.Add(data);

            }
            return list;
        }

        public bool ReemprimirQr(QrColaboradorModel row)
        {
            /**if (row.QrId > 0)
            {
                var qr = Repository.Get(row.QrId);
                qr.FechaEntrega = null;
                qr.NumeroQrGenerados = qr.NumeroQrGenerados + 1;
                Repository.Update(qr);
            }*/
            return true;
        }

        public bool RegitrarPerdida(List<QrColaboradorModel> list)
        {/*
            foreach (var q in list)
            {
                if (q.QrId > 0)
                {
                    try
                    {
                        var qr = Repository.Get(q.QrId);
                        qr.FechaEntrega = null;
                        Repository.Update(qr);
                        Random x = new Random();
                        var xunique = x.Next(9999, 999999);
                        var colaborador = _colaboradoresRepository.Get(q.ColaboradorId);
                        if (colaborador != null)
                        {
                            colaborador.codigo_seguridad_qr = xunique.ToString();
                            _colaboradoresRepository.Update(colaborador);
                        }
                    }
                    catch (Exception e)
                    {
                    }

                }
            }*/
            return true;
        }


        public List<ReingresoModel> ListColaboradoresReingreso(string search)
        {
            var query = _colaboradoresRepository.GetAllIncluding(c => c.Area, c => c.Cargo, c => c.TipoIdentificacion)
                                              .Where(c => c.vigente)
                                              .Where(c => c.es_externo == false)
                                              .Where(c => c.estado != RRHHCodigos.ESTADO_INACTIVO)
                                              .Where(c => c.nombres_apellidos.Contains(search) || c.numero_identificacion.StartsWith(search))
              .OrderBy(c => c.primer_apellido)
              .OrderByDescending(c => c.fecha_ingreso.Value)

                                              .ToList();
            var list = new List<ReingresoModel>();
            foreach (var q in query)
            {
                var data = new ReingresoModel()
                {
                    Area = q.catalogo_area_id.HasValue ? q.Area.nombre : "",
                    Cargo = q.catalogo_cargo_id.HasValue ? q.Cargo.nombre : "",
                    CodigoSap = q.es_externo.HasValue && q.es_externo.Value ? q.numero_identificacion : q.empleado_id_sap.HasValue ? q.empleado_id_sap.Value.ToString() : "",
                    Id = q.Id,
                    Identificacion = q.numero_identificacion,
                    NombreCompleto = q.nombres_apellidos,
                    TipoUsuario = q.es_externo.HasValue ? q.es_externo.Value ? "EXTERNO" : "INTERNO" : "",
                    PrimerApellido = q.primer_apellido,
                    SegundoApellido = q.segundo_apellido,
                    Nombres = q.nombres,
                    esExterno = q.es_externo.HasValue ? q.es_externo.Value : false,
                    Estado = q.estado,
                    FechaUltimoIngreso = q.fecha_ingreso.HasValue ? q.fecha_ingreso.Value.ToShortDateString() : "",
                    NumeroLejajo = q.numero_legajo_temporal,
                    TipoIdentificacion = q.TipoIdentificacion.nombre,
                    NumeroReingresos = 0,
                    Reingresos = new List<ReingresoHistoricoModel>()
                };


                list.Add(data);

            }

            foreach (var h in list)
            {
                var querybaja = _colaboradoresBajaRepository.GetAllIncluding(c => c.Colaboradores.Cargo)
                                              .Where(c => c.vigente)
                                              .Where(c => c.Colaboradores.numero_identificacion == h.Identificacion)
              .OrderByDescending(c => c.fecha_baja.Value)
                                              .ToList();

                var listhistorico = new List<ReingresoHistoricoModel>();
                foreach (var q in querybaja)
                {
                    var data = new ReingresoHistoricoModel()
                    {
                     
                        Cargo = q.Colaboradores.catalogo_cargo_id.HasValue ? q.Colaboradores.Cargo.nombre : "",
                        ColaboradorId=q.ColaboradoresId,
                        Estado= Enum.GetName(typeof(BajaEstado), q.estado),
                        FechaSalida =q.fecha_baja.HasValue?q.fecha_baja.Value.ToShortDateString():"",
                        FechaUltimoIngreso=q.Colaboradores.fecha_ingreso.HasValue?q.Colaboradores.fecha_ingreso.Value.ToShortDateString():""
                    };


                    listhistorico.Add(data);

                }

                if (listhistorico.Count > 0) {
                    h.Reingresos.AddRange(listhistorico);
                    h.NumeroReingresos = listhistorico.Count;
                }


            }

            return list;
        }

        public bool ActualizarIdSapLocal(int ColaboradorId, int empleado_id_sap_local)
        {
            var col = _colaboradoresRepository.Get(ColaboradorId);
            col.empleado_id_sap_local = empleado_id_sap_local;
            _colaboradoresRepository.Update(col);

            return true;
            
        }
    }
}
