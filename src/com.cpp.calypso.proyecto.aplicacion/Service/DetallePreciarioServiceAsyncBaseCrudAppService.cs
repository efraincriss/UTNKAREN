using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.entityframework;
using OfficeOpenXml;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class DetallePreciarioServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetallePreciario, DetallePreciarioDto, PagedAndFilteredResultRequestDto>, IDetallePreciarioAsyncBaseCrudAppService
    {
        private IBaseRepository<Preciario> _repositorypreciario;
        private IBaseRepository<Catalogo> _repositorycatalogo;
        private IBaseRepository<Item> _itemrepository;
        public DetallePreciarioServiceAsyncBaseCrudAppService(IBaseRepository<DetallePreciario> repository,
            IBaseRepository<Preciario> repositorypreciario,
            IBaseRepository<Item> itemrepository, IBaseRepository<Catalogo> repositorycatalogo
            ) : base(repository)
        {
            _repositorypreciario = repositorypreciario;
            _itemrepository = itemrepository;
            _repositorycatalogo=repositorycatalogo;
    }

       

        public DetallePreciarioDto comprobarexistenciaitem(int PreciarioId,int ItemId)
        {
            DetallePreciarioDto encontrado = null;
            var item = Repository.GetAll().Where(x=>x.PreciarioId== PreciarioId).Where(x=>x.ItemId==ItemId).Where(x=>x.vigente);
            var detalle = (from r in item
                where r.PreciarioId == PreciarioId
                where r.ItemId == ItemId
                where r.vigente == true
                select new DetallePreciarioDto
                {
                    Id = r.Id,
                    PreciarioId = r.PreciarioId,
                    ItemId = r.ItemId,
                    vigente = r.vigente,
                    precio_unitario = r.precio_unitario

                }).FirstOrDefault();
            if ( detalle!=null  && detalle.Id > 0)
            {
                return detalle;
            }

            return encontrado;
        }

        public ExcelPackage GenerarExcelPreciarioValores(int id)
        {
            var items = this.GetValoresPreciarioActual(id).Where(x=>x.Item.GrupoId!=3).OrderBy(dto => dto.Item.codigo);

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Preciario Actual");
             workSheet.DefaultRowHeight = 12;
            workSheet.Cells[1, 2].Value = "Código Item";
            workSheet.Cells[1, 3].Value = "Nombre Item";
            workSheet.Cells[1, 4].Value = "Unidad";
            workSheet.Cells[1, 5].Value = "Precio Unitario";
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Row(1).Height = 20;
            workSheet.Cells["B1:E1"].AutoFilter = true;
            workSheet.Column(1).Hidden = true;
            workSheet.Column(2).Width = 20;
            workSheet.Column(3).Width = 40;
            workSheet.Column(5).Width = 40;



            int c = 2;
            workSheet.Cells[1, 1].Value = id;
            foreach (var pitem in items)
            {
                workSheet.Cells[c, 1].Value = pitem.Id;
                workSheet.Cells[c, 2].Value = pitem.Item.codigo;
                workSheet.Cells[c, 3].Value = pitem.Item.nombre;
                if (pitem.Item.UnidadId >= 0) {
                workSheet.Cells[c, 4].Value = _repositorycatalogo.Get(pitem.Item.UnidadId).nombre;
                }
                workSheet.Cells[c, 5].Value = pitem.precio_unitario;
                workSheet.Cells[c,6].Value = pitem.ItemId;
                c = c + 1;
            }
            return excel;
        }

        public DetallePreciarioDto GetDetalles(int DetallePreciarioId)
        {
            var detallesQuery = Repository.GetAllIncluding(c=>c.Preciario);
            var items = (from c in detallesQuery
                         where c.Id == DetallePreciarioId
                         where c.vigente == true
                         select new DetallePreciarioDto
                         {
                             Id = c.Id,
                             Preciario = c.Preciario,
                             PreciarioId = c.PreciarioId,
                             Item = c.Item,
                             ItemId = c.ItemId,
                             comentario = c.comentario,
                             precio_unitario = c.precio_unitario,
                             vigente = c.vigente,
                        

                         }).FirstOrDefault();

            return items;
        
    }

        public List<DetallePreciarioDto> GetDetallesPreciarios(int PreciarioId)
        {
            var detallesQuery = Repository.GetAll();
            var items = (from c in detallesQuery
                                where c.PreciarioId == PreciarioId
                                where c.vigente == true
                                select new DetallePreciarioDto
                                {
                                    Id = c.Id,
                                   PreciarioId=c.PreciarioId,
                                    Preciario = c.Preciario,
                                    ItemId = c.ItemId,
                                    Item =c.Item,
                                   comentario=c.comentario,
                                   precio_unitario=c.precio_unitario,
                                    vigente = c.vigente,
                                }).ToList();

                  foreach (var w in items)
            {
                      var name = _itemrepository.GetAll()
                    .Where(o => o.vigente == true)
                    .Where(o => o.codigo == w.Item.item_padre).FirstOrDefault();

                if(name!=null && name.Id > 0) { 
                w.nombreitempadre = name.nombre;
                }
            }

            return items;

       
        }

        public List<DetallePreciario> GetValoresPreciarioActual(int id)
        {
            var items = Repository.GetAll().
                 Where(c => c.PreciarioId == id).
                 Where(c => c.vigente == true).ToList();

            return items;
        }

        public decimal ObtenerPrecioIncrementado(Item Item, decimal preciobase, decimal sumaporcentajes)
        {
          var detallesQuery = Repository.GetAllIncluding(c=>c.Preciario,c=>c.Item).Where(c=>c.vigente==true);

            decimal precioincrementado = 0;
            foreach (var item in detallesQuery.ToList())
            {
                if (item.ItemId == Item.Id) {
                    precioincrementado = preciobase+(preciobase+sumaporcentajes);
                }
           
            }
            return precioincrementado;

        }
        
        public decimal ObtenerPrecioUnitario(Item Item)
        {
            var detallesQuery = Repository.GetAllIncluding(c=>c.Preciario,c=>c.Item).Where(c=>c.vigente==true);

            decimal preciounitario = 0;
            foreach (var item in detallesQuery.ToList())
            {
                if (item.ItemId == Item.Id) {
                    preciounitario = item.precio_unitario;
                }
           
            }
            return preciounitario;

        }

       
    }
    

}
