using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class Colaboradores : Entity, IFullAudited
    {
        [CanBeNull]
        [DisplayName("Tipo de identificacion")]
        [ForeignKey("TipoIdentificacion")]
        public int? catalogo_tipo_identificacion_id { get; set; }
        public virtual Catalogo TipoIdentificacion { get; set; }

        [DisplayName("Género")]
        [ForeignKey("Genero")]
        public int? catalogo_genero_id { get; set; }
        public virtual Catalogo Genero { get; set; }

        [DisplayName("Etnia")]
        [ForeignKey("Etnia")]
        public int? catalogo_etnia_id { get; set; }
        public virtual Catalogo Etnia { get; set; }

        [DisplayName("Estado Civil")]
        [ForeignKey("EstadoCivil")]
        public int? catalogo_estado_civil_id { get; set; }
        public virtual Catalogo EstadoCivil { get; set; }

        [CanBeNull]
        [DisplayName("Código Siniestro")]
        [ForeignKey("CodigoSiniestro")]
        public int? catalogo_codigo_siniestro_id { get; set; }
        public virtual Catalogo CodigoSiniestro { get; set; }

        [CanBeNull]
        [DisplayName("Formación Educativa")]
        [ForeignKey("FormacionEducativa")]
        public int? catalogo_formacion_educativa_id { get; set; }
        public virtual Catalogo FormacionEducativa { get; set; }

        [DisplayName("Agrupación para Requisitos")]
        [ForeignKey("GrupoPersonal")]
        public int? catalogo_grupo_personal_id { get; set; }
        public virtual Catalogo GrupoPersonal { get; set; }

        [DisplayName("Destino (Estancia):")]
        [ForeignKey("DestinoEstancia")]
        public int? catalogo_destino_estancia_id { get; set; }
        public virtual Catalogo DestinoEstancia { get; set; }

        [DisplayName("Sitio de Trabajo")]
        public string catalogo_sitio_trabajo_id { get; set; }

        [DisplayName("Área")]
        [ForeignKey("Area")]
        public int? catalogo_area_id { get; set; }
        public virtual Catalogo Area { get; set; }

        [DisplayName("Cargo")]
        [ForeignKey("Cargo")]
        public int? catalogo_cargo_id { get; set; }
        public virtual Catalogo Cargo { get; set; }

        [DisplayName("Vinculo Laboral")]
        [ForeignKey("VinculoLaboral")]
        public int? catalogo_vinculo_laboral_id { get; set; }
        public virtual Catalogo VinculoLaboral { get; set; }

        [DisplayName("Clase")]
        [ForeignKey("Clase")]
        public int? catalogo_clase_id { get; set; }
        public virtual Catalogo Clase { get; set; }

        [DisplayName("Plan de Beneficios")]
        [ForeignKey("PlanBeneficios")]
        public int? catalogo_plan_beneficios_id { get; set; }
        public virtual Catalogo PlanBeneficios { get; set; }

        [DisplayName("Opción Plan de Salud")]
        [ForeignKey("PlanSalud")]
        public int? catalogo_plan_salud_id { get; set; }
        public virtual Catalogo PlanSalud { get; set; }

        [DisplayName("Cobertura Dependiente")]
        [ForeignKey("CoberturaDependiente")]
        public int? catalogo_cobertura_dependiente_id { get; set; }
        public virtual Catalogo CoberturaDependiente { get; set; }

        [DisplayName("Planes de Beneficios")]
        [ForeignKey("PlanesBeneficios")]
        public int? catalogo_planes_beneficios_id { get; set; }
        public virtual Catalogo PlanesBeneficios { get; set; }

        [DisplayName("Asociación")]
        [ForeignKey("Asociacion")]
        public int? catalogo_asociacion_id { get; set; }
        public virtual Catalogo Asociacion { get; set; }

        [DisplayName("Tipo Apto Médico")]
        [ForeignKey("AptoMedico")]
        public int? catalogo_apto_medico_id { get; set; }
        public virtual Catalogo AptoMedico { get; set; }

        [DisplayName("División Personal")]
        [ForeignKey("DivisionPersonal")]
        public int? catalogo_division_personal_id { get; set; }
        public virtual Catalogo DivisionPersonal { get; set; }

        [DisplayName("Subdivisión Personal")]
        [ForeignKey("SubdivisionPersonal")]
        public int? catalogo_subdivision_personal_id { get; set; }
        public virtual Catalogo SubdivisionPersonal { get; set; }

        [DisplayName("Tipo de Contrato")]
        [ForeignKey("TipoContrato")]
        public int? catalogo_tipo_contrato_id { get; set; }
        public virtual Catalogo TipoContrato { get; set; }

        [DisplayName("Clase de Contrato")]
        [ForeignKey("ClaseContrato")]
        public int? catalogo_clase_contrato_id { get; set; }
        public virtual Catalogo ClaseContrato { get; set; }

        [DisplayName("Función")]
        [ForeignKey("Funcion")]
        public int? catalogo_funcion_id { get; set; }
        public virtual Catalogo Funcion { get; set; }

        [DisplayName("Tipo de Nómina")]
        [ForeignKey("TipoNomina")]
        public int? catalogo_tipo_nomina_id { get; set; }
        public virtual Catalogo TipoNomina { get; set; }

        [DisplayName("Período de Nómina")]
        [ForeignKey("PeriodoNomina")]
        public int? catalogo_periodo_nomina_id { get; set; }
        public virtual Catalogo PeriodoNomina { get; set; }

        [DisplayName("Forma de Pago")]
        [ForeignKey("FormaPago")]
        public int? catalogo_forma_pago_id { get; set; }
        public virtual Catalogo FormaPago { get; set; }

        [DisplayName("Grupo (Categoría O PC)")]
        [ForeignKey("Grupo")]
        public int? catalogo_grupo_id { get; set; }
        public virtual Catalogo Grupo { get; set; }

        [DisplayName("Sub Grupo (Cuartil)")]
        [ForeignKey("SubGrupo")]
        public int? catalogo_subgrupo_id { get; set; }
        public virtual Catalogo SubGrupo { get; set; }

        [DisplayName("Banco")]
        [ForeignKey("Banco")]
        public int? catalogo_banco_id { get; set; }
        public virtual Catalogo Banco { get; set; }

        [DisplayName("Tipo de Cuenta")]
        [ForeignKey("TipoCuenta")]
        public int? catalogo_tipo_cuenta_id { get; set; }
        public virtual Catalogo TipoCuenta { get; set; }

        [DisplayName("Empresa")]
        //[ForeignKey("Empresa")]
        public int? empresa_id { get; set; }
        //public virtual Empresa Empresa { get; set; }

        [DisplayName("Nacionalidad")]
        public int? PaisId { get; set; }
        public virtual Pais Pais { get; set; }

        [DisplayName("País de Nacimiento")]
        public int pais_pais_nacimiento_id { get; set; }

        [DisplayName("Régimen (Rotación):")]
        public int? AdminRotacionId { get; set; }
        public virtual AdminRotacion AdminRotacion { get; set; }

        [DisplayName("Proyecto")]
        [ForeignKey("Proyecto")]
        public int? ContratoId { get; set; }
        public virtual Catalogo Proyecto { get; set; }

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
        [ForeignKey("Encuadre")]
        public int? catalogo_encuadre_id { get; set; }
        public virtual Catalogo Encuadre { get; set; }

        [DisplayName("Encargado de personal:")]
        [ForeignKey("EncargadoPersonal")]
        public int? catalogo_encargado_personal_id { get; set; }
        public virtual Catalogo EncargadoPersonal { get; set; }

        [DisplayName("Validación Cédula")]
        public bool validacion_cedula { get; set; } = false;

        [CanBeNull]
        [DisplayName("Nombres Apellidos")]
        public string nombres_apellidos { get; set; }

        [DisplayName("Codigo Incapacidad")]
        [ForeignKey("CodigoIncapacidad")]
        public int? catalogo_codigo_incapacidad_id { get; set; }
        public virtual Catalogo CodigoIncapacidad { get; set; }

        [DisplayName("Sector")]
        [ForeignKey("Sector")]
        public int? catalogo_sector_id { get; set; }
        public virtual Catalogo Sector { get; set; }

        [DisplayName("Vía de Pago")]
        [ForeignKey("ViaPago")]
        public int? catalogo_via_pago_id { get; set; }
        public virtual Catalogo ViaPago { get; set; }

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

        public bool es_reingreso { get; set; } = false;


        [DataType(DataType.Date)]
        [DisplayName("Fecha Sustituto Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_carga_masiva { get; set; }

        [Obligado]
        [DisplayName("Tiene Ausentismo")]
        public bool tiene_ausentismo { get; set; } = false;

        public string codigo_seguridad_qr { get; set; }


        [DisplayName("ID Empleado SAP Local")]
        public int? empleado_id_sap_local { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Anulacion Alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_anulacion_alta { get; set; }


        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public bool es_visita { get; set; } = false;

        public string empresa_tercero { get; set; } = "";

        public virtual List<Capacitacion> Capacitaciones { get; set; }

        public virtual List<ColaboradorBaja> ColaboradorBajas { get; set; }



    }
}
