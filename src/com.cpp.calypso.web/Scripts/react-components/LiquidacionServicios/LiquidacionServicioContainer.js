import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import Field from "../Base/Field-v2";
import Wrapper from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { DataTable } from 'primereact-v2/datatable';
import { Column } from 'primereact-v2/column';
import { Growl } from "primereact/components/growl/Growl";
import { Card } from 'primereact-v2/card';
import CurrencyFormat from "react-currency-format";
import { Accordion, AccordionTab } from "primereact-v2/accordion";

class LiquidacionServicioContainer extends React.Component {
  constructor(props) {
    super(props);
    //nuevos
    this.state = {
      blocking: false,
      data: [],
      visible: false,
      visibleeditar: false,
      errors: {},

      //LIQUIDACION SERVICIOS//
      //CABECERA
      FechaDesde:"",
      FechaHasta:"",
      Proveedores: [],
      ProveedorId: 0,

      MontoHospedaje: 0.0,
      MontoAlimentacion: 0.0,
      MontoViandas: 0.0,

      hospedaje: [],
      alimentacion: [],
      viandas: [],
      
      hospedajeseleccionados:[],
      alimentacionseleccionados:[],
      viandasseleccionados:[],

      
      // Inputs del Formulario
      Identificador: 0,
      lugares: [],
      Codigo: "",
      Nombre: "",
      OrigenId: 0,
      DestinoId: 0,
      Distancia: 0.0,
      Duracion: 0,
      Sector: "",
      Descripcion: "",

      EstadoId: 0,
      //Horario
      RutaId: 0,
      Rutas: [],
      Horarioid: 0,
      Horarios: [],
      HorarioName: "",
      NombreDestino: "",
      NombreOrigen: "",
      Vehiculos: [],
      VehiculoId: 0,
      Observacion: "",

      horas: ["00:00"],
      TipoVehiculo: ""
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleChangeCorreo = this.handleChangeCorreo.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.handleChangeIdentificacion = this.handleChangeIdentificacion.bind(
      this
    );

    //LIQUIDACION SERVICIOS
    this.onChangeValueProveedor = this.onChangeValueProveedor.bind(this);

    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);
    this.OcultarFormularioEditar = this.OcultarFormularioEditar.bind(this);
    this.MostrarFormularioEditar = this.MostrarFormularioEditar.bind(this);
    this.alertMessage = this.alertMessage.bind(this);
    this.infoMessage = this.infoMessage.bind(this);
    this.Loading = this.Loading.bind(this);
    this.StopLoading = this.StopLoading.bind(this);

    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.EnviarFormularioEditar = this.EnviarFormularioEditar.bind(this);

    //METODOS
    this.ObtenerRutas = this.ObtenerRutas.bind(this);
    this.ObtenerDetalleRuta = this.ObtenerDetalleRuta.bind(this);
    this.ObtenerCatalogos = this.ObtenerCatalogos.bind(this);
    this.generarBotones = this.generarBotones.bind(this);
    this.EliminarRuta = this.EliminarRuta.bind(this);
    this.RedireccionarDetalle = this.RedireccionarDetalle.bind(this);
    this.onChangeValueHorario = this.onChangeValueHorario.bind(this);

    //
    this.onChangeValueRuta = this.onChangeValueRuta.bind(this);
    this.ObtenerRutasHorarios = this.ObtenerRutasHorarios.bind(this);
  }

  componentDidMount() {
    this.ObtenerCatalogos();
    this.props.unlockScreen();
  }

