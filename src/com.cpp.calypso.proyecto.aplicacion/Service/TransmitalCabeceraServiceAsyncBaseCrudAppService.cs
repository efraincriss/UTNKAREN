using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using Xceed.Words.NET;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class TransmitalCabeceraServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<TransmitalCabecera, TransmitalCabeceraDto, PagedAndFilteredResultRequestDto>, ITransmitalCabeceraAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<TransmitalDetalle> _repositorydetalle;
        private readonly IBaseRepository<Colaborador> _colaboradorRepository;
        private readonly IBaseRepository<OfertaComercial> _ofertacomercial;
        private readonly IBaseRepository<Usuario> _usuarioRepository;
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<Contrato> _contratoRepository;
        IBaseRepository<Catalogo> _catalogo;
        public TransmitalCabeceraServiceAsyncBaseCrudAppService(
            IBaseRepository<TransmitalCabecera> repository,
            IBaseRepository<TransmitalDetalle> repositorydetalle,
            IBaseRepository<Colaborador> colaboradorRepository,
            IBaseRepository<OfertaComercial> ofertacomercial,
            IBaseRepository<Usuario> usuarioRepository,
             IBaseRepository<Archivo> archivoRepository,
             IBaseRepository<Catalogo> catalogo,
           IBaseRepository<Contrato> contratoRepository

            ) : base(repository)
        {
            _repositorydetalle = repositorydetalle;
            _colaboradorRepository = colaboradorRepository;
            _ofertacomercial = ofertacomercial;
            _usuarioRepository = usuarioRepository;
            _archivoRepository = archivoRepository;
            _catalogo = catalogo;
            _contratoRepository = contratoRepository;

        }

        public List<TransmitalCabeceraDto> GetAllTransmitalCabeceras()
        {
            var Query = Repository.GetAllIncluding(c => c.OfertaComercial, c => c.Empresa, c => c.Cliente, c => c.Contrato, c => c.OfertaComercial.Contrato);
            var items = (from r in Query
                         where r.vigente == true
                         select new TransmitalCabeceraDto()
                         {
                             Id = r.Id,
                             OfertaComercialId = r.OfertaComercialId,
                             descripcion = r.descripcion,
                             estado = r.estado,
                             fecha_recepcion = r.fecha_recepcion,
                             vigente = r.vigente,
                             codigo_carta = r.codigo_carta,
                             codigo_transmital = r.codigo_transmital,
                             copia_a = r.copia_a,
                             dirigido_a = r.dirigido_a,
                             enviado_por = r.enviado_por,
                             fecha_emision = r.fecha_emision,
                             fecha_ultima_modificacion = r.fecha_ultima_modificacion,                
                             EmpresaId = r.EmpresaId,
                             ClienteId = r.ClienteId,
                             ContratoId = r.ContratoId

                         }).ToList();
            return items;
        }

        public TransmitalCabeceraDto GetDetalle(int TransmitalId)
        {
            var Query = Repository.GetAllIncluding(c => c.OfertaComercial, c => c.Empresa, c => c.Cliente, c => c.Contrato).Where(e => e.vigente == true).Where(c => c.Id == TransmitalId).ToList();

            var item = (from r in Query
                        where r.Id == TransmitalId
                        where r.vigente == true
                        select new TransmitalCabeceraDto()
                        {
                            Id = r.Id,
                            OfertaComercialId = r.OfertaComercialId,
                            descripcion = r.descripcion,
                            estado = r.estado,
                            fecha_recepcion = r.fecha_recepcion,
                            vigente = r.vigente,
                            codigo_carta = r.codigo_carta,
                            codigo_transmital = r.codigo_transmital,
                            copia_a = r.copia_a,
                            dirigido_a = r.dirigido_a,
                            enviado_por = r.enviado_por,
                            fecha_emision = r.fecha_emision,
                            fecha_ultima_modificacion = r.fecha_ultima_modificacion,
                            EmpresaId = r.EmpresaId,
                            ClienteId = r.ClienteId,
                            ContratoId = r.ContratoId,
                            version = r.version,
                            tiene_ofertacomercial = r.OfertaComercialId.HasValue ? true : false,
                            codigo_oferta_comercial = r.OfertaComercial != null ? r.OfertaComercial.codigo : ""

                        }).FirstOrDefault(); // Tercera

            string dirigido = "";
            if (item != null && item.dirigido_a != null && item.dirigido_a.Length > 0)
            {
                String[] copias = item.dirigido_a.Split(',');
                if (copias.Length > 0)
                {
                    foreach (var c in copias)
                    {
                        var colaborador = _colaboradorRepository.Get(Int32.Parse(c));
                        var tipo = _catalogo.Get(colaborador.ClienteId);
                        if (colaborador != null)
                        {
                            dirigido = dirigido + " " + colaborador.nombres + " " + colaborador.apellidos.ToUpper() + "(" + tipo.nombre + ")";
                        }
                    }
                }

            }
            string copia = "";
            if (item != null && item.copia_a != null && item.copia_a.Length > 0)
            {
                String[] copias = item.copia_a.Split(',');
                if (copias.Length > 0)
                {
                    foreach (var c in copias)
                    {
                        var colaborador = _colaboradorRepository.Get(Int32.Parse(c));
                        var tipo = _catalogo.Get(colaborador.ClienteId);
                        if (colaborador != null)
                        {
                            copia = copia + " " + colaborador.nombres + " " + colaborador.apellidos.ToUpper() + "(" + tipo.nombre + ")";
                        }
                    }
                }

            }
            if (item != null) {
            var definitivas = _repositorydetalle.GetAllIncluding(c => c.Transmital)
                                           .Where(c => c.vigente)
                                           .Where(c => c.TransmitalId == item.Id)
                                           .Where(c => c.es_oferta)

                                           .ToList();
            item.dirigido_a = dirigido;
            item.copia_a = copia;
            item.tiene_oferta = definitivas.Count > 0 ? true : false;
            }

            return item;
        }

        public List<TransmitalCabeceraDto> GetTransmitalCabeceras(int OfertaId)
        {
            var Query = Repository.GetAllIncluding(c => c.OfertaComercial);
            var items = (from r in Query
                         where r.vigente == true
                         where r.OfertaComercialId == OfertaId
                         select new TransmitalCabeceraDto()
                         {
                             Id = r.Id,
                             OfertaComercialId = r.OfertaComercialId,
                             descripcion = r.descripcion,
                             estado = r.estado,
                             fecha_recepcion = r.fecha_recepcion,
                             vigente = r.vigente,
                             codigo_carta = r.codigo_carta,
                             codigo_transmital = r.codigo_transmital,
                             copia_a = r.copia_a,
                             dirigido_a = r.dirigido_a,
                             enviado_por = r.enviado_por,
                             fecha_emision = r.fecha_emision,
                             fecha_ultima_modificacion = r.fecha_ultima_modificacion,

                         }).ToList();
            return items;
        }

        public bool EliminarVigencia(int Transmitalid)
        {

            var transmital = Repository.Get(Transmitalid);
            var hijos = _repositorydetalle.GetAllIncluding(c => c.Transmital).Where(c => c.TransmitalId == Transmitalid).ToList();
            if (hijos.Count() > 0)
            {
                return false;
            }
            if (transmital != null)
            {
                transmital.vigente = false;
                Repository.Update(transmital);
                return true;
            }
            else
            {
                return false;
            }
        }
        public int secuencialTransmital(int ClienteId)
        {
            int secuencia = 0;
            var listado_codigos = Repository.GetAll().Where(c => c.vigente).
                Where(c=>c.ClienteId==ClienteId)
                                                    .Where(c=>c.codigo_transmital.StartsWith("3808"))                                    
                .Select(c => c.codigo_transmital).ToList();
            if (listado_codigos.Count > 0)
            {

                List<int> numeracion = (from l in listado_codigos
                                        select Convert.ToInt32(l.Split('-')[l.Split('-').Length - 1])
                                    ).ToList();

                if (numeracion.Count > 0)
                {
                    secuencia = numeracion.Max() + 1;
                }
            }



            return secuencia;
        }

        public string CrearTransmital(TransmitalCabecera transmital)
        {
            var contrato = _contratoRepository.Get(transmital.ContratoId.Value);
            transmital.EmpresaId = contrato.EmpresaId;
            transmital.ClienteId = contrato.ClienteId;

            int count =this.secuencialTransmital(contrato.ClienteId); 

            if (transmital.OfertaComercialId != null && transmital.OfertaComercialId > 0)
            {
                var siexite = Repository.GetAll()
                                       .Where(c => c.vigente)
                                       .Where(c => c.OfertaComercialId == transmital.OfertaComercialId)
                                       .FirstOrDefault();
                if (siexite != null && siexite.Id > 0)
                {
                    return "EXISTE";
                }


                var oferta = _ofertacomercial.Get(transmital.OfertaComercialId.Value);
                transmital.codigo_transmital = "3808-B-TD-" + String.Format("{0:000000}", count);
                transmital.version = oferta.version;

                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

                var usuario = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();

                if (usuario != null)
                {
                    transmital.enviado_por = usuario.Nombres + " " + usuario.Apellidos.ToUpper() + " (CPP)";
                }
                else
                {
                    transmital.enviado_por = "";
                }


            }
            else
            {
                transmital.codigo_transmital = "3808-B-TD-" + String.Format("{0:000000}", count);
                transmital.version = "A";
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

                var usuario = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();

                if (usuario != null)
                {
                    transmital.enviado_por = usuario.Nombres + " " + usuario.Apellidos.ToUpper() + " (CPP)";
                }
                else
                {
                    transmital.enviado_por = "";
                }
            }
            
            var resultado = Repository.Insert(transmital);

            return "OK";


        }

        public List<Colaborador> ListaColaboradoresTransmital()
        {
            var colaboradores = _colaboradorRepository.GetAll().Where(c => c.vigente).ToList();

            return colaboradores;
        }

        public string GenerarWord(int id)
        {
            var Contrato = Repository.Get(id).Contrato;
            var Transmital = Repository.Get(id);

            List<String> data = new List<string>();
            string dirigido = "";
            if (Transmital.dirigido_a.Length > 0)
            {
                String[] copias = Transmital.dirigido_a.Split(',');
                if (copias.Length > 0)
                {
                    foreach (var c in copias)
                    {
                        var colaborador = _colaboradorRepository.Get(Int32.Parse(c));
                        var tipo = _catalogo.Get(colaborador.ClienteId);
                        if (colaborador != null)
                        {
                            dirigido = colaborador.nombres + " " + colaborador.apellidos.ToUpper() + "(" + tipo.nombre + ")" ;
                            data.Add(dirigido);
                        }
                    }
                }

            }
            if (data.Count > 0) {
                dirigido = String.Join(",", data);
            }


            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/TransmitalPlantilla.docx");
            //
            if (File.Exists((string)filename))
            {
                Random a = new Random();
                var valor = a.Next(1, 100000);
                string salida = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosProyectos/Transmittals/" + Transmital.codigo_transmital + "" + valor + ".docx");

                using (var plantilla = DocX.Load(filename))
                {
                    var document = DocX.Create(salida);
                    document.InsertDocument(plantilla);
                    document.ReplaceText("{codigo_transmital}", Transmital.codigo_transmital);
                    document.ReplaceText("{fecha}", DateTime.Today.ToShortDateString());
                    document.ReplaceText("{descripcion}", Contrato.sitio_referencia);
                    document.ReplaceText("{de}", Transmital.enviado_por);
                    document.ReplaceText("{para}", dirigido);

                    if (Transmital.copia_a.Length > 0)
                    {
                        String[] copias = Transmital.copia_a.Split(',');
                        if (copias.Length > 0)
                        {
                            int index = 1;
                            int indexshaya = 1;
                            foreach (var c in copias)
                            {
                                if (c != "0")
                                {
                                    var colaborador = _colaboradorRepository.Get(Int32.Parse(c));
                                    var tipo = _catalogo.Get(colaborador.ClienteId);
                                    if (tipo.codigo == "TCPP")
                                    {
                                        document.ReplaceText("{c" + index + "}", colaborador.nombres + " " + colaborador.apellidos.ToUpper() + "(" + tipo.nombre + ")");
                                        index = index + 1;
                                    }
                                    else
                                    {
                                        document.ReplaceText("{s" + indexshaya + "}", colaborador.nombres + " " + colaborador.apellidos.ToUpper() + "(" + tipo.nombre + ")");
                                        indexshaya = indexshaya + 1;
                                    }
                                }



                            }

                            while (index <= 4)
                            {
                                document.ReplaceText("{c" + index + "}", "");
                                index = index + 1;
                            }
                            while (indexshaya <= 4)
                            {
                                document.ReplaceText("{s" + indexshaya + "}", "");
                                indexshaya = indexshaya + 1;
                            }


                        }

                        //Tabla

                        var detalles_transmital = _repositorydetalle.GetAll()
                                                    .Where(c => c.vigente)
                                                    .Where(c => c.TransmitalId == Transmital.Id)
                                                    .ToList();

                        if (detalles_transmital.Count > 0)
                        {
                            Table t = document.Tables[0];
                            t.Alignment = Alignment.center;

                            int fila = 12;

                            foreach (var dt in detalles_transmital)
                            {
                                if (fila <= 22)
                                {
                                    t.Rows[fila].Cells[1].Paragraphs.First().Append(dt.codigo_detalle).FontSize(10D).Font(new Font("Arial"));
                                    t.Rows[fila].Cells[2].Paragraphs.First().Append(dt.descripcion).FontSize(10D).Font(new Font("Arial"));
                                    t.Rows[fila].Cells[3].Paragraphs.First().Append(Transmital.version).FontSize(10D).Font(new Font("Arial"));
                                    t.Rows[fila].Cells[4].Paragraphs.First().Append("" + dt.nro_hojas).FontSize(10D).Font(new Font("Arial"));
                                    t.Rows[fila].Cells[5].Paragraphs.First().Append("" + dt.nro_copias).FontSize(10D).Font(new Font("Arial"));
                                    t.Rows[fila].Cells[6].Paragraphs.First().Append(Transmital.tipo_formato).FontSize(10D).Font(new Font("Arial"));
                                    t.Rows[fila].Cells[7].Paragraphs.First().Append(Transmital.tipo_proposito).FontSize(10D).Font(new Font("Arial"));
                                    t.Rows[fila].Cells[8].Paragraphs.First().Append(Transmital.tipo).FontSize(10D).Font(new Font("Arial"));
                                }
                                else
                                {


                                }

                                fila = fila + 1;
                            }



                        }
                    }


                    document.Save();

                    return salida;
                }

            }
            else
            {
                return null;
            }


        }


        public string GenerarWordTransmittal(int id)
        {
            var transmital = Repository.GetAll().Where(c => c.vigente)
                                                      .Where(c => c.OfertaComercialId.HasValue)
                                                      .Where(c => c.OfertaComercialId == id)
                                                      .FirstOrDefault();
            if (transmital != null && transmital.Id > 0)
            {
                var Contrato = Repository.Get(transmital.Id).Contrato;
                var Transmital = Repository.Get(transmital.Id);

                List<String> data = new List<string>();
                string dirigido = "";
                if (Transmital.dirigido_a.Length > 0)
                {
                    String[] copias = Transmital.dirigido_a.Split(',');
                    if (copias.Length > 0)
                    {
                        foreach (var c in copias)
                        {
                            var colaborador = _colaboradorRepository.Get(Int32.Parse(c));
                            var tipo = _catalogo.Get(colaborador.ClienteId);
                            if (colaborador != null)
                            {
                                dirigido = colaborador.nombres + " " + colaborador.apellidos.ToUpper() + "(" + tipo.nombre + ")";
                                data.Add(dirigido);
                            }
                        }
                    }

                }
                if (data.Count > 0)
                {
                    dirigido = String.Join(",", data);
                }


                string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/TransmitalPlantilla.docx");
                //
                if (File.Exists((string)filename))
                {
                    Random a = new Random();
                    var valor = a.Next(1, 100000);
                    string salida = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosProyectos/Transmittals/" + Transmital.codigo_transmital + "" + valor + ".docx");

                    using (var plantilla = DocX.Load(filename))
                    {
                        var document = DocX.Create(salida);
                        document.InsertDocument(plantilla);
                        document.ReplaceText("{codigo_transmital}", Transmital.codigo_transmital);
                        document.ReplaceText("{fecha}", DateTime.Today.ToShortDateString());
                        document.ReplaceText("{descripcion}", Contrato.sitio_referencia);
                        document.ReplaceText("{de}", Transmital.enviado_por);
                        document.ReplaceText("{para}", dirigido);

                        if (Transmital.copia_a.Length > 0)
                        {
                            String[] copias = Transmital.copia_a.Split(',');
                            if (copias.Length > 0)
                            {
                                int index = 1;
                                int indexshaya = 1;
                                foreach (var c in copias)
                                {
                                    if (c != "0")
                                    {
                                        var colaborador = _colaboradorRepository.Get(Int32.Parse(c));
                                        var tipo = _catalogo.Get(colaborador.ClienteId);
                                        if (tipo.codigo == "TCPP")
                                        {
                                            document.ReplaceText("{c" + index + "}", colaborador.nombres + " " + colaborador.apellidos.ToUpper() + "(" + tipo.nombre + ")");
                                            index = index + 1;
                                        }
                                        else
                                        {
                                            document.ReplaceText("{s" + indexshaya + "}", colaborador.nombres + " " + colaborador.apellidos.ToUpper() + "(" + tipo.nombre + ")");
                                            indexshaya = indexshaya + 1;
                                        }
                                    }



                                }

                                while (index <= 4)
                                {
                                    document.ReplaceText("{c" + index + "}", "");
                                    index = index + 1;
                                }
                                while (indexshaya <= 4)
                                {
                                    document.ReplaceText("{s" + indexshaya + "}", "");
                                    indexshaya = indexshaya + 1;
                                }


                            }

                            //Tabla

                            var detalles_transmital = _repositorydetalle.GetAll()
                                                        .Where(c => c.vigente)
                                                        .Where(c => c.TransmitalId == Transmital.Id)
                                                        .ToList();

                            if (detalles_transmital.Count > 0)
                            {
                                Table t = document.Tables[0];
                                t.Alignment = Alignment.center;

                                int fila = 12;

                                foreach (var dt in detalles_transmital)
                                {
                                    if (fila <= 22)
                                    {
                                        t.Rows[fila].Cells[1].Paragraphs.First().Append(dt.codigo_detalle).FontSize(10D).Font(new Font("Arial"));
                                        t.Rows[fila].Cells[2].Paragraphs.First().Append(dt.descripcion).FontSize(10D).Font(new Font("Arial"));
                                        t.Rows[fila].Cells[3].Paragraphs.First().Append(Transmital.version).FontSize(10D).Font(new Font("Arial"));
                                        t.Rows[fila].Cells[4].Paragraphs.First().Append("" + dt.nro_hojas).FontSize(10D).Font(new Font("Arial"));
                                        t.Rows[fila].Cells[5].Paragraphs.First().Append("" + dt.nro_copias).FontSize(10D).Font(new Font("Arial"));
                                        t.Rows[fila].Cells[6].Paragraphs.First().Append(Transmital.tipo_formato).FontSize(10D).Font(new Font("Arial"));
                                        t.Rows[fila].Cells[7].Paragraphs.First().Append(Transmital.tipo_proposito).FontSize(10D).Font(new Font("Arial"));
                                        t.Rows[fila].Cells[8].Paragraphs.First().Append(Transmital.tipo).FontSize(10D).Font(new Font("Arial"));
                                    }
                                    else
                                    {


                                    }

                                    fila = fila + 1;
                                }



                            }
                        }


                        document.Save();

                        return salida;
                    }

                }
                else
                {
                    return "";
                }
            }
            else {
                return "";
            }


        }

        public string CrearTransmitalOfertaComercial(int id, TransmitalCabecera transmistal)
        {
            var o = _ofertacomercial.Get(id);
            var contrato = _contratoRepository.GetAll().Where(c => c.Id == o.ContratoId).FirstOrDefault();
            var siexite = Repository.GetAll()
                                   .Where(c => c.vigente)
                                   .Where(c => c.OfertaComercialId == id)
                                   .FirstOrDefault();
            if (siexite != null && siexite.Id > 0)
            {
                return "EXISTE";
            }
            string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();

            var usuario = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();

            int count = this.secuencialTransmital(contrato.ClienteId); ;
            var codigo_transmital = "3808-B-TD-" + String.Format("{0:000000}", count);
            TransmitalCabecera nuevo = new TransmitalCabecera()
            {
                Id = 0,
                ContratoId = o.ContratoId,
                EmpresaId = o.Contrato.EmpresaId,
                ClienteId = o.Contrato.ClienteId,
                OfertaComercialId = o.Id,
                fecha_emision = DateTime.Today,
                tipo =transmistal.tipo,
                tipo_formato =transmistal.tipo_formato,
                tipo_proposito =transmistal.tipo_proposito,
                descripcion = transmistal.descripcion,
                vigente = true,
                dirigido_a = transmistal.dirigido_a,
                copia_a = transmistal.copia_a,
                version = o.version,
                codigo_transmital = codigo_transmital,
                codigo_carta = "",

            };

            if (usuario != null)
            {
                nuevo.enviado_por = usuario.Nombres + " " + usuario.Apellidos.ToUpper() + " (CPP)";
            }
            else
            {
                nuevo.enviado_por = "";
            }

            var resultado = Repository.InsertAndGetId(nuevo);
            o.TransmitalId = resultado;
            _ofertacomercial.Update(o);
            return "OK";
        }

        public TransmitalCabecera IdOfertaComercialTransmital(int id)
        {
            var transmital = Repository.GetAllIncluding(c => c.OfertaComercial)
                                     .Where(c => c.OfertaComercialId == id)
                                     .Where(c => c.vigente).FirstOrDefault();

            if (transmital != null && transmital.Id > 0)
            {
                return transmital;
            }
            else
            {
                return new TransmitalCabecera();

            }

        }

        public bool CrearDetalle(TransmitalDetalle d)
        {

            d.vigente = true;
            var e = _repositorydetalle.InsertAndGetId(d);
            return e > 0 ? true : false;
        }

        public string EditDetalle(TransmitalDetalle d)
        {


            var e = _repositorydetalle.Get(d.Id);
            e.descripcion = d.descripcion;
            e.nro_hojas = d.nro_hojas;
            e.nro_copias = d.nro_copias;
            if (e.ArchivoId != d.ArchivoId)
            {
                e.ArchivoId = d.ArchivoId;
            }

            if (d.es_oferta)
            {
                var definitivas = _repositorydetalle.GetAllIncluding(c => c.Transmital)
                                          .Where(c => c.vigente)
                                          .Where(c => c.TransmitalId == d.TransmitalId)
                                          .Where(c => c.es_oferta)
                                          .Where(c => c.Id != d.Id)
                                          .ToList();
                if (definitivas.Count > 0)
                {
                    return "EXISTE_OFERTA";
                }
                else
                {
                    e.es_oferta = d.es_oferta;
                }
            }

            var u = _repositorydetalle.Update(e);
            return u != null && u.Id > 0 ? "OK" : "ERROR";
        }

        public bool DeleteDetalle(int Id)
        {
            var e = _repositorydetalle.Get(Id);
            _repositorydetalle.Delete(e);
            return true;
        }

        public int CrearArchivo(HttpPostedFileBase x)
        {
            if (x != null)
            {
                string fileName = x.FileName;
                string fileContentType = x.ContentType;
                byte[] fileBytes = new byte[x.ContentLength];
                var data = x.InputStream.Read(fileBytes, 0,
                Convert.ToInt32(x.ContentLength));

                Archivo n = new Archivo
                {
                    Id = 0,
                    codigo = "TMITTAL" + 1,
                    nombre = fileName,
                    vigente = true,
                    fecha_registro = DateTime.Now,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                };

                var archivoid = _archivoRepository.InsertAndGetId(n);
                return archivoid;
            }
            else
            {
                return 0;
            }
        }


        public bool SearchDefinitiva(TransmitalDetalle d)
        {
            var definitivas = _repositorydetalle.GetAllIncluding(c => c.Transmital)
                                                .Where(c => c.vigente)
                                                .Where(c => c.TransmitalId == d.TransmitalId)
                                                .Where(c => c.es_oferta)

                                                .ToList();
            return definitivas.Count > 0 ? true : false;

        }

        public string EditarTransmital(TransmitalCabecera transmital)
        {
            var e = Repository.Get(transmital.Id);
            e.descripcion = transmital.descripcion;
            e.dirigido_a = transmital.dirigido_a;
            e.copia_a = transmital.copia_a;
            e.dirigido_a = transmital.dirigido_a;
            e.fecha_emision = transmital.fecha_emision;
            if (transmital.OfertaComercialId.HasValue)
            {
            e.OfertaComercialId = transmital.OfertaComercialId.Value;
            }
            var u = Repository.Update(e);
            return "OK";
        }

        public string DeleteTransmital(int Id)
        {
            var detalles = _repositorydetalle.GetAll().Where(c => c.TransmitalId == Id).ToList();
            if (detalles.Count > 0) {
                _repositorydetalle.Delete(detalles);
            }

            var ofertaComercialTransmital = _ofertacomercial.GetAll().Where(c => c.TransmitalId == Id).ToList();
            foreach (var oferta in ofertaComercialTransmital)
            {
                var entityOferta = _ofertacomercial.Get(oferta.Id);
                entityOferta.TransmitalId = null;
                _ofertacomercial.Update(entityOferta);


            }
            Repository.Delete(Id);
            return "OK";
        }

        public bool tieneTransmital(int Id)
        {
            var e = Repository.GetAll().Where(c => c.OfertaComercialId.HasValue).Where(c => c.vigente).Where(c => c.OfertaComercialId == Id).FirstOrDefault();
            return e != null && e.Id > 0 ? true : false;
        }

        public string nombresTransmital(int Id)
        {
            var e = Repository.GetAll().Where(c => c.OfertaComercialId.HasValue).Where(c => c.vigente).Where(c => c.OfertaComercialId == Id).FirstOrDefault();
            return e != null && e.Id > 0 ? e.codigo_transmital : "";
        }
    }
}