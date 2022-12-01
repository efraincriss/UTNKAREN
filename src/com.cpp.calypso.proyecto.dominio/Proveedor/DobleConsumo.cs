using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{

    public class DobleConsumo : Entity, ISoftDelete
    {
        public DateTime Fecha { get; set; }

        public int TipoComidaId { get; set; }

        public Catalogo TipoComida { get; set; }

        public OrigenConsumo? OrigenConsumoId { get; set; }

        public int ProveedorId { get; set; }

        public dominio.Proveedor.Proveedor Proveedor { get; set; }

        public int ColaboradorId { get; set; }

        public Colaboradores Colaborador { get; set; }

        public string Identificador { get; set; }

        public DateTime? fs { get; set; }

        public DateTime? fr { get; set; }

        public int Version { get; set; }

        public string uid { get; set; }

        public bool IsDeleted { get; set; }
    }
}
