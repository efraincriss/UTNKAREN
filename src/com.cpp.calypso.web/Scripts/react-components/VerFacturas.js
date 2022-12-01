import React from "react";
import ReactDOM from "react-dom";
import Moment from "moment";
import axios from "axios";
import BlockUi from "react-block-ui";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import { Dropdown } from "primereact/components/dropdown/Dropdown";
import { Button } from "primereact/components/button/Button";
import { InputText } from "primereact-v2/components/inputtext/InputText";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import CurrencyFormat from "react-currency-format";

class VerFacturas extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      blocking: true,
      lista_facturas: [],
      lista_empresas: [],
      lista_clientes: [],
      visible_editar_factura: false,

      //EDITAR EMPRESA
      EmpresaId: 0,
      ClienteId: 0,
      numero_documento: 0,
      tipo_documento: "",
      fecha_emision: null,
      fecha_vencimiento: null,
      descripcion: "",
      proyecto: "",
      codigosap: "",
      documento_compensación: "",
      codigoauxicial: "", //ov
      codigoprincipal: "", //OS
      valor_importe: 0,
      valor_iva: 0,
      valor_total: 0,
      valor_a_cobrar: 0,

      //Retenciones
      documento_retencion: "",
      total_retencion: 0,
      retencion_iva: 0,
      retencion_ir: 0,
      //Cobros
      documento_cobro: "",
      valor_cobrado: 0,

      key_form: 89247,
    };
    this.onHideVisibleActividadForm = this.onHideVisibleActividadForm.bind(
      this
    );
    this.showForm = this.showForm.bind(this);
    this.OnEdit = this.OnEdit.bind(this);
    this.ObtenerFacturas = this.ObtenerFacturas.bind(this);
    this.ObtenerEmpresas = this.ObtenerEmpresas.bind(this);
    this.ObtenerClientes = this.ObtenerClientes.bind(this);
    this.EmpresaNombreFormato = this.EmpresaNombreFormato.bind(this);
    this.ClienteNombreFormato = this.ClienteNombreFormato.bind(this);
    this.dateFormat = this.dateFormat.bind(this);
    this.OcultarFormularioEdicion = this.OcultarFormularioEdicion.bind(this);
    this.enumFormatter = this.enumFormatter.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.VerFactura = this.VerFactura.bind(this);
    this.FacturaMonto = this.FacturaMonto.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }
  componentDidMount() {
    this.ObtenerFacturas();
    this.ObtenerEmpresas();
    this.ObtenerClientes();
  }

  ObtenerFacturas() {
    axios
      .get("/proyecto/Factura/ObtenerFacturas/", {})
      .then((response) => {
        console.log(response.data);
        this.setState({ lista_facturas: response.data, blocking: false });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  FacturaMonto(cell, row) {
    return (
      <CurrencyFormat
        value={cell}
        displayType={"text"}
        thousandSeparator={true}
        prefix={"$"}
      />
    );
  }
  FacturaImporte(cell, row) {
    return (
      <CurrencyFormat
        value={cell}
        displayType={"text"}
        thousandSeparator={true}
        prefix={"$"}
      />
    );
  }

  VerFactura(id) {
    console.log(id);
    window.location.href = "/proyecto/Factura/Details/" + id;
  }
  OcultarFormularioEdicion() {
    this.setState({ visible_editar_factura: false });
  }
  MostrarFormularioEdicion(e) {
    console.log(e);
    this.setState({ visible_editar_factura: true });
    axios
      .get("/proyecto/Factura/ObtenerFacturasD/" + e, {})
      .then((response) => {
        console.log(response.data);
        this.setState({
          EmpresaId: response.data.EmpresaId,
          ClienteId: response.data.ClienteId,
          numero_documento: response.data.numero_documento,
          descripcion: response.data.descripcion,
          codigosap: response.data.codigosap,
          tipo_documento: response.data.tipo_documento,
          valor_iva: response.data.valor_iva,
          valor_total: response.data.valor_total,
          valor_a_cobrar: response.data.valor_a_cobrar,
          valor_importe: response.data.valor_importe,
          valor_cobrado: response.data.valor_cobrado,
          documento_compensación: response.data.doc_compensacion,
          codigoauxicial: "" + response.data.os,
          codigoprincipal: "" + response.data.ov,
          documento_retencion: response.data.numero_retencion,
          fecha_emision: response.data.fecha_emision,
          fecha_vencimiento: response.data.fecha_vencimiento,
          retencion_ir: response.data.retencion_ir,
          retencion_iva: response.data.retencion_iva,
          total_retencion:
            response.data.retencion_ir + response.data.retencion_iva,
          documento_cobro: response.data.doc_compensacion,
        });
      })
      .catch((error) => {
        console.log(error);
        this.growl.show({
          severity: "warn",
          summary: "Error",
          detail: "No se Cargo la Factura",
        });
      });
  }
  ObtenerEmpresas() {
    axios
      .get("/proyecto/Factura/ObtenerFacturasE/", {})
      .then((response) => {
        var empresas = response.data.map((i) => {
          return { label: i.razon_social, value: i.Id };
        });
        this.setState({ lista_empresas: empresas, blocking: false });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  ObtenerClientes() {
    axios
      .get("/proyecto/Factura/ObtenerFacturasC/", {})
      .then((response) => {
        var clientes = response.data.map((i) => {
          return { label: i.razon_social, value: i.Id };
        });
        this.setState({ lista_clientes: clientes, blocking: false });
      })
      .catch((error) => {
        console.log(error);
      });
  }
  generateButton(cell, row) {
    return (
      <div>
        <button
          className="btn btn-outline-success btn-sm"
          onClick={() => {
            this.VerFactura(row.Id);
          }}
          style={{ float: "left", marginRight: "0.3em" }}
        >
          Ver
        </button>
        <button
          className="btn btn-outline-info btn-sm"
          onClick={() => {
            this.MostrarFormularioEdicion(row.Id);
          }}
          style={{ float: "left", marginRight: "0.3em" }}
        >
          Editar
        </button>
      </div>
    );
  }

  dateFormat(cell, row) {
    if (cell === null) {
      return "dd/mm/yy";
    }
    return Moment(cell).format("DD-MM-YYYY");
  }
  CatalogoNombre(cell, row) {
    return cell.nombre;
  }

  handleSubmit(event) {
    axios
      .post("/Proyecto/DetalleAvanceIngenieria/Create", {
        Id: this.state.Id,
        AvanceIngenieriaId: document.getElementById("AvanceIngenieriaId")
          .className,
        tipo_registro: this.state.registro,
        ComputoId: this.state.computo,
        cantidad_horas: this.state.cantidad_horas,
        vigente: true,
        valor_real: 0,
        fecha_real: new Date(),
      })
      .then((response) => {
        this.props.updateData();
        this.props.showSuccess("Ingresado correctamente");
        this.props.onHide();
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("Intentalo mas tarde");
      });
  }

  render() {
    return (
      <div>
        <Growl
          ref={(el) => {
            this.growl = el;
          }}
          position="bottomright"
          baseZIndex={1000}
        ></Growl>
        <BlockUi tag="div" blocking={this.state.blocking}>
          <BootstrapTable
            data={this.state.lista_facturas}
            hover={true}
            pagination={true}
          >
            <TableHeaderColumn
              dataField="numero_documento"
              filter={{ type: "TextFilter", delay: 500 }}
              isKey={true}
              width={"10%"}
              dataAlign="center"
              dataSort={true}
              tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            >
              {" "}
              #Factura
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="nombreEmpresa"
              //dataFormat={this.EmpresaNombreFormato}
              filter={{ type: "TextFilter", delay: 500 }}
              dataAlign="center"
              dataSort={true}
              width={"10%"}
              tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            >
              Empresa
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="nombreCliente"
              //dataFormat={this.ClienteNombreFormato}
              filter={{ type: "TextFilter", delay: 500 }}
              dataAlign="center"
              dataSort={true}
              width={"10%"}
              tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            >
              Cliente
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="formatFechaEmision"
              filter={{ type: "TextFilter", delay: 500 }}
            //  dataFormat={this.dateFormat}
              dataSort={true}
              width={"10%"}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "10px",
                textAlign: "center",
              }}
              thStyle={{
                whiteSpace: "normal",
                fontSize: "10px",
                textAlign: "center",
              }}
            >
              Fecha
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="valor_importe"
              dataFormat={this.FacturaImporte}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              width={"10%"}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "10px",
                textAlign: "right",
              }}
              thStyle={{
                whiteSpace: "normal",
                fontSize: "10px",
                textAlign: "center",
              }}
            >
              Importe
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="valor_total"
              dataFormat={this.FacturaMonto}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              width={"10%"}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "10px",
                textAlign: "right",
              }}
              thStyle={{
                whiteSpace: "normal",
                fontSize: "10px",
                textAlign: "center",
              }}
            >
              Monto X Cobrar
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="nombreEstado"
              //dataFormat={this.CatalogoNombre}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              width={"10%"}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "10px",
                textAlign: "right",
              }}
              thStyle={{
                whiteSpace: "normal",
                fontSize: "10px",
                textAlign: "center",
              }}
            >
              Estado
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Operaciones"
              width={"15%"}
              dataFormat={this.generateButton.bind(this)}
              tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            >
              Operaciones
            </TableHeaderColumn>
          </BootstrapTable>

          <Dialog
            header="Detalles de la Factura"
            visible={this.state.visible_editar_factura}
            width="900px"
            modal={true}
            onHide={this.OcultarFormularioEdicion}
            resizable={true}
            closeOnEscape={true}
            dismissableMask={true}
            responsive={true}
          >
            <form onSubmit={this.handleSubmit}>
              <fieldset>
                <legend>Factura:</legend>
                <div className="row">
                  <div className="col">
                    <div className="form-group">
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Empresa:
                      </label>
                      <div>
                        <Dropdown
                          value={this.state.EmpresaId}
                          options={this.state.lista_empresas}
                          onChange={(e) => {
                            this.setState({ EmpresaId: e.value });
                          }}
                          filter={true}
                          filterPlaceholder="Selecciona una Empresa"
                          filterBy="label,value"
                          placeholder="Selecciona una Empresa"
                          style={{ width: "100%" }}
                          disabled
                        />
                      </div>
                    </div>
                    <div className="form-group">
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Número de Documento:
                      </label>
                      <InputText
                        value={this.state.numero_documento}
                        onChange={(e) =>
                          this.setState({ numero_documento: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                    <div className="form-group">
                      <label>Fecha Emisión</label>
                      <input
                        type="date"
                        id="no-filter"
                        name="fecha_emision"
                        className="form-control"
                        onChange={this.handleChange}
                        value={Moment(this.state.fecha_emision).format(
                          "YYYY-MM-DD"
                        )}
                        disabled
                      />
                    </div>

                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Descripción:
                      </label>
                      <InputText
                        value={this.state.descripcion}
                        onChange={(e) =>
                          this.setState({ descripcion: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Código Sap:
                      </label>
                      <InputText
                        value={this.state.codigosap}
                        onChange={(e) =>
                          this.setState({ codigosap: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Código Auxiliar:
                      </label>
                      <InputText
                        value={this.state.codigoauxicial}
                        onChange={(e) =>
                          this.setState({ codigoauxicial: e.target.value })
                        }
                        style={{ width: "100%" }}
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Valor Importe:
                      </label>
                      <InputText
                        value={this.state.valor_importe}
                        type="number"
                        onChange={(e) =>
                          this.setState({ valor_importe: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Valor Total:
                      </label>
                      <InputText
                        value={this.state.valor_total}
                        type="number"
                        onChange={(e) =>
                          this.setState({ valor_total: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                  </div>
                  <div className="col">
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Cliente:
                      </label>

                      <Dropdown
                        value={this.state.ClienteId}
                        options={this.state.lista_clientes}
                        onChange={(e) => {
                          this.setState({ ClienteId: e.value });
                        }}
                        filter={true}
                        filterPlaceholder="Selecciona un Cliente"
                        filterBy="label,value"
                        placeholder="Selecciona un Cliente"
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Tipo de Documento:
                      </label>
                      <InputText
                        value={this.state.tipo_documento}
                        onChange={(e) =>
                          this.setState({ tipo_documento: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>

                    <div className="form-group">
                      <label>Fecha Vencimiento</label>
                      <input
                        type="date"
                        id="no-filter"
                        name="fecha_vencimiento"
                        className="form-control"
                        onChange={this.handleChange}
                        value={Moment(this.state.fecha_vencimiento).format(
                          "YYYY-MM-DD"
                        )}
                        disabled
                      />
                    </div>

                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Proyecto:
                      </label>
                      <InputText
                        value={this.state.proyecto}
                        onChange={(e) =>
                          this.setState({ proyecto: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Documento Compensación:
                      </label>
                      <InputText
                        value={this.state.documento_compensación}
                        onChange={(e) =>
                          this.setState({
                            documento_compensación: e.target.value,
                          })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Código Principal:
                      </label>
                      <InputText
                        value={this.state.codigoprincipal}
                        onChange={(e) =>
                          this.setState({ codigoprincipal: e.target.value })
                        }
                        style={{ width: "100%" }}
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Valor IVA:
                      </label>
                      <InputText
                        value={this.state.valor_iva}
                        type="number"
                        onChange={(e) =>
                          this.setState({ valor_iva: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Valor a Cobrar:
                      </label>
                      <InputText
                        value={this.state.valor_a_cobrar}
                        type="number"
                        onChange={(e) =>
                          this.setState({ valor_a_cobrar: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                  </div>
                </div>
              </fieldset>

              <fieldset>
                <legend>Retenciones:</legend>

                <div className="row">
                  <div className="col">
                    <div className="form-group">
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Documento de Retención:
                      </label>
                      <InputText
                        value={this.state.documento_retencion}
                        onChange={(e) =>
                          this.setState({ documento_retencion: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Total Retención:
                      </label>
                      <InputText
                        value={this.state.total_retencion}
                        onChange={(e) =>
                          this.setState({ total_retencion: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                  </div>
                  <div className="col">
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Retención IVA:
                      </label>
                      <InputText
                        value={this.state.retencion_iva}
                        onChange={(e) =>
                          this.setState({ retencion_iva: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Retención IR:
                      </label>
                      <InputText
                        value={this.state.retencion_ir}
                        onChange={(e) =>
                          this.setState({ retencion_ir: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                  </div>
                </div>
              </fieldset>
              <fieldset>
                <legend>Cobros:</legend>
                <div className="row">
                  <div className="col">
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Documento Cobro:
                      </label>
                      <InputText
                        value={this.state.documento_cobro}
                        onChange={(e) =>
                          this.setState({ documento_cobro: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                  </div>
                  <div className="col">
                    <div className="form-group" style={{ heigh: "10px" }}>
                      <label htmlFor="label" style={{ fontSize: "12px" }}>
                        Valor cobrado:
                      </label>
                      <InputText
                        value={this.state.valor_cobrado}
                        onChange={(e) =>
                          this.setState({ valor_cobrado: e.target.value })
                        }
                        style={{ width: "100%" }}
                        disabled
                      />
                    </div>
                  </div>
                </div>
              </fieldset>
              <Button
                type="submit"
                label="Guardar"
                icon="fa fa-fw fa-folder-open"
              />
              <Button
                type="button"
                label="Cancelar"
                icon="fa fa-fw fa-ban"
                onClick={this.OcultarFormularioEdicion}
              />
            </form>
          </Dialog>
        </BlockUi>
      </div>
    );
  }
  handleChange(event) {
    event.stopPropagation();

    this.setState({ [event.target.name]: event.target.value });
  }

  onHideVisibleActividadForm(event) {
    this.setState({ visible_actividades_form: false });
  }
  enumFormatter(cell, row, enumObject) {
    return enumObject[cell];
  }
  EmpresaNombreFormato(cell, row) {
    return cell.razon_social;
  }
  ClienteNombreFormato(cell, row) {
    return cell.razon_social;
  }
  OnEdit(id) {
    axios
      .post("/proyecto/Wbs/Delete/" + id, {})
      .then((response) => {
        if (response.data == "Ok") {
          this.props.showSuccess("Se eliminó el registro");
          this.props.updateData();
          this.props.reset();
        } else {
          this.props.showWarn("La actividad tiene computos registrados");
        }
      })
      .catch((error) => {
        this.props.showWarn("No se puedo eliminar el registro");
      });
  }

  showForm() {
    this.setState({
      visible_editar_factura: true,
      key_form: Math.random(),
    });
  }
}

ReactDOM.render(
  <VerFacturas />,
  document.getElementById("content-viewfactura")
);
