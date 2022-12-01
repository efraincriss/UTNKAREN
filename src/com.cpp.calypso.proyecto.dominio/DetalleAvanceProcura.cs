using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class DetalleAvanceProcura: Entity
    {

        [Obligado]
        [DisplayName("Avance Procura")]
        public int AvanceProcuraId { get; set; }


        public virtual AvanceProcura AvanceProcura { get; set; }

        [Obligado]
        [DisplayName("Item")]
        public int DetalleOrdenCompraId { get; set; }

        public virtual DetalleOrdenCompra DetalleOrdenCompra { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Presentación")]
        public virtual DateTime fecha_real { get; set; }

        [Obligado]
        [DisplayName("Valor Real")]
        public virtual decimal valor_real { get; set; }

      
        [DisplayName("Cantidad")]
        public virtual decimal cantidad { get; set; }

        [Obligado]
        [DisplayName("Precio Unitario")]
        public virtual decimal precio_unitario { get; set; }

        [Obligado]
        public virtual bool vigente { get; set; } = true;

        [DisplayName("Estado")]
        public virtual EstadoDetalleProcura estado { get; set; }

        [DisplayName("Ingreso Acumulado")]
        public decimal ingreso_acumulado { get; set; }

        [DisplayName("Cálculo Diario")]
        public decimal calculo_diario { get; set; }

        [DisplayName("Cálculo Anterior")]
        public decimal calculo_anterior { get; set; }

        public bool estacertificado { get; set; } = false;

        public enum EstadoDetalleProcura
        {
           Registrado=1,
            Aprobado=2
        }

    }
}
