import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import Field from "../../Base/Field-v2";
import wrapForm from "../../Base/BaseWrapper";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import validationRules from '../../Base/validationRules';
import moment from 'moment';
import http from "../../Base/HttpService";
import { Checkbox } from 'primereact-v2/checkbox';
import { MODULO_TRANSPORTE, CONTROLLER_RUTA } from "../../Base/Strings";
import { Card, CardBody, CardTitle, CardSubtitle, Button, Modal, ModalBody, ModalHeader, FormCheckbox } from "shards-react";

class Tranporte extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: false,
      data: [],
      visible_first: false,
      visible_second: false,
      visible_third: false,
      errors: {},
      ProveedorId: 0,
      FechaDesde: null,
      FechaHasta: null,
      Fecha: null,
      VehiculoId: 0,
      Vehiculos: [],
      Proveedores: [],
      Rutas: [],
      RutaId: 0,
      checked: true,
      checked2: true,
      checked3: true,

      // Datos del Formulario de Retiro Viandas
      dataRetiroTransportista: {
        check: true,
        proveedorId: 0,
        tipoComidaId: 0,
        fecha: '',
      },
      // Show/Hide formulario de Retiro Viandas
      openModal: false,
      // Guarda los transportistas
      transportistas: [],
      // Guarda el Catalogo de tipos comidas viandas
      tiposComidasViandas: [],


    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormularioNombres = this.EnviarFormularioNombres.bind(this);
    this.EnviarFormulario_two = this.EnviarFormulario_two.bind(this);
    this.EnviarFormulario_three = this.EnviarFormulario_three.bind(this);
    this.ObtenerDatos = this.ObtenerDatos.bind(this);
    this.ObtenerRutas = this.ObtenerRutas.bind(this);
    this.ObtenerVehiculo = this.ObtenerVehiculo.bind(this);
  }

  componentDidMount() {

    this.ObtenerDatos();

  }

  isValid() {
    const errors = {};
    if (this.state.FechaDesde == null) {
      errors.FechaDesde = "Campo Requerido";
    }
    if (this.state.FechaHasta == null) {
      errors.FechaHasta = "Campo Requerido";
    }

    if (this.state.FechaHasta != null && this.state.FechaDesde != null && this.state.FechaHasta < this.state.FechaDesde) {
      errors.FechaHasta = "Fecha Hasta no puede se menor a Fecha Desde";
    }
    if (!this.state.checked) {
      if (this.state.ProveedorId == 0) {
        errors.ProveedorId = "Campo Requerido";
      }
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }


  isValidTwo() {
    const errors = {};
    if (this.state.FechaDesde == null) {
      errors.FechaDesde = "Campo Requerido";
    }
    if (this.state.FechaHasta == null) {
      errors.FechaHasta = "Campo Requerido";
    }
    if (this.state.FechaHasta != null && this.state.FechaDesde != null && this.state.FechaHasta < this.state.FechaDesde) {
      errors.FechaHasta = "Fecha Hasta no puede se menor a Fecha Desde";
    }
    if (!this.state.checked2) {
      if (this.state.ProveedorId == 0) {
        errors.ProveedorId = "Campo Requerido";
      }
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }

  isValidThree() {
    const errors = {};
    if (this.state.Fecha == null) {
      errors.Fecha = "Campo Requerido";
    }
    if (!this.state.checked3) {
      if (this.state.ProveedorId == 0) {
        errors.ProveedorId = "Campo Requerido";
      }
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }
  VaciarCampos() {
    this.setState({
      ProveedorId: 0,
      FechaDesde: null,
      FechaHasta: null,
      Fecha: null,
      VehiculoId: 0,
      Vehiculos: [],
      Rutas: [],
      RutaId: 0,
      errors: {}
    });
  }

  ObtenerDatos() {
    axios
      .post("/Transporte/Ruta/ObtenerProveedoresTransporte", {})
      .then(response => {
        console.log(response.data.result);
        var items = response.data.result.map(item => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Proveedores: items });
        this.props.unlockScreen()
      })
      .catch(error => {
        console.log(error);
        abp.notify.error(
          "Ocurrió un Error al Consultar Proveedores de Transporte",
          "Consulta"
        );
        this.props.unlockScreen()
      });
  }

  ObtenerRutas(Id) {
    axios
      .post("/Transporte/Ruta/ObtenerRutasProveedor", {
        Id: Id
      })
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.Nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Rutas: items });
        this.props.unlockScreen()
      })
      .catch(error => {
        console.log(error);
        abp.notify.error(
          "Ocurrió un Error al Consultar Rutas del Proveedor",
          "Consulta"
        );
        this.props.unlockScreen()
      });
  }
  ObtenerVehiculo(Id) {
    axios
      .post("/Transporte/Ruta/ObtenerVehiculosProveedor", {
        Id: Id
      })
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.CodigoEquipoInventario + ' - ' + item.NumeroPlaca, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Vehiculos: items });
        this.props.unlockScreen()
      })
      .catch(error => {
        console.log(error);
        abp.notify.error(
          "Ocurrió un Error al Consultar Vehiculos del Proveedor",
          "Consulta"
        );
        this.props.unlockScreen()
      });
  }



  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };
  onChangeValueProveedor = (name, value) => {
    console.log(value);
    this.setState({
      [name]: value
    });
    if (value > 0) {
      this.ObtenerRutas(value);
      this.ObtenerVehiculo(value);
    }


  };


  // ================== Retiro Transportistas ====================== //

  ObtenerTransportistas = () => {
    let url = '';
    url = `/${MODULO_TRANSPORTE}/${CONTROLLER_RUTA}/ObtenerTransportistas`;
    return http.get(url);
  }

  ObtenerTiposComidasViandas = () => {
    let url = '';
    url = `/${MODULO_TRANSPORTE}/${CONTROLLER_RUTA}/ObtenerTiposComidasViandas`;
    return http.get(url);
  }


  loadData = () => {
    this.props.blockScreen();
    var self = this;
    Promise.all([this.ObtenerTransportistas(), this.ObtenerTiposComidasViandas()])
      .then(function ([transportistas, tiposComidasViandas]) {
        let proveedoresMapped = self.buildDropdown(transportistas.data.result, 'nombres_completos', 'Id');
        let tiposComidasMapped = self.buildDropdown(tiposComidasViandas.data.result, 'nombre', 'Id');
        self.setState({
          transportistas: proveedoresMapped,
          tiposComidasViandas: tiposComidasMapped
        }, self.props.unlockScreen)
      })
      .catch((error) => {
        self.props.unlockScreen();
        console.log(error);
      });
  }

  buildDropdown = (data, nameField = 'name', valueField = 'Id') => {
    if (data.success === true) {
      return data.result.map(i => {
        return { label: i[nameField], value: i[valueField] }
      });
    } else if (data !== undefined) {
      return data.map(i => {
        return { label: i[nameField], value: i[valueField] }
      });
    }
    return {};
  }

  toggle = () => {
    if (!this.state.openModal) {
      if (this.state.transportistas.length === 0 || this.state.tiposComidasViandas.length === 0) {
        console.log("Consultando");
        this.loadData();
      }
    }
    this.setState({
      openModal: !this.state.openModal
    });
  }

  isValidRetirosTransportista = () => {
    const errors = {};
    if (this.state.dataRetiroTransportista.fecha === '') {
      errors.fecha = "Campo Requerido";
    }
    if (this.state.dataRetiroTransportista.tipoComidaId == 0) {
      errors.tipoComidaId = "Campo Requerido";
    }
    if (!this.state.dataRetiroTransportista.check && this.state.dataRetiroTransportista.proveedorId === 0) {
      errors.proveedorId = "Campo Requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }

  handleSubmit = (event) => {
    event.preventDefault();

    if (!this.isValidRetirosTransportista()) {
      return;
    }
    this.props.blockScreen();

    console.log('Consultando');

    let url = `/${MODULO_TRANSPORTE}/${CONTROLLER_RUTA}/ObtenerReporteRetiroViandas`;
    http.get(url, {
      params: {
        fecha: this.state.dataRetiroTransportista.fecha,


        check: this.state.dataRetiroTransportista.check,


        conductor_asignado_id: this.state.dataRetiroTransportista.proveedorId,
        tipoComidaId: this.state.dataRetiroTransportista.tipoComidaId

      },
      responseType: 'arraybuffer',
    })
      .then((response) => {
        console.log(response)
        var nombre = response.headers["content-disposition"].split('=');

        const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', nombre[1]);
        document.body.appendChild(link);
        link.click();
        this.props.showSuccess("Reporte Generado Correctamente");
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Ocurrió un Error al descargar el Excel")
        this.props.unlockScreen();
      });

  }

  onChange = (event) => {
    const target = event.target;
    const value = target.type === "checkbox" ? target.checked : target.value;
    const name = target.name;
    const updatedData = {
      ...this.state.dataRetiroTransportista
    };
    updatedData[name] = value;
    this.setState({
      dataRetiroTransportista: updatedData,
    });

  }

  onChangeVal = (name, value) => {
    const updatedData = {
      ...this.state.dataRetiroTransportista
    };
    updatedData[name] = value;

    this.setState({
      dataRetiroTransportista: updatedData,
    });
  };

  handleChangeChacked = () => {
    let check = this.state.dataRetiroTransportista.check;

    const updatedData = {
      ...this.state.dataRetiroTransportista
    };
    updatedData["check"] = !check;
    updatedData["proveedorId"] = 0;

    this.setState({
      dataRetiroTransportista: updatedData,
    });
  }


  // =============================================================== //

  render() {
    const { check } = this.state.dataRetiroTransportista;
    return (
      <div>
        <div>
          <table className="table table-striped">
            <thead>
              <tr>
                <th scope="col">Nº</th>
                <th scope="col">Descripción</th>
                <th scope="col"> </th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <th scope="row">1</th>
                <td>Reporte Diario de Personas Transportadas
                </td>

                <td>  <button
                  type="button"
                  className="btn btn-outline-primary"
                  icon="fa fa-fw fa-ban"
                  onClick={() => this.MostrarFormulario(1)}
                ><i className="fa fa-vcard">{" "}Nuevo</i>
                </button></td>
              </tr>
              
              <tr>
                <th scope="row">2</th>
                <td>Reporte Diario de Viajes por Proveedor y Vehículo</td>
                <td>  <button
                  type="button"
                  className="btn btn-outline-primary"
                  icon="fa fa-fw fa-ban"
                  onClick={() => this.MostrarFormulario(2)}
                > <i className="fa fa-vcard">{" "}Nuevo</i>

                </button></td>

              </tr>
              <tr>
                <th scope="row">3</th>
                <td>Reporte Diario de Trabajo por Proveedor y Vehículo
                </td>
                <td>  <button
                  type="button"
                  className="btn btn-outline-primary"
                  icon="fa fa-fw fa-ban"
                  onClick={() => this.MostrarFormulario(3)}
                ><i className="fa fa-vcard">{" "}Nuevo</i>
                </button></td>
              </tr>

              <tr>
                <th scope="row">4</th>
                <td>Reporte Retiro de Viandas por Transportista</td>
                <td>  <button
                  type="button"
                  className="btn btn-outline-primary"
                  icon="fa fa-fw fa-ban"
                  onClick={() => this.toggle()}
                ><i className="fa fa-vcard">{" "}Nuevo</i>
                </button></td>

              </tr>
            </tbody>
          </table>

        </div>

        <Modal size="lg" open={this.state.openModal} toggle={this.toggle} centered={true}>
          <ModalHeader>Reporte de Transporte de Viandas</ModalHeader>
          <ModalBody>
            <div>
              <form onSubmit={this.handleSubmit}>
                <div className="row">
                  <div className="col">
                    <Field
                      name="fecha"
                      label="Fecha Reporte"
                      required
                      type="date"
                      edit={true}
                      readOnly={false}
                      value={this.state.dataRetiroTransportista.fecha}
                      onChange={this.onChange}
                      error={this.state.errors.fecha}
                    />
                  </div>

                  <div className="col">
                    <Field
                      name="tipoComidaId"
                      required
                      value={this.state.dataRetiroTransportista.tipoComidaId}
                      label="Tipo Comida"
                      options={this.state.tiposComidasViandas}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeVal}
                      error={this.state.errors.tipoComidaId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                  </div>
                </div>

                <div className="row" style={{ marginTop: '1px' }}>
                  <div className="col">
                    {
                      !check &&
                      <Field
                        name="proveedorId"
                        required
                        value={this.state.dataRetiroTransportista.proveedorId}
                        label="Transportista"
                        options={this.state.transportistas}
                        type={"select"}
                        filter={true}
                        onChange={this.onChangeVal}
                        error={this.state.errors.proveedorId}
                        readOnly={false}
                        placeholder="Seleccione.."
                        filterPlaceholder="Seleccione.."
                      />
                    }
                  </div>
                </div>

                <div>
                  <FormCheckbox
                    toggle
                    small
                    checked={check}
                    onChange={this.handleChangeChacked}>
                    🚀 (Todos los Transportistas)
                  </FormCheckbox>
                </div>
                <button type="submit" className="btn btn-outline-primary" style={{ marginTop: '1px' }}>
                  Generar
                </button>
                &nbsp;
              <button
                  type="button"
                  className="btn btn-outline-primary"
                  style={{ marginTop: '1px' }}
                  icon="fa fa-fw fa-ban"
                  onClick={this.OcultarFormulario}
                >
                  Cancelar
              </button>
              </form>
            </div>
          </ModalBody>
        </Modal>

        <Dialog
          header="Reporte Diario de Personas Transportadas"
          visible={this.state.visible_first}
          onHide={this.OcultarFormulario}
          modal={true}
          width="720px"
        >
          <div>
            <form onSubmit={this.EnviarFormulario}>
              <div className="row">
                <div className="col">
                  <Field
                    name="FechaDesde"
                    label="Fecha Desde"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.FechaDesde}
                    onChange={this.handleChange}
                    error={this.state.errors.FechaDesde}
                  />


                </div>
                <div className="col">
                  <Field
                    name="FechaHasta"
                    label="Fecha Hasta"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.FechaHasta}
                    onChange={this.handleChange}
                    error={this.state.errors.FechaHasta}
                  />

                </div>

              </div>
              {!this.state.checked &&
                <div className="row">
                  <div className="col">
                    <Field
                      name="ProveedorId"
                      required
                      value={this.state.ProveedorId}
                      label="Proveedor"
                      options={this.state.Proveedores}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValueProveedor}
                      error={this.state.errors.ProveedorId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                  </div>
                  <div className="col">
                    <Field
                      name="RutaId"
                      value={this.state.RutaId}
                      label="Rutas"
                      options={this.state.Rutas}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValue}
                      error={this.state.errors.RutaId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                  </div>
                </div>
              }
              <div className="row">
                <div className="col">
                  <Checkbox checked={this.state.checked} onChange={e => this.setState({ checked: e.checked })} /> (Todos los Proveedores)

                </div>
              </div>
              <br />
              <button type="submit" className="btn btn-outline-primary">
                Generar
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
        </Dialog>
        <Dialog
          header="Reporte Diario de Viajes por Proveedor y Vehículo"
          visible={this.state.visible_second}
          onHide={this.OcultarFormulario}
          modal={true}
          width="720px"
        >
          <div>
            <form onSubmit={this.EnviarFormulario_two}>
              <div className="row">
                <div className="col">
                  <Field
                    name="FechaDesde"
                    label="Fecha Desde"

                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.FechaDesde}
                    onChange={this.handleChange}
                    error={this.state.errors.FechaDesde}
                  />


                </div>
                <div className="col">
                  <Field
                    name="FechaHasta"
                    label="Fecha Hasta"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.FechaHasta}
                    onChange={this.handleChange}
                    error={this.state.errors.FechaHasta}
                  />

                </div>
              </div>
              {!this.state.checked2 &&
                <div className="row">
                  <div className="col">
                    <Field
                      name="ProveedorId"
                      required
                      value={this.state.ProveedorId}
                      label="Proveedor"
                      options={this.state.Proveedores}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValueProveedor}
                      error={this.state.errors.ProveedorId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                  </div>
                  <div className="col">
                    <Field
                      name="VehiculoId"

                      value={this.state.VehiculoId}
                      label="Vehiculos"
                      options={this.state.Vehiculos}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValue}
                      error={this.state.errors.VehiculoId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                  </div>
                </div>
              }
              <div className="row">
                <div className="col">
                  <Checkbox checked={this.state.checked2} onChange={e => this.setState({ checked: e.checked2 })} /> (Todos los Proveedores)

                </div>
              </div>
              <br />
              <button type="submit" className="btn btn-outline-primary">
                Generar
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
        </Dialog>
        <Dialog
          header="Reporte Diario de Trabajo por Proveedor y Vehículo"
          visible={this.state.visible_third}
          onHide={this.OcultarFormulario}
          modal={true}
          width="720px"
        >
          <div>
            <form onSubmit={this.EnviarFormulario_three}>
              <div className="row">
                <div className="col">
                  <Field
                    name="Fecha"
                    label="Fecha"
                    required
                    type="date"
                    edit={true}
                    readOnly={false}
                    value={this.state.Fecha}
                    onChange={this.handleChange}
                    error={this.state.errors.Fecha}
                  />

                </div>
              </div>

              {!this.state.checked3 &&
                <div className="row">
                  <div className="col">
                    <Field
                      name="ProveedorId"
                      required
                      value={this.state.ProveedorId}
                      label="Proveedor"
                      options={this.state.Proveedores}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValueProveedor}
                      error={this.state.errors.ProveedorId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />

                  </div>
                  <div className="col">

                    <Field
                      name="VehiculoId"

                      value={this.state.VehiculoId}
                      label="Vehiculos"
                      options={this.state.Vehiculos}
                      type={"select"}
                      filter={true}
                      onChange={this.onChangeValue}
                      error={this.state.errors.VehiculoId}
                      readOnly={false}
                      placeholder="Seleccione.."
                      filterPlaceholder="Seleccione.."
                    />
                  </div>
                </div>
              }
              <div className="row">
                <div className="col">
                  <Checkbox checked={this.state.checked3} onChange={e => this.setState({ checked: e.checked3 })} /> (Todos los Proveedores)

                </div>
              </div>
              <br />
              <button type="submit" className="btn btn-outline-primary">
                Generar
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
        </Dialog>

      </div>
    )
  }

  EnviarFormulario(event) {
    event.preventDefault();

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      this.props.blockScreen();

      axios.get("/Transporte/Ruta/ObtenerReportesPersonasTransportadas", {
        params: {
          ProveedorId: this.state.ProveedorId,
          RutaId: this.state.RutaId,
          FechaDesde: this.state.FechaDesde,
          FechaHasta: this.state.FechaHasta,
          VehiculoId: this.state.VehiculoId,
          Fecha: this.state.Fecha,
          check:this.state.checked

        },
        responseType: 'arraybuffer',
      })
        .then((response) => {
          console.log(response)
          var nombre = response.headers["content-disposition"].split('=');

          const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
          const link = document.createElement('a');
          link.href = url;
          link.setAttribute('download', nombre[1]);
          document.body.appendChild(link);
          link.click();
          this.props.showSuccess("Reporte Descargado Correctamente");
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.showWarn("Ocurrió un Error al descargar el Excel")
          this.props.unlockScreen();
        });

    }
  }
  EnviarFormularioNombres(event) {
    event.preventDefault();

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      this.props.blockScreen();

      axios.get("/Transporte/Ruta/ObtenerReportesPersonasTransportadas", {
        params: {
          ProveedorId: this.state.ProveedorId,
          RutaId: this.state.RutaId,
          FechaDesde: this.state.FechaDesde,
          FechaHasta: this.state.FechaHasta,
          VehiculoId: this.state.VehiculoId,
          Fecha: this.state.Fecha,
          check:this.state.checked
        },
        responseType: 'arraybuffer',
      })
        .then((response) => {
          console.log(response)
          var nombre = response.headers["content-disposition"].split('=');

          const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
          const link = document.createElement('a');
          link.href = url;
          link.setAttribute('download', nombre[1]);
          document.body.appendChild(link);
          link.click();
          this.props.showSuccess("Reporte Descargado Correctamente");
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.showWarn("Ocurrió un Error al descargar el Excel")
          this.props.unlockScreen();
        });

    }
  }
  EnviarFormulario_two(event) {
    event.preventDefault();

    if (!this.isValidTwo()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      this.props.blockScreen();
      axios.get("/Transporte/Ruta/ObtenerReportesDiarioViajes", {
        params: {
          ProveedorId: this.state.ProveedorId,
          RutaId: this.state.RutaId,
          FechaDesde: this.state.FechaDesde,
          FechaHasta: this.state.FechaHasta,
          VehiculoId: this.state.VehiculoId,
          Fecha: this.state.Fecha,
          check:this.state.checked2
        },
        responseType: 'arraybuffer',
      })
        .then((response) => {
          var nombre = response.headers["content-disposition"].split('=');

          const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
          const link = document.createElement('a');
          link.href = url;
          link.setAttribute('download', nombre[1]);
          document.body.appendChild(link);
          link.click();
          this.props.showSuccess("Reporte Descargado Correctamente");
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.unlockScreen();
          this.props.showWarn("Ocurrió un Error al descargar el Excel")
        });

    }
  }
  EnviarFormulario_three(event) {
    event.preventDefault();

    if (!this.isValidThree()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      return;
    } else {
      this.props.blockScreen();
      axios.get("/Transporte/Ruta/ObtenerReporteDiariodeTrabajo", {
        params: {
          ProveedorId: this.state.ProveedorId,
          RutaId: this.state.RutaId,
          FechaDesde: this.state.FechaDesde,
          FechaHasta: this.state.FechaHasta,
          VehiculoId: this.state.VehiculoId,
          Fecha: this.state.Fecha,
          check:this.state.checked3
        },
        responseType: 'arraybuffer',
      })
        .then((response) => {
          var nombre = response.headers["content-disposition"].split('=');

          const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
          const link = document.createElement('a');
          link.href = url;
          link.setAttribute('download', nombre[1]);
          document.body.appendChild(link);
          link.click();
          this.props.showSuccess("Reporte Descargado Correctamente");
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.unlockScreen();
          this.props.showWarn("Ocurrió un Error al descargar el Excel")
        });

    }
  }




  MostrarFormulario(type) {
    if (type == 1) {
      this.setState({ visible_first: true });
    }
    if (type == 2) {
      this.setState({ visible_second: true });
    }
    if (type == 3) {
      this.setState({ visible_third: true });
    }
  }

  OcultarFormulario() {
    this.setState({ visible_first: false, visible_second: false, visible_third: false, openModal: false });
    this.VaciarCampos()
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

}
const Container = wrapForm(Tranporte);
ReactDOM.render(<Container />, document.getElementById("content"));
