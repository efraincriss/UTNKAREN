import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";
import CurrencyFormat from "react-currency-format";
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v3.3/dialog";
import { Growl } from "primereact-v3.3/growl";
import { Card } from "primereact-v3.3/card";

class PosList extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      errors: {},
      editable: true,
      data: [],
      tipoComida:[],
      select:null, 
      tipoOpcionComidaId:0,
      horarioInicio:null,
      horarioFin:null,
      action: "list",
      dialog: false,
      zonaId:0,
      zonas:[],


    };

    this.handleChange = this.handleChange.bind(this);
    this.onChangeValue = this.onChangeValue.bind(this);
    this.onChangeValueZona = this.onChangeValueZona.bind(this);
    this.mostrarForm = this.mostrarForm.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind();
    this.isValid = this.isValid.bind();
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
  }

  componentDidMount() {
   // this.GetList();
    this.GetCatalogs();
  }
  isValid = () => {
    const errors = {};

    {/*if (this.state.tipoOpcionComidaId == "") {
      errors.tipoOpcionComidaId = "Campo Requerido";
    }*/}
    if (this.state.horarioInicio == null) {
      errors.horarioInicio = "Campo Requerido";
    }
    if (this.state.horarioFin == null) {
        errors.horarioFin = "Campo Requerido";
      }
    this.setState({ errors });
    return Object.keys(errors).length === 0;
  };


  OcultarFormularioOrden = () => {
    this.setState({ dialog:false, action:"list" });
  };
  GetList = (zonaId) => {
    this.props.blockScreen();
    axios
      .post("/Proveedor/Proveedor/GetListHorarios", {
        Id:zonaId
      })
      .then((response) => {
        console.log(response.data);
        this.setState({ data: response.data });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn(error);
        this.props.unlockScreen();
      });
  };
  GetCatalogs = () => {
    this.props.blockScreen();
    axios
      .post("/Proveedor/Proveedor/GetZonasCobertura", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        items.unshift({ label: "Seleccione", value: "0", dataKey: 0 });
        this.setState({ zonas: items });
       this.props.unlockScreen();
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

  onChangeValueZona = (name, value) => {
    this.setState({
      [name]: value,
    },this.GetList(value));

  
  };


 
 
  mostrarForm = (row) => {
      console.log('row',row);
   
      this.setState({
        tipoOpcionComidaId:row.tipoOpcionComidaId,
        horarioInicio:row.horarioInicio,
        horarioFin:row.horarioFin,
        action: "edit",
        dialog: true,
        select:row,
      });
   
  };
 
  generarBotones = (cell, row) => {
    return (
      <div>
       
        <button
          className="btn btn-outline-info btn-sm"
          style={{ marginRight: "0.2em" }}
          onClick={() => this.mostrarForm(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar Horario"
        >
          <i className="fa fa-edit" />
        </button>
      {/** <button
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
        </button> */} 
      </div>
    );
  };
 
  renderShowsTotal(start, to, total) {
    return (
      <p style={{ color: "black" }}>
        De {start} hasta {to}, Total Registros {total}
      </p>
    );
  }
  render() {
    const options = {
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

    if (this.state.action === "list") {
      return (
        <BlockUi tag="div" blocking={this.state.blocking}>
          <Growl
            ref={(el) => {
              this.growl = el;
            }}
            baseZIndex={1000}
          />
        
            <div className="row">
          <div className="col" align="left">
            <Field
              name="zonaId"
              required
              value={this.state.zonaId}
              label="Zona de Cobertura"
              options={this.state.zonas}
              type={"select"}
              filter={true}
              onChange={this.onChangeValueZona}
              error={this.state.errors.zonaId}
              readOnly={false}
              placeholder="Seleccione Zona.."
              filterPlaceholder="Seleccione.."
            />
          </div>
         
          <div className="col" align="right" style={{ paddingTop: "35px" }}>
           
          </div>
        </div>
        
          <br />
          <div>
            <BootstrapTable
              data={this.state.data}
              hover={true}
              pagination={true}
              options={options}
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
                width={"20%"}
              
               
                dataField="nombreTipoOpcionComida"
          
                filter={{ type: "TextFilter", delay: 500 }}
                tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
                dataSort={true}
        
              >
                Tipo Opción Comida
              </TableHeaderColumn>
            
              <TableHeaderColumn
                dataField="formathorarioInicio"
               
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
                width={"10%"}
                tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              >
                Horario Inicio{" "}
              </TableHeaderColumn>
              <TableHeaderColumn
                dataField="formathorarioFin"
               
                filter={{ type: "TextFilter", delay: 500 }}
                dataSort={true}
                width={"10%"}
                tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
                thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              >
                Horario Fin{" "}
              </TableHeaderColumn>
             
              <TableHeaderColumn
                dataField="tipoOpcionComidaId"
                export={false}
                isKey
                width={"10%"}
                dataFormat={this.generarBotones}
                thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              >
                Opciones
              </TableHeaderColumn>
            </BootstrapTable>
          </div>
        </BlockUi>
      );
        } else {
      return (
        <BlockUi tag="div" blocking={this.state.blocking}>
          <div>
            <form onSubmit={this.EnviarFormulario}>
            <div className="row">
                <div className="col">
                <label><strong>Tipo Opción Comida: </strong>{this.state.select!=null?this.state.select.nombreTipoOpcionComida:""}</label>
                    </div>
                    </div>   
              <div className="row">
                <div className="col">
                <Field
                    name="horarioInicio"
                    label="Horario Inicio"
                    required
                    type="time"
                    edit={true}
                    readOnly={false}
                    value={this.state.horarioInicio}
                    onChange={this.handleChange}
                    error={this.state.errors.horarioInicio}
                  />
                {/*<Field
                    name="tipoOpcionComidaId"
                    required
                    value={this.state.tipoOpcionComidaId}
                    label="Tipos Opcion Comida"
                    options={this.state.tipoComida}
                    type={"select"}
                    filter={true}
                    onChange={this.onChangeValue}
                    error={this.state.errors.tipoOpcionComidaId}
                    readOnly={false}
                    placeholder="Seleccione.."
                    filterPlaceholder="Seleccione.."
                  />*/}
                </div>
                <div className="col">
                  <Field
                    name="horarioFin"
                    label="Horario Fin"
                    required
                    type="time"
                    edit={true}
                    readOnly={false}
                    value={this.state.horarioFin}
                    onChange={this.handleChange}
                    error={this.state.errors.horarioFin}
                  />
                </div>
              </div>
              <button type="submit" className="btn btn-outline-primary">
                Guardar
              </button>
              &nbsp;
              <button
                type="button"
                className="btn btn-outline-primary"
                icon="fa fa-fw fa-ban"
                onClick={this.OcultarFormularioOrden}
              >
                Cancelar
              </button>
            </form>
          </div>
        </BlockUi>
      );
    }
  }

  EnviarFormulario = (event) => {
    event.preventDefault();
    this.props.blockScreen();

    if (!this.isValid()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      this.props.unlockScreen();
      return;
    } else {
       
    
        axios
          .post("/Proveedor/Proveedor/GetCreate", {
             tipoOpcionComidaId:this.state.tipoOpcionComidaId, 
             HorarioInicio:this.state.horarioInicio, 
             HorarioFin:this.state.horarioFin,
             zonaId:this.state.zonaId
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("Actualizado Correctamente", "Info");
              this.setState({ action: "list" });
              this.GetList(this.state.zonaId);
            }else{
                abp.notify.error(
                    response.data,
                    "Error"
                  );
                  this.props.unlockScreen();
            }
           
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(
              "Existe un incoveniente inténtelo más tarde ",
              "Error"
            );
            this.props.unlockScreen();
          });
      }
    
  };
  EnviarFormularioDetalle = (event) => {
    event.preventDefault();
    this.props.blockScreen();

    if (!this.isValidDetalle()) {
      abp.notify.error(
        "No ha ingresado los campos obligatorios  o existen datos inválidos.",
        "Validación"
      );
      this.props.unlockScreen();
      return;
    } else {
      if (this.state.action_detalles == "create") {
        axios
          .post("/Proyecto/OrdenServicio/FDCreateDetalle", {
            Id: 0,
            OrdenServicioId: this.state.po.Id,
            ProyectoId: this.state.ProyectoId,
            GrupoItemId: this.state.GrupoItemId,
            valor_os: this.state.valor_os,
            OfertaComercialId: this.state.OfertaComercialId,
            vigente: true,
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("Guardado", "Correcto");
              this.setState({ dialogdetalles: false });
              this.GetListDetalles(this.state.po.Id);
              this.GetDetalleOS(this.state.po.Id);
            } else {
              abp.notify.error(
                "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
                "Error"
              );
            }
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(error, "Error");
          });
      } else {
        axios
          .post("/Proyecto/OrdenServicio/FDEditDetalle", {
            Id: this.state.Idd,
            OrdenServicioId: this.state.po.Id,
            ProyectoId: this.state.ProyectoId,
            GrupoItemId: this.state.GrupoItemId,
            valor_os: this.state.valor_os,
            OfertaComercialId: this.state.OfertaComercialId,
            vigente: true,
          })
          .then((response) => {
            if (response.data == "OK") {
              abp.notify.success("Guardado", "Correcto");
              this.setState({ dialogdetalles: false });
              this.GetListDetalles(this.state.po.Id);
              this.GetDetalleOS(this.state.po.Id);
            } else {
              abp.notify.error(
                "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
                "Error"
              );
            }
          })
          .catch((error) => {
            console.log(error);
            abp.notify.error(
              "Ha ocurrido un error, por favor inténtalo de nuevo más tarde",
              "Error"
            );
          });
      }
    }
  };

  onDownloaImagen = (Id) => {
    return (window.location = `/Proyecto/AvanceObra/Descargar/${Id}`);
  };

  MostrarFormulario() {
    this.setState({ visible: true });
  }

  onHide = () => {
    this.setState({ dialogdetalles: false });
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

  OcultarFormulario = () => {
    this.setState({ visible: false });
  };
}
const Container = wrapForm(PosList);
ReactDOM.render(<Container />, document.getElementById("content"));
