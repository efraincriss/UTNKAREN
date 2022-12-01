import React from "react"
import ReactDOM from "react-dom"
import BlockUi from "react-block-ui"
import axios from "axios"
import CurrencyFormat from "react-currency-format"
import Field from "../Base/Field-v2"
import wrapForm from "../Base/BaseWrapper"
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table"
import { Dialog } from "primereact-v3.3/dialog"
import { Growl } from "primereact-v3.3/growl"
import { Card } from "primereact-v3.3/card"

class PosList extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      errors: {},
      editable: true,
      data: [],
      estados: [],
      proyectos: [],
      ofertas: [],
      grupos: [],
      action: "list",

      /*OS */
      Id: 0,
      codigo_orden_servicio: "",
      fecha_orden_servicio: null,
      monto_aprobado_os: 0,
      monto_aprobado_suministros: 0,
      monto_aprobado_construccion: 0,
      monto_aprobado_ingeniería: 0,
      monto_aprobado_subcontrato: 0,
      comentarios: "",
      version_os: "A",
      EstadoId: 0,
      anio: 2020,
      referencias_po: "",
      ArchivoId: null,
      uploadFile: "",
      tieneArchivo: false,
      po: null,

      /**Detalles */
      Idd: 0,
      ProyectoId: 0,
      GrupoItemId: 0,
      valor_os: 0,
      OfertaComercialId: 0,
      detalles: [],
      action_detalles: "create",
      dialogdetalles: false,
    }

    this.handleChange = this.handleChange.bind(this)
    this.onChangeValue = this.onChangeValue.bind(this)
    this.mostrarForm = this.mostrarForm.bind(this)
    this.OcultarFormulario = this.OcultarFormulario.bind()
    this.isValid = this.isValid.bind()
    this.EnviarFormulario = this.EnviarFormulario.bind(this)
  }

  componentDidMount() {
    this.GetList()
    this.GetCatalogs()
  }
  isValid = () => {
    const errors = {}

    if (this.state.codigo_orden_servicio == "") {
      errors.codigo_orden_servicio = "Campo Requerido"
    }
    if (this.state.fecha_orden_servicio == null) {
      errors.fecha_orden_servicio = "Campo Requerido"
    }
    if (this.state.EstadoId == 0) {
      errors.EstadoId = "Campo Requerido"
    }

    this.setState({ errors })
    return Object.keys(errors).length === 0
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
    this.setState({ po: null, action: "list" })
  }
  OcultarFormularioOrdenDetalle = () => {
    this.setState({ po: null, action: "list" })
    this.GetList()
  }
  GetList = () => {
    this.props.blockScreen()
    axios
      .post("/Proyecto/OrdenServicio/FGetList", {})
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

  GetDetalleOS = (Id) => {
    this.props.blockScreen()
    axios
      .post("/Proyecto/OrdenServicio/FDGetDetalleOs", {
        Id: Id,
      })
      .then((response) => {
        console.log(response.data)
        this.setState({ po: response.data })
        this.props.unlockScreen()
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(error)
        this.props.unlockScreen()
      })
  }
  GetListDetalles = (Id) => {
    this.props.blockScreen()
    axios
      .post("/Proyecto/OrdenServicio/FGetOrdenHijos", { Id: Id })
      .then((response) => {
        console.log("hijos", response.data)
        this.setState({ detalles: response.data })
        this.props.unlockScreen()
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(error)
        this.props.unlockScreen()
      })
  }
  GetCatalogs = () => {
    this.props.blockScreen()
    axios
      .post("/Proyecto/OrdenServicio/FGetCatalogos", {})
      .then((response) => {
        console.log(response.data)
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id }
        })
        this.setState({ estados: items })
      })
      .catch((error) => {
        console.log(error)
      })
    axios
      .post("/Proyecto/OrdenServicio/FGetProyectos", {})
      .then((response) => {
        console.log("proyectos", response.data)
        this.setState({ proyectos: response.data })
      })
      .catch((error) => {
        console.log(error)
      })
    axios
      .post("/Proyecto/OrdenServicio/FGetOfertas", {})
      .then((response) => {
        console.log("ofertas", response.data)
        this.setState({ ofertas: response.data })
      })
      .catch((error) => {
        console.log(error)
      })
    axios
      .post("/Proyecto/OrdenServicio/FGetGrupos", {})
      .then((response) => {
        console.log("ofertas", response.data)
        this.setState({ grupos: response.data })
      })
      .catch((error) => {
        console.log(error)
      })
  }

  onAfterSaveCell = (row, cellName, cellValue) => {
    console.log(row)
    console.log(cellName)
    var data = {}

    if (cellName === "nombre_grupo") {
      var grupoId = this.state.grupos.filter((o) => o.label === cellValue)[0]
        .dataKey
      console.log(grupoId)
      data = {
        ...row,
        GrupoItemId: grupoId,
      }
    } else if (cellName === "codigoOferta") {
      var ofertaId = this.state.ofertas.filter((o) => o.label === cellValue)[0]
        .dataKey
      console.log(ofertaId)
      data = {
        ...row,
        OfertaComercialId: ofertaId,
      }
    } else {
      data = row
    }
    console.log(data)
    this.props.blockScreen()
    axios
      .post("/Proyecto/OrdenServicio/FDEditDetalle", data)
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

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    })
  }

  Eliminar = (Id) => {
    console.log(Id)
    axios
      .post("/Proyecto/OrdenServicio/FGetDelete/", {
        Id: Id,
      })
      .then((response) => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente")

          this.GetList()
        } else if (response.data == "NOPUEDE") {
          this.props.showWarn("No se puedo Eliminar")
        }
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn("Ocurrió un Error")
      })
  }
  EliminarDetalle = (Id) => {
    console.log(Id)
    console.log(this.state.po.Id)
    axios
      .post("/Proyecto/OrdenServicio/FDRemoveDetalle/", {
        Id: Id,
        OrdenServicioId: this.state.po.Id,
      })
      .then((response) => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente")
          this.setState({ dialogdetalles: false })
          this.GetListDetalles(this.state.po.Id)
          this.GetDetalleOS(this.state.po.Id)
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
    console.log("Orden", row)

    if (row != null && row.Id > 0) {
      this.GetDetalleOS(row.Id)
      this.setState({ action: "detalles", detalles: [] })
      this.GetListDetalles(row.Id)
    }
  }

  mostrarForm = (row) => {
    if (row != null && row.Id > 0) {
      this.setState({
        Id: row.Id,
        codigo_orden_servicio: row.codigo_orden_servicio,
        fecha_orden_servicio: row.fecha_orden_servicio,
        monto_aprobado_os: row.monto_aprobado_os,
        monto_aprobado_suministros: row.monto_aprobado_suministros,
        monto_aprobado_construccion: row.monto_aprobado_construccion,
        monto_aprobado_ingeniería: row.monto_aprobado_ingeniería,
        monto_aprobado_subcontrato: row.monto_aprobado_subcontrato,
        version_os: row.version_os,
        EstadoId: row.EstadoId,
        anio: row.anio,
        referencias_po: row.referencias_po,
        ArchivoId: row.ArchivoId,
        tieneArchivo: row.tieneArchivo,
        comentarios: row.comentarios,
        po: row,
        action: "edit",
        editable: true,
      })
    } else {
      this.setState({
        Id: 0,
        codigo_orden_servicio: "",
        fecha_orden_servicio: null,
        monto_aprobado_os: 0,
        monto_aprobado_suministros: 0,
        monto_aprobado_construccion: 0,
        monto_aprobado_ingeniería: 0,
        monto_aprobado_subcontrato: 0,
        version_os: "A",
        EstadoId: 0,
        anio: 0,
        referencias_po: "",
        ArchivoId: null,
        uploadFile: "",
        tieneArchivo: false,
        po: null,
        action: "create",
        editable: false,
        comentarios: "",
      })
    }
  }
  mostrarFormDetalle = (row) => {
    if (row != null && row.Id > 0) {
      console.log(row)
      this.setState({
        Idd: row.Id,
        ProyectoId: row.ProyectoId,
        GrupoItemId: row.GrupoItemId,
        valor_os: row.valor_os,
        OfertaComercialId: row.OfertaComercialId,
        action_detalles: "edit",
        dialogdetalles: true,
      })
    } else {
      console.log("New Form Detalles")
      this.setState({
        Idd: 0,
        ProyectoId: 0,
        GrupoItemId: 0,
        valor_os: 0,
        OfertaComercialId: 0,
        action_detalles: "create",
        dialogdetalles: true,
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
              this.EliminarDetalle(row.Id)
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
    const options = {
      exportCSVText: "Exportar",
      exportCSVSeparator: ";",
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
        <BlockUi tag="div" blocking={this.state.blocking}>
          <Growl
            ref={(el) => {
              this.growl = el
            }}
            baseZIndex={1000}
          />
          <div align="right">
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.mostrarForm}
            >
              Nuevos
            </button>
          </div>
          <br />
          <div>
            <BootstrapTable
              data={this.state.data}
              hover={true}
              pagination={true}
              options={options}
              exportCSV={true}
              csvFileName="Ordenes Servicio.csv"
            >
              <TableHeaderColumn
                dataField="any"
                export={false}
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
                width={"10%"}
                export
                csvHeader="Código"
                dataField="codigo_orden_servicio"
                tdStyle={{ whiteSpace: "normal" }}
                thStyle={{ whiteSpace: "normal" }}
                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}
                width={"8%"}
              >
                Codigo
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="version_os"
                export={false}
                hidden
                width={"8%"}
                tdStyle={{ whiteSpace: "normal" }}
                thStyle={{ whiteSpace: "normal" }}
                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}
              >
                Versión
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="formatFechaOs"
                export
                csvHeader="Fecha"
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
                width={"8%"}
                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              >
                Fecha{" "}
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="monto_aprobado_ingeniería"
                export
                csvHeader="Monto Ingeniería"
                tdStyle={{ whiteSpace: "normal" }}
                thStyle={{ whiteSpace: "normal" }}
                width={"10%"}
                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{
                  whiteSpace: "normal",
                  fontSize: "11px",
                  textAlign: "right",
                }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}
              >
                M. Ingeniería
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="monto_aprobado_construccion"
                export
                csvHeader="Monto Contrucción"
                tdStyle={{ whiteSpace: "normal" }}
                thStyle={{ whiteSpace: "normal" }}
                width={"8%"}
                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{
                  whiteSpace: "normal",
                  fontSize: "11px",
                  textAlign: "right",
                }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}
              >
                M. Construcción
              </TableHeaderColumn>

              <TableHeaderColumn
                dataField="monto_aprobado_suministros"
                export
                csvHeader="Monto Suministros"
                tdStyle={{ whiteSpace: "normal" }}
                thStyle={{ whiteSpace: "normal" }}
                width={"10%"}
                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{
                  whiteSpace: "normal",
                  fontSize: "11px",
                  textAlign: "right",
                }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}
              >
                M. Suministros
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="monto_aprobado_subcontrato"
                export
                csvHeader="Monto Sub Contratos"
                tdStyle={{ whiteSpace: "normal" }}
                thStyle={{ whiteSpace: "normal" }}
                width={"10%"}
                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{
                  whiteSpace: "normal",
                  fontSize: "11px",
                  textAlign: "right",
                }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}
              >
                M. Subcontratos
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="monto_aprobado_os"
                export
                csvHeader="Monto Total"
                tdStyle={{ whiteSpace: "normal" }}
                thStyle={{ whiteSpace: "normal" }}
                width={"10%"}
                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{
                  whiteSpace: "normal",
                  fontSize: "11px",
                  textAlign: "right",
                }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}
              >
                M. Aprobado OS
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="nombreEstado"
                width={"8%"}
                export
                csvHeader="Estado"
                tdStyle={{ whiteSpace: "normal" }}
                thStyle={{ whiteSpace: "normal" }}
                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                dataSort={true}
              >
                Estado
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="Id"
                export={false}
                isKey
                width={"14%"}
                dataFormat={this.generarBotones}
                thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              >
                Opciones
              </TableHeaderColumn>
            </BootstrapTable>
          </div>
        </BlockUi>
      )
    } else if (this.state.action === "detalles") {
      const cellEditProp = {
        mode: "click",
        blurToSave: true,
        afterSaveCell: this.onAfterSaveCell,
      }
      return (
        <BlockUi tag="div" blocking={this.state.blocking}>
          <div align="right">
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.OcultarFormularioOrdenDetalle}
            >
              Regresar
            </button>
          </div>
          <Card title="Orden de Servicio">
            <div className="row">
              <div className="col">
                <h6>
                  <b>Código :</b>{" "}
                  {this.state.po != null
                    ? this.state.po.codigo_orden_servicio
                    : ""}
                </h6>
              </div>

              <div className="col">
                <h6>
                  <b>Fecha :</b>{" "}
                  {this.state.po != null ? this.state.po.formatFechaOs : ""}
                </h6>
              </div>

              <div className="col">
                <h6>
                  <b>Estado :</b>{" "}
                  {this.state.po != null ? this.state.po.nombreEstado : ""}
                </h6>
              </div>
              <div className="col">
                <h6>
                  <b>Año :</b> {this.state.po != null ? this.state.po.anio : ""}
                </h6>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <h6>
                  <b>Referencias Po :</b>{" "}
                  {this.state.po != null ? this.state.po.referencias_po : ""}
                </h6>
              </div>
              <div className="col">
                <h6>
                  <b>Comentarios :</b>{" "}
                  {this.state.po != null ? this.state.po.comentarios : ""}
                </h6>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="callout callout-info">
                  <small className="text-muted"> Total</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.po != null
                          ? this.state.po.monto_aprobado_os
                          : 0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
              <div className="col">
                <div className="callout callout-danger">
                  <small className="text-muted">Ingeniería</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.po != null
                          ? this.state.po.monto_aprobado_ingeniería
                          : 0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
              <div className="col">
                <div className="callout callout-warning">
                  <small className="text-muted">Suministros</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.po != null
                          ? this.state.po.monto_aprobado_suministros
                          : 0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
              <div className="col">
                <div className="callout callout-danger">
                  <small className="text-muted">Construcción</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.po != null
                          ? this.state.po.monto_aprobado_construccion
                          : 0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
              <div className="col">
                <div className="callout callout-warning">
                  <small className="text-muted">Sub Contratos</small>
                  <br />
                  <strong className="h4">
                    <CurrencyFormat
                      value={
                        this.state.po != null
                          ? this.state.po.monto_aprobado_subcontrato
                          : 0
                      }
                      displayType={"text"}
                      thousandSeparator={true}
                      prefix={"$"}
                    />
                  </strong>
                </div>
              </div>
            </div>
          </Card>
          <br></br>
          <div className="row">
            <div align="right">
              <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={this.mostrarFormDetalle}
              >
                Nuevo
              </button>

              <div>
                <BootstrapTable
                  data={this.state.detalles}
                  hover={true}
                  pagination={true}
                  cellEdit={cellEditProp}
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
                    width={"10%"}
                    dataField="codigo_proyecto"
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                    editable={false}
                  >
                    Código Proyecto
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="nombre_proyecto"
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                    editable={false}
                  >
                    Proyecto
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    width={"15%"}
                    dataField="codigoOferta"
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                    editable={false}
                    editable={{
                      type: "select",
                      options: {
                        values: this.state.ofertas.map((a) => a.label),
                      },
                    }}
                  >
                    Oferta Comercial
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    width={"10%"}
                    dataField="nombre_grupo"
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                    editable={{
                      type: "select",
                      options: {
                        values: this.state.grupos.map((a) => a.label),
                      },
                    }}
                  >
                    Grupo
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    width={"10%"}
                    dataField="valor_os"
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                    dataFormat={this.valorFormat}
                  >
                    Valor
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
              </div>
            </div>
          </div>

          <Dialog
            header="Detalle"
            visible={this.state.dialogdetalles}
            style={{ width: "50vw" }}
            onHide={this.onHide}
          >
            <form onSubmit={this.EnviarFormularioDetalle}>
              <div className="row">
                <div className="col">
                  <Field
                    name="GrupoItemId"
                    required
                    value={this.state.GrupoItemId}
                    label="Grupo"
                    options={this.state.grupos}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.GrupoItemId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="ProyectoId"
                    required
                    value={this.state.ProyectoId}
                    label="Proyecto"
                    options={this.state.proyectos}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.ProyectoId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="OfertaComercialId"
                    required
                    value={this.state.OfertaComercialId}
                    label="Oferta"
                    options={this.state.ofertas}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.OfertaComercialId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                  <Field
                    name="valor_os"
                    label="Valor"
                    required
                    type={"number"}
                    edit={true}
                    readOnly={false}
                    value={this.state.valor_os}
                    onChange={this.handleChange}
                    error={this.state.errors.valor_os}
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
          </Dialog>
        </BlockUi>
      )
    } else {
      return (
        <BlockUi tag="div" blocking={this.state.blocking}>
          <div>
            <form onSubmit={this.EnviarFormulario}>
              <div className="row">
                <div className="col">
                  <Field
                    name="codigo_orden_servicio"
                    label="Código OS"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.codigo_orden_servicio}
                    onChange={this.handleChange}
                    error={this.state.errors.codigo_orden_servicio}
                  />
                </div>
                <div className="col">
                  <Field
                    name="fecha_orden_servicio"
                    label="Fecha Orden Servicio"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fecha_orden_servicio}
                    onChange={this.handleChange}
                    error={this.state.errors.fecha_orden_servicio}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="anio"
                    label="Año"
                    type="number"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.anio}
                    onChange={this.handleChange}
                    error={this.state.errors.anio}
                  />
                </div>
                <div className="col">
                  <Field
                    name="referencias_po"
                    label="Referencias PO"
                    required
                    type="text"
                    edit={true}
                    readOnly={false}
                    value={this.state.referencias_po}
                    onChange={this.handleChange}
                    error={this.state.errors.referencias_po}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="EstadoId"
                    required
                    value={this.state.EstadoId}
                    label="Estado"
                    options={this.state.estados}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.EstadoId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
                <div className="col">
                  <Field
                    name="uploadFile"
                    label="Documento"
                    type={"file"}
                    edit={true}
                    readOnly={false}
                    onChange={this.handleChange}
                    error={this.state.errors.uploadFile}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="comentarios"
                    label="Comentarios"
                    type={"TEXTAREA"}
                    edit={true}
                    readOnly={false}
                    value={this.state.comentarios}
                    onChange={this.handleChange}
                    error={this.state.errors.comentarios}
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
        </BlockUi>
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
        const formData = new FormData()
        formData.append("Id", 0)
        formData.append("ArchivoId", null)
        formData.append(
          "codigo_orden_servicio",
          this.state.codigo_orden_servicio
        )
        formData.append("fecha_orden_servicio", this.state.fecha_orden_servicio)
        formData.append("monto_aprobado_os", this.state.monto_aprobado_os)
        formData.append(
          "monto_aprobado_suministros",
          this.state.monto_aprobado_suministros
        )
        formData.append(
          "monto_aprobado_construccion",
          this.state.monto_aprobado_construccion
        )
        formData.append(
          "monto_aprobado_ingeniería",
          this.state.monto_aprobado_ingeniería
        )
        formData.append(
          "monto_aprobado_subcontrato",
          this.state.monto_aprobado_subcontrato
        )
        formData.append("EstadoId", this.state.EstadoId)
        formData.append("version_os", this.state.version_os)
        formData.append("comentarios", this.state.comentarios)
        formData.append("anio", this.state.anio)
        formData.append("referencias_po", this.state.referencias_po)
        if (this.state.uploadFile != "") {
          formData.append("UploadedFile", this.state.uploadFile)
        } else {
          formData.append("UploadedFile", null)
        }
        const multipart = {
          headers: {
            "content-type": "multipart/form-data",
          },
        }
        axios
          .post("/Proyecto/OrdenServicio/FGetCreate", formData, multipart)
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("OS Guardado", "Correcto")
              this.setState({ action: "list" })
              this.GetList()
            }
            if (response.data == "CODE_EXIST") {
              abp.notify.error("El Código de la OS ya existe", "Error")
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
        const formData = new FormData()
        formData.append("Id", this.state.Id)
        formData.append("ArchivoId", null)
        formData.append(
          "codigo_orden_servicio",
          this.state.codigo_orden_servicio
        )
        formData.append("fecha_orden_servicio", this.state.fecha_orden_servicio)
        formData.append("monto_aprobado_os", this.state.monto_aprobado_os)
        formData.append(
          "monto_aprobado_suministros",
          this.state.monto_aprobado_suministros
        )
        formData.append(
          "monto_aprobado_construccion",
          this.state.monto_aprobado_construccion
        )
        formData.append(
          "monto_aprobado_ingeniería",
          this.state.monto_aprobado_ingeniería
        )
        formData.append(
          "monto_aprobado_subcontrato",
          this.state.monto_aprobado_subcontrato
        )
        formData.append("EstadoId", this.state.EstadoId)
        formData.append("version_os", this.state.version_os)
        formData.append("comentarios", this.state.comentarios)
        formData.append("anio", this.state.anio)
        formData.append("referencias_po", this.state.referencias_po)
        if (this.state.uploadFile != "") {
          formData.append("UploadedFile", this.state.uploadFile)
        } else {
          formData.append("UploadedFile", null)
        }
        const multipart = {
          headers: {
            "content-type": "multipart/form-data",
          },
        }

        axios
          .post("/Proyecto/OrdenServicio/FGetEdit", formData, multipart)
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("OS Guardado", "Correcto")
              this.setState({ action: "list" })
              this.GetList()
            }
            if (response.data == "CODE_EXIST") {
              abp.notify.error("El Código de la OS ya existe", "Error")
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
    this.setState({ dialogdetalles: false })
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
const Container = wrapForm(PosList)
ReactDOM.render(<Container />, document.getElementById("content"))
