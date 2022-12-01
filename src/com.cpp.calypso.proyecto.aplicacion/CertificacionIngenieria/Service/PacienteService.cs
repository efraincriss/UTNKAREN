using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{


    public class PacienteAsyncBaseCrudAppService : AsyncBaseCrudAppService<Paciente, PacienteDto, PagedAndFilteredResultRequestDto>, IPacienteAsyncBaseCrudAppService
    {

        private readonly IBaseRepository<MNA> _MnaRepo;
        private readonly IBaseRepository<Katz> _KatzRepo;
        private readonly IBaseRepository<Catalogo> _catRepo;
        public PacienteAsyncBaseCrudAppService(
            IBaseRepository<Paciente> repository,
            IBaseRepository<MNA> MnaRepo,
          IBaseRepository<Katz> KatzRepo,
          IBaseRepository<Catalogo> catRepo
        ) : base(repository)
        {
            _MnaRepo = MnaRepo;
            _KatzRepo = KatzRepo;
            _catRepo = catRepo;
        }

        public HijosPaciente detallePaciente(int Id)
        {
            var a = new HijosPaciente();
            var mna = _MnaRepo.GetAll().Where(c => c.PacienteId == Id).ToList();
            var kat = _KatzRepo.GetAll().Where(c => c.PacienteId == Id).ToList();
            a.mnas = mna;
            a.Katzs = kat;
            return a;

        }

        public bool Editar(Paciente i)
        {
            var e = Repository.Get(i.Id);
            e.Identificacion = i.Identificacion;
            e.NombresApellidos = i.NombresApellidos;
            e.Peso = i.Peso;
            e.SexoId = i.SexoId;
            e.Talla = i.Talla;
            e.Edad = i.Edad;
            return true;
        }

        public bool EditarKat(Katz i)
        {
            var e = _KatzRepo.Get(i.Id);
            e.Alimentacion = i.Alimentacion;
            e.Bano = i.Bano;
            e.Continencia = i.Continencia;
            e.Sanitario = i.Sanitario;
            e.Vestido = i.Vestido;
            e.Transferencias = i.Transferencias;
            e.Calificacion = i.Calificacion;
            e.Puntuacion = i.Puntuacion;

            return true;
        }

        public bool EditarMNA(MNA i)
        {
            var e = _MnaRepo.Get(i.Id);

            e.CircunferenciaBraquialId = i.CircunferenciaBraquialId;
            e.CircunferenciaPiernaId = i.CircunferenciaPiernaId;
            e.ComidaDiariaId = i.ComidaDiariaId;
            e.ConsideracionEnfermoId = i.ConsideracionEnfermoId;
            e.ConsumeCarne = i.ConsumeCarne;
            e.ConsumeLacteos = i.ConsumeLacteos;
            e.ConsumeLegumbres = i.ConsumeLegumbres;
            e.ConsumoFrutasVerdurasId = i.ConsumoFrutasVerdurasId;
            e.ConsumoPersonaId = i.ConsumoPersonaId;
            e.EnfermedadAgudaId = i.EnfermedadAgudaId;
            e.EstadoSaludId = i.EstadoSaludId;
            e.Fecha = i.Fecha;
            e.IndiceMasaId = i.IndiceMasaId;
            e.MedicamentoDiaId = i.MedicamentoDiaId;
            e.ModoAlimentarseId = i.ModoAlimentarseId;
            e.MovilidadId = i.MovilidadId;
            e.NumeroVasosAguaId = i.NumeroVasosAguaId;
            e.PerdidaApetitoId = i.PerdidaApetitoId;
            e.PerdidaPesoId = i.PerdidaPesoId;
            e.ProblemasNeuroId = i.ProblemasNeuroId;
            e.UlceraLesionId = i.UlceraLesionId;
            e.ViveDomicilioId = i.ViveDomicilioId;
            e.Puntuacion = i.Puntuacion;
            e.ValoracionCompleta = i.ValoracionCompleta;

            if (!i.ValoracionCompleta)
            {
                if (i.Puntuacion >= 12)
                {
                    e.DetallePuntuacion = "NORMAL";
                }
            }
            else
            {
                if (i.Puntuacion >= 24)
                {
                    e.DetallePuntuacion = "ESTADO NUTRICIONAL SATISFACTORIO";
                }
                if (i.Puntuacion > 16 && i.Puntuacion <= Convert.ToDecimal(23.5))
                {
                    e.DetallePuntuacion = "RIESGO DE MALNUTRICIÓN";
                }
                if (i.Puntuacion < 17)
                {
                    e.DetallePuntuacion = "MAL ESTADO NUTRICIONAL";
                }
            }

            return true;
        }

        public bool eliminarKat(int id)
        {
            var e = _KatzRepo.Get(id);
            _KatzRepo.Delete(e);
            return true;
        }

        public bool eliminarMNA(int id)
        {
            var e = _MnaRepo.Get(id);
            _MnaRepo.Delete(e);
            return true;
        }

        public bool eliminarPaciente(int id)
        {
            var e = Repository.Get(id);
            Repository.Delete(e);
            return true;
        }

        public bool insertarEntidad(Paciente entity)
        {
            Repository.Insert(entity);
            return true;
        }

        public bool insertarKat(Katz entity)
        {
            _KatzRepo.Insert(entity);
            return true;
        }

        public bool insertarMNA(MNA entity)
        {
            if (!entity.ValoracionCompleta)
            {
                if (entity.Puntuacion >= 12)
                {
                    entity.DetallePuntuacion = "NORMAL";
                }
            }
            else
            {
                if (entity.Puntuacion >= 24)
                {
                    entity.DetallePuntuacion = "ESTADO NUTRICIONAL SATISFACTORIO";
                }
                if (entity.Puntuacion > 16 && entity.Puntuacion <= Convert.ToDecimal(23.5))
                {
                    entity.DetallePuntuacion = "RIESGO DE MALNUTRICIÓN";
                }
                if (entity.Puntuacion < 17)
                {
                    entity.DetallePuntuacion = "MAL ESTADO NUTRICIONAL";
                }
            }
            _MnaRepo.Insert(entity);
            return true;
        }

        public decimal ObtenerTotales(int PerdidaApetitoId, int PerdidaPesoId, int MovilidadId, int EnfermedadAgudaId, int ProblemasNeuroId, int IndiceMasaId, int ViveDomicilioId, int MedicamentoDiaId, int UlceraLesionId, int ComidaDiariaId, int ConsumoPersonaId, int ConsumoFrutasVerdurasId, int NumeroVasosAguaId, int ModoAlimentarseId, int ConsideracionEnfermoId, int EstadoSaludId, int CircunferenciaBraquialId, int CircunferenciaPiernaId)
        {
            decimal valor = 0;
            if (PerdidaApetitoId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == PerdidaApetitoId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (PerdidaPesoId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == PerdidaPesoId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (MovilidadId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == MovilidadId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (EnfermedadAgudaId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == EnfermedadAgudaId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (ProblemasNeuroId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == ProblemasNeuroId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (IndiceMasaId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == IndiceMasaId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            return valor;
        }

        public decimal ObtenerTotales2(int PerdidaApetitoId, int PerdidaPesoId, int MovilidadId, int EnfermedadAgudaId, int ProblemasNeuroId,
            int IndiceMasaId, int ViveDomicilioId, int MedicamentoDiaId, int UlceraLesionId, int ComidaDiariaId,
            int ConsumoPersonaId, int ConsumoFrutasVerdurasId, int NumeroVasosAguaId, int ModoAlimentarseId, int ConsideracionEnfermoId,
            int EstadoSaludId, int CircunferenciaBraquialId, int CircunferenciaPiernaId)
        {
            decimal valor = 0;
            #region 1
            if (PerdidaApetitoId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == PerdidaApetitoId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (PerdidaPesoId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == PerdidaPesoId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (MovilidadId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == MovilidadId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (EnfermedadAgudaId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == EnfermedadAgudaId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (ProblemasNeuroId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == ProblemasNeuroId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (IndiceMasaId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == IndiceMasaId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }


            if (ViveDomicilioId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == ViveDomicilioId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (MedicamentoDiaId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == MedicamentoDiaId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (UlceraLesionId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == UlceraLesionId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }

            }
            if (ComidaDiariaId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == ComidaDiariaId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            #endregion


            if (ConsumoPersonaId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == ConsumoPersonaId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }

            if (ConsumoFrutasVerdurasId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == ConsumoFrutasVerdurasId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (NumeroVasosAguaId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == NumeroVasosAguaId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (ModoAlimentarseId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == ModoAlimentarseId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (ConsideracionEnfermoId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == ConsideracionEnfermoId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }



            if (EstadoSaludId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == EstadoSaludId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }

            if (CircunferenciaBraquialId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == CircunferenciaBraquialId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            if (CircunferenciaPiernaId > 0)
            {
                var cat = _catRepo.GetAll().Where(x => x.Id == CircunferenciaPiernaId).FirstOrDefault();
                if (cat != null)
                {
                    valor = valor + cat.valor_numerico.Value;
                }
            }
            return valor;
        }

        public List<PacienteDto> pacientes()
        {
            var query = Repository.GetAllIncluding(x => x.Sexo).ToList();
            var dto = (from e in query
                       select new PacienteDto()
                       {
                           Identificacion = e.Identificacion,
                           Id = e.Id,
                           NombresApellidos = e.NombresApellidos,
                           Edad = e.Edad,
                           Peso = e.Peso,
                           SexoId = e.SexoId,
                           Talla = e.Talla,
                           sexoString = e.Sexo.nombre

                       }).ToList();

            return dto;
        }

        public ExcelPackage Reporte()
        {
            ExcelPackage package = new ExcelPackage();
            var workbook = package.Workbook;
            var worksheet1 = workbook.Worksheets.Add("Pacientes");
            var worksheet2 = workbook.Worksheets.Add("MNA");
            var worksheet3 = workbook.Worksheets.Add("KATZ");
            ExcelWorksheet h1 = package.Workbook.Worksheets[1];
            ExcelWorksheet h = package.Workbook.Worksheets[2];
            ExcelWorksheet h3 = package.Workbook.Worksheets[3];


            int count = 1;
            string cell = "A" + count;
            h.Cells[cell].Value = "IDENTIFICACIÓN";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "B" + count;
            h.Cells[cell].Value = "NOMBRES Y APELLIDOS";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "C" + count;
            h.Cells[cell].Value = "EDAD";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "D" + count;
            h.Cells[cell].Value = "TALLA";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "E" + count;
            h.Cells[cell].Value = "PESO";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "F" + count;
            h.Cells[cell].Value = "SEXO";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "G" + count;
            h.Cells[cell].Value = "FECHA";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "H" + count;
            h.Cells[cell].Value = "1. Ha perdido el apetito? Ha comido menos por tener hambre, problemas digestivos, dificultad para masticar o alimentarse en lo últimos tres meses.";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "I" + count;
            h.Cells[cell].Value = "2. Pérdida reciente de peso (< 3 meses)";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "J" + count;
            h.Cells[cell].Value = "3. Movilidad";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);


            cell = "K" + count;
            h.Cells[cell].Value = "4. Ha habido enfermedad aguda o situación de estrés psicológico en los últimos tres meses ";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "L" + count;
            h.Cells[cell].Value = "5. Problemas Neuropsicológicos";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "M" + count;
            h.Cells[cell].Value = "6.Índice de masa corporal";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "N" + count;
            h.Cells[cell].Value = "7. La persona vive en su domicilio";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "O" + count;
            h.Cells[cell].Value = "8. Toma más de 3 medicamentos al día ";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "P" + count;
            h.Cells[cell].Value = "9. Úlceras o lesiones cutáneas";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "Q" + count;
            h.Cells[cell].Value = "10. Cuantas comidas hace al día?";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "R" + count;
            h.Cells[cell].Value = "11. La persona consume?";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "S" + count;
            h.Cells[cell].Value = "12.Consume frutas o verduras por lo menos dos veces al día?";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "T" + count;
            h.Cells[cell].Value = "13. Cuantos vasos de agua o otros líquidos toma al día?";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "U" + count;
            h.Cells[cell].Value = "14.Consume frutas o verduras por lo menos dos veces al día?";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "V" + count;
            h.Cells[cell].Value = "15. El enfermo se considera, a él mismo bien nutrido (problemas nutricionales) ?";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "W" + count;
            h.Cells[cell].Value = "16. Comparándose con las personas de su 16.edad. Como esta su estado de salud?";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "X" + count;
            h.Cells[cell].Value = "17. Circunferencia braquial (CB en cm.)";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "Y" + count;
            h.Cells[cell].Value = "18. Circunferencia de la pierna (CC en cm.)";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "Z" + count;
            h.Cells[cell].Value = "PUNTUACIÓN";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);


            cell = "AA" + count;
            h.Cells[cell].Value = "ESTADO";
            h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            



            count++;

            var query = _MnaRepo.GetAllIncluding(x => x.Paciente.Sexo,
                 c => c.PerdidaApetito,
                  c => c.PerdidaPeso,
                   c => c.Movilidad,
                    c => c.EnfermedadAguda,
                     c => c.ProblemasNeuro,
                      c => c.IndiceMasa,
                       c => c.ViveDomicilio,
                        c => c.MedicamentoDia,
                         c => c.UlceraLesion,
                          c => c.ComidaDiaria,
                           c => c.ConsumoPersona,
                            c => c.ConsumoFrutasVerduras,
                             c => c.NumeroVasosAgua,
                              c => c.ModoAlimentarse,
                               c => c.ConsideracionEnfermo,
                                c => c.EstadoSalud,
                                 c => c.CircunferenciaBraquial,
                                  c => c.CircunferenciaPierna
                                                   ).ToList();

            foreach (var ra in query)
            {
                cell = "A" + count;
                h.Cells[cell].Value = ra.Paciente.Identificacion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);



                cell = "B" + count;
                h.Cells[cell].Value = ra.Paciente.NombresApellidos;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                cell = "C" + count;
                h.Cells[cell].Value = ra.Paciente.Edad;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                cell = "D" + count;
                h.Cells[cell].Value = ra.Paciente.Talla;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                cell = "E" + count;
                h.Cells[cell].Value = ra.Paciente.Peso;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "F" + count;
                h.Cells[cell].Value = ra.Paciente.Sexo.nombre;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "G" + count;
                h.Cells[cell].Value = ra.Fecha.ToShortDateString();
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "H" + count;
                h.Cells[cell].Value = ra.PerdidaApetito.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);


                cell = "I" + count;
                h.Cells[cell].Value =ra.PerdidaPeso.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                cell = "J" + count;
                h.Cells[cell].Value = ra.Movilidad.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "K" + count;
                h.Cells[cell].Value = ra.EnfermedadAguda.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "L" + count;
                h.Cells[cell].Value = ra.ProblemasNeuro.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);



                cell = "M" + count;
                h.Cells[cell].Value = ra.IndiceMasa.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "N" + count;
                h.Cells[cell].Value = ra.ViveDomicilio.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "O" + count;
                h.Cells[cell].Value = ra.MedicamentoDia.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "P" + count;
                h.Cells[cell].Value = ra.UlceraLesion.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "Q" + count;
                h.Cells[cell].Value = ra.ComidaDiaria.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "R" + count;
                h.Cells[cell].Value = ra.ConsumoPersona.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "S" + count;
                h.Cells[cell].Value = ra.ConsumoFrutasVerduras.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "T" + count;
                h.Cells[cell].Value = ra.NumeroVasosAgua.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "U" + count;
                h.Cells[cell].Value = ra.ModoAlimentarse.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "V" + count;
                h.Cells[cell].Value = ra.ConsideracionEnfermo.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                cell = "W" + count;
                h.Cells[cell].Value = ra.EstadoSalud.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "X" + count;
                h.Cells[cell].Value = ra.CircunferenciaBraquial.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "Y" + count;
                h.Cells[cell].Value = ra.CircunferenciaPierna.descripcion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "Z" + count;
                h.Cells[cell].Value = ra.Puntuacion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                cell = "AA" + count;
                h.Cells[cell].Value = ra.DetallePuntuacion;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);




                count++;




            }
            h.Cells[1, 1, h.Dimension.End.Row, h.Dimension.End.Column].AutoFilter = true;


            //pacientes
            count = 1;
            cell = "A" + count;
            h1.Cells[cell].Value = "IDENTIFICACIÓN";
            h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h1.Cells[cell].Style.Font.Bold = true;
            h1.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h1.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h1.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "B" + count;
            h1.Cells[cell].Value = "NOMBRES Y APELLIDOS";
            h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h1.Cells[cell].Style.Font.Bold = true;
            h1.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h1.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h1.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "C" + count;
            h1.Cells[cell].Value = "EDAD";
            h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h1.Cells[cell].Style.Font.Bold = true;
            h1.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h1.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h1.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "D" + count;
            h1.Cells[cell].Value = "SEXO";
            h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h1.Cells[cell].Style.Font.Bold = true;
            h1.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h1.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h1.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "E" + count;
            h1.Cells[cell].Value = "TALLA";
            h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h1.Cells[cell].Style.Font.Bold = true;
            h1.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h1.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h1.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "F" + count;
            h1.Cells[cell].Value = "PESO";
            h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h1.Cells[cell].Style.Font.Bold = true;
            h1.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h1.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h1.Cells[cell].Style.Font.Color.SetColor(Color.White);



            int p = 2;
            var pacientes = Repository.GetAllIncluding(c => c.Sexo).ToList();
            foreach (var pa in pacientes)
            {
                cell = "A" + p;
                h1.Cells[cell].Value = pa.Identificacion;
                h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "B" + p;
                h1.Cells[cell].Value = pa.NombresApellidos;
                h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);


                cell = "C" + p;
                h1.Cells[cell].Value = pa.Edad;
                h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "D" + p;
                h1.Cells[cell].Value = pa.Sexo.nombre;
                h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                cell = "E" + p;
                h1.Cells[cell].Value = pa.Talla;
                h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                cell = "F" + p;
                h1.Cells[cell].Value = pa.Peso;
                h1.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                p++;
            }

            var kartz = _KatzRepo.GetAllIncluding(c => c.Paciente).ToList();

            count = 1;
            cell = "A" + count;
            h3.Cells[cell].Value = "IDENTIFICACIÓN";
            h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h3.Cells[cell].Style.Font.Bold = true;
            h3.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h3.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h3.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "B" + count;
            h3.Cells[cell].Value = "NOMRBES Y APELLIDOS";
            h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h3.Cells[cell].Style.Font.Bold = true;
            h3.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h3.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h3.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "C" + count;
            h3.Cells[cell].Value = "BAÑO";
            h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h3.Cells[cell].Style.Font.Bold = true;
            h3.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h3.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h3.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "D" + count;
            h3.Cells[cell].Value = "VESTIDO";
            h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h3.Cells[cell].Style.Font.Bold = true;
            h3.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h3.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h3.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "E" + count;
            h3.Cells[cell].Value = "USO SANITARIO";
            h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h3.Cells[cell].Style.Font.Bold = true;
            h3.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h3.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h3.Cells[cell].Style.Font.Color.SetColor(Color.White);

            cell = "F" + count;
            h3.Cells[cell].Value = "TRANSFERENCIA";
            h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h3.Cells[cell].Style.Font.Bold = true;
            h3.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h3.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h3.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "G" + count;
            h3.Cells[cell].Value = "CONTINENCIA";
            h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h3.Cells[cell].Style.Font.Bold = true;
            h3.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h3.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h3.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "H" + count;
            h3.Cells[cell].Value = "ALIMENTACIÓN";
            h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h3.Cells[cell].Style.Font.Bold = true;
            h3.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h3.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h3.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "I" + count;
            h3.Cells[cell].Value = "PUNTUACIÓN";
            h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h3.Cells[cell].Style.Font.Bold = true;
            h3.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h3.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h3.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "J" + count;
            h3.Cells[cell].Value = "CALIFICACIÓN";
            h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            h3.Cells[cell].Style.Font.Bold = true;
            h3.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            h3.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
            h3.Cells[cell].Style.Font.Color.SetColor(Color.White);

            p = 2;
            foreach (var pa in kartz)
            {


                cell = "A" + p;
                h3.Cells[cell].Value = pa.Paciente.Identificacion;
                h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);
                cell = "B" + p;
                h3.Cells[cell].Value = pa.Paciente.NombresApellidos;
                h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "C" + p;
                h3.Cells[cell].Value = pa.Bano?"SI":"NO";
                h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "D" + p;
                h3.Cells[cell].Value = pa.Vestido ? "SI" : "NO";
                h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "E" + p;
                h3.Cells[cell].Value = pa.Sanitario ? "SI" : "NO";
                h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "F" + p;
                h3.Cells[cell].Value = pa.Transferencias ? "SI" : "NO";
                h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "G" + p;
                h3.Cells[cell].Value = pa.Continencia ? "SI" : "NO";
                h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "H" + p;
                h3.Cells[cell].Value = pa.Alimentacion ? "SI" : "NO";
                h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "I" + p;
                h3.Cells[cell].Value = pa.Puntuacion;
                h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);

                cell = "J" + p;
                h3.Cells[cell].Value = pa.Calificacion;
                h3.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Dotted);





                p++;
            }




            return package;
        }

    }
}