  VaciarCampos =()=> {
    this.setState({
      FechaDesde:"",
      FechaHasta:"",
      ProveedorId: 0,
      hospedajeseleccionados:[],
      alimentacionseleccionados:[],
      viandasseleccionados:[],
      hospedaje:[],
      alimentacion:[],
      viandas:[]
    });
  }
  ObtenerReservasPendientes= (value) =>{

    console.log(value)
    this.props.blockScreen();
    axios
    .post("/Proveedor/LiquidacionServicio/ObtenerReservasPendientesLiquidacion", {
      FechaDesde:this.state.FechaDesde,
      FechaHasta:this.state.FechaHasta,
      ProveedorId: value
    })
    .then(response => {
      console.log(response.data);
      this.setState({ hospedaje: response.data});
      this.props.unlockScreen();
    })
    .catch(error => {
      console.log(error);
      this.props.showWarn("Ocurrió un error al consultar Reservas pendientes de liquidación")
      this.props.unlockScreen();
    });

  }
  ObtenerConsumosPendientes= (value) =>{

    console.log(value)
    this.props.blockScreen();
    axios
    .post("/Proveedor/LiquidacionServicio/ObtenerConsumosPendientesLiquidacion", {
      FechaDesde:this.state.FechaDesde,
      FechaHasta:this.state.FechaHasta,
      ProveedorId: value
    })
    .then(response => {
      console.log(response.data);
      this.setState({ alimentacion: response.data});
      this.props.unlockScreen();
    })
    .catch(error => {
      console.log(error);
      this.props.showWarn("Ocurrió un error al consultar Consumos pendientes de liquidación")
      this.props.unlockScreen();
    });

  }

  ObtenerViandasPendientes= (value) =>{

    console.log(value)
    this.props.blockScreen();
    axios
    .post("/Proveedor/LiquidacionServicio/ObtenerViandasPendientesLiquidacion", {
      FechaDesde:this.state.FechaDesde,
      FechaHasta:this.state.FechaHasta,
      ProveedorId: value
    })
    .then(response => {
      console.log(response.data);
      this.setState({ viandas: response.data});
      this.props.unlockScreen();
    })
    .catch(error => {
      console.log(error);
      this.props.showWarn("Ocurrió un error al consultar Viandas pendientes de liquidación")
      this.props.unlockScreen();
    });

  }

  GenerarLiquidaciones=()=>{
    if(this.state.hospedajeseleccionados.length>0){
  this.GenerarLiquidacionHospedaje();
    }
    if(this.state.alimentacionseleccionados.length>0 || this.state.viandasseleccionados.length>0){
  this.GenerarLiquidacionConsumo();
}

  if(this.state.hospedajeseleccionados.length==0 &&
     this.state.alimentacionseleccionados.length==0 &&
     this.state.viandasseleccionados.length==0
     )
     {
    this.props.showWarning("Ninguna Fila de Detalles Seleccionada")
  }
  }

  GenerarLiquidacionHospedaje= () =>{
    if(this.state.hospedajeseleccionados.length>0){
    this.props.blockScreen();
    axios
    .post("/Proveedor/LiquidacionServicio/Create", {
      FechaDesde:this.state.FechaDesde,
      FechaHasta:this.state.FechaHasta,
      ProveedorId: this.state.ProveedorId,
      hospedaje:this.state.hospedajeseleccionados
    })
    .then(response => {
      console.log(response.data);
      if(response.data!="OK"){
        this.props.showWarn(response.data);
        this.props.unlockScreen();
      }else{
        this.props.showSuccess("Liquidación de Hospedaje Generado Correctamente");
        this.setState({
          hospedajeseleccionados:[],
          hospedaje:[],
        });
        this.props.unlockScreen();
      }
          
    })
    .catch(error => {
      console.log(error);
      this.props.showWarn("Ocurrió un error al consultar Reservas pendientes de liquidación")
      this.props.unlockScreen();
    });
  }else{

    this.props.showWarning("Ninguna Fila de Detalles de Hospedaje Seleccionado")
  }
  } 
  GenerarLiquidacionConsumo= () =>{
    if(this.state.alimentacionseleccionados.length>0 || this.state.viandasseleccionados.length>0){
    this.props.blockScreen();
    axios
    .post("/Proveedor/LiquidacionServicio/CreateCosumo", {
      FechaDesde:this.state.FechaDesde,
      FechaHasta:this.state.FechaHasta,
      ProveedorId: this.state.ProveedorId,
      consumo:this.state.alimentacionseleccionados,
      viandas:this.state.viandasseleccionados
    })
    .then(response => {
      console.log(response.data);
      if(response.data!="OK"){
        this.props.showWarn(response.data);
        this.props.unlockScreen();
      }else{
        this.props.showSuccess("Liquidación de Consumo Generado Correctamente");
        this.setState({
          alimentacionseleccionados:[],
          alimentacion:[],
        });
        this.props.unlockScreen();
      }
          
    })
    .catch(error => {
      console.log(error);
      this.props.showWarn("Error al generar  Liquidación de Consumo")
      this.props.unlockScreen();
    });
  }else{

    this.props.showWarning("Ninguna Fila de Detalles de Consumo Seleccionado")
  }
  }
  





