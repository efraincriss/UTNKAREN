﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto
{
    public class ResultadoCertificacion
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public GrupoCertificadoIngenieriaDto Grupo { get; set; }

    }
}
