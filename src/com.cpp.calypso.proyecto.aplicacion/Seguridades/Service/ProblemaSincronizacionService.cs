using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Security;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Seguridades.Dto;
using com.cpp.calypso.proyecto.aplicacion.Seguridades.Interface;
using com.cpp.calypso.proyecto.dominio.Seguridades;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion.Seguridades.Service
{
    public class ProblemaSincronizacionAsyncBaseCrudAppService : AsyncBaseCrudAppService<ProblemaSincronizacion, ProblemaSincronizacionDto, PagedAndFilteredResultRequestDto>, IProblemaSincronizacionAsyncBaseCrudAppService
    {
        public ProblemaSincronizacionAsyncBaseCrudAppService(
            IBaseRepository<ProblemaSincronizacion> repository
            ) : base(repository)
        {
        }

        public List<ProblemaSincronizacionDto> ObtenerProblemas(DateTime? fechaInicio, DateTime? fechaFin, bool solucionado)
        {
            var query = Repository.GetAll().Where(o => o.Solucionado == solucionado);

            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                var startDate = fechaInicio.Value.Date;
                var endDate = fechaFin.Value.Date.AddTicks(-1).AddDays(1);
                query = query.Where(o => o.Fecha >= startDate && o.Fecha <= endDate);
            } else if (fechaInicio.HasValue)
            {
                var startDate = fechaInicio.Value.Date;
                
                query = query.Where(o => o.Fecha >= startDate);
            }else if (fechaFin.HasValue)
            {
                var endDate = fechaFin.Value.Date.AddTicks(-1).AddDays(1);
                query = query.Where(o => o.Fecha <= endDate);
            }

            var list = query.ToList();

            var dtos = Mapper.Map<List<ProblemaSincronizacionDto>>(list);

            return dtos;

        }

        public void SolucionarProblema(int problemaSincronizacionId, string observacion)
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var problema = Repository.Get(problemaSincronizacionId);

            problema.Solucionado = true;
            problema.Observaciones = observacion;
            problema.FechaSolucion = DateTime.Now;
            problema.UsuarioId = userId.HasValue ? (int)userId.Value : 0;
        }

        public void MarcarNoSolucionado(int problemaSincronizacionId)
        {
            var problema = Repository.Get(problemaSincronizacionId);

            problema.Solucionado = false;
        }


        public bool SolucionarMultiple(List<int> ids, string observacion)
        {
            foreach(int id in ids)
            {
                SolucionarProblema(id, observacion);
            }

            return true;
        }

        public ExcelPackage DescargarListadoErroresDeSincronizacion(List<int> ids)
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/Seguridades/PlantillaReporteErroresSincronizacion.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Carga de Jornales", pck.Workbook.Worksheets[1]);
            }

            ExcelWorksheet errores = excel.Workbook.Worksheets[1];

            int countFilas = 2;
            int secuencial = 1;
            
            foreach (var id in ids)
            {
                var error = Repository.Get(id);
                
                errores.Cells["A" + countFilas].Value = secuencial;
                errores.Cells["B" + countFilas].Value = error.Fecha;
                errores.Cells["C" + countFilas].Value = error.Fuente;
                errores.Cells["D" + countFilas].Value = error.Entidad;
                errores.Cells["E" + countFilas].Value = error.Problema;
                errores.Cells["F" + countFilas].Value = error.FechaSolucion.HasValue ?  error.FechaSolucion.GetValueOrDefault().ToShortDateString() : "";
                errores.Cells["G" + countFilas].Value = error.UsuarioId;
                errores.Cells["H" + countFilas].Value = error.Observaciones;
                errores.Cells["I" + countFilas].Value = error.Uid;
                errores.Cells["J" + countFilas].Value = error.Solucionado ? "SI" : "NO";
                countFilas++;
                secuencial++;
            }

            return excel;
        }
    }
}