  ObtenerRutas() {
    axios
      .post("/Transporte/RutaHorarioVehiculo/ListaRutaHorasVehiculo", {})
      .then(response => {
        console.log(response.data.result);

        this.setState({ data: response.data.result, blocking: false });
      })
      .catch(error => {
        console.log(error);
      });
  }

  ObtenerRutasHorarios(value) {
    console.log(this.state.RutaId);
    console.log(value);
    if (this.state.RutaId > 0 && value > 0) {
      axios
        .post("/Transporte/RutaHorarioVehiculo/ListaByRutaHorario", {
          rutaid: this.state.RutaId,
          horarioid: value
        })
        .then(response => {
          console.log(response.data.result);

          this.setState({ data: response.data.result, blocking: false });
        })
        .catch(error => {
          console.log(error);
        });
    }
  }
  MoneyFormat=(rowData, column)=>{
    return <CurrencyFormat value={rowData['Tarifa']} displayType={'text'} thousandSeparator={true} prefix={'$'} />
  }


  ObtenerDetalleRuta(Id) {
    console.log(Id);
    axios
      .post("/Transporte/Ruta/ObtenerDetallesRuta", {
        Id: Id
      })
      .then(response => {
        if (response.data != "Error") {
          console.log(response.data);
          this.setState({
            Identificador: Id,
            Codigo: response.data.Codigo,
            Nombre: response.data.Nombre,
            OrigenId: response.data.OrigenId,
            DestinoId: response.data.DestinoId,
            Distancia: response.data.Distancia,
            Duracion: response.data.Duracion,
            Sector: response.data.Sector,
            Descripcion: response.data.Descripcion,
            NombreDestino: response.data.Destino.Nombre,
            NombreOrigen: response.data.Origen.Nombre,
            EstadoId: 0,
            blocking: false
          });
        }
      })
      .catch(error => {
        console.log(error);
      });
  }

  ObtenerCatalogos() {
    this.Loading();
    axios
      .post("/Proveedor/LiquidacionServicio/ObtenerProveedores", {})
      .then(response => {
        console.log(response.data);
        var items = response.data.map(item => {
          return { label: item.razon_social, dataKey: item.Id, value: item.Id };
        });
        this.setState({ Proveedores: items, blocking: false });
        this.StopLoading();
      })
      .catch(error => {
        console.log(error);
        this.StopLoading();
      });
  }
  onChangeValue = (name, value) => {
    this.setState({
      [name]: value
    });
  };
  onChangeValueProveedor = (name, value) => {
    
    if(this.state.FechaDesde ===""){
      this.setState({
        [name]: 0,
        hospedajeseleccionados:[],
        alimentacionseleccionados:[],
        viandasseleccionados:[],
        hospedaje:[],
        alimentacion:[],
        viandas:[]
      });
    this.props.showWarn("Campo Obligatorio Fecha Desde");
    }else if(this.state.FechaHasta ===""){
      this.setState({
        [name]: 0,
        hospedajeseleccionados:[],
        alimentacionseleccionados:[],
        viandasseleccionados:[],
        hospedaje:[],
        alimentacion:[],
        viandas:[]
      });
      this.props.showWarn("Campo Obligatorio Fecha Hasta");
    }else if(this.state.FechaHasta !="" && this.state.FechaHasta<this.state.FechaDesde){
      this.setState({
        [name]: 0,
        hospedajeseleccionados:[],
        alimentacionseleccionados:[],
        viandasseleccionados:[],
        hospedaje:[],
        alimentacion:[],
        viandas:[]
      });
      this.props.showWarn("Fecha Hasta debe ser mayor a Fecha Desde");
    }else{
    
    this.setState({
      [name]: value,
      hospedajeseleccionados:[],
        alimentacionseleccionados:[],
        viandasseleccionados:[],
        hospedaje:[],
        alimentacion:[],
        viandas:[]
    });
    this.setState({MontoHospedaje:0,MontoAlimentacion:0,MontoViandas:0});
    this.ObtenerReservasPendientes(value);
    this.ObtenerConsumosPendientes(value);
    this.ObtenerViandasPendientes(value);
  }
  };

