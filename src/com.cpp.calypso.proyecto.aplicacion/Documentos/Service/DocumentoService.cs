using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Interface;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Models;
using com.cpp.calypso.proyecto.dominio.Documentos;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Service
{
    public class DocumentoAsyncBaseCrudAppService : AsyncBaseCrudAppService<Documento, DocumentoDto, PagedAndFilteredResultRequestDto>, IDocumentoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Seccion> _seccionRepository;
        private readonly IBaseRepository<ImagenSeccion> _imagenseccionRepository;
        private readonly IBaseRepository<Usuario> _usuarioRepository;
        private readonly IBaseRepository<UsuarioAutorizado> _usuarioAUTRepository;
        public DocumentoAsyncBaseCrudAppService(
            IBaseRepository<Documento> repository,
            IBaseRepository<Seccion> seccionRepository,
             IBaseRepository<ImagenSeccion> imagenseccionRepository,
             IBaseRepository<Usuario> usuarioRepository,
             IBaseRepository<UsuarioAutorizado> usuarioAUTRepository
            ) : base(repository)
        {
            _seccionRepository = seccionRepository;
            _imagenseccionRepository=imagenseccionRepository;
            _usuarioRepository = usuarioRepository;
            _usuarioAUTRepository = usuarioAUTRepository;
    }


        public List<DocumentoDto> ObtenerDocumentosDeCarpeta(int carpetaId)
        {
            var documentos = Repository.GetAll()
                .Include(o => o.TipoDocumento)
                .Include(o => o.DocumentoPadre)
                .Where(o => o.CarpetaId == carpetaId)
                .ToList();

            var dtos = Mapper.Map<List<DocumentoDto>>(documentos);
            return dtos;
        }

        public async Task<bool> CrearDocumentoAsync(DocumentoDto dto)
        {
            var entity = Mapper.Map<Documento>(dto);
            await Repository.InsertAsync(entity);
            return true;
        }

        public bool ActualizarDocumento(DocumentoDto dto)
        {
            var entity = Mapper.Map<Documento>(dto);
            Repository.Update(entity);
            return true;
        }

        public ResultadoEliminacionResponse EliminarDocumento(int id)
        {
            var documento = Repository
                .Get(id);

            var secciones = _seccionRepository.GetAll().Where(c => c.DocumentoId == documento.Id).ToList();

            foreach (var seccion in secciones)
            {
                _seccionRepository.Delete(seccion);
            }

            
            Repository.Delete(id);
            return new ResultadoEliminacionResponse
            {
                Eliminado = true,
                Error = ""
            };

        }

        public DocumentoDto ObtenerDocumento(int documentoId)
        {
            var entity = Repository.GetAll()
                .Include(o => o.Carpeta)
                .Include(o => o.TipoDocumento)
                .Include(o => o.DocumentoPadre)
                .First(o => o.Id == documentoId);
            return Mapper.Map<DocumentoDto>(entity);

        }

        public List<DocumentoDto> ObtenerDocumentosTipoAnexoPorCarpeta(int carpetaId)
        {
            var list = Repository.GetAll()
                .Where(o => o.CarpetaId == carpetaId)
                .Where(o => o.TipoDocumento.codigo == "ANEXO")
                .ToList();

            return Mapper.Map<List<DocumentoDto>>(list);
        }

        public List<Documento> ObtenerDocumentosporTipo(string TipoDocumento, int carpetaId)
        {
            var list = Repository.GetAllIncluding(c=>c.TipoDocumento);
            if (carpetaId > 0)
            {
                list = list.Where(o => o.CarpetaId == carpetaId);

            }
            if (TipoDocumento.Length > 0)
            {
                list=list.Where(o => o.TipoDocumento.codigo == TipoDocumento);
            }      
           
            var data =list.ToList();
            return data;
        }

        public List<Documento> ObtenerDocumentosporCarpeta(int carpetaId)
        {
            var list = Repository.GetAllIncluding(c => c.TipoDocumento);
            if (carpetaId > 0)
            {
                list = list.Where(o => o.CarpetaId == carpetaId);

            }
            var data = list.ToList();
            return data;
        }




        public List<Seccion> ObtenerSeccionporDocumento(int DocumentoId)
        {
            var secciones = _seccionRepository.GetAll();
            if (DocumentoId > 0)
            {
                secciones = secciones.Where(c => c.DocumentoId == DocumentoId);

            }
            secciones = secciones.OrderBy(c => c.Codigo);
            return secciones.ToList();
        }
        public List<Seccion> ObtenerSeccionporCarpeta(int carpetaId)
        {
            var secciones = _seccionRepository.GetAll().Include(o => o.Documento);
            if (carpetaId > 0)
            {
                secciones = secciones.Where(c => c.Documento.CarpetaId == carpetaId);

            }
            secciones = secciones.OrderBy(c => c.Codigo);
            return secciones.ToList();
        }


        public ExcelPackage ListadoDocumentos()
        {

            ExcelPackage excel = new ExcelPackage();
            var h = excel.Workbook.Worksheets.Add("ReporteDocumentos");


            string cell = "";
            var count = 2;

            var datos = new List<ReporteDocumentos>();

            var query = Repository.GetAllIncluding(c => c.Carpeta.Estado, c => c.TipoDocumento).ToList() ;

            foreach (var i in query)
            {

                var secciones = _seccionRepository.GetAll().Where(c => c.DocumentoId == i.Id).ToList().Count();
                var imagenes = _imagenseccionRepository.GetAll().Where(c => c.Seccion.DocumentoId == i.Id).ToList().Count();

                var data = new ReporteDocumentos() {
                    Contrato = i.Carpeta.NombreCorto,
                    CodigoDocumento = i.Codigo,
                    NombreDocumento = i.Nombre,
                    TipoDocumento = i.TipoDocumento.nombre,
                    CantidadPag = "" + i.CantidadPaginas,
                    CantidadSecciones = "" + secciones,
                    TieneImagen=imagenes>0?"SI":"NO",
                    EstadoCarpeta=i.Carpeta.Estado.nombre,
                    Nombrecarpeta=i.Carpeta.NombreCompleto,
                    FechaCreación= i.CreationTime.ToString("dd/MM/yyyy"),
                };

                if (i.CreatorUserId.HasValue) { 
                var usuario = _usuarioRepository.Get(Convert.ToInt32(i.CreatorUserId.Value));
                
                data.UsuarioCreador = usuario!=null?usuario.Nombres + " "+usuario.Apellidos :"";
                }

                datos.Add(data);
            }
      
            count = 1;
            h.Row(1).Height = 80;
            cell = "B" + count+":L"+count;
            h.Cells[cell].Merge =true;
            h.Cells[cell].Value = "Reporte Documentos";
            h.Cells[cell].Style.Font.Italic = true;
            h.Cells[cell].Style.Font.Size = 18;
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
             h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            count = 2;
            h.Column(2).Width = 22;
            h.Column(3).Width = 22;
            h.Column(4).Width = 22;
            h.Column(5).Width = 22;
            h.Column(6).Width = 22;
            h.Column(7).Width = 22;
            h.Column(8).Width = 22;
            h.Column(9).Width = 22;
            h.Column(10).Width = 22;
            h.Column(11).Width = 22;
            h.Column(12).Width = 22;


            cell = "B" + count;
            h.Cells[cell].Value = "CONTRATO";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "C" + count;
            h.Cells[cell].Value = "CODIGO DOCUMENTO";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "D" + count;
            h.Cells[cell].Value = "NOMBRE DOCUMENTO";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "E" + count;
            h.Cells[cell].Value = "TIPO DOCUMENTO";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "F" + count;
            h.Cells[cell].Value = "CANT PAGINA";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "G" + count;
            h.Cells[cell].Value = "CANTIDAD SECCIONES";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "H" + count;
            h.Cells[cell].Value = "TIENE IMAGEN";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "I" + count;
            h.Cells[cell].Value = "ESTADO CONTRATO";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "J" + count;

            h.Cells[cell].Value = "NOMBRE CONTRATO";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "K" + count;
            h.Cells[cell].Value = "FECHA CREACION";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "L" + count;
            h.Cells[cell].Value = "USUARIO";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            count = 3;

            foreach (var c in datos)
            {
                cell = "B" + count;
                h.Cells[cell].Value = c.Contrato;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "C" + count;
                h.Cells[cell].Value = c.CodigoDocumento;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "D" + count;
                h.Cells[cell].Value = c.NombreDocumento;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "E" + count;
                h.Cells[cell].Value = c.TipoDocumento;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "F" + count;
                h.Cells[cell].Value = c.CantidadPag;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                cell = "G" + count;
                h.Cells[cell].Value = c.CantidadSecciones;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                cell = "H" + count;
                h.Cells[cell].Value = c.TieneImagen;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                cell = "I" + count;
                h.Cells[cell].Value = c.EstadoCarpeta;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                h.Cells[cell].Style.WrapText = true;
                h.Cells[cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                cell = "J" + count;
                h.Cells[cell].Value = c.Nombrecarpeta;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "K" + count;
                h.Cells[cell].Value = c.FechaCreación;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "L" + count;
                h.Cells[cell].Value = c.UsuarioCreador;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                count++;



            }
            h.View.ZoomScale = 60;
            h.View.PageBreakView = true;
            h.PrinterSettings.PrintArea = h.Cells[1,2, h.Dimension.End.Row, h.Dimension.End.Column];
            h.PrinterSettings.FitToPage = true;

            return excel;
        }

        public ExcelPackage ListadoUsuariosAutorizados()
        {
            ExcelPackage excel = new ExcelPackage();
            var h = excel.Workbook.Worksheets.Add("ReporteDocumentos");
            var datos = new List<ReporteUsuariosAutorizados>();
            var query = _usuarioAUTRepository.GetAllIncluding(c => c.Carpeta, c => c.Usuario).ToList();

            string cell = "";

           var count = 1;
            h.Row(1).Height = 80;
            cell = "B" + count + ":D" + count;
            h.Cells[cell].Merge = true;
            h.Cells[cell].Value = "Reporte Usuario Autorizados";
            h.Cells[cell].Style.Font.Italic = true;
            h.Cells[cell].Style.Font.Size = 18;
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
             count = 2;

            cell = "B" + count;
            h.Cells[cell].Value = "CODIGO CONTRATO";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "C" + count;
            h.Cells[cell].Value = "NOMBRE";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "D" + count;
            h.Cells[cell].Value = "USUARIO";
            h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            count = 3;
            foreach (var i in query)
            {
                var data = new ReporteUsuariosAutorizados()
                {
                    CodigoContrato = i.Carpeta.Codigo,
                    NombreDocumento = i.Carpeta.NombreCompleto,
                    Usuario = i.Usuario.Nombres + " " + i.Usuario.Apellidos
                };
                datos.Add(data);
            }

            foreach (var c in datos)
            {
                cell = "B" + count;
                h.Cells[cell].Value = c.CodigoContrato;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "C" + count;
                h.Cells[cell].Value = c.NombreDocumento;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                cell = "D" + count;
                h.Cells[cell].Value = c.Usuario;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
             
                count++;


            }
            h.View.ZoomScale = 60;
            h.View.PageBreakView = true;
            h.PrinterSettings.PrintArea = h.Cells[1, 2, h.Dimension.End.Row, h.Dimension.End.Column];
            h.PrinterSettings.FitToPage = true;
            return excel;
        }
    }
}
