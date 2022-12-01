import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
import moment from "moment";
import { MultiSelect } from "primereact-v2/multiselect";
class MigracionContratos extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visible: false,
      errors: {},
      editable: true,
      data: [],
      list: "",
      action: "create",
      Id: 0,
      desde: 0,
      hasta: 0
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.mostrarForm = this.mostrarForm.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind();
    this.isValid = this.isValid.bind();
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
  }

  componentDidMount() {
    this.props.unlockScreen();
  }

  UpdateMontos = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/OfertaComercial/UpdateMontos", {})
      .then((response) => {
        console.log("---------updateMontos-------------");
        console.log(response.data);
        this.props.unlockScreen();
      })
      .catch((error) => {
        this.props.unlockScreen();
      
        console.log(error);
      });
  };

  ActualizarReservas= () => {
    this.props.blockScreen();
    axios
      .post("/Proveedor/ReporteConsumo/ActualizarReservas", {})
      .then((response) => {
        console.log("---------ActualizarReservas-------------");
        console.log(response.data);
        this.props.unlockScreen();
      })
      .catch((error) => {
        this.props.unlockScreen();
      
        console.log(error);
      });
  };
  isValid = () => {
    console.log(this.state.dirigido_a.length);
    const errors = {};

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };
  GetList = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerListTransmital", {})
      .then(response => {
        console.log(response.data);
        this.setState({ data: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetListByContrato = Id => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerListByContrato/" + Id, {})
      .then(response => {
        console.log(response.data);
        this.setState({ data: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetOfertasByContrato = Id => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerOfertbyContrato/" + Id, {})
      .then(response => {
        console.log(response.data);
        var datos = response.data.map(item => {
          return {
            label: item.codigo + " - " + item.descripcion,
            dataKey: item.Id,
            value: item.Id
          };
        });
        this.setState({ ofertascomerciales: datos });
        this.state.ofertascomerciales.unshift({
          label: "Seleccione..",
          dataKey: 0,
          value: null
        });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetCatalogs = () => {
    axios
      .post("/Proyecto/Contrato/GetContratosApi", {})
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.Codigo, dataKey: item.Id, value: item.Id };
        });

        this.setState({ contratos: items });
      })
      .catch(error => {
        console.log(error);
      });
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerTransmitalUsers", {})
      .then(response => {
        console.log(response.data);
        var datos = response.data.map(item => {
          return {
            label:
              item.apellidos +
              " " +
              item.nombres +
              " ( " +
              item.nombre_cliente +
              " )",
            dataKey: item.Id,
            value: item.Id
          };
        });

        this.setState({ colaboradores: datos });
      })
      .catch(error => {
        console.log(error);
      });
  };

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };

  onChangeValueContrato = (name, value) => {
    this.setState({
      [name]: value
    });
    this.GetListByContrato(value);
    this.GetOfertasByContrato(value);
  };
  Eliminar = Id => {
    console.log(Id);
    axios
      .post("/Proyecto/TransmitalCabecera/ObtenerDeleteTransmital/", {
        Id: Id
      })
      .then(response => {
        if (response.data == "OK") {
          this.props.showSuccess("Eliminado Correctamente");
          this.GetList();
        } else if (response.data == "NOPUEDE") {
          this.props.showWarn("No se puedo Eliminar");
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un Error");
      });
  };

  mostrarForm = row => {
    if (row != null && row.Id > 0) {
    } else {
      this.setState({
        Id: 0,
        desde: 0,
        hasta: 0,
        action: "create",
        visible: true,
        editable: true
      });
    }
  };

  Redireccionar = id => {
    window.location.href = "/Proyecto/TransmitalCabecera/Details/" + id;
  };
  generarBotones = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-success"
          onClick={() => {
            this.Redireccionar(row.Id);
          }}
          style={{ marginRight: "0.3em" }}
        >
          Ver
        </button>
        {/*<button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.mostrarForm(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-edit" />
        </button>
        */}
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
          title="Eliminar"
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  };
  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={el => {
            this.growl = el;
          }}
          baseZIndex={1000}
        />{/*
        <div className="row">
          <div className="col">
            <Field
              name="list"
              label="LIST REQUERIMIENTOS //// ADC001,ADC002"
              required
              edit={true}
              readOnly={false}
              value={this.state.list}
              onChange={this.handleChange}
              error={this.state.errors.list}
            />
          </div>
        </div>
        <div className="row">
          <div className="col">
            {" "}
            <Field
              name="desde"
              label="Id Desde"
              edit={true}
              readOnly={false}
              value={this.state.desde}
              onChange={this.handleChange}
              error={this.state.errors.desde}
            />
          </div>
          <div className="col">
            {" "}
            <Field
              name="hasta"
              label="Hasta"
              edit={true}
              readOnly={false}
              value={this.state.hasta}
              onChange={this.handleChange}
              error={this.state.errors.hasta}
            />
          </div>
        </div>
        <div className="row">
          <button
            className="btn btn-outline-info"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.CargarOfertas()}
            data-toggle="tooltip"
            data-placement="top"
            title="Cargar Ofertas"
            disabled
          >
            Migrar Ofertas
            <i className="fa fa-edit" />
          </button>
          <button
            className="btn btn-outline-info"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.CargarTransmital()}
            data-toggle="tooltip"
            data-placement="top"
            title="Cargar Transmittals"
            disabled
          >
            Migrar Transmittals
            <i className="fa fa-edit" />
          </button>
          <button
            className="btn btn-outline-info"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.CargarRequerimientos()}
            data-toggle="tooltip"
            data-placement="top"
            title="Cargar Requerimietnos"
            disabled
          >
            Migrar Requerimientos
            <i className="fa fa-edit" />
          </button>
          <button
            className="btn btn-outline-info"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.CargarRelaciones()}
            data-toggle="tooltip"
            data-placement="top"
            title="Cargar Relaciones"
            disabled
          >
            Migrar Relaciones
            <i className="fa fa-edit" />
          </button>
          <button
            className="btn btn-outline-info"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.CargarOS()}
            data-toggle="tooltip"
            data-placement="top"
            title="Cargar OS"
            disabled
          >
            Migrar OS
            <i className="fa fa-edit" />
          </button>
          <button
            className="btn btn-outline-info"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.CargarCartas()}
            data-toggle="tooltip"
            data-placement="top"
            title="Cargar cartas"
            disabled
          >
            Migrar Cartas
            <i className="fa fa-edit" />
          </button>
          <button
            className="btn btn-outline-info"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.CargarRelacionesEstado()}
            data-toggle="tooltip"
            data-placement="top"
            title="Cargar cartas"
            disabled
          >
            Migrar Estados
            <i className="fa fa-edit" />
          </button>
          <button
            className="btn btn-outline-info"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.CargarActualizacionMontos()}
            data-toggle="tooltip"
            data-placement="top"
            disabled
            title="Cargar Montos Actualizados"
          >
            CArgar Actualizaciones Montos Requerimientos
            <i className="fa fa-edit" />
          </button>

          <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={() => this.UpdateMontos()}
              disabled
            >
              UpdateMontos
            </button>

            
        </div>
        <Dialog
          header="Nuevo"
          visible={this.state.visible}
          onHide={this.OcultarFormulario}
          modal={true}
          style={{ width: "900px", overflow: "auto" }}
        >
          <div>
            <form onSubmit={this.EnviarFormulario}>
              <div className="row">
                <div className="col">
                  <Field
                    name="OfertaComercialId"
                    value={this.state.OfertaComercialId}
                    label="Oferta Comercial"
                    options={this.state.ofertascomerciales}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.OfertaComercialId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />
                </div>
              </div>
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
                    name="descripcion"
                    label="Descripción"
                    required
                    edit={true}
                    readOnly={false}
                    value={this.state.descripcion}
                    onChange={this.handleChange}
                    error={this.state.errors.nombres}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <label htmlFor="label">Dirigido a</label>
                  <br />
                  <MultiSelect
                    value={this.state.dirigido_a}
                    options={this.state.colaboradores}
                    onChange={e => this.setState({ dirigido_a: e.value })}
                    style={{ width: "100%" }}
                    defaultLabel="Seleccione.."
                    filter={true}
                    placeholder="Seleccione"
                  />{" "}
                </div>
                <div className="col">
                  <label htmlFor="label">Con Copia a</label>
                  <br />
                  <MultiSelect
                    value={this.state.copia_a}
                    options={this.state.colaboradores}
                    onChange={e => this.setState({ copia_a: e.value })}
                    style={{ width: "100%" }}
                    defaultLabel="Seleccione.."
                    filter={true}
                    placeholder="Seleccione"
                  />
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <Field
                    name="fecha_emision"
                    label="Fecha Emisión"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.fecha_emision}
                    onChange={this.handleChange}
                    error={this.state.errors.fecha_emision}
                  />
                </div>
                <div className="col"></div>
              </div>
              <button type="submit" className="btn btn-outline-primary">
                Guardar
              </button>
              &nbsp;
              <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={this.OcultarFormulario}
              >
                Cancelar
              </button>
            </form>
          </div>
        </Dialog>*/}
        <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={() => this.ActualizarReservas()}
              
            >
              Actualizar Campos Nuevos Reservas Hoteles
            </button>
      </BlockUi>
    );
  }

  onDownloaImagen = Id => {
    return (window.location = `/Proyecto/AvanceObra/Descargar/${Id}`);
  };
  CargarOfertas = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/Tempoferta/CargarOfertas2Async", {
        desde: this.state.desde,
        hasta: this.state.hasta
      })
      .then(response => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Ofertas Cargadas Correctamente");
          this.props.unlockScreen();
        } else {
          this.props.showWarn(response.data);
          this.props.unlockScreen();
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn(error.toString());
      });
  };
  CargarTransmital = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TempRequerimiento/CargarTransmital", {
        desde: this.state.desde,
        hasta: this.state.hasta
      })
      .then(response => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Transmittals Cargadas Correctamente");
          this.props.unlockScreen();
        } else {
          this.props.showWarn(response.data);
          this.props.unlockScreen();
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn(error.toString());
      });
  };
  CargarCartas = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TempRequerimiento/CargaCartasOsAsync", {
        desde: this.state.desde,
        hasta: this.state.hasta
      })
      .then(response => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Cartas Cargadas Correctamente");
          this.props.unlockScreen();
        } else {
          this.props.showWarn(response.data);
          this.props.unlockScreen();
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn(error.toString());
      });
  };
  CargarRequerimientos = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TempRequerimiento/CargarRequerimientosAsync", {
        desde: this.state.desde,
        hasta: this.state.hasta
      })
      .then(response => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Requerimientos Cargadas Correctamente");
          this.props.unlockScreen();
        } else {
          this.props.showWarn(response.data);
          this.props.unlockScreen();
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn(error.toString());
      });
  };
  CargarOS = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TempRequerimiento/CargassOsAsync", {
        desde: this.state.desde,
        hasta: this.state.hasta
      })
      .then(response => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("OS Cargadas Correctamente");
          this.props.unlockScreen();
        } else {
          this.props.showWarn(response.data);
          this.props.unlockScreen();
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn(error.toString());
      });
  };
  CargarRelacionesEstado = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TempRequerimiento/CargarEstado", {
        desde: this.state.desde,
        hasta: this.state.hasta
      })
      .then(response => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Estado Requerimiento Cargadas Correctamente");
          this.props.unlockScreen();
        } else {
          this.props.showWarn(response.data);
          this.props.unlockScreen();
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn(error.toString());
      });
  };
  CargarActualizacionMontos = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TempRequerimiento/CargarActualizaciones", {
        list: this.state.list
      })
      .then(response => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Requerimientos Actualizadoss Correctamente");
          this.props.unlockScreen();
        } else {
          this.props.showWarn(response.data);
          this.props.unlockScreen();
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn(error.toString());
      });
  };
  CargarRelaciones = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/TempRequerimiento/CargarOfertaComercialPresupuesto", {
        desde: this.state.desde,
        hasta: this.state.hasta
      })
      .then(response => {
        console.log(response.data);
        if (response.data == "OK") {
          this.props.showSuccess("Migraciones Cargadas Correctamente");
          this.props.unlockScreen();
        } else {
          this.props.showWarn(response.data);
          this.props.unlockScreen();
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn(error.toString());
      });
  };
  EnviarFormulario(event) {
    event.preventDefault();
  }

  MostrarFormulario() {
    this.setState({ visible: true });
  }

  handleChange(event) {
    if (event.target.files) {
      console.log(event.target.files);
      let files = event.target.files || event.dataTransfer.files;
      if (files.length > 0) {
        let uploadFile = files[0];
        this.setState({
          uploadFile: uploadFile
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
const Container = wrapForm(MigracionContratos);
ReactDOM.render(<Container />, document.getElementById("content"));
