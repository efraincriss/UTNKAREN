import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import CurrencyFormat from "react-currency-format";
export default class OSLigadas extends React.Component {
  constructor(props) {
    super(props);
  }
  dateFormatOS(cell, row) {
    if (cell == null) {
      return "dd/mm/yy";
    }

    return moment(cell).format("DD/MM/YYYY");
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

  render() {
    return (
      <div className="row" style={{ marginTop: "1em" }}>
        <div className="col">
        <BootstrapTable
                  data={this.props.data}
                  hover={true}
                  pagination={true}
                >
                  <TableHeaderColumn
                    isKey={true}
                    width={"10%"}
                    dataField="codigo_orden_servicio"
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                    width={"8%"}
                  >
                    Codigo
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="version_os"
                    hidden
                    width={"8%"}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    Versión
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="fecha_orden_servicio"
                    filter={{ type: "TextFilter", delay: 500 }}
                    dataFormat={this.dateFormatOS}
                    dataSort={true}
                    width={"8%"}
                    tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                  >
                    Fecha{" "}
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="monto_aprobado_construccion"
                    dataFormat={this.MontosFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    width={"8%"}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    M. Construcción
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="monto_aprobado_ingeniería"
                    dataFormat={this.MontosFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    width={"10%"}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    M. Ingeniería
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="monto_aprobado_suministros"
                    dataFormat={this.MontosFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    width={"10%"}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    M. Suministros
                  </TableHeaderColumn>
                  <TableHeaderColumn
                    dataField="monto_aprobado_subcontrato"
                    dataFormat={this.MontosFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    width={"10%"}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    M. Subcontratos
                  </TableHeaderColumn>

                  <TableHeaderColumn
                    dataField="monto_aprobado_os"
                    dataFormat={this.MontosFormato}
                    tdStyle={{ whiteSpace: "normal" }}
                    thStyle={{ whiteSpace: "normal" }}
                    width={"10%"}
                    filter={{ type: "TextFilter", delay: 500 }}
                    tdStyle={{
                      whiteSpace: "normal",
                      fontSize: "11px",
                      textAlign: "right",
                    }}
                    thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
                    dataSort={true}
                  >
                    M. Aprobado OS
                  </TableHeaderColumn>
               
                </BootstrapTable>

        </div>
      </div>
    );
  }

  generarBotones = (cell, row) => {
    return (
      <button
        className="btn btn-outline-info"
        data-toggle="tooltip"
        data-placement="left"
        title="VER"
        onClick={() => this.onRedirect(cell)}
      >
        <i className="fa fa-eye"></i>
      </button>
    );
  };

  onRedirect = (colaboradorId) => {
    switch (this.props.source) {
      case "requisitos":
        window.location.href = `/Accesos/ValidacionRequisito/Index?colaboradorId=${colaboradorId}`;
        console.log("aaa");
        break;
      case "tarjetas":
        window.location.href = `${config.appUrl}Accesos/${CONTROLLER_TARJETA_ACCESO}/Index?colaboradorId=${colaboradorId}`;
        break;

      default:
        window.location.href = `${config.appUrl}Accesos/${CONTROLLER_TARJETA_ACCESO}/Index?colaboradorId=${colaboradorId}`;
    }
  };
}
