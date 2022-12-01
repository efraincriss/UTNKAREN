import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import { InputText } from "primereact-v3.3/inputtext";
import wrapForm from "../../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
class AsignacionUsuario extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      errors: {},
      editable: true,
      data: [],
      asigned: [],
      selected: null,
      Colaborador: null,
      Colaboradores: [],

      visible: false,
      read: false,
      write: false,
      both: false,
      search: "",
      view: "lista",
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.onChangeValueColaborador = this.onChangeValueColaborador.bind(this);
    this.isValid = this.isValid.bind();
  }

  componentDidMount() {
    this.GetCatalogs();
  }
  isValid = () => {
    const errors = {};

    if (this.state.Colaborador === null) {
      errors.Colaborador = "Campo Requerido";
    }
    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };

  GetCatalogs = () => {
    this.props.blockScreen();
    axios
      .post("/Accesos/UsuarioRequisito/ObtenerReponsableRequisito", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ data: items });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  seachColaborador = (event) => {
    event.preventDefault();
    if (this.state.search.length > 0) {
      this.props.blockScreen();
      axios
        .post("/Accesos/UsuarioRequisito/ObtenerColaborador", {
          search: this.state.search,
        })
        .then((response) => {
          this.setState({ Colaboradores: response.data });
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.unlockScreen();
        });
    }
  };

  onChangeCatalogo = (event) => {
    this.setState({ selected: event.value });
    /// this.GetAsignacionesUsuarios(event.value);
  };

  selectColaborador = (row) => {
    this.setState({ Colaborador: row });
    this.GetAsignacionesUsuarios(row.value);
  };
  GetAsignacionesUsuarios = (value) => {
    this.props.blockScreen();
    axios
      .post("/Accesos/UsuarioRequisito/ObtenerAsignaciones", {
        colaboradorId: value,
      })
      .then((response) => {
        this.setState({ asigned: response.data });
        this.changeview("detalle");
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  changeview = (type) => {
    this.setState({ view: type });
  };

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };
  onChangeValueColaborador = (name, value) => {
    const _this = this;
    this.setState({
      [name]: value,
    });
    if (value.both) {
      _this.props.showWarning(
        "Ya se asignaron los dos permisos para: " + value.label
      );
    }
  };

  readFormatter = (cell, row) => {
    return row.read ? "SI" : "NO";
  };
  writeFormatter = (cell, row) => {
    return row.write ? "SI" : "NO";
  };
  botonesColaborador = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-primary btn-sm"
          onClick={() => this.selectColaborador(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Ver Permisos"
        >
          <i className="fa fa-eye" />
          &nbsp; Permisos
        </button>
      </div>
    );
  };
  onAfterSaveCell(row, cellName, cellValue) {
    this.props.blockScreen();
    axios
      .post("/Accesos/UsuarioRequisito/ActualizaryCrear", {
        m: row,
      })
      .then((response) => {
        if (response.data == "OK") {
          this.GetAsignacionesUsuarios(row.colaboradorId);
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  }
  render() {
    const optionsColaboradores = {
      sizePerPage: 5,
      noDataText: "No existen datos registrados",
      sizePerPageList: [
        {
          text: "5",
          value: 5,
        },
        {
          text: "10",
          value: 10,
        },
      ],
    };
    const cellEditProp = {
      mode: "click",
      blurToSave: true,
      afterSaveCell: this.onAfterSaveCell.bind(this), // a hook for after saving cell
    };
    if (this.state.view === "lista") {
      return (
        <div>
          <div className="row">
            <div className="col-12">
              <form onSubmit={this.seachColaborador}>
                <div className="row">
                  <div className="col-10">
                    <InputText
                      style={{ width: "100%" }}
                      value={this.state.search}
                      onChange={(e) =>
                        this.setState({ search: e.target.value })
                      }
                      placeholder="Identificación o Apellidos Nombres"
                    />
                  </div>
                  <div className="col-2" align="right">
                    <button
                      style={{ width: "100%" }}
                      type="submit"
                      className="btn btn-outline-primary"
                    >
                      Buscar
                    </button>
                  </div>
                </div>
              </form>
            </div>
          </div>
          <br />
          <div className="row">
            <div className="col">
              <BootstrapTable
                data={this.state.Colaboradores}
                hover={true}
                pagination={true}
                options={optionsColaboradores}
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
                  dataField="other"
                  width={"17%"}
                  tdStyle={{ whiteSpace: "normal" }}
                  thStyle={{ whiteSpace: "normal" }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  dataSort={true}
                >
                  Identificación
                </TableHeaderColumn>
                <TableHeaderColumn
                  dataField="label"
                  tdStyle={{ whiteSpace: "normal" }}
                  thStyle={{ whiteSpace: "normal" }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  dataSort={true}
                >
                  Apellidos y Nombres
                </TableHeaderColumn>
                <TableHeaderColumn
                  width={"12%"}
                  dataField="dataKey"
                  isKey
                  dataFormat={this.botonesColaborador}
                  tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                >
                  Opciones
                </TableHeaderColumn>
              </BootstrapTable>
            </div>
          </div>
        </div>
      );
    } else {
      return (
        <div>
          <div className="row">
            <div className="col">
              <div className="card border-info">
                <div className="card-header">Colaborador</div>
                <div className="card-body">
                  <div className="row">
                    <div className="col-3">
                      <strong>IDENTIFACIÓN: </strong>
                      {this.state.Colaborador != null
                        ? this.state.Colaborador.other
                        : ""}
                    </div>
                    <div className="col-7" style={{ textAlign: "justify" }}>
                      <strong>NOMBRES COMPLETOS: </strong>
                      {this.state.Colaborador != null
                        ? this.state.Colaborador.label
                        : ""}
                    </div>
                    <div className="col-2" align="right">
                      <button
                        style={{ width: "100%" }}
                        className="btn btn-outline-primary"
                        onClick={() => this.changeview("lista")}
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Regresar"
                      >
                        Regresar
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="row">
            <div className="col">
              <BootstrapTable
                data={this.state.asigned}
                hover={true}
                pagination={true}
                cellEdit={cellEditProp}
                //options={optionsColaboradores}
              >
                <TableHeaderColumn
                  dataField="any"
                  editable={false}
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
                  dataField="nombreResponsabilidad"
                  tdStyle={{ whiteSpace: "normal" }}
                  thStyle={{ whiteSpace: "normal" }}
                  filter={{ type: "TextFilter", delay: 500 }}
                  tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  dataSort={true}
                  editable={false}
                >
                  Grupo Requisito
                </TableHeaderColumn>
                <TableHeaderColumn
                  hidden
                  isKey
                  dataField="catalogoReponsabilidadId"
                  tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                ></TableHeaderColumn>
                <TableHeaderColumn
                  dataField="read"
                  width={"15%"}
                  dataFormat={this.readFormatter}
                  tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  editable={{ type: "checkbox" }}
                >
                  Lectura (Presione para Editar)
                </TableHeaderColumn>
                <TableHeaderColumn
                  tdStyle={{
                    whiteSpace: "normal",
                    fontSize: "11px",
                    textAlign: "justify",
                  }}
                  thStyle={{
                    whiteSpace: "normal",
                    fontSize: "11px",
                    textAlign: "justify",
                  }}
                  dataField="write"
                  width={"15%"}
                  dataFormat={this.writeFormatter}
                  editable={{ type: "checkbox" }}
                >
                  Edición (Presione para Editar)
                </TableHeaderColumn>
              </BootstrapTable>
            </div>
          </div>
        </div>
      );
    }
  }
  SendDelete = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Accesos/UsuarioRequisito/DeleteApi", {
        Id: Id,
      })
      .then((response) => {
        this.GetAsignacionesUsuarios(this.state.selected);
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
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
}
const Container = wrapForm(AsignacionUsuario);
ReactDOM.render(<Container />, document.getElementById("content"));
