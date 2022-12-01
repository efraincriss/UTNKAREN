import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import Wrapper from "../Base/BaseWrapper";
import { Growl } from "primereact/components/growl/Growl";
import { Card } from 'primereact-v2/card';
import { DataTable } from 'primereact-v2/datatable';
import { Column } from 'primereact-v2/column';
import CurrencyFormat from "react-currency-format";
import { Accordion, AccordionTab } from "primereact-v2/accordion";

class LiquidacionServicioDetails extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: false,
      data: [],
      visible: false,
      visibleeditar: false,
      errors: {},

      //LIQUIDACION SERVICIOS//
      //CABECERA
      Liquidacion: null,
      //
      FechaDesde: new Date(),
      FechaHasta: new Date(),
      Proveedores: [],
      ProveedorId: 0,

      MontoHospedaje: 0.0,
      MontoAlimentacion: 0.0,
      MontoViandas: 0.0,

      hospedaje: [],
      hospedajependientes: [],
      alimentacion: [],
      alimentacionpendientes: [],

      viandas: [],
      viandaspendientes: [],

      hospedajeseleccionados: [],
      hospedajeseleccionadospendientes: [],

      alimentacionseleccionados: [],
      alimentacionseleccionadospendientes: [],

      viandasseleccionados: [],
      viandasseleccionadospendientes: [],
    };

    this.RedireccionarDetalle = this.RedireccionarDetalle.bind(this);
  }

  componentDidMount() {
    this.ObtenerDetalleLiquidacion();
    this.props.unlockScreen();
  }

  ObtenerDetalleLiquidacion = () => {
    console.log(document.getElementById("LiquidacionId").className)
    this.props.blockScreen();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerDetallesLiquidacion", {
        id: document.getElementById("LiquidacionId").className
      })
      .then(response => {
        console.log(response.data);
        this.setState({ Liquidacion: response.data });

        if (response.data != null && response.data.ProveedorId > 0) {
          this.ObtenerReservasLiquidadas(response.data.ProveedorId);
          this.ObtenerReservasPendientes(response.data.ProveedorId);
          this.ObtenerConsumoLiquidadas(response.data.ProveedorId);
          this.ObtenerConsumosPendientes(response.data.ProveedorId);
          this.ObtenerViandasLiquidadas(response.data.ProveedorId);
          this.ObtenerViandasPendientes(response.data.ProveedorId);
        }
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un error al consultar los detalles de la Liquidación")
        this.props.unlockScreen();
      });
  }

  ObtenerReservasPendientes = (ProveedorId) => {
    console.log(ProveedorId)
    this.props.blockScreen();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerReservasPendientesLiquidacion", {
        FechaDesde: this.state.Liquidacion.FechaDesde,
        FechaHasta: this.state.Liquidacion.FechaHasta,
        ProveedorId: ProveedorId
      })
      .then(response => {
        console.log(response.data);
        this.setState({ hospedajependientes: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un error al consultar Reservas pendientes de liquidación")
        this.props.unlockScreen();
      });

  }
  ObtenerReservasLiquidadas = (ProveedorId) => {

    console.log(ProveedorId)
    this.props.blockScreen();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerReservasLiquidadas", {
        FechaDesde: this.state.Liquidacion.FechaDesde,
        FechaHasta: this.state.Liquidacion.FechaHasta,
        ProveedorId: ProveedorId
      })
      .then(response => {
        console.log(response.data);
        this.setState({ hospedaje: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un error al consultar Reservas liquidadas")
        this.props.unlockScreen();
      });

  }
  ObtenerConsumosPendientes = (ProveedorId) => {
    console.log(ProveedorId)
    this.props.blockScreen();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerConsumosPendientesLiquidacion", {
        FechaDesde: this.state.Liquidacion.FechaDesde,
        FechaHasta: this.state.Liquidacion.FechaHasta,
        ProveedorId: ProveedorId
      })
      .then(response => {
        console.log(response.data);
        this.setState({ alimentacionpendientes: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un error al consultar Consumos pendientes de liquidación")
        this.props.unlockScreen();
      });

  }
  ObtenerConsumoLiquidadas = (ProveedorId) => {

    console.log(ProveedorId)
    this.props.blockScreen();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerConsumosLiquidadas", {
        FechaDesde: this.state.Liquidacion.FechaDesde,
        FechaHasta: this.state.Liquidacion.FechaHasta,
        ProveedorId: ProveedorId
      })
      .then(response => {
        console.log(response.data);
        this.setState({ alimentacion: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un error al consultar Consumos liquidadas")
        this.props.unlockScreen();
      });

  }

  ObtenerViandasPendientes = (ProveedorId) => {
    console.log(ProveedorId)
    this.props.blockScreen();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerViandasPendientesLiquidacion", {
        FechaDesde: this.state.Liquidacion.FechaDesde,
        FechaHasta: this.state.Liquidacion.FechaHasta,
        ProveedorId: ProveedorId
      })
      .then(response => {
        console.log(response.data);
        this.setState({ viandaspendientes: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un error al consultar Viandas pendientes de liquidación")
        this.props.unlockScreen();
      });

  }
  ObtenerViandasLiquidadas = (ProveedorId) => {

    console.log(ProveedorId)
    this.props.blockScreen();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerViandasLiquidadas", {
        FechaDesde: this.state.Liquidacion.FechaDesde,
        FechaHasta: this.state.Liquidacion.FechaHasta,
        ProveedorId: ProveedorId
      })
      .then(response => {
        console.log(response.data);
        this.setState({ viandas: response.data });
        this.props.unlockScreen();
      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Ocurrió un error al consultar Viandas liquidadas")
        this.props.unlockScreen();
      });

  }

  AgregarDetalleLiquidacion = () => {
    console.log(this.state.hospedajeseleccionadospendientes);
    if(this.state.hospedajeseleccionadospendientes.length==0){
      this.props.showWarn("Debe seleccionar al menos una fila")
    }else{

    console.log(document.getElementById("LiquidacionId").className)
    this.props.blockScreen();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerAgregarLiquidacionHospedaje", {
        id: document.getElementById("LiquidacionId").className,
        lista: this.state.hospedajeseleccionadospendientes
      })
      .then(response => {
        console.log(response.data);
        if (response.data === "NO_SELECCIONADOS") {
          this.props.showWarn("Debe seleccionar al menos una fila")
          this.props.unlockScreen();
        }
        if (response.data === "OK") {
          this.props.showSuccess("Items Agregados Correctamente");
          this.ObtenerDetalleLiquidacion();
          this.ObtenerReservasLiquidadas(this.state.Liquidacion.ProveedorId);
          this.ObtenerReservasPendientes(this.state.Liquidacion.ProveedorId);
          this.props.unlockScreen();
        }
        if (response.data === "ERROR") {
          this.props.showWarn("Existe un inconveniente intente más tarde")
          this.props.unlockScreen();
        }

      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Existe un inconveniente intente más tarde")
        this.props.unlockScreen();
      });
    }
  }

  RemoverDetalleLiquidacion = () => {


    console.log(document.getElementById("LiquidacionId").className)
    console.log(this.state.hospedajeseleccionados);
    if (this.state.hospedajeseleccionados.length == 0) {
      this.props.showWarn("Debe seleccionar al menos una fila")
    } else {


      this.props.blockScreen();


      axios
        .post("/Proveedor/LiquidacionServicio/ObtenerRemoveLiquidacionesHospedaje", {
          id: document.getElementById("LiquidacionId").className,
          lista: this.state.hospedajeseleccionados
        })
        .then(response => {
          console.log(response.data);
          if (response.data === "NO_SELECCIONADOS") {
            this.props.showWarn("Debe seleccionar al menos una fila")
            this.props.unlockScreen();
          }
          if (response.data === "OK") {
            this.props.showSuccess("Items Agregados Correctamente");
            this.ObtenerDetalleLiquidacion();
            this.ObtenerReservasLiquidadas(this.state.Liquidacion.ProveedorId);
            this.ObtenerReservasPendientes(this.state.Liquidacion.ProveedorId);
       
          }
          if (response.data === "ERROR") {
            this.props.showWarn("Existe un inconveniente intente más tarde")
            this.props.unlockScreen();
          }

        })
        .catch(error => {
          console.log(error);
          this.props.showWarn("Existe un inconveniente intente más tarde")
          this.props.unlockScreen();
        });

    }
  }

  AgregarConsumoLiquidacion = () => {
    console.log(this.state.alimentacionseleccionadospendientes);
    if(this.state.alimentacionseleccionadospendientes.length==0){
      this.props.showWarn("Debe seleccionar al menos una fila")
    }else{

    console.log(document.getElementById("LiquidacionId").className)
    this.props.blockScreen();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerAgregarLiquidacionConsumo", {
        id: document.getElementById("LiquidacionId").className,
        lista: this.state.alimentacionseleccionadospendientes
      })
      .then(response => {
        console.log(response.data);
        if (response.data === "NO_SELECCIONADOS") {
          this.props.showWarn("Debe seleccionar al menos una fila")
          this.props.unlockScreen();
        }
        if (response.data === "OK") {
          this.props.showSuccess("Items Agregados Correctamente");
          this.ObtenerDetalleLiquidacion();
          this.ObtenerConsumoLiquidadas(this.state.Liquidacion.ProveedorId);
          this.ObtenerConsumosPendientes(this.state.Liquidacion.ProveedorId);
          this.props.unlockScreen();
        }
        if (response.data === "ERROR") {
          this.props.showWarn("Existe un inconveniente intente más tarde")
          this.props.unlockScreen();
        }

      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Existe un inconveniente intente más tarde")
        this.props.unlockScreen();
      });
    }
  }

  RemoverConsumoLiquidacion = () => {


    console.log(document.getElementById("LiquidacionId").className)
    console.log(this.state.alimentacionseleccionados);
    if (this.state.alimentacionseleccionados.length == 0) {
      this.props.showWarn("Debe seleccionar al menos una fila")
    } else {


      this.props.blockScreen();


      axios
        .post("/Proveedor/LiquidacionServicio/ObtenerRemoveLiquidacionesConsumo", {
          id: document.getElementById("LiquidacionId").className,
          lista: this.state.alimentacionseleccionados
        })
        .then(response => {
          console.log(response.data);
          if (response.data === "NO_SELECCIONADOS") {
            this.props.showWarn("Debe seleccionar al menos una fila")
            this.props.unlockScreen();
          }
          if (response.data === "OK") {
            this.props.showSuccess("Items Agregados Correctamente");
            this.ObtenerDetalleLiquidacion();
            this.ObtenerConsumoLiquidadas(this.state.Liquidacion.ProveedorId);
            this.ObtenerConsumosPendientes(this.state.Liquidacion.ProveedorId);
       
          }
          if (response.data === "ERROR") {
            this.props.showWarn("Existe un inconveniente intente más tarde")
            this.props.unlockScreen();
          }

        })
        .catch(error => {
          console.log(error);
          this.props.showWarn("Existe un inconveniente intente más tarde")
          this.props.unlockScreen();
        });

    }
  }

  AgregarViandaLiquidacion = () => {
    console.log(this.state.viandasseleccionadospendientes);
    if(this.state.viandasseleccionadospendientes.length==0){
      this.props.showWarn("Debe seleccionar al menos una fila")
    }else{

    console.log(document.getElementById("LiquidacionId").className)
    this.props.blockScreen();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerAgregarLiquidacionVianda", {
        id: document.getElementById("LiquidacionId").className,
        lista: this.state.viandasseleccionadospendientes
      })
      .then(response => {
        console.log(response.data);
        if (response.data === "NO_SELECCIONADOS") {
          this.props.showWarn("Debe seleccionar al menos una fila")
          this.props.unlockScreen();
        }
        if (response.data === "OK") {
          this.props.showSuccess("Items Agregados Correctamente");
          this.ObtenerDetalleLiquidacion();
          this.ObtenerViandasLiquidadas(this.state.Liquidacion.ProveedorId);
          this.ObtenerViandasPendientes(this.state.Liquidacion.ProveedorId);
          this.props.unlockScreen();
        }
        if (response.data === "ERROR") {
          this.props.showWarn("Existe un inconveniente intente más tarde")
          this.props.unlockScreen();
        }

      })
      .catch(error => {
        console.log(error);
        this.props.showWarn("Existe un inconveniente intente más tarde")
        this.props.unlockScreen();
      });
    }
  }

  RemoverViandaLiquidacion = () => {


    console.log(document.getElementById("LiquidacionId").className)
    console.log(this.state.alimentacionseleccionados);
    if (this.state.alimentacionseleccionados.length == 0) {
      this.props.showWarn("Debe seleccionar al menos una fila")
    } else {


      this.props.blockScreen();


      axios
        .post("/Proveedor/LiquidacionServicio/RemoverLiquidacionVianda", {
          id: document.getElementById("LiquidacionId").className,
          lista: this.state.alimentacionseleccionados
        })
        .then(response => {
          console.log(response.data);
          if (response.data === "NO_SELECCIONADOS") {
            this.props.showWarn("Debe seleccionar al menos una fila")
            this.props.unlockScreen();
          }
          if (response.data === "OK") {
            this.props.showSuccess("Items Agregados Correctamente");
            this.ObtenerDetalleLiquidacion();
            this.ObtenerViandasLiquidadas(this.state.Liquidacion.ProveedorId);
            this.ObtenerViandasPendientes(this.state.Liquidacion.ProveedorId);
       
          }
          if (response.data === "ERROR") {
            this.props.showWarn("Existe un inconveniente intente más tarde")
            this.props.unlockScreen();
          }

        })
        .catch(error => {
          console.log(error);
          this.props.showWarn("Existe un inconveniente intente más tarde")
          this.props.unlockScreen();
        });

    }
  }
  handleSubmit = () => {

    axios.get("/Proveedor/LiquidacionServicio/ObtenerRViandas", {
      params: {
        id: this.state.Liquidacion.Id
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

  handleSubmitAlimentación = () => {

    axios.get("/Proveedor/LiquidacionServicio/ObtenerRAlimentacion", {
      params: {
        id: this.state.Liquidacion.Id
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

  

  RedireccionarDetalle() {
    window.location.href = "/Proveedor/LiquidacionServicio/IndexLiquidacion/";
  }

  RenderTabs = () => {
    if (this.state.Liquidacion != null && this.state.Liquidacion.NombreTipoServicio == "HOSPEDAJE") {
      return (
        <div>
          <li className="nav-item">
            <a
              className="nav-link active"
              id="gestion-tab"
              data-toggle="tab"
              href="#gestion"
              role="tab"
              aria-controls="home"
              aria-expanded="true"
            >
              Hospedaje <i className="fa fa-hotel fa-1x" />
            </a>
          </li>
        </div>
      )
    } else {
      return (
        <div>
          <li className="nav-item">
            <a
              className="nav-link active"
              id="gestion-tab"
              data-toggle="tab"
              href="#gestion"
              role="tab"
              aria-controls="home"
              aria-expanded="true"
            >
              Hospedaje <i className="fa fa-hotel fa-1x" />
            </a>
          </li>
          <li className="nav-item">
            <a
              className="nav-link"
              id="op-tab"
              data-toggle="tab"
              href="#op"
              role="tab"
              aria-controls="home"
              aria-expanded="true"
            >
              Alimentación <i className="fa fa-cutlery fa-1x" />
            </a>
          </li>
          <li className="nav-item">
            <a
              className="nav-link"
              id="vi-tab"
              data-toggle="tab"
              href="#vi"
              role="tab"
              aria-controls="home"
              aria-expanded="true"
            >
              Viandas <i className="fa fa-shopping-bag fa-1x" />
            </a>
          </li>
        </div>
      )
    }

  }

  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={el => {
            this.growl = el;
          }}
          baseZIndex={1000}
        />
        <div className="content-section implementation">
          <Card>
            <div>
              <div className="row">
                <div className="col-4" />

                <div className="col-8" align="right">
                {this.state.Liquidacion!=null && this.state.Liquidacion.NombreTipoServicio==="ALIMENTACION" &&
                  <button
                    className="btn btn-outline-primary"
                    style={{ marginLeft: "0.3em" }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Reporte Masivo"
                    onClick={() => this.handleSubmitAlimentación()}
                  >Reporte Alimentación
                  </button>
                  }


                  {this.state.Liquidacion!=null && this.state.Liquidacion.NombreTipoServicio==="VIANDAS" &&
                  <button
                    className="btn btn-outline-primary"
                    style={{ marginLeft: "0.3em" }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Reporte Masivo"
                    onClick={() => this.handleSubmit()}
                  >Reporte Viandas
                  </button>
                  }
                  <button
                    className="btn btn-outline-primary"
                    style={{ marginLeft: "0.3em" }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Reporte Masivo"
                    onClick={() => this.RedireccionarDetalle()}
                  >Regresar
                  </button>
                </div>
              </div>
              <br />
              <div className="row">
                <div className="col-6">
                  <h6>
                    <b>Proveedor:</b>{" "}{this.state.Liquidacion != null ? this.state.Liquidacion.NombreContratoProveedor : ""}
                  </h6>
                  <h6>
                    <b>Fecha Desde:</b>{" "}{this.state.Liquidacion != null ? this.state.Liquidacion.FormatFechaDesde : ""}
                  </h6>
                  <h6>
                    <b>Fecha Hasta:</b>{" "}{this.state.Liquidacion != null ? this.state.Liquidacion.FormatFechaHasta : ""}
                  </h6>
                  <h6>
                    <b>Fecha Pago:</b>{" "}{this.state.Liquidacion != null ? this.state.Liquidacion.FormatFechaPago : ""}
                  </h6>
                </div>

                <div className="col-6">
                  <h6>
                    <b>Código:</b>{" "}{this.state.Liquidacion != null ? this.state.Liquidacion.Codigo : ""}
                  </h6>
                  <h6>
                    <b>Estado:</b>{" "}{this.state.Liquidacion != null ? this.state.Liquidacion.NombreEstado : ""}
                  </h6>
                  <h6>
                    <b>Tipo Servicio:</b>{" "}{this.state.Liquidacion != null ? this.state.Liquidacion.NombreTipoServicio : ""}
                  </h6>
                  <h6>
                    <div className="callout callout-info">
                      <small className="text-muted">Monto Consumido:</small>
                      <br />
                      <strong className="h4">
                        <CurrencyFormat
                          value={this.state.Liquidacion != null ? this.state.Liquidacion.MontoConsumido : 0}
                          displayType={"text"}
                          thousandSeparator={true}
                          prefix={"$"}
                        />
                      </strong>
                    </div>
                  </h6>

                </div>
              </div>

            </div>
          </Card>
        </div>
        <br />
        <div className="content-section implementation">
          <Card>
            <div>
              <ul className="nav nav-tabs" id="gestion_tabs" role="tablist">
                <li className="nav-item">
                  <a
                    className="nav-link active"
                    id="gestion-tab"
                    data-toggle="tab"
                    href="#gestion"
                    role="tab"
                    aria-controls="home"
                    aria-expanded="true"
                  >
                    Hospedaje <i className="fa fa-hotel fa-1x" />
                  </a>
                </li>
                <li className="nav-item">
                  <a
                    className="nav-link"
                    id="op-tab"
                    data-toggle="tab"
                    href="#op"
                    role="tab"
                    aria-controls="home"
                    aria-expanded="true"
                  >
                    Alimentación <i className="fa fa-cutlery fa-1x" />
                  </a>
                </li>
                <li className="nav-item">
                  <a
                    className="nav-link"
                    id="vi-tab"
                    data-toggle="tab"
                    href="#vi"
                    role="tab"
                    aria-controls="home"
                    aria-expanded="true"
                  >
                    Viandas <i className="fa fa-shopping-bag fa-1x" />
                  </a>
                </li>
                {/*this.RenderTabs()*/}
              </ul>
              <div className="tab-content" id="myTabContent">
                <div
                  className="tab-pane fade show active"
                  id="gestion"
                  role="tabpanel"
                  aria-labelledby="gestion-tab"
                >
                  <Accordion>
                    <AccordionTab header="Liquidados">
                      <div align="right">
                        <button
                          type="button"
                          className="btn btn-outline-primary btn-sm"
                          icon="fa fa-fw fa-ban"
                          onClick={this.RemoverDetalleLiquidacion}
                        >
                          Eliminar Items
                      </button>
                      </div>
                      <br />
                      <DataTable value={this.state.hospedaje} paginator={true} rows={10} rowsPerPageOptions={[5, 10, 20, 30]}
                        selection={this.state.hospedajeseleccionados} onSelectionChange={e => this.setState({ hospedajeseleccionados: e.value })}>
                        <Column selectionMode="multiple" style={{ width: '5em', fontSize: '10px' }} />
                        <Column field="Id" header="N°" style={{ fontSize: '10px' }} />
                        <Column field="NombreProveedor" header="Proveedor" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="FechaConsumo" header="F. Consumo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Legajo" header="Legajo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Identificacion" header="Identificación" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Nombres" header="Nombres" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Cargo" header="Cargo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Habitacion" header="Habitación" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Espacio" header="Espacio" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Tipo" header="Tipo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="FechaSalida" header="F. Salida" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Tarifa" header="Tarifa" filter={true} style={{ fontSize: '10px' }} />
                      </DataTable>

                    </AccordionTab>
                    <AccordionTab header="Pendientes de Liquidación">
                      <div align="right">
                        <button
                          type="button"
                          className="btn btn-outline-primary btn-sm"
                          icon="fa fa-fw fa-ban"
                          onClick={this.AgregarDetalleLiquidacion}
                        >
                          Agregar Items a Liquidación
                      </button>
                      </div>
                      <br />
                      <DataTable value={this.state.hospedajependientes} paginator={true} rows={10} rowsPerPageOptions={[5, 10, 20, 30]}
                        selection={this.state.hospedajeseleccionadospendientes} onSelectionChange={e => this.setState({ hospedajeseleccionadospendientes: e.value })}>
                        <Column selectionMode="multiple" style={{ width: '5em', fontSize: '10px' }} />
                        <Column field="Id" header="N°" style={{ fontSize: '10px' }} />
                        <Column field="NombreProveedor" header="Proveedor" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="FechaConsumo" header="F. Consumo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Legajo" header="Legajo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Identificacion" header="Identificación" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Nombres" header="Nombres" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Cargo" header="Cargo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Habitacion" header="Habitación" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Espacio" header="Espacio" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Tipo" header="Tipo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="FechaSalida" header="F. Salida" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Tarifa" header="Tarifa" filter={true} style={{ fontSize: '10px' }} />
                      </DataTable>
                    </AccordionTab>

                  </Accordion>


                </div>
                <div
                  className="tab-pane fade"
                  id="op"
                  role="tabpanel"
                  aria-labelledby="op-tab"
                >
                  <Accordion>
                    <AccordionTab header="Liquidados">
                    <div align="right">
                        <button
                          type="button"
                          className="btn btn-outline-primary btn-sm"
                          icon="fa fa-fw fa-ban"
                          onClick={this.RemoverConsumoLiquidacion}
                        >
                          Eliminar Items
                      </button>
                      </div>
                      <br />
                      <DataTable value={this.state.alimentacion} paginator={true} rows={10} rowsPerPageOptions={[5, 10, 20, 30]}
                        selection={this.state.alimentacionseleccionados} onSelectionChange={e => this.setState({ alimentacionseleccionados: e.value })}>
                        <Column selectionMode="multiple" style={{ width: '5em', fontSize: '10px' }} />
                        <Column field="Id" header="N°" style={{ fontSize: '10px' }} />
                        <Column field="NombreProveedor" header="Proveedor" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="FechaConsumo" header="F. Consumo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Legajo" header="Legajo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Identificacion" header="Identificación" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Nombres" header="Nombres" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Cargo" header="Cargo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="TipoComida" header="Tipo Comida" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="OpcionComida" header="Opcion Comida" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Tarifa" header="Tarifa" style={{ fontSize: '10px' }} />
                      </DataTable>
                    </AccordionTab>
                    <AccordionTab header="Pendientes de Liquidación">
                    <div align="right">
                        <button
                          type="button"
                          className="btn btn-outline-primary btn-sm"
                          icon="fa fa-fw fa-ban"
                          onClick={this.AgregarConsumoLiquidacion}
                        >
                          Agregar Items a Liquidación
                      </button>
                      </div>
                      <br />
                      <DataTable value={this.state.alimentacion} paginator={true} rows={10} rowsPerPageOptions={[5, 10, 20, 30]}
                        selection={this.state.alimentacionseleccionados} onSelectionChange={e => this.setState({ alimentacionseleccionados: e.value })}>
                        <Column selectionMode="multiple" style={{ width: '3em', fontSize: '10px' }} />
                        <Column field="Id" header="N°" style={{ fontSize: '10px' }} />
                        <Column field="NombreProveedor" header="Proveedor" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="FechaConsumo" header="F. Consumo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Legajo" header="Legajo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Identificacion" header="Identificación" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Nombres" header="Nombres" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Cargo" header="Cargo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="TipoComida" header="Tipo Comida" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="OpcionComida" header="Opcion Comida" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Tarifa" header="Tarifa" style={{ fontSize: '10px' }} />
                      </DataTable>
                    </AccordionTab>

                  </Accordion>


                </div>
                <div
                  className="tab-pane fade"
                  id="vi"
                  role="tabpanel"
                  aria-labelledby="vi-tab"
                >
                  <Accordion>
                    <AccordionTab header="Liquidados">
                    <div align="right">
                        <button
                          type="button"
                          className="btn btn-outline-primary btn-sm"
                          icon="fa fa-fw fa-ban"
                          onClick={this.RemoverViandaLiquidacion}
                        >
                          Eliminar Items
                      </button>
                      </div>
                      <br />
                      <DataTable value={this.state.viandas} paginator={true} rows={10} rowsPerPageOptions={[5, 10, 20, 30]}
                        selection={this.state.viandasseleccionados} onSelectionChange={e => this.setState({ viandasseleccionados: e.value })}>
                        <Column selectionMode="multiple" style={{ width: '3em', fontSize: '10px' }} />
                        <Column field="Id" header="N°" style={{ fontSize: '10px' }} />
                        <Column field="NombreProveedor" header="Proveedor" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="FechaPedido" header="F. Pedido" filter={true}  style={{fontSize: '10px'}}/>
                        <Column field="FechaConsumo" header="F. Consumo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Locacion" header="Locación" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="IdSolicitante" header="ID Solicitante" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="NombreSolicitante" header="Nombres" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="TotalSolicitado" header="Total Solicitado" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Tarifa" header="Tarifa" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Total" header="Total" filter={true} style={{ fontSize: '10px' }} />
                      </DataTable>

                    </AccordionTab>
                    <AccordionTab header="Pendientes de Liquidación">
                    <div align="right">
                        <button
                          type="button"
                          className="btn btn-outline-primary btn-sm"
                          icon="fa fa-fw fa-ban"
                          onClick={this.AgregarViandaLiquidacion}
                        >
                          Agregar Items a Liquidación
                      </button>
                      </div>
                      <br />
                      <DataTable value={this.state.viandas} paginator={true} rows={10} rowsPerPageOptions={[5, 10, 20, 30]}
                        selection={this.state.viandasseleccionados} onSelectionChange={e => this.setState({ viandasseleccionados: e.value })}>
                        <Column selectionMode="multiple" style={{ width: '3em', fontSize: '10px' }} />
                        <Column field="Id" header="N°" style={{ fontSize: '10px' }} />
                        <Column field="NombreProveedor" header="Proveedor" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="FechaPedido" header="F. Pedido" filter={true}  style={{fontSize: '10px'}}/>
                        
                        <Column field="FechaConsumo" header="F. Consumo" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Locacion" header="Locación" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="IdSolicitante" header="ID Solicitante" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="NombreSolicitante" header="Nombres" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="TotalSolicitado" header="Total Solicitado" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Tarifa" header="Tarifa" filter={true} style={{ fontSize: '10px' }} />
                        <Column field="Total" header="Total" filter={true} style={{ fontSize: '10px' }} />
                      </DataTable>
                    </AccordionTab>

                  </Accordion>


                </div>
              </div>
            </div>
          </Card>
        </div>
      </BlockUi>
    );
  }

}
const Container = Wrapper(LiquidacionServicioDetails);
ReactDOM.render(<Container />, document.getElementById("content"));
