import React, { Component } from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import { Dialog } from "primereact-v2/dialog";
import { Button } from "primereact/components/button/Button";
import { Growl } from "primereact/components/growl/Growl";
import WbsPresupuestosVista from "./WbsRdo/WbsPresupuestosVista";
import WbsRdo from "./WbsRdo/WbsRdo";
import WbsSelect from "./WbsRdo/WbsSelect";

class ActualizacionWbs extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: true,
      origen: 0,
      destino: 0,
      data: [],
      visible: false,
    };
    this.WbsRdo = React.createRef();
    this.WbsSelect = React.createRef();
    this.DesbloquearPantalla = this.DesbloquearPantalla.bind(this);
    this.BloquearPantalla = this.BloquearPantalla.bind(this);
    this.EstablecerNodoOrigen = this.EstablecerNodoOrigen.bind(this);
    this.EstablecerNodoDestino = this.EstablecerNodoDestino.bind(this);
    this.SetearDatosRDO = this.SetearDatosRDO.bind(this);
    this.OcultarDialogo = this.OcultarDialogo.bind(this);
    this.MostrarDialogo = this.MostrarDialogo.bind(this);
    this.EnviarWBS = this.EnviarWBS.bind(this);
    this.successMessage = this.successMessage.bind(this);
    this.warnMessage = this.warnMessage.bind(this);
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
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <div className="row">
                  <div className="col" align="right">
                    <button
                      className="btn btn-outline-primary"
                      onClick={() => this.Direccionar("WBS")}
                      style={{ marginLeft: "0.4em" }}
                    >
                      WBS
                    </button>
                    <button
                      className="btn btn-outline-primary"
                      onClick={() => this.Direccionar("COMPUTOS")}
                      style={{ marginLeft: "0.4em" }}
                    >
                      Computos
                    </button>
                    <button
                      className="btn btn-outline-primary"
                      onClick={() => this.Direccionar("Presupuesto")}
                      style={{ marginLeft: "0.4em" }}
                    >
                      Regresar
                    </button>
                  </div>
                  <hr />
                </div>

                <div className="row">
                  <div className="col-sm-12 col-md-6">
                    <WbsRdo
                      ref={this.WbsRdo}
                      OfertaId={document.getElementById("OfertaId").className}
                      SetearDatosRDO={this.SetearDatosRDO}
                    />
                  </div>

                  <div className="col-sm-12 col-md-6">
                    <WbsPresupuestosVista
                      PresupuestoId={
                        document.getElementById("PresupuestoId").className
                      }
                      DesbloquearPantalla={this.DesbloquearPantalla}
                      BloquearPantalla={this.BloquearPantalla}
                      EstablecerNodoOrigen={this.EstablecerNodoOrigen}
                      MostrarDialogo={this.MostrarDialogo}
                    />
                  </div>
                </div>

                <Dialog
                  header="Seleccionar Destino"
                  visible={this.state.visible}
                  style={{ width: "70vw" }}
                  modal={true}
                  footer={footer}
                  maximizable
                  baseZIndex={99}
                  onHide={this.OcultarDialogo}
                >
                  <WbsSelect
                    ref={this.WbsSelect}
                    DesbloquearPantalla={this.DesbloquearPantalla}
                    BloquearPantalla={this.BloquearPantalla}
                    OfertaId={document.getElementById("OfertaId").className}
                    SetearDatosRDO={this.SetearDatosRDO}
                    data={this.state.data}
                    EstablecerNodoDestino={this.EstablecerNodoDestino}
                  />
                </Dialog>
              </div>
            </div>
          </div>
        </div>
      </BlockUi>
    );
  }

  EnviarWBS() {
    /*this.WbsRdo.current.updateData();
        this.OcultarDialogo();*/

    if (this.state.origen === 0) {
      this.warnMessage("Selecciona el WBS origen");
    } else if (this.state.destino === 0) {
      this.warnMessage("Selecciona el WBS destino");
    } else {
      this.WbsSelect.current.BloquearPantalla();
      axios
        .post("/proyecto/Wbs/CopiarWbs/", {
          origen: this.state.origen,
          destino: this.state.destino,
          PresupuestoId: document.getElementById("PresupuestoId").className,
          OfertaId: document.getElementById("OfertaId").className,
        })
        .then((response) => {
          if (response.data === "OK") {
            this.WbsRdo.current.updateData();
            this.OcultarDialogo();
            this.WbsSelect.current.DesbloquearPantalla();
            this.successMessage("Se copio el WBS correctamente");
          } else {
            this.warnMessage("Revisa el origen y destino del WBS");
            this.WbsSelect.current.DesbloquearPantalla();
          }
        })
        .catch((error) => {
          console.log(error);
          this.warnMessage("Intentalo más tarde");
          this.WbsSelect.current.DesbloquearPantalla();
        });
    }
  }

  EstablecerNodoOrigen(event) {
    this.setState({ origen: event.value });
  }

  EstablecerNodoDestino(id) {
    this.setState({ destino: parseInt(id) });
  }

  SetearDatosRDO(data) {
    this.setState({ data: data });
  }

  DesbloquearPantalla() {
    this.setState({ blocking: false });
  }

  BloquearPantalla() {
    this.setState({ blocking: true });
  }

  Direccionar(pantalla) {
    if (pantalla === "Presupuesto") {
      window.location.href =
        "/Proyecto/Requerimiento/Details/" +
        document.getElementById("RequerimientoId").className;
    } else if (pantalla === "WBS") {
      window.location.href =
        "/Proyecto/WBS/Index/" +
        document.getElementById("PresupuestoId").className;
    } else if (pantalla === "COMPUTOS") {
      window.location.href =
        "/Proyecto/Computo/EstructuraComputos/" +
        document.getElementById("PresupuestoId").className;
    }
  }

  MostrarDialogo() {
    this.setState({ visible: true });
  }

  OcultarDialogo() {
    this.setState({ visible: false, origen: 0, destino: 0, label: "" });
  }

  successMessage(msg) {
    this.setState({ message: msg }, this.showInfo);
  }

  warnMessage(msg) {
    this.setState({ message: msg }, this.showWarn);
  }

  showInfo() {
    this.growl.show({
      severity: "info",
      summary: "",
      detail: this.state.message,
    });
  }

  showWarn() {
    this.growl.show({
      severity: "warn",
      summary: "",
      detail: this.state.message,
    });
  }
}

ReactDOM.render(
  <ActualizacionWbs />,
  document.getElementById("content-main-presupuesto")
);
