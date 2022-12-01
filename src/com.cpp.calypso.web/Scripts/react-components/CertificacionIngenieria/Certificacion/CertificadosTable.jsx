import React, { Fragment } from "react";
import { Button } from "primereact-v2/button";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import CurrencyFormat from "react-currency-format"

export class CertificadosTable extends React.Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  componentDidMount() {
    console.log("props", this.props);
  }
  render() {
    const options = {
      afterColumnFilter: this.props.afterColumnFilter,
      
    };
    return (
      <BootstrapTable data={this.props.data} hover={true} pagination={true}
        options={options}
      >
        <TableHeaderColumn
          dataField="NombreProyecto"
          width={"15%"}
          tdStyle={{
            whiteSpace: "normal",
            textAlign: "left",
            fontSize: "11px",
          }}
          thStyle={{
            whiteSpace: "normal",
            textAlign: "left",
            fontSize: "11px",
          }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
          dataFormat={this.formatoFechaCorta}
        >
          Proyecto
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="NumeroCertificado"
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
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Num. Cert
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="HorasActualCertificadas"
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
          filter={{ type: "TextFilter", delay: 500, }}

          dataSort={true}
        >
          Horas Act Certificadas
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="HorasAnteriorCertificadas"
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
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
          dataFormat={this.valoresFormat}
        >
          Horas Ant Certificadas
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="MontoActualCertificado"
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
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
          dataFormat={this.valoresFormat}
        >
          Monto Act Certificadas
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="MontoAnteriorCertificado"
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
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
          dataFormat={this.valoresFormat}
        >
          Monto Ant Certificadas
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="TotalHorasDirectos"
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
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
          dataFormat={this.valoresFormat}
        >
          Monto Directos
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="TotalHorasIndirectos"
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
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
          dataFormat={this.valoresFormat}
        >
          Monto Indirectos
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="EstadoString"
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
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Estado
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="Id"
          isKey
          width={"12%"}
          dataFormat={this.generarBotones}
          thStyle={{
            whiteSpace: "normal",
            textAlign: "center",
            fontSize: "11px",
          }}
        ></TableHeaderColumn>
      </BootstrapTable>
    );
  }

  generarBotones = (cell, row) => {
    return (
      <Fragment>
        <Button
          style={{ marginLeft: "0.2em" }}
          label=""
          className="p-button-outlined"
          onClick={() => {
            if (
              window.confirm(
                `Esta segur@ de aprobar el certificado, ¿Desea continuar?`
              )
            )
              this.props.aprobarCertificado(row.Id);
          }}
          icon="pi pi-check"
        />
        <button
          className="btn btn-outline-success btn-sm"
          style={{ marginLeft: "0.2em" }}
          onClick={() => this.props.mostrarFormularioGastos(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Ver"
        >
          <i className="fa fa-eye"></i>
        </button>

      </Fragment>
    );
  };

  valoresFormat = (cell, row) => {
    return (
      <CurrencyFormat
        value={cell}
        displayType={"text"}
        thousandSeparator={true}
        decimalScale={2}
        fixedDecimalScale={true}
      />
    )
  }

  onEdit = (row) => {
    this.props.mostrarFormularioPocentajes(row);
  };
}
