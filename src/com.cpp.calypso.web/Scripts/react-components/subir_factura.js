import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import { Growl } from "primereact/components/growl/Growl";

import BlockUi from "react-block-ui";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { FileUpload } from "primereact/components/fileupload/FileUpload";

const selectRowProp = {
  mode: "checkbox",
};
export class SubirFactura extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visible: false,
      data: [],
      model: null,
      products: [],
      detallesseleccionados: [],
      AvanceProcuraId: 0,
      message: "",
      blocking: true,
      file: null,
    };
    this.successMessage = this.successMessage.bind(this);
    this.warnMessage = this.warnMessage.bind(this);
    this.Submit = this.Submit.bind(this);
    this.onHide = this.onHide.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.showSuccess = this.showSuccess.bind(this);
    this.showWarn = this.showWarn.bind(this);
    this.GetData = this.GetData.bind(this);
    this.onUpload = this.onUpload.bind(this);
    this.onSelect = this.onSelect.bind(this);
    this.onBasicUpload = this.onBasicUpload.bind(this);
    this.onBasicUploadAuto = this.onBasicUploadAuto.bind(this);
    this.renderShowsTotal = this.renderShowsTotal.bind(this);
    this.onSelectAll = this.onSelectAll.bind(this);
  }

  componentDidMount() {
    this.setState({ blocking: false });
  }

  onUpload(event) {
    this.growl.show({
      severity: "info",
      summary: "Success",
      detail: "File Uploaded",
    });
  }

  onBasicUpload(event) {
    this.growl.show({
      severity: "info",
      summary: "Success",
      detail: "File Uploaded with Basic Mode",
    });

    this.setState({ file: event.files[0] });
    this.GetData(event);
  }

  onBasicUploadAuto(event) {
    this.growl.show({
      severity: "info",
      summary: "Success",
      detail: "File Uploaded with Auto Mode",
    });
  }
  onSelect(event) {
    this.GetData();
    console.log("termino");
  }
  onSelectAll(isSelected) {
    if (isSelected) {
      return this.state.data.map((row) => row.id);
    } else {
      return [];
    }
  }
  renderShowsTotal(start, to, total) {
    return (
      <p style={{ color: "blue" }}>
        De {start} A {to}, Total {total}&nbsp;&nbsp;
      </p>
    );
  }

  render() {
    const selectRowProp = {
      mode: "checkbox",
    };

    return (
      <div>
        <div className="content-section implementation">
          <h3>Archivo</h3>
          <FileUpload
            mode="basic"
            chooseLabel="Seleccionar"
            cancelLabel="Cancelar"
            uploadLabel="Cargar"
            name="UploadedFile"
            accept="file_extension|media_type"
            maxFileSize={999000000}
            onUpload={this.onBasicUpload}
            className="btn btn-outline-info btn-sm"
          />
          <button
            onClick={this.Submit}
            className="btn btn-outline-indigo btn-sm"
            style={{ marginBottom: "1em" }}
          >
            Guardar
          </button>

          <hr />
          <div className="row">
            <div style={{ width: "100%" }}>
              <ul className="nav nav-tabs" id="empresa_tabs" role="tablist">
                <li className="nav-item">
                  <a
                    className="nav-link active"
                    id="novedades-tab"
                    data-toggle="tab"
                    href="#novedades"
                    role="tab"
                    aria-controls="home"
                    aria-expanded="true"
                  >
                    Facturas Válidas
                  </a>
                </li>
                <li className="nav-item">
                  <a
                    className="nav-link"
                    id="ofertas-tab"
                    data-toggle="tab"
                    href="#ofertas"
                    role="tab"
                    aria-controls="profile"
                  >
                    Facturas No Válidas
                  </a>
                </li>
              </ul>

              <div className="tab-content" id="myTabContent">
                <BlockUi tag="div" blocking={this.state.blocking}>
                  <div
                    className="tab-pane fade show active"
                    id="novedades"
                    role="tabpanel"
                    aria-labelledby="novedades-tab"
                  >
                    <div id="productList1">
                      <BootstrapTable
                        data={this.state.data}
                        selectRow={selectRowProp}
                        pagination={true}
                      >
                        <TableHeaderColumn
                          dataField="id"
                          isKey={true}
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Fila
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="sociedad"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Sociedad
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="fecha_documento"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Fecha Documento
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="factura"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Factura
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="detalle"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Detalle
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="clase_documento"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Clase Documento
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="documento_compensacion"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          D. Compensación
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="importe_moneda"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Importe Moneda
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="fecha_compensacion"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Fecha Compensacion
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="fecha_pago"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Fecha Pago
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="cliente"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Cliente
                        </TableHeaderColumn>
                      </BootstrapTable>
                    </div>
                  </div>

                  <div
                    className="tab-pane fade"
                    id="ofertas"
                    role="tabpanel"
                    aria-labelledby="ofertas-tab"
                  >
                    <div id="productList">
                      <BootstrapTable
                        data={this.state.products}
                        selectRow={selectRowProp}
                        pagination={true}
                      >
                        <TableHeaderColumn
                          dataField="id"
                          isKey={true}
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Fila
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="sociedad"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Sociedad
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="fecha_documento"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Fecha Documento
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="factura"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Factura
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="detalle"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Detalle
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="clase_documento"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Clase Documento
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="documento_compensacion"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          D. Compensación
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="importe_moneda"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Importe Moneda
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="fecha_compensacion"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Fecha Compensacion
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="fecha_pago"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Fecha Pago
                        </TableHeaderColumn>
                        <TableHeaderColumn
                          dataField="cliente"
                          filter={{ type: "TextFilter", delay: 1000 }}
                        >
                          Cliente
                        </TableHeaderColumn>
                      </BootstrapTable>
                    </div>
                  </div>
                </BlockUi>
              </div>
            </div>
          </div>

          <Growl
            ref={(el) => {
              this.growl = el;
            }}
          ></Growl>
        </div>
      </div>
    );
  }

  Submit() {
    this.setState({ blocking: true });
    console.log(this.state.model);
    axios
      .post("/Proyecto/Factura/CreateFacturas", {
        a: "s",

        data: this.state.model,
      })
      .then((response) => {
        this.setState({ blocking: false, data: [] });
        this.successMessage("Se insertaron los registros.");
      })
      .catch((error) => {
        console.log(error);
        this.setState({ blocking: false });
        this.warnMessage("Vuelve a intentar más tarde");
      });
  }

  handleSubmit(event) {
    event.preventDefault();
  }
  GetData(event) {
    if (event.files[0] != null) {
      const formData = new FormData();
      formData.append("UploadedFile", event.files[0]);
      this.setState({ blocking: true });
      const config = {
        headers: {
          "content-type": "multipart/form-data",
        },
      };
      axios
        .post("/proyecto/Factura/CargaFactura/", formData, config)
        .then((response) => {
          console.log(response);
          this.setState({
            data: response.data.Validas,
            products: response.data.NoValidas,
            model: response.data,
            blocking: false,
          });
        })
        .catch((error) => {
          console.log(error);
        });
    } else {
      console.log("error llamada");
    }
  }

  onHide(event) {
    this.setState({ visible: false, blockSubmit: false });
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
  successMessage(msg) {
    this.setState({ message: msg }, this.showSuccess);
  }

  warnMessage(msg) {
    this.setState({ message: msg }, this.showWarn);
  }
}

ReactDOM.render(<SubirFactura />, document.getElementById("content_factura"));
