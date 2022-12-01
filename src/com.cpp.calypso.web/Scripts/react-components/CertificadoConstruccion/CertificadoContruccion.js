import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";

import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v3.3/dialog";
import { Growl } from "primereact-v3.3/growl";
import { Button } from "primereact-v3.3/button";
import { Card } from "primereact-v3.3/card";
import { DataTable } from "primereact-v3.3/datatable";
import { Column } from "primereact-v3.3/column";
class CertificadoConstruccion extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      errors: {},
      ContratoId: 0,
      Contrato: null,
      ProyectoId: 0,
      fechaCorte: null,
      fechaEmision: new Date(),
      Contratos: [],
      Proyectos: [],
      Proyecto: null,
      vista: "principal",
      CertificadoId:null,

      dataconstruccion: [],
      seleccionadoscontruccion: [],
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.isValid = this.isValid.bind();
    this.IngresarAllCertificados = this.IngresarAllCertificados.bind(this);
  }

  componentDidMount() {
    this.GetContratos();
  }

  
  GetCert = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/Certificado/APIDetalles?id="+Id, {})
      .then((response) => {
        console.log(response.data);
     
        this.setState({ CertificadoId: response.data.Id }, this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
      });
  };

  isValid = () => {
    const errors = {};

    if (this.state.codigo_orden_servicio == "") {
      errors.codigo_orden_servicio = "Campo Requerido";
    }
    if (this.state.fecha_orden_servicio == null) {
      errors.fecha_orden_servicio = "Campo Requerido";
    }
    if (this.state.EstadoId == 0) {
      errors.EstadoId = "Campo Requerido";
    }

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };

  GetContratos = () => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/Certificado/APIContratos", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.Codigo, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Contratos: items }, this.props.unlockScreen());
      })
      .catch((error) => {
        console.log(error);
      });
  };

  GetProyectos = (Id) => {
    this.props.blockScreen();
    axios
      .post("/Proyecto/Certificado/APIProyectos", {
        Id: Id,
      })
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return {
            label: item.codigo + " - " + item.nombre_proyecto,
            dataKey: item.Id,
            value: item.Id,
          };
        });
        this.setState(
          {
            Proyectos: items,
            Contrato: this.state.Contratos.filter((c) => c.value == Id)[0],
          },
          this.props.unlockScreen()
        );
      })
      .catch((error) => {
        console.log(error);
      });
  };

  GetAvancesSinCertificar = () => {
    this.props.blockScreen();
    axios
      .post("/proyecto/Certificado/AvanceObraSinCertificar", {
        id: this.state.ProyectoId,
        fechaCorte: this.state.fechaCorte,
      })
      .then((response) => {
        console.log("response", response.data);
        this.setState(
          { dataconstruccion: response.data },
          this.props.unlockScreen()
        );
      })
      .catch((error) => {
        console.log(error);
      });
  };

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  onChangeValue = (name, value) => {
    this.setState({
      [name]: value,
    });
  };

  onChangeValueContrato = (name, value) => {
    this.setState({
      [name]: value,
    });

    this.GetProyectos(value);
  };
  onChangeValueProyecto = (name, value) => {
    this.setState({
      [name]: value,
      Proyecto: this.state.Proyectos.filter((c) => c.value == value)[0],
    });
    console.log("Proyecto", this.state.Proyecto);
  };

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

  renderGeneracion = () => {
    this.setState({ vista: "generaracion" });
    console.log("contrato", this.state.Contrato);
    console.log("Proeycto", this.state.Proyecto);
    this.GetAvancesSinCertificar();
  };
  renderListadoGeneral = () => {
   window.location.href='/Proyecto/Certificado'
  };
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
              this.EliminarDetalle(row.Id);
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

  RenderVista = () => {
    window.location.href='/Proyecto/Certificado'
    //this.setState({ vista: "principal" });
  };
  render() {
    if (this.state.vista === "principal") {
      return (
        <>
          <div className="row">
            <div className="col">
              <Field
                name="ContratoId"
                required
                value={this.state.ContratoId}
                label="Contrato"
                options={this.state.Contratos}
                type={"select"}
                filter={true}
                onChange={this.onChangeValueContrato}
                error={this.state.errors.ContratoId}
                readOnly={false}
                placeholder="Seleccione.."
                filterPlaceholder="Seleccione.."
              />
            </div>
            <div className="col">
              <Field
                name="ProyectoId"
                required
                value={this.state.ProyectoId}
                label="Proyecto"
                options={this.state.Proyectos}
                type={"select"}
                filter={true}
                onChange={this.onChangeValueProyecto}
                error={this.state.errors.ProyectoId}
                readOnly={false}
                placeholder="Seleccione.."
                filterPlaceholder="Seleccione.."
              />
            </div>
          </div>
          <div className="row">
            <div className="col">
              <Field
                name="fechaCorte"
                label="Fecha Corte"
                required
                type="date"
                edit={true}
                readOnly={false}
                value={this.state.fechaCorte}
                onChange={this.handleChange}
                error={this.state.errors.fechaCorte}
              />
            </div>
            <div className="col">
              <Field
                name="fechaEmision"
                label="Fecha Emisión"
                required
                type="date"
                edit={true}
                readOnly={false}
                value={this.state.fechaEmision}
                onChange={this.handleChange}
                error={this.state.errors.fechaEmision}
              />
            </div>
          </div>
          <hr />
          <div className="row">
            <div className="col">
              <button
                onClick={() => this.renderGeneracion()}
                className="btn btn-outline-primary"
              >
                Siguiente
              </button>
              <button
                onClick={() => this.renderListadoGeneral()}
                className="btn btn-outline-primary"
              >
                Cancelar
              </button>
            </div>
          </div>
        </>
      );
    } else {
      return (
        <BlockUi tag="div">
          <div align="right">
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={() => this.RenderVista()}
            >
              Regresar
            </button>
          </div>
          <Card title="Generación de Certificado">
            <div className="row">
              <div className="col">
                <h6>
                  <b>Contrato :</b>{" "}
                  {this.state.Contrato != null &&
                  this.state.Contrato !== undefined
                    ? this.state.Contrato.label
                    : ""}
                </h6>
                <h6>
                  <b>Fecha de Corte :</b>{" "}
                  {this.state.fechaCorte != null ? this.state.fechaCorte : ""}
                </h6>
              </div>

              <div className="col">
                <h6>
                  <b>Proyecto :</b>{" "}
                  {this.state.Proyecto != null &&
                  this.state.Proyecto !== undefined
                    ? this.state.Proyecto.label
                    : ""}
                </h6>
                <h6>
                  <b>FechaEmisión :</b>{" "}
                  {this.state.fechaCorte != null ? this.state.fechaCorte : ""}
                </h6>
              </div>
            </div>
          </Card>
          <br></br>
          <div className="col" align="right">
            <button
              onClick={() => this.IngresarAllCertificados()}
              className="btn btn-outline-primary"
            >
              Generar Certificados
            </button>{" "}
            {this.state.CertificadoId!=null && 
            <button
            onClick={() => this.DescargarCert()}
            className="btn btn-outline-primary"
            style={{ marginLeft: "0.2em" }}
          >
            Descargar Certificado
          </button>
            }
          </div>
          <hr />
          <div className="row">
            <div className="col">
              <DataTable
                value={this.state.dataconstruccion}
                header="Avances de Obra sin Certificar"
                selection={this.state.seleccionadoscontruccion}
                style={{ fontSize: "10px" }}
                paginator={true}
                dataKey="Id"
                rows={10}
                rowsPerPageOptions={[5, 10, 20]}
                onSelectionChange={(e) =>
                  this.setState({ seleccionadoscontruccion: e.value })
                }
              >
                <Column
                  selectionMode="multiple"
                  headerStyle={{ width: "3em" }}
                ></Column>
                <Column
                  field="codigoOferta"
                  header="Oferta"
                  filter
                />
                <Column field="AvanceObraId" header="Avance Obra" filter />
                <Column field="fechar" header="Fecha" filter />
                <Column field="item_codigo" header="Item" filter />
                <Column field="nombre_item" header="Descripción" filter />
                <Column
                  field="cantidad_diaria"
                  header="Cantidad Avance"
                  filter
                />
                <Column
                  field="cantidad_presupuestada"
                  header="Cant Presupuestada"
                  filter
                />
                <Column field="total" header="Monto Total" filter />
              </DataTable>
            </div>
          </div>
        </BlockUi>
      );
    }
  }

  IngresarAllCertificados = (event) => {
      console.log('event');
    console.log("seleccionados", this.state.seleccionadoscontruccion);
    this.props.blockScreen();
    if (this.state.seleccionadoscontruccion.length > 0) {
      axios
        .post("/Proyecto/Certificado/GenerarCertificados", {
          data: this.state.seleccionadoscontruccion.map((item) => {
            return item.Id;
          }),
          proyectoId: this.state.ProyectoId,
          fechaCorte: this.state.fechaCorte,
          fechaEmision:this.state.fechaEmision
        })
        .then((response) => {
          //this.GetDataProcura();
        
          // this.GetDataIngenieria();
          //this.GetMontoCertificados();
          var r = response.data;
          if (r != "ERROR" && r!=="OK") {
            this.props.showSuccess("Guardado Correctamente");
            this.GetCert(response.data);
            this.setState(
              {
                seleccionadoscontruccion: [],
              },
              this.GetAvancesSinCertificar()       );

          
          } else if (r == "Error") {
            this.props.showWarn("Existió un incoveniente inténtelo más tarde");
            this.props.unlockScreen();
          }
        })
        .catch((error) => {
          console.log(error);
          this.props.unlockScreen();
        });
    } else {
      this.props.showWarn("Debe Seleccionar al menos un avance de obra");
      this.props.unlockScreen();
    }
  };

  DescargarCert=()=>{
    this.props.blockScreen();
    axios
      .get(
        "/Proyecto/Certificado/IndexExcel/" ,
        { 
          params: {
            id:  this.state.CertificadoId,
            id2: this.state.ProyectoId,
          },responseType: "arraybuffer" }
      )
      .then(response => {
        console.log(response.headers["content-disposition"]);
        var nombre = response.headers["content-disposition"].split("=");

        const url = window.URL.createObjectURL(
          new Blob([response.data], {
            type:
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
          })
        );
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", nombre[1]);
        document.body.appendChild(link);
        link.click();
       this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Existió un incoveniente inténtelo más tarde");
        this.props.unlockScreen();
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
}
const Container = wrapForm(CertificadoConstruccion);
ReactDOM.render(<Container />, document.getElementById("content"));
