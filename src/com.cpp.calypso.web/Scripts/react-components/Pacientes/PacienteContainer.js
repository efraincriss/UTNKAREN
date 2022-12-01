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


class PacienteContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      errors: {},
      errorsMNA: {},
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
      ConsumoPersonaId: 0,
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
      Bano: false,
      Vestido: false,

      Sanitario: false,

      Transferencias: false,
      Continencia: false,
      Alimentacion: false,

      Calificacion: "",

    }

    this.handleChange = this.handleChange.bind(this)

    this.handleChangebox = this.handleChangebox.bind(this)
    this.onChangeValue = this.onChangeValue.bind(this)
    this.onChangeValueT = this.onChangeValueT.bind(this)
    this.mostrarForm = this.mostrarForm.bind(this)
    this.mostrarFormMNA = this.mostrarFormMNA.bind(this)
    this.mostrarFormKatz = this.mostrarFormKatz.bind(this)
    this.OcultarFormulario = this.OcultarFormulario.bind()
    this.isValid = this.isValid.bind()
    this.isValidMNA = this.isValidMNA.bind()
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
          preguntas18: items18
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
    if (this.state.Edad == 0) {
      errors.Edad = "Campo Requerido"
    }
    if (this.state.Talla == 0) {
      errors.Talla = "Campo Requerido"
    }
    if (this.state.Peso == 0) {
      errors.Peso = "Campo Requerido"
    }

    this.setState({ errors })
    return Object.keys(errors).length === 0
  }
  contarKatz = () => {
    let total = 0;
    if (this.state.Bano) {
      total = total + 1;
    }
    if (this.state.Vestido) {
      total = total + 1;
    }
    if (this.state.Sanitario) {
      total = total + 1;
    }
    if (this.state.Transferencias) {
      total = total + 1;
    }
    if (this.state.Continencia) {
      total = total + 1;
    }
    if (this.state.Alimentacion) {
      total = total + 1;
    }
    this.setState({ totalkat: total });

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
        total11: row.Puntuacion,
        total2: row.Puntuacion

      })
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
        ConsumoPersonaId: 0,
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
        totalkat: row.Puntuacion
      })
    } else {
      this.setState({
        IdKATZ: 0,
        viewKatz: true,
        view: false,
        actionKatz: "createKatz",
        Bano: false,
        Vestido: false,

        Sanitario: false,

        Transferencias: false,
        Continencia: false,
        Alimentacion: false,

        Calificacion: "",




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
                          <div className="col" align="right">
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
                                  label="1. Ha perdido el apetito? Ha comido menos por tener hambre, problemas digestivos, dificultad para masticar o alimentarse en lo últimos tres meses."
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
                                  label="4.Ha habido enfermedad aguda o situación de estrés psicológico en los últimos tres meses "
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
                                  label="6.Índice de masa corporal "
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
                              <div className="alert alert-success" role="alert">

                                <b >≥12 puntos: </b>Normal, no es necesaria una valoración completa.
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
                        {(this.state.total11 != 0 && this.state.total11 < 12) && (
                          <div className="alert alert-danger" role="alert">
                            <b >≤ 11 puntos: </b>Posible malnutrición, continuar con la valoración.
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
                                  label="7. La persona vive en su domicilio"
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
                                  label="8. Toma más de 3 medicamentos al día "
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
                                  label="9. Úlceras o lesiones cutáneas"
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
                                  label="10. Cuantas comidas hace al día?"
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
                                <Field
                                  name="ConsumoPersonaId"
                                  required
                                  value={this.state.ConsumoPersonaId}
                                  label="11. La persona consume?"
                                  options={this.state.preguntas11}
                                  type={"select"}
                                  // filter={true}
                                  onChange={this.onChangeValueT}
                                  error={this.state.errorsMNA.ConsumoPersonaId}
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
                                  label="12.Consume frutas o verduras por lo menos dos veces al día?"
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

                            <div className="row">
                              <div className="col">
                                <Field
                                  name="NumeroVasosAguaId"
                                  required
                                  value={this.state.NumeroVasosAguaId}
                                  label="13. Cuantos vasos de agua o otros líquidos toma al día?"
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
                                  label="14.Consume frutas o verduras por lo menos dos veces al día?"
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
                                  label="15. El enfermo se considera, a él mismo bien nutrido (problemas nutricionales) ?"
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
                                  label="16. Comparándose con las personas de su 16.edad. Como esta su estado de salud?"
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
                                  label="18. Circunferencia de la pierna (CC en cm.)"
                                  options={this.state.preguntas15}
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
                              <div className="col">
                                <label><strong>Puntuación Total:  </strong>{this.state.total2}</label>
                              </div>
                            </div>

                          </div>
                        </div>
                        {this.state.total2 >= 24 && (
                          <div className="alert alert-success" role="alert">

                            <b >≥24 puntos: </b> Estado nutricional satifactorio.
                          </div>
                        )}
                        {(this.state.total2 != 0 && this.state.total2 > 16 && this.state.total2 <= 23.5) && (
                          <div className="alert alert-warning" role="alert">
                            <b >17 a 23.5 puntos: </b>Riesgo de malnutrición.
                          </div>
                        )}
                        {(this.state.total2 != 0 && this.state.total2 < 17) && (
                          <div className="alert alert-danger" role="alert">
                            <b > menos de 17 puntos: </b> Mal estado nutricional.
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
                  {this.state.viewKatz && (
                    <Card title="KATZ">

                      <form onSubmit={this.EnviarFormularioKTZ}>
                        <div className="row align-items-center">
                          <div className="col">
                            <h4> 1) Baño (Esponja, regadera o tina)</h4>
                            <p> Sí: No recibe asistencia (puede entrar y salir de la tina u otra forma de baño).
                            </p>
                            <p>Sí: Que reciba asistencia durante el baño en una sola parte del cuerpo
                              (ej. espalda o pierna). </p>
                            <p>  No: Que reciba asistencia durante el baño en más de una parte.</p>
                          </div>

                          <div className="col">
                            <Fragment>
                              <Checkbox
                                inputId="bb1"
                                checked={this.state.Bano}
                                onChange={(e) => this.handleChangebox(e)}
                                name="Bano"
                              />
                              <label htmlFor="bb1" className="p-checkbox-label">
                                SI/NO
                              </label>
                            </Fragment>
                          </div>
                        </div>
                        <hr></hr>
                        <div className="row align-items-center">
                          <div className="col">
                            <h4> 2) Vestido</h4>
                            <p> Sí: Que pueda tomar las prendas y vestirse completamente, sin asistencia.</p>
                            <p> Sí: Que pueda tomar las prendas y vestirse sin asistencia excepto en
                              abrocharse los zapatos.</p>
                            <p>  No: Que reciba asistencia para tomar las prendas y vestirse.</p>
                          </div>

                          <div className="col">
                            <Fragment>
                              <Checkbox
                                inputId="bb2"
                                checked={this.state.Vestido}
                                onChange={(e) => this.handleChangebox(e)}
                                name="Vestido"
                              />
                              <label htmlFor="bb2" className="p-checkbox-label">
                                SI/NO
                              </label>
                            </Fragment>
                          </div>
                        </div>
                        <hr></hr>
                        <div className="row align-items-center">
                          <div className="col">
                            <h4>  3) Uso del sanitario</h4>
                            <p> Sí: Sin ninguna asistencia (puede utilizar algún objeto de soporte como bastón
                              o silla de ruedas y/o que pueda arreglar su ropa o el uso de pañal o cómodo).</p>
                            <p> Sí: Que reciba asistencia al ir al baño, en limpiarse y que pueda manejar por si
                              mismo/a el pañal o cómodo vaciándolo.</p>
                            <p>  No: Que no vaya al baño por si mismo/a.</p>
                          </div>

                          <div className="col">
                            <Fragment>
                              <Checkbox
                                inputId="bb3"
                                checked={this.state.Sanitario}
                                onChange={(e) => this.handleChangebox(e)}
                                name="Sanitario"
                              />
                              <label htmlFor="bb3" className="p-checkbox-label">
                                SI/NO
                              </label>
                            </Fragment>
                          </div>
                        </div>
                        <hr></hr>
                        <div className="row align-items-center">
                          <div className="col">
                            <h4> 4) Transferencias</h4>
                            <p> Sí: Que se mueva dentro y fuera de la cama y silla sin ninguna asistencia
                              (puede estar utilizando un auxiliar de la marcha u objeto de soporte).</p>
                            <p> Sí: Que pueda moverse dentro y fuera de la cama y silla con asistencia.</p>
                            <p>  No: Que no pueda salir de la cama.</p>
                          </div>

                          <div className="col">
                            <Fragment>
                              <Checkbox
                                inputId="bb4"
                                checked={this.state.Transferencias}
                                onChange={(e) => this.handleChangebox(e)}
                                name="Transferencias"
                              />
                              <label htmlFor="bb4" className="p-checkbox-label">
                                SI/NO
                              </label>
                            </Fragment>
                          </div>
                        </div>
                        <hr></hr>
                        <div className="row align-items-center">
                          <div className="col">
                            <h4> 5) Continencia</h4>
                            <p> Sí: Control total de esfínteres.</p>
                            <p>  Sí: Que tenga accidentes ocasionales que no afectan su vida social.</p>
                            <p>   No: Necesita ayuda para supervisión del control de esfínteres, utiliza sonda
                              o es incontinente.</p>
                          </div>

                          <div className="col">
                            <Fragment>
                              <Checkbox
                                inputId="bb5"
                                checked={this.state.Continencia}
                                onChange={(e) => this.handleChangebox(e)}
                                name="Continencia"
                              />
                              <label htmlFor="bb5" className="p-checkbox-label">
                                SI/NO
                              </label>
                            </Fragment>
                          </div>
                        </div>
                        <hr></hr>
                        <div className="row align-items-center">
                          <div className="col">
                            <h4> 6) Alimentación</h4>
                            <p> Sí: Que se alimente por si solo sin asistencia alguna.</p>
                            <p>  Sí: Que se alimente solo y que tenga asistencia sólo para cortar la carne o
                              untar mantequilla.</p>
                            <p> No: Que reciba asistencia en la alimentación o que se alimente parcial o
                              totalmente por vía enteral o parenteral.
                            </p>
                          </div>

                          <div className="col">
                            <Fragment>
                              <Checkbox
                                inputId="bb6"
                                checked={this.state.Alimentacion}
                                onChange={(e) => this.handleChangebox(e)}
                                name="Alimentacion"
                              />
                              <label htmlFor="bb6" className="p-checkbox-label">
                                SI/NO
                              </label>
                            </Fragment>
                          </div>
                        </div>
                        <div className="row">
                          <div className="col">

                            <div class="card card-accent-info">
                              <div class="card-header">
                                <b>CALIFICACIÓN DE KATZ</b>
                              </div>
                              <div class="card-body">
                                <p>[A] Independencia en todas las actividades básicas de la vida diaria.</p>
                                <p>[B] Independencia en todas las actividades menos en una.</p>
                                <p>[C] Independencia en todo menos en bañarse y otra actividad adicional.</p>
                                <p>[D] Independencia en todo menos bañarse, vestirse y otra actividad adicional.</p>
                                <p>[E] Dependencia en el baño, vestido, uso del sanitario y otra actividadadicional.</p>
                                <p>[F] Dependencia en el baño, vestido, uso del sanitario, transferencias y otra actividad.</p>
                                <p>[G] Dependiente en las seis actividades básicas de la vida diaria.</p>
                                <p>[H] Dependencia en dos actividades pero que no clasifican en C, D, E, y F.</p>

                              </div>
                            </div>


                          </div>
                          <div className="col">
                            <label><strong>Resultado </strong>{this.state.totalkat}/6</label>
                          </div>
                          <div className="col">
                            <Field
                              name="Calificacion"
                              label="Calificación"
                              required
                              edit={true}
                              readOnly={false}
                              value={this.state.Calificacion}
                              onChange={this.handleChange}
                              error={this.state.errorsMNA.Calificacion}
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
                    label="Identificacion"
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
            Edad: this.state.Edad

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
            Edad: this.state.Edad
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

    if (this.state.Calificacion == "") {
      abp.notify.error(
        "Se debe agregar un calificación al anexo Katz.",
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
          .post("/Documentos/Documento/FGetEditMNA", {
            Id: this.state.IdKATZ,
            PacienteId: this.state.pacienteSeleccionado.Id,
            Id: 0,
            PacienteId: this.state.pacienteSeleccionado.Id,
            Bano: this.state.Bano,
            Vestido: this.state.Vestido,

            Sanitario: this.state.Sanitario,

            Transferencias: this.state.Transferencias,
            Continencia: this.state.Continencia,
            Alimentacion: this.state.Alimentacion,

            Calificacion: this.state.Calificacion,
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
      this.contarKatz()
    }, 500);
  };
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
