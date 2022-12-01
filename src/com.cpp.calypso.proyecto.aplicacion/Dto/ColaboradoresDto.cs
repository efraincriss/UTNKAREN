using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.seguridad.aplicacion;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Colaboradores))]
    [Serializable]
    public class ColaboradoresDto : EntityDto, IFullAudited
    {
        [CanBeNull]
        [DisplayName("Tipo de identificacion")]
        public int? catalogo_tipo_identificacion_id { get; set; }

        [DisplayName("Género")]
        public int? catalogo_genero_id { get; set; }

        [DisplayName("Etnia")]
        public int? catalogo_etnia_id { get; set; }

        [DisplayName("Estado Civil")]
        public int? catalogo_estado_civil_id { get; set; }

        [CanBeNull]
        [DisplayName("Código Siniestro")]
        public int? catalogo_codigo_siniestro_id { get; set; }

        [CanBeNull]
        [DisplayName("Formación Educativa")]
        public int? catalogo_formacion_educativa_id { get; set; }

        [DisplayName("Agrupación para Requisitos")]
        public int? catalogo_grupo_personal_id { get; set; }

        [DisplayName("Destino (Estancia):")]
        public int? catalogo_destino_estancia_id { get; set; }

        [DisplayName("Sitio de Trabajo")]
        public string catalogo_sitio_trabajo_id { get; set; }

        [DisplayName("Área")]
        public int? catalogo_area_id { get; set; }

        [DisplayName("Cargo")]
        public int? catalogo_cargo_id { get; set; }

        [DisplayName("Vinculo Laboral")]
        public int? catalogo_vinculo_laboral_id { get; set; }

        [DisplayName("Clase")]
        public int? catalogo_clase_id { get; set; }

        [DisplayName("Plan de Beneficios")]
        public int? catalogo_plan_beneficios_id { get; set; }

        [DisplayName("Opción Plan de Salud")]
        public int? catalogo_plan_salud_id { get; set; }

        [DisplayName("Cobertura Dependiente")]
        public int? catalogo_cobertura_dependiente_id { get; set; }

        [DisplayName("Planes de Beneficios")]
        public int? catalogo_planes_beneficios_id { get; set; }

        [DisplayName("Asociación")]
        public int? catalogo_asociacion_id { get; set; }

        [DisplayName("Tipo Apto Médico")]
        public int? catalogo_apto_medico_id { get; set; }

        [DisplayName("División Personal")]
        public int? catalogo_division_personal_id { get; set; }

        [DisplayName("Subdivisión Personal")]
        public int? catalogo_subdivision_personal_id { get; set; }

        [DisplayName("Tipo de Contrato")]
        public int? catalogo_tipo_contrato_id { get; set; }

        [DisplayName("Clase de Contrato")]
        public int? catalogo_clase_contrato_id { get; set; }

        [DisplayName("Función")]
        public int? catalogo_funcion_id { get; set; }

        [DisplayName("Tipo de Nómina")]
        public int? catalogo_tipo_nomina_id { get; set; }

        [DisplayName("Período de Nómina")]
        public int? catalogo_periodo_nomina_id { get; set; }

        [DisplayName("Forma de Pago")]
        public int? catalogo_forma_pago_id { get; set; }

        [DisplayName("Grupo (Categoría O PC)")]
        public int? catalogo_grupo_id { get; set; }

        [DisplayName("Sub Grupo (Cuartil)")]
        public int? catalogo_subgrupo_id { get; set; }

        [DisplayName("Banco")]
        public int? catalogo_banco_id { get; set; }

        [DisplayName("Tipo de Cuenta")]
        public int? catalogo_tipo_cuenta_id { get; set; }

        [DisplayName("Empresa")]
        public int? empresa_id { get; set; }

        [DisplayName("Nacionalidad")]
        public int? PaisId { get; set; }
        public virtual Pais Pais { get; set; }

        [DisplayName("País de Nacimiento")]
        public int pais_pais_nacimiento_id { get; set; }

        [DisplayName("Régimen (Rotación):")]
        public int? AdminRotacionId { get; set; }
        public virtual AdminRotacion AdminRotacion { get; set; }

        [DisplayName("Proyecto")]
        public int? ContratoId { get; set; }

        [DisplayName("Contacto")]
        public int? ContactoId { get; set; }
        public virtual Contacto Contacto { get; set; }

        [DisplayName("ID Empleado:")]
        public int? empleado_id_sap { get; set; }

        [DisplayName("ID Candidato")]
        public int? candidato_id_sap { get; set; }

        [CanBeNull]
        [MaxLength(25)]
        [DisplayName("Nro Identificación:")]
        public string numero_identificacion { get; set; }

        [CanBeNull]
        [DisplayName("Nombres")]
        public string nombres { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Ingreso")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_ingreso { get; set; }

        [CanBeNull]
        [DisplayName("Prmer Apellido:")]
        public string primer_apellido { get; set; }

        [CanBeNull]
        [DisplayName("Segundo Apellido:")]
        public string segundo_apellido { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_nacimiento { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Matrimonio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_matrimonio { get; set; }

        [CanBeNull]
        [DisplayName("Meta 4")]
        public string meta4 { get; set; }

        [DisplayName("Posición")]
        public string posicion { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Caducidad Contrato")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_caducidad_contrato { get; set; }

        [DisplayName("Es Ejecutor de Obra?")]
        public bool? ejecutor_obra { get; set; }

        [DisplayName("Remuneración Mensual")]
        public decimal? remuneracion_mensual { get; set; }

        [DisplayName("Número de Cuenta")]
        public string numero_cuenta { get; set; }

        [DisplayName("Número de Legajo")]
        public string numero_legajo_temporal { get; set; }

        [DisplayName("Número de Legajo")]
        public string numero_legajo_definitivo { get; set; }

        [DisplayName("Numero Hijos:")]
        public int? numero_hijos { get; set; }

        [CanBeNull]
        [DisplayName("Estado")]
        public string estado { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public bool vigente { get; set; } = true;

        [DisplayName("Encuadre")]
        public int? catalogo_encuadre_id { get; set; }

        [DisplayName("Encargado de personal:")]
        public int? catalogo_encargado_personal_id { get; set; }

        [DisplayName("Validación Cédula")]
        public bool validacion_cedula { get; set; } = false;

        [CanBeNull]
        [DisplayName("Nombres Apellidos")]
        public string nombres_apellidos { get; set; }

        [DisplayName("Codigo Incapacidad")]
        public int? catalogo_codigo_incapacidad_id { get; set; }

        [DisplayName("Sector")]
        public int? catalogo_sector_id { get; set; }

        [DisplayName("Vía de Pago")]
        public int? catalogo_via_pago_id { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Fecha Alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_alta { get; set; }

        [CanBeNull]
        [DisplayName("Código Dactilar")]
        [MaxLength(10)]
        public string codigo_dactilar { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Registro Civil")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_registro_civil { get; set; }

        [Obligado]
        [DisplayName("Viene Registro Civil")]
        public bool viene_registro_civil { get; set; } = false;

        [DataType(DataType.Date)]
        [DisplayName("Fecha Sustituto Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_sustituto_desde { get; set; }

        [DisplayName("Es sustituto")]
        public bool? es_sustituto { get; set; } = false;

        [DisplayName("Es externo")]
        public bool? es_externo { get; set; } = false;
        
        [DisplayName("Registro Masivo")]
        public bool es_carga_masiva { get; set; } = false;

        public bool es_visita { get; set; } = false;

        public string empresa_tercero { get; set; } = "";

        [DataType(DataType.Date)]
        [DisplayName("Fecha Sustituto Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_carga_masiva { get; set; }

        [Obligado]
        [DisplayName("Tiene Ausentismo")]
        public bool tiene_ausentismo { get; set; } = false;

        public string codigo_seguridad_qr { get; set; }

        public bool es_reingreso { get; set; } = false;


        [DisplayName("ID Empleado SAP Local")]
        public int? empleado_id_sap_local { get; set; }


        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }


        public virtual string nombre_identificacion { get; set;  }

        public virtual string nombre_estado { get; set;  }

		public virtual string nombre_destino { get; set; }
        public virtual string nombre_grupo_personal { get; set; }
        public virtual string nombre_genero { get; set; }
        public virtual string codigo_genero { get; set; }

        public virtual int nro { get; set; }

        public virtual string apellidos_nombres { get; set; }
		
		public virtual string h_desde { get; set; }
		
		public virtual string h_hasta { get; set; }
		public virtual string telefono { get; set; }

		public virtual List<RequisitoColaboradorDto> requisitos { get; set; }
        public virtual List<ColaboradorRequisitoDto> req_cumple { get; set; }

        //variables para formacion educativa
        public virtual int? formacion { get; set; }
        public virtual string institucion_educativa { get; set; }
        public virtual int? catalogo_titulo_id { get; set; }
        public virtual DateTime? fecha_registro_senecyt { get; set; }

        //variables para discapacidad
        public virtual bool? discapacidad { get; set; }
        public virtual int? catalogo_tipo_discapacidad_id { get; set; }
        public virtual int? catalogo_porcentaje_id { get; set; }

        //variables para bajas

        public virtual int? catalogo_motivo_baja_id { get; set; }
        public virtual int? baja_id { get; set; }
        public virtual string motivo_baja{ get; set; }
        public virtual DateTime? fecha_baja { get; set; }
        public virtual DateTime? liquidado { get; set; }
        public virtual BajaEstado estado_baja { get; set; }


        //variables para contactos
        public virtual string calle { get; set; }
        public virtual string numero { get; set; }
        public virtual string codigo_postal { get; set; }
        public virtual string region { get; set; }
        public virtual string email { get; set; }
        public virtual string subregion { get; set; }
        public virtual int parroquia { get; set; }
        public virtual int comunidad { get; set; }





        public virtual List<ColaboradorCargaSocial> cargas { get; set; }
        public virtual ColaboradoresVisitaDto visita { get; set; }
        public virtual UsuarioDto Usuario { get; set; }
        public virtual List<ColaboradorResponsabilidadDto> responsabilidades { get; set; }
        public virtual List<ColaboradoresAusentismoDto> ausentismos { get; set; }


        //VARIABLE ESTACION
        public virtual string nombreestancia { get; set; }
        //SERVICIOS ASIGANDOS
        public virtual string serviciosvigentes { get; set; }
        public virtual string tienereservaactiva { get; set; }
        public virtual string fechavigenciacolaboradorqr { get; set; }
        public virtual int numeroHuellas { get; set; }

        public virtual string MergeApellidos { get; set; }

        public virtual string estado_colaborador { get; set; }
        public virtual bool posee_ausentismos { get; set; }
        public virtual bool posee_ausentismos_vigentes { get; set; }

        public virtual string nombreTipo { get; set; }
        public virtual string estaLiquidado { get; set; }

        public virtual string  fechaIngresoFormat { get; set; }
        public virtual DateTime? fechaActualizacionFormat { get; set; }
    }
}
