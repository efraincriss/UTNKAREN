import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import moment from "moment";
import CurrencyFormat from "react-currency-format";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import { Dropdown } from "primereact/components/dropdown/Dropdown";
import { Checkbox } from "primereact/components/checkbox/Checkbox";

class OfertasComerciales extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: true,
      data: [],

      visible: false,

      // Inputs del Formulario

      fecha_registro: moment(new Date()).format("DD-MM-YYYY"),

      //OfertaComercial
      contratos: [],
      ContratoId: 0,
      ClaseId: 0,
      descripcion: "",
      codigo: "Por Generar",
      version: "A",
      tipo_trabajo: [],
      tipo_trabajo_id: 0,
      tipo_contratacion: [],
      tipo_contratacion_id: 0,
      centro_costos: [],
      centro_costos_id: 0,
      estado_oferta: [],
      estado_oferta_id: 0,
      estatus_ejecucion: [],
      estatus_ejecucion_id: 0,
      ingenieria: [],
      ingenieria_id: 0,
      alcance: 4172,
      alcances: [],
      codigo_shaya: "",
      estado: 2027,
      service_request: false,
      service_order: false,
      revision: "",
      actacierreid: 0,
      comentarios: "",

      //referenci
    };

    this.handleChange = this.handleChange.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind(this);

    this.alertMessage = this.alertMessage.bind(this);
    this.infoMessage = this.infoMessage.bind(this);
    this.EnviarFormulario = this.EnviarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);

    this.GenerarBotones = this.GenerarBotones.bind(this);
    this.IrPrueba = this.IrPrueba.bind(this);

    //ofertaComercial
    this.ObtenerCatalogos = this.ObtenerCatalogos.bind(this);
    this.ConsultarSR = this.ConsultarSR.bind(this);

    //
  }

  componentDidMount() {
    this.ConsultarSR();
    this.getContratos();
    this.ObtenerCatalogos();
  }
  ObtenerCatalogos() {
    //tipotrabajo
    axios
      .post("/proyecto/catalogo/GetCatalogo/1003", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ tipo_trabajo: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //tipo contratacion
    axios
      .post("/proyecto/catalogo/GetCatalogo/2005", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ tipo_contratacion: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //Centro de Costos
    axios
      .post("/proyecto/catalogo/GetCatalogo/1004", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ centro_costos: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //estadooferta
    axios
      .post("/proyecto/catalogo/GetCatalogo/1005", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ estado_oferta: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //estatus ejecución
    axios
      .post("/proyecto/catalogo/GetCatalogo/1006", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ estatus_ejecucion: items });
      })
      .catch((error) => {
        console.log(error);
      });
    //ingenieria
    axios
      .post("/proyecto/catalogo/GetCatalogo/3012", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ ingenieria: items });
      })
      .catch((error) => {
        console.log(error);
      });

    //alcances
    axios
      .post("/proyecto/catalogo/GetCatalogo/3013", {})
      .then((response) => {
        var items = response.data.map((item) => {
          return { label: item.nombre, dataKey: item.Id, value: item.Id };
        });
        this.setState({ alcances: items });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  IrPrueba(id) {
    console.log(id);
    window.location.href = "/Proyecto/OfertaComercial/DetailsOferta/" + id;
  }

  IraProyecto(row) {
    console.log(row);
    if (row.proyecto_ligados_id != null) {
      var Id = row.proyecto_ligados_id.split(" ; ");
      console.log(Id);
      if (Id.length > 0) {
        console.log(Id[0]);
        window.location.href =
          "/Proyecto/Proyecto/Details?id=" + Id[0] + "&idOferta=" + row.Id;
      }
    }
  }
  GenerarBotones(cell, row) {
    return (
      <div>
        <button
          className="btn btn-outline-success btn-sm"
          onClick={() => {
            this.IrPrueba(row.Id);
          }}
          style={{ float: "left", marginRight: "0.3em" }}
        >
          Ver
        </button>

        {row.proyecto_ligados_id != null && row.proyecto_ligados_id !== "" && (
          <button
            className="btn btn-outline-primary btn-sm"
            //   style={{ float: "left", marginRight: "0.3em" }}
            onClick={() => {
              this.IraProyecto(row);
            }}
          >
            Ir a Proyecto
          </button>
        )}
      </div>
    );
  }
  MontosContruccion = (cell, row) => {
    if (cell == null) {
      return (
        <CurrencyFormat
          value={0}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    } else {
      return (
        <CurrencyFormat
          value={cell}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  };
  render() {
    const tableStyle = { whiteSpace: "normal", fontSize: "12px" };

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
            <div className="form-group">
              <label htmlFor="label">Contrato</label>
              <Dropdown
                value={this.state.ContratoId}
                options={this.state.contratos}
                onChange={(e) => {
                  this.setState({ ContratoId: e.value }),
                    this.ConsultarSRC(e.value);
                }}
                filter={true}
                filterPlaceholder="Selecciona un Contrato"
                filterBy="label,value"
                placeholder="Selecciona una Contrato"
                style={{ width: "100%" }}
              />
            </div>
          </div>
          <div className="col" align="right">
           
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.MostrarFormulario}
            >
              Nuevo
            </button>
          </div>
        </div>

        <br />
        <br />
        <BootstrapTable data={this.state.data} hover={true} pagination={true}>
          <TableHeaderColumn
            dataField="codigo"
            isKey
            tdStyle={tableStyle}
            thStyle={tableStyle}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Oferta
          </TableHeaderColumn>
          <TableHeaderColumn
            width={"8%"}
            dataField="version"
            tdStyle={tableStyle}
            thStyle={tableStyle}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Versión
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="descripcion"
            tdStyle={tableStyle}
            thStyle={tableStyle}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Descripción
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="nombre_estado_proceso"
            tdStyle={tableStyle}
            thStyle={tableStyle}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Estado Proceso
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="nombre_estado"
            tdStyle={tableStyle}
            thStyle={tableStyle}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Estado
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="proyecto_ligados"
            tdStyle={tableStyle}
            thStyle={tableStyle}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Proyectos Ligados
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="monto_so_aprobado"
            tdStyle={{
              whiteSpace: "normal",
              fontSize: "12px",
              textAlign: "right",
            }}
            dataFormat={this.MontosContruccion}
            thStyle={{
              whiteSpace: "normal",
              fontSize: "12px",
              textAlign: "right",
            }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Monto Aprobado
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="tieneOrdenProceder"
            tdStyle={tableStyle}
            thStyle={tableStyle}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Tiene Orden Proceder
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="Operaciones"
            width={"15%"}
            dataFormat={this.GenerarBotones.bind(this)}
          ></TableHeaderColumn>
        </BootstrapTable>

        <Dialog
          header="Generación de Ofertas Comerciales"
          visible={this.state.visible}
          width="800px"
          modal={true}
          onHide={this.OcultarFormulario}
        >
          <form onSubmit={this.EnviarFormulario}>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label>Fecha Oferta</label>
                  <input
                    type="datetime-local"
                    id="no-filter"
                    name="fecha_registro"
                    className="form-control"
                    onChange={this.handleChange}
                    value={this.state.fecha_registro}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Centro de Costos</label>
                  <Dropdown
                    value={this.state.centro_costos_id}
                    options={this.state.centro_costos}
                    onChange={(e) => {
                      this.setState({ centro_costos_id: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Centro de Costos"
                    filterBy="label,value"
                    placeholder="Selecciona Centro de Costos"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Alcance</label>
                  <Dropdown
                    value={this.state.alcance}
                    options={this.state.alcances}
                    onChange={(e) => {
                      this.setState({ alcance: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona un Alcance"
                    filterBy="label,value"
                    placeholder="Selecciona un Alcance"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Forma Contratación</label>
                  <Dropdown
                    value={this.state.tipo_contratacion_id}
                    options={this.state.tipo_contratacion}
                    onChange={(e) => {
                      this.setState({ tipo_contratacion_id: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Tipo Contratación"
                    filterBy="label,value"
                    placeholder="Selecciona Tipo Contratación"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Estado</label>
                  <select
                    onChange={this.handleChange}
                    className="form-control"
                    name="estado"
                  >
                    <option value="1"> Activo</option>
                    <option value="0"> Inactivo</option>
                  </select>
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label htmlFor="label">Tipo de Trabajo</label>
                  <Dropdown
                    value={this.state.tipo_trabajo_id}
                    options={this.state.tipo_trabajo}
                    onChange={(e) => {
                      this.setState({ tipo_trabajo_id: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Tipo Trabajo"
                    filterBy="label,value"
                    placeholder="Selecciona Tipo Trabajo"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>

                <div className="form-group">
                  <label>Descripción</label>
                  <input
                    type="text"
                    name="descripcion"
                    className="form-control"
                    onChange={this.handleChange}
                    value={this.state.descripcion}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Estatus de Ejecución</label>
                  <Dropdown
                    value={this.state.estatus_ejecucion_id}
                    options={this.state.estatus_ejecucion}
                    onChange={(e) => {
                      this.setState({ estatus_ejecucion_id: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Estatus E"
                    filterBy="label,value"
                    placeholder="Selecciona Estatus E"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="label">Acta de Cierre</label>
                  <select
                    onChange={this.handleChange}
                    className="form-control"
                    name="actacierreid"
                  >
                    <option value="0">No</option>
                    <option value="1">Si</option>
                  </select>
                </div>
                <div className="form-group">
                  <label htmlFor="label">Estatus del Proceso</label>
                  <Dropdown
                    value={this.state.estado_oferta_id}
                    options={this.state.estado_oferta}
                    onChange={(e) => {
                      this.setState({ estado_oferta_id: e.value });
                    }}
                    filter={true}
                    filterPlaceholder="Selecciona Estatus P"
                    filterBy="label,value"
                    placeholder="Selecciona Estatus P"
                    style={{ width: "100%", heigh: "18px" }}
                  />
                </div>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label>Comentarios</label>
                  <textarea
                    type="text"
                    name="comentarios"
                    className="form-control"
                    onChange={this.handleChange}
                    value={this.state.comentarios}
                  />
                </div>
              </div>
            </div>

            <div className="row">
              <div className="col">
                <div className="form-group">
                  <label>Service Request</label>
                  <Checkbox
                    checked={this.state.service_request}
                    onChange={(e) =>
                      this.setState({ service_request: e.checked })
                    }
                  />
                </div>
              </div>
              <div className="col">
                <div className="form-group">
                  <label>Change Order</label>
                  <Checkbox
                    checked={this.state.service_order}
                    onChange={(e) =>
                      this.setState({ service_order: e.checked })
                    }
                  />
                </div>
              </div>
            </div>

            <button
              type="submit"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-folder-open"
              style={{ marginRight: "0.3em" }}
            >
              Guardar
            </button>
            <button
              type="button"
              className="btn btn-outline-primary"
              icon="fa fa-fw fa-ban"
              onClick={this.OcultarFormulario}
            >
              Cancelar
            </button>
          </form>
        </Dialog>
      </BlockUi>
    );
  }

  EnviarFormulario(event) {
    event.preventDefault();
    if (this.state.ContratoId > 0) {

      if(this.state.estado_oferta_id==0){
        
      }
      axios
        .post("/proyecto/OfertaComercial/Crear", {
          ContratoId: this.state.ContratoId,
          TransmitalId: 0,
          estado: this.state.estado_oferta_id,
          estado_oferta: this.state.estado_oferta_id,
          descripcion: this.state.descripcion,
          service_request: this.state.service_request,
          service_order: this.state.service_order,
          monto_so_referencial: 0,
          monto_ofertado: 0,
          monto_so_aprobado: 0,
          monto_ofertado_pendiente: 0,
          monto_certificado_aprobado: 0,
          dias_emision_oferta: 0,
          version: "B",
          codigo: "OF-BS0001",
          alcance: this.state.alcance,
          es_final: true,
          vigente: true,
          tipo_Trabajo_Id: this.state.tipo_trabajo_id,
          centro_de_Costos_Id: this.state.centro_costos_id,
          estatus_de_Ejecucion: this.state.estatus_ejecucion_id,
          codigo_shaya: this.state.codigo_shaya,
          revision_Oferta: this.state.revision,
          forma_contratacion: this.state.tipo_contratacion_id,
          acta_cierre: this.state.actacierreid,
          computo_completo: false,
          fecha_oferta: this.state.fecha_registro,
          comentarios: this.state.comentarios,
        })
        .then((response) => {
          if ((response.data = "o")) {
            this.infoMessage("Oferta Comercial Creada");
            this.setState({ visible: false });
            this.ConsultarSR();
          } else {
            this.alertMessage("Ocurrió un Error");
          }
        })
        .catch((error) => {
          console.log(error);
          this.alertMessage("Ocurrió un Error");
        });
    } else {
      this.alertMessage("Debe Seleccionar un Contrato");
    }
  }

  ConsultarSR() {
    axios
      .post("/Proyecto/OfertaComercial/Listar", {})
      .then((response) => {
        console.log("---------versiones-------------");
        console.log(response.data);
        this.setState({ data: response.data, blocking: false });
      })
      .catch((error) => {
        this.setState({ blocking: false });
        this.alertMessage(
          "Ocurrió un error al consultar las ofertas Comerciales"
        );
        console.log(error);
      });
  }
  

  ConsultarSRC(id) {
    this.setState({ blocking: true });
    axios
      .post("/Proyecto/OfertaComercial/ListarporContrato/", {
        Id: id,
      })
      .then((response) => {
        console.log("---------versiones-------------");
        console.log(response.data);
        this.setState({ data: response.data, blocking: false });
      })
      .catch((error) => {
        this.setState({ blocking: false });
        this.alertMessage(
          "Ocurrió un error al consular las ofertas Comerciales"
        );
        console.log(error);
      });
  }

  ContratoFormato(cell, row) {
    return cell.Codigo;
  }

  CambiarRender() {
    this.setState({ MostrarLista: !this.state.MostrarLista });
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  Redireccionar(id) {
    window.location.href = "/Proyecto/Oferta/Detalle/" + id;
  }

  Loading() {
    this.setState({ blocking: true });
  }

  StopLoading() {
    this.setState({ blocking: false });
  }

  OcultarFormulario() {
    this.setState({ visible: false });
  }

  MostrarFormulario() {
    if (this.state.ContratoId > 0) {
      this.setState({ visible: true });
    } else {
      abp.notify.error("Seleccione un Contrato", "AVISO");
    }
  }

  showInfo() {
    this.growl.show({
      severity: "info",
      summary: "Información",
      detail: this.state.message,
    });
  }

  infoMessage(msg) {
    this.setState({ message: msg }, this.showInfo);
  }

  showAlert() {
    this.growl.show({
      severity: "warn",
      summary: "Alerta",
      detail: this.state.message,
    });
  }

  alertMessage(msg) {
    this.setState({ message: msg }, this.showAlert);
  }
  getContratos() {
    axios
      .post("/Proyecto/Contrato/GetContratosApi", {})
      .then((response) => {
        console.log(response.data);
        var items = response.data.map((item) => {
          return { label: item.Codigo, dataKey: item.Id, value: item.Id };
        });
        this.setState({ contratos: items });
      })
      .catch((error) => {
        console.log(error);
      });
  }
}

ReactDOM.render(<OfertasComerciales />, document.getElementById("content"));
