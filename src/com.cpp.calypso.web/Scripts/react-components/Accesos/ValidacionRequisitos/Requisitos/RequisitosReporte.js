import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import Field from "../../../Base/Field-v2";
import Wrapper from "../../../Base/BaseWrapper";
import { Checkbox } from 'primereact-v2/checkbox';
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Card } from 'primereact-v2/card';
import {
  TIPO_REQUISITO, TIPO_ACCION, TIPO_DEPARTAMENTO
} from "../../../Base/Constantes";
class RequisitosReporte extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data: [],
      visible: false,
      visibleeditar: false,
      errors: {},
      colaborador: null,
      colaboradores: [],

      Identificacion: '',
      ApellidosNombres: '',
      DiasVencimiento: 0,
      //TipoRequisitos: [],
      //TipoRequisitoId: 0,
      TipoAcciones: [],
      TipoAccionId: 0,
      Departamentos: [],
      DepartamentoId: 0,

      Vencido: false,
      Obligatorio: true

    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.ObtenerCatalogos = this.ObtenerCatalogos.bind(this);
    this.generarBotones = this.generarBotones.bind(this);
    this.VaciarCampos = this.VaciarCampos.bind(this);
    this.Busqueda = this.Busqueda.bind(this);
    this.DescargarCumplimientoIndividual = this.DescargarCumplimientoIndividual.bind(this);
    this.DescargarListadoCumplimientos = this.DescargarListadoCumplimientos.bind(this);
    this.RenderizarColaborador = this.RenderizarColaborador.bind(this);
  }

  componentDidMount() {
    this.ObtenerCatalogos();

  }

  VaciarCampos() {
    this.props.blockScreen()
    this.setState({
      Identificacion: '',
      ApellidosNombres: '',
      DiasVencimiento: 0,
      // TipoRequisitoId: 0,
      TipoAccionId: 0,
      DepartamentoId: 0,
      Vencido: false,
      Obligatorio: true,
      colaborador: null,
      data: [],
      colaboradores: []
    });
    this.props.unlockScreen()
  }

  ObtenerCatalogos() {
    this.props.blockScreen()

    axios
      .get(`/Accesos/ValidacionRequisito/FilterCatalogo/?code=${TIPO_ACCION}`, {})
      .then(response => {
        var items = response.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ TipoAcciones: items });
        this.props.unlockScreen()
      })

      .catch(error => {
        console.log(error);
      });

    axios
      .get(`/Accesos/ValidacionRequisito/FilterCatalogo/?code=${TIPO_DEPARTAMENTO}`, {})
      .then(response => {
        var items = response.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Departamentos: items });
        this.props.unlockScreen()
      })
      .catch(error => {
        console.log(error);
      });
    /*axios
      .post("/Accesos/ValidacionRequisito/ObtenerTiposRequisitos", {})
      .then(response => {
        
        var items = response.data.map(item => {
          return { label: item.codigo + " - " + item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ TipoRequisitos: items });
        this.props.unlockScreen()
      })
      .catch(error => {
        console.log(error);
      });

 */
  }
  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };

  generarBotones = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormularioEditar(row)}
        >
          <i className="fa fa-edit" />
        </button>{" "}
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          onClick={() => {
            if (
              window.confirm(
                `Esta acción eliminará el registro, ¿Desea continuar?`
              )
            )
              this.EliminarRuta(row.Id);
          }}
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  }


  Busqueda() {

    if (this.state.TipoAccionId == 0) {

      this.props.showWarn("Debe seleccionar una acción");
    }
    else {



      this.props.blockScreen();
      this.setState({ data: [], colaborador: null });


      axios
        .get(`/Accesos/ValidacionRequisito/ObtenerColaborador?identificacion=${this.state.Identificacion}&nombres=${this.state.ApellidosNombres}`, {})
        .then(response => {
          if (response.data != null && response.data.length == 1) {
            this.setState({ colaborador: response.data[0] })

          }
          this.setState({ colaboradores: response.data });

          if (response.data != null && response.data.length > 0) {
            axios
              .post("/Accesos/ValidacionRequisito/ObtenerListaRequisitosporColaborador", {
                // ColaboradorId:response.data.Id,
                Colaboradores: response.data,
                AccionId: this.state.TipoAccionId,
                DepartamentoId: this.state.DepartamentoId,
                Vencidos: this.state.Vencido,
                Obligatorios: this.state.Obligatorio,
                DiasVencimiento: this.state.DiasVencimiento,
                NombreColaborador: "-",
                Identificacion: "-"
              })
              .then(response => {

                this.setState({ data: response.data });
                this.props.unlockScreen()

              })
              .catch(error => {
                console.log(error);
                this.props.unlockScreen()

              });

          } else {

            this.setState({ data: [], colaborador: null });
            this.props.unlockScreen()
          }


        })
        .catch(error => {
          console.log(error);
        });
    }
  }

  Secuencial(cell, row, enumObject, index) {
    return (<div>{index + 1}</div>)
  }
  RenderizarColaborador() {
    if (this.state.colaborador != null) {
      return <div>

        <Card className="ui-card-shadow">
          <div>
            <b><label>Información Colaborador:</label></b><br />

            <div className="row">

              <div className="col-xs-12 col-md-3">
                <h6 className="text-gray-700" style={{ fontSize: "12px" }}>
                  <b>Identificación:</b> <br />{this.state.colaborador != null ? this.state.colaborador.Identificacion : ""}
                </h6>

              </div>

              <div className="col-xs-12 col-md-3">
                <h6 className="text-gray-700" style={{ fontSize: "12px" }}>
                  <b>Nombres y Apellidos :</b><br /> {this.state.colaborador != null ? this.state.colaborador.NombresApellidos : ""}

                </h6>

              </div>
              <div className="col-xs-12 col-md-3">
                <h6 className="text-gray-700" style={{ fontSize: "12px" }}>
                  <b>Agrupación para Requisitos :</b> <br />{this.state.colaborador != null ? this.state.colaborador.GrupoPersonal : ""}

                </h6>

              </div>
              <div className="col-xs-12 col-md-3">
                <button
                  className="btn btn-outline-success btn-sm"
                  style={{ marginLeft: "0.3em" }}
                  data-toggle="tooltip"
                  data-placement="top"
                  title="Cumplimiento Individual"
                  onClick={() => this.DescargarCumplimientoIndividual()}
                >
                  <i className="fa fa-eye" />{" "}
                  Cumplimiento Individual
                  </button>
              </div>
            </div>
          </div>
        </Card>
      </div>
    }

  }

  DescargarCumplimientoIndividual() {
    this.props.blockScreen();

    if (this.state.colaborador != null && this.state.colaboradores != null) {

      axios.get("/Accesos/ValidacionRequisito/ObtenerCumplimientoIndividual", {
        params: {
          ColaboradorId: this.state.colaborador.Id,
          AccionId: this.state.TipoAccionId,
          DepartamentoId: this.state.DepartamentoId,
          Vencidos: this.state.Vencido,
          Obligatorios: this.state.Obligatorio,
          DiasVencimiento: this.state.DiasVencimiento,
          Colaboradores: JSON.stringify(this.state.colaboradores),
          NombreColaborador: this.state.colaborador != null ? this.state.colaborador.NombresApellidos : "_",
          Identificacion: this.state.colaborador != null ? this.state.colaborador.Identificacion : "_"
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
          this.props.showSuccess("Excel Cumplimiento Individual Descargada");
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.showWarn("Ocurrió un Error al descargar el Excel")
          this.props.unlockScreen();
        });
    } else {
      this.props.showWarn("Debe buscar un colaborador para esta acción")
      this.props.unlockScreen();

    }
  }
  DescargarListadoCumplimientos() {
    if (this.state.TipoAccionId == 0) {

      this.props.showWarn("Debe seleccionar una acción");
    }
    else {



      this.props.blockScreen();

      this.setState({ colaborador: null, Identificacion: "", ApellidosNombres: "" })
      axios
        .get(`/Accesos/ValidacionRequisito/ObtenerColaborador?identificacion=${this.state.Identificacion}&nombres=${this.state.ApellidosNombres}`, {})
        .then(response => {
          console.log(response.data)
          if (response.data != null) {
            console.log(response.data)

            axios.get("/Accesos/ValidacionRequisito/ObtenerListaCumplimientos", {
              params: {
                ColaboradorId: 0,
                //Colaboradores: JSON.stringify(response.data),
                AccionId: this.state.TipoAccionId,
                DepartamentoId: this.state.DepartamentoId,
                Vencidos: this.state.Vencido,
                Obligatorios: this.state.Obligatorio,
                DiasVencimiento: this.state.DiasVencimiento,
                NombreColaborador: "_",
                //Identificacion:""
              },
              responseType: 'arraybuffer'
            })
              .then((response) => {
                var nombre = response.headers["content-disposition"].split('=');

                const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', nombre[1]);
                document.body.appendChild(link);
                link.click();
                this.props.showSuccess("Excel Listado Cumplimientos Descargada");
                this.props.unlockScreen();
              })
              .catch((error) => {
                console.log(error);
                this.props.unlockScreen();

              });
          }


        })
        .catch(error => {
          console.log(error);
        });

    }
  }
  render() {

    return (
      <div>
        <div className="content-section implementation">
          <Card>
            <div>
              <div className="row">
                <div className="col-4" />

                <div className="col-8" align="right">
                  <button
                    className="btn btn-outline-primary btn-sm"
                    style={{ marginLeft: "0.3em" }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title=" Buscar "
                    onClick={() => this.Busqueda()}
                  >
                    <i className="fa fa-search" />{" "}
                    Buscar
                  </button>
                  <button
                    className="btn btn-outline-primary btn-sm"
                    style={{ marginLeft: "0.3em" }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title=" Buscar "
                    onClick={() => this.VaciarCampos()}
                  >
                    <i className="fa fa-refresh" />{" "}
                    Vaciar Campos
                  </button>

                  <button
                    className="btn btn-outline-primary btn-sm"
                    style={{ marginLeft: "0.3em" }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Lista Cumplimiento"
                    onClick={() => this.DescargarListadoCumplimientos()}
                  >
                    <i className="fa fa-address-book" />{" "}
                    Lista Cumplimiento
                  </button>

                </div>
              </div>

              <div className="row">
                <div className="col">
                  <Field
                    name="Identificacion"
                    label="No. de Identificación"
                    edit={true}
                    readOnly={false}
                    value={this.state.Identificacion}
                    onChange={this.handleChange}
                    error={this.state.errors.Identificacion}
                  />
                  <Field
                    name="TipoAccionId"
                    value={this.state.TipoAccionId}
                    label="Tipo Acción"
                    options={this.state.TipoAcciones}
                    type={"select"}
                    onChange={this.onChangeValue}
                    error={this.state.errors.TipoAccionId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                    filter={true}
                    required
                  />

                </div>
                <div className="col">
                  <Field
                    name="ApellidosNombres"
                    label="Apellidos  Nombres"
                    edit={true}
                    readOnly={false}
                    value={this.state.ApellidosNombres}
                    onChange={this.handleChange}
                    error={this.state.errors.ApellidosNombres}
                  />

                  <Field
                    name="DepartamentoId"
                    value={this.state.DepartamentoId}
                    label="Departamento"
                    options={this.state.Departamentos}
                    type={"select"}
                    onChange={this.onChangeValue}
                    error={this.state.errors.DepartamentoId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                    filter={true}
                  />
                </div>

              </div>

              <div className="row">
                <div className="col" >
                  <Field
                    name="DiasVencimiento"
                    label="Dias Vencimiento"
                    type="number"
                    edit={true}
                    readOnly={false}
                    value={this.state.DiasVencimiento}
                    onChange={this.handleChange}
                    error={this.state.errors.DiasVencimiento}
                  />
                </div>
                <div className="col">
                  <br />
                  <Checkbox checked={this.state.Vencido} onChange={e => this.setState({ Vencido: e.checked })} />

                  {" "}<label>Vencidos: </label>
                  <br />
                  <Checkbox checked={this.state.Obligatorio} onChange={e => this.setState({ Obligatorio: e.checked })} />

                  {" "}<label>Obligatorios: </label>

                </div>

              </div>

            </div>
          </Card><br />
          {this.RenderizarColaborador()}
        </div>
        <br />
        <div className="content-section implementation">
          <Card>
            <div>
              <BootstrapTable data={this.state.data} hover={true} pagination={true}>
                <TableHeaderColumn
                  dataField="Id"
                  //dataFormat={this.Secuencial}
                  width={"8%"}
                  tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                  isKey={true}
                >
                  Nº
            </TableHeaderColumn>

                <TableHeaderColumn
                  dataField="Identificacion"
                  tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  dataSort={true}
                >
                  No.Identificación
            </TableHeaderColumn>

                <TableHeaderColumn
                  dataField="NombresCompletos"
                  tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  dataSort={true}
                >
                  Nombres Completos
            </TableHeaderColumn>

                <TableHeaderColumn
                  dataField="Departamento"
                  tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  dataSort={true}
                >
                  Departamento
            </TableHeaderColumn>


                <TableHeaderColumn
                  dataField="Requisito"
                  tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  dataSort={true}
                >
                  Requisito
            </TableHeaderColumn>
                <TableHeaderColumn
                  dataField="Cumple"
                  tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  dataSort={true}
                >
                  Cumple
            </TableHeaderColumn>

                <TableHeaderColumn
                  dataField="fecha_caducidad"

                  tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  dataSort={true}
                >
                  Fecha Vencimiento
            </TableHeaderColumn>
              </BootstrapTable>
            </div>


          </Card>
        </div>
      </div>
    );

  }

  EnviarFormulario(event) {

  }

  MostrarFormulario() {

  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }


  OcultarFormulario() {
    this.setState({ visible: false });
    // this.VaciarCampos();
  }

}
const Container = Wrapper(RequisitosReporte);
ReactDOM.render(<Container />, document.getElementById("content"));
