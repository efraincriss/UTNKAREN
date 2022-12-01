import React, { Fragment } from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../../Base/BaseWrapper";
import { TIPO_ACCION, TIPO_DEPARTAMENTO } from "../../../Base/Constantes";
import http from "../../../Base/HttpService";
import { Card } from "primereact-v2/card";
import Field from "../../../Base/Field-v2";
import axios from "axios";

class AlertaVencimientoContainer extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      accionesList: [],
      departamentosList: [],

      form: {
        ApellidosNombres: '',
        DepartamentoId: '',
        DiasVencimiento: '',
        Identificacion: '',
        RequisitosId: [],
      },
      errors: {},
    }

    this.handleChange = this.handleChange.bind(this);
    this.onDropdownChangeValue = this.onDropdownChangeValue.bind(this);
  }

  componentDidMount() {
    this.consultarDatos();
  }

  render() {
    return (
      <Fragment>
        <Card>
          <div className="row">
            <div className="col-4" />

            <div className="col-8" align="right">
              <button
                className="btn btn-outline-primary btn-sm"
                style={{ marginLeft: "0.3em" }}
                data-toggle="tooltip"
                data-placement="top"
                title=" Buscar "
                onClick={() => this.buscar()}
              >
                <i className="fa fa-cloud-download" />{" "}
                Descargar Excel
              </button>
              <button
                className="btn btn-outline-primary btn-sm"
                style={{ marginLeft: "0.3em" }}
                data-toggle="tooltip"
                data-placement="top"
                title=" Buscar "
                onClick={() => this.vaciarCampos()}
              >
                <i className="fa fa-refresh" />{" "} Vaciar Campos
              </button>

            </div>
          </div>

          <div className="row">
            <div className="col-6">
              <Field
                name="Identificacion"
                label="No. de Identificación"
                edit={true}
                readOnly={false}
                value={this.state.form.Identificacion}
                onChange={this.handleChange}
                error={this.state.errors.Identificacion}
              />
            </div>
            <div className="col-6">
              <Field
                name="ApellidosNombres"
                label="Apellidos Nombres"
                edit={true}
                readOnly={false}
                value={this.state.form.ApellidosNombres}
                onChange={this.handleChange}
                error={this.state.errors.ApellidosNombres}
              />
            </div>
          </div>

          <div className="row">
            <div className="col-6">
              <Field
                name="DiasVencimiento"
                label="Dias Vencimiento"
                type="number"
                edit={true}
                readOnly={false}
                value={this.state.form.DiasVencimiento}
                onChange={this.handleChange}
                error={this.state.errors.DiasVencimiento}
              />
            </div>
            <div className="col-6">
              <Field
                name="DepartamentoId"
                value={this.state.form.DepartamentoId}
                label="Departamento"
                options={this.state.departamentosList}
                type={"select"}
                onChange={this.onDropdownChangeValue}
                error={this.state.errors.DepartamentoId}
                readOnly={false}
                placeholder="Seleccione.."
                filterPlaceholder="Seleccione.."
                filter={true}
              />
            </div>
          </div>

          <div className="row">
            <div className="col">
              <Field
                name="RequisitosId"
                value={this.state.form.RequisitosId}
                label="Requisitos"
                options={this.state.requisitosList}
                type={"MULTI-SELECT"}
                onChange={this.onDropdownChangeValue}
                error={this.state.errors.RequisitosId}
                readOnly={false}
                placeholder="Seleccione.."
                fixedPlaceholder="Seleccione.."
                filter={true}
              />
            </div>
          </div>



        </Card>
      </Fragment>
    );
  }

  buscar = () => {
    const isValid = this.validarFiltrosObligatorios();

    if (isValid) {
      this.props.blockScreen();
      axios.post(`/Accesos/AlertaVencimientos/ObtenerVencimientos`, this.state.form, { responseType: 'arraybuffer' })
        .then(response => {
          console.log(response)
          var nombre = response.headers["content-disposition"].split('=');

          const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
          const link = document.createElement('a');
          link.href = url;
          link.setAttribute('download', nombre[1]);
          document.body.appendChild(link);
          link.click();
          this.props.showSuccess("Excel Alerta de  Vencimientos Descargado");
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.showWarn("Ocurrió un Error al descargar el Excel")
          this.props.unlockScreen();
        });
    }
  }

  vaciarCampos = () => {
    this.setState({
      form: {
        ApellidosNombres: '',
        DepartamentoId: '',
        DiasVencimiento: '',
        Identificacion: '',
        RequisitosId: [],
      }
    })
  }

  consultarDatos = () => {
    this.props.blockScreen();
    var self = this;
    Promise.all([this.obtenerTipoAccion(), this.obtenerDepartamentos(), this.obtenerRequisitos()])
      .then(function ([acciones, departamentos, requisitos]) {

        var accionesModel = acciones.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });

        var departamentosModel = departamentos.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });

        var requisitosModel = requisitos.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });

        self.setState({
          accionesList: accionesModel,
          departamentosList: departamentosModel,
          requisitosList: requisitosModel
        }, self.props.unlockScreen)
      })
      .catch((error) => {
        self.props.unlockScreen();
        console.log(error);
      });
  }

  validarFiltrosObligatorios = () => {
    let isValid = true;
    let errors = {};
    const { DiasVencimiento } = this.state.form;
    if (DiasVencimiento === '') {
      errors = {
        ...errors,
        DiasVencimiento: 'Días de vencimiento es requerido'
      }
      isValid = false;
    }
    this.setState({ errors })
    return isValid;
  }

  handleChange(event) {
    const form = {
      ...this.state.form,
      [event.target.name]: event.target.value
    }
    this.setState({ form });
  }

  onDropdownChangeValue = (name, value) => {
    const form = {
      ...this.state.form,
      [name]: value
    }
    this.setState({
      form
    });
  };

  obtenerTipoAccion() {
    return http.get(`/Accesos/ValidacionRequisito/FilterCatalogo/?code=${TIPO_ACCION}`)
  }

  obtenerDepartamentos() {
    return http.get(`/Accesos/ValidacionRequisito/FilterCatalogo/?code=${TIPO_DEPARTAMENTO}`)
  }

  obtenerRequisitos() {
    return http.get(`/Accesos/AlertaVencimientos/ObtenerRequisitos`)
  }

}


const Container = Wrapper(AlertaVencimientoContainer);
ReactDOM.render(
  <Container />,
  document.getElementById('alerta_vencimiento_container')
);