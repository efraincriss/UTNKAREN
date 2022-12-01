import ReactDOM from "react-dom";
import React, { Component } from "react";
import Field from "./Base/Field-v2";

import axios from "axios";
import wrapForm from "./Base/BaseWrapper";
import ComputoForm from "./forms/ComputoForm";

import { Tree } from "primereact_/tree";
import { ContextMenu } from "primereact_/contextmenu";
import { Button } from "primereact_/button";
import { Dialog } from "primereact_/dialog";
import { Checkbox } from "primereact_/checkbox";
export class ComputoRDO extends Component {
  constructor(props) {
    super(props);
    this.state = {
      data: null,
      selectedNodeKey: null,
      menu: [],
      checked: false,

      selectedFile: { nombres: "" },
      visible: false,
      visiblecomputo: false,
      table_data: [],
      computoId: 0,
      item_codigo: "",
      WbsOfertaId: 0,
      cantidad: 0,
      codigoitem: "",
      precio_unitario: 0.0,
      canSubmit: false,
      nombrepadre: "",
      codigoI: "",
      nombrei: "",
      itemsoferta: [],
      fecha_registro: "",
      fecha_actualizacion: "",
      item: 0,
      item_list: [],
      itemsprocura: [],
      unidades: [],
      visibleRight: false,
      nombreu: "",

      blockmonto: false,
      costo_total: 0,
    };
    this.onSelectionChange = this.onSelectionChange.bind(this);
    this.onHideComputo = this.onHideComputo.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.deleteItem = this.deleteItem.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  componentDidMount() {
    this.updateData();
  }
  handleSubmit(event) {
    event.preventDefault();
    axios
      .post("/proyecto/Computo/EditComputo", {
        Id: this.state.computoId,
        cantidad: this.state.cantidad,
        precio_unitario: this.state.precio_unitario,
        costo_total: this.state.costo_total,
        vigente: true,
        WbsOfertaId: this.state.WbsOfertaId,
        ItemId: 1,
        estado: true,
        codigo_primavera: "a",
      })
      .then((response) => {
        console.log(response);
        if (response.data === "OK") {
          this.updateData();
          this.props.showSuccess("Registro Actualizado");
          this.setState({
            blocking: false,
            selectedFile: { nombres: "" },
            table_data: [],
            computoId: 0,
            item_codigo: "",
            WbsOfertaId: 0,
            cantidad: 0,
            precio_unitario: 0.0,
            canSubmit: false,
          });
        } else {
          this.props.showWarn();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Existió un inconveniente inténtelo más tarde");
      });
  }
  updateData = () => {
    axios
      .get(
        "/proyecto/Computo/ApiComputo/" +
          document.getElementById("OfertaId").className,
        {}
      )
      .then((response) => {
        console.log("dataArbol", response);
        this.setState({
          data: response.data,
        });
        this.props.unlockScreen();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  GetMontosPresupuesto = () => {
    console.log("CaluclarPresupuesto");
    this.props.blockScreen();
    axios
      .post("/proyecto/Presupuesto/ActualizarCostosRdo", {
        contratoId: document.getElementById("ContratoId").className,
        oferta: document.getElementById("OfertaId").className,
      })
      .then((response) => {
        console.log("response", response);
        this.props.unlockScreen();
        this.props.showSuccess("Cantidades del Presupuesto Actualizadas");
        //this.updateData();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };
  SelectNode = (event) => {
    console.log(event.value);
    if (event.value > 0) {
      /*  this.props.blockScreen();
      axios
        .post("/proyecto/Item/DetailsApi/" + event.value, {})
        .then((response) => {
          this.setState({
            Seleccionado: response.data,
          });
          this.props.unlockScreen();
        })
        .catch((error) => {
          console.log(error);
          this.props.unlockScreen();
        });*/
    } else {
      abp.notify.error("Debe Seleccionar un Nodo", "Error");
    }
  };
  handleChange(event) {
    console.log(event);
    this.setState({ [event.target.name]: event.target.value });
  }
  onSelectionChange(event) {
    let e = event.node;
    console.log(e);
    if (e.tipo == "actividad") {
      var ids = e.data.split(",");
      this.setState({
        selectedFile: e,
        WbsOfertaId: ids[2],
        blocking: false,
        computoId: 0,
        item_codigo: "",
        item_padre: "",
        cantidad: 0,
        codigoitem: "",
        precio_unitario: 0.0,
        visiblecomputo: true,
        fecha_registro: "",
        fecha_actualizacion: "",
        nombreu: "",
      });
    }
    if (e.tipo == "computo") {
      var ids = e.data.split("!");
      this.setState({
        selectedFile: e,
        computoId: ids[0],
        blocking: false,
        cantidad: parseFloat(ids[1].replace(",", ".")),
        precio_unitario: parseFloat(ids[2].replace(",", ".")),
        costo_total: parseFloat(ids[3].replace(",", ".")),
        visiblecomputo: false,
        item_padre: ids[4],
        item_codigo: ids[6],
        codigoitem: ids[5],
        fecha_registro: ids[7],
        fecha_actualizacion: ids[8],
        //ceac ids[9]
        nombreu: ids[10],
      });
      this.GetInfoComputo(ids[0]);
    }
  }
  onHideComputo(event) {
    console.log("entrohi");
    this.setState({ visiblecomputo: false });
  }
  GetInfoComputo = (id) => {
    this.props.blockScreen();
    axios
      .post("/proyecto/Computo/ComputoGetInfo", {
        id: id,
      })
      .then((response) => {
        console.log("ComputoInfo", response);
        let data = response.data;
        this.setState({ checked: data.es_temporal }, this.props.unlockScreen());
     
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  deleteItem(id) {
    console.log("Id",id);
    console.log("this.state.computoId",this.state.computoId);
    axios
      .post("/proyecto/Computo/DeleteComputoArbol/" + this.state.computoId, {})
      .then((response) => {
        var r = response.data;

        if (r == "Eliminado") {
          console.log("entro guardado");

          this.updateData();
          this.setState(
            { message: "Eliminado Correctamente" },
            this.showSuccess
          );
        }
        if (r == "ErrorEliminado") {
          console.log("entro error");
          this.setState({ message: "No se pudo Eliminar" }, this.showWarn);
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.warnMessage("Algo ocurrió mal");
      });
  }
  CambiarEstado = (checked) => {
    console.log("Selected", this.state.selectedFile);
    this.setState({ checked: checked }, this.props.blockScreen());

    axios
      .post("/proyecto/Computo/ComputoActiveTemporal", {
        id: this.state.computoId,
        es_temporal: checked,
      })
      .then((response) => {
        console.log("REsultCheck", response);
        this.updateData();
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  };

  render() {
    return (
      <div>
        <Button
          label="Recalcular Presupuesto"
          icon="pi pi-arrow-right"
          onClick={() => this.GetMontosPresupuesto()}
          style={{ marginRight: ".25em" }}
        />
        <div className="row">
          <div className="col-8">
            <h3>Estructura</h3>

            <ContextMenu
              appendTo={document.body}
              model={this.state.menu}
              ref={(el) => (this.cm = el)}
            />
            <Tree
              filter={true}
              filterMode="strict"
              value={this.state.data}
              dragdropScope="demo"
              onSelect={this.onSelectionChange}
              selectionMode="single"
              selectionKeys={this.state.selectedNodeKey}
              onSelectionChange={(e) =>
                this.setState({ selectedNodeKey: e.value })
              }
              onContextMenuSelectionChange={(event) => this.SelectNode(event)}
              onContextMenu={(event) => this.cm.show(event.originalEvent)}
              style={{ width: "100%" }}
            />
          </div>
          <div className="col-4" >
            <h3>Información</h3>
            <strong>¿Item Temporal en RDO ? </strong>{" "}
            <Checkbox
             tooltip="Para activar presione el Check"
              checked={this.state.checked}
              onChange={(e) => this.CambiarEstado(e.checked)}
              disabled={this.state.computoId>0?false:true}
            />
            <form onSubmit={this.handleSubmit}>
              <div className="form-group">
                <input
                  type="hidden"
                  value={this.state.selectedFile.nombres}
                  id="wbs"
                  className="form-control"
                  disabled
                />
              </div>
              <div className="form-group">
                <b>
                  <label style={{ fontSize: "12px" }}>
                    Nombre Padre:&nbsp;
                  </label>
                </b>
                <label style={{ fontSize: "12px" }}>
                  {this.state.item_padre}
                </label>
              </div>
              <div className="form-group">
                <b>
                  <label style={{ fontSize: "12px" }}>
                    {" "}
                    Código Item:&nbsp;
                  </label>
                </b>
                <label style={{ fontSize: "12px" }}>
                  {this.state.codigoitem}
                </label>
              </div>
              <div className="form-group">
                <b>
                  <label style={{ fontSize: "12px" }}>Nombre Item:&nbsp;</label>
                </b>
                <label style={{ fontSize: "12px" }}>
                  {this.state.item_codigo}
                </label>
              </div>
              <div className="form-group">
                <b>
                  <label style={{ fontSize: "12px" }}>
                    Fecha Registro:&nbsp;
                  </label>
                </b>
                <label style={{ fontSize: "12px" }}>
                  {this.state.fecha_registro}
                </label>
              </div>
              <div className="form-group">
                <b>
                  <label style={{ fontSize: "12px" }}>
                    Fecha Actualización :&nbsp;
                  </label>
                </b>
                <label style={{ fontSize: "12px" }}>
                  {this.state.fecha_actualizacion}
                </label>
              </div>
              <div className="row">
                <div className="col">
                  <div className="form-group">
                    <label style={{ fontSize: "12px" }} htmlFor="cantidad">
                      Cantidad
                    </label>
                    <input
                      type="text"
                      id="cantidad"
                      value={this.state.cantidad}
                      className="form-control"
                      onChange={this.handleChange}
                      name="cantidad"
                    />
                  </div>
                </div>
                <div className="col">
                  <div className="form-group">
                    <label style={{ fontSize: "12px" }} htmlFor="Unidad">
                      Unidad
                    </label>
                    <input
                      type="text"
                      disabled
                      id="Unidad"
                      className="form-control"
                      value={this.state.nombreu}
                    />
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <div className="form-group">
                    <label style={{ fontSize: "12px" }} htmlFor="PU">
                      P.U
                    </label>
                    <input
                      type="text"
                      disabled
                      id="PU"
                      className="form-control"
                      value={this.state.precio_unitario}
                    />
                  </div>
                </div>
                <div className="col">
                  <div className="form-group">
                    <label style={{ fontSize: "12px" }} htmlFor="Total">
                      Total
                    </label>
                    <input
                      type="text"
                      disabled
                      id="total"
                      className="form-control"
                      value={(
                        this.state.precio_unitario * this.state.cantidad
                      ).toLocaleString(undefined, { maximumFractionDigits: 2 })}
                    />
                  </div>
                </div>
              </div>
              <button
                type="submit"
                className="btn btn-outline-primary"
                disabled={this.state.computoId>0?false:true}
              >
                Actualizar
              </button>
              &nbsp;
              <button
                type="button"
                className="btn btn-outline-primary"
                disabled={this.state.computoId>0?false:true}
                onClick={() => this.deleteItem(this.state.computoId)}
              >
                {" "}
                Eliminar
              </button>
            </form>
          </div>
          <Dialog
            header="Ingreso de Computos"
            visible={this.state.visiblecomputo}
            style={{ width: "50vw" }}
            modal={true}
            onHide={this.onHideComputo}
          >
            <ComputoForm
              getItems={this.getItems}
              itemsoferta={this.state.itemsoferta}
              item_list={this.state.item_list}
              item={this.state.item}
              itemsprocura={this.state.itemsprocura}
              unidades={this.state.unidades}
              onHide={this.onHideComputo}
              updateData={this.updateData}
              data={this.state.table_data}
              WbsOfertaId={this.state.WbsOfertaId}
              selectComputo={this.selectComputo}
            />
          </Dialog>
        </div>
      </div>
    );
  }
}

const rootElement = document.getElementById("content-computos");
const Container = wrapForm(ComputoRDO);
ReactDOM.render(<Container />, rootElement);
