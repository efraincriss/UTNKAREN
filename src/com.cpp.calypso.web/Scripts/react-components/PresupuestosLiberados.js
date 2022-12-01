import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import moment from "moment";
import CurrencyFormat from "react-currency-format";
import { Dropdown } from "primereact/components/dropdown/Dropdown";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";

class PresupuestosLiberados extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: true,
      data: [],
      message: "",
    };

    this.handleChange = this.handleChange.bind(this);
    this.OcultarFormulario = this.OcultarFormulario.bind(this);
    this.MostrarFormulario = this.MostrarFormulario.bind(this);
    this.alertMessage = this.alertMessage.bind(this);
    this.infoMessage = this.infoMessage.bind(this);

    this.Loading = this.Loading.bind(this);
    this.StopLoading = this.StopLoading.bind(this);

    this.ConsultarSR = this.ConsultarSR.bind(this);
  }

  componentDidMount() {
    this.ConsultarSR();
  }
  GenerarBotones(cell, row) {
    return (
      <div>
        <button
          onClick={() => this.Redireccionar(row)}
          className="btn btn-sm btn-outline-indigo"
        >
          Ver
        </button>
      </div>
    );
  }
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
    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl
          ref={(el) => {
            this.growl = el;
          }}
          position="bottomright"
          baseZIndex={1000}
        ></Growl>
        <BootstrapTable
          data={this.state.data}
          hover={true}
          pagination={true}
          options={options}
        >
          <TableHeaderColumn
            dataField="codigo_proyecto"
            isKey
            tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            filter={{ type: "TextFilter", delay: 500 }}
            width={"10%"}
            dataSort={true}
          >
            Proyecto
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="codigo_requerimiento"
            tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            filter={{ type: "TextFilter", delay: 500 }}
            width={"10%"}
            dataSort={true}
          >
            Cod_Trab
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="version"
            tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            filter={{ type: "TextFilter", delay: 500 }}
            width={"6%"}
            dataSort={true}
          >
            Version
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="NombreEstadoAprobacion"
            hidden
            tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            filter={{ type: "TextFilter", delay: 500 }}
            width={"10%"}
            dataSort={true}
          >
            Estado
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="monto_construccion"
            dataFormat={this.MontosFormato}
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            tdStyle={{
              whiteSpace: "normal",
              fontSize: "10px",
              textAlign: "right",
            }}
            width={"10%"}
            thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
            dataSort={true}
          >
            M. Construcción
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="monto_ingenieria"
            dataFormat={this.MontosFormato}
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            tdStyle={{
              whiteSpace: "normal",
              fontSize: "10px",
              textAlign: "right",
            }}
            width={"10%"}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            dataSort={true}
          >
            M. Ingeniería
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="monto_procura"
            dataFormat={this.MontosFormato}
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            tdStyle={{
              whiteSpace: "normal",
              fontSize: "10px",
              textAlign: "right",
            }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            width={"10%"}
            dataSort={true}
          >
            M. Suministros
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="monto_subcontratos"
            dataFormat={this.MontosFormato}
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            tdStyle={{
              whiteSpace: "normal",
              fontSize: "10px",
              textAlign: "right",
            }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            width={"10%"}
            dataSort={true}
          >
            M. SubContra
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="monto_total"
            dataFormat={this.MontosFormato}
            tdStyle={{ whiteSpace: "normal" }}
            thStyle={{ whiteSpace: "normal" }}
            filter={{ type: "TextFilter", delay: 500 }}
            tdStyle={{
              whiteSpace: "normal",
              fontSize: "10px",
              textAlign: "right",
            }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            width={"10%"}
            dataSort={true}
          >
            Total
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="fecha_registro"
            filter={{ type: "TextFilter", delay: 500 }}
            dataFormat={this.dateFormat}
            dataSort={true}
            width={"10%"}
            tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          >
            Fecha Pres
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="formatFechaActualizacionPresupuesto"
            filter={{ type: "TextFilter", delay: 500 }}
           // dataFormat={this.dateFormat}
            dataSort={true}
            width={"10%"}
            tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          >
            Fecha Actualización Presu
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="codigo_oferta"
            tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            filter={{ type: "TextFilter", delay: 500 }}
            width={"10%"}
            dataSort={true}
          >
            Oferta
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="formatFechaUltimoEnvioOferta"
            filter={{ type: "TextFilter", delay: 500 }}
           // dataFormat={this.dateFormat}
            dataSort={true}
            width={"10%"}
            tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          >
            Fecha últ envío
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="estado_oferta"
            tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
            filter={{ type: "TextFilter", delay: 500 }}
            width={"10%"}
            dataSort={true}
          >
            Estado Oferta
          </TableHeaderColumn>
        </BootstrapTable>
      </BlockUi>
    );
  }

  ConsultarSR() {
    axios
      .post("/Proyecto/OfertaComercial/ListarPresupuestosLiberados", {})
      .then((response) => {
        console.log(response.data);
        this.setState({ data: response.data, blocking: false });
      })
      .catch((error) => {
        this.setState({ blocking: false });
        this.alertMessage("Ocurrió un error al consultar los Requerimientos");
        console.log(error);
      });
  }
  dateFormat(cell, row) {
    if (cell == null) {
      return "dd/mm/yy";
    } else {
      var x = cell;
      return moment(x).format("DD/MM/YYYY");
    }
  }

  MontosFormato(cell, row) {
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
  }

  handleChange(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  Redireccionar(id) {
    window.location.href = "/Proyecto/OfertaPresupuesto/Details/" + id;
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
    this.setState({ visible: true });
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
}

ReactDOM.render(<PresupuestosLiberados />, document.getElementById("content"));
