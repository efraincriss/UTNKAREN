using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class TotalesSegundoContrato
    {
        public decimal A_VALOR_COSTO_TOTAL_INGENIERÍA_BASICA_YDETALLE_AIU_ANEXO1 { get; set; }
        public decimal VALOR_PROCURA { get; set; }
        public decimal Administracion_sobre_Procura_Contratista{ get; set; }
        public decimal B_VALOR_COSTO_DIRECTO_PROCURA_CONTRATISTA_ANEXO2 { get; set; }
        public decimal VALOR_SUBCONTRATOS { get; set; }
        public decimal Administracion_sobre_Subcontratos_Contratista { get; set; }
        public decimal C_VALOR_COSTO_DIRECTO_SUBCONTRATOS_CONTRATISTA { get; set; }

        public decimal VALOR_COSTO_DIRECTO_OBRAS_CIVILES { get; set; }
        public decimal VALOR_COSTO_DIRECTO_OBRAS_MECANICAS { get; set; }
        public decimal VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS { get; set; }
        public decimal VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL { get; set; }
        public decimal VALOR_COSTO_DIRECTO_SERVICIOS_ESPECIALES { get; set; }
        public decimal DESCUENTO_ITEMS_MECÁNICOS_ELÉCTRICOS_INSTRUMENTACIÓN { get; set; }
        public decimal D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN { get; set; }
        public decimal Administracion_sobre_Obra { get; set; }
        public decimal Imprevistos_sobre_Obra { get; set; }
        public decimal Utilidad_sobre_Obra { get; set; }
        public decimal E_INDIRECTOS_SOBRE_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN { get; set; }
        public decimal COSTO_TOTAL_DEL_PROYECTO_ABCDE { get; set; }
    }
}
