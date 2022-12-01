import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import { Button } from "primereact/components/button/Button";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import { Sidebar } from "primereact-v2/sidebar";
import ArbolWbs from "./WbsPresupuesto/ArbolWbs";
import UploadPdfForm from "./Presupuestos/UploadPdfForm";
import { Card } from "primereact-v2/card";
import { ListBox } from "primereact-v2/listbox";
import ComputoPresupuestoForm from "./Presupuestos/ComputoPresupuestoForm";

export default class ComputosPresupuesto extends React.Component {
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

      //Subida de Archivo

      upload: false,
      uploadFile: "",
      visibleSecondFormat: false,

      /*Second Format */
      secondvalues: null,
      formartc: "",

      visibleNegativos: false,
      datosnegativos: [],
      visibleDuplicados: false,
      messageDuplicado: "",
    };
    this.getItems = this.getItems.bind(this);
    this.getItemsProcura = this.getItemsProcura.bind(this);
    this.getunidades = this.getunidades.bind(this);
    this.deleteItem = this.deleteItem.bind(this);
    this.updateData = this.updateData.bind(this);
    this.onSelect = this.onSelect.bind(this);
    this.onHide = this.onHide.bind(this);
    this.onHideComputo = this.onHideComputo.bind(this);
    this.selectComputo = this.selectComputo.bind(this);
    this.showDialog = this.showDialog.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.showSuccess = this.showSuccess.bind(this);
    this.showWarn = this.showWarn.bind(this);

    this.GetMontosPresupuesto = this.GetMontosPresupuesto.bind(this);

    //SUBIDA DE ARCHIVOS
    this.handleChangeUploadFile = this.handleChangeUploadFile.bind(this);
    this.handleSubmitUploadFile = this.handleSubmitUploadFile.bind(this);
    this.handleValidateNegativeUploadFile = this.handleValidateNegativeUploadFile.bind(
      this
    );
    this.mostrarUpload = this.mostrarUpload.bind(this);
    this.ocultarUpload = this.ocultarUpload.bind(this);
    this.DescargarFormatoSubidaComputos = this.DescargarFormatoSubidaComputos.bind(
      this
    );
  }

  componentWillMount() {
    this.updateData();
    this.getItems();
    this.getItemsProcura();
    this.getunidades();
  }

  onSelect(e) {
    if (e.node.tipo == "actividad") {
      var ids = e.node.data.split(",");
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
    if (e.node.tipo == "computo") {
      var ids = e.node.data.split("!");
      this.setState({
        WbsOfertaId: ids[2],
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
        <div className="row">
          <div className="col">
            {this.state.formartc != null &&
              this.state.formartc === "Contrato_2016" && (
                <button
                  className="btn btn-outline-primary"
                  style={{ marginLeft: "0.3em" }}
                  onClick={(e) =>
                    this.setState(
                      { visibleRight: true, blockmonto: true },
                      this.GetMontosPresupuesto
                    )
                  }
                >
                  <i className="fa fa-eye"> Mostrar Presupuesto</i>
                </button>
              )}
            {this.state.formartc != null &&
              this.state.formartc === "Contrato_2019" && (
                <button
                  className="btn btn-outline-indigo"
                  style={{ marginLeft: "0.3em" }}
                  onClick={(e) =>
                    this.setState(
                      { visibleSecondFormat: true, blockmonto: true },
                      this.GetMontosSecondFormat
                    )
                  }
                >
                  <i className="fa fa-eye"> Mostrar Presupuesto</i>
                </button>
              )}
          </div>
          <div className="col" align="right">
            <button
              className="btn btn-outline-primary"
              style={{ marginLeft: "0.3em" }}
              onClick={() => this.DescargarFormatoSubidaComputos()}
            >
              <i className="fa fa-download"> Descargar Excel</i>
            </button>

            <button
              className="btn btn-outline-primary"
              style={{ marginLeft: "0.3em" }}
              onClick={() => this.mostrarUpload()}
            >
              <i className="fa fa-cloud-upload"> Subir Excel</i>
            </button>
          </div>
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
                      <label style={{ fontSize: "12px" }}>Ingenieria: </label>
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

          <Sidebar
            visible={this.state.visibleSecondFormat}
            position="right"
            baseZIndex={1000000}
            onHide={(e) => this.setState({ visibleSecondFormat: false })}
            className="p-sidebar-lg"
            style={{ width: "45em" }}
          >
            <BlockUi tag="div" blocking={this.state.blockmonto}>
              <div className="container">
                <h6>
                  <b>PRESUPUESTO:</b>
                </h6>
                <div className="row">
                  <div className="col" align="justify">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        COSTO TOTAL INGENIERÍA BASICA Y DETALLE (AIU):{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.A_VALOR_COSTO_TOTAL_INGENIERÍA_BASICA_YDETALLE_AIU_ANEXO1.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>

                <div className="row">
                  <div className="col" align="justify">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        VALOR PROCURA:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.VALOR_PROCURA.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>

                <div className="row">
                  <div className="col" align="justify">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        Administracion sobre Procura Contratista (%):{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.Administracion_sobre_Procura_Contratista.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>

                <div className="row">
                  <div className="col" align="justify">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        COSTO DIRECTO PROCURA CONTRATISTA:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.B_VALOR_COSTO_DIRECTO_PROCURA_CONTRATISTA_ANEXO2.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>

                <div className="row">
                  <div className="col" align="justify">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        VALOR SUBCONTRATOS:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.VALOR_SUBCONTRATOS.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>

                <div className="row">
                  <div className="col" align="justify">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        Administracion sobre Subcontratos Contratista (%):{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.Administracion_sobre_Subcontratos_Contratista.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>

                <div className="row">
                  <div className="col" align="justify">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        COSTO DIRECTO SUBCONTRATOS CONTRATISTA:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.C_VALOR_COSTO_DIRECTO_SUBCONTRATOS_CONTRATISTA.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>

                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        VALOR COSTO DIRECTO OBRAS CIVILES:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.VALOR_COSTO_DIRECTO_OBRAS_CIVILES.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>

                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        VALOR COSTO DIRECTO OBRAS MECÁNICAS{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>

                <div className="row">
                  <div className="col" align="justify">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        VALOR COSTO DIRECTO OBRAS ELECTRICAS{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
                <div className="row">
                  <div className="col" align="justify">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        VALOR COSTO DIRECTO OBRAS INSTRUMENTOS & CONTROL{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        VALOR COSTO DIRECTO SERVICIOS ESPECIALES{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.VALOR_COSTO_DIRECTO_SERVICIOS_ESPECIALES.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        VALOR COSTO DIRECTO SERVICIOS ESPECIALES{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.VALOR_COSTO_DIRECTO_SERVICIOS_ESPECIALES.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        DESCUENTO 1% ITEMS MECÁNICOS, ELÉCTRICOS E
                        INSTRUMENTACIÓN{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.DESCUENTO_ITEMS_MECÁNICOS_ELÉCTRICOS_INSTRUMENTACIÓN.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        VALOR COSTO DIRECTO CONSTRUCCIÓN:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        Administracion sobre Obra (%):{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.Administracion_sobre_Obra.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        Imprevistos sobre Obra (%):{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.Imprevistos_sobre_Obra.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        Utilidad sobre Obra (%):{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.Utilidad_sobre_Obra.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        INDIRECTOS SOBRE VALOR COSTO DIRECTO CONSTRUCCIÓN:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.E_INDIRECTOS_SOBRE_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
                <div className="row">
                  <div className="col" align="left">
                    <b>
                      <label style={{ fontSize: "11px" }}>
                        COSTO TOTAL DEL PROYECTO:{" "}
                      </label>
                    </b>
                  </div>
                  <div className="col" align="right">
                    ${" "}
                    {this.state.secondvalues != null
                      ? this.state.secondvalues.COSTO_TOTAL_DEL_PROYECTO_ABCDE.toLocaleString(
                          undefined,
                          {
                            maximumFractionDigits: 2,
                          }
                        )
                      : 0}
                  </div>
                </div>
              </div>
            </BlockUi>
          </Sidebar>
        </div>
        <hr />
        <div className="row">
          <div
            className="col-sm-8"
            style={{
              maxHeight: "550px",
              overflowX: "scroll",
              overflowY: "scroll",
            }}
          >
            <ArbolWbs
              onDragDrop={this.onDragDrop}
              onSelect={this.onSelect}
              data={this.state.data}
              expandedKeys={this.state.expandedKeys}
            />
            <Sidebar
              visible={this.state.visibleNegativos}
              position="top"
              className="p-sidebar-lg"
              baseZIndex={1000000}
              onHide={() => this.setState({ visibleNegativos: false })}
            >
              <h4 style={{ fontWeight: "normal" }}>
                Considerar que existen valores negativos en los siguientes
                items, desea continuar?
              </h4>
              <ListBox
                options={this.state.datosnegativos}
                optionLabel="name"
                style={{ width: "50rem" }}
                listStyle={{ maxHeight: "300px" }}
              />
              <hr />
              <Button
                type="button"
                onClick={() => this.handleSubmitUploadFile()}
                label="Continuar Carga"
                className="p-button-success"
                style={{ marginRight: ".25em" }}
              />
              <Button
                type="button"
                onClick={() => this.setState({ visibleNegativos: false })}
                label="Cancelar"
                className="p-button-secondary"
              />
            </Sidebar>
            <Dialog
              header="Subir Excel Cómputos"
              visible={this.state.upload}
              width="500px"
              modal={true}
              onHide={this.ocultarUpload}
            >
              <UploadPdfForm
                handleChange={this.handleChangeUploadFile}
                label="Anexo 10"
              />
            </Dialog>

            <Dialog
              header="No se puede continuar"
              visible={this.state.visibleDuplicados}
              style={{ width: "50vw" }}
              onHide={() => this.setState({ visibleDuplicados: false })}
            >
              {" "}
              {this.state.messageDuplicado.length > 0 && (
                <div>
                  <p>
                    {this.state.messageDuplicado} </p>
                    <p>Para solucionar puede tomar
                    dos opciones:{" "}</p>
                 
                  <p>1: modificar puntualmente en el sistema o</p>{" "}
                  <p>2: descargar matriz y corregir error previo a la carga.
                  </p>{" "}
                  <hr />
                  <Button
                    type="button"
                    onClick={() => this.setState({ visibleDuplicados: false })}
                    label="Cancelar"
                    className="p-button-secondary"
                  />
                </div>
              )}
            </Dialog>
          </div>

          <div className="col-sm-4">
            <form onSubmit={this.handleSubmit}>
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
                //disable={this.state.canSubmit}
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
              <ComputoPresupuestoForm
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

  handleChangeUploadFile = (event) => {
    if (event.files) {
      if (event.files.length > 0) {
        let uploadFile = event.files[0];
        this.setState(
          {
            uploadFile: uploadFile,
          },
          this.handleValidateNegativeUploadFile(uploadFile)
        );
      } else {
        this.setState(
          { message: "Error al Seleccionar Archivo" },
          this.showWarn
        );
      }
    } else {
      this.setState({ message: "Error al Seleccionar Archivo" }, this.showWarn);
    }
  };

  ocultarUpload = () => {
    this.setState({ upload: false, uploadFile: "" });
  };

  mostrarUpload = () => {
    this.setState({ upload: true });
  };

  handleValidateNegativeUploadFile = (uploadFile) => {
    this.setState({ blocking: true, datosnegativos: [] });

    const formData = new FormData();
    formData.append("UploadedFile", uploadFile);
    formData.append(
      "ofertaid",
      document.getElementById("PresupuestoId").className
    );
    const config = {
      headers: {
        "content-type": "multipart/form-data",
      },
    };

    axios
      .post(
        "/Proyecto/OfertaPresupuesto/ObtenerValoresNegativos/",
        formData,
        config
      )
      .then((response) => {
        console.log("VALIDATIONS UPLOAD", response);

        if (response.data[0] == "NEGATIVOS") {
          this.setState({
            visibleNegativos: true,
            blocking: false,
            datosnegativos: JSON.parse(response.data[1]).map((item) => {
              return {
                name: item,
                code: item + "_" + (Math.floor(Math.random() * 100) + 1),
              };
            }),
          });
        } else if (response.data[0] == "SA") {
          this.setState(
            { message: response.data[1], blocking: false },
            this.showWarn
          );
        } else {
          this.handleSubmitUploadFile();
        }
      })
      .catch((error) => {
        console.log(error);
        this.setState({ message: error, blocking: false }, this.showWarn);
      });
  };

  handleSubmitUploadFile = () => {
    this.setState({ visibleNegativos: false, blocking: true });

    const formData = new FormData();
    formData.append("UploadedFile", this.state.uploadFile);
    formData.append(
      "ofertaid",
      document.getElementById("PresupuestoId").className
    );

    const config = {
      headers: {
        "content-type": "multipart/form-data",
      },
    };

    axios
      .post("/Proyecto/OfertaPresupuesto/SubirExcel/", formData, config)
      .then((response) => {
        console.log("RESULTADO UPLOAD", response);
        if (response.data[0] == "OK") {
          this.setState(
            {
              message: response.data[1],
              upload: false,
              visibleNegativos: false,
              visibleDuplicados: false,
            },
            this.showSuccess
          );
          this.updateData();
        }
        if (response.data[0] == "Error") {
          this.setState(
            {
              message: response.data[1],
              blocking: false,
              messageDuplicado: response.data[1],
              visibleDuplicados: true,
            },
            this.showWarn
          );
        }
        if (response.data[0] == "SA") {
          this.setState(
            { message: response.data[1], blocking: false },
            this.showWarn
          );
        }
      })
      .catch((error) => {
        console.log(error);
        this.setState({ message: error, blocking: false }, this.showWarn);
      });
  };

  DescargarFormatoSubidaComputos() {
    this.setState({ blocking: true });
    axios
      .get(
        "/Proyecto/OfertaPresupuesto/ExportarE/" +
          document.getElementById("PresupuestoId").className,
        { responseType: "arraybuffer" }
      )
      .then((response) => {
        console.log(response.headers["content-disposition"]);
        var nombre = response.headers["content-disposition"].split("=");

        const url = window.URL.createObjectURL(
          new Blob([response.data], {
            type:
              "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          })
        );
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", nombre[1]);
        document.body.appendChild(link);
        link.click();
        this.setState(
          { message: "Formato Descargado", blocking: false },
          this.showSuccess
        );
      })
      .catch((error) => {
        console.log(error);
        this.setState(
          { message: "Ocurrió un Error Inténtelo más tarde", blocking: false },
          this.showWarn
        );
      });
  }

  handleSubmit(event) {
    event.preventDefault();
    this.setState({ blocking: true });
    axios
      .post("/proyecto/ComputoPresupuesto/EditComputo", {
        Id: this.state.computoId,
        cantidad: this.state.cantidad,
        precio_unitario: this.state.precio_unitario,
        costo_total: this.state.precio_unitario * this.state.cantidad,
        vigente: true,
        WbsPresupuestoId: this.state.WbsOfertaId,
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
        "/proyecto/ComputoPresupuesto/ApiComputo/" +
          document.getElementById("PresupuestoId").className,
        {}
      )
      .then((response) => {
        this.setState({ data: response.data, blocking: false });
      })
      .catch((error) => {
        console.log(error);
        this.setState({ blocking: false });
      });
    var formato = document.getElementById("FId").className;
    this.setState({ formartc: formato });
    console.log("formato", this.state.formartc);
  }

  deleteItem(id) {
    axios
      .post(
        "/proyecto/ComputoPresupuesto/DeleteComputoArbol/" +
          this.state.computoId,
        {}
      )
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
    //this.GetMontosPresupuesto();
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
      life: 7000,
      severity: "error",
      summary: "Error",
      detail: this.state.message,
    });
  }

  getItems() {
    axios
      .post(
        "/Proyecto/ComputoPresupuesto/ItemsparaOfertaC/" +
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
      .post("/Proyecto/ComputoPresupuesto/ItemsProcura")
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
  GetMontosSecondFormat = () => {
    axios
      .post("/proyecto/OfertaPresupuesto/ActualizarCostos", {
        Id: document.getElementById("PresupuestoId").className,
      })
      .then((response) => {
        console.log(response.data);
      })
      .catch((error) => {
        console.log(error);
      });

    axios
      .post("/Proyecto/ComputoPresupuesto/ItemsSecondFormat", {
        Id: document.getElementById("PresupuestoId").className,
      })
      .then((response) => {
        console.log(response.data);
        this.setState({ secondvalues: response.data, blockmonto: false });
      })
      .catch((error) => {
        this.setState({ blockmonto: false });
        console.log(error);
      });
  };

  GetMontosPresupuesto() {
    axios
      .post("/proyecto/OfertaPresupuesto/ActualizarCostos", {
        Id: document.getElementById("PresupuestoId").className,
      })
      .then((response) => {
        console.log(response.data);
        this.setState({ blockmonto: false });
      })
      .catch((error) => {
        this.setState({ blockmonto: false });
        console.log(error);
      });

    axios
      .post("/proyecto/OfertaPresupuesto/MontosPresupuestos", {
        id: document.getElementById("PresupuestoId").className,
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
        this.updateData();
      })
      .catch((error) => {
        console.log(error);
      });
  }
}

ReactDOM.render(
  <ComputosPresupuesto />,
  document.getElementById("content-computos")
);
