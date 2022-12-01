import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";

import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import { Checkbox } from "primereact/components/checkbox/Checkbox";
import TreeWbs from "../wbs_components/TreeWbs";
import ComputoForm from "./ComputoForm";
import { Dropdown } from "primereact/components/dropdown/Dropdown";
export default class CrearComputo extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      message: "",
      data: [],
      key: 7954,
      blocking: true,
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
      EAC: 0,

      cantidadAjustada: false,
      tipoAjuste: [
        { label: "Ingeniería", value: "Ingeniería" },
        { label: "Red Line", value: "Red Line" },
        { label: "Topografía", value: "Topografía" },
      ],
      tipoAjusteString: "",
    };
    this.getItems = this.getItems.bind(this);
    this.getItemsProcura = this.getItemsProcura.bind(this);
    this.getunidades = this.getunidades.bind(this);
    this.deleteItem = this.deleteItem.bind(this);
    this.updateData = this.updateData.bind(this);
    this.onSelectionChange = this.onSelectionChange.bind(this);
    this.onHide = this.onHide.bind(this);
    this.onHideComputo = this.onHideComputo.bind(this);
    this.selectComputo = this.selectComputo.bind(this);
    this.showDialog = this.showDialog.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  componentWillMount() {
    console.log("DataDialog");
    this.updateData();
    this.getItems();
    //this.getItemsProcura();
    this.getunidades();
  }

  onSelectionChange(e) {
    if (e.selection.tipo == "actividad") {
      var ids = e.selection.data.split(",");
      this.setState({
        selectedFile: e.selection,
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
      });
    }
    if (e.selection.tipo == "computo") {
      var ids = e.selection.data.split("!");
      console.log("data", ids);
      var ajustado = ids[11];
      console.log("Ajustado", ajustado);
      
      console.log("Ajustado to Lower", ajustado.toLowerCase().includes('F')?false:true);
      this.setState({
        selectedFile: e.selection,
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
        EAC: ids[9],
        cantidadAjustada: ajustado.toLowerCase().includes('f')?false:true,
        tipoAjusteString: ids[12],
      });
    }
  }

  onchangeCantidadAjustada = (value) => {
    this.setState({ cantidadAjustada: value });
    console.log("CantidadAjustada", value);
    console.log("ComputoId", this.state.computoId);
    if (!value) {
      this.setState({ blocking: true });
      axios
        .post("/proyecto/Computo/ComputoActiveAjustado", {
          Id: this.state.computoId,
          cantidadAjustada: value,
          tipo: "",
        })
        .then((response) => {
            this.updateData();
          if (response.data === "OK") {
             
            this.setState(
              { message: "Actualizado Correctamente" },
              this.showSuccess
            );
            /*this.setState({
              blocking: false,
            });*/
          } else {
            this.setState(
              { message: "Existe un incoveniente inténtelo más tarde" },
              this.showWarn
            );
            this.setState({
              blocking: false,
            });
          }
        })
        .catch((error) => {
          console.log(error);
          this.setState({
            blocking: false,
          });
          this.setState(
            { message: "Existe un incoveniente inténtelo más tarde" },
            this.showWarn
          );
        });
    }
  };
  onchangeCantidadTipo = (value) => {
    console.log("Value", value);
    console.log("ComputoId", this.state.computoId);
    this.setState({ tipoAjusteString: value });
    if (this.state.cantidadAjustada) {
      this.setState({ blocking: true });
      axios
        .post("/proyecto/Computo/ComputoActiveAjustado", {
          Id: this.state.computoId,
          cantidadAjustada: this.state.cantidadAjustada,
          tipo: value,
        })
        .then((response) => {
            this.updateData();
          console.log("Response Check", reponse);
          if (response.data === "OK") {
         
          /*  this.setState({
              blocking: false,
            });*/
            this.setState(
              { message: "Actualizado Correctamente" },
              this.showSuccess
            );
          } else {
            this.setState(
              { message: "Existe un incoveniente inténtelo más tarde" },
              this.showWarn
            );
            this.setState({
              blocking: false,
            });
          }
        })
        .catch((error) => {
          console.log(error);
          this.setState({
            blocking: false,
          });
          this.setState(
            { message: "Existe un incoveniente inténtelo más tarde" },
            this.showWarn
          );
        });
    }
  };

  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <div className="row">
          <div className="col-sm-8">
            <TreeWbs
              key={this.state.key}
              onSelectionChange={this.onSelectionChange}
              data={this.state.data}
            />
          </div>

          <div className="col-sm-4">
            {this.state.computoId > 0 && (
              <>
                <div className="row">
                  <div className="col">
                    <label>
                      <strong>Cantidad Ajustada</strong>
                    </label>{" "}
                    <Checkbox
                      checked={this.state.cantidadAjustada}
                      onChange={(e) => this.onchangeCantidadAjustada(e.checked)}
                    />
                  </div>
                </div>
                <br />
                {this.state.cantidadAjustada && (
                  <div className="row">
                    <div className="col">
                      <Dropdown
                        value={this.state.tipoAjusteString}
                        options={this.state.tipoAjuste}
                        onChange={(e) => {
                          this.onchangeCantidadTipo(e.value);
                        }}
                        filter={true}
                        filterPlaceholder="Selecciona un Tipo *"
                        filterBy="label,value"
                        placeholder="Selecciona un Tipo *"
                        style={{ width: "100%" }}
                        required
                      />
                    </div>
                  </div>
                )}
              </>
            )}
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
                  <label>Nombre Padre: </label>
                </b>
                <label>{this.state.item_padre}</label>
              </div>
              <div className="form-group">
                <b>
                  <label> Código Item: </label>
                </b>
                <label>{this.state.codigoitem}</label>
              </div>
              <div className="form-group">
                <b>
                  <label>Nombre Item: </label>
                </b>
                <label>{this.state.item_codigo}</label>
              </div>
              <div className="form-group">
                <b>
                  <label>Fecha Registro: </label>
                </b>
                <label>{this.state.fecha_registro}</label>
              </div>
              <div className="form-group">
                <b>
                  <label>fecha Actualización : </label>
                </b>
                <label>{this.state.fecha_actualizacion}</label>
              </div>
              <div className="row">
                <div className="col">
                  {" "}
                  <div className="form-group">
                    <label htmlFor="cantidad">Cantidad</label>
                    <input
                      type="text"
                      disabled
                      id="cantidad"
                      value={this.state.cantidad}
                      className="form-control"
                      onChange={this.handleChange}
                      name="cantidad"
                    />
                  </div>
                </div>
                <div className="col">
                  {" "}
                  <div className="form-group">
                    <label htmlFor="PU">Precio Unitario</label>
                    <input
                      type="text"
                      disabled
                      id="PU"
                      className="form-control"
                      value={this.state.precio_unitario}
                    />
                  </div>{" "}
                </div>
              </div>
              <div className="row">
                <div className="col">
                  <div className="form-group">
                    <label htmlFor="eac">EAC</label>
                    <input
                      type="text"
                      id="eac"
                      className="form-control"
                      value={this.state.EAC}
                      onChange={this.handleChange}
                      name="EAC"
                    />
                  </div>
                </div>

                <div className="col">
                  <div className="form-group">
                    <label htmlFor="Total">Total</label>
                    <input
                      type="text"
                      disabled
                      id="total"
                      className="form-control"
                      value={this.state.precio_unitario * this.state.cantidad}
                    />
                  </div>
                </div>
              </div>
              <div className="form-group">
                <label htmlFor="Total">Total</label>
                <input
                  type="text"
                  disabled
                  id="total"
                  className="form-control"
                  value={this.state.precio_unitario * this.state.cantidad}
                />
              </div>
              <button
                type="submit"
                className="btn btn-outline-primary"
                disable={this.state.canSubmit}
              >
                Actualizar
              </button>
              &nbsp;
            </form>

            <Dialog
              header="Ingreso de Computos"
              visible={this.state.visiblecomputo}
              width="500px"
              modal={false}
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
                warnMessage={this.props.warnMessage}
                successMessage={this.props.successMessage}
                selectComputo={this.selectComputo}
              />
            </Dialog>
          </div>
        </div>
      </BlockUi>
    );
  }

  handleSubmit(event) {
    event.preventDefault();
    axios
      .post("/proyecto/Computo/EditComputo", {
        Id: this.state.computoId,
        cantidad: this.state.cantidad,
        precio_unitario: this.state.precio_unitario,
        costo_total: this.state.precio_unitario * this.state.cantidad,
        vigente: true,
        WbsOfertaId: this.state.WbsOfertaId,
        ItemId: 1,
        estado: true,
        codigo_primavera: "a",
        cantidad_eac: this.state.EAC,
      })
      .then((response) => {
        if (response.data === "OK") {
          this.updateData();
          this.props.successMessage("Computo actualizado");
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
            EAC: 0,
          });
        } else {
          this.props.warnMessage("Algo ocurrió mal");
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.warnMessage("Algo ocurrió mal");
      });
  }

  updateData() {
    axios
      .get(
        "/proyecto/Computo/ApiComputo/" +
          document.getElementById("OfertaId").className,
        {}
      )
      .then((response) => {
        console.log("response", response.data);
        this.setState({
          data: response.data,
          blocking: false,
          key: Math.random(),
        });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  deleteItem(id) {
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
  onHide(event) {
    this.setState({ visible: false });
  }
  onHideComputo(event) {
    this.setState({ visiblecomputo: false });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  selectComputo(id, codigo, precio_unitario, cantidad, itempadre, coditem) {
    this.setState({
      computoId: id,
      visible: false,
      item_codigo: codigo,
      precio_unitario: precio_unitario,
      cantidad: cantidad,
      item_padre: itempadre,
      codigoitem: coditem,
    });
  }

  showDialog(e) {
    e.preventDefault();
    this.setState({ visible: true });
  }

  getItems() {
    axios
      .post(
        "/Proyecto/Computo/ItemsparaOfertaC/" +
          document.getElementById("ContratoId").className,
        {
          f: document.getElementById("FechaOfertaId").className,
        }
      )
      .then((response) => {
        var computos = response.data.map((i) => {
          return { label: i.codigo + " - " + i.nombre, value: i.Id };
        });

        this.setState({ itemsoferta: response.data, item_list: computos });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getItemsProcura() {
    axios
      .post("/Proyecto/Computo/ItemsProcura")
      .then((response) => {
        var itemsp = response.data.map((i) => {
          return { label: i.codigo + " - " + i.nombre, value: i.Id };
        });

        this.setState({ itemsprocura: itemsp });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  getunidades() {
    axios
      .post("/Proyecto/Computo/CatalogoUnidades")
      .then((response) => {
        var uns = response.data.map((i) => {
          return { label: i.nombre, value: i.Id };
        });

        this.setState({ unidades: uns });
      })
      .catch((error) => {
        console.log(error);
      });
  }
}
