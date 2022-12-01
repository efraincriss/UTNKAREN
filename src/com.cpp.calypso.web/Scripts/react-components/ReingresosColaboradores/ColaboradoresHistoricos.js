import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import { InputText } from "primereact-v3.3/inputtext";
import wrapForm from "../Base/BaseWrapper";
import { Dialog } from "primereact-v2/dialog";
import Field from "../Base/Field-v2";
import QRCode from "qrcode.react";
import { Card } from "primereact-v3.3/card";
import { Checkbox } from "primereact-v3.3/checkbox";
import { TabView, TabPanel } from "primereact-v3.3/tabview";
import { DataTable } from "primereact-v3.3/datatable";
import { Column } from "primereact-v3.3/column";

import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
class ColaboradoresHistoricos extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      errors: {},
      editable: true,
      data: [],
      asigned: [],
      checkeds: [],
      selected: null,
      Colaborador: null,
      Colaboradores: [],

      search: "",
      view: "lista",

      loadingqr: false,

      /*Generación QR */
      QrDialog: false,
      /*Generación QR */
      QrDialogE: false,
      DataColaborador: "",
      EncryptedData: "",
      selectIds: [],
    };
    this.MostrarDialogQrE = this.MostrarDialogQrE.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.actionBodyTemplate = this.actionBodyTemplate.bind(this);
    this.onRowSelect = this.onRowSelect.bind(this);
    this.onSelectAll = this.onSelectAll.bind(this);
    this.handleSelect = this.handleSelect.bind(this);
  }
  actionBodyTemplate(rowData) {
    return (
      <>
        <button
          className="btn btn-outline-primary"
          onClick={() => this.selectColaborador(rowData)}
          data-toggle="tooltip"
          data-placement="top"
          title="Generar Qr"
          style={{ marginRight: "0.1em" }}
        >
          <i className="fa fa-qrcode" />
        </button>
        <button
          className="btn btn-outline-success"
          onClick={() => this.descargarTarjetasIndividual(rowData)}
          data-toggle="tooltip"
          data-placement="top"
          style={{ marginRight: "0.1em" }}
          title="Descargar Tarjeta"
        >
          <i className="fa fa-credit-card-alt" />
        </button>
        <button
          className="btn btn-outline-indigo"
          onClick={() => this.addListado(rowData)}
          data-toggle="tooltip"
          data-placement="top"
          title="Añadir a Listado Masivo"
          style={{ marginRight: "0.1em" }}
        >
          <i className="fa fa-plus-circle" />
        </button>
      </>
    );
  }
  componentDidMount() {
    this.props.unlockScreen();
  }
  seachColaborador = (event) => {
    event.preventDefault();
    if (this.state.search.length > 0) {
      this.props.blockScreen();
      axios
        .post("/RRHH/ColaboradoresQr/ObtenerColaboradorHistorico", {
          search: this.state.search,
        })
        .then((response) => {
          console.log("Response", response);
          this.setState({ Colaboradores: response.data });
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.unlockScreen();
        });
    }
  };

  selectColaborador = (row) => {
    this.setState({ Colaborador: row });
    this.changeview("detalle");
    console.log("Colaborador", this.state.Colaborador);
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
  botonesColaborador = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-primary"
          onClick={() => this.selectColaborador(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Ver Movimientos"
          style={{ marginRight: "0.1em" }}
        >
          Ver
        </button>
      </div>
    );
  };
  botonesColaboradorReingreso = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-primary"
          onClick={() => this.LoadColaboradorEdit(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Ver"
          style={{ marginRight: "0.1em" }}
        >
          Ver
        </button>
      </div>
    );
  };
  botonesAssignados = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-danger"
          onClick={() => this.deleterow(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
          style={{ marginRight: "0.2em" }}
        >
          <i className="fa fa-trash-o" />
        </button>
      </div>
    );
  };
  MostrarDialogQrE(row) {
    axios
      .post("/RRHH/Colaboradores/GetDataQrE/", {
        Id: row.Id,
      })
      .then((response) => {
        console.log(response.data);
        this.setState({
          QrDialog: true,
          loadingqr: false,
          EncryptedData: response.data,
        });
      })
      .catch((error) => {
        console.log(error);
      });

    this.setState({
      iseleccionado: row,
      loadingqr: true,
      checked: row.validacion_cedula,
      QrDialog: true,
    });
  }
  renderShowsTotal(start, to, total) {
    return (
      <p style={{ color: "blue" }}>
        De {start} hasta {to}, Total Registros {total}
      </p>
    );
  }
  onRowSelect(row, isSelected, e) {
    console.log("isSelected", isSelected);
    console.log("row", row);
    console.log("e", e);
    let newList = [];

    if (isSelected) {
      //Add
      newList = [...this.state.selectIds, row];
    } else {
      //Delete
      let propsLocal = { ...this.state };
      newList = propsLocal.selectIds.filter((item) => item.Id !== row.Id);
    }

    this.handleSelect(isSelected, newList);
  }

  onSelectAll(isSelected, rows) {
    console.log("isSelected", isSelected);
    console.log("rows", rows);
    let newList = [];

    if (isSelected) {
      //Add All
      for (var i = 0; i < rows.length; i++) {
        newList.push(rows[i]);
      }
    }

    this.handleSelect(isSelected, newList);
  }
  handleSelect(isSelected, selectIds) {
    var newSelectIds = [];
    this.setState({ selectIds: selectIds });
  }

  render() {
    console.log("Selecion:");
    console.log(this.state.selectIds);

    const selectRowProp = {
      mode: "checkbox",
      width: "13%",
      clickToSelect: true,
      onSelect: this.onRowSelect,
      onSelectAll: this.onSelectAll,
      selected: this.state.selectIds.map((item) => {
        return item.Id;
      }),
    };
    console.log(selectRowProp);
    const optionsColaboradores = {
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
    };

    if (this.state.view === "lista") {
      return (
        <div>
          <div className="row" style={{ marginTop: "1em" }}>
            <div className="col">
              <div className="row">
                <div className="col-10">
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
                <div className="col-2"></div>
              </div>
              <br />
              <div className="row">
                <div className="col">
                  <BootstrapTable
                    data={this.state.Colaboradores}
                    options={optionsColaboradores}
                   // selectRow={selectRowProp}
                    pagination
                    striped
                    hover
                  >
                    <TableHeaderColumn
                      dataField="any"
                      dataFormat={this.Secuencial}
                      width={"6%"}
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
                      dataField="NumeroLejajo"
                      tdStyle={{ whiteSpace: "normal" }}
                      thStyle={{ whiteSpace: "normal" }}
                      filter={{ type: "TextFilter", delay: 500 }}
                      tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                      thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                      dataSort={true}
                    >
                      Lejajo
                    </TableHeaderColumn>
                    <TableHeaderColumn
                      dataField="TipoIdentificacion"
                      tdStyle={{ whiteSpace: "normal" }}
                      thStyle={{ whiteSpace: "normal" }}
                      filter={{ type: "TextFilter", delay: 500 }}
                      tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                      thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                      dataSort={true}
                    >
                      Tipo Id
                    </TableHeaderColumn>
                    <TableHeaderColumn
                      dataField="Identificacion"
                      width={"12%"}
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
                      dataField="NombreCompleto"
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
                      width={"10%"}
                      dataField="FechaUltimoIngreso"
                      tdStyle={{ whiteSpace: "normal" }}
                      thStyle={{ whiteSpace: "normal" }}
                      filter={{ type: "TextFilter", delay: 500 }}
                      tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                      thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                      dataSort={true}
                    >
                      Fecha Ingreso
                    </TableHeaderColumn>

                    <TableHeaderColumn
                      dataField="Estado"
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
                      dataField="NumeroReingresos"
                      tdStyle={{ whiteSpace: "normal" }}
                      thStyle={{ whiteSpace: "normal" }}
                      filter={{ type: "TextFilter", delay: 500 }}
                      tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                      thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                      dataSort={true}
                    >
                      Numero Reingresos
                    </TableHeaderColumn>
                    <TableHeaderColumn
                      width={"15%"}
                      dataField="Id"
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
          </div>
        </div>
      );
    } else {
      return (
        <div>
          <div className="row">
            <div className="col-10"></div>
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
          <br />
          <div className="row">
            <div className="col">
              <div className="card border-info">
                <div className="card-header">Colaborador</div>
                <div className="card-body">
                  <div className="row">
                    <div className="col-6">
                      <h6>
                        <b>IDENTIFICACIÓN:</b>{" "}
                        {this.state.Colaborador != null
                          ? this.state.Colaborador.Identificacion
                          : ""}
                      </h6>
                      <h6>
                        <b>ÁREA:</b>{" "}
                        {this.state.Colaborador != null
                          ? this.state.Colaborador.Area
                          : ""}
                      </h6>
                      <h6>
                        <b>TIPO USUARIO:</b>{" "}
                        {this.state.Colaborador != null
                          ? this.state.Colaborador.TipoUsuario
                          : ""}
                      </h6>
                    </div>
                    <div className="col-6">
                      <h6>
                        <b>NOMBRES COMPLETOS: </b>{" "}
                        {this.state.Colaborador != null
                          ? this.state.Colaborador.NombreCompleto
                          : ""}
                      </h6>
                      <h6>
                        <b>CARGO:</b>{" "}
                        {this.state.Colaborador != null
                          ? this.state.Colaborador.Cargo
                          : ""}
                      </h6>
                      <h6>
                        <b>
                          {this.state.Colaborador != null &&
                          !this.state.Colaborador.esExterno
                            ? "CÓDIGO SAP :"
                            : "CÓDIGO COLABORADOR EXTERNO :"}
                        </b>{" "}
                        {this.state.Colaborador != null
                          ? this.state.Colaborador.CodigoSap
                          : ""}
                      </h6>
                    </div>
                  </div>
                  <hr></hr>
                  <br />
                  <div className="row">
                    <div className="col">
                      <BootstrapTable
                        data={
                          this.state.Colaborador != null
                            ? this.state.Colaborador.Reingresos
                            : []
                        }
                        options={optionsColaboradores}
                       // selectRow={selectRowProp}
                        pagination
                        striped
                        hover
                      >
                        <TableHeaderColumn
                          dataField="any"
                          dataFormat={this.Secuencial}
                          width={"6%"}
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
                         
                          dataField="FechaUltimoIngreso"
                          tdStyle={{ whiteSpace: "normal" }}
                          thStyle={{ whiteSpace: "normal" }}
                          filter={{ type: "TextFilter", delay: 500 }}
                          tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                          thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                          dataSort={true}
                        >
                          Fecha Ingreso
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          
                          dataField="FechaSalida"
                          tdStyle={{ whiteSpace: "normal" }}
                          thStyle={{ whiteSpace: "normal" }}
                          filter={{ type: "TextFilter", delay: 500 }}
                          tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                          thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                          dataSort={true}
                        >
                          Fecha Salida
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="Cargo"
                          tdStyle={{ whiteSpace: "normal" }}
                          thStyle={{ whiteSpace: "normal" }}
                          filter={{ type: "TextFilter", delay: 500 }}
                          tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                          thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                          dataSort={true}
                        >
                         Cargo
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="Estado"
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
                          width={"15%"}
                          dataField="ColaboradorId"
                          isKey
                          dataFormat={this.botonesColaboradorReingreso}
                          tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                          thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                        >
                          Opciones
                        </TableHeaderColumn>
                      </BootstrapTable>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      );
    }
  }
  LoadColaboradorEdit = (id) => {
    sessionStorage.setItem("id_colaborador", id);
    return (window.location.href = "/RRHH/Colaboradores/Edit/");
  };
  sendChecked = () => {
    let data = this.state.Colaboradores;
    console.clear();
    console.log("Seleccionados", this.state.selectIds);
    this.state.selectIds.forEach((row) => {
      console.log("row", row);
      const exist = this.state.asigned.filter((c) => c.Id == row.Id).length;
      if (exist > 0) {
        data = data.filter((x) => {
          return x.Id != row.Id;
        });
      } else {
        this.state.asigned.push(row);
        data = data.filter((x) => {
          return x.Id != row.Id;
        });
      }
    });
    console.log("asigned", this.state.asigned);
    console.log("data", data);
    this.setState({ Colaboradores: data });
  };
  downloadQR = () => {
    const canvas = document.getElementById("123456");
    const pngUrl = canvas
      .toDataURL("image/png")
      .replace("image/png", "image/octet-stream");
    console.log("URL", pngUrl);
    let downloadLink = document.createElement("a");
    downloadLink.href = pngUrl;
    downloadLink.download =
      this.state.Colaborador.Identificacion + "_" + ".png";
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
  };
  descargarTarjetasIndividual = (row) => {
    const individual = [];
    individual.push(row);
    this.props.blockScreen();
    axios
      .post("/RRHH/ColaboradoresQr/ObtainUrl", {
        rows: individual,
      })
      .then((response) => {
        console.log(response.data);
        window.location.href = `/RRHH/ColaboradoresQr/DescargarTarjetas?url=${response.data}`;
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  descargarTarjetasMasiva = () => {
    this.props.blockScreen();
    axios
      .post("/RRHH/ColaboradoresQr/ObtainUrl", {
        rows: this.state.asigned,
      })
      .then((response) => {
        console.log(response.data);
        window.location.href = `/RRHH/ColaboradoresQr/DescargarTarjetas?url=${response.data}`;
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  addListado = (row) => {
    const data = this.state.asigned.filter((c) => c.Id == row.Id).length;
    console.log("data", data);
    if (data > 0) {
      this.props.showWarn(
        "Ya se agregó el colaborador " +
          row.NombreCompleto +
          " a la lista Masiva de generación de tarjetas"
      );
    } else {
      this.state.asigned.push(row);
      this.setState({
        Colaboradores: this.state.Colaboradores.filter((c) => c.Id !== row.Id),
      });
      this.props.showSuccess("Agregado a Listado Masivo");
    }
  };
  deleterow = (row) => {
    this.setState({
      asigned: this.state.asigned.filter((c) => c.Id !== row.Id),
    });
    this.state.Colaboradores.push(row);
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
const Container = wrapForm(ColaboradoresHistoricos);
ReactDOM.render(<Container />, document.getElementById("content"));
