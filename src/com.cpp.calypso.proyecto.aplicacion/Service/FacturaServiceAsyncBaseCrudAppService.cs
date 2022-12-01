using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class FacturaServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<Factura, FacturaDto, PagedAndFilteredResultRequestDto>, IFacturaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<RetencionFactura> _retencionfacturarepository;
        private readonly IBaseRepository<CobroFactura> _cobrofacturarepository;
        private readonly IBaseRepository<Cobro> _cobrorepository;
        private readonly IBaseRepository<Cliente> _clienterepository;
        private readonly IBaseRepository<Empresa> _empresarepository;

        public FacturaServiceAsyncBaseCrudAppService(IBaseRepository<Factura> repository,


         IBaseRepository<Cobro> cobrorepository,
         IBaseRepository<CobroFactura> cobrofacturarepository,
        IBaseRepository<RetencionFactura> retencionfacturarepository,

        IBaseRepository<Cliente> clienterepository,
        IBaseRepository<Empresa> empresarepository
        ) : base(repository)
        {
            _cobrorepository = cobrorepository;
            _cobrofacturarepository = cobrofacturarepository;
            _retencionfacturarepository = retencionfacturarepository;

            _clienterepository = clienterepository;
            _empresarepository = empresarepository;
        }

        // Carga de Facturas Nuevo //

        public Facturas BuscarFacturasDB(List<FacturaExcel> Lista)
        {
            var Validas = (from e in Lista where e.clase_documento == "DB" select e).ToList();
            var NoValidas = (from f in Lista where f.clase_documento != "DB" select f).ToList();
            Facturas fa = new Facturas
            {
                Validas = Validas,
                NoValidas = NoValidas
            };
            return fa;
        }


        public List<FacturaExcel> CrearFacturas(Facturas Lista)
        {

            List<FacturaExcel> creadas = new List<FacturaExcel>();
            List<FacturaExcel> nocreadas = new List<FacturaExcel>();

            var factseparadas = this.RepartirFacturasCuenta(Lista);

            foreach (var f in factseparadas.Facturas1113091010)
            {
                bool comprobar = this.comprobarfactura(f.numero_documento, f.fecha_documento);
                if (comprobar)
                {
                    nocreadas.Add(f);
                }

                else
                {

                    int existecliente = comprobarcliente(f.cliente); //comprobar si el cliente existe;
                    int existeempresa = comprobarempresa(f.sociedad); //comprobar si la empresa existe;

                    if (existecliente > 0 && existeempresa > 0)
                    {
                        decimal valoriva = 0;
                        var iva2114210001 = (from x in factseparadas.Facturas2114210001 where x.referencia == f.referencia select x).FirstOrDefault();
                        if (iva2114210001 != null)
                        {
                            valoriva = iva2114210001.importe_moneda * (-1);
                        }

                        decimal valorimporte = 0;
                        var importe4112041000 = (from x in factseparadas.Facturas4112041000 where x.referencia == f.referencia select x).FirstOrDefault();
                        if (importe4112041000 != null)
                        {
                            valorimporte = importe4112041000.importe_moneda * (-1);
                        }

                        var cliente = _clienterepository.Get(existecliente);
                        Factura fa = new Factura
                        {
                            EmpresaId = existeempresa,
                            ClienteId = existecliente,
                            tipo_documento = "Factura",
                            numero_documento = f.referencia,
                            fecha_emision = f.fecha_documento,
                            fecha_vencimiento = f.fecha_documento.Add(new TimeSpan(cliente.dias_plazo)),
                            descripcion = f.detalle,
                            valor_importe = valorimporte,
                            valor_iva = valoriva,
                            valor_total = f.importe_moneda,
                            valor_a_cobrar = 0,
                            valor_cobrado = 0,

                            //CÁLCULO DE RETENCIONES
                            retencion_ir = 0,
                            retencion_iva = 0,
                            numero_retencion = "",


                            // DOCUMENTO DE COBRO
                            doc_compensacion = f.documento_compensacion,
                            documento_pago = f.documento_compensacion,
                            codigosap = f.numero_documento,
                            estado = 4054,
                            vigente = true,
                        };

                        var nfactura = Repository.InsertAndGetId(fa);
                        //RETENCIONES

                        var Factura = Repository.Get(nfactura);
                        if (Factura != null && Factura.Id > 0)
                        {
                            var retenciones = this.BuscarRetencionesAB(Lista.NoValidas, Factura.numero_documento);
                            Factura.retencion_iva = retenciones.iva1114020012 * (-1);
                            Factura.retencion_ir = retenciones.renta1114020013 * (-1);
                            Factura.valor_a_cobrar = Factura.valor_total - (retenciones.iva1114020012 * (-1)) - (retenciones.renta1114020013 * (-1));
                            Factura.numero_retencion = retenciones.numeroretencion;
                            var resultado = Repository.Update(Factura);

                        }

                    }
                    else
                    {
                        nocreadas.Add(f);
                    }
                }




            }

            return Lista.Validas;
        }


        public bool comprobarfactura(string numerofactura, DateTime fecha_documento)
        {
            var factura = Repository.GetAll().Where(c => c.vigente == true).
                Where(e => e.numero_documento == numerofactura).Where(e => e.fecha_emision == fecha_documento).FirstOrDefault();

            if (factura != null && factura.Id > 0)
            {
                return true;
            }

            return false;

        }

        //




        public int comprobarcliente(string codigosapcliente)
        {
            var cliente = _clienterepository.GetAll().Where(c => c.vigente == true).
              Where(e => e.codigo_sap == codigosapcliente).FirstOrDefault();

            if (cliente != null && cliente.Id > 0)
            {
                return cliente.Id;
            }

            return 0;
        }

        public int comprobarempresa(string codigosapempresa)
        {
            var empresa = _empresarepository.GetAll().Where(c => c.vigente == true).
                 Where(e => e.codigo_sap == codigosapempresa).FirstOrDefault();

            if (empresa != null && empresa.Id > 0)
            {
                return empresa.Id;
            }

            return 0;
        }



        public List<FacturaExcel> FiltrarExcel(List<FacturaExcel> Lista)
        {
            string numerofactura = "";
            List<FacturaExcel> filtrado = new List<FacturaExcel>();
            foreach (var aitem in Lista)
            {
                bool siesta = filtrado.Any(z => z.numero_documento == aitem.numero_documento);
                if (!siesta)
                {
                    filtrado.Add(aitem);
                }
            }

            return filtrado;
        }

        public List<FacturaExcel> FiltrarExcelNumeroFactura(List<FacturaExcel> Lista, string numerofactura)
        {
            List<FacturaExcel> filtrado = new List<FacturaExcel>();
            var x = (from e in Lista where e.numero_documento == numerofactura select e);
            filtrado.AddRange(x);
            return filtrado;
        }

        public FacturaDto GetDetalle(int FacturaId)
        {
            var Query = Repository.GetAllIncluding(c => c.Cliente, c => c.Empresa);

            var item = (from o in Query
                        where o.Id == FacturaId
                        where o.vigente == true
                        select new FacturaDto()
                        {
                            Id = o.Id,
                            Cliente = o.Cliente,
                            ClienteId = o.ClienteId,
                            codigosap = o.codigosap,
                            Empresa = o.Empresa,
                            EmpresaId = o.EmpresaId,
                            fecha_emision = o.fecha_emision,
                            fecha_vencimiento = o.fecha_vencimiento,
                            numero_documento = o.numero_documento,
                            tipo_documento = o.tipo_documento,
                            valor_a_cobrar = o.valor_a_cobrar,
                            valor_cobrado = o.valor_cobrado,
                            valor_importe = o.valor_importe,
                            valor_iva = o.valor_iva,
                            valor_total = o.valor_total,
                            vigente = o.vigente,
                            estado = o.estado,
                            //
                            descripcion = o.descripcion,
                            orden_servicio = o.orden_servicio,

                            documento_pago = o.documento_pago,
                            doc_compensacion = o.doc_compensacion,
                            numero_retencion = o.numero_retencion,
                            retencion_ir = o.retencion_ir,
                            retencion_iva = o.retencion_iva,
                            ov = o.ov,
                            os = o.os,
                        }).FirstOrDefault();
            return item;

        }

        public List<FacturaDto> GetFacturas()
        {
            var Query = Repository.GetAllIncluding(c => c.Cliente, c => c.Empresa, c => c.Catalogo).ToList();

            var item = (from o in Query
                        where o.vigente == true
                        select new FacturaDto()
                        {
                            Id = o.Id,

                            ClienteId = o.ClienteId,
                            Cliente = o.Cliente,
                            codigosap = o.codigosap,
                            EmpresaId = o.EmpresaId,
                            Empresa = o.Empresa,
                            fecha_emision = o.fecha_emision,
                            fecha_vencimiento = o.fecha_vencimiento,
                            numero_documento = o.numero_documento,
                            tipo_documento = o.tipo_documento,
                            valor_a_cobrar = o.valor_a_cobrar,
                            valor_cobrado = o.valor_cobrado,
                            valor_importe = o.valor_importe,
                            valor_iva = o.valor_iva,
                            valor_total = o.valor_total,
                            vigente = o.vigente,
                            estado = o.estado,
                            Catalogo = o.Catalogo,
                            nombreCliente=o.Cliente.razon_social,
                            nombreEmpresa=o.Empresa.razon_social,
                            nombreEstado=o.Catalogo.nombre,
                            formatFechaEmision = o.fecha_emision.ToString("dd/MM/yyyy"),
                            formatFechaVencimiento = o.fecha_vencimiento.HasValue ? o.fecha_vencimiento.Value.ToString("dd/MM/yyyy") : "",

                        }).ToList();
            return item;

        }



        public List<FacturaExcel> CrearCobros(List<FacturaExcel> Lista)
        {

            var facturas = Repository.GetAll().Where(c => c.vigente == true).ToList();

            foreach (var f in facturas)
            {

                var cobrosf = (from r in Lista
                               where r.documento_compensacion == f.doc_compensacion


                               select r).ToList();
                if (cobrosf != null && cobrosf.Count > 0)
                {
                    foreach (var item in cobrosf)
                    {
                        var cobro = this.ObtenerCobro(item.documento_compensacion);
                        if (cobro != null && cobro.Id > 0)
                        {


                            CobroFactura b = new CobroFactura
                            {
                                Id = 0,
                                FacturaId = f.Id,
                                CobroId = cobro.Id,
                                monto = f.valor_total - f.retencion_ir - f.retencion_iva,
                                vigente = true,


                            };


                            var cobrofactura = _cobrofacturarepository.Insert(b);

                            Factura e = Repository.Get(f.Id);

                            if (cobrofactura != null)
                            {
                                e.valor_cobrado = cobrofactura.monto;
                                e.documento_pago = cobrofactura.Factura.documento_pago;
                                e.estado = 4173;
                                var resultado = Repository.Update(e);
                            }
                            cobro.monto_detalle = cobro.monto_detalle + cobrofactura.Factura.valor_cobrado;
                            var actualizarcobro = _cobrorepository.Update(cobro);
                        }
                        else
                        {
                            Cobro a = new Cobro();
                            a.EmpresaId = f.EmpresaId;
                            a.ClienteId = f.ClienteId;
                            a.documento_sap = item.numero_documento;
                            a.descripcion = item.detalle;
                            a.fecha_documento = item.fecha_documento;
                            a.fecha_carga = DateTime.Now;
                            a.monto = item.importe_moneda * (-1);
                            a.documento_compensacion = item.documento_compensacion;
                            a.vigente = true;

                            a.Id = 0;

                            var CobroId = _cobrorepository.InsertAndGetId(a);

                            CobroFactura b = new CobroFactura
                            {
                                Id = 0,
                                FacturaId = f.Id,
                                CobroId = CobroId,
                                monto = f.valor_total - f.retencion_ir - f.retencion_iva,
                                vigente = true,


                            };
                            var cobrofactura = _cobrofacturarepository.Insert(b);
                            Factura e = Repository.Get(f.Id);

                            if (cobrofactura != null)
                            {
                                e.valor_cobrado = cobrofactura.monto;
                                e.documento_pago = cobrofactura.Factura.documento_pago;
                                e.estado = 4173;
                                var resultado = Repository.Update(e);
                            }

                            var cobros = _cobrorepository.Get(CobroId);
                            cobros.monto_detalle = cobro.monto_detalle + cobrofactura.Factura.valor_cobrado;
                            var actualizarcobro = _cobrorepository.Update(cobros);

                        }



                    }
                }

            }

            return Lista;
        }

        public RetencionDB BuscarRetencionesAB(List<FacturaExcel> Lista, string referencia)
        {

            if (Lista.Count > 0)
            {
                var retencioncodigo = referencia.Substring(6);
                var dato = "" + Int32.Parse(retencioncodigo);
                var retencion = (from e in Lista
                                 where e.clase_documento == "AB"

                                 where e.detalle.Split(' ')[e.detalle.Split(' ').Length - 1] == dato
                                 select e).ToList();

                decimal riva = 0;
                decimal rrenta = 0;
                string numeroretencion = "";

                var ri = (from r in retencion where r.cuenta == "1114020012" select r).FirstOrDefault();
                if (ri != null)
                {
                    riva = ri.importe_moneda * (-1);
                    numeroretencion = ri.referencia;
                }
                var rr = (from r in retencion where r.cuenta == "1114020013" select r).FirstOrDefault();
                if (rr != null)
                {
                    rrenta = rr.importe_moneda * (-1);
                    numeroretencion = rr.referencia;
                }

                RetencionDB a = new RetencionDB();
                a.iva1114020012 = riva;
                a.renta1114020013 = rrenta;
                a.numeroretencion = numeroretencion;
                return a;
            }
            else {
                return new RetencionDB();
            }


        }

        public Facturas BuscarCobrosDZ(List<FacturaExcel> Lista)
        {
            var Validas = (from e in Lista where e.clase_documento == "DZ" select e).ToList();
            var NoValidas = (from f in Lista where f.clase_documento != "DZ" select f).ToList();
            Facturas fa = new Facturas
            {
                Validas = Validas,
                NoValidas = NoValidas
            };
            return fa;

        }

        public Cobro ObtenerCobro(string documento_compensacion)
        {
            var cobro = _cobrorepository.GetAll().Where(c => c.vigente == true).Where(c => c.documento_compensacion == documento_compensacion).FirstOrDefault();
            if (cobro != null && cobro.Id > 0)
            {
                return cobro;
            }
            return new Cobro();
        }

        public FacturasDB RepartirFacturasCuenta(Facturas Lista)
        {
            List<FacturaExcel> F1113091010 = new List<FacturaExcel>();
            List<FacturaExcel> F2114210001 = new List<FacturaExcel>();
            List<FacturaExcel> F4112041000 = new List<FacturaExcel>();

       
            F1113091010 = (from x in Lista.Validas where x.cuenta!=null where x.cuenta.Substring(0, 5).ToString() == "11130" select x).ToList();
            F2114210001 = (from y in Lista.Validas where y.cuenta != null where y.cuenta.Substring(0, 5) == "21142" select y).ToList();
            F4112041000 = (from z in Lista.Validas where z.cuenta != null where z.cuenta.Substring(0, 5) == "41120" select z).ToList();

            FacturasDB a = new FacturasDB();

            a.Facturas1113091010 = F1113091010;
            a.Facturas2114210001 = F2114210001;
            a.Facturas4112041000 = F4112041000;
            return a;
        }

        public Factura AnularFactura(int Id)
        {
            var factura = Repository.Get(Id);
            if (factura != null && factura.Id > 0)
            {
                factura.estado = 4176;
                var anulado = Repository.Update(factura);
                return factura;
            }
            else
            {
                new Factura();
            }
            return new Factura();
        }

        public string ActualizarCobros(int Id)
        {
            var cobro = _cobrorepository.Get(Id);
            if (cobro != null && cobro.Id > 0)
            {
                var lista = _cobrofacturarepository.GetAll().Where(c => c.CobroId == cobro.Id).Where(c => c.vigente == true).ToList();

                var x = (from e in lista select e.monto).Sum();

                cobro.monto_detalle = x;

                var actualizarcobro = _cobrorepository.Update(cobro);
                return "OK";
            }
            return "ERROR";
        }

        public TiposFacturas CargarArchivosFacturas(HttpPostedFileBase UploadedFile)
        {
            List<FacturaExcel> Lista = new List<FacturaExcel>();
            if (UploadedFile != null)
            {
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;

                        if (noOfCol > 29 || noOfCol < 29)
                        {
                            return new TiposFacturas();
                        }
                        else
                        {

                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var cuenta = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();
                                if (cuenta.Equals("") || cuenta == "")
                                {
                                    cuenta = "0";
                                }

                                var sociedad = (workSheet.Cells[rowIterator, 4].Value ?? "").ToString();
                                if (sociedad.Equals("") || sociedad == "")
                                {
                                    sociedad = "0";
                                }

                                var fechadocumento = (workSheet.Cells[rowIterator, 15].Value ?? "").ToString();
                                if (fechadocumento.Equals("") || fechadocumento == "")
                                {
                                    fechadocumento = "01/01/1990";
                                }

                                var factura = (workSheet.Cells[rowIterator, 9].Value ?? "").ToString();
                                if (factura.Equals("") || factura == "")
                                {
                                    factura = "0";
                                }
                                var referencia = (workSheet.Cells[rowIterator, 10].Value ?? "").ToString();
                                if (referencia.Equals("") || referencia == "")
                                {
                                    referencia = "0";
                                }

                                var detalle = (workSheet.Cells[rowIterator, 12].Value ?? "").ToString();
                                if (detalle.Equals("") || detalle == "")
                                {
                                    detalle = "0";
                                }

                                var clasedocumento = (workSheet.Cells[rowIterator, 11].Value ?? "").ToString();

                                if (clasedocumento.Equals("") || clasedocumento == "")
                                {
                                    clasedocumento = "0";
                                }

                                var doccompensacion = (workSheet.Cells[rowIterator, 20].Value ?? "").ToString();


                                if (doccompensacion.Equals("") || doccompensacion == "")
                                {
                                    doccompensacion = "0";
                                }

                                var importemoneda = (workSheet.Cells[rowIterator, 19].Value ?? "").ToString();
                                if (importemoneda.Equals("") || importemoneda == "")
                                {
                                    importemoneda = "0";
                                }

                                var fechacompensacion = (workSheet.Cells[rowIterator, 24].Value ?? "").ToString();

                                if (fechacompensacion.Equals("") || fechacompensacion == "")
                                {
                                    fechacompensacion = "01/01/1990";
                                }

                                var fecha_pago = (workSheet.Cells[rowIterator, 16].Value ?? "").ToString();
                                if (fecha_pago.Equals("") || fecha_pago == "")
                                {
                                    fecha_pago = "01/01/1990";
                                }

                                var cliente = (workSheet.Cells[rowIterator, 6].Value ?? "").ToString();

                                if (cliente.Equals("") || cliente == "")
                                {
                                    cliente = "0";
                                }



                                FacturaExcel a = new FacturaExcel
                                {
                                    id = rowIterator,
                                    cuenta = cuenta,
                                    sociedad = sociedad,
                                    fecha_documento = DateTime.Parse(fechadocumento),
                                    numero_documento = factura,
                                    referencia = referencia,
                                    detalle = detalle,
                                    clase_documento = clasedocumento,
                                    documento_compensacion = doccompensacion,
                                    importe_moneda = Decimal.Parse(importemoneda),
                                    fecha_compensacion = DateTime.Parse(fechacompensacion),
                                    fecha_pago = DateTime.Parse(fecha_pago),
                                    cliente = cliente

                                };
                                Lista.Add(a);
                            }


                        }
                    }
                }
            }
            ///

            var DB = (from e in Lista where e.clase_documento == "DB" select e).ToList();
            var DR = (from e in Lista where e.clase_documento == "DR" select e).ToList();
            var DE = (from e in Lista where e.clase_documento == "DE" select e).ToList();
            var DI = (from e in Lista where e.clase_documento == "DI" select e).ToList();
            var DZ = (from e in Lista where e.clase_documento == "DZ" select e).ToList();
            var DF = (from e in Lista where e.clase_documento == "DF" select e).ToList();
            var DG = (from e in Lista where e.clase_documento == "DG" select e).ToList();
            var AB = (from e in Lista where e.clase_documento == "AB" select e).ToList();

            TiposFacturas tiposfacturas = new TiposFacturas() {
              DB=DB,
              DR=DR,
              DE=DE,
              DI=DI,
              AB=AB,
              DZ=DZ,
              DF=DF,
              DG=DG

            };

            return tiposfacturas;
        }

        public ExcelPackage ReporteFacturas()
        {
            ExcelPackage package = new ExcelPackage();
            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.Add("Reingresos");
            ExcelWorksheet h = package.Workbook.Worksheets[1];


            int count = 1;
            string cell = "A" + count;
            h.Cells[cell].Value = "ID SAP";
            cell = "B" + count;
            h.Cells[cell].Value = "LEGAJO";
            cell = "C" + count;
            h.Cells[cell].Value = "NOMBRES";
            cell = "D" + count;
            h.Cells[cell].Value = "CARGO";
            cell = "E" + count;
            h.Cells[cell].Value = "FECHA NACIMIENTO";
            cell = "F" + count;
            h.Cells[cell].Value = "FECHA INGRESO";
            cell = "G" + count;
            h.Cells[cell].Value = "FECHA SALIDA";
            cell = "H" + count;
            h.Cells[cell].Value = "PROYECTO";
            cell = "I" + count;
            h.Cells[cell].Value = "REINGRESO(SI / NO)";
            cell = "J" + count;
            h.Cells[cell].Value = "MOTIVO DE BAJA(ÚLTIMO)";
            cell = "K" + count;
            h.Cells[cell].Value = "STATUS";

            count++;
            return package;
        }
    }
}