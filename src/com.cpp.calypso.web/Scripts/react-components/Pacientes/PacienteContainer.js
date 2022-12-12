import ReactDOM from "react-dom"
import React, { Fragment } from "react";
import BlockUi from "react-block-ui"
import axios from "axios"
import CurrencyFormat from "react-currency-format"
import Field from "../Base/Field-v2"

import wrapForm from "../Base/BaseWrapper"
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table"
import { Dialog } from "primereact-v3.3/dialog"
import { Growl } from "primereact-v3.3/growl"
import { Card } from "primereact-v3.3/card"
import { TabView, TabPanel } from "primereact-v3.3/tabview";
import { Checkbox } from "primereact-v3.3/checkbox"
import { Panel } from 'primereact-v3.3/panel';
import { Fieldset } from 'primereact-v3.3/fieldset';
import { ToggleButton } from 'primereact-v3.3/togglebutton';

class PacienteContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      errors: {},
      errorsMNA: {},
      errorsKatz: {},
      action: "list", actionMNA: "", actionKatz: "createKTZ",
      view: false,
      viewKatz: false,
      data: [],
      pacienteSeleccionado: null,
      total11: 0,
      total2: 0,
      totalkat: 0,
      preguntas1: [],
      preguntas2: [],
      preguntas3: [],
      preguntas4: [],
      preguntas5: [],
      preguntas6: [],
      preguntas7: [],
      preguntas8: [],
      preguntas9: [],
      preguntas10: [],
      preguntas11: [],
      preguntas12: [],
      preguntas13: [],
      preguntas14: [],
      preguntas15: [],
      preguntas16: [],

      preguntas17: [],
      preguntas18: [],

      catalogosexo: [],



      /*Paciente */
      Id: 0,
      Identificacion: "",
      NombresApellidos: "",
      Peso: 0,
      Edad: 0,
      SexoId: 0,
      Talla: 0,
      Centros: [],
      CentroId: 0,

      GrupoEdad: "",
      NivelEducativo: "",
      EstadoCivil: "",
      ViveSolo: "",
      ConsumeAlcohol: "",
      ConsumeCigarillo: "",
      AutoReporteSalud: "",
      HipertencionArterial: "",
      InsuficienciaArterial: "",
      InsuficienciaCardicaCongestiva: "",
      Epoc: "",
      EnfermedadCerebroVascular: "",
      Hospitalizacion: "",
      Emergencia: "",
      opciones: [
        { label: "SI", value: "SI" },
        { label: "NO", value: "NO" },
      ],
      opcionesAlcohol: [
        { label: "Actual", value: "Actual" },
        { label: "Pasado", value: "Pasado" },
        { label: "NO", value: "NO" },
      ],
      tipoSalud: [
        { label: "Bueno", value: "Bueno" },
        { label: "Regular", value: "Regular" },
        { label: "Malo", value: "Malo" },

      ],
      opcionesVisitas: [
        { label: "Ninguna", value: "Ninguna" },
        { label: "1", value: "1" },
        { label: "2", value: "2" },
        { label: "3", value: "3" },
        { label: "4", value: "4" },
        { label: "Más de 4", value: "Más de 4" },
      ],

      niveles: [
        { label: "Primaria", value: "Primaria" },
        { label: "Secundaria", value: "Secundaria" },
        { label: "Superior", value: "Superior" },
      ],
      estados: [
        { label: "Soltero(a)", value: "Soltero(a)" },
        { label: "Casado(a)", value: "Casado(a)" },
        { label: "Viudo(a)", value: "Viudo(a)" },
        { label: "Divorciado(a)", value: "Divorciado(a)" },
      ],


      gruposEdad: [
        { label: "60 - 65", value: "60 - 65" },
        { label: "66 - 71", value: "66 - 71" },
        { label: "72 - 77", value: "72 - 77" },
        { label: "78 - 83", value: "78 - 83" },
        { label: "84 - 89", value: "84 - 89" },
        { label: "90 - 95", value: "90 - 95" },
        { label: "96 - 101", value: "96 - 101" },
      ],







      detalles: [],

      IdMNA: 0,
      PacienteId: 0,
      Fecha: "",
      PerdidaApetitoId: 0,
      PerdidaPesoId: 0,
      MovilidadId: 0,
      EnfermedadAgudaId: 0,
      ProblemasNeuroId: 0,
      IndiceMasaId: 0,
      ViveDomicilioId: 0,
      MedicamentoDiaId: 0,
      UlceraLesionId: 0,
      ComidaDiariaId: 0,
      ConsumoPersonaId: 11392,
      ConsumeLacteos: false,
      ConsumeLegumbres: false,
      ConsumeCarne: false,
      ConsumoFrutasVerdurasId: 0,
      NumeroVasosAguaId: 0,
      ModoAlimentarseId: 0,
      ConsideracionEnfermoId: 0,
      EstadoSaludId: 0,
      CircunferenciaBraquialId: 0,
      CircunferenciaPiernaId: 0,


      IdKATZ: 0,
      Bano: null,
      Vestido: null,

      Sanitario: null,

      Transferencias: null,
      Continencia: null,
      Alimentacion: null,

      Calificacion: "",
      CalificacionDependencia: "",

      opcionkatz: [
        { label: "1 - Independiente", value: true },
        { label: "0 - Dependiente", value: false }

      ],

    }

    this.handleChange = this.handleChange.bind(this)

    this.handleChangebox = this.handleChangebox.bind(this)
    this.onChangeValue = this.onChangeValue.bind(this)
    this.onChangeValueT = this.onChangeValueT.bind(this)
    this.mostrarForm = this.mostrarForm.bind(this)
    this.mostrarFormMNA = this.mostrarFormMNA.bind(this)
    this.mostrarFormKatz = this.mostrarFormKatz.bind(this)
    this.OcultarFormulario = this.OcultarFormulario.bind(this)
    this.isValid = this.isValid.bind(this)
    this.isValidMNA = this.isValidMNA.bind(this)
    this.isValidaKatz = this.isValidaKatz.bind(this)
    this.EnviarFormulario = this.EnviarFormulario.bind(this)
    this.EnviarFormularioMNA = this.EnviarFormularioMNA.bind(this)
    this.EnviarFormularioKTZ = this.EnviarFormularioKTZ.bind(this)
  }

  componentDidMount() {
    this.GetList()
    this.GetCatalogs()
  }
  GetCatalogs = () => {
    this.props.blockScreen()
    axios
      .post("/Documentos/Documento/FGetCatalogosSexo", {})
      .then((response) => {
        console.log(response.data)
        var items1 = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })

        this.setState({
          catalogosexo: items1
        })
      })
      .catch((error) => {
        console.log(error)
      })
    axios
      .post("/Documentos/Documento/FGetCatalogos", {})
      .then((response) => {
        console.log(response.data)
        var items1 = response.data.pr1.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items2 = response.data.pr2.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items3 = response.data.pr3.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items4 = response.data.pr4.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items5 = response.data.pr5.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items6 = response.data.pr6.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items7 = response.data.pr7.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items8 = response.data.pr8.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items9 = response.data.pr9.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items10 = response.data.pr10.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items11 = response.data.pr11.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items12 = response.data.pr12.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items13 = response.data.pr13.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items14 = response.data.pr14.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items15 = response.data.pr15.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items16 = response.data.pr16.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items17 = response.data.pr17.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        var items18 = response.data.pr18.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })

        var itemscentros = response.data.centros.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })







        this.setState({
          preguntas1: items1,
          preguntas2: items2,
          preguntas3: items3,
          preguntas4: items4,
          preguntas5: items5,
          preguntas6: items6,
          preguntas7: items7,
          preguntas8: items8,
          preguntas9: items9,
          preguntas10: items10,
          preguntas11: items11,
          preguntas12: items12,
          preguntas13: items13,
          preguntas14: items14,
          preguntas15: items15,
          preguntas16: items16,

          preguntas17: items17,
          preguntas18: items18,
          Centros: itemscentros
        })
      })
      .catch((error) => {
        console.log(error)
      })

  }
  isValid = () => {
    const errors = {}

    //if (this.state.Identificacion == "") {
    // errors.Identificacion = "Campo Requerido"
    //}

    if (this.state.NombresApellidos == "") {
      errors.NombresApellidos = "Campo Requerido"
    }

    if (this.state.SexoId == 0) {
      errors.SexoId = "Campo Requerido"
    }
    if (this.state.Edad == "") {
      errors.Edad = "Campo Requerido"
    }
    if (this.state.Talla == "") {
      errors.Talla = "Campo Requerido"
    }
    if (this.state.Peso == "") {
      errors.Peso = "Campo Requerido"
    }
    if (this.state.CentroId == 0) {
      errors.CentroId = "Campo Requerido"
    }

    if (this.state.GrupoEdad == "") {
      errors.GrupoEdad = "Campo Requerido"
    }

    if (this.state.NivelEducativo == "") {
      errors.NivelEducativo = "Campo Requerido"
    }

    if (this.state.EstadoCivil == "") {
      errors.EstadoCivil = "Campo Requerido"
    }

    if (this.state.ViveSolo == "") {
      errors.ViveSolo = "Campo Requerido"
    }

    if (this.state.ConsumeAlcohol == "") {
      errors.ConsumeAlcohol = "Campo Requerido"
    }

    if (this.state.ConsumeCigarillo == "") {
      errors.ConsumeCigarillo = "Campo Requerido"
    }

    if (this.state.AutoReporteSalud == "") {
      errors.AutoReporteSalud = "Campo Requerido"
    }

    if (this.state.HipertencionArterial == "") {
      errors.HipertencionArterial = "Campo Requerido"
    }

    if (this.state.InsuficienciaArterial == "") {
      errors.InsuficienciaArterial = "Campo Requerido"
    }

    if (this.state.InsuficienciaCardicaCongestiva == "") {
      errors.InsuficienciaCardicaCongestiva = "Campo Requerido"
    }

    if (this.state.Epoc == "") {
      errors.Epoc = "Campo Requerido"
    }

    if (this.state.EnfermedadCerebroVascular == "") {
      errors.EnfermedadCerebroVascular = "Campo Requerido"
    }

    /*if (this.state.Hospitalizacion == "") {
      errors.Hospitalizacion = "Campo Requerido"
    }

    if (this.state.Emergencia == "") {
      errors.Emergencia = "Campo Requerido"
    }*/

    this.setState({ errors })
    return Object.keys(errors).length === 0
  }
  contarKatz = () => {
    let total = 0;
    let totalDep = 0;
    let calif = "";
    let califD = "";
    if (this.state.Bano) {
      total = total + 1;
    }
    if (this.state.Bano === false) {
      totalDep = totalDep + 1;

    }
    if (this.state.Vestido) {
      total = total + 1;
    }

    if (this.state.Vestido === false) {
      totalDep = totalDep + 1;

    }
    if (this.state.Sanitario) {
      total = total + 1;
    }
    if (this.state.Sanitario === false) {
      totalDep = totalDep + 1;

    }
    if (this.state.Transferencias) {
      total = total + 1;
    }
    if (this.state.Transferencias === false) {
      totalDep = totalDep + 1;

    }
    if (this.state.Continencia) {
      total = total + 1;
    }
    if (this.state.Continencia === false) {
      totalDep = totalDep + 1;

    }
    if (this.state.Alimentacion) {
      total = total + 1;
    }
    if (this.state.Alimentacion === false) {
      totalDep = totalDep + 1;

    }
    if (total == 6) {
      calif = "A";
    }
    if (total == 5) {
      calif = "B";
    };
    if (
      (this.state.Bano != null && this.state.Bano == false) &&
      (this.state.Vestido != null && this.state.Vestido == false ||
        this.state.Sanitario != null && this.state.Sanitario == false ||
        this.state.Transferencias != null && this.state.Transferencias == false ||
        this.state.Continencia != null && this.state.Continencia == false ||
        this.state.Alimentacion != null && this.state.Alimentacion == false)) {
      calif = "C";

    }
    if (
      (this.state.Bano != null && this.state.Bano == false) && (this.state.Vestido != null && this.state.Vestido == false) &&
      (
        this.state.Sanitario != null && this.state.Sanitario == false ||
        this.state.Transferencias != null && this.state.Transferencias == false ||
        this.state.Continencia != null && this.state.Continencia == false ||
        this.state.Alimentacion != null && this.state.Alimentacion == false)) {
      calif = "D";

    }
    if (
      (this.state.Bano != null && this.state.Bano == false) && (this.state.Vestido != null && this.state.Vestido == false) && (
        this.state.Sanitario != null && this.state.Sanitario == false) &&
      (

        this.state.Transferencias != null && this.state.Transferencias == false ||
        this.state.Continencia != null && this.state.Continencia == false ||
        this.state.Alimentacion != null && this.state.Alimentacion == false)) {
      calif = "E";

    }
    if (
      (this.state.Bano != null && this.state.Bano == false) && (this.state.Vestido != null && this.state.Vestido == false) && (
        this.state.Sanitario != null && this.state.Sanitario == false) && (this.state.Transferencias != null && this.state.Transferencias == false) &&
      (
        this.state.Continencia != null && this.state.Continencia == false ||
        this.state.Alimentacion != null && this.state.Alimentacion == false)) {
      calif = "F";

    }
    if (totalDep === 6) {
      calif = "G"
    }
    if (totalDep >= 2 && (calif != "C" && calif != "D" && calif != "E" && calif != "F")
    ) {
      calif = "H"
    }

    if (calif === "A" || calif === "B") {
      califD = "Ausencia de incapacidad o incapacidad leve.";
    }
    if (calif === "C" || calif === "D" || calif == "H") {
      califD = "Incapacidad moderada.";
    }
    if (calif === "E" || calif === "F" || calif == "G") {
      califD = "Incapacidad severa.";
    }


    if (totalDep === 6) {
      calif = "G"
    }
    console.log("CALIFICACION ", calif);
    console.log("total ", total);
    console.log("Dependencia", califD)

    this.setState({ totalkat: total, Calificacion: calif, CalificacionDependencia: califD });

  }

  descargarFormatoCargaMasiva = () => {
    this.props.blockScreen();
    var formData = new FormData();
    axios
      .post(
        `/Documentos/Documento/GetReporteSeguimientoMovil`,
        formData,
        { responseType: "arraybuffer" }
      )
      .then((response) => {
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
        this.props.showSuccess("Formato descargado exitosamente");
        this.props.unlockScreen();
        this.setState({
          ContratoId: 0,
        });
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(
          "Ocurrió un error al descargar el archivo, intentalo nuevamente"
        );
        this.props.unlockScreen();
      });
  };


  isValidMNA = () => {
    const errorsMNA = {}
    if (this.state.PerdidaApetitoId == 0) {
      errorsMNA.PerdidaApetitoId = "Campo Requerido"
    }
    if (this.state.PerdidaPesoId == 0) {
      errorsMNA.PerdidaPesoId = "Campo Requerido"
    }
    if (this.state.MovilidadId == 0) {
      errorsMNA.MovilidadId = "Campo Requerido"
    }
    if (this.state.EnfermedadAgudaId == 0) {
      errorsMNA.EnfermedadAgudaId = "Campo Requerido"
    }
    if (this.state.ProblemasNeuroId == 0) {
      errorsMNA.ProblemasNeuroId = "Campo Requerido"
    }
    if (this.state.IndiceMasaId == 0) {
      errorsMNA.IndiceMasaId = "Campo Requerido"
    }
    if (this.state.total11 < 12) {
      if (this.state.ViveDomicilioId == 0) {
        errorsMNA.ViveDomicilioId = "Campo Requerido"
      }
      if (this.state.MedicamentoDiaId == 0) {
        errorsMNA.MedicamentoDiaId = "Campo Requerido"
      }
      if (this.state.UlceraLesionId == 0) {
        errorsMNA.UlceraLesionId = "Campo Requerido"
      }
      if (this.state.ComidaDiariaId == 0) {
        errorsMNA.ComidaDiariaId = "Campo Requerido"
      }
      if (this.state.ConsumoPersonaId == 0) {
        errorsMNA.ConsumoPersonaId = "Campo Requerido"
      }
      if (this.state.ConsumoFrutasVerdurasId == 0) {
        errorsMNA.ConsumoFrutasVerdurasId = "Campo Requerido"
      }
      if (this.state.NumeroVasosAguaId == 0) {
        errorsMNA.NumeroVasosAguaId = "Campo Requerido"
      }
      if (this.state.ModoAlimentarseId == 0) {
        errorsMNA.ModoAlimentarseId = "Campo Requerido"
      }
      if (this.state.ConsideracionEnfermoId == 0) {
        errorsMNA.ConsideracionEnfermoId = "Campo Requerido"
      }
      if (this.state.EstadoSaludId == 0) {
        errorsMNA.EstadoSaludId = "Campo Requerido"
      }
      if (this.state.CircunferenciaBraquialId == 0) {
        errorsMNA.CircunferenciaBraquialId = "Campo Requerido"
      }
      if (this.state.CircunferenciaPiernaId == 0) {
        errorsMNA.CircunferenciaPiernaId = "Campo Requerido"
      }
    }
    if (this.state.Fecha == "") {
      errorsMNA.Fecha = "Campo Requerido"
    }

    this.setState({ errorsMNA })
    return Object.keys(errorsMNA).length === 0
  }

  isValidDetalle = () => {
    const errors = {}

    if (this.state.ProyectoId == 0) {
      errors.ProyectoId = "Campo Requerido"
    }
    if (this.state.OfertaComercialId == 0) {
      errors.OfertaComercialId = "Campo Requerido"
    }
    if (this.state.GrupoItemId == 0) {
      errors.GrupoItemId = "Campo Requerido"
    }

    this.setState({ errors })
    return Object.keys(errors).length === 0
  }
  isValidaKatz = () => {

    const errorsKatz = {}
    if (this.state.Bano == null) {
      errorsKatz.Bano = "Campo Requerido";
    }
    if (this.state.Vestido == null) {
      errorsKatz.Vestido = "Campo Requerido";
    }
    if (this.state.Sanitario == null) {
      errorsKatz.Sanitario = "Campo Requerido";
    }
    if (this.state.Transferencias == null) {
      errorsKatz.Transferencias = "Campo Requerido";
    }
    if (this.state.Continencia == null) {
      errorsKatz.Continencia = "Campo Requerido";
    }
    if (this.state.Alimentacion == null) {
      errorsKatz.Alimentacion = "Campo Requerido";
    }
    console.log("errores", errorsKatz)
    this.setState({ errorsKatz })
    return Object.keys(errorsKatz).length === 0
  }

  OcultarFormularioOrden = () => {
    this.setState({ pacienteSeleccionado: null, action: "list" })
  }
  OcultarFormularioOrdenDetalle = () => {
    this.setState({ pacienteSeleccionado: null, action: "list" })
    this.GetList()
  }
  GetList = () => {
    this.props.blockScreen()
    axios
      .post("/Documentos/Documento/FGetList", {})
      .then((response) => {
        console.log(response.data)
        this.setState({ data: response.data })
        this.props.unlockScreen()
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(error)
        this.props.unlockScreen()
      })
  }

  GetTotal1 = () => {

    let valorIMC = this.state.pacienteSeleccionado != null ? (this.state.pacienteSeleccionado.Peso / (this.state.pacienteSeleccionado.Talla * this.state.pacienteSeleccionado.Talla)) : 0;
    if (valorIMC < 19) {
      this.setState({ IndiceMasaId: 11372 });
    }
    if (valorIMC >= 19 && valorIMC < 21) {
      this.setState({ IndiceMasaId: 11374 });
    }
    if (valorIMC >= 21 && valorIMC < 23) {
      this.setState({ IndiceMasaId: 11375 });
    }
    if (valorIMC >= 23) {
      this.setState({ IndiceMasaId: 11376 });
    }

    axios
      .post("/Documentos/Documento/GetTotal1", {
        PerdidaApetitoId: this.state.PerdidaApetitoId,
        PerdidaPesoId: this.state.PerdidaPesoId,
        MovilidadId: this.state.MovilidadId,
        EnfermedadAgudaId: this.state.EnfermedadAgudaId,
        ProblemasNeuroId: this.state.ProblemasNeuroId,
        IndiceMasaId: this.state.IndiceMasaId,
        ViveDomicilioId: this.state.ViveDomicilioId,
        MedicamentoDiaId: this.state.MedicamentoDiaId,
        UlceraLesionId: this.state.UlceraLesionId,
        ComidaDiariaId: this.state.ComidaDiariaId,
        ConsumoPersonaId: this.state.ConsumoPersonaId,
        ConsumeLacteos: this.state.ConsumeLacteos,
        ConsumeLegumbres: this.state.ConsumeLegumbres,
        ConsumeCarne: this.state.ConsumeCarne,
        ConsumoFrutasVerdurasId: this.state.ConsumoFrutasVerdurasId,
        NumeroVasosAguaId: this.state.NumeroVasosAguaId,
        ModoAlimentarseId: this.state.ModoAlimentarseId,
        ConsideracionEnfermoId: this.state.ConsideracionEnfermoId,
        EstadoSaludId: this.state.EstadoSaludId,
        CircunferenciaBraquialId: this.state.CircunferenciaBraquialId,
        CircunferenciaPiernaId: this.state.CircunferenciaPiernaId,

      })
      .then((response) => {
        console.log(response.data)
        this.setState({ total11: response.data }, this.GetTotal2())

      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(error)
        this.props.unlockScreen()
      })
  }

  GetTotal2 = () => {

    axios
      .post("/Documentos/Documento/GetTotal2", {
        PerdidaApetitoId: this.state.PerdidaApetitoId,
        PerdidaPesoId: this.state.PerdidaPesoId,
        MovilidadId: this.state.MovilidadId,
        EnfermedadAgudaId: this.state.EnfermedadAgudaId,
        ProblemasNeuroId: this.state.ProblemasNeuroId,
        IndiceMasaId: this.state.IndiceMasaId,
        ViveDomicilioId: this.state.ViveDomicilioId,
        MedicamentoDiaId: this.state.MedicamentoDiaId,
        UlceraLesionId: this.state.UlceraLesionId,
        ComidaDiariaId: this.state.ComidaDiariaId,
        ConsumoPersonaId: this.state.ConsumoPersonaId,
        ConsumeLacteos: this.state.ConsumeLacteos,
        ConsumeLegumbres: this.state.ConsumeLegumbres,
        ConsumeCarne: this.state.ConsumeCarne,
        ConsumoFrutasVerdurasId: this.state.ConsumoFrutasVerdurasId,
        NumeroVasosAguaId: this.state.NumeroVasosAguaId,
        ModoAlimentarseId: this.state.ModoAlimentarseId,
        ConsideracionEnfermoId: this.state.ConsideracionEnfermoId,
        EstadoSaludId: this.state.EstadoSaludId,
        CircunferenciaBraquialId: this.state.CircunferenciaBraquialId,
        CircunferenciaPiernaId: this.state.CircunferenciaPiernaId,
      })
      .then((response) => {
        console.log(response.data)
        this.setState({ total2: response.data })

      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(error)
        this.props.unlockScreen()
      })
  }


  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    })
  }

  onChangeValueT = (name, value) => {
    this.setState({
      [name]: value,
    });
    setTimeout(() => {



      this.GetTotal1()
    }, 500);
  }
  onChangeValueDependencia = (name, value) => {
    this.setState({
      [name]: value,
    })
    setTimeout(() => {
      this.contarKatz()
    }, 500);
  }

  Eliminar = (Id) => {
    console.log(Id)
    axios
      .post("/Documentos/Documento/FGetDelete/", {
        Id: Id,
      })
      .then((response) => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente")

          this.GetList()
        } else if (response.data == "FALSE") {
          this.props.showWarn("No se puedo Eliminar")
        }
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn("Ocurrió un Error")
      })
  }

  EliminarDetalleMNA = (Id) => {
    console.log(Id)
    console.log(this.state.pacienteSeleccionado.Id)
    axios
      .post("/Documentos/Documento/FGetDeleteMNA/", {
        Id: Id,

      })
      .then((response) => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente")
          this.setState({ action: "detalles" })
          this.GetListDetalles(this.state.pacienteSeleccionado.Id)

        } else {
          this.props.showWarn(response.data)
        }
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(error)
      })
  }

  EliminarDetalleKatz = (Id) => {
    console.log(Id)
    console.log(this.state.pacienteSeleccionado.Id)
    axios
      .post("/Documentos/Documento/FGetDeleteK/", {
        Id: Id,

      })
      .then((response) => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente")
          this.setState({ action: "detalles" })
          this.GetListDetalles(this.state.pacienteSeleccionado.Id)

        } else {
          this.props.showWarn(response.data)
        }
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(error)
      })
  }
  mostrarDetalles = (row) => {
    console.clear()
    console.log("Paciente", row)

    if (row != null && row.Id > 0) {

      this.setState({ pacienteSeleccionado: row, action: "detalles", detalles: [] })
      this.GetListDetalles(row.Id)
    }
  }
  GetListDetalles = (pacienteId) => {
    this.props.blockScreen()
    axios
      .post("/Documentos/Documento/FGetListPacie", { id: pacienteId })
      .then((response) => {
        console.log("ANEXOS", response.data)
        this.setState({ detalles: response.data })
        this.props.unlockScreen()
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(error)
        this.props.unlockScreen()
      })

  }

  mostrarForm = (row) => {
    if (row != null && row.Id > 0) {
      this.setState({
        Id: row.Id,
        action: "edit",
        Identificacion: row.Identificacion,
        NombresApellidos: row.NombresApellidos,
        Peso: row.Peso,
        Edad: row.Edad,
        SexoId: row.SexoId,
        Talla: row.Talla,

        CentroId: row.CentroId,

        GrupoEdad: row.GrupoEdad,
        NivelEducativo: row.NivelEducativo,
        EstadoCivil: row.EstadoCivil,
        ViveSolo: row.ViveSolo,
        ConsumeAlcohol: row.ConsumeAlcohol,
        ConsumeCigarillo: row.ConsumeCigarillo,
        AutoReporteSalud: row.AutoReporteSalud,
        HipertencionArterial: row.HipertencionArterial,
        InsuficienciaArterial: row.InsuficienciaArterial,
        InsuficienciaCardicaCongestiva: row.InsuficienciaCardicaCongestiva,
        Epoc: row.Epoc,
        EnfermedadCerebroVascular: row.EnfermedadCerebroVascular,
        Hospitalizacion: row.Hospitalizacion,
        Emergencia: row.Emergencia

      })
    } else {
      this.setState({
        Id: 0,
        action: "create",
        Identificacion: "",
        NombresApellidos: "",
        Peso: 0,
        Edad: 0,
        SexoId: 0,
        Talla: 0,

        CentroId: 0,

        GrupoEdad: "",
        NivelEducativo: "",
        EstadoCivil: "",
        ViveSolo: "",
        ConsumeAlcohol: "",
        ConsumeCigarillo: "",
        AutoReporteSalud: "",
        HipertencionArterial: "",
        InsuficienciaArterial: "",
        InsuficienciaCardicaCongestiva: "",
        Epoc: "",
        EnfermedadCerebroVascular: "",
        Hospitalizacion: "",
        Emergencia: ""



      })
    }
  }
  mostrarFormMNA = (row) => {
    if (row != null && row.Id > 0) {
      this.setState({
        IdMNA: row.Id,
        view: true,
        viewKatz: false,
        actionMNA: "edit",
        PacienteId: row.PacienteId,
        Fecha: row.Fecha,
        PerdidaApetitoId: row.PerdidaApetitoId,
        PerdidaPesoId: row.PerdidaPesoId,
        MovilidadId: row.MovilidadId,
        EnfermedadAgudaId: row.EnfermedadAgudaId,
        ProblemasNeuroId: row.ProblemasNeuroId,
        IndiceMasaId: row.IndiceMasaId,
        ViveDomicilioId: row.ViveDomicilioId,
        MedicamentoDiaId: row.MedicamentoDiaId,
        UlceraLesionId: row.UlceraLesionId,
        ComidaDiariaId: row.ComidaDiariaId,
        ConsumoPersonaId: row.ConsumoPersonaId,
        ConsumeLacteos: row.ConsumeLacteos,
        ConsumeLegumbres: row.ConsumeLegumbres,
        ConsumeCarne: row.ConsumeCarne,
        ConsumoFrutasVerdurasId: row.ConsumoFrutasVerdurasId,
        NumeroVasosAguaId: row.NumeroVasosAguaId,
        ModoAlimentarseId: row.ModoAlimentarseId,
        ConsideracionEnfermoId: row.ConsideracionEnfermoId,
        EstadoSaludId: row.EstadoSaludId,
        CircunferenciaBraquialId: row.CircunferenciaBraquialId,
        CircunferenciaPiernaId: row.CircunferenciaPiernaId,

      })
      setTimeout(() => {
        this.seleccionarConsumePersona()

        this.GetTotal1()
      }, 500);

    } else {
      this.setState({
        IdMNA: 0,
        view: true,
        viewKatz: false,
        PacienteId: this.state.pacienteSeleccionado.Id,
        Fecha: "",
        actionMNA: "createMNA",
        PerdidaApetitoId: 0,
        PerdidaPesoId: 0,
        MovilidadId: 0,
        EnfermedadAgudaId: 0,
        ProblemasNeuroId: 0,
        IndiceMasaId: 0,
        ViveDomicilioId: 0,
        MedicamentoDiaId: 0,
        UlceraLesionId: 0,
        ComidaDiariaId: 0,
        ConsumoPersonaId: 11392,
        ConsumeLacteos: false,
        ConsumeLegumbres: false,
        ConsumeCarne: false,
        ConsumoFrutasVerdurasId: 0,
        NumeroVasosAguaId: 0,
        ModoAlimentarseId: 0,
        ConsideracionEnfermoId: 0,
        EstadoSaludId: 0,
        CircunferenciaBraquialId: 0,
        CircunferenciaPiernaId: 0,





      })
    }
  }

  mostrarFormKatz = (row) => {
    if (row != null && row.Id > 0) {
      this.setState({
        IdKATZ: row.Id,
        viewKatz: true,
        view: false,
        actionKatz: "editKatz",
        Bano: row.Bano,
        Vestido: row.Vestido,

        Sanitario: row.Sanitario,

        Transferencias: row.Transferencias,
        Continencia: row.Continencia,
        Alimentacion: row.Alimentacion,

        Calificacion: row.Calificacion,
        CalificacionDependencia: row.CalificacionDependiente,
        totalkat: row.Puntuacion
      })
    } else {
      this.setState({
        IdKATZ: 0,
        viewKatz: true,
        view: false,
        actionKatz: "createKatz",
        Bano: null,
        Vestido: null,
        totalkat: 0,
        Sanitario: null,
        Transferencias: null,
        Continencia: null,
        Alimentacion: null,
        Calificacion: "",
        CalificacionDependencia: ""
      })
    }
  }



  onDownload = (Id) => {
    return (window.location = `/Proyecto/TransmitalCabecera/Descargar/${Id}`)
  }

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
          title="Administra Anexos"
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
              this.Eliminar(row.Id)
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    )
  }
  generarBotonesDetalles = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info btn-sm"
          style={{ marginRight: "0.2em" }}
          onClick={() => this.mostrarFormMNA(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar MNA"
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
              this.EliminarDetalleMNA(row.Id)
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    )
  }



  generarBotonesDetallesKtz = (cell, row) => {

    return (
      <div>
        <button
          className="btn btn-outline-info btn-sm"
          style={{ marginRight: "0.2em" }}
          onClick={() => this.mostrarFormKatz(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar Ktz"
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
              this.EliminarDetalleKatz(row.Id)
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    )
  }
  renderShowsTotal(start, to, total) {
    return (
      <p style={{ color: "black" }}>
        De {start} hasta {to}, Total Registros {total}
      </p>
    )
  }
  render() {
    let msgTotalPunto = "";
    if (this.state.total11 >= 12) {
      msgTotalPunto = "";
    }

    const options = {

      sizePerPage: 10,
      noDataText: "No existen datos registrados",
      sizePerPageList: [
        {
          text: "10",
          value: 10,
        },
        {
          text: "20",
          value: 20,
        },
      ],
      paginationShowsTotal: this.renderShowsTotal, // Accept bool or function
    }

    if (this.state.action === "list") {
      return (
        <Card title="Administración de Anexos" subTitle="Pacientes, MNA, KATZ">
          <div align="right">
            <button
              className="btn btn-outline-primary mr-4"
              type="button"
              data-toggle="collapse"
              aria-expanded="false"
              onClick={() => this.descargarFormatoCargaMasiva()}
            >
              Descargar BDD
            </button>
            {" "}
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.mostrarForm}
            >
              Nuevo
            </button>
          </div>
          <br />
          <div>
            <BootstrapTable
              data={this.state.data}
              hover={true}
              pagination={true}
            >
              <TableHeaderColumn
                dataField="any"
                dataFormat={this.Secuencial}
                width={"8%"}
                tdStyle={{
                  whiteSpace: "normal",
                  textAlign: "center",
                  fontSize: "11px",
                }}
                thStyle={{
                  whiteSpace: "normal",
                  textAlign: "center",
                  fontSize: "11px",
                }}
              >
                Nº
              </TableHeaderColumn>

              <TableHeaderColumn
                width={"14%"}
                dataField="Identificacion"

                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}

              >
                Identificación
              </TableHeaderColumn>

              <TableHeaderColumn
                width={"20%"}
                dataField="NombresApellidos"

                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}

              >
                Nombres y Apellidos
              </TableHeaderColumn>
              <TableHeaderColumn

                dataField="Edad"

                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}

              >
                Edad (años)
              </TableHeaderColumn>
              <TableHeaderColumn

                dataField="Peso"

                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}

              >
                Peso (Kg)
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="Talla"
                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}
              >
                Talla (m)
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="Id"


                width={"14%"}
                dataFormat={this.generarBotones}
                thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                isKey={true}
              >
                Opciones
              </TableHeaderColumn>
            </BootstrapTable>
          </div>
        </Card>
      )
    } else if (this.state.action === "detalles") {

      let valorIMC = this.state.pacienteSeleccionado != null ? (this.state.pacienteSeleccionado.Peso / (this.state.pacienteSeleccionado.Talla * this.state.pacienteSeleccionado.Talla)) : 0;

      return (
        <Panel header="Anexos Katz  y MNA">

          <div className="row">
            <div className="col" align="right">

              <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={this.OcultarFormularioOrdenDetalle}
              >
                Regresar
              </button>

            </div>
          </div>
          <br></br>
          <Panel header="Paciente">
            <div className="row">
              <div className="col">
                <h6>
                  <b>Identificación :</b>{" "}
                  {this.state.pacienteSeleccionado != null
                    ? this.state.pacienteSeleccionado.Identificacion
                    : ""}
                </h6>
              </div>

              <div className="col">
                <h6>
                  <b>Nombres y Apellidos :</b>{" "}
                  {this.state.pacienteSeleccionado != null ? this.state.pacienteSeleccionado.NombresApellidos : ""}
                </h6>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <h6>
                  <b>Edad :</b>{" "}
                  {this.state.pacienteSeleccionado != null ? this.state.pacienteSeleccionado.Edad : ""}
                </h6>
              </div>
              <div className="col">
                <h6>
                  <b>Talla (m) :</b> {this.state.pacienteSeleccionado != null ? this.state.pacienteSeleccionado.Talla : ""}
                </h6>
              </div>
            </div>

            <div className="row">

              <div className="col">
                <h6>
                  <b>Peso (kg) :</b>{" "}
                  {this.state.pacienteSeleccionado != null ? this.state.pacienteSeleccionado.Peso : ""}
                </h6>
              </div>
              <div className="col">
                <h6>
                  <b>Centro Geriátrico :</b> {this.state.pacienteSeleccionado != null ? this.state.pacienteSeleccionado.centroString : ""}
                </h6>
              </div>
            </div>
            <div className="row">

              <div className="col">
                <h6>
                  <b>Grupo Edad:</b>{" "}
                  {this.state.pacienteSeleccionado != null ? this.state.pacienteSeleccionado.GrupoEdad : ""}
                </h6>
              </div>
              <div className="col">
                <h6>
                  <b>Estado de Salud:</b> {this.state.pacienteSeleccionado != null ? this.state.pacienteSeleccionado.AutoReporteSalud : ""}
                </h6>
              </div>
            </div>
          </Panel>
          <br></br>

          <TabView className="tabview-custom">


            <TabPanel header="MINI-EXAMEN DEL ESTADO NUTRICIONAL (MNA)">
              <div className="row">
                <div className="col">
                  <div className="row">
                    <div className="col">
                      <div align="right">
                        <button
                          type="button"
                          className="btn btn-outline-primary"
                          icon="fa fa-fw fa-ban"
                          onClick={this.mostrarFormMNA}
                        >
                          Nuevo Registro
                        </button>


                        <div>
                          <br></br>
                          {this.state.view === false && (
                            <BootstrapTable
                              data={this.state.detalles.mnas}
                              hover={true}
                              pagination={true}

                            >
                              <TableHeaderColumn
                                dataField="any"
                                dataFormat={this.Secuencial}
                                width={"8%"}
                                tdStyle={{
                                  whiteSpace: "normal",
                                  textAlign: "center",
                                  fontSize: "11px",
                                }}
                                thStyle={{
                                  whiteSpace: "normal",
                                  textAlign: "center",
                                  fontSize: "11px",
                                }}
                                editable={false}
                              >
                                Nº
                              </TableHeaderColumn>

                              <TableHeaderColumn

                                dataField="Fecha"
                                filter={{ type: "TextFilter", delay: 500 }}
                                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                                dataSort={true}
                                editable={false}
                                dataFormat={this.formatoFechaCorta}
                              >
                                Fecha
                              </TableHeaderColumn>
                              <TableHeaderColumn

                                dataField="Puntuacion"
                                filter={{ type: "TextFilter", delay: 500 }}
                                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                                dataSort={true}
                                editable={false}
                              >
                                Puntuación
                              </TableHeaderColumn>
                              <TableHeaderColumn

                                dataField="DetallePuntuacion"
                                filter={{ type: "TextFilter", delay: 500 }}
                                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                                dataSort={true}
                                editable={false}
                              >
                                Estado
                              </TableHeaderColumn>

                              <TableHeaderColumn
                                dataField="Id"
                                isKey
                                width={"14%"}
                                dataFormat={this.generarBotonesDetalles}
                                thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                              >
                                Opciones
                              </TableHeaderColumn>
                            </BootstrapTable>
                          )}
                          <hr></hr>

                        </div>
                      </div>
                    </div>
                  </div>
                  {this.state.view && (
                    <div>

                      <form onSubmit={this.EnviarFormularioMNA}>
                        <div className="row">
                          <div className="col">
                            <Field
                              name="Fecha"
                              label="Fecha Registro Valoración"
                              required
                              type="date"
                              edit={true}
                              readOnly={false}
                              value={this.state.Fecha}
                              onChange={this.handleChange}
                              error={this.state.errorsMNA.Fecha}
                            />
                          </div>
                          <div className="col" align="right" style={{ fontSize: '20px' }}>
                            <p><b>Puntuación Total :</b>{this.state.total2}</p>
                          </div>
                        </div>
                        <div className="card border-warning">
                          <div className="card-body">
                            <div className="row">
                              <div className="col">
                                <Field
                                  name="PerdidaApetitoId"
                                  required
                                  value={this.state.PerdidaApetitoId}
                                  label="1. Ha perdido el apetito? Ha comido menos por falta de
                                  apetito, problemas digestivos, dificultades de
                                  masticación o deglución en los últimos 3 meses?."
                                  options={this.state.preguntas1}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.PerdidaApetitoId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />

                              </div>
                              <div className="col">
                                <Field
                                  name="PerdidaPesoId"
                                  required
                                  value={this.state.PerdidaPesoId}
                                  label="2. Pérdida reciente de peso (< 3 meses)"
                                  options={this.state.preguntas2}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.PerdidaPesoId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />


                              </div>
                            </div>
                            <div className="row">
                              <div className="col">
                                <Field
                                  name="MovilidadId"
                                  required
                                  value={this.state.MovilidadId}
                                  label="3. Movilidad"
                                  options={this.state.preguntas3}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.MovilidadId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />

                              </div>
                              <div className="col">
                                <Field
                                  name="EnfermedadAgudaId"
                                  required
                                  value={this.state.EnfermedadAgudaId}
                                  label="4.Ha tenido enfermedad aguda o situación de estrés psicológico en los últimos tres meses "
                                  options={this.state.preguntas4}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.EnfermedadAgudaId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />


                              </div>
                            </div>
                            <div className="row">
                              <div className="col">
                                <Field
                                  name="ProblemasNeuroId"
                                  required
                                  value={this.state.ProblemasNeuroId}
                                  label="5. Problemas Neuropsicológicos"
                                  options={this.state.preguntas5}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.ProblemasNeuroId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />

                              </div>
                              <div className="col">
                                <Field
                                  name="IndiceMasaId"
                                  required
                                  value={this.state.IndiceMasaId}
                                  label={"6.Índice de masa corporal (IMC) = peso en kg / (talla en m)²;   IMC= " + valorIMC.toFixed(2)}
                                  options={this.state.preguntas6}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.IndiceMasaId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />


                              </div>
                            </div>

                          </div>
                        </div>
                        {this.state.total11 >= 12 && (
                          <><div className="row">
                            <div className="col">
                              <div className="alert alert-success" role="alert" style={{ fontSize: '20px' }}>

                                <b >12 - 14 puntos: </b>Estado nutricional normal, no es necesaria una valoración completa.
                              </div>
                            </div>
                            <div className="col">

                              <button type="submit" className="btn btn-outline-primary">
                                Guardar
                              </button>
                              &nbsp;
                              <button
                                type="button"
                                className="btn btn-outline-primary"
                                icon="fa fa-fw fa-ban"
                                onClick={this.onHide}
                              >
                                Cancelar
                              </button>
                            </div>
                          </div>


                          </>)}
                        {(this.state.total11 != 0 && this.state.total11 < 12 && this.state.total11 >= 8) && (
                          <div className="alert alert-warning" role="alert" style={{ fontSize: '20px' }}>
                            <b >8 - 11 puntos: </b>Riesgo de malnutrición, continuar con la valoración.
                          </div>
                        )}
                        {(this.state.total11 != 0 && this.state.total11 <= 7) && (
                          <div className="alert alert-danger" role="alert" style={{ fontSize: '20px' }}>
                            <b >0 - 7 puntos: </b>Malnutrición, continuar con la valoración.
                          </div>
                        )}

                        <div className="card border-primary">
                          <div className="card-body">
                            <div className="row">
                              <div className="col">
                                <Field
                                  name="ViveDomicilioId"
                                  required
                                  value={this.state.ViveDomicilioId}
                                  label="7. El paciente vive independiente en su domicilio?"
                                  options={this.state.preguntas7}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.ViveDomicilioId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />

                              </div>
                              <div className="col">
                                <Field
                                  name="MedicamentoDiaId"
                                  required
                                  value={this.state.MedicamentoDiaId}
                                  label="8. Toma más de 3 medicamentos al día? "
                                  options={this.state.preguntas8}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.MedicamentoDiaId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />


                              </div>
                            </div>

                            <div className="row">
                              <div className="col">
                                <Field
                                  name="UlceraLesionId"
                                  required
                                  value={this.state.UlceraLesionId}
                                  label="9. Úlceras o lesiones cutáneas?"
                                  options={this.state.preguntas9}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.UlceraLesionId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />

                              </div>
                              <div className="col">
                                <Field
                                  name="ComidaDiariaId"
                                  required
                                  value={this.state.ComidaDiariaId}
                                  label="10. Cuántas comidas completas toma al día?"
                                  options={this.state.preguntas10}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.ComidaDiariaId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />


                              </div>
                            </div>

                            <div className="row">

                              <div className="col">
                                <div className="row">
                                  <div className="col">
                                    <p>11. Consume el paciente</p>
                                  </div>
                                </div>
                                <div className="row">
                                  <div className="col-8">
                                    <label htmlFor="ab1" className="p-checkbox-label">
                                      ¿Productos lácteos al menos una vez al día?
                                    </label> {" "}
                                  </div>
                                  <div className="col-4">
                                    <ToggleButton style={{ width: '50px' }}
                                      checked={this.state.ConsumeLacteos}
                                      onChange={(e) => this.changeBoxLacters(e.value)}
                                      onLabel="SI"
                                      offLabel="NO"
                                    />
                                  </div>
                                </div>
                                <div className="row">
                                  <div className="col-8">
                                    <label htmlFor="ab2" className="p-checkbox-label">
                                      ¿Huevos o legumbres una o dos veces a la  semana?
                                    </label>
                                  </div>
                                  <div className="col-4">
                                    <ToggleButton style={{ width: '50px' }}
                                      checked={this.state.ConsumeLegumbres}
                                      onChange={(e) => this.changeBoxLeg(e.value)}
                                      onLabel="SI"
                                      offLabel="NO" />

                                  </div>
                                </div>

                                <div className="row">
                                  <div className="col-8">
                                    <label htmlFor="ab3" className="p-checkbox-label">
                                      ¿Carne, pescado o aves diariamente?
                                    </label>
                                  </div>
                                  <div className="col-4">
                                    <ToggleButton style={{ width: '50px' }}
                                      checked={this.state.ConsumeCarne}
                                      onChange={(e) => this.changeBoxCarnes(e.value)} onLabel="SI"
                                      offLabel="NO" />
                                  </div>
                                </div>



                                <Field
                                  name="ConsumoPersonaId"
                                  required
                                  value={this.state.ConsumoPersonaId}
                                  label="Valoración"
                                  options={this.state.preguntas11}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.ConsumoPersonaId}
                                  edit={false}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />

                              </div>
                              <div className="col">
                                <Field
                                  name="ConsumoFrutasVerdurasId"
                                  required
                                  value={this.state.ConsumoFrutasVerdurasId}
                                  label="12.Consume frutas o verduras al menos dos veces al día?"
                                  options={this.state.preguntas12}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.ConsumoFrutasVerdurasId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />


                              </div>
                            </div>
                            <hr></hr>
                            <div className="row">
                              <div className="col">
                                <Field
                                  name="NumeroVasosAguaId"
                                  required
                                  value={this.state.NumeroVasosAguaId}
                                  label="13. Cuantos vasos de agua u otros líquidos toma al día? (agua, zumos, café, te, leche)"
                                  options={this.state.preguntas13}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.NumeroVasosAguaId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />

                              </div>
                              <div className="col">
                                <Field
                                  name="ModoAlimentarseId"
                                  required
                                  value={this.state.ModoAlimentarseId}
                                  label="14. Forma de alimentarse"
                                  options={this.state.preguntas14}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.ModoAlimentarseId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />


                              </div>
                            </div>

                            <div className="row">
                              <div className="col">
                                <Field
                                  name="ConsideracionEnfermoId"
                                  required
                                  value={this.state.ConsideracionEnfermoId}
                                  label="15. Se considera el paciente que está bien nutrido?"
                                  options={this.state.preguntas15}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.ConsideracionEnfermoId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />

                              </div>
                              <div className="col">
                                <Field
                                  name="EstadoSaludId"
                                  required
                                  value={this.state.EstadoSaludId}
                                  label="16. En comparación con las personas de su edad, cómo encuentra el
                                  paciente su estado de salud?"
                                  options={this.state.preguntas16}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.EstadoSaludId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />


                              </div>
                            </div>

                            <div className="row">
                              <div className="col">
                                <Field
                                  name="CircunferenciaBraquialId"
                                  required
                                  value={this.state.CircunferenciaBraquialId}
                                  label="17. Circunferencia braquial (CB en cm.)"
                                  options={this.state.preguntas17}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.CircunferenciaBraquialId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />


                              </div>
                              <div className="col">
                                <Field
                                  name="CircunferenciaPiernaId"
                                  required
                                  value={this.state.CircunferenciaPiernaId}
                                  label="18. Circunferencia braquial (CB en cm)"
                                  options={this.state.preguntas18}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.CircunferenciaPiernaId}
                                  readOnly={false}
                                  placeholder="Seleccione.."
                                  filterPlaceholder="Seleccione.."
                                />


                              </div>
                            </div>

                            <div className="row">

                              <div className="col">

                              </div>
                              <div className="col" style={{ fontSize: '20px' }}>
                                <label><strong>Puntuación Total:  </strong>{this.state.total2}</label>
                              </div>
                            </div>

                          </div>
                        </div>
                        {this.state.total2 >= 24 && (
                          <div className="alert alert-success" role="alert" style={{ fontSize: '20px' }}>

                            <b >De 24 a 30 puntos: </b> Estado nutricional normal.
                          </div>
                        )}
                        {(this.state.total2 != 0 && this.state.total2 > 16.5 && this.state.total2 <= 23.5) && (
                          <div className="alert alert-warning" role="alert" style={{ fontSize: '20px' }}>
                            <b >De 17 a 23.5 puntos: </b>Riesgo de malnutrición.
                          </div>
                        )}
                        {(this.state.total2 != 0 && this.state.total2 < 17) && (
                          <div className="alert alert-danger" role="alert" style={{ fontSize: '20px' }}>
                            <b > menos de 17 puntos: </b> Malnutrición.
                          </div>
                        )}


                        <button type="submit" className="btn btn-outline-primary">
                          Guardar
                        </button>
                        &nbsp;
                        <button
                          type="button"
                          className="btn btn-outline-primary"
                          icon="fa fa-fw fa-ban"
                          onClick={this.onHide}
                        >
                          Cancelar
                        </button>
                      </form>
                    </div>
                  )
                  }

                </div>
              </div>
            </TabPanel>

            <TabPanel header="ESCALA KATZ">
              <div className="row">
                <div className="col">
                  <div align="right">

                    <button
                      type="button"
                      className="btn btn-outline-primary"
                      icon="fa fa-fw fa-ban"
                      onClick={this.mostrarFormKatz}
                    >
                      Nuevo Registro
                    </button>


                  </div>
                  <br></br>
                  {this.state.viewKatz === false && (
                    <BootstrapTable
                      data={this.state.detalles.Katzs}
                      hover={true}
                      pagination={true}

                    >
                      <TableHeaderColumn
                        dataField="any"
                        dataFormat={this.Secuencial}
                        width={"8%"}
                        tdStyle={{
                          whiteSpace: "normal",
                          textAlign: "center",
                          fontSize: "11px",
                        }}
                        thStyle={{
                          whiteSpace: "normal",
                          textAlign: "center",
                          fontSize: "11px",
                        }}
                        editable={false}
                      >
                        Nº
                      </TableHeaderColumn>
                      <TableHeaderColumn

                        dataField="FechaRegistro"
                        filter={{ type: "TextFilter", delay: 500 }}
                        tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                        thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                        dataSort={true}
                        editable={false}
                        dataFormat={this.formatoFechaCorta}
                      >
                        Fecha
                      </TableHeaderColumn>

                      <TableHeaderColumn

                        dataField="Puntuacion"
                        filter={{ type: "TextFilter", delay: 500 }}
                        tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                        thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                        dataSort={true}
                        editable={false}
                      >
                        Puntuación
                      </TableHeaderColumn>
                      <TableHeaderColumn

                        dataField="Calificacion"

                        filter={{ type: "TextFilter", delay: 500 }}
                        tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                        thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                        dataSort={true}
                        editable={false}
                      >
                        Calificación
                      </TableHeaderColumn>

                      <TableHeaderColumn

                        dataField="CalificacionDependiente"

                        filter={{ type: "TextFilter", delay: 500 }}
                        tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                        thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                        dataSort={true}
                        editable={false}
                      >
                        Grado
                      </TableHeaderColumn>
                      <TableHeaderColumn
                        dataField="Id"
                        isKey
                        width={"14%"}
                        dataFormat={this.generarBotonesDetallesKtz}
                        thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                      >
                        Opciones
                      </TableHeaderColumn>
                    </BootstrapTable>
                  )}
                  {this.state.viewKatz && (
                    <Card >

                      <form onSubmit={this.EnviarFormularioKTZ}>
                        <div className="row">
                          <div className="col-2"><b>ACTIVIDAD</b></div>
                          <div className="col-6"><b>DESCRIPCIÓN DE DEPENDENCIA</b></div>
                          <div className="col-4"><b>PUNTUACIÓN</b></div>
                        </div>
                        <hr></hr>
                        <div className="row">
                          <div className="col-2"><b>1) Baño (Esponja, regadera o tina)</b></div>
                          <div className="col-6">
                            <p><b>Independiente:</b> Necesita ayuda para lavarse una sola parte (con la espalda o una extremidad incapacitada) o se baña completamente sin ayuda.</p>
                            <p><b>Dependiente:</b>  Necesita ayuda para lavarse más de una parte del cuerpo, para salir o entrar en la bañera o no se lava solo.</p>
                          </div>
                          <div className="col-4">
                            <Field
                              name="Bano"
                              required
                              value={this.state.Bano}
                              label="Seleccione Dependencia"
                              options={this.state.opcionkatz}
                              type={"select"}
                              // filter={true}
                              onChange={this.onChangeValueDependencia}
                              error={this.state.errorsKatz.Bano}
                              readOnly={false}
                              placeholder="Seleccione.."
                              filterPlaceholder="Seleccione.."
                            />

                          </div>
                        </div>
                        <hr></hr>
                        <div className="row">
                          <div className="col-2"><b>2) Vestido</b></div>
                          <div className="col-6">
                            <p><b>Independiente:</b> Coge la ropa de cajones y armarios, se la pone y puede abrocharse. Se excluye el acto de atarse los zapatos.</p>
                            <p><b>Dependiente:</b> No se viste por sí mismo o permanece parcialmente desvestido.</p>
                          </div>
                          <div className="col-4">
                            <Field
                              name="Vestido"
                              required
                              value={this.state.Vestido}
                              label="Seleccione Dependencia"
                              options={this.state.opcionkatz}
                              type={"select"}
                              // filter={true}
                              onChange={this.onChangeValueDependencia}
                              error={this.state.errorsKatz.Vestido}
                              readOnly={false}
                              placeholder="Seleccione.."
                              filterPlaceholder="Seleccione.."
                            />

                          </div>
                        </div>
                        <hr></hr>
                        <div className="row">
                          <div className="col-2"><b>3) Uso del sanitario</b></div>
                          <div className="col-6">
                            <p><b>Independiente:</b> Accede al sanitario, entra y sale de él, se limpia los órganos excretores y se arregla la ropa.</p>
                            <p><b>Dependiente:</b> Necesita ayuda para ir al sanitario.</p>
                          </div>
                          <div className="col-4">
                            <Field
                              name="Sanitario"
                              required
                              value={this.state.Sanitario}
                              label="Seleccione Dependencia"
                              options={this.state.opcionkatz}
                              type={"select"}
                              // filter={true}
                              onChange={this.onChangeValueDependencia}
                              error={this.state.errorsKatz.Sanitario}
                              readOnly={false}
                              placeholder="Seleccione.."
                              filterPlaceholder="Seleccione.."
                            />

                          </div>
                        </div>
                        <hr></hr>

                        <div className="row">
                          <div className="col-2"><b>4) Movilidad</b></div>
                          <div className="col-6">
                            <p><b>Independiente:</b> Se levanta y acuesta en la cama por sí mismo y puede sentarse y levantarse de una silla por sí mismo.</p>
                            <p><b>Dependiente:</b> Necesita ayuda para levantarse y acostarse en la cama y/o silla, no realiza uno o más desplazamientos.</p>
                          </div>
                          <div className="col-4">
                            <Field
                              name="Transferencias"
                              required
                              value={this.state.Transferencias}
                              label="Seleccione Dependencia"
                              options={this.state.opcionkatz}
                              type={"select"}
                              // filter={true}
                              onChange={this.onChangeValueDependencia}
                              error={this.state.errorsKatz.Transferencias}
                              readOnly={false}
                              placeholder="Seleccione.."
                              filterPlaceholder="Seleccione.."
                            />

                          </div>
                        </div>
                        <hr></hr>
                        <div className="row">
                          <div className="col-2"><b>5) Continencia</b></div>
                          <div className="col-6">
                            <p><b>Independiente:</b> Control completo de micción y defecación.</p>
                            <p><b>Dependiente:</b> Incontinencia urinaria o fecal parcial o total.</p>
                          </div>
                          <div className="col-4">
                            <Field
                              name="Continencia"
                              required
                              value={this.state.Continencia}
                              label="Seleccione Dependencia"
                              options={this.state.opcionkatz}
                              type={"select"}
                              // filter={true}
                              onChange={this.onChangeValueDependencia}
                              error={this.state.errorsKatz.Continencia}
                              readOnly={false}
                              placeholder="Seleccione.."
                              filterPlaceholder="Seleccione.."
                            />

                          </div>
                        </div>
                        <hr></hr>

                        <div className="row">
                          <div className="col-2"><b>6) Alimentación</b></div>
                          <div className="col-6">
                            <p><b>Independiente:</b> Lleva el alimento a la boca desde el plato o equivalente. Se excluye cortar la carne.</p>
                            <p><b>Dependiente:</b> Necesita ayuda para comer, no come en absoluto o requiere alimentación parenteral.</p>
                          </div>
                          <div className="col-4">
                            <Field
                              name="Alimentacion"
                              required
                              value={this.state.Alimentacion}
                              label="Seleccione Dependencia"
                              options={this.state.opcionkatz}
                              type={"select"}
                              // filter={true}
                              onChange={this.onChangeValueDependencia}
                              error={this.state.errorsKatz.Alimentacion}
                              readOnly={false}
                              placeholder="Seleccione.."
                              filterPlaceholder="Seleccione.."
                            />

                          </div>
                        </div>
                        <hr></hr>


                        <div className="row">
                          <div className="col-6">
                            <p> <b>CLASIFICACIÓN</b></p>
                            <p>A. Independiente en todas sus funciones.</p>
                            <p>B. Independiente en todas las funciones menos en una de ellas.</p>
                            <p>C. Independiente en todas las funciones menos en el baño y otra cualquiera.</p>
                            <p>D. Independiente en todas las funciones menos en el baño, vestido y otra cualquiera.</p>
                            <p>E. Independiente en todas las funciones menos en el baño, vestido, uso del sanitario y otra cualquiera.</p>
                            <p>F. Independencia en todas las funciones menos en el baño, vestido, uso del sanitario, movilidad y otra cualquiera de las dos restantes.</p>
                            <p>G. Dependiente en todas las funciones.</p>
                            <p>H. Dependiente en al menos dos funciones, pero no clasificable como C, D, E o F.</p>
                          </div>
                          <div className="col-3">
                            <p> <b>RESULTADO:</b></p>
                            <p style={{ fontSize: "15px" }}>{this.state.totalkat}/6</p>
                          </div>
                          <div className="col-3">
                            <p> <b>CALIFICACIÓN INDEPENDENCIA:</b></p>
                            <p style={{ fontSize: "15px" }}>{this.state.Calificacion}</p>
                            <hr></hr>
                            <p> <b>GRADO DE DEPENDENCIA:</b></p>
                            <p style={{ fontSize: "15px" }}>{this.state.CalificacionDependencia}</p>
                          </div>
                        </div>

                        <hr></hr>
                        <button type="submit" className="btn btn-outline-primary">
                          Guardar
                        </button>
                        &nbsp;
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
                  )
                  }
                </div>
              </div>
            </TabPanel>
          </TabView>
        </Panel>
      )
    } else {
      return (
        <Card subTitle="Paciente">
          <div>
            <form onSubmit={this.EnviarFormulario}>
              <div className="row">
                <div className="col">
                  <Field
                    name="Identificacion"
                    label="Identificación"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Identificacion}
                    onChange={this.handleChange}
                    error={this.state.errors.Identificacion}
                  />
                </div>
                <div className="col">
                  <Field
                    name="NombresApellidos"
                    label="Nombres y Apellidos"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.NombresApellidos}
                    onChange={this.handleChange}
                    error={this.state.errors.NombresApellidos}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="Edad"
                    label="Edad (años)"
                    type="number"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Edad}
                    onChange={this.handleChange}
                    error={this.state.errors.Edad}
                  />
                </div>
                <div className="col">
                  <Field
                    name="SexoId"
                    required
                    value={this.state.SexoId}
                    label="Sexo"
                    options={this.state.catalogosexo}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.SexoId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="Talla"
                    label="Talla (m)"
                    type="number"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Talla}
                    onChange={this.handleChange}
                    error={this.state.errors.Talla}
                  />
                </div>
                <div className="col">
                  <Field
                    name="Peso"
                    label="Peso (Kg)"
                    type="number"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.Peso}
                    onChange={this.handleChange}
                    error={this.state.errors.Peso}
                  />
                </div>
              </div>
              <hr></hr>


              <div className="row">
                <div className="col">
                  <Field
                    name="CentroId"
                    required
                    value={this.state.CentroId}
                    label="Centro Geriátrico "
                    options={this.state.Centros}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.CentroId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="GrupoEdad"
                    required
                    value={this.state.GrupoEdad}
                    label="Grupo Edad"
                    options={this.state.gruposEdad}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.GrupoEdad}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="NivelEducativo"
                    required
                    value={this.state.NivelEducativo}
                    label="Nivel Educativo "
                    options={this.state.niveles}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.NivelEducativo}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="EstadoCivil"
                    required
                    value={this.state.EstadoCivil}
                    label="Estado Civil"
                    options={this.state.estados}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.EstadoCivil}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="ViveSolo"
                    required
                    value={this.state.ViveSolo}
                    label="Vive Solo "
                    options={this.state.opciones}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.ViveSolo}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="AutoReporteSalud"
                    required
                    value={this.state.AutoReporteSalud}
                    label="Auto Reporte Salud"
                    options={this.state.tipoSalud}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.AutoReporteSalud}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>

              <div className="row">
                <div className="col">
                  <Field
                    name="ConsumeAlcohol"
                    required
                    value={this.state.ConsumeAlcohol}
                    label="Consume Alcohol "
                    options={this.state.opcionesAlcohol}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.ConsumeAlcohol}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="ConsumeCigarillo"
                    required
                    value={this.state.ConsumeCigarillo}
                    label="Consume Cigarillo"
                    options={this.state.opcionesAlcohol}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.ConsumeCigarillo}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="HipertencionArterial"
                    required
                    value={this.state.HipertencionArterial}
                    label="Hipertención Arterial "
                    options={this.state.opciones}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.HipertencionArterial}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="InsuficienciaArterial"
                    required
                    value={this.state.InsuficienciaArterial}
                    label="Diabetes"
                    options={this.state.opciones}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.InsuficienciaArterial}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>

              <div className="row">
                <div className="col">
                  <Field
                    name="InsuficienciaCardicaCongestiva"
                    required
                    value={this.state.InsuficienciaCardicaCongestiva}
                    label="Insuficiencia Cardiaca Congestiva "
                    options={this.state.opciones}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.InsuficienciaCardicaCongestiva}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="Epoc"
                    required
                    value={this.state.Epoc}
                    label="EPOC"
                    options={this.state.opciones}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.Epoc}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="EnfermedadCerebroVascular"
                    required
                    value={this.state.EnfermedadCerebroVascular}
                    label="Enfermedad CerebroVascular "
                    options={this.state.opciones}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.EnfermedadCerebroVascular}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">

                </div>
              </div>
              {/*  <div className="row">
                <div className="col">
                  <Field
                    name="Hospitalizacion"
                    required
                    value={this.state.Hospitalizacion}
                    label="Hospitalizaciones el último año"
                    options={this.state.opcionesVisitas}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.Hospitalizacion}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="Emergencia"
                    required
                    value={this.state.Emergencia}
                    label="Visitas a emergencias el último año"
                    options={this.state.opcionesVisitas}
                    type={"select"}
                    //filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.Emergencia}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>
              */}
              <button type="submit" className="btn btn-outline-primary">
                Guardar
              </button>
              &nbsp;
              <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={this.OcultarFormularioOrden}
              >
                Cancelar
              </button>
            </form>
          </div>
        </Card>
      )
    }
  }

  valorFormat = (cell, row) => {
    return (
      <CurrencyFormat
        value={cell}
        displayType={"text"}
        thousandSeparator={true}
        prefix={"$"}
      />
    )
  }

  EnviarFormulario = (event) => {
    event.preventDefault()
    this.props.blockScreen()

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      )
      this.props.unlockScreen()
      return
    } else {
      if (this.state.action == "create") {
        axios
          .post("/Documentos/Documento/CreateP", {
            Id: 0,
            Identificacion: this.state.Identificacion,
            NombresApellidos: this.state.NombresApellidos,
            SexoId: this.state.SexoId,
            Talla: this.state.Talla,
            Peso: this.state.Peso,
            Edad: this.state.Edad,
            CentroId: this.state.CentroId,

            GrupoEdad: this.state.GrupoEdad,
            NivelEducativo: this.state.NivelEducativo,
            EstadoCivil: this.state.EstadoCivil,
            ViveSolo: this.state.ViveSolo,
            ConsumeAlcohol: this.state.ConsumeAlcohol,
            ConsumeCigarillo: this.state.ConsumeCigarillo,
            AutoReporteSalud: this.state.AutoReporteSalud,
            HipertencionArterial: this.state.HipertencionArterial,
            InsuficienciaArterial: this.state.InsuficienciaArterial,
            InsuficienciaCardicaCongestiva: this.state.InsuficienciaCardicaCongestiva,
            Epoc: this.state.Epoc,
            EnfermedadCerebroVascular: this.state.EnfermedadCerebroVascular,
            Hospitalizacion: this.state.Hospitalizacion,
            Emergencia: this.state.Emergencia


          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("Paciente Guardado", "Correcto")
              this.setState({ action: "list" })
              this.GetList()
            }

          })
          .catch((error) => {
            console.log(error)
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            )
          })
      } else {

        axios
          .post("/Documentos/Documento/FGetEdit", {
            Id: this.state.Id,
            Identificacion: this.state.Identificacion,
            NombresApellidos: this.state.NombresApellidos,
            SexoId: this.state.SexoId,
            Talla: this.state.Talla,
            Peso: this.state.Peso,
            Edad: this.state.Edad,
            CentroId: this.state.CentroId,

            GrupoEdad: this.state.GrupoEdad,
            NivelEducativo: this.state.NivelEducativo,
            EstadoCivil: this.state.EstadoCivil,
            ViveSolo: this.state.ViveSolo,
            ConsumeAlcohol: this.state.ConsumeAlcohol,
            ConsumeCigarillo: this.state.ConsumeCigarillo,
            AutoReporteSalud: this.state.AutoReporteSalud,
            HipertencionArterial: this.state.HipertencionArterial,
            InsuficienciaArterial: this.state.InsuficienciaArterial,
            InsuficienciaCardicaCongestiva: this.state.InsuficienciaCardicaCongestiva,
            Epoc: this.state.Epoc,
            EnfermedadCerebroVascular: this.state.EnfermedadCerebroVascular,
            Hospitalizacion: this.state.Hospitalizacion,
            Emergencia: this.state.Emergencia
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success(" Guardado", "Correcto")
              this.setState({ action: "list" })
              this.GetList()
            }

          })
          .catch((error) => {
            console.log(error)
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            )
          })
      }
    }
  }
  EnviarFormularioMNA = (event) => {
    event.preventDefault()
    this.props.blockScreen()

    if (!this.isValidMNA()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      )
      this.props.unlockScreen()
      return
    } else {
      if (this.state.actionMNA == "createMNA") {
        axios
          .post("/Documentos/Documento/CreatePMNA", {
            Id: 0,
            PacienteId: this.state.pacienteSeleccionado.Id,
            Fecha: this.state.Fecha,
            PerdidaApetitoId: this.state.PerdidaApetitoId,
            PerdidaPesoId: this.state.PerdidaPesoId,
            MovilidadId: this.state.MovilidadId,
            EnfermedadAgudaId: this.state.EnfermedadAgudaId,
            ProblemasNeuroId: this.state.ProblemasNeuroId,
            IndiceMasaId: this.state.IndiceMasaId,
            ViveDomicilioId: this.state.ViveDomicilioId,
            MedicamentoDiaId: this.state.MedicamentoDiaId,
            UlceraLesionId: this.state.UlceraLesionId,
            ComidaDiariaId: this.state.ComidaDiariaId,
            ConsumoPersonaId: this.state.ConsumoPersonaId,
            ConsumeLacteos: this.state.ConsumeLacteos,
            ConsumeLegumbres: this.state.ConsumeLegumbres,
            ConsumeCarne: this.state.ConsumeCarne,
            ConsumoFrutasVerdurasId: this.state.ConsumoFrutasVerdurasId,
            NumeroVasosAguaId: this.state.NumeroVasosAguaId,
            ModoAlimentarseId: this.state.ModoAlimentarseId,
            ConsideracionEnfermoId: this.state.ConsideracionEnfermoId,
            EstadoSaludId: this.state.EstadoSaludId,
            CircunferenciaBraquialId: this.state.CircunferenciaBraquialId,
            CircunferenciaPiernaId: this.state.CircunferenciaPiernaId,
            Puntuacion: this.state.total2,
            ValoracionCompleta: this.state.total11 < 12 ? true : false,
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("MNA Guardado", "Correcto")
              this.setState({ action: "detalles", view: false })
              this.GetListDetalles(this.state.pacienteSeleccionado.Id)
            }

          })
          .catch((error) => {
            console.log(error)
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            )
          })
      } else {

        axios
          .post("/Documentos/Documento/FGetEditMNA", {
            Id: this.state.IdMNA,
            PacienteId: this.state.pacienteSeleccionado.Id,
            Fecha: this.state.Fecha,
            PerdidaApetitoId: this.state.PerdidaApetitoId,
            PerdidaPesoId: this.state.PerdidaPesoId,
            MovilidadId: this.state.MovilidadId,
            EnfermedadAgudaId: this.state.EnfermedadAgudaId,
            ProblemasNeuroId: this.state.ProblemasNeuroId,
            IndiceMasaId: this.state.IndiceMasaId,
            ViveDomicilioId: this.state.ViveDomicilioId,
            MedicamentoDiaId: this.state.MedicamentoDiaId,
            UlceraLesionId: this.state.UlceraLesionId,
            ComidaDiariaId: this.state.ComidaDiariaId,
            ConsumoPersonaId: this.state.ConsumoPersonaId,
            ConsumeLacteos: this.state.ConsumeLacteos,
            ConsumeLegumbres: this.state.ConsumeLegumbres,
            ConsumeCarne: this.state.ConsumeCarne,
            ConsumoFrutasVerdurasId: this.state.ConsumoFrutasVerdurasId,
            NumeroVasosAguaId: this.state.NumeroVasosAguaId,
            ModoAlimentarseId: this.state.ModoAlimentarseId,
            ConsideracionEnfermoId: this.state.ConsideracionEnfermoId,
            EstadoSaludId: this.state.EstadoSaludId,
            CircunferenciaBraquialId: this.state.CircunferenciaBraquialId,
            CircunferenciaPiernaId: this.state.CircunferenciaPiernaId,
            Puntuacion: this.state.total2,
            ValoracionCompleta: this.state.total11 < 12 ? true : false,
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success(" Guardado", "Correcto")
              this.setState({ action: "detalles", view: "" })
              this.GetListDetalles(this.state.pacienteSeleccionado.Id)
            }

          })
          .catch((error) => {
            console.log(error)
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            )
          })
      }
    }
  }

  EnviarFormularioKTZ = (event) => {
    event.preventDefault()
    this.props.blockScreen()

    if (!this.isValidaKatz) {
      abp.notify.error(
        "Debe completar datos del formulario Katz",
        "Validación"
      )
      this.props.unlockScreen()
      return
    } else {
      if (this.state.actionKatz == "createKatz") {
        axios
          .post("/Documentos/Documento/CreatePK", {
            Id: 0,
            PacienteId: this.state.pacienteSeleccionado.Id,
            Bano: this.state.Bano,
            Vestido: this.state.Vestido,

            Sanitario: this.state.Sanitario,

            Transferencias: this.state.Transferencias,
            Continencia: this.state.Continencia,
            Alimentacion: this.state.Alimentacion,

            Calificacion: this.state.Calificacion,
            CalificacionDependiente: this.state.CalificacionDependencia,
            Puntuacion: this.state.totalkat

          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("Paciente Guardado", "Correcto")
              this.setState({ action: "detalles", view: false, viewKatz: false })
              this.GetListDetalles(this.state.pacienteSeleccionado.Id)
            }

          })
          .catch((error) => {
            console.log(error)
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            )
          })
      } else {

        axios
          .post("/Documentos/Documento/FGetEditK", {
            Id: this.state.IdKATZ,
            PacienteId: this.state.pacienteSeleccionado.Id,
            Bano: this.state.Bano,
            Vestido: this.state.Vestido,

            Sanitario: this.state.Sanitario,

            Transferencias: this.state.Transferencias,
            Continencia: this.state.Continencia,
            Alimentacion: this.state.Alimentacion,

            Calificacion: this.state.Calificacion,
            CalificacionDependiente: this.state.CalificacionDependencia,
            Puntuacion: this.state.Puntuacion
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success(" Guardado", "Correcto")
              this.setState({ action: "detalles", view: false, viewKatz: false })
              this.GetListDetalles(this.state.pacienteSeleccionado.Id)
            }

          })
          .catch((error) => {
            console.log(error)
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            )
          })
      }
    }
  }

  EnviarFormularioDetalle = (event) => {
    event.preventDefault()
    this.props.blockScreen()

    if (!this.isValidDetalle()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      )
      this.props.unlockScreen()
      return
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
              abp.notify.success("Guardado", "Correcto")
              this.setState({ dialogdetalles: false })
              this.GetListDetalles(this.state.po.Id)
              this.GetDetalleOS(this.state.po.Id)
            } else {
              abp.notify.error(
                "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
                "Error"
              )
            }
          })
          .catch((error) => {
            console.log(error)
            abp.notify.error(error, "Error")
          })
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
              abp.notify.success("Guardado", "Correcto")
              this.setState({ dialogdetalles: false })
              this.GetListDetalles(this.state.po.Id)
              this.GetDetalleOS(this.state.po.Id)
            } else {
              abp.notify.error(
                "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
                "Error"
              )
            }
          })
          .catch((error) => {
            console.log(error)
            abp.notify.error(
              "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
              "Error"
            )
          })
      }
    }
  }

  onDownloaImagen = (Id) => {
    return (window.location = `/Proyecto/AvanceObra/Descargar/${Id}`)
  }

  MostrarFormulario() {
    this.setState({ visible: true })
  }

  onHide = () => {
    this.setState({ action: "detalles", view: false, viewKatz: false })
  }
  formatoFechaCorta = (cell, row) => {
    if (cell != null) {
      var informacion = cell.split("T");
      return informacion[0];
    } else {
      return null;
    }
  };

  handleChangebox = (event) => {
    console.log(event.target);
    const target = event.target;
    const value = target.type === "checkbox" ? target.checked : target.value;
    const name = target.name;
    this.setState({ [name]: value })


    setTimeout(() => {
      this.seleccionarConsumePersona()
    }, 500);
  };

  changeBoxLacters = (value) => {
    this.setState({ ConsumeLacteos: value });
    setTimeout(() => {
      this.seleccionarConsumePersona()

      this.GetTotal1()
    }, 500);
  }
  changeBoxCarnes = (value) => {
    this.setState({ ConsumeCarne: value });
    setTimeout(() => {
      this.seleccionarConsumePersona()
      this.GetTotal1()
    }, 500);

  }
  changeBoxLeg = (value) => {

    this.setState({ ConsumeLegumbres: value });
    setTimeout(() => {
      this.seleccionarConsumePersona()
      this.GetTotal1()
    }, 500);
  }
  seleccionarConsumePersona = () => {
    var totalSI = 0;

    if (this.state.ConsumeLacteos) {
      totalSI = totalSI + 1;
    }
    if (this.state.ConsumeLegumbres) {
      totalSI = totalSI + 1;
    }
    if (this.state.ConsumeCarne) {
      totalSI = totalSI + 1;
    }
    if (totalSI === 0 || totalSI === 1) {
      this.setState({ ConsumoPersonaId: 11392 });
    }
    if (totalSI === 2) {
      this.setState({ ConsumoPersonaId: 11393 });
    }
    if (totalSI === 3) {
      this.setState({ ConsumoPersonaId: 11394 });
    }

  }

  handleChange(event) {
    if (event.target.files) {
      console.log(event.target.files)
      let files = event.target.files || event.dataTransfer.files
      if (files.length > 0) {
        let uploadFile = files[0]
        this.setState({
          uploadFile: uploadFile,
        })
      }
    } else {
      this.setState({ [event.target.name]: event.target.value })
    }
  }

  OcultarFormulario = () => {
    this.setState({ visible: false })
  }
}
const Container = wrapForm(PacienteContainer)
ReactDOM.render(<Container />, document.getElementById("content"))
