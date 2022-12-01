using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Constantes
{
    public static class RRHHCodigos
    {
        //Estados Colaborador
        public const string ESTADO_TEMPORAL = "TEMPORAL";
        public const string ESTADO_INFORMACION_COMPLETA = "INFORMACION COMPLETA";
        public const string ESTADO_ACTIVO = "ACTIVO";
        public const string ESTADO_INACTIVO = "INACTIVO";
        public const string ESTADO_ENVIADO_SAP = "ENVIADO SAP";

        public const string ESTADO_ALTAANULADA= "ALTA ANULADA";

        //Estados Bajas
        public const string BAJA_ENVIAR_SAP = "ENVIAR_SAP";
        public const string BAJA_ENVIADO_SAP = "ENVIADO_SAP";
        public const string BAJA_LIQUIDADO = "LIQUIDADO";
        public const string BAJA_INACTIVO = "INACTIVO";

        //Generar Excel Altas
        public const string MOTIVO_MEDIDA = "MOTIVOMEDIDA";
        public const string MOTIVO_MEDIDA_ALTA = "01";
        public const string MOTIVO_MEDIDA_REINGRESO = "08";

        //Carga Masiva
        public const string TIPO_IDENTIFICACION = "TIPOINDENTIFICACION";
        public const string GENERO = "GENERO";
        public const string ETNIA = "ETNIA";
        public const string NACIONALIDAD = "NACIONALIDADES";
        public const string DESTINO = "DESTINOS";
        public const string ENCARGADO_PERSONAL = "ENCARGADO";
        public const string SECTOR = "SECTOR";
        public const string CARGO = "CARGO";
        public const string TIPO_IDENTIFICACION_CEDULA = "CÉDULA";
        public const string CODIGO_IDENTIFICACION_CEDULA = "CEDULA";
        public const string PROYECTO = "PROYECTO";
        public const string GRUPOPERSONAL = "GRUPOPERSONAL";

        //Discapacidad
        public const string CODIGO_INCAPACIDAD = "CODIGOINCAPACIDAD";
        public const string CODIGO_SINIESTRO = "CODIGOSINIESTRO";
        public const string CODIGO_INCAPACIDAD_NOINCAPACITADO = "9-NOINC";
        public const string CODIGO_SINIESTRO_NOINCAPACITADO = "42-NOIN";

        //Destino
        public const string DESTINO_FORANEO = "FOR";

        //Grupo Personal
        public const string GRUPO_PERSONAL = "GRUPOPERSONAL";
        public const string GRUPO_PERSONAL_VISITA = "VIS";

        //Consulta Publica Registro Civil
        public const string SEXO_HOMBRE = "HOMBRE";
        public const string SEXO_MUJER = "MUJER";
        public const string REGISTRO_CIVIL_SOLTERO = "SOLTERO";
        public const string REGISTRO_CIVIL_CASADO = "CASADO";
        public const string REGISTRO_CIVIL_DIVORCIADO = "DIVORCIADO";
        public const string REGISTRO_CIVIL_ECUATORIANA = "ECUATORIANA";

        //CODIGOS Catalogos
        public const string GENERO_VARON = "VAR";
        public const string GENERO_MUJER = "MUJ";
        public const string ESTADO_CIVIL_SOLTERO = "SOL";
        public const string ESTADO_CIVIL_CASADO = "CAS";
        public const string ESTADO_CIVIL_DIVORCIADO = "DIV";
        public const string NACIONALIDAD_ECUATORIANA = "239";
        public const string CODIGO_CATALOGO_GENERAL = "inicial";
        

    }
}
