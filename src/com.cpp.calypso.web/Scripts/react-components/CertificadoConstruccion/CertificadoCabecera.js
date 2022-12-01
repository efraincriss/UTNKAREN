import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";

import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v3.3/dialog";
import { Growl } from "primereact-v3.3/growl";
import { Card } from "primereact-v3.3/card";
import { Button } from "primereact-v3.3/button";
import { DataTable } from "primereact-v3.3/datatable";
import { Column } from "primereact-v3.3/column";
class CertificadoCabecera extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      errors: {},
      Certificado: null,
      pass: "",
      viewdisable:false
    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.onActivar = this.onActivar.bind(this);
    this.isValid = this.isValid.bind();
    this.IngresarAllCertificados = this.IngresarAllCertificados.bind(this);
  }

  componentDidMount() {
    this.GetContratos();

  }
  ViewOpen = () => {

    this.setState({ viewdisable: true });
}

  onHideview = () => {

    this.setState({ viewdisable: false,pass:""});
}
  onActivar() {
    if (this.state.pass === "") {
      abp.notify.error("Debe ingresar el código de Seguridad", "Error");
    } else {
        this.props.blockScreen();
      console.log("onActivar ");

      var self = this;
      self.setState({ blocking: true });

      let url = "";
      url = `/Proyecto/Certificado/EnableDisableApi`;

      let data = {
        id: document.getElementById("Id").className,

        pass: this.state.pass,
      };

      axios
        .post(url, data)
        .then((response) => {
         

          if (response.data=="OK") {
            abp.notify.success("Certificado Desaprobado", "Aviso");
            this.onHideview();
            this.GetContratos();

            window.location.href="/Proyecto/Certificado/DetailsCertificado/"+this.state.Certificado.Id;

          } else {
            abp.notify.error("El código de seguridad es incorrecto", "Error");
            //TODO:
            //Presentar errores...
            //var message = $.fn.responseAjaxErrorToString(data);
            // abp.notify.error(message, 'Error');
            this.props.unlockScreen();
          }

        })
        .catch((error) => {
          console.log(error);
            this.props.unlockScreen();
        });
    }
  }
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
      .post("/Proyecto/Certificado/APIDetalles?id="+ document.getElementById("Id").className, {})
      .then((response) => {
        console.log(response.data);
     
        this.setState({ Certificado: response.data }, this.props.unlockScreen());
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
    window.location.href = "/Proyecto/Certificado";
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
    window.href.location = "/Proyecto/Certificado";
    //this.setState({ vista: "principal" });
  };
  render() {
    return (
      <div className="col">
        {this.state.Certificado!=null && this.state.Certificado.estado_actual==1 &&
        <button
          onClick={() => this.ViewOpen()}
          className="btn btn-outline-primary btn-sm"
          style={{ marginRight: "0.2em" }}
        >
          Desaprobar
        </button>
        }
        <button
          onClick={() => this.renderListadoGeneral()}
          className="btn btn-outline-primary btn-sm"
        >
          Regresar
        </button>

        <Dialog header="Desaprobar Certificado" visible={this.state.viewdisable} style={{ width: '50vw' }} modal onHide={this.onHideview} >

<div>Está seguro de Desaprobar el Certificado. ¿Desea continuar?</div>
<br />
<Field
    name="pass"
    label="Código de Seguridad"
    required
    edit={true}
    value={this.state.pass}
    onChange={this.handleChange}


/> <br />
<div align="right">
    <Button label="SI" icon="pi pi-check" onClick={this.onActivar} />{" "}
    <Button label="NO" icon="pi pi-times" className="p-button-secondary" onClick={this.onHideview} />
</div>
</Dialog>

      </div>
    );
  }

  IngresarAllCertificados = (event) => {
    console.log("event");
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
          fechaEmision: this.state.fechaEmision,
        })
        .then((response) => {
          //this.GetDataProcura();
          this.GetAvancesSinCertificar();
          // this.GetDataIngenieria();
          //this.GetMontoCertificados();
          var r = response.data;
          if (r == "OK") {
            this.props.showSuccess("Guardado Correctamente");
            this.setState(
              {
                seleccionadoscontruccion: [],
              },
              this.props.unlockScreen()
            );
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
const Container = wrapForm(CertificadoCabecera);
ReactDOM.render(<Container />, document.getElementById("cabecera"));