  onChangeValueHorario = (name, value) => {
    this.setState({
      [name]: value
    });

    this.ObtenerRutasHorarios(value);
  };

  onChangeValueRuta = (name, value) => {
    this.setState({
      [name]: value
    });
    console.log(value);
    this.ObtenerDetalleRuta(value);
    axios
      .post("/Transporte/Ruta/ObtenerHorariosRuta", {
        id: value
      })
      .then(response => {
        console.log(response.data);

        var items = response.data.map(item => {
          return { label: item.Horario, dataKey: item.Id, value: item.Id };
        });
        this.setState({
          Horarioid: 0,
          Horarios: items,
          blocking: false,
          data: [],
          VehiculoId: 0
        });
        this.StopLoading();
      })
      .catch(error => {
        console.log(error);
        this.StopLoading();
      });
  };

  RedireccionarDetalle() {
     window.location.href = "/Proveedor/LiquidacionServicio/IndexLiquidacion/";
  }
  EliminarRuta(Id) {
    console.log(Id);
    axios
      .post("/Transporte/RutaHorarioVehiculo/Delete", {
        Id: Id
      })
      .then(response => {
        if (response.data != "Error") {
          this.infoMessage("Eliminado Correctamente");
          this.ObtenerRutasHorarios(this.state.Horarioid);
        }
      })
      .catch(error => {
        console.log(error);
        this.alertMessage("Ocurrió un Error");
      });
  }
  montosHoteles=(e)=>{
    this.setState({ hospedajeseleccionados: e.value });
    console.log(e.value )
    if(e.value.length>0){
  this.setState({MontoHospedaje:e.value.map(item => item.Tarifa).reduce((prev, next) => prev + next)});
}else{
  this.setState({MontoHospedaje:0.0});

  
}
  }
  montosAlimentacion=(e)=>{
    this.setState({ alimentacionseleccionados: e.value });
    console.log(e.value )
    if(e.value.length>0){
  this.setState({MontoAlimentacion:e.value.map(item => item.Tarifa).reduce((prev, next) => prev + next)});
}else{
  this.setState({MontoAlimentacion:0.0});

  
}}
montosViandas=(e)=>{
  this.setState({ viandasseleccionados: e.value });
  console.log(e.value )
  if(e.value.length>0){
this.setState({MontoViandas:e.value.map(item => item.Total).reduce((prev, next) => prev + next)});
}else{
this.setState({MontoViandas:0.0});


}
  }


