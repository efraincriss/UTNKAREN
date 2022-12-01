import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import { Button } from "primereact/components/button/Button";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import TreeWbs from "./wbs_components/TreeWbs";
import ComputoForm from "./forms/ComputoForm";
import { Sidebar } from "primereact/components/sidebar/Sidebar";

export default class CreateTreeComputos extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
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
      visibleRight: false,
      nombreu: "",

      blockmonto: false,
      costo_total: 0,

      //montos de Presupuestos
      montopc: 0,
      montopi: 0,
      montop: 0,
      montoa: 0,
      montoi: 0,
      montou: 0,
      montoprc: 0,
      montototal: 0,
      montofinal: 0,
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
    this.showSuccess = this.showSuccess.bind(this);
    this.showWarn = this.showWarn.bind(this);
    this.GetMontosPresupuesto = this.GetMontosPresupuesto.bind(this);
  }

  componentWillMount() {
    this.updateData();
    this.getItems();
    this.getItemsProcura();
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
        nombreu: "",
      });
    }
    if (e.selection.tipo == "computo") {
      var ids = e.selection.data.split("!");
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
        //ceac ids[9]
        nombreu: ids[10],
      });
    }
  }

  render() {
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={(el) => {
            this.growl = el;
          }}
          position="bottomright"
          baseZIndex={1000}
        ></Growl>
        <div className="content-section implementation">
          <Sidebar
            visible={this.state.visibleRight}
            position="right"
            baseZIndex={1000000}
            onHide={(e) => this.setState({ visibleRight: false })}
          >
            <BlockUi tag="div" blocking={this.state.blockmonto}>
              <h3 style={{ fontWeight: "normal" }}>Presupuesto:</h3>

              <fieldset>
                <legend>Subtotales:</legend>
                <div className="row">
                  <div className="col">
                    <b>
                      <label style={{ fontSize: "12px" }}>Ingeniería: </label>
                    </b>
                  </div>
                  <div className="col">
                    <label>
                      ${" "}
                      {this.state.montopi.toLocaleString(undefined, {
                        maximumFractionDigits: 2,
                      })}
                    </label>
                  </div>
                </div>

                <div className="row">
                  <div className="col">
                    <b>
                      <label style={{ fontSize: "12px" }}>Construcción: </label>
                    </b>
                  </div>
                  <div className="col">
                    <label>
                      ${" "}
                      {this.state.montopc.toLocaleString(undefined, {
                        maximumFractionDigits: 2,
                      })}
                    </label>
                  </div>
                </div>

                <div className="row">
                  <div className="col">
                    <b>
                      <label style={{ fontSize: "12px" }}>Procura: </label>
                    </b>
                  </div>
                  <div className="col">
                    <label>
                      ${" "}
                      {this.state.montop.toLocaleString(undefined, {
                        maximumFractionDigits: 2,
                      })}
                    </label>
                  </div>
                </div>
              </fieldset>

              <fieldset>
                <legend>Administración:</legend>
                <div className="row">
                  <div className="col">
                    <b>
                      <label style={{ fontSize: "12px" }}>
                        Administración sobre Obra:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col">
                    <label>
                      ${" "}
                      {this.state.montoa.toLocaleString(undefined, {
                        maximumFractionDigits: 2,
                      })}
                    </label>
                  </div>
                </div>

                <div className="row">
                  <div className="col">
                    <b>
                      <label style={{ fontSize: "12px" }}>
                        Imprevistos sobre Obra:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col">
                    <label>
                      ${" "}
                      {this.state.montoi.toLocaleString(undefined, {
                        maximumFractionDigits: 2,
                      })}
                    </label>
                  </div>
                </div>
                <div className="row">
                  <div className="col">
                    <b>
                      <label style={{ fontSize: "12px" }}>
                        Utilidad sobre Obra:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col">
                    <label>
                      ${" "}
                      {this.state.montou.toLocaleString(undefined, {
                        maximumFractionDigits: 2,
                      })}
                    </label>
                  </div>
                </div>

                <div className="row">
                  <div className="col">
                    <b>
                      <label style={{ fontSize: "12px" }}>
                        Administración Procura Contratista:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col">
                    <label>
                      ${" "}
                      {this.state.montoprc.toLocaleString(undefined, {
                        maximumFractionDigits: 2,
                      })}
                    </label>
                  </div>
                </div>

                <div className="row">
                  <div className="col">
                    <b>
                      <label>Total: </label>
                    </b>
                  </div>
                  <div className="col">
                    <label>
                      ${" "}
                      {this.state.montototal.toLocaleString(undefined, {
                        maximumFractionDigits: 2,
                      })}
                    </label>
                  </div>
                </div>
              </fieldset>

              <fieldset>
                <legend>Total General:</legend>

                <div className="row">
                  <div className="col">
                    <b>
                      <label>Total: </label>
                    </b>
                  </div>
                  <div className="col">
                    <label style={{ textAlign: "right" }}>
                      ${" "}
                      {this.state.montofinal.toLocaleString(undefined, {
                        maximumFractionDigits: 2,
                      })}
                    </label>
                  </div>
                </div>
              </fieldset>

              <Button
                type="button"
                onClick={(e) => this.setState({ visibleRight: false })}
                label="Cerrrar"
                className="p-button-secondary"
              />
            </BlockUi>
          </Sidebar>

          <Button
            label="Mostrar Presupuesto"
            icon="pi pi-arrow-right"
            onClick={(e) =>
              this.setState(
                { visibleRight: true, blockmonto: true },
                this.GetMontosPresupuesto
              )
            }
            style={{ marginRight: ".25em" }}
          />
        </div>
        <div className="row">
          <div
            className="col-sm-8"
            style={{ overflowX: "scroll", overflowY: "scroll" }}
          >
            <TreeWbs
              key={this.state.key}
              onSelectionChange={this.onSelectionChange}
              data={this.state.data}
            />
          </div>

          <div className="col-sm-4">
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
                disable={this.state.canSubmit}
              >
                Actualizar
              </button>
              &nbsp;
              <button
                type="button"
                className="btn btn-outline-primary"
                onClick={() => this.deleteItem(this.state.computoId)}
              >
                {" "}
                Eliminar
              </button>
            </form>

            <Dialog
              header="Ingreso de Computos"
              visible={this.state.visiblecomputo}
              width="700px"
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
          this.showSuccess();
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
          this.showWarn();
        }
      })
      .catch((error) => {
        console.log(error);
        this.showWarn();
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
        this.showWarnE();
      });
  }
  onHide(event) {
    this.setState({ visible: false, blockmonto: false });
    this.GetMontosPresupuesto();
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

  showSuccess() {
    this.growl.show({
      life: 5000,
      severity: "success",
      summary: "Proceso exitoso!",
      detail: this.state.message,
    });
  }

  showWarn() {
    this.growl.show({
      life: 5000,
      severity: "error",
      summary: "Error",
      detail: this.state.message,
    });
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
  GetMontosPresupuesto() {
    axios
      .post("/proyecto/Presupuesto/ActualizarCostosRdo", {
        contratoId: document.getElementById("ContratoId").className,
        oferta: document.getElementById("OfertaId").className,
      })
      .then((response) => {
        this.updateData();
      })
      .catch((error) => {
        console.log(error);
      });
    axios
      .post("/proyecto/Computo/MontosPresupuestos", {
        id: document.getElementById("OfertaId").className,
      })
      .then((response) => {
        this.setState({
          blockmonto: false,
          montopc: response.data[0],
          montopi: response.data[1],
          montop: response.data[2],
          montoa: response.data[4],
          montoi: response.data[5],
          montou: response.data[6],
          montoprc: response.data[7],
          montototal: response.data[8],
          montofinal: response.data[9],
          blocking: false,
        });
      })
      .catch((error) => {
        console.log(error);
      });
  }
}

ReactDOM.render(
  <CreateTreeComputos />,
  document.getElementById("content-computos")
);
