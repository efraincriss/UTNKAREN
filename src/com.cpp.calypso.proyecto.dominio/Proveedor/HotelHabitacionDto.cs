using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace com.cpp.calypso.proyecto.dominio.Proveedor
{
    [Serializable]
    public class HotelHabitacionDto : EntityDto
    {
        public string razon_social { get; set; }

        public int espacios_totales { get; set; }

        public int espacios_libres { get; set; }

        public int espacio_ocupados { get; set; }

    }
}
