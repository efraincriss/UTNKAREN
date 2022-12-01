import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import CurrencyFormat from "react-currency-format";
import wrapForm from "../Base/BaseWrapper";

import moment from "moment";


import { Card } from "primereact_/card";
import { Button } from "primereact_/button";
import { Dropdown } from "primereact-v2/dropdown";
import { Checkbox } from "primereact_/checkbox";
import { Dialog } from "primereact/components/dialog/Dialog";
import { MultiSelect } from "primereact/components/multiselect/MultiSelect";
import Field from "../Base/Field-v2";
import { TabView, TabPanel } from "primereact_/tabview";
import PresupuestosLigados from "./PresupuestosLigados";
import VersionesLigadas from "./VersionesLigadas";
import OSLigadas from "./OSLigadas";
import ArchivosOrdenes from "./ArchivosList";
class DetalleOferta extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      errors: {},
      errorscarta: {},
      oferta: null,
      presupuestos: [],
      pos: [],
      versiones: [],
      vista: "details",
      action: "",
      formtittle: "",
      /** OfertaComercial */
      contratos: [],
      contratodescripcion: "",
      ContratoId: 0,
      ClaseId: 0,
      descripcion: "",
      codigo: "Por Generar",
      version: "A",
      tipo_trabajo: [],
      tipo_trabajo_id: 0,
      tipo_contratacion: [],
      tipo_contratacion_id: 0,
      centro_costos: [],
      centro_costos_id: 0,
      estado_oferta: [],
      estado_oferta_id: 0,
      estatus_ejecucion: [],
      estatus_ejecucion_id: 0,
      ingenieria: [],
      ingenieria_id: 0,
      alcance: 4172,
      alcances: [],
      codigo_shaya: "",
      estado: 0,
      service_request: false,
      service_order: false,
      revision: "",
      actacierreid: 0,
      OfertaPadreId: 0,
      IdOferta: 0, //I Oferta Comercial Principal,
      fecha_registro: null,
      comentarios: "",
      link_documentum: "",

      //Orden Proceder
      ordenProceder: false,
      link_ordenProdecer: "",
      fechaordenProceder: null,

      monto_editado: false,
      valor_os: 0.0,
      valor_of: 0,
      valor_os_actual: 0.0,
      valor_os_anterior: 0.0,
      /**Presupuesto */
      visiblepresupuesto: false,

      PresupuestoId: 0,
      proyectos: [],
      requerimientos: [],
      ProyectoId: 0,
      RequerimientoId: 0,

      /**Transmittal */
      colaboradores: [],
      visibletransmital: false,
      dirigido_a: [],
      copia_a: [],
      descripcionTransmittal: "",
      tipo_formatos: [
        { label: "Papel", value: "P" },
        { label: "Informático", value: "I" },
        { label: "Extranet", value: "X" },
      ],
      tipo_formato: "I",
      tipo_propositos: [
        { label: "Información", value: "PI" },
        { label: "Compras", value: "PC" },
        { label: "Aprobación", value: "PA" },
      ],
      tipo_proposito: "PA",
      tipos: [
        { label: "Minuta de Reunión", value: "MI" },
        { label: "Consulta Técnica", value: "TQ" },
        { label: "Procedimiento", value: "PR" },
        { label: "Carta", value: "LT" },
        { label: "Comercial", value: "CO" },
        { label: "Otros", value: "OT" },
      ],
      tipo: "CO",

      /* Nuevo User */
      cedula: "",
      apellidos: "",
      nombres: "",
      correo: "",
      ClienteId: 0,
      actionuser: "create",
      clientes: [],
      visibleuser: false,
      types: [],

      //

      ModelMontos: null,
      viewPresupuestoEmail: false,
      presupuestoSeleccionado: null
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.mostrarForm = this.mostrarForm.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind();
    this.isValid = this.isValid.bind();
    this.isValidCarta = this.isValidCarta.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormularioUser = this.EnviarFormularioUser.bind(this);
  }

  componentDidMount() {
    this.getCatalogos();
    this.getInfoOfertaAll(true);
    this.ObtenerMontosRequerimientos();
  }
  mostrarInfoPresupue = (row) => {
    console.log('presupuesto', row)
    this.setState({ viewPresupuestoEmail: true, presupuestoSeleccionado: row.Presupuesto ? row.Presupuesto : null })
  }
  ocultarInfoPresupue = () => {
    this.setState({ viewPresupuestoEmail: false })
  }

  getInfoOfertaAll = (init) => {
    axios
      .post(
        "/Proyecto/OfertaComercial/GetInformacionOferta?id=" +
        document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        let ofertaComercial = response.data;
        console.log("ofertaComercial", ofertaComercial);
        this.setState(
          {
            oferta: ofertaComercial,
            valor_os: ofertaComercial.monto_so_aprobado,
            valor_of: ofertaComercial.monto_ofertado_migracion_actual,
            valor_os_actual: ofertaComercial.monto_so_aprobado_migracion_actual,
            valor_os_anterior: ofertaComercial.monto_so_aprobado_migracion_anterior,
          },
          this.getPresupuestosLigados(ofertaComercial.Id, init)
        );
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  getInfoOferta = () => {
    axios
      .post(
        "/Proyecto/OfertaComercial/GetInformacionOferta?id=" +
        document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        let ofertaComercial = response.data;
        console.log("ofertaComercial", ofertaComercial);
        this.setState(
          {
            oferta: ofertaComercial,
            valor_os: ofertaComercial.monto_so_aprobado,
          },
          this.props.unlockScreen()
        );
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  getPresupuestosLigados = (Id, init) => {
    axios
      .post("/Proyecto/OfertaComercial/ListarPresupuesto?id=" + Id, {})
      .then((response) => {
        let result = response.data;
        console.log("presupuestos", result);
        this.setState({ presupuestos: result }, this.getVersiones(Id, init));
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  getVersiones = (Id, init) => {
    axios
      .post("/Proyecto/OfertaComercial/ListarVersiones?id=" + Id, {})
      .then((response) => {
        let result = response.data;
        this.setState({ versiones: result }, this.props.unlockScreen());
        this.ConsultarOS();
        if (
          this.state.oferta != null &&
          this.state.oferta.tienePresupuestoLigado
        ) {
          if (init) {
            this.openConfirmActualizarDatos();
          }
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  saveValueOsChange = () => {
    console.log('this.state.valor_os', this.state.valor_os);
    if (this.state.valor_os !== this.state.oferta.monto_so_aprobado) {
      this.props.blockScreen();
      axios
        .post("/proyecto/OfertaComercial/ActualizarMontoOS", {
          Id: this.state.oferta.Id,
          monto_aprobado: (this.state.valor_os == null || this.state.valor_os == '') ? 0 : this.state.valor_os,
        })
        .then((response) => {
          console.log("UpdateMonto", response);
          this.getInfoOferta();
        })
        .catch((error) => {
          console.log(error);
        });
    }
  };

  saveValueOsActual = () => {
    console.log('this.state.valor_os', this.state.valor_os_actual);
    if (this.state.valor_os_actual !== this.state.oferta.monto_so_aprobado_migracion_actual) {
      this.props.blockScreen();
      axios
        .post("/proyecto/OfertaComercial/ActualizarOSMigradoActual", {
          Id: this.state.oferta.Id,
          monto_so_aprobado_migracion_actual: (this.state.valor_os_actual == null || this.state.valor_os_actual == '') ? 0 : this.state.valor_os_actual,
        })
        .then((response) => {
          console.log("UpdateMonto", response);
          this.getInfoOferta();
        })
        .catch((error) => {
          console.log(error);
        });
    }
  };
  saveValueOsAnterior = () => {
    console.log('this.state.valor_osant', this.state.valor_os_anterior);
    if (this.state.valor_os_anterior !== this.state.oferta.monto_so_aprobado_migracion_anterior) {
      this.props.blockScreen();
      axios
        .post("/proyecto/OfertaComercial/ActualizarOSMigradoAnterior", {
          Id: this.state.oferta.Id,
          monto_so_aprobado_migracion_anterior: (this.state.valor_os_anterior == null || this.state.valor_os_anterior == '') ? 0 : this.state.valor_os_anterior,
        })
        .then((response) => {
          console.log("UpdateMonto", response);
          this.getInfoOferta();
        })
        .catch((error) => {
          console.log(error);
        });
    }
  };

  saveValueOfChange = () => {
    console.log('this.state.valor_of', this.state.valor_of);
    if (this.state.valor_of !== this.state.oferta.monto_ofertado_migracion_actual) {
      this.props.blockScreen();
      axios
        .post("/proyecto/OfertaComercial/ActualizarOfertaMigrado", {
          Id: this.state.oferta.Id,
          monto_ofertado_migracion_actual: (this.state.valor_of == null || this.state.valor_of == '') ? 0 : this.state.valor_of,
        })
        .then((response) => {
          console.log("UpdateMonto", response);
          this.getInfoOferta();
        })
        .catch((error) => {
          console.log(error);
        });
    }
  };
  changeMontoOs = (event) => {
    console.log("valor_os_anterio", this.state.valor_os);
    console.log(event);
    this.setState({ valor_os: event.value });
  };
  changeMontoOsActual = (event) => {
    console.log("valor_os_actual", this.state.valor_os_actual);
    console.log(event);
    this.setState({ valor_os_actual: event.value });
  };
  changeMontoOsAnterior = (event) => {
    console.log("valor_os_actual", this.state.valor_os_anterior);
    console.log(event);
    this.setState({ valor_os_anterior: event.value });
  };
  changeMontoOf = (event) => {
    console.log("valor_of_o", this.state.valor_of);
    console.log(event);
    this.setState({ valor_of: event.value });
  };



  eliminarPresupuestoLigado = (Id) => {
    axios
      .post("/proyecto/OfertaComercial/Eliminar", {
        Id: Id,
      })
      .then((response) => {
        console.log("ResponseEliminar");
        if ((response.data = "o")) {
          this.getInfoOferta();
          this.getPresupuestosLigados(this.state.oferta.Id, false);
          this.ObtenerMontosRequerimientos();
        }
      })
      .catch((error) => {
        console.log(error);
      });
  };
  getCatalogos = () => {
    //tipotrabajo
    axios
      .post("/proyecto/catalogo/GetCatalogo/1003", {})
      .then((response) => {
        console.log("TipoTrabajos", response.data);
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ tipo_trabajo: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //tipo contratacion
    axios
      .post("/proyecto/catalogo/GetCatalogo/2005", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ tipo_contratacion: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //Centro de Costos
    axios
      .post("/proyecto/catalogo/GetCatalogo/1004", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ centro_costos: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //estadooferta
    axios
      .post("/proyecto/catalogo/GetCatalogo/1005", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ estado_oferta: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //estatus ejecución
    axios
      .post("/proyecto/catalogo/GetCatalogo/1006", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ estatus_ejecucion: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //ingenieria
    axios
      .post("/proyecto/catalogo/GetCatalogo/3012", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ ingenieria: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //alcances
    axios
      .post("/proyecto/catalogo/GetCatalogo/3013", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ alcances: items });
      })
      .catch((error) => {
        console.log(error);
      });
    axios
      .post("/Proyecto/OfertaComercial/GetContratos", {})
      .then((response) => {
        console.log("contratos", response.data);
        var items = response.data.map((item) => {
          return {
            label: item.Codigo + " - " + item.objeto,
            dataKey: item.Id,
            value: item.Id,
          };
        });
        this.setState({ contratos: items });
      })
      .catch((error) => {
        console.log(error);
      });
  };
  handleSubmit = (event) => {
    event.preventDefault();
    if (this.state.ProyectoId == 0) {
      this.props.showWarn("Selecciona un proyecto");
    } else if (this.state.RequerimientoId == 0) {
      this.props.showWarn("Selecciona un Sr");
    } else {
      this.props.blockScreen();
      axios
        .post("/proyecto/OfertaComercial/CrearOfertaCPresupuesto", {
          OfertaComercialId: this.state.oferta.Id,
          RequerimientoId: this.state.RequerimientoId,
          PresupuestoId: 0,
          fecha_asignacion: new Date(),
          vigente: true,
        })
        .then((response) => {
          console.log(response.data);
          if (response.data == "creado") {
            this.props.showSuccess("Presupuesto Agregado Correctamente");
            this.getInfoOferta();
            this.getPresupuestosLigados(this.state.oferta.Id, true);
            this.setState(
              { visiblepresupuesto: false },
              this.ObtenerMontosRequerimientos()
            );
          } else if (response.data == "no_tiene_presupuesto_definitivo") {
            this.props.showWarn(
              "El Requerimiento no tiene un Presupuesto Definitivo"
            );
          } else {
            this.props.showWarn(response.data);
          }
        })
        .catch((error) => {
          console.log(error);
          this.props.showWarn("Existió un inconveniente inténtelo más tarde");
        });
    }
  };
  GenerarTransmital = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/CrearTransmitalOfertaComercial/", {
        Id: this.state.oferta.Id,
        ContratoId: 0,
        EmpresaId: 0,
        ClienteId: 0,
        OfertaComercialId: 0,
        fecha_emision: new Date(),
        tipo: this.state.tipo,
        tipo_formato: this.state.tipo_formato,
        tipo_proposito: this.state.tipo_proposito,
        descripcion: this.state.descripcionTransmittal,
        vigente: true,
        dirigido_a:
          this.state.dirigido_a != null && this.state.dirigido_a.length > 0
            ? this.state.dirigido_a.toString()
            : "0",
        copia_a:
          this.state.copia_a != null && this.state.copia_a.length > 0
            ? this.state.copia_a.toString()
            : "0",
        version: "",
        codigo_transmital: "000",
        codigo_carta: "",
        enviado_por: ".",
        vigente: true,
      })
      .then((response) => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Tranmisttal Generado");
          this.setState({ vista: "details" }, this.getInfoOferta());
        } else if (response.data == "EXISTE") {
          this.props.unlockScreen();
          this.props.showWarn(
            "Ya se generó un Transmittal para la Oferta Comercial"
          );
          this.setState({ visibletransmital: false });
        } else {
          this.props.unlockScreen();
          this.props.showWarn("Ocurrió un error inténtelo más tarde");
        }
      })
      .catch((error) => {
        console.log(error);
      });
  };
  ObtenerMontosRequerimientos = () => {
    axios
      .post(
        "/Proyecto/OfertaComercial/GetMontosRequerimientosOferta/" +
        document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        console.log("montos requerimientos", response.data);
        this.setState({
          ModelMontos: response.data,
        });
      })
      .catch((error) => {
        console.log(error);
      });
  };

  ConsultarOS = () => {
    axios
      .post(
        "/Proyecto/OfertaComercial/GetOSbyOfertaComercial/" +
        document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        console.log(response.data);
        this.setState({ pos: response.data });
      })
      .catch((error) => {
        console.log(error);
      });
  };
  isValid = () => {
    const errors = {};
    if (this.state.ordenProceder) {
      if (this.state.fechaordenProceder == "") {
        errors.codigo_orden_servicio = "Campo Requerido";
      }

    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  isValidDetalle = () => {
    const errors = {};

    if (this.state.ProyectoId == 0) {
      errors.ProyectoId = "Campo Requerido";
    }
    if (this.state.OfertaComercialId == 0) {
      errors.OfertaComercialId = "Campo Requerido";
    }
    if (this.state.GrupoItemId == 0) {
      errors.GrupoItemId = "Campo Requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };

  OcultarFormularioOrden = () => {
    this.setState({ po: null, action: "list" });
  };
  OcultarFormularioOrdenDetalle = () => {
    this.setState({ po: null, action: "list" });
    this.GetList();
  };
  GetList = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/OrdenServicio/FGetList", {})
      .then((response) => {
        console.log(response.data);
        this.setState({ data: response.data });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };

  GetDetalleOS = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/OrdenServicio/FDGetDetalleOs", {
        Id: Id,
      })
      .then((response) => {
        console.log(response.data);
        this.setState({ po: response.data });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };
  GetListDetalles = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/OrdenServicio/FGetOrdenHijos", { Id: Id })
      .then((response) => {
        console.log("hijos", response.data);
        this.setState({ detalles: response.data });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };
  GetCatalogs = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/OrdenServicio/FGetCatalogos", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ estados: items });
      })
      .catch((error) => {
        console.log(error);
      });
    axios
      .post("/Proyecto/OrdenServicio/FGetProyectos", {})
      .then((response) => {
        console.log("proyectos", response.data);
        this.setState({ proyectos: response.data });
      })
      .catch((error) => {
        console.log(error);
      });
    axios
      .post("/Proyecto/OrdenServicio/FGetOfertas", {})
      .then((response) => {
        console.log("ofertas", response.data);
        this.setState({ ofertas: response.data });
      })
      .catch((error) => {
        console.log(error);
      });
    axios
      .post("/Proyecto/OrdenServicio/FGetGrupos", {})
      .then((response) => {
        console.log("ofertas", response.data);
        this.setState({ grupos: response.data });
      })
      .catch((error) => {
        console.log(error);
      });
  };

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };

  Eliminar = (Id) => {
    console.log(Id);
    axios
      .post("/Proyecto/OrdenServicio/FGetDelete/", {
        Id: Id,
      })
      .then((response) => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente");

          this.GetList();
        } else if (response.data == "NOPUEDE") {
          this.props.showWarn("No se puedo Eliminar");
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Ocurrió un Error");
      });
  };
  EliminarDetalle = (Id) => {
    console.log(Id);
    console.log(this.state.po.Id);
    axios
      .post("/Proyecto/OrdenServicio/FDRemoveDetalle/", {
        Id: Id,
        OrdenServicioId: this.state.po.Id,
      })
      .then((response) => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente");
          this.setState({ dialogdetalles: false });
          this.GetListDetalles(this.state.po.Id);
          this.GetDetalleOS(this.state.po.Id);
        } else {
          this.props.showWarn(response.data);
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
      });
  };
  mostrarDetalles = (row) => {
    console.clear();
    console.log("Orden", row);

    if (row != null && row.Id > 0) {
      this.GetDetalleOS(row.Id);
      this.setState({ action: "detalles", detalles: [] });
      this.GetListDetalles(row.Id);
    }
  };

  mostrarForm = (accion) => {
    let data = this.state.oferta;
    console.log("action", accion);

    if (accion != null && accion === "version") {
      console.log("nueva version");
      this.setState({ fecha_registro: new Date() });
    }
    this.setState({
      vista: "form",
      action: accion,
      formtittle:
        accion === "edit"
          ? "Edición Oferta Comercial " +
          this.state.oferta.codigo +
          "_" +
          this.state.oferta.version
          : "Nueva Versión Oferta Comercial " +
          this.state.oferta.codigo +
          "_" +
          this.state.oferta.version,
    });
    if (data !== null) {
      this.setState({
        descripcion: data.descripcion,
        codigo: data.codigo,
        tipo_trabajo_id: data.tipo_Trabajo_Id,
        tipo_contratacion_id: data.forma_contratacion,
        centro_costos_id: data.centro_de_Costos_Id,
        estado_oferta_id: data.estado_oferta,
        estatus_ejecucion_id: data.estatus_de_Ejecucion,
        alcance: data.alcance,
        codigo_shaya: data.codigo_shaya,
        estado: data.estado,
        service_request: data.service_request,
        service_order: data.service_order,
        revision: data.revision,
        descripcion: data.descripcion,
        actacierreid: data.acta_cierre,
        OfertaPadreId: data.OfertaPadreId,
        version: data.version,
        ContratoId: data.ContratoId,
        fecha_registro: data.fecha_oferta,
        monto_ofertado: data.monto_ofertado,
        monto_aprobado: data.monto_so_aprobado,
        monto_pendiente_aprobacion: data.monto_ofertado_pendiente_aprobacion,
        comentarios: data.comentarios,
        link_documentum: data.link_documentum,
        monto_editado: data.monto_editado,
        link_ordenProceder: data.link_ordenProceder,
        ordenProceder: data.orden_proceder,
        fechaordenProceder: data.fecha_orden_proceder,
      });
    }
  };
  mostrarFormDetalle = (row) => {
    if (row != null && row.Id > 0) {
      console.log(row);
      this.setState({
        Idd: row.Id,
        ProyectoId: row.ProyectoId,
        GrupoItemId: row.GrupoItemId,
        valor_os: row.valor_os,
        OfertaComercialId: row.OfertaComercialId,
        action_detalles: "edit",
        dialogdetalles: true,
      });
    } else {
      console.log("New Form Detalles");
      this.setState({
        Idd: 0,
        ProyectoId: 0,
        GrupoItemId: 0,
        valor_os: 0,
        OfertaComercialId: 0,
        action_detalles: "create",
        dialogdetalles: true,
      });
    }
  };

  onDownload = (Id) => {
    return (window.location = `/Proyecto/TransmitalCabecera/Descargar/${Id}`);
  };

  generarBotones = (cell, row) => {
    return (
      <div>
        {row.tieneArchivo && (
          <button
            className="btn btn-outline-indigo btn-sm"
            style={{ marginRight: "0.2em" }}
            onClick={() => this.onDownload(row.ArchivoId)}
            data-toggle="tooltip"
            data-placement="top"
            title="Descargar Adjunto"
          >
            <i className="fa fa-cloud-download"></i>
          </button>
        )}

        <button
          className="btn btn-outline-success btn-sm"
          style={{ marginRight: "0.2em" }}
          onClick={() => this.mostrarDetalles(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Agregar Detalles"
        >
          <i className="fa fa-eye"></i>
        </button>
        <button
          className="btn btn-outline-info btn-sm"
          style={{ marginRight: "0.2em" }}
          onClick={() => this.mostrarForm(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger btn-sm"
          style={{ marginRight: "0.2em" }}
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.Eliminar(row.Id);
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };

  actualizarDatos = () => {
    this.props.blockScreen();
    axios
      .post("/proyecto/OfertaComercial/ActualizarDatos", {
        OfertaComercialId: this.state.oferta.Id,
        PresupuestoId: 0,
        fecha_asignacion: new Date(),
        vigente: true,
      })
      .then((response) => {
        if (response.data == "o") {
          this.props.showSuccess("Datos Actualizados");
          this.setState({
            visiblepresupuesto: false,
          });
          this.getInfoOferta();
        } else if (response.data == "ne") {
          this.props.showWarn(
            "Los Requerimientos Ligados No Estan En Estado Enviado"
          );
          this.props.unlockScreen();
        } else {
          this.props.unlockScreen();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  calcularmontos = () => {
    axios
      .get(
        "/Proyecto/OfertaComercial/GetMontos/" +
        document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        this.setState({
          monto_aprobado: response.data.monto_aprobado,
          monto_ofertado: response.data.monto_ofertado,
          monto_pendiente_aprobacion: response.data.monto_pendiente_aprobacion,

          monto_so_ingenieria: response.data.monto_ingenieria,
          monto_so_construccion: response.data.monto_construccion,
          monto_so_suministros: response.data.monto_suminitros,
          monto_so_subcontratos: response.data.monto_subcontratos,
          monto_so_total: response.data.monto_total_os,
        });
      })
      .catch((error) => {
        console.log(error);
      });
  };
  openConfirmActualizarDatos = () => {
    let _this = this;
    abp.message.confirm(
      "La Oferta Comercial " +
      _this.state.oferta.codigo +
      " tiene presupuestos Ligados. Esta acción actualizará los datos de la cabecera. ¿Desea Continuar?",
      "¿Actualizar Datos?",
      function (isConfirmed) {
        if (isConfirmed) {
          _this.actualizarDatos();
        }
      }
    );
  };

  generarBotonesDetalles = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info btn-sm"
          style={{ marginRight: "0.2em" }}
          onClick={() => this.mostrarFormDetalle(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger btn-sm"
          style={{ marginRight: "0.2em" }}
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.EliminarDetalle(row.Id);
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };

  MostrarFormularioPresupuesto = () => {
    this.setState({ visiblepresupuesto: true });
    this.getProyectos();
  };
  OcultarFormularioPresupuesto = () => {
    this.setState({ visiblepresupuesto: false });
  };
  handleChangeProyecto = (e) => {
    this.props.blockScreen();
    this.setState({ ProyectoId: e });
    console.log("entro evento" + e);
    this.setState(
      {
        ProyectoId: e,
        RequerimientoId: 0,
        requerimientos: [],
        PresupuestoId: 0,
      },
      this.getRequerimientos(e)
    );
  };
  getRequerimientos = (Id) => {
    axios
      .post("/Proyecto/Oferta/GetRequerimientosProyectoApi/" + Id, {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.descripcion,
            dataKey: item.Id,
            value: item.Id,
          };
        });

        this.setState({ requerimientos: items }, this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
      });
  };

  handleChangeRequerimientos = (e) => {
    this.setState({
      RequerimientoId: e,
      PresupuestoId: 0,
    });
  };

  getProyectos = () => {
    this.props.blockScreen();
    axios
      .post(
        "/Proyecto/Proyecto/GetProyectosApi/" + this.state.oferta.ContratoId,
        {}
      )
      .then((response) => {
        var items = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.nombre_proyecto,
            dataKey: item.Id,
            value: item.Id,
          };
        });

        this.setState({ proyectos: items }, this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
      });
  };

  DescargarMatriz = () => {
    this.props.blockScreen();
    axios
      .get(
        "/proyecto/OfertaComercial/GenerarPrespuesto?OfertaId=" +
        this.state.oferta.Id,
        { responseType: "arraybuffer" }
      )
      .then((response) => {
        console.log(response.headers["content-disposition"]);
        var nombre = response.headers["content-disposition"].split("=");

        const url = window.URL.createObjectURL(
          new Blob([response.data], {
            type:
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          })
        );
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", nombre[1]);
        document.body.appendChild(link);
        link.click();
        this.props.unlockScreen();
        this.props.showSuccess("Matriz Descargada");
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
        this.props.showWarn("Error al descargar la matriz");
      });
  };

  EnvioManual = () => {
    axios
      .post(
        "/Proyecto/OfertaComercial/GetMailto/" +
        document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        //this.setState({ mailto: response.data });
        window.location.href = response.data;
      })
      .catch((error) => {
        console.log(error);
      });
  };

  EnvioOrdenProceder = () => {
    axios
      .post(
        "/Proyecto/OfertaComercial/GetMailtoOrdenProceder/" +
        document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        //this.setState({ mailtoOrden: response.data });
        window.location.href = response.data;
      })
      .catch((error) => {
        console.log(error);
      });
  };
  MostrarFormularioTransmital = () => {
    if (this.state.oferta != null && !this.state.oferta.tieneTransmital) {
      this.setState({
        visibletransmital: true,
        vista: "formTransmittal",
        formtittle:
          "Transmittal para la Oferta " +
          this.state.oferta.codigo +
          "_" +
          this.state.oferta.version,
      });
      this.ConsultarColaboradores();
    } else {
      abp.notify.warn(
        "La Oferta Comercial ya tiene un transmittal generado",
        "AVISO"
      );
    }
  };
  ConsultarColaboradores() {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTransmitalUsers", {})
      .then((response) => {
        console.log(response.data);
        var colaborador = response.data.map((item) => {
          return {
            label:
              item.apellidos +
              " " +
              item.nombres +
              " ( " +
              item.nombre_cliente +
              " )",
            dataKey: item.Id,
            value: item.Id,
          };
        });
        this.setState(
          { colaboradores: colaborador },
          this.props.unlockScreen()
        );
      })
      .catch((error) => {
        this.props.unlockScreen();
        this.props.showWarn("Ocurrió un error al consultar  Colaboradores");
        console.log(error);
      });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTypes", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ types: items });
      })
      .catch((error) => {
        console.log(error);
      });
  }
  Redireccionar = () => {
    window.location.href = "/Proyecto/OfertaComercial/";
  };
  Anular = () => {
    this.props.blockScreen();
    axios
      .post("/proyecto/OfertaComercial/GetAnular", {
        Id: this.state.oferta.Id,
      })
      .then((response) => {
        if ((response.data = "o")) {
          this.props.showSuccess("Oferta Comercial Anulado");
          this.getInfoOferta();
          this.props.unlockScreen();
        } else {
          this.props.CancelarLoading();
          this.props.unlockScreen();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  Cancelar = () => {
    this.props.blockScreen();
    axios
      .post("/proyecto/OfertaComercial/GetCancelar", {
        Id: this.state.oferta.Id,
      })
      .then((response) => {
        if ((response.data = "o")) {
          this.props.showSuccess("Oferta Comercial Cancelada");
          this.getInfoOferta();
          this.props.unlockScreen();
        } else {
          this.props.unlockScreen();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  render() {


    if (this.state.vista === "details") {

      let totalOferta = this.state.oferta != null
        ? (this.state.oferta
          .monto_ofertado_migracion_actual + this.state.oferta.monto_ofertado)
        : 0;
      let totalSO =
        this.state.oferta != null
          ? this.state.oferta.monto_so_aprobado
          : 0;
      let pendienteAprobacion = (totalOferta - totalSO)
      return (
        <div className="row">
          <Card style={{ width: "100%" }} className="ui-card-shadow">
            <div className="row">
              <div className="col-12">
                <button
                  style={{ marginLeft: "0.3em" }}
                  className="btn btn-outline-primary btn-sm"
                  onClick={() => {
                    this.openConfirmActualizarDatos();
                  }}
                >
                  Actualizar Datos
                </button>
                <button
                  style={{ marginLeft: "0.3em" }}
                  className="btn btn-outline-primary btn-sm"
                  onClick={() => this.DescargarMatriz()}
                >
                  Oferta Económica
                </button>

                <a
                  href={
                    "/Proyecto/OfertaComercial/ListarWordOfertaComercial/" +
                    document.getElementById("OfertaComercialId").className
                  }
                  className="btn btn-outline-primary btn-sm"
                  style={{ marginLeft: "0.3em" }}
                >
                  {" "}
                  Doc. Oferta
                </a>
                {this.state.oferta != null &&
                  !this.state.oferta.tieneTransmital && (
                    <button
                      style={{ marginLeft: "0.3em" }}
                      className="btn btn-outline-primary btn-sm"
                      onClick={() => this.MostrarFormularioTransmital()}
                    >
                      Generar Transmital
                    </button>
                  )}
                <button
                  style={{ marginLeft: "0.3em" }}
                  className="btn btn-outline-primary btn-sm"
                  onClick={() => {
                    if (
                      window.confirm(
                        `Esta seguro continuar con el proceso de envio de la orden de Proceder, esta acción procesará su servidor de correos, ¿Desea continuar?`
                      )
                    )
                      this.EnvioOrdenProceder();
                  }}
                >
                  Envio O. Proceder
                </button>
                <button
                  style={{ marginLeft: "0.3em" }}
                  className="btn btn-outline-primary btn-sm"
                  onClick={() => {
                    if (
                      window.confirm(
                        `Esta seguro continuar con el proceso de envio de la oferta, esta acción procesará su servidor de correos y actualizará el estado de la Oferta a Presentado, ¿Desea continuar?`
                      )
                    )
                      this.EnvioManual();
                  }}
                >
                  Envio Manual
                </button>
                <button
                  style={{ marginLeft: "0.3em" }}
                  className="btn btn-outline-primary btn-sm"
                  onClick={() => this.Anular()}
                >
                  Anular
                </button>
                <button
                  style={{ marginLeft: "0.3em" }}
                  className="btn btn-outline-primary btn-sm"
                  onClick={() => this.Cancelar()}
                >
                  Cancelar
                </button>
                <button
                  style={{ marginLeft: "0.3em" }}
                  className="btn btn-outline-primary btn-sm"
                  onClick={() => {
                    this.mostrarForm("edit");
                  }}
                >
                  Editar
                </button>
                <button
                  style={{ marginLeft: "0.3em" }}
                  className="btn btn-outline-primary btn-sm"
                  onClick={() => this.Redireccionar()}
                >
                  Regresar
                </button>
              </div>
            </div>
            <hr />
            <div className="row">
              <div className="col-6">
                <h6>
                  <b>Contrato:</b>{" "}
                  {this.state.oferta != null
                    ? this.state.oferta.codigoContrato
                    : ""}
                </h6>
                <h6>
                  <b>Código:</b>{" "}
                  {this.state.oferta != null ? this.state.oferta.codigo : ""}
                </h6>
                <h6>
                  <b>Descripción:</b>{" "}
                  {this.state.oferta != null
                    ? this.state.oferta.descripcion
                    : ""}
                </h6>
                <h6>
                  <b>Versión:</b>{" "}
                  {this.state.oferta != null ? this.state.oferta.version : ""}
                </h6>
                <h6>
                  <b>¿Es la versión final ? :</b>{" "}
                  {this.state.oferta != null && this.state.oferta.es_final
                    ? this.state.oferta.es_final === 1
                      ? "SI"
                      : "NO"
                    : ""}
                </h6>
                <h6>
                  <b>¿Tiene orden de Proceder ? :</b>{" "}
                  {this.state.oferta != null && this.state.oferta.orden_proceder
                    ? "SI"
                    : "NO"}
                </h6>

              </div>
              <div className="col-6">
                <h6>
                  <b>Estado Oferta Comercial: </b>{" "}
                  {this.state.oferta != null
                    ? this.state.oferta.nombreEstadoOferta
                    : ""}
                </h6>
                <h6>
                  <b>Fecha Oferta: </b>{" "}
                  {this.state.oferta != null
                    ? this.state.oferta.fechaOferta
                    : ""}
                </h6>
                <h6>
                  <b>Fecha Ultimo Envío: </b>{" "}
                  {this.state.oferta != null
                    ? this.state.oferta.fechaUltimoEnvio
                    : ""}
                </h6>

                <h6>
                  <b>¿Tiene Transmital ? :</b>{" "}
                  {this.state.oferta != null &&
                    this.state.oferta.tieneTransmital ? (
                    <strong>
                      <a
                        href={
                          "/Proyecto/TransmitalCabecera/Details/" +
                          this.state.oferta.TransmitalId +
                          "?id2=" +
                          this.state.oferta.Id
                        }
                      >
                        SI ({this.state.oferta.codigoTransmittal})
                      </a>
                    </strong>
                  ) : (
                    <a>NO</a>
                  )}
                </h6>
                {this.state.oferta != null &&
                  this.state.oferta.link_documentum !== null && (
                    <h6>
                      <b>
                        Doc. Compartido (la URL no debe incluir tildes en los
                        nombres en las carpetas) :
                      </b>{" "}
                      <strong>
                        <a
                          href={this.state.oferta.link_documentum}
                          target="_blank"
                        >
                          {this.state.oferta.link_documentum}
                        </a>
                      </strong>
                    </h6>
                  )}
              </div>
            </div>
            <div className="row" style={{ fontSize: "16px" }}>
              {false && (
                <>
                  <div className="col-3">
                    <div className="callout callout-info">
                      <small className="text-muted">Monto Ofertado</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={
                            this.state.oferta != null
                              ? this.state.oferta.monto_ofertado
                              : 0
                          }
                          displayType={"text"}
                          thousandSeparator={true}
                          decimalScale={2}
                          prefix={"$"}
                          style={{ fontSize: "16px" }}
                        />
                      </strong>

                    </div>
                  </div>
                  <div className="col-3">
                    <div className="callout callout-info">
                      <small className="text-muted">Monto Oferta Migrado</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          onBlur={() => {
                            this.saveValueOfChange();
                          }}

                          value={this.state.valor_of}
                          onValueChange={(event) => this.changeMontoOf(event)}

                          decimalScale={2}
                          displayType={"input"}
                          thousandSeparator={true}
                          prefix={"$"}
                          style={{ fontSize: "16px" }}
                        />
                      </strong>
                    </div>
                  </div>
                </>
              )
              }
              <div className="col-3">

                <div className="callout callout-default">
                  <small className="text-muted">Monto Ofertado</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        totalOferta}
                      displayType={"text"}
                      decimalScale={2}
                      thousandSeparator={true}
                      prefix={"$"}
                      style={{ fontSize: "16px", fontWeight: "bold" }}
                    />
                  </strong>

                </div>



              </div>
              <div className="col-3">
                <div className="callout callout-warning">
                  <small className="text-muted">
                    {this.state.oferta !== null &&
                      this.state.oferta.monto_editado
                      ? "Monto Aprobado (Actualizado)"
                      : "Monto Aprobado"}
                  </small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      onBlur={() => {
                        this.saveValueOsChange();
                      }}

                      value={this.state.valor_os}
                      onValueChange={(event) => this.changeMontoOs(event)}
                      displayType={
                        this.state.oferta != null &&
                          !this.state.oferta.puedeEditarMontoAprobado
                          ? "text"
                          : "input"
                      }
                      style={{
                        color:
                          this.state.oferta != null &&
                            this.state.oferta.monto_editado
                            ? "red"
                            : "black",
                        fontSize: "16px"
                      }}
                      thousandSeparator={true}
                      prefix={"$"}
                      decimalScale={2}
                    />

                  </strong>
                </div>
              </div>
              <div className="col-3">
                <div className="callout callout-danger">
                  <small className="text-muted">Monto P. Aprobación</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      /* value={
                         this.state.oferta != null
                           ? this.state.oferta
                             .monto_ofertado_pendiente_aprobacion
                           : 0
                       }*/
                      value={pendienteAprobacion}
                      displayType={"text"}
                      decimalScale={2}
                      style={{ fontSize: "16px" }}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
            </div>
            <div className="row" style={{ fontSize: "16px" }}>
             
              {/* <div className="col-3">
                <div className="callout callout-warning">
                  <small className="text-muted">Monto SO Migrado</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat

                      onBlur={() => {
                        this.saveValueOsActual();
                      }}

                      value={this.state.valor_os_actual}
                      onValueChange={(event) => this.changeMontoOsActual(event)}
                      displayType={"input"}
                      decimalScale={2}
                      thousandSeparator={true}
                      style={{ fontSize: "16px" }}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
              <div className="col-3">
                <div className="callout callout-warning">
                  <small className="text-muted">Monto SO Migrado Anterior</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      onBlur={() => {
                        this.saveValueOsAnterior();
                      }}

                      value={this.state.valor_os_anterior}
                      onValueChange={(event) => this.changeMontoOsAnterior(event)}
                      displayType={"input"}
                      decimalScale={2}
                      thousandSeparator={true}
                      style={{ fontSize: "16px" }}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
              <div className="col-3">
                <div className="callout callout-default">
                  <small className="text-muted">Total SO</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={totalSO}
                      displayType={"text"}
                      thousandSeparator={true}
                      decimalScale={2}
                      prefix={"$"}
                      style={{ fontSize: "16px", fontWeight: "bold" }}
                    />
                  </strong>
                </div>
              </div>
               */}
            </div>

            <div className="row" style={{ fontSize: "16px" }}>

              
            </div>
            {this.state.ModelMontos != null && !this.state.ModelMontos.success && (
              <div>
                {" "}
                <label>Montos Requerimientos</label>
                <div className="row">
                  <div className="col">
                    <div className="callout callout-primary">
                      <small className="text-muted">Ingenieria</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={
                            this.props.ModelMontos != null
                              ? this.props.ModelMontos.monto_ingenieria
                              : 0
                          }
                          displayType={"text"}
                          style={{ fontSize: "16px" }}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>
                  <div className="col">
                    <div className="callout callout-success">
                      <small className="text-muted">Construcción</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={
                            this.props.ModelMontos != null
                              ? this.props.ModelMontos.monto_contruccion
                              : 0
                          }
                          displayType={"text"}
                          style={{ fontSize: "16px" }}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>
                  <div className="col">
                    <div className="callout callout-info">
                      <small className="text-muted">Suministros</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={
                            this.props.ModelMontos != null
                              ? this.props.ModelMontos.monto_suministros
                              : 0
                          }
                          displayType={"text"}
                          style={{ fontSize: "16px" }}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>
                  <div className="col">
                    <div className="callout callout-warning">
                      <small className="text-muted">Subcontratos</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={
                            this.props.ModelMontos != null
                              ? this.props.ModelMontos.monto_subcontratos
                              : 0
                          }
                          displayType={"text"}
                          style={{ fontSize: "16px" }}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>

                  <div className="col">
                    <div className="callout callout-danger">
                      <small className="text-muted">Total</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={
                            this.props.ModelMontos != null
                              ? this.props.ModelMontos.monto_total
                              : 0
                          }
                          displayType={"text"}
                          style={{ fontSize: "16px" }}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>
                </div>
              </div>
            )}
            <hr />
            <TabView className="tabview-custom">
              <TabPanel header="Requerimientos">
                {this.state.oferta != null && this.state.oferta.Id > 0 && (
                  <div>
                    <div className="col" align="right">
                      <button
                        className="btn btn-outline-primary"
                        onClick={() => this.MostrarFormularioPresupuesto()}
                      >
                        Seleccionar SR
                      </button>
                    </div>
                    <PresupuestosLigados
                      presupuestos={this.state.presupuestos}
                      showSuccess={this.showSuccess}
                      showWarning={this.showWarning}
                      showWarn={this.showWarn}
                      blockScreen={this.blockScreen}
                      unlockScreen={this.unlockScreen}
                      Eliminar={this.eliminarPresupuestoLigado}
                      mostrarInfoPresupue={this.mostrarInfoPresupue}
                      oculatarInfoPresupue={this.ocultarInfoPresupue}
                    />
                  </div>
                )}
              </TabPanel>
              <TabPanel header="POs">
                <OSLigadas
                  data={this.state.pos}
                  showSuccess={this.showSuccess}
                  showWarning={this.showWarning}
                  showWarn={this.showWarn}
                  blockScreen={this.blockScreen}
                  unlockScreen={this.unlockScreen}
                />
              </TabPanel>
              <TabPanel header="Versiones">
                {this.state.oferta != null && this.state.oferta.Id > 0 && (
                  <div>
                    <div className="col" align="right">
                      <button
                        className="btn btn-outline-primary"
                        onClick={() => this.mostrarForm("version")}
                      >
                        Nueva Versión
                      </button>
                    </div>

                    <VersionesLigadas
                      versiones={this.state.versiones}
                      showSuccess={this.showSuccess}
                      showWarning={this.showWarning}
                      showWarn={this.showWarn}
                      blockScreen={this.blockScreen}
                      unlockScreen={this.unlockScreen}
                    />
                  </div>
                )}
              </TabPanel>
              <TabPanel header="Archivos Ordenes de Proceder">
                {this.state.oferta != null && this.state.oferta.Id > 0 && (
                  <div>
                    <ArchivosOrdenes
                      showSuccess={this.props.showSuccess}
                      showWarning={this.props.showWarning}
                      showWarn={this.props.showWarn}
                      blockScreen={this.props.blockScreen}
                      unlockScreen={this.props.unlockScreen}
                    />
                  </div>
                )}
              </TabPanel>
            </TabView>
          </Card>
          <Dialog
            modal={true}
            header="Selección de Presupuesto"
            visible={this.state.visiblepresupuesto}
            width="70%"
            onHide={this.OcultarFormularioPresupuesto}
          >
            <form onSubmit={this.handleSubmit} style={{ height: "300px" }}>
              <div className="form-group">
                <label htmlFor="label">Proyecto</label>
                <br />
                <Dropdown
                  value={this.state.ProyectoId}
                  options={this.state.proyectos}
                  onChange={(e) => {
                    this.handleChangeProyecto(e.value);
                  }}
                  filter={true}
                  filterPlaceholder="Seleccione un Proyecto"
                  filterBy="label,value"
                  placeholder="Seleccione un Proyecto"
                  style={{ width: "95%" }}
                />
                <br />
              </div>

              <div className="form-group">
                <label htmlFor="label">SR (Service Request)</label>
                <Dropdown
                  value={this.state.RequerimientoId}
                  options={this.state.requerimientos}
                  onChange={(e) => {
                    this.setState({ RequerimientoId: e.value }),
                      this.handleChangeRequerimientos;
                  }}
                  filter={true}
                  filterPlaceholder="Seleccione un Requerimiento"
                  filterBy="label,value"
                  placeholder="Seleccione un requerimiento"
                  style={{ width: "95%" }}
                />
                <br />
              </div>

              <button
                type="submit"
                className="btn btn-outline-primary"
                style={{ marginLeft: "0.3em" }}
              >
                Guardar
              </button>
              <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                style={{ marginLeft: "0.3em" }}
                onClick={this.OcultarFormularioPresupuesto}
              >
                Cancelar
              </button>
            </form>
          </Dialog>


          <Dialog
            modal={true}
            header="Confirmación Mail"
            visible={this.state.viewPresupuestoEmail}
            width="400px"
            onHide={this.ocultarInfoPresupue}
          >
            <div className="row">
              <div className="col">
                <label><strong>Asunto:</strong></label><br />

                <label>{this.state.presupuestoSeleccionado != null ? this.state.presupuestoSeleccionado.asuntoCorreo : ""}</label>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <label><strong>Descripción</strong></label><br></br>
                <label>{this.state.presupuestoSeleccionado != null ? this.state.presupuestoSeleccionado.descripcionCorreo : ""}</label>
              </div>
            </div>
          </Dialog>

        </div>
      );
    } else if (this.state.vista === "form") {
      return (
        <Card title={this.state.formtittle}>
          <form onSubmit={this.EnviarFormulario}>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label htmlFor="label">Contrato</label>
                  <Dropdown
                    value={this.state.ContratoId}
                    options={this.state.contratos}
                    onChange={(e) => {
                      this.setState({ ContratoId: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona un Contrato"
                    filterBy="label,value"
                    placeholder="Selecciona un Contrato"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label>Fecha Oferta</label>
                  <input
                    type="datetime-local"
                    id="no-filter"
                    name="fecha_registro"
                    className="form-control"
                    onChange={this.handleChange}
                    //value={moment(this.state.fecha_registro).format(
                    // "YYYY-MM-DD"
                    // )}
                    value={this.state.fecha_registro}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Centro de Costos</label>
                  <Dropdown
                    value={this.state.centro_costos_id}
                    options={this.state.centro_costos}
                    onChange={(e) => {
                      this.setState({ centro_costos_id: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Centro de Costos"
                    filterBy="label,value"
                    placeholder="Selecciona Centro de Costos"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Alcance</label>
                  <Dropdown
                    value={this.state.alcance}
                    options={this.state.alcances}
                    onChange={(e) => {
                      this.setState({ alcance: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona un Alcance"
                    filterBy="label,value"
                    placeholder="Selecciona un Alcance"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Forma Contratación</label>
                  <Dropdown
                    value={this.state.tipo_contratacion_id}
                    options={this.state.tipo_contratacion}
                    onChange={(e) => {
                      this.setState({ tipo_contratacion_id: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Tipo Contratación"
                    filterBy="label,value"
                    placeholder="Selecciona Tipo Contratación"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
                <div className="form-group">
                  <label>Código SO Shaya</label>
                  <input
                    type="text"
                    name="codigo_shaya"
                    className="form-control"
                    onChange={this.handleChange}
                    value={this.state.codigo_shaya}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Estado</label>
                  <select
                    onChange={this.handleChange}
                    className="form-control"
                    name="estado"
                  >
                    <option value="1"> Activo</option>
                    <option value="0"> Inactivo</option>
                  </select>
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="label">Tipo de Trabajo</label>
                  <Dropdown
                    value={this.state.tipo_trabajo_id}
                    options={this.state.tipo_trabajo}
                    onChange={(e) => {
                      this.setState({ tipo_trabajo_id: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Tipo Trabajo"
                    filterBy="label,value"
                    placeholder="Selecciona Tipo Trabajo"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>

                <div className="form-group">
                  <label>Descripción</label>
                  <input
                    type="text"
                    name="descripcion"
                    className="form-control"
                    onChange={this.handleChange}
                    value={this.state.descripcion}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Estatus de Ejecución</label>
                  <Dropdown
                    value={this.state.estatus_ejecucion_id}
                    options={this.state.estatus_ejecucion}
                    onChange={(e) => {
                      this.setState({ estatus_ejecucion_id: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Estatus E"
                    filterBy="label,value"
                    placeholder="Selecciona Estatus E"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Acta de Cierre</label>
                  <select
                    onChange={this.handleChange}
                    className="form-control"
                    name="actacierreid"
                  >
                    <option value="0">No</option>
                    <option value="1">Si</option>
                  </select>
                </div>
                <div className="form-group">
                  <label htmlFor="label">Estatus del Proceso</label>
                  <Dropdown
                    value={this.state.estado_oferta_id}
                    options={this.state.estado_oferta}
                    onChange={(e) => {
                      this.setState({ estado_oferta_id: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Estatus P"
                    filterBy="label,value"
                    placeholder="Selecciona Estatus P"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label>Comentarios</label>
                  <textarea
                    type="text"
                    name="comentarios"
                    className="form-control"
                    onChange={this.handleChange}
                    value={this.state.comentarios}
                  />
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label>Carpeta Compartida</label>
                  <input
                    type="text"
                    name="link_documentum"
                    className="form-control"
                    onChange={this.handleChange}
                    value={this.state.link_documentum}
                  />
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <Checkbox
                  checked={this.state.service_request}
                  onChange={(e) =>
                    this.setState({ service_request: e.checked })
                  }
                />{" "}
                &nbsp; Service Request
              </div>
              <div className="col">
                <Checkbox
                  checked={this.state.service_order}
                  onChange={(e) => this.setState({ service_order: e.checked })}
                />{" "}
                &nbsp;Change Order
              </div>
            </div>
            <hr></hr>
            <div className="row">
              <div className="col">
                <Checkbox
                  checked={this.state.ordenProceder}
                  onChange={(e) => this.onCheckOrdenProceder(e.checked)}
                />{" "}
                &nbsp; Tiene Orden Proceder
              </div>
              <div className="col">
                {this.state.ordenProceder && (
                  <Field
                    name="fechaordenProceder"
                    label="Fecha Orden Proceder"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fechaordenProceder}
                    onChange={this.handleChange}
                    error={this.state.errors.fechaordenProceder}
                  />
                )}
              </div>
            </div>

            <hr></hr>
            <button
              type="submit"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-folder-open"
              style={{ marginRight: "0.3em" }}
            >
              Guardar
            </button>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.onHide}
            >
              Cancelar
            </button>
          </form>
        </Card>
      );
    } else if (this.state.vista === "formTransmittal") {
      return (
        <Card title={this.state.formtittle}>
          <div>
            <div className="row">
              <div className="col">
                <Field
                  name="tipo_formato"
                  value={this.state.tipo_formato}
                  label="Formato"
                  options={this.state.tipo_formatos}
                  type={"select"}
                  filter={true}
                  required
                  onChange={this.onChangeValue}
                  error={this.state.errors.tipo_formato}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
              <div className="col">
                <Field
                  name="tipo_proposito"
                  value={this.state.tipo_proposito}
                  label="Propósito"
                  options={this.state.tipo_propositos}
                  type={"select"}
                  filter={true}
                  required
                  onChange={this.onChangeValue}
                  error={this.state.errors.tipo_proposito}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
            </div>
            <div className="row">
              <div className="col">
                <Field
                  name="tipo"
                  value={this.state.tipo}
                  label="Tipo"
                  options={this.state.tipos}
                  type={"select"}
                  filter={true}
                  required
                  onChange={this.onChangeValue}
                  error={this.state.errors.tipo}
                  readOnly={false}
                  placeholder="Seleccione.."
                  filterPlaceholder="Seleccione.."
                />
              </div>
              <div className="col">
                <Field
                  name="descripcionTransmittal"
                  label="Descripción Transmittal"
                  required
                  edit={true}
                  readOnly={false}
                  value={this.state.descripcionTransmittal}
                  onChange={this.handleChange}
                  error={this.state.errors.nombres}
                />
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label>Drigido a</label>
                  <MultiSelect
                    value={this.state.dirigido_a}
                    options={this.state.colaboradores}
                    onChange={(e) => this.setState({ dirigido_a: e.value })}
                    style={{ width: "100%" }}
                    filter={true}
                    defaultLabel="Selecciona  Usuarios"
                    className="form-control"
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Con Copia a</label>
                  <MultiSelect
                    value={this.state.copia_a}
                    options={this.state.colaboradores}
                    onChange={(e) => this.setState({ copia_a: e.value })}
                    style={{ width: "100%" }}
                    filter={true}
                    defaultLabel="Selecciona  Usuarios"
                    className="form-control"
                  />
                </div>
              </div>
            </div>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-folder-open"
              style={{ marginRight: "0.3em" }}
              onClick={this.GenerarTransmital}
            >
              {" "}
              Guardar
            </button>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={() => this.setState({ vista: "details" })}
            >
              Cancelar
            </button>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.mostrarFormUser}
            >
              Añadir Usuario
            </button>
          </div>
          <Dialog
            header="Nuevo"
            visible={this.state.visibleuser}
            onHide={this.OcultarFormularioUser}
            modal={true}
            style={{ width: "500px", overflow: "auto" }}
          >
            <div>
              <form onSubmit={this.EnviarFormularioUser}>
                <div className="row">
                  <div className="col">
                    <Field
                      name="cedula"
                      label="Identificación"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.cedula}
                      onChange={this.handleChange}
                      error={this.state.errorscarta.cedula}
                    />
                  </div>
                  <div className="col">
                    <Field
                      name="ClienteId"
                      required
                      value={this.state.ClienteId}
                      label="Usuario"
                      options={this.state.types}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValue}
                      error={this.state.errorscarta.ClienteId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col">
                    <Field
                      name="apellidos"
                      label="Apellidos"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.apellidos}
                      onChange={this.handleChange}
                      error={this.state.errorscarta.apellidos}
                    />
                  </div>
                  <div className="col">
                    <Field
                      name="nombres"
                      label="Nombres"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.nombres}
                      onChange={this.handleChange}
                      error={this.state.errorscarta.nombres}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col">
                    <Field
                      name="correo"
                      label="Correo Electrónico"
                      required
                      edit={true}
                      readOnly={false}
                      value={this.state.correo}
                      onChange={this.handleChange}
                      error={this.state.errorscarta.correo}
                    />
                  </div>
                </div>
                <button type="submit" className="btn btn-outline-primary">
                  Guardar
                </button>
                &nbsp;
                <button
                  type="button"
                  className="btn btn-outline-primary"
                  icon="fa fa-fw fa-ban"
                  onClick={this.OcultarFormularioUser}
                >
                  Cancelar
                </button>
              </form>
            </div>
          </Dialog>
        </Card>
      );
    } else {
      return <div>Error</div>;
    }
  }

  OcultarFormularioUser = () => {
    this.setState({ visibleuser: false });
  };

  isValidCarta = () => {
    const errorscarta = {};

    if (this.state.cedula == "") {
      errorscarta.cedula = "Campo Requerido";
    }
    if (this.state.ClienteId == 0) {
      errorscarta.ClienteId = "Campo Requerido";
    }
    if (this.state.apellidos == "") {
      errorscarta.apellidos = "Campo Requerido";
    }
    if (this.state.nombres == "") {
      errorscarta.nombres = "Campo Requerido";
    }
    if (this.state.correo == "") {
      errorscarta.correo = "Campo Requerido";
    }

    this.setState({ errorscarta });
    return Object.keys(errorscarta).length === 0;
  };
  EnviarFormularioUser = (event) => {
    event.preventDefault();

    if (!this.isValidCarta()) {
      return;
    } else {
      if (this.state.actionuser == "create") {
        axios
          .post("/Proyecto/TransmitalCabecera/ObtenerCrear", {
            Id: 0,
            cedula: this.state.cedula,
            apellidos: this.state.apellidos,
            nombres: this.state.nombres,
            correo: this.state.correo,
            ClienteId: this.state.ClienteId,
          })
          .then((response) => {
            if (response.data == "OK") {
              this.props.showSuccess("Guardado Correctamente");
              this.setState({ visibleuser: false });
              this.ConsultarColaboradores();
            } else if (response.data == "MISMO") {
              this.props.showWarn(
                "Ya existe un usuario con la misma identificación"
              );
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.showWarn("Ocurrió un Error");
          });
      } else {
        axios
          .post("/Proyecto/TransmitalCabecera/ObtenerEditar", {
            Id: this.state.Id,
            cedula: this.state.cedula,
            apellidos: this.state.apellidos,
            nombres: this.state.nombres,
            correo: this.state.correo,
            ClienteId: this.state.ClienteId,
          })
          .then((response) => {
            if (response.data == "OK") {
              this.props.showSuccess("Guardado Correctamente");
              this.setState({ visible: false });
              this.GetList();
            } else if (response.data == "MISMO") {
              this.props.showWarn(
                "Ya existe un usuario con la misma identificación"
              );
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.showWarn("Ocurrió un Error");
            this.StopLoading();
          });
      }
    }
  };
  mostrarFormUser = () => {
    this.setState({
      cedula: "",
      apellidos: "",
      nombres: "",
      correo: "",
      ClienteId: 0,
      actionuser: "create",
      visibleuser: true,
    });
  };
  RedireccionarTransmital = (event) => {
    event.preventDefault();
    alert("totransmittal");
  };
  EnviarFormulario = (event) => {
    event.preventDefault();
    this.props.blockScreen();

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      this.props.unlockScreen();
      return;
    } else {
      if (this.state.action === "edit") {
        console.log("OrdeProceder", this.state.ordenProceder);
        axios
          .post("/proyecto/OfertaComercial/EditarOfertaComercial", {
            TransmitalId: this.state.oferta.TransmitalId,
            estado: this.state.estado,
            descripcion: this.state.descripcion,
            service_request: this.state.service_request,
            service_order: this.state.service_order,
            version: this.state.version,
            codigo: this.state.codigo,
            alcance: this.state.alcance,
            es_final: this.state.oferta.es_final,
            vigente: true,
            OfertaPadreId: this.state.OfertaPadreId,
            tipo_Trabajo_Id: this.state.tipo_trabajo_id,
            centro_de_Costos_Id: this.state.centro_costos_id,
            estatus_de_Ejecucion: this.state.estatus_ejecucion_id,
            codigo_shaya: this.state.codigo_shaya,
            revision_Oferta: this.state.revision,
            forma_contratacion: this.state.tipo_contratacion_id,
            acta_cierre: this.state.actacierreid,
            computo_completo: false,
            Id: this.state.oferta.Id,
            ContratoId: this.state.ContratoId,
            fecha_oferta: this.state.fecha_registro,
            comentarios: this.state.comentarios,
            estado_oferta: this.state.estado_oferta_id,
            link_documentum: this.state.link_documentum,
            monto_editado: this.state.monto_editado,
            monto_ofertado: this.state.oferta.monto_ofertado,
            monto_so_aprobado: this.state.oferta.monto_so_aprobado,
            monto_ofertado_pendiente_aprobacion:
              this.state.oferta.monto_ofertado -
              this.state.oferta.monto_so_aprobado,
            fecha_ultimo_envio: this.state.oferta.fecha_ultimo_envio,
            fecha_primer_envio: this.state.oferta.fecha_primer_envio,
            orden_proceder: this.state.ordenProceder,
            fecha_orden_proceder: this.state.fechaordenProceder,
            link_ordenProceder: this.state.link_ordenProdecer,
          })
          .then((response) => {
            if (response.data == "e") {
              this.props.showWarn(
                "Existió un inconveniente inténtelo mas tarde"
              );
              this.props.unlockScreen();
            } else {
              this.props.showSuccess("Editado Correctamente");
              this.getInfoOferta();
              this.setState({ vista: "details" });
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.unlockScreen();
          });
      } else {
        axios
          .post("/proyecto/OfertaComercial/CrearNuevaVersion", {
            TransmitalId: this.state.oferta.TransmitalId,
            estado: this.state.estado,
            descripcion: this.state.descripcion,
            service_request: this.state.service_request,
            service_order: this.state.service_order,
            version: this.state.version,
            codigo: this.state.codigo,
            alcance: this.state.alcance,
            es_final: this.state.oferta.es_final,
            vigente: true,
            OfertaPadreId: this.state.OfertaPadreId,
            tipo_Trabajo_Id: this.state.tipo_trabajo_id,
            centro_de_Costos_Id: this.state.centro_costos_id,
            estatus_de_Ejecucion: this.state.estatus_ejecucion_id,
            codigo_shaya: this.state.codigo_shaya,
            revision_Oferta: this.state.revision,
            forma_contratacion: this.state.tipo_contratacion_id,
            acta_cierre: this.state.actacierreid,
            computo_completo: false,
            Id: this.state.oferta.Id,
            ContratoId: this.state.oferta.ContratoId,
            fecha_oferta: this.state.fecha_registro,
            comentarios: this.state.comentarios,
            estado_oferta: this.state.estado_oferta_id,
            link_documentum: this.state.link_documentum,
            monto_editado: this.state.monto_editado,
            orden_proceder: this.state.ordenProceder,
            fecha_orden_proceder: this.state.fechaordenProceder,
            link_ordenProceder: this.state.link_ordenProdecer,
          })
          .then((response) => {
            if (response.data == "e") {
              this.props.showWarn(
                "Existió un inconveniente inténtelo mas tarde"
              );
              this.props.unlockScreen();
            } else {
              this.props.showSuccess("Versión Creada Correctamente");
              this.getInfoOfertaAll(false);
              this.setState({ vista: "details" });
            }
          })
          .catch((error) => {
            console.log(error);
            this.props.unlockScreen();
          });
      }
    }
  };
  EnviarFormularioDetalle = (event) => {
    event.preventDefault();
    this.props.blockScreen();

    if (!this.isValidDetalle()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      this.props.unlockScreen();
      return;
    } else {
      if (this.state.action_detalles == "create") {
        axios
          .post("/Proyecto/OrdenServicio/FDCreateDetalle", {
            Id: 0,
            OrdenServicioId: this.state.po.Id,
            ProyectoId: this.state.ProyectoId,
            GrupoItemId: this.state.GrupoItemId,
            valor_os: this.state.valor_os,
            OfertaComercialId: this.state.OfertaComercialId,
            vigente: true,
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("Guardado", "Correcto");
              this.setState({ dialogdetalles: false });
              this.GetListDetalles(this.state.po.Id);
              this.GetDetalleOS(this.state.po.Id);
            } else {
              abp.notify.error(
                "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
                "Error"
              );
            }
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(error, "Error");
          });
      } else {
        axios
          .post("/Proyecto/OrdenServicio/FDEditDetalle", {
            Id: this.state.Idd,
            OrdenServicioId: this.state.po.Id,
            ProyectoId: this.state.ProyectoId,
            GrupoItemId: this.state.GrupoItemId,
            valor_os: this.state.valor_os,
            OfertaComercialId: this.state.OfertaComercialId,
            vigente: true,
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("Guardado", "Correcto");
              this.setState({ dialogdetalles: false });
              this.GetListDetalles(this.state.po.Id);
              this.GetDetalleOS(this.state.po.Id);
            } else {
              abp.notify.error(
                "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
                "Error"
              );
            }
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(
              "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
              "Error"
            );
          });
      }
    }
  };

  onCheckOrdenProceder = (check) => {
    if (check) {
      this.setState({ ordenProceder: check, fechaordenProceder: new Date() });
    } else {
      this.setState({
        ordenProceder: check,
        fechaordenProceder: null,
        link_ordenProdecer: "",
      });
    }
  };
  onDownloaImagen = (Id) => {
    return (window.location = `/Proyecto/AvanceObra/Descargar/${Id}`);
  };

  MostrarFormulario() {
    this.setState({ visible: true });
  }

  onHide = () => {
    this.setState({ vista: "details", visibletransmital: false });
  };

  handleChange(event) {
    if (event.target.files) {
      console.log(event.target.files);
      let files = event.target.files || event.dataTransfer.files;
      if (files.length > 0) {
        let uploadFile = files[0];
        this.setState({
          uploadFile: uploadFile,
        });
      }
    } else {
      this.setState({ [event.target.name]: event.target.value });
    }
  }

  OcultarFormulario = () => {
    this.setState({ visible: false });
  };
}
const Container = wrapForm(DetalleOferta);
ReactDOM.render(<Container />, document.getElementById("content"));
