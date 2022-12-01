import React, { Component } from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import moment from "moment";
import BlockUi from "react-block-ui";
import Formsy from "formsy-react";
import Field from "./Base/Field-v2";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import { Dropdown } from "primereact/components/dropdown/Dropdown";
import { Button } from "primereact/components/button/Button";
import ArbolWbs from "./WbsPresupuesto/ArbolWbs";
import ActividadFormPresupuesto from "./WbsPresupuesto/ActividadForm";
import NivelFormPresupuestos from "./WbsPresupuesto/NivelForm";
import WbsSelect from "./WbsPresupuesto/WbsSelect";
import { ContextMenu } from "primereact-v2/contextmenu";

class WbsPresupuestoContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: true,
      visible: false,
      visible_actividades: false,
      PadreSeleccionado: ".",
      NivelIdSeleccionado: 0,
      NombreNivel: "",
      data: [],
      key: 46978,
      disciplinas: [],
      actividades: [],
      ActividadIdSeleccionada: 0,
      expandir: true,

      //Parte Derecha
      codigonivel: "",
      disciplinaid: 0,
      nombredisciplina: "",
      observacion: "",
      revision: "",
      fechainicio: null,
      fechafin: null,
      visible_actividades_form: false,
      actividadseleccionada: 0,
      visibleRight: false,
      ofertaid: 0,
      es_actividad: 0,
      estado: 0,
      vigente: 1,
      expandedKeys: {},
      tiporequerimiento: 1,

      visiblecatalogo: false,
      nuevocatalogo: "",
      nombreactividad: "",
      //mostrar dialogo copiar y pegar
      visible_copia: false,
      origen: 0,
      destino: 0,

      //
    };
    this.WbsSelect = React.createRef();
    this.onClick = this.onClick.bind(this);
    this.onHide = this.onHide.bind(this);
    this.updateData = this.updateData.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.onDelete = this.onDelete.bind(this);
    this.showActividad = this.showActividad.bind(this);
    this.onHideVisibleActividad = this.onHideVisibleActividad.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.onDragDrop = this.onDragDrop.bind(this);
    this.onSelect = this.onSelect.bind(this);
    this.showForm = this.showForm.bind(this);
    this.onHideVisibleActividad = this.onHideVisibleActividad.bind(this);
    this.getCatalogos = this.getCatalogos.bind(this);
    this.successMessage = this.successMessage.bind(this);
    this.warnMessage = this.warnMessage.bind(this);
    this.Refrescar = this.Refrescar.bind(this);
    this.Expander = this.Expander.bind(this);
    this.getRequerimiento = this.getRequerimiento.bind(this);
    this.NuevoCatalogo = this.NuevoCatalogo.bind(this);
    this.showRegistroActividadButton = this.showRegistroActividadButton.bind(
      this
    );
    this.MostrarNuevoCatalogo = this.MostrarNuevoCatalogo.bind(this);
    this.onHideCatalgo = this.onHideCatalgo.bind(this);

    this.MostrarDialogo = this.MostrarDialogo.bind(this);
    this.OcultarDialogo = this.OcultarDialogo.bind(this);
    this.EstablecerNodoOrigen = this.EstablecerNodoOrigen.bind(this);
    this.EstablecerNodoDestino = this.EstablecerNodoDestino.bind(this);
    this.SetearPresupuesto = this.SetearPresupuesto.bind(this);
    this.EnviarWBS = this.EnviarWBS.bind(this);
    this.DesbloquearPantalla = this.DesbloquearPantalla.bind(this);
    this.BloquearPantalla = this.BloquearPantalla.bind(this);
    this.handleChangeLast = this.handleChangeLast.bind(this);
  }

  componentWillMount() {
    this.updateData();
    this.getCatalogos();
    this.getRequerimiento();
  }

  MostrarDialogo() {
    this.setState({ visible_copia: false });
    //this.setState({ visible_copia: true });
  }
  OcultarDialogo() {
    this.setState({ visible_copia: false, origen: 0, destino: 0, label: "" });
  }
  EstablecerNodoDestino(id) {
    this.setState({ destino: parseInt(id) });
  }
  EstablecerNodoOrigen(event) {
    this.setState({ origen: event.value });
  }

  SetearPresupuesto(data) {
    this.setState({ data: data });
  }

  DesbloquearPantalla() {
    this.setState({ blocking: false });
  }

  BloquearPantalla() {
    this.setState({ blocking: true });
  }

  NuevoCatalogo() {
    if (this.state.nuevocatalogo == "") {
      this.warnMessage("Nombre Actividad obligatorio");
    } else {
      axios
        .post("/proyecto/WbsPresupuesto/CreateCatalogo", {
          TipoCatalogoId: 4,
          nombre: this.state.nuevocatalogo,
          descripcion: this.state.nuevocatalogo,
          codigo: ".",
          predeterminado: 1,
          vigente: true,
          ordinal: 1,
        })
        .then((response) => {
          if (response.data == "Ok") {
            this.getCatalogos();
            this.setState({ nuevocatalogo: "", visiblecatalogo: false });
            this.successMessage("Actividad Agregada al Catálogo");
          } else {
            this.warnMessage(
              "Ya existe una Actividad con el mismo nombre dentro del Catálogo"
            );
          }
        })
        .catch((error) => {
          console.log(error);
          this.warnMessage("Ocurrio un error");
        });
    }
  }

  showRegistroActividadButton() {
    //  if (this.state.tiporequerimiento == 0) {

    return (
      <Button
        type="button"
        label="Nueva Actividad"
        icon="fa fa-fw fa-folder-open"
        onClick={this.MostrarNuevoCatalogo}
      />
    );
    // }
  }

  render() {
    const footer = (
      <div>
        <Button
          label="Enviar"
          icon="pi pi-check"
          onClick={(e) => {
            if (window.confirm("Estás Seguro?")) this.EnviarWBS();
          }}
        />
        <Button
          label="Cancelar"
          icon="pi pi-times"
          onClick={this.OcultarDialogo}
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
        <div className="row">
          <div className="col">
            <Button
              label="Ingresar Nivel"
              icon="pi pi-upload"
              onClick={this.onClick}
            />
            <Button
              label="Refrescar"
              icon="pi pi-refresh"
              onClick={this.Refrescar}
            />
            <Button
              onClick={this.Expander}
              label="Exp/Contraer"
              icon="pi pi-upload"
            />
            {/*this.showRegistroActividadButton()*/}
            <hr />
          </div>
          <div className="col"></div>
        </div>

        <div className="row" style={{ height: "500px", maxHeight: "550px" }}>
          <div
            className="col-sm-8"
            style={{ overflowX: "scroll", overflowY: "scroll" }}
          >
            <ArbolWbs
              updateData={this.updateData}
              DesbloquearPantalla={this.DesbloquearPantalla}
              BloquearPantalla={this.BloquearPantalla}
              MostrarDialogo={this.MostrarDialogo}
              onDragDrop={this.onDragDrop}
              key={this.state.key}
              onSelect={this.onSelect}
              data={this.state.data}
              onToggle={(e) => this.setState({ expandedKeys: e.value })}
              expandedKeys={this.state.expandedKeys}
              onContextMenuSelectionChange={(event) =>
                this.EstablecerNodoOrigen(event)
              }
            />
          </div>

          <div className="col-sm-12 col-md-4">
            <div className="card">
              <div className="card-body">
                <div className="form-group">
                  <input
                    type="hidden"
                    value={this.state.actividadseleccionada}
                    className="form-control"
                    disabled
                  />
                </div>

                <div className="form-group">
                  <b>
                    <label style={{ fontSize: "12px" }}>
                      Actividad :&nbsp;
                    </label>
                  </b>
                  <label style={{ fontSize: "12px" }}>
                    {this.state.nombredisciplina}
                  </label>
                </div>

                <div className="form-group">
                  <b>
                    <label style={{ fontSize: "12px" }}>
                      {" "}
                      Observación :&nbsp;
                    </label>
                  </b>
                  <label style={{ fontSize: "12px" }}>
                    {this.state.observacion}
                  </label>
                </div>

                <div className="form-group">
                  <b>
                    <label style={{ fontSize: "12px" }}>
                      Fecha Inicio :&nbsp;
                    </label>
                  </b>
                  <label style={{ fontSize: "12px" }}>
                    {this.state.fechainicio}
                  </label>
                </div>

                <div className="form-group">
                  <b>
                    <label style={{ fontSize: "12px" }}>
                      Fecha Fin :&nbsp;
                    </label>
                  </b>
                  <label style={{ fontSize: "12px" }}>
                    {this.state.fechafin}
                  </label>
                </div>

                <button
                  className="btn btn-outline-primary"
                  onClick={this.showForm}
                  style={{ float: "left", marginRight: "0.3em" }}
                >
                  Editar
                </button>
                <button
                  className="btn btn-outline-danger"
                  onClick={() => {
                    if (
                      window.confirm(
                        "Estás seguro de eliminar este registro del WBS?"
                      )
                    )
                      this.onDelete(this.state.actividadseleccionada);
                  }}
                  style={{ float: "left", marginRight: "0.3em" }}
                >
                  Eliminar
                </button>
              </div>
            </div>
          </div>
          <Dialog
            header="Registro de Nuevas Actividades"
            visible={this.state.visiblecatalogo}
            width="600px"
            modal={true}
            onHide={this.onHideCatalgo}
          >
            <Formsy
              onValidSubmit={() => this.NuevoCatalogo()}
              method="post"
              ref="form"
            >
              <Field
                name="nuevocatalogo"
                label="Nombre Actividad"
                edit={true}
                readOnly={false}
                value={this.state.nuevocatalogo}
                onChange={this.handleChangeLast}
              />

              <button type="submit" className="btn btn-primary">
                Guardar
              </button>
              <button
                style={{ marginLeft: "0.4em" }}
                type="button"
                className="btn btn-primary"
                onClick={this.onHideCatalgo}
              >
                Cancelar
              </button>
            </Formsy>
          </Dialog>

          <Dialog
            header="Actividades"
            visible={this.state.visible_actividades_form}
            width="350px"
            modal={true}
            onHide={this.onHideVisibleActividad}
          >
            <Formsy
              onValidSubmit={() => this.handleSubmit()}
              method="post"
              ref="form"
            >
              <div className="form-group">
                <label htmlFor="label">Disciplina:</label>
                <Dropdown
                  value={this.state.disciplinaid}
                  options={this.state.disciplinas}
                  onChange={(e) => {
                    this.setState({ disciplinaid: e.value });
                  }}
                  filter={true}
                  filterPlaceholder="Selecciona una Disciplina"
                  filterBy="label,value"
                  placeholder="Selecciona una Disciplina"
                  style={{ width: "100%", heigh: "18px" }}
                  required
                />
              </div>

              <div className="form-group">
                <label>Observación</label>
                <input
                  id="no-filter"
                  name="observacion"
                  className="form-control"
                  onChange={this.handleChange}
                  validations="isText"
                  value={this.state.observacion}
                />
              </div>

              <div className="form-group">
                <label>Fecha Inicio</label>
                <input
                  type="date"
                  id="no-filter"
                  name="fechainicio"
                  className="form-control"
                  onChange={this.handleChange}
                  value={moment(this.state.fechainicio).format("YYYY-MM-DD")}
                  required
                />
              </div>

              <div className="form-group">
                <label>Fecha Fin</label>
                <input
                  type="date"
                  id="no-filter"
                  name="fechafin"
                  className="form-control"
                  onChange={this.handleChange}
                  value={moment(this.state.fechafin).format("YYYY-MM-DD")}
                  required
                />
              </div>

              <button type="submit" className="btn btn-primary">
                Guardar
              </button>
              <button
                style={{ marginLeft: "0.4em" }}
                type="button"
                className="btn btn-primary"
                onClick={this.onHideVisibleActividad}
              >
                Cancelar
              </button>
            </Formsy>
          </Dialog>

          <Dialog
            header={this.state.NombreNivel}
            visible={this.state.visible}
            width="775px"
            modal={true}
            onHide={this.onHide}
          >
            <NivelFormPresupuestos
              //key={this.props.key_nivel_form}
              codigo_padre={this.state.PadreSeleccionado}
              updateData={this.updateData}
              getCatalogos={this.getCatalogos}
              showActividad={this.showActividad}
              showSuccess={this.successMessage}
              showWarn={this.warnMessage}
              onHide={this.onHide}
              WbsIdSeleccionado={this.state.NivelIdSeleccionado}
              NombreNivel={this.state.NombreNivel}
              presupuestoId={
                document.getElementById("content-presupuestos").className
              }
              requerimiento={this.state.tiporequerimiento}
            />
          </Dialog>

          <Dialog
            header="Actividades"
            visible={this.state.visible_actividades}
            width="660px"
            height="450px"
            modal={true}
            onHide={this.onHideVisibleActividad}
          >
            <ActividadFormPresupuesto
              ListActividades={this.state.actividades}
              updateData={this.updateData}
              getCatalogos={this.getCatalogos}
              codigo_padre={this.state.PadreSeleccionado}
              onHide={this.onHideVisibleActividad}
              // key={this.state.key_actividad_form}
              showSuccess={this.successMessage}
              showWarn={this.warnMessage}
              tiporequerimiento={this.state.tiporequerimiento}
              MostrarNuevoCatalogo={this.MostrarNuevoCatalogo}
              presupuestoId={
                document.getElementById("content-presupuestos").className
              }
            />
          </Dialog>

          <Dialog
            header="Seleccionar Destino"
            visible={this.state.visible_copia}
            style={{ width: "70vw" }}
            modal={true}
            footer={footer}
            onHide={this.OcultarDialogo}
          >
            <WbsSelect
              updateData={this.updateData}
              DesbloquearPantalla={this.DesbloquearPantalla}
              BloquearPantalla={this.BloquearPantalla}
              ref={this.WbsSelect}
              OfertaId={document.getElementById("PresupuestoId").className}
              data={this.state.data}
              EstablecerNodoDestino={this.EstablecerNodoDestino}
              SetearDatosRDO={this.SetearPresupuesto}
              MostrarDialogo={this.MostrarDialogo}
            />
          </Dialog>
        </div>
      </BlockUi>
    );
  }

  onClick(event) {
    event.preventDefault();
    this.setState({
      visible: true,
      PadreSeleccionado: ".",
      NivelIdSeleccionado: 0,
      NombreNivel: "",
    });
  }

  updateData() {
    this.setState({ blocking: true });
    axios
      .get(
        "/proyecto/WbsPresupuesto/ApiWbs/" +
          document.getElementById("PresupuestoId").className,
        {}
      )
      .then((response) => {
        this.setState({ data: response.data, blocking: false });
      })
      .catch((error) => {
        this.setState({ blocking: false });
        console.log(error);
      });
  }

  handleSubmit() {
    event.preventDefault();
    axios
      .post("/proyecto/WbsPresupuesto/Edit", {
        PresupuestoId: this.state.ofertaid,
        Id: this.state.actividadseleccionada,
        estado: this.state.estado,
        observaciones: this.state.observacion,
        fecha_final: moment(this.state.fechafin).format("YYYY-MM-DD"),
        fecha_inicial: moment(this.state.fechainicio).format("YYYY-MM-DD"),
        es_actividad: this.state.es_actividad,
        vigente: this.state.vigente,
        id_nivel_codigo: this.state.PadreSeleccionado,
        id_nivel_padre_codigo: this.state.codigonivel,
        nivel_nombre: this.state.nombredisciplina,
        vigente: true,
        DisciplinaId: this.state.disciplinaid,
      })
      .then((response) => {
        if (response.data == "ErrorFechas") {
          this.warnMessage("La fecha final debe ser mayor a la inicial");
        } else {
          this.updateData();
          this.onHide();
          this.successMessage("Actividad Actualizada");
          this.setState({ visible_actividades_form: false });
        }
      })
      .catch((error) => {
        console.log(error);
        this.warnMessage("No se puedo realizar la acción");
      });
  }

  onDelete(id) {
    if (this.state.actividadseleccionada > 0) {
      event.preventDefault();
      axios
        .post("/proyecto/WbsPresupuesto/Delete/" + id, {})
        .then((response) => {
          if (response.data == "Ok") {
            this.growl.show({
              severity: "success",
              summary: "Correcto",
              detail: "Se Elimino el registro",
            });
            this.updateData();
          } else {
            this.warnMessage("La actividad tiene computos registrados");
          }
        })
        .catch((error) => {
          this.showWarn("No se puedo eliminar el registro");
        });
    } else {
      this.warnMessage("Debe seleccionar una actividad");
    }
  }

  Refrescar(event) {
    this.updateData();
  }

  showActividad() {
    this.setState({ visible_actividades: true, visible: false });
  }

  onHideVisibleActividad(event) {
    this.setState({
      visible_actividades: false,
      NombreNivel: "",
      PadreSeleccionado: "",
      visible: false,
      codigonivel: "",
      NivelIdSeleccionado: 0,
      NombreNivel: "",
      visibleRight: false,
      visible_actividades_form: false,
      actividadseleccionada: 0,
      ActividadIdSeleccionada: 0,
      observacion: "",
      revision: "",
      fechainicio: null,
      fechafin: null,
      es_actividad: 0,
      estado: 0,
      nombredisciplina: "",
      disciplinaid: 0,
    });
  }

  onHideCatalgo(event) {
    this.setState({
      visiblecatalogo: false,
      nuevocatalogo: "",
    });
  }
  handleChangeLast = (event) => {
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
      console.log("nombre Actividad", event.target.value);
    }
  };

  handleChange(event) {
    event.stopPropagation();
    this.setState({ [event.target.name]: event.target.value });
  }

  onDragDrop(event) {
    this.setState({ data: event.value, blocking: false });
  }

  onSelect(event) {
    var ids = event.node.data.split(",");
    if (event.node.tipo == "padre") {
      this.setState({
        PadreSeleccionado: ids[0],
        visible: true,
        codigonivel: ids[1],
        NivelIdSeleccionado: ids[2],
        NombreNivel: event.node.label,
        visibleRight: false,
        visible_actividades_form: false,
        ofertaid: ids[3],
        actividadseleccionada: 0,
        ActividadIdSeleccionada: 0,
        observacion: "",
        revision: "",
        fechainicio: null,
        fechafin: null,
        es_actividad: 0,
        estado: 0,
        nombredisciplina: "",
        disciplinaid: 0,
      });
    } else {
      this.setState({
        PadreSeleccionado: ids[0],
        codigonivel: ids[1],
        actividadseleccionada: ids[2],
        nombredisciplina: event.node.label,
        ofertaid: ids[3],
        observacion: ids[4],
        fechainicio: ids[5],
        fechafin: ids[6],
        es_actividad: ids[7],
        estado: ids[8],
        disciplinaid: ids[9],
      });
      if (ids[5] == "1/1/0001") {
        this.setState({ fechainicio: "dd/mm/aaaa" });
      } else {
        this.setState({ fechainicio: ids[5] });
      }
      if (ids[6] == "1/1/0001") {
        this.setState({ fechafin: "dd/mm/aaaa" });
      } else {
        this.setState({ fechafin: ids[6] });
      }
    }
    this.setState({ selectedFile: event.node });
  }

  showForm() {
    if (this.state.actividadseleccionada > 0) {
      this.setState({ visible_actividades_form: true });
    } else {
      this.growl.show({
        severity: "warn",
        summary: "Error",
        detail: "Debe seleccionar una actividad",
      });
    }
  }
  MostrarNuevoCatalogo() {
    this.setState({ visiblecatalogo: true });
  }
  onHide(event) {
    this.setState({ visible: false, NombreNivel: "" });
  }

  onHideVisibleActividad(event) {
    this.setState({
      visible_actividades: false,
      NombreNivel: "",
      PadreSeleccionado: "",
      visible: false,
      codigonivel: "",
      NivelIdSeleccionado: 0,
      NombreNivel: "",
      visibleRight: false,
      visible_actividades_form: false,
      actividadseleccionada: 0,
      ActividadIdSeleccionada: 0,
      observacion: "",
      fechainicio: null,
      fechafin: null,
      es_actividad: 0,
      estado: 0,
      nombredisciplina: "",
      disciplinaid: 0,
    });
  }

  getCatalogos() {
    axios
      .post("/proyecto/catalogo/GetCatalogo/4", {})
      .then((response) => {
        var actividades = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ actividades: actividades });
      })
      .catch((error) => {
        console.log(error);
      });

    axios
      .post("/proyecto/catalogo/GetCatalogo/2", {})
      .then((response) => {
        var disciplinas = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ disciplinas: disciplinas });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getRequerimiento() {
    var requerimiento = document.getElementById("RequerimientoTipo").className;

    if (requerimiento == "1") {
      this.setState({ tiporequerimiento: 1 });
    } else {
      this.setState({ tiporequerimiento: 0 });
    }
  }

  Expander() {
    this.setState({ blocking: true });
    axios
      .post(
        "/proyecto/WbsPresupuesto/ApiWbsK/" +
          document.getElementById("PresupuestoId").className,
        {}
      )
      .then((response) => {
        var llaves = response.data;
        if (this.state.expandir) {
          let expandedKeys = {};
          llaves.forEach((product) => {
            if (expandedKeys[product]) delete expandedKeys[product];
            else expandedKeys[product] = true;
          });
          this.setState({
            expandedKeys: expandedKeys,
            expandir: false,
            blocking: false,
          });
        } else {
          this.setState(
            { expandedKeys: {}, expandir: true, blocking: false },
            this.props.DesbloquearPantalla
          );
        }
      })
      .catch((error) => {
        console.log(error);
        this.setState({ blocking: false });
      });
  }

  showSuccess() {
    this.growl.show({
      life: 5000,
      severity: "info",
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

  successMessage(msg) {
    this.setState({ message: msg }, this.showSuccess);
  }

  warnMessage(msg) {
    this.setState({ message: msg }, this.showWarn);
  }

  EnviarWBS() {
    /*this.WbsRdo.current.updateData();
        this.OcultarDialogo();*/

    if (this.state.origen === 0) {
      this.warnMessage("Selecciona el WBS origen");
    } else if (this.state.destino === 0) {
      this.warnMessage("Selecciona el WBS destino");
    } else {
      this.BloquearPantalla();
      axios
        .post("/proyecto/WbsPresupuesto/CreateCopiaWbs/", {
          origen: this.state.origen,
          destino: this.state.destino,
          PresupuestoId: document.getElementById("PresupuestoId").className,
        })
        .then((response) => {
          if (response.data === "OK") {
            this.updateData();
            this.OcultarDialogo();
            this.DesbloquearPantalla();
            this.successMessage("Se copio el WBS correctamente");
          } else {
            this.warnMessage("Revisa el origen y destino del WBS");
            this.DesbloquearPantalla();
          }
        })
        .catch((error) => {
          console.log(error);
          this.warnMessage("Intentalo más tarde");
          this.DesbloquearPantalla();
        });
    }
  }
}

ReactDOM.render(
  <WbsPresupuestoContainer />,
  document.getElementById("content-presupuestos")
);
