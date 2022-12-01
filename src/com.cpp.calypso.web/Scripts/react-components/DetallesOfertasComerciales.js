import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import moment from "moment";
import CurrencyFormat from "react-currency-format";
import BlockUi from "react-block-ui";
import CabeceraDetallePrespuesto from "./OfertasComerciales/CabeceraDetallePresupuesto";
import { Dropdown } from "primereact/components/dropdown/Dropdown";
import { Checkbox } from "primereact/components/checkbox/Checkbox";
import { Button } from "primereact/components/button/Button";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";


/*DEBUG */
import Field from "./Base/Field-v2";
import wrapForm from "./Base/BaseWrapper";

class DetalleOfertaComercial extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visibleEditar: false,
      visibleeliminar: false,
      data: [],
      detalleoferta: [],
      blocking: true,
      OfertaIdActual: 0,
      Oferta: {},
      visible: false,
      FormKey: 8498,
      file: null,
      tieneTransmital: false,
      codigoTransmital: "",
      visiblearchivo: false,

      //Nueva Version Oferta Comercial
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
      //Presupuesto
      visiblepresupuesto: false,
      PresupuestoId: 0,

      //seleccion Oferta

      blockingpresupuesto: true,
      proyectos: [],
      requerimientos: [],
      ofertas: [],
      contratos: [],
      ContratoId: 0,
      ProyectoId: 0,
      RequerimientoId: 0,
      OfertaId: 0,
      message: "",
      blockSubmit: false,

      /*OS */
      Id: 0,
      errors: {},
      uploadFile: "",
      OfertaComercialId: 0,
      ArchivoId: null,
      visibleorden: false,
      codigo_orden_servicio: "",
      fecha_orden_servicio: null,
      monto_aprobado_os: 0,
      monto_aprobado_suministros: 0,
      monto_aprobado_construccion: 0,
      monto_aprobado_ingenieria: 0,
      version_os: "",
      action: "create",

      ///

      datapresupuesto: [],
      dataos: [],

      //cabeceras
      monto_aprobado: 0.0,
      monto_ofertado: 0.0,
      monto_pendiente_aprobacion: 0.0,

      //
      nombre_estado: "",

      //
      archivos: [],

      //Nuevo Orden de Servivio
      Id: 0,
      cabeceraos: 0,
      cabeceraosId: 0,
      datadetalles: 0,

      monto_so_ingenieria: 0.0,
      monto_so_construccion: 0.0,
      monto_so_suministros: 0.0,
      monto_so_subcontratos: 0.0,
      monto_so_total: 0.0,

      /* Montos Requerimientos*/
      ModelMontos: null,
      mailto: "",
    };

    this.successMessage = this.successMessage.bind(this);
    this.warnMessage = this.warnMessage.bind(this);
    this.alertMessage = this.alertMessage.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormularioEditar = this.MostrarFormularioEditar.bind(this);
    this.OcultarFormularioPresupuesto = this.OcultarFormularioPresupuesto.bind(
      this
    );
    this.MostrarFormularioPresupuesto = this.MostrarFormularioPresupuesto.bind(
      this
    );

    this.OcultarFormularioOrden = this.OcultarFormularioOrden.bind(this);
    this.MostrarFormularioOrden = this.MostrarFormularioOrden.bind(this);

    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormularioEditar = this.MostrarFormularioEditar.bind(this);
    this.OcultarFormularioEditar = this.OcultarFormularioEditar.bind(this);
    this.MostrarFormularioEditarOferta = this.MostrarFormularioEditarOferta.bind(
      this
    );
    this.MostrarFormularioArchivo = this.MostrarFormularioArchivo.bind(this);
    this.OcultarFormularioArchivo = this.OcultarFormularioArchivo.bind(this);
    this.ObtenerCatalogos = this.ObtenerCatalogos.bind(this);
    this.ObtenerDetalleOferta = this.ObtenerDetalleOferta.bind(this);
    this.NuevaVersion = this.NuevaVersion.bind(this);
    this.EditarOferta = this.EditarOferta.bind(this);
    this.ConsultarVersiones = this.ConsultarVersiones.bind(this);

    //
    this.getProyectos = this.getProyectos.bind(this);
    this.getRequerimientos = this.getRequerimientos.bind(this);
    this.getOfertas = this.getOfertas.bind(this);
    this.getContratos = this.getContratos.bind(this);
    this.getFormSelect = this.getFormSelect.bind(this);
    this.getFormSelectContratos = this.getFormSelectContratos.bind(this);
    this.getFormSelectOferta = this.getFormSelectOferta.bind(this);

    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleChangeProyecto = this.handleChangeProyecto.bind(this);
    this.handleChangeRequerimientos = this.handleChangeRequerimientos.bind(
      this
    );

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);

    this.handleChange = this.handleChange.bind(this);
    this.ConsultarPresupuesto = this.ConsultarPresupuesto.bind(this);
    this.ConsultarOS = this.ConsultarOS.bind(this);
    this.handleSubmitOrden = this.handleSubmitOrden.bind(this);
    this.Editar = this.Editar.bind(this);
    this.Eliminar = this.Eliminar.bind(this);
    this.VerVersion = this.VerVersion.bind(this);
    this.onClick = this.onClick.bind(this);
    this.onHide = this.onHide.bind(this);
    this.IrOrdenServicio = this.IrOrdenServicio.bind(this);
    this.calcularmontos = this.calcularmontos.bind(this);

    this.getArchivos = this.getArchivos.bind(this);

    this.Loading = this.Loading.bind(this);
    this.CancelarLoading = this.CancelarLoading.bind(this);

    this.getdetallesordenesservicio = this.getdetallesordenesservicio.bind(
      this
    );
    this.getcabeceraos = this.getcabeceraos.bind(this);
    /*OS */
    this.mostrarForm = this.mostrarForm.bind(this);
    this.isValid = this.isValid.bind();
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
  }

  componentDidMount() {
    this.ObtenerDetalleOferta();
    this.ObtenerTieneTransmital();
    this.ObtenerCodigoTransmital();
    this.ObtenerCatalogos();
    this.ObtenerMontosRequerimientos();
    this.ConsultarPresupuesto();
    this.ConsultarOS();
    this.ConsultarVersiones();

    this.setState({ blocking: false });

    //this.getArchivos();
    //this.calcularmontos();
  }
 


  render() {
    const footer = (
      <div>
        <Button
          label="No"
          icon="pi pi-times"
          onClick={this.onHide}
          className="p-button-secondary"
        />
      </div>
    );
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={(el) => {
            this.growl = el;
          }}
          position="bottomright"
          baseZIndex={1000}
        ></Growl>
        {this.state.OfertaIdActual && this.state.OfertaIdActual > 0 && (
          <div>
          <CabeceraDetallePrespuesto
            contratodescripcion={this.state.contratodescripcion}
            data={this.state.detalleoferta}
            MostrarFormulario={this.MostrarFormularioEditar}
            DesaprobarPresupuesto={this.DesaprobarPresupuesto}
            AprobarPresupuesto={this.AprobarPresupuesto}
            PresupuestoId={this.state.PresupuestoId}
            OfertaIdActual={this.state.OfertaIdActual}
            monto_aprobado={this.state.monto_aprobado}
            monto_ofertado={this.state.monto_ofertado}
            monto_pendiente_aprobacion={this.state.monto_pendiente_aprobacion}
            nombre_estado={this.state.nombre_estado}
            detalles_oferta={this.ObtenerDetalleOferta}
            //
            CalcularMonto={this.calcularmontos}
            ConsultarPresupuesto={this.ConsultarPresupuesto}
            MostrarEdicion={this.MostrarFormularioEditarOferta}
            Loading={this.Loading}
            CancelarLoading={this.CancelarLoading}
            ObtenerTieneTransmital={this.ObtenerTieneTransmital}
            fecha_oferta={this.state.fecha_registro}
            referencia={
              "/Proyecto/OfertaComercial/ListarWordOfertaComercial/" +
              document.getElementById("OfertaComercialId").className
            }
            successMessage={this.successMessage}
            warnMessage={this.warnMessage}
            tieneTransmital={this.state.tieneTransmital}
            ModelMontos={this.state.ModelMontos}
            codigoTransmital={this.state.codigoTransmital}
            mailto={this.state.mailto}
            EnvioManual={this.EnvioManual}
          />
       

        <Dialog
          header="Generación de Ofertas Comerciales"
          visible={this.state.visible}
          width="800px"
          modal={true}
          onHide={this.OcultarFormulario}
        >
          <form onSubmit={this.NuevaVersion}>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label>Fecha Oferta</label>
                  <input
                    type="date"
                    id="no-filter"
                    name="fecha_registro"
                    className="form-control"
                    onChange={this.handleChange}
                    value={moment(this.state.fecha_registro).format(
                      "YYYY-MM-DD"
                    )}
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
                <div className="form-group">
                  <label>Service Request</label>
                  <Checkbox
                    checked={this.state.service_request}
                    onChange={(e) =>
                      this.setState({ service_request: e.checked })
                    }
                  />
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label>Change Order</label>
                  <Checkbox
                    checked={this.state.service_order}
                    onChange={(e) =>
                      this.setState({ service_order: e.checked })
                    }
                  />
                </div>
              </div>
            </div>

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
              onClick={this.OcultarFormulario}
            >
              Cancelar
            </button>
          </form>
        </Dialog>
        <Dialog
          header="Editar Oferta Comercial"
          visible={this.state.visibleEditar}
          width="800px"
          modal={true}
          onHide={this.OcultarFormularioEditar}
        >
          <form onSubmit={this.EditarOferta}>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label>Fecha Oferta</label>
                  <input
                    type="date"
                    id="no-filter"
                    name="fecha_registro"
                    className="form-control"
                    onChange={this.handleChange}
                    value={moment(this.state.fecha_registro).format(
                      "YYYY-MM-DD"
                    )}
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
                <div className="form-group">
                  <label>Service Request</label>
                  <Checkbox
                    checked={this.state.service_request}
                    onChange={(e) =>
                      this.setState({ service_request: e.checked })
                    }
                  />
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label>Change Order</label>
                  <Checkbox
                    checked={this.state.service_order}
                    onChange={(e) =>
                      this.setState({ service_order: e.checked })
                    }
                  />
                </div>
              </div>
            </div>

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
              onClick={this.OcultarFormularioEditar}
            >
              Cancelar
            </button>
          </form>
        </Dialog>
        <div className="row">
          <div style={{ width: "100%" }}>
            <ul className="nav nav-tabs" id="gestion_tabs" role="tablist">
              <li className="nav-item">
                <a
                  className="nav-link active"
                  id="gestion-tab"
                  data-toggle="tab"
                  href="#gestion"
                  role="tab"
                  aria-controls="home"
                  aria-expanded="true"
                >
                  Requerimientos
                </a>
              </li>
              <li className="nav-item">
                <a
                  className="nav-link"
                  id="op-tab"
                  data-toggle="tab"
                  href="#op"
                  role="tab"
                  aria-controls="home"
                  aria-expanded="true"
                >
                  P.O.
                </a>
              </li>
              {/* 
            <li className="nav-item">
                 <a className="nav-link " id="archivos-tab" data-toggle="tab" href="#archivos" role="tab" aria-controls="profile">Archivos</a>
            </li>
        */}
              <li className="nav-item">
                <a
                  className="nav-link "
                  id="jerarquia-tab"
                  data-toggle="tab"
                  href="#jerarquia"
                  role="tab"
                  aria-controls="profile"
                >
                  Versiones
                </a>
              </li>
            </ul>
            <div className="tab-content" id="myTabContent">
              <div
                className="tab-pane fade"
                id="jerarquia"
                role="tabpanel"
                aria-labelledby="jerarquia-tab"
              >
                <div className="col" align="right">
                  <button
                    className="btn btn-outline-primary"
                    onClick={() => this.MostrarFormularioEditar()}
                  >
                    Nueva Versión
                  </button>
                </div>
                <br />
                <BootstrapTable
                  data={this.state.data}
                  hover={true}
                  pagination={true}
                >
                  <TableHeaderColumn
                    isKey={true}
                    dataField="Contrato"
                    dataFormat={this.ContratoFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Contrato
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="codigo"
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Oferta
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="version"
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Versión
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="descripcion"
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Descripción
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="estado"
                    dataFormat={this.EstadoFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Estado
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="es_final"
                    dataFormat={this.FinalFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataSort={true}
                  >
                    Es Final
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Operaciones"
                    dataFormat={this.GenerarBotones.bind(this)}
                  ></TableHeaderColumn>
                </BootstrapTable>
              </div>
              <div
                className="tab-pane fade show active"
                id="gestion"
                role="tabpanel"
                aria-labelledby="gestion-tab"
              >
                <div className="col" align="right">
                  <button
                    className="btn btn-outline-primary"
                    onClick={() => this.MostrarFormularioPresupuesto()}
                  >
                    Seleccionar SR
                  </button>
                </div>
                <br />
                <BootstrapTable
                  data={this.state.datapresupuesto}
                  hover={true}
                  pagination={true}
                >
                   <TableHeaderColumn
                    isKey={true}
                    dataField="Requerimiento"
                    dataFormat={this.CodigoFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                    width={"10%"}
                  >
                    Proyecto
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    
                    dataField="Presupuesto"
                    dataFormat={this.CodigoFormatoProyecto}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                    width={"10%"}
                  >
                    Requerimiento
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="Presupuesto"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataFormat={this.dateFormat}
                    dataSort={true}
                    width={"10%"}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  >
                    Fecha Presupuesto
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="Presupuesto"
                    dataFormat={this.DescripcionFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    Descripción
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="Presupuesto"
                    dataFormat={this.MontosContruccion}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    width={"10%"}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    M. Construcción
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Presupuesto"
                    dataFormat={this.MontosSuministros}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    width={"10%"}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    M. Suministros
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Presupuesto"
                    dataFormat={this.MontosIngenieria}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    width={"10%"}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    M. Ingeniería
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Presupuesto"
                    dataFormat={this.MontosSubContratos}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    width={"10%"}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    M. Sub Contratos
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Presupuesto"
                    dataFormat={this.MontosTotal}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    width={"10%"}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    M. Total
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="Presupuesto"
                    dataFormat={this.VersionFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    width={"8%"}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    Versión
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="Operaciones"
                    dataFormat={this.GenerarBotonesPresupuestos.bind(this)}
                  ></TableHeaderColumn>
                </BootstrapTable>

                <Dialog
                  header="Selección de Presupuesto"
                  visible={this.state.visiblepresupuesto}
                  width="70%"
                  modal={true}
                  onHide={this.OcultarFormularioPresupuesto}
                >
                  <BlockUi tag="div" blocking={this.state.blockingpresupuesto}>
                    <form
                      onSubmit={this.handleSubmit}
                      style={{ height: "300px" }}
                    >
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
                  </BlockUi>
                </Dialog>

                <Dialog
                  header="Eliminar"
                  visible={this.state.visibleeliminar}
                  width="350px"
                  footer={footer}
                  minY={70}
                  onHide={this.onHide}
                  maximizable={true}
                >
                  Esta Seguro de Guardar?
                </Dialog>
              </div>

              <div
                className="tab-pane fade"
                id="archivos"
                role="tabpanel"
                aria-labelledby="archivos-tab"
              ></div>

              <div
                className="tab-pane fade"
                id="op"
                role="tabpanel"
                aria-labelledby="op-tab"
              >
                <div className="row">
                  <div className="col">
                    <div className="callout callout-info">
                      <small className="text-muted">Monto Ingenieria</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={this.state.monto_so_ingenieria}
                          displayType={"text"}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>
                  <div className="col">
                    <div className="callout callout-danger">
                      <small className="text-muted">Monto Construcción</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={this.state.monto_so_construccion}
                          displayType={"text"}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>
                  <div className="col">
                    <div className="callout callout-warning">
                      <small className="text-muted">Monto Suministros</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={this.state.monto_so_suministros}
                          displayType={"text"}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>

                  <div className="col-sm-5 col-md-3">
                    <div className="callout callout-info">
                      <small className="text-muted">Monto Subcontratos</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={this.state.monto_so_subcontratos}
                          displayType={"text"}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>

                  <div className="col">
                    <div className="callout callout-success">
                      <small className="text-muted">Monto Total</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={this.state.monto_so_total}
                          displayType={"text"}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </div>
                </div>
                {/**
                <div className="col" align="right">
                  <button
                    className="btn btn-outline-primary"
                    onClick={this.mostrarForm}
                  >
                    Nueva PO
                  </button>
                </div>
                        <br />*/}
                <BootstrapTable
                  data={this.state.dataos}
                  hover={true}
                  pagination={true}
                >
                  <TableHeaderColumn
                    isKey={true}
                    width={"10%"}
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
                    dataField="fecha_orden_servicio"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataFormat={this.dateFormatOS}
                    dataSort={true}
                    width={"8%"}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  >
                    Fecha{" "}
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="monto_aprobado_construccion"
                    dataFormat={this.MontosFormato}
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
                    dataField="monto_aprobado_ingeniería"
                    dataFormat={this.MontosFormato}
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
                    dataField="monto_aprobado_suministros"
                    dataFormat={this.MontosFormato}
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
                    dataFormat={this.MontosFormato}
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
                    dataFormat={this.MontosFormato}
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
                  {/**  <TableHeaderColumn
                    dataField="Operaciones"
                    width={"13%"}
                    dataFormat={this.GenerarBotonesOS.bind(this)}
                  ></TableHeaderColumn>
                  */}
                </BootstrapTable>

                <Dialog
                  header="Nuevo"
                  visible={this.state.visibleorden}
                  onHide={this.OcultarFormularioOrden}
                  modal={true}
                  style={{ width: "500px", overflow: "auto" }}
                >
                  <div>
                    <form onSubmit={this.EnviarFormulario}>
                      <div className="row">
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
                </Dialog>
              </div>
            </div>
          </div>
        </div>
        </div>)}
     </BlockUi>
    );
  }
  EliminarOs = (Id) => {
    console.log(Id);
    axios
      .post("/Proyecto/OfertaComercial/GetDelete", formData, multipart)
      .then((response) => {
        if (response.data == "OK") {
          abp.notify.success("OS Eliminado", "Correcto");
          this.ConsultarOS();
        }
      })
      .catch((error) => {
        console.log(error);
        abp.notify.error(
          "Existe un incoveniente inténtelo más tarde ",
          "Error"
        );
      });
  };

  onDownloaImagen = (Id) => {
    return (window.location = `/Proyecto/OfertaComercial/Descargar/${Id}`);
  };
  ButtonsOs = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.mostrarForm(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar Descripción"
        >
          <i className="fa fa-edit" />
        </button>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
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
          title="Eliminar Ruta"
        >
          <i className="fa fa-trash" />
        </button>
        <button
          className="btn btn-outline-indigo"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.onDownloaImagen(row.ArchivoId)}
          data-toggle="tooltip"
          data-placement="top"
          title="Descargar Imagen"
        >
          <i className="fa fa-cloud-download"></i>
        </button>
      </div>
    );
  };

  isValid = () => {
    const errors = {};

    if (this.state.codigo_orden_servicio == "") {
      errors.codigo_orden_servicio = "Campo Requerido";
    }
    if (this.state.fecha_orden_servicio == null) {
      errors.fecha_orden_servicio = "Campo Requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  EnviarFormulario(event) {
    event.preventDefault();

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      if (this.state.action == "create") {
        const formData = new FormData();
        formData.append("Id", 0);
        formData.append(
          "OfertaComercialId",
          document.getElementById("OfertaComercialId").className
        );
        formData.append("ArchivoId", null);
        formData.append(
          "codigo_orden_servicio",
          this.state.codigo_orden_servicio
        );
        formData.append(
          "fecha_orden_servicio",
          this.state.fecha_orden_servicio
        );
        formData.append("monto_aprobado_os", this.state.monto_aprobado_os);
        formData.append(
          "monto_aprobado_suministros",
          this.state.monto_aprobado_suministros
        );
        formData.append(
          "monto_aprobado_construccion",
          this.state.monto_aprobado_construccion
        );
        formData.append(
          "monto_aprobado_construccion",
          this.state.monto_aprobado_ingenieria
        );
        formData.append("version_os", this.state.version_os);
        if (this.state.uploadFile != "") {
          formData.append("UploadedFile", this.state.uploadFile);
        } else {
          formData.append("UploadedFile", null);
        }
        const multipart = {
          headers: {
            "content-type": "multipart/form-data",
          },
        };
        axios
          .post("/Proyecto/OfertaComercial/GetCreate", formData, multipart)
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("OS Guardado", "Correcto");
              this.setState({ visibleorden: false });
              this.ConsultarOS();
              this.calcularmontos();
            }
            if (response.data == "CODE_EXIST") {
              abp.notify.error("El Código de la OS ya existe", "Error");
            }
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            );
          });
      } else {
        const formData = new FormData();
        formData.append("Id", this.state.Id);
        formData.append("OfertaComercialId", this.state.OfertaComercialId);
        formData.append("ArchivoId", null);
        formData.append(
          "codigo_orden_servicio",
          this.state.codigo_orden_servicio
        );
        formData.append(
          "fecha_orden_servicio",
          this.state.fecha_orden_servicio
        );
        formData.append("monto_aprobado_os", this.state.monto_aprobado_os);
        formData.append(
          "monto_aprobado_suministros",
          this.state.monto_aprobado_suministros
        );
        formData.append(
          "monto_aprobado_construccion",
          this.state.monto_aprobado_construccion
        );
        formData.append(
          "monto_aprobado_construccion",
          this.state.monto_aprobado_ingenieria
        );
        formData.append("version_os", this.state.version_os);
        if (this.state.uploadFile != "") {
          formData.append("UploadedFile", this.state.uploadFile);
        } else {
          formData.append("UploadedFile", null);
        }
        const multipart = {
          headers: {
            "content-type": "multipart/form-data",
          },
        };

        axios
          .post("/Proyecto/OfertaComercial/GetEdit", formData, multipart)
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("OS Guardado", "Correcto");
              this.setState({ visibleorden: false });
              this.ConsultarOS();
              this.calcularmontos();
            }
            if (response.data == "CODE_EXIST") {
              abp.notify.error("El Código de la OS ya existe", "Error");
            }
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            );
          });
      }
    }
  }
  mostrarForm = (row) => {
    console.log(row);
    if (row != null && row.Id > 0) {
      this.setState({
        Id: row.Id,
        codigo_orden_servicio: row.codigo_orden_servicio,
        fecha_orden_servicio: row.fecha_orden_servicio,
        version_os: row.version_os,
        monto_aprobado_os: row.monto_aprobado_os,
        monto_aprobado_suministros: row.monto_aprobado_suministros,
        monto_aprobado_construccion: row.monto_aprobado_construccion,
        monto_aprobado_ingeniería: row.monto_aprobado_ingeniería,
        ArchivoId: row.ArchivoId,
        OfertaComercialId: row.OfertaComercialId,
        action: "edit",
        visibleorden: true,
        editable: false,
      });
    } else {
      this.setState({
        Id: 0,
        codigo_orden_servicio: "",
        fecha_orden_servicio: null,
        version_os: "A",
        monto_aprobado_os: 0.0,
        monto_aprobado_suministros: 0.0,
        monto_aprobado_construccion: 0.0,
        monto_aprobado_ingeniería: 0.0,
        ArchivoId: null,
        OfertaComercialId: 0,
        uploadFile: "",
        action: "create",
        visibleorden: true,
        editable: true,
      });
    }
  };

  MontosContruccion(cell, row) {
    if (cell == null) {
      return (
        <CurrencyFormat
          value={0}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    } else {
      return (
        <CurrencyFormat
          value={cell.monto_construccion}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  }
  MontosIngenieria(cell, row) {
    if (cell == null) {
      return (
        <CurrencyFormat
          value={0}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    } else {
      return (
        <CurrencyFormat
          value={cell.monto_ingenieria}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  }
  MontosSuministros(cell, row) {
    if (cell == null) {
      return (
        <CurrencyFormat
          value={0}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    } else {
      return (
        <CurrencyFormat
          value={cell.monto_suministros}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  }
  MontosSubContratos = (cell, row) => {
    if (cell == null) {
      return (
        <CurrencyFormat
          value={0}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    } else {
      return (
        <CurrencyFormat
          value={cell.monto_subcontratos}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  };
  MontosTotal(cell, row) {
    if (row == null) {
      return (
        <CurrencyFormat
          value={0}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    } else {
      console.log("presupuesto", row);
      var total = row.Presupuesto != null ? row.Presupuesto.monto_total : 0;
      return (
        <CurrencyFormat
          value={total}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  }
  MontosFormato(cell, row) {
    if (cell == null) {
      return (
        <CurrencyFormat
          value={0}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    } else {
      return (
        <CurrencyFormat
          value={cell}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  }
  dateFormat(cell, row) {
    if (cell == null) {
      return "dd/mm/yy";
    } else {
      var x = cell.fecha_registro;
      return moment(x).format("DD/MM/YYYY");
    }
  }
  dateFormatOS(cell, row) {
    if (cell == null) {
      return "dd/mm/yy";
    }

    return moment(cell).format("DD/MM/YYYY");
  }
  Loading() {
    this.setState({ blocking: true });
  }

  CancelarLoading() {
    this.setState({ blocking: false });
  }

  ContratoFormato(cell, row) {
    return cell.Codigo;
  }
  ProyectoFormato(cell, row) {
    return cell.Proyecto.codigo;
  }

  DescripcionFormato(cell, row) {
    if (cell == null) {
      return "";
    } else {
      return cell.descripcion;
    }
  }

  VersionFormato(cell, row) {
    if (cell == null) {
      return "";
    } else {
      return cell.version;
    }
  }
  CodigoFormato(cell, row) {
    if (cell == null) {
      return "";
    } else {
      return cell.codigo;
    }
  }
  CodigoFormatoProyecto(cell, row) {
    if (cell == null) {
      return "";
    } else {
      if(cell.Proyecto!=null){
        return cell.Proyecto.codigo;
      }else{
        return "";
      }
      
    }
  }
  onClick(event) {
    this.setState({ visibleeliminar: true });
  }
  onHide(event) {
    this.setState({ visibleeliminar: false });
  }
  VerVersion(cell) {
    window.location.href = "/Proyecto/OfertaComercial/Details/" + cell.Id;
  }
  ObtenerCatalogos() {
    //tipotrabajo
    axios
      .post("/proyecto/catalogo/GetCatalogo/1003", {})
      .then((response) => {
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
  }

  ObtenerDetalleOferta() {
    axios
      .post(
        "/Proyecto/OfertaComercial/GetDetalleOferta/" +
          document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        console.log(response.data);
        this.setState({
          OfertaIdActual: response.data.Id,
          detalleoferta: response.data,
          descripcion: response.data.descripcion,
          codigo: response.data.codigo,
          tipo_trabajo_id: response.data.tipo_Trabajo_Id,
          tipo_contratacion_id: response.data.forma_contratacion,
          centro_costos_id: response.data.centro_de_Costos_Id,
          estado_oferta_id: response.data.estado_oferta,
          estatus_ejecucion_id: response.data.estatus_de_Ejecucion,
          alcance: response.data.alcance,
          codigo_shaya: response.data.codigo_shaya,
          estado: response.data.estado,
          service_request: response.data.service_request,
          service_order: response.data.service_order,
          revision: response.data.revision,
          descripcion: response.data.descripcion,
          actacierreid: response.data.acta_cierre,
          OfertaPadreId: response.data.OfertaPadreId,
          version: response.data.version,
          IdOferta: response.data.Id,
          ContratoId: response.data.ContratoId,
          fecha_registro: response.data.fecha_oferta,
          contratodescripcion:
            response.data.Contrato.Codigo +
            " " +
            response.data.Contrato.descripcion,
          nombre_estado: response.data.Catalogo.nombre,

          blocking: false,
          monto_ofertado: response.data.monto_ofertado,
          monto_aprobado: response.data.monto_so_aprobado,
          monto_pendiente_aprobacion:
            response.data.monto_ofertado_pendiente_aprobacion,
          comentarios: response.data.comentarios,
          link_documentum: response.data.link_documentum,
        });
      })
      .catch((error) => {
        this.setState({ blocking: false });
        this.alertMessage(
          "Ocurrió un error al consultar detalle de la Oferta Comercial"
        );
        console.log(error);
      });
  }

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
  ObtenerTieneTransmital = () => {
    axios
      .post(
        "/Proyecto/OfertaComercial/GetTieneTransmital/" +
          document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        console.log("tiene transmital", response.data);
        if (response.data != null && response.data == "SI") {
          this.setState({
            tieneTransmital: true,
          });
        }

        console.log(this.state.tieneTransmital);
        console.log(this.state.dataos);
      })
      .catch((error) => {
        console.log(error);
      });
  };
  EnvioManual = () => {
    console.log("ejecucion ");
    axios
      .post(
        "/Proyecto/OfertaComercial/GetMailto/" +
          document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        this.setState({ mailto: response.data });
        window.location.href = response.data;
      })
      .catch((error) => {
        console.log(error);
      });
  };
  ObtenerCodigoTransmital = () => {
    axios
      .post(
        "/Proyecto/OfertaComercial/GetCodigoTransmital/" +
          document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        console.log("codigo transmital", response.data);

        this.setState({
          codigoTransmital: response.data,
        });
      })
      .catch((error) => {
        console.log(error);
      });
  };
  ConsultarVersiones() {
    axios
      .post(
        "/Proyecto/OfertaComercial/ListarVersiones/" +
          document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        console.log("Versiones", response.data);
        if (response.data != "n") {
          this.setState({ data: response.data, blocking: false });
        }
      })
      .catch((error) => {
        this.setState({ blocking: false });
        this.alertMessage(
          "Ocurrió un error al consular las ofertas Comerciales"
        );
        console.log(error);
      });
  }

  ConsultarPresupuesto() {
    axios
      .post(
        "/Proyecto/OfertaComercial/ListarPresupuesto/" +
          document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        console.log("presupuesto", response.data);
        this.setState({ datapresupuesto: response.data, blocking: false });
      })
      .catch((error) => {
        this.setState({ blocking: false });
        this.alertMessage(
          "Ocurrió un error al consultar Presupuesto asignados"
        );
        console.log(error);
      });
  }

  ConsultarOS() {
    axios
      .post(
        "/Proyecto/OfertaComercial/GetOSbyOfertaComercial/" +
          document.getElementById("OfertaComercialId").className,
        {}
      )
      .then((response) => {
        console.log(response.data);
        this.setState({ dataos: response.data, blocking: false });
      })
      .catch((error) => {
        this.setState({ blocking: false });
        this.alertMessage("Ocurrió un error al consultar las OS");
        console.log(error);
      });
  }

  Editar(event) {
    console.log(this.state.link_documentum)
    event.preventDefault();

    axios
      .post("/proyecto/OfertaComercial/Editar", {
        TransmitalId: 0,
        estado: this.state.detalleestado,
        descripcion: this.state.descripcion,
        service_request: this.state.service_request,
        service_order: this.state.service_order,
        version: this.state.version,
        codigo: this.state.codigo,
        alcance: this.state.alcance,
        es_final: true,
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
        Id: this.state.IdOferta,
        ContratoId: this.state.ContratoId,
        link_documentum: this.state.link_documentum,
      })
      .then((response) => {
        if ((response.data = "e")) {
          this.growl.show({
            life: 5000,
            severity: "error",
            summary: "",
            detail: "Ocurrio  un error",
          });
        } else {
          this.growl.show({
            life: 5000,
            severity: "success",
            summary: "",
            detail: "Version Creada",
          });
          window.location.href =
            "/Proyecto/OfertaComercial/Details/" + response.data;
        }
      })
      .catch((error) => {
        console.log(error);
        this.growl.show({
          life: 5000,
          severity: "error",
          summary: "",
          detail: "Ocurrio un Error",
        });
      });
  }

  NuevaVersion(event) {
    event.preventDefault();

    axios
      .post("/proyecto/OfertaComercial/CrearNuevaVersion", {
        TransmitalId: 0,
        descripcion: this.state.descripcion,
        service_request: this.state.service_request,
        service_order: this.state.service_order,
        version: this.state.version,
        codigo: this.state.codigo,
        alcance: this.state.alcance,
        es_final: true,
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
        Id: this.state.IdOferta,
        ContratoId: this.state.ContratoId,
        comentarios: this.state.comentarios,
        estado_oferta: this.state.estado_oferta_id,
        link_documentum: this.state.link_documentum,
      })
      .then((response) => {
        if (response.data == "e") {
          this.growl.show({
            life: 5000,
            severity: "error",
            summary: "",
            detail: "Ocurrio  un error",
          });
        } else {
          this.growl.show({
            life: 5000,
            severity: "success",
            summary: "",
            detail: "Version Creada",
          });
          this.ObtenerTieneTransmital();
          this.ObtenerCodigoTransmital();
          this.ObtenerDetalleOferta();
          this.ConsultarVersiones();

          this.setState({ visible: false });
          window.location.href =
            "/Proyecto/OfertaComercial/Details/" +
            document.getElementById("OfertaComercialId").className;
        }
      })
      .catch((error) => {
        console.log(error);
        this.growl.show({
          life: 5000,
          severity: "error",
          summary: "",
          detail: "Ocurrio un Error",
        });
      });
  }

  EditarOferta(event) {
    event.preventDefault();

    axios
      .post("/proyecto/OfertaComercial/EditarOfertaComercial", {
        TransmitalId: 0,
        estado: this.state.detalleestado,
        descripcion: this.state.descripcion,
        service_request: this.state.service_request,
        service_order: this.state.service_order,
        version: this.state.version,
        codigo: this.state.codigo,
        alcance: this.state.alcance,
        es_final: true,
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
        Id: this.state.IdOferta,
        ContratoId: this.state.ContratoId,
        fecha_oferta: new Date(),
        comentarios: this.state.comentarios,
        estado_oferta: this.state.estado_oferta_id,
        link_documentum:this.state.link_documentum
      })
      .then((response) => {
        if (response.data == "e") {
          this.alertMessage("No se pudo Modificar");
          this.setState({ visibleEditar: false });
        } else {
          this.successMessage("Editado  Correctamente");
          this.ObtenerDetalleOferta();
          this.setState({ visibleEditar: false });
        }
      })
      .catch((error) => {
        console.log(error);
        this.alertMessage("No se Modifico");
        this.setState({ visibleEditar: false });
      });
  }
  OcultarFormulario() {
    this.setState({ visible: false });
  }

  OcultarFormularioEditar() {
    this.setState({ visibleEditar: false });
  }
  OcultarFormularioPresupuesto() {
    this.setState({ visiblepresupuesto: false });
  }

  MostrarFormularioEditar() {
    this.setState({ visible: true });
  }

  MostrarFormularioEditarOferta() {
    this.setState({ visibleEditar: true });
  }
  OcultarFormularioOrden() {
    this.setState({ visibleorden: false });
  }

  MostrarFormularioOrden() {
    this.setState({ visibleorden: true });
    this.getProyectos();
  }
  MostrarFormularioArchivo() {
    this.setState({ visiblearchivo: true });
  }
  OcultarFormularioArchivo() {
    this.setState({ visiblearchivo: false });
  }

  MostrarFormularioPresupuesto() {
    this.setState({ visiblepresupuesto: true });
    this.getProyectos();
  }

  showSuccess() {
    this.growl.show({
      life: 5000,
      severity: "success",
      summary: "",
      detail: this.state.message,
    });
  }

  showWarn() {
    this.growl.show({
      life: 5000,
      severity: "error",
      summary: "",
      detail: this.state.message,
    });
  }

  showAlert() {
    this.growl.show({
      severity: "warn",
      summary: "",
      detail: this.state.message,
    });
  }

  successMessage(msg) {
    this.setState({ message: msg }, this.showSuccess);
  }

  warnMessage(msg) {
    this.setState({ message: msg }, this.showWarn);
  }

  alertMessage(msg) {
    this.setState({ message: msg }, this.showAlert);
  }
  getContratos() {
    axios
      .post("/Proyecto/Contrato/GetContratosApi", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ contratos: items });
      })
      .catch((error) => {
        console.log(error);
      });
  }
  GenerarBotones(cell, row) {
    return (
      <div>
        <button
          className="btn btn-outline-success btn-sm"
          onClick={() => {
            this.VerVersion(row);
          }}
          style={{ float: "left", marginRight: "0.3em" }}
        >
          Ver
        </button>
        {/*  <button
          className="btn btn-outline-info btn-sm"
          onClick={() => {
            this.EliminarOs(row.Id);
          }}
          style={{ float: "left", marginRight: "0.3em" }}
        >
          Eliminar
        </button>*/}
      </div>
    );
  }
  GenerarBotonesPresupuestos(cell, row) {
    return (
      <div>
        <button
          className="btn btn-outline-info btn-sm"
          onClick={() => {
            if (window.confirm("Estás segur@?")) this.Eliminar(row.Id);
          }}
          style={{ float: "left", marginRight: "0.3em" }}
        >
          Eliminar
        </button>
      </div>
    );
  }
  onDownload = (Id) => {
    return (window.location = `/Proyecto/TransmitalCabecera/Descargar/${Id}`);
  };

  GenerarBotonesOS(cell, row) {
    return (
      <div>
        <button
          className="btn btn-outline-indigo btn-sm"
          style={{ marginRight: "0.3em" }}
          onClick={() => this.onDownload(row.ArchivoId)}
          data-toggle="tooltip"
          data-placement="top"
          title="Descargar Documento"
        >
          <i className="fa fa-cloud-download"></i>
        </button>
        {/**  <button
          className="btn btn-outline-success btn-sm"
          onClick={() => {
            this.IrOrdenServicio(row.Id);
          }}
          style={{ marginRight: "0.3em" }}
        >
          Ver
        </button>
        <button
          className="btn btn-outline-primary btn-sm"
          onClick={() => {
            this.mostrarForm(row);
          }}
          style={{ marginRight: "0.3em" }}
        >
          Editar
        </button>
        <button
          className="btn btn-outline-danger btn-sm"
          onClick={() => {
            if (
              window.confirm("Esta Seguró? Esta acción eliminará este registro")
            )
              this.EliminarOs(row.id);
          }}
          style={{ marginRight: "0.3em" }}
        >
          Eliminar
        </button>
      */}
      </div>
    );
  }

  ///seleccione
  handleSubmit(event) {
    event.preventDefault();
    if (this.state.ProyectoId == 0) {
      this.setState({ message: "Selecciona un proyecto" }, this.showWarn);
    } else if (this.state.RequerimientoId == 0) {
      this.setState({ message: "Selecciona un Sr" }, this.showWarn);
    } else {
      axios
        .post("/proyecto/OfertaComercial/CrearOfertaCPresupuesto", {
          OfertaComercialId: this.state.OfertaIdActual,
          RequerimientoId: this.state.RequerimientoId,
          PresupuestoId: 0,
          fecha_asignacion: new Date(),
          vigente: true,
        })
        .then((response) => {
          console.log(response.data);
          if (response.data == "creado") {
            console.log("k");
            this.successMessage("Presupuesto Agregado Correctamente");

            this.setState({ visiblepresupuesto: false });
            this.ConsultarPresupuesto();
            this.ObtenerMontosRequerimientos();
            this.calcularmontos();
          } else if (response.data == "no_tiene_presupuesto_definitivo") {
            this.alertMessage(
              "El Requerimiento no tiene un Presupuesto Definitivo"
            );
          } else {
            this.alertMessage(response.data);
          }
        })
        .catch((error) => {
          console.log(error);
          this.alertMessage("Ocurrió un Error");
        });
    }
  }

  handleSubmitOrden(event) {
    event.preventDefault();
    axios
      .post("/proyecto/OfertaComercial/CrearOS", {
        OfertaComercialId: this.state.OfertaIdActual,
        codigo_orden_servicio: this.state.codigo_orden_servicio,
        fecha_orden_servicio: this.state.fecha_orden_servicio,
        monto_aprobado_os: this.state.monto_aprobado_os,
        monto_aprobado_suministros: this.state.monto_aprobado_suministros,
        monto_aprobado_construccion: this.state.monto_aprobado_construccion,
        monto_aprobado_ingenieria: this.state.monto_aprobado_ingenieria,
        version_os: this.state.version_os,
        vigente: true,
      })
      .then((response) => {
        if ((response.data = "o")) {
          this.setState({
            visibleorden: false,
            codigo_orden_servicio: "",
            fecha_orden_servicio: null,
            monto_aprobado_os: 0,
            monto_aprobado_suministros: 0,
            monto_aprobado_construccion: 0,
            monto_aprobado_ingenieria: 0,
            version_os: "",
          });
          this.successMessage("OP Creada Correctamente");
          this.ConsultarOS();
          this.calcularmontos();
        } else {
          this.alertMessage("Ocurrió un Error");
        }
      })
      .catch((error) => {
        console.log(error);
        this.alertMessage("Ocurrió un Error");
      });
  }

  Eliminar(id) {
    axios
      .post("/proyecto/OfertaComercial/Eliminar", {
        Id: id,
      })
      .then((response) => {
        if ((response.data = "o")) {
          this.successMessage("ELiminado Correctamente");
          this.ConsultarPresupuesto();
          this.ObtenerMontosRequerimientos();
          this.calcularmontos();
        } else {
          this.alertMessage("Ocurrió un Error");
        }
      })
      .catch((error) => {
        console.log(error);
        this.alertMessage("Ocurrió un Error");
      });
  }
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
  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };

  EstadoFormato(cell, row) {
    if (cell == 0) {
      return "Por Enviar";
    } else {
      return "Terminado";
    }
  }
  FinalFormato(cell, row) {
    if (cell == 1) {
      return "SI";
    } else {
      return "NO";
    }
  }

  IrOrdenServicio(id) {
    window.location.href = "/Proyecto/DetalleOrdenServicio/Index/" + id;
  }

  handleChangeProyecto(e) {
    this.setState({ ProyectoId: e });
    console.log("entro evento" + e);
    this.setState(
      {
        ProyectoId: e,
        RequerimientoId: 0,
        requerimientos: [],
        ofertas: [],
        PresupuestoId: 0,
        blockingpresupuesto: true,
      },
      this.getRequerimientos
    );
  }

  handleChangeRequerimientos(e) {
    this.setState({
      RequerimientoId: e,
      PresupuestoId: 0,
      ofertas: [],
      blockingpresupuesto: false,
    });
  }

  getProyectos() {
    axios
      .post("/Proyecto/Proyecto/GetProyectosApi/" + this.state.ContratoId, {})
      .then((response) => {
        var items = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.nombre_proyecto,
            dataKey: item.Id,
            value: item.Id,
          };
        });

        this.setState({ proyectos: items, blockingpresupuesto: false });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getArchivos() {
    axios
      .post(
        "/Proyecto/Proyecto/ListarArchivos/" + this.state.OfertaIdActual,
        {}
      )
      .then((response) => {
        this.setState({ archivos: response.data, blockingpresupuesto: false });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getRequerimientos() {
    axios
      .post(
        "/Proyecto/Oferta/GetRequerimientosProyectoApi/" +
          this.state.ProyectoId,
        {}
      )
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.descripcion,
            dataKey: item.Id,
            value: item.Id,
          };
        });

        this.setState({ requerimientos: items, blockingpresupuesto: false });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getOfertas() {
    axios
      .post("/Proyecto/OfertaComercial/ListarPorRequerimiento/", {
        id: this.state.RequerimientoId,
        tipo: document.getElementById("content").className,
      })
      .then((response) => {
        this.setState({ ofertas: response.data, blockingpresupuesto: false });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getFormSelectContratos(list) {
    return list.map((item) => {
      return (
        <option key={item.Id} value={item.Id}>
          {item.Codigo}
        </option>
      );
    });
  }

  getFormSelect(list) {
    return list.map((item) => {
      return (
        <option key={item.Id} value={item.Id}>
          {item.codigo}
        </option>
      );
    });
  }

  getFormSelectOferta(list) {
    return list.map((item) => {
      return (
        <option key={item.Id} value={item.Id}>
          {item.codigo}- {item.descripcion} - {item.version}
        </option>
      );
    });
  }

  calcularmontos() {
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
  }

  getdetallesordenesservicio() {
    if (IdOS > 0) {
      axios
        .post("/Proyecto/OfertaComercial/ListarDetalleOS/", { Id: IdOS })
        .then((response) => {
          this.setState({ datadetalles: response.data });
        })
        .catch((error) => {
          console.log(error);
        });
    }
  }

  ConsultarColaboradores() {
    this.setState({ blocking: true });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerColaboradores/", {})
      .then((response) => {
        console.log(response.data);
        var colaborador = response.data.map((item) => {
          return { label: item.nombres, dataKey: item.Id, value: item.Id };
        });
        this.setState({ colaboradores: colaborador, blocking: false });
      })
      .catch((error) => {
        this.setState({ blocking: false });
        this.alertMessage("Ocurrió un error al  Colaboradores");
        console.log(error);
      });
  }

  getcabeceraos() {
    if (IdOS > 0) {
      axios
        .post("/Proyecto/OfertaComercial/GetOrdenServicio/", { Id: IdOS })
        .then((response) => {
          this.setState({
            cabeceraos: response.data,
            cabeceraosId: response.data.Id,
          });
        })
        .catch((error) => {
          console.log(error);
        });
    }
  }
}
ReactDOM.render(<DetalleOfertaComercial />, document.getElementById("content"));
