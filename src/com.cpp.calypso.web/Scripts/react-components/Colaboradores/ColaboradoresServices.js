import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { Card } from "primereact-v2/card";
import { Button } from "primereact-v2/button";

import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
import { Checkbox } from "primereact-v2/checkbox";
import { ToggleButton } from "primereact-v2/togglebutton";
import { MultiSelect } from "primereact-v2/multiselect";
class ColaboradoresServices extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      colaborador: null,
      tiposComidas: [],
      selectComidas: [],
      tiposTransporte: [],
      selectTansporte: [],

      /*SERVICES COLABORADOR */
      Alimentacion: false,
      Hospedaje: false,
      Transporte: false
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
  }

  componentDidMount() {
    this.GetCatalogs();
    this.GetData();
  }

  GetData = () => {
    this.props.blockScreen();
    axios
      .post(
        "/RRHH/Colaboradores/GetSimpleColaborador/" +
          document.getElementById("Id").className,
        {}
      )
      .then(response => {
        //   console.log("Colaborador", response.data);
        this.setState({
          colaborador: response.data
        });
        if (this.state.colaborador != null) {
          var comidas = this.state.colaborador.selectComidas.map(item => {
            return item.Id;
          });
          //   console.log(comidas);
          var transportes = this.state.colaborador.selectTansporte.map(item => {
            return item.Id;
          });
          //   console.log(transportes);
          this.setState({
            Alimentacion: this.state.colaborador.Alimentacion,
            Hospedaje: this.state.colaborador.Hospedaje,
            Transporte: this.state.colaborador.Transporte,
            selectComidas: comidas,
            selectTansporte: transportes
          });
          this.props.unlockScreen();
        }
      })
      .catch(error => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  GetCatalogs = () => {
    axios
      .post("/RRHH/Colaboradores/GetByCodeApi/?code=TIPOCOMIDA", {})
      .then(response => {
        // console.log("TIPOS COMIDA", response.data.result);
        var items = response.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ tiposComidas: items });
      })
      .catch(error => {
        console.log(error);
      });
    axios
      .post("/RRHH/Colaboradores/GetByCodeApi/?code=TIPODEMOVILIZACION", {})
      .then(response => {
        //   console.log("TIPOS TRANSPORTE", response.data.result);
        var items = response.data.result.map(item => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ tiposTransporte: items });
      })
      .catch(error => {
        console.log(error);
      });
  };

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };

  render() {
    return (
      <div>
        <div className="row">
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <div className="content-section implementation">
                  <div className="row">
                    <div className="col-8">
                      <h5>SERVICIOS A COLABORADOR:</h5>
                    </div>
                  </div>
                  <hr />

                  <div>
                    <div className="row">
                      <div className="col-6">
                        <h6>
                          <b>TIPO IDENTIFICACIÓN :</b>{" "}
                          {this.state.colaborador != null
                            ? this.state.colaborador.tipoIdentificacion
                            : ""}
                        </h6>
                        <h6>
                          <b>NOMBRES COMPLETOS :</b>{" "}
                          {this.state.colaborador != null
                            ? this.state.colaborador.nombresCompletos
                            : ""}
                        </h6>
                        <h6>
                          <b>ID LEGAJO :</b>{" "}
                          {this.state.colaborador != null
                            ? this.state.colaborador.idLegajo
                            : ""}
                        </h6>
                        <h6>
                          <b>TIPO COLABORADOR :</b>{" "}
                          {this.state.colaborador != null
                            ? this.state.colaborador.tipoColaborador
                            : ""}
                        </h6>
                      </div>
                      <div className="col-6">
                        <h6>
                          <b>NRO IDENTIFICACIÓN :</b>{" "}
                          {this.state.colaborador != null
                            ? this.state.colaborador.identificacion
                            : ""}
                        </h6>
                        <h6>
                          <b>ESTADO :</b>{" "}
                          {this.state.colaborador != null
                            ? this.state.colaborador.estado
                            : ""}
                        </h6>
                        <h6>
                          <b>ID SAP GLOBAL :</b>{" "}
                          {this.state.colaborador != null
                            ? this.state.colaborador.idSap
                            : ""}
                        </h6>
                        <h6>
                          <b>ID SAP LOCAL :</b>{" "}
                          {this.state.colaborador != null
                            ? this.state.colaborador.idSapLocal
                            : ""}
                        </h6>
                      </div>
                    </div>
                  </div>
                  <hr />
                  <div className="row">
                    <div className="col-6">
                      {" "}
                      <label>
                              <b>SERVICIOS:</b>
                            </label>
                      <br />
                      <div className="content-section implementation">
                        <ToggleButton
                          style={{ width: "150px" }}
                          onLabel="Alimentación"
                          offLabel="Alimentación"
                          onIcon="pi pi-check"
                          offIcon="pi pi-times"
                          checked={this.state.Alimentacion}
                          onChange={e =>
                            this.setState({ Alimentacion: e.value })
                          }
                        />
                        <hr />
                        <ToggleButton
                          style={{ width: "150px" }}
                          onLabel="Transporte"
                          offLabel="Transporte"
                          onIcon="pi pi-check"
                          offIcon="pi pi-times"
                          checked={this.state.Transporte}
                          onChange={e => this.setState({ Transporte: e.value })}
                        />

                        <hr />
                        <ToggleButton
                          style={{ width: "150px" }}
                          onLabel="Hospedaje"
                          offLabel="Hospedaje"
                          onIcon="pi pi-check"
                          offIcon="pi pi-times"
                          checked={this.state.Hospedaje}
                          onChange={e => this.setState({ Hospedaje: e.value })}
                        />
                      </div>
                    </div>
                    <div className="col-6">
                      <div className="content-section implementation multiselect-demo">
                        {this.state.Alimentacion && (
                          <div>
                            <label>
                              <b>TIPOS COMIDA</b>
                            </label>
                            <br />
                            <MultiSelect
                              value={this.state.selectComidas}
                              options={this.state.tiposComidas}
                              onChange={e =>
                                this.setState({ selectComidas: e.value })
                              }
                              style={{ minWidth: "15em" }}
                              defaultLabel="Seleccione.."
                              filter={true}
                              placeholder="Seleccione"
                            />
                          </div>
                        )}
                        <br />
                        {this.state.Transporte && (
                          <div>
                            <label>
                              <b>TIPOS MOVILIZACIÓN</b>
                            </label>
                            <br />
                            <MultiSelect
                              value={this.state.selectTansporte}
                              options={this.state.tiposTransporte}
                              onChange={e =>
                                this.setState(
                                  { selectTansporte: e.value },
                                  console.log(this.state.selectTansporte)
                                )
                              }
                              style={{ minWidth: "15em" }}
                              defaultLabel="Seleccione.."
                              filter={true}
                              placeholder="Seleccione"
                            />
                          </div>
                        )}
                      </div>
                    </div>
                  </div>
                  <hr />
                  <div className="row">
                    <div className="col">
                      <Button
                        label="Guardar"
                        icon="pi pi-check"
                        onClick={this.GuardarServicios}
                        className="p-button-secondary"
                      />{" "}
                      <Button
                        label="Regresar"
                        onClick={this.Redireccionar}
                        icon="pi pi-times"
                        className="p-button-secondary"
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
  VaciarCampos = () => {
    this.setState({
      colaborador: null,  
      selectComidas: [],
      selectTansporte: [],

      /*SERVICES COLABORADOR */
      Alimentacion: false,
      Hospedaje: false,
      Transporte: false
    });
  };
  Redireccionar = () => {
    return (window.location = `/RRHH/Colaboradores/Servicios`);
  };
  GuardarServicios = () => {
    this.props.blockScreen();
    if (this.state.Alimentacion) {
      if (this.state.selectComidas.length > 0) {
      } else {
        this.props.unlockScreen();
        abp.notify.error("Debe Seleccionar los tipos de comida", "Error");
        return;
      }
    }
    if (this.state.Transporte) {
      if (this.state.selectTansporte.length > 0) {
      } else {
        this.props.unlockScreen();
        abp.notify.error("Debe Seleccionar los tipos de transporte", "Error");
        return;
      }
    }

    axios
      .post("/RRHH/Colaboradores/GetInsertServices", {
        Id: document.getElementById("Id").className,
        Alimentacion: this.state.Alimentacion,
        Hospedaje: this.state.Hospedaje,
        Transporte: this.state.Transporte,
        selectComidas: this.state.selectComidas,
        selectTransporte: this.state.selectTansporte
      })
      .then(response => {
        // console.log(response.data);
        if (response.data === "OK") {
          this.props.showSuccess("Guardado Correctamente");
          this.VaciarCampos();
          this.GetData();
        } else {
          abp.notify.error("Ocurrió un error inténtelo más tarde.", "Error");
          this.props.unlockScreen();
        }
      })
      .catch(error => {
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
          uploadFile: uploadFile
        });
      }
    } else {
      this.setState({ [event.target.name]: event.target.value });
    }
  }
}
const Container = wrapForm(ColaboradoresServices);
ReactDOM.render(<Container />, document.getElementById("content"));
