using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Models
{
    public class DetallesSinCertificar
    {
        public List<DetallesDirectosIngenieriaDto> Directos { get; set; }
        public List<DetalleIndirectosIngenieriaDto> Indirectos { get; set; }
        public List<DetalleDirectoE500Dto> DirectosE500 { get; set; }
        public List<Proyecto> ProyectosADistribuir { get; set; }

        

        public int PorcentajeMaximoDistribucion { get; set; }



        public List<DetallesDirectosIngenieriaDto> DirectosPendientes { get; set; }
        public List<DetalleIndirectosIngenieriaDto> IndirectosPendientes { get; set; }
        public List<DetalleDirectoE500Dto> DirectosE500Pendientes { get; set; }
    }
}