  generarBotones = (cell, row) => {
    return (
      <div>
        {/* <button
          className="btn btn-outline-success"
          onClick={() => this.RedireccionarDetalle(row.Id)}
        >
          <i className="fa fa-eye" />
        </button>*/}
        <button
          className="btn btn-outline-info"
          style={{ marginLeft: "0.3em" }}
          onClick={() => this.MostrarFormularioEditar(row)}
        >
          <i className="fa fa-edit" />
        </button>
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
  };

  onSelectAll = isSelected => {
    if (isSelected) {
      return products.map(row => row.id);
    } else {
      return [];
    }
  };

  render() {
    const selectRowProp = {
      mode: "checkbox",
      clickToSelect: true,
      onSelectAll: this.onSelectAll
    };
    
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
                <button
                    className="btn btn-outline-primary"
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Vaciar Campos"
                    onClick={this.VaciarCampos}
                  ><i className="fa fa-refresh" /> Vaciar
                  
                  </button>{" "}
                  <button
                    className="btn btn-outline-indigo "
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Generar Liquidaciones"
                    onClick={() => this.GenerarLiquidaciones()}
                  ><i className="fa fa-share" /> Generar
                  
                  </button>{" "}
                  <button
                    className="btn btn-outline-primary"
                    style={{ marginLeft: "0.3em" }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Regresar"
                    onClick={() => this.RedireccionarDetalle()}
                  >Regresar
                  </button>
                </div>
              </div>

              <div className="row">
                <div className="col-4">
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
                <div className="col-4">
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
                <div className="col-4">
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
              
              </div>
              <br />
              <div className="row">
                <div className="col-sm-5 col-md-3">
                  <div className="callout callout-info">
                    <small className="text-muted">Hospedaje</small>
                    <br />
                    <strong className="h4">
                      <CurrencyFormat
                        value={this.state.MontoHospedaje}
                        displayType={"text"}
                        thousandSeparator={true}
                        prefix={"$"}
                      />
                    </strong>
                  </div>
                </div>
                <div className="col-sm-5 col-md-3">
                  <div className="callout callout-danger">
                    <small className="text-muted">Alimentación</small>
                    <br />
                    <strong className="h4">
                      <CurrencyFormat
                        value={this.state.MontoAlimentacion}
                        displayType={"text"}
                        thousandSeparator={true}
                        prefix={"$"}
                      />
                    </strong>
                  </div>
                </div>
                <div className="col-sm-5 col-md-3">
                  <div className="callout callout-warning">
                    <small className="text-muted">Viandas</small>
                    <br />
                    <strong className="h4">
                      <CurrencyFormat
                        value={this.state.MontoViandas}
                        displayType={"text"}
                        thousandSeparator={true}
                        prefix={"$"}
                      />
                    </strong>
                  </div>
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
              </ul>
              <div className="tab-content" id="myTabContent">
                <div
                  className="tab-pane fade show active"
                  id="gestion"
                  role="tabpanel"
                  aria-labelledby="gestion-tab"
                >

                   <DataTable value={this.state.hospedaje} paginator={true} rows={10} rowsPerPageOptions={[5,10,20]} responsive={true}
                            selection={this.state.hospedajeseleccionados} onSelectionChange={e => this.montosHoteles(e)}>
                            <Column selectionMode="multiple" style={{ width: '5em', fontSize: '10px'}} />
                            <Column field="Id" header="N°"  style={{width: '5em',fontSize: '10px'}}/>
                            <Column field="NombreProveedor" header="Proveedor" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="FechaConsumo" header="F. Consumo" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="Legajo" header="Legajo" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="Identificacion" header="Identificación" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="Nombres" header="Nombres" filter={true} style={{fontSize: '10px'}} />
                            <Column field="Cargo" header="Cargo" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="Habitacion" header="Habitación" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="Espacio" header="Espacio" filter={true} style={{fontSize: '10px'}} />
                            <Column field="Tipo" header="Tipo" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="FechaSalida" header="F. Salida" filter={true} style={{fontSize: '10px'}} />
                            <Column field="Tarifa" header="Tarifa"  filter={true}   body={this.MoneyFormat} style={{fontSize: '10px'}} />
                    </DataTable>            
                </div>
                <div
                  className="tab-pane fade"
                  id="op"
                  role="tabpanel"
                  aria-labelledby="op-tab"
                >
                          <DataTable value={this.state.alimentacion} paginator={true} rows={10} rowsPerPageOptions={[5,10,20]} responsive={true}
                            selection={this.state.alimentacionseleccionados} onSelectionChange={e => this.montosAlimentacion(e)}>
                            <Column selectionMode="multiple" style={{ width: '5em', fontSize: '10px'}} />
                            <Column field="Id" header="N°"  style={{width: '5em',fontSize: '10px'}}/>
                            <Column field="NombreProveedor" header="Proveedor" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="FechaConsumo" header="F. Consumo" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="Legajo" header="Legajo" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="Identificacion" header="Identificación" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="Nombres" header="Nombres" filter={true} style={{fontSize: '10px'}} />
                            <Column field="Cargo" header="Cargo" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="TipoComida" header="Tipo Comida" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="OpcionComida" header="Opcion Comida" filter={true} style={{fontSize: '10px'}} />
                            <Column field="Tarifa" header="Tarifa" style={{fontSize: '10px'}} />
                        </DataTable>      

                </div>
                <div
                  className="tab-pane fade"
                  id="vi"
                  role="tabpanel"
                  aria-labelledby="vi-tab"
                  >
                  <DataTable value={this.state.viandas} paginator={true} rows={10} rowsPerPageOptions={[5,10,20]} responsive={true}
                            selection={this.state.viandasseleccionados} onSelectionChange={e => this.montosViandas(e)}>
                            <Column selectionMode="multiple" style={{ width: '5em', fontSize: '10px'}} />
                            <Column field="Id" header="N°"  style={{width: '5em',fontSize: '10px'}}/>
                            <Column field="NombreProveedor" header="Proveedor" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="FechaPedido" header="F. Pedido" filter={true}  style={{fontSize: '10px'}}/>
                           
                            <Column field="FechaConsumo" header="F. Consumo" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="Locacion" header="Locación" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="IdSolicitante" header="ID Solicitante" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="NombreSolicitante" header="Nombres" filter={true} style={{fontSize: '10px'}} />
                            <Column field="TotalSolicitado" header="Total Solicitado" filter={true}  style={{fontSize: '10px'}}/>
                            <Column field="Tarifa" header="Tarifa"  filter={true}  style={{fontSize: '10px'}} />
                            <Column field="Total" header="Total" filter={true}  style={{fontSize: '10px'}}/>
                        </DataTable>      
                  </div>
              </div>
            </div>
          </Card>
        </div>
      </BlockUi>
    );
  }

  EnviarFormulario(event) {
    event.preventDefault();

    if (this.state.RutaId === 0) {
      this.alertMessage("Seleccione un Ruta");
    } else if (this.state.Horarioid == 0) {
      this.alertMessage("Seleccione un Horario");
    } else if (this.state.FechaHasta < this.state.FechaDesde) {
      this.alertMessage("La Fecha Hasta debe ser mayor a la Fecha Desde");
    } else if (this.state.VehiculoId == 0) {
      this.alertMessage("Seleccione un Vehiculo");
    } else if (this.state.FechaHasta < this.state.FechaDesde) {
      this.alertMessage("La Fecha Hasta debe ser mayor a la Fecha Desde");
    } else {
      axios
        .post("/Transporte/RutaHorarioVehiculo/Create", {
          Id: 0,
          RutaHorarioId: this.state.Horarioid,
          VehiculoId: this.state.VehiculoId,
          FechaDesde: this.state.FechaDesde,
          FechaHasta: this.state.FechaHasta,
          HoraLlegada: this.state.horas,
          Observacion: this.state.Observacion,
          horarioid: this.state.Horarioid,
          Estado: 1
        })
        .then(response => {
          if (response.data == "OK") {
            this.infoMessage("Ruta Asignada a Vehiculo ");
            this.setState({ visible: false });
            this.ObtenerRutasHorarios(this.state.Horarioid);
          } else {
            this.alertMessage("Ocurrió un Error");
            this.StopLoading();
          }
        })
        .catch(error => {
          console.log(error);
          this.alertMessage("Ocurrió un Error");
          this.StopLoading();
        });
    }
  }
  EnviarFormularioEditar(event) {
    event.preventDefault();

    if (this.state.RutaId === 0) {
      this.alertMessage("Seleccione un Ruta");
    } else if (this.state.Horarioid == 0) {
      this.alertMessage("Seleccione un Horario");
    } else if (this.state.FechaHasta < this.state.FechaDesde) {
      this.alertMessage("La Fecha Hasta debe ser mayor a la Fecha Desde");
    } else if (this.state.VehiculoId == 0) {
      this.alertMessage("Seleccione un Vehiculo");
    } else if (this.state.FechaHasta < this.state.FechaDesde) {
      this.alertMessage("La Fecha Hasta debe ser mayor a la Fecha Desde");
    } else {
      axios
        .post("/Transporte/RutaHorarioVehiculo/Edit", {
          Id: this.state.Identificador,
          RutaHorarioId: this.state.Horarioid,
          VehiculoId: this.state.VehiculoId,
          FechaDesde: this.state.FechaDesde,
          FechaHasta: this.state.FechaHasta,
          HoraLlegada: this.state.horas,
          Observacion: this.state.Observacion,
          horarioid: this.state.Horarioid,
          Estado: 1
        })
        .then(response => {
          if (response.data == "OK") {
            this.infoMessage("Editado Correctamente");
            this.setState({ visibleeditar: false });
            this.ObtenerRutasHorarios(this.state.Horarioid);
          } else {
            this.alertMessage("Ocurrió un Error");
          }
        })
        .catch(error => {
          console.log(error);
          this.alertMessage("Ocurrió un Error");
        });
    }
  }
  MostrarFormulario() {
    console.log(this.state.RutaId);
    console.log(this.state.Horarioid);
    if (this.state.RutaId > 0 && this.state.Horarioid > 0) {
      this.setState({
        visible: true,
        VehiculoId: 0,
        FechaDesde: new Date(),
        FechaHasta: new Date()
      });

      axios
        .post("/Transporte/RutaHorarioVehiculo/ObtenerHoraLLegada", {
          rutaid: this.state.RutaId,
          horarioid: this.state.Horarioid
        })
        .then(response => {
          console.log(response.data);

          this.setState({ horas: response.data });
        })
        .catch(error => {
          console.log(error);
        });
    } else {
      this.props.showWarn("Seleccione un Ruta y un Horario");
    }
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value.toUpperCase() });
  }
  handleChangeCorreo(event) {
    this.setState({ [event.target.name]: event.target.value.toLowerCase() });
  }
  handleChangeIdentificacion(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  Loading() {
    this.setState({ blocking: true });
  }

  StopLoading() {
    this.setState({ blocking: false });
  }

  OcultarFormulario() {
    this.setState({ visible: false });
    // this.VaciarCampos();
  }

  OcultarFormularioEditar() {
    this.setState({ visibleeditar: false });
  }


  MostrarFormularioEditar(row) {
    console.log(row.Id);
    console.log(row);
    if (row.Id > 0) {
      this.setState({
        RutaHorarioId: row.RutaHorarioId,
        FechaDesde: row.FechaDesde,
        FechaHasta: row.FechaHasta,
        horas: row.HoraLlegada,
        Identificador: row.Id,
        VehiculoId: row.VehiculoId,
        Observacion: row.Observacion,

        visibleeditar: true
      });
    }
  }

  showInfo() {
    this.growl.show({
      severity: "info",
      summary: "Información",
      detail: this.state.message
    });
  }

  infoMessage(msg) {
    this.setState({ message: msg }, this.showInfo);
  }

  showAlert() {
    this.growl.show({
      severity: "error",
      summary: "Alerta",
      detail: this.state.message
    });
  }

  alertMessage(msg) {
    this.setState({ message: msg }, this.showAlert);
  }
}
const Container = Wrapper(LiquidacionServicioContainer);
ReactDOM.render(<Container />, document.getElementById("content"));
