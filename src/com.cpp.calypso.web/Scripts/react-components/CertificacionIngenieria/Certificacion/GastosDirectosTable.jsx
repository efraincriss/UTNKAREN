import React, { Fragment } from "react";
import { Button } from "primereact-v2/button";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import CurrencyFormat from "react-currency-format";

export class GastosDirectosTable extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
   
    };
  }

  render() {
    const options = {
      afterColumnFilter:this.props.afterColumnFilter,

    };
    return (
      <>
       
        <BootstrapTable
          data={this.props.data}
          hover={true}
          pagination={true}
          options={options}
        >
          <TableHeaderColumn
            isKey
            dataField="ColaboradoresIdentificacion"
            width={"15%"}
            tdStyle={{ whiteSpace: "normal", textAlign: "left" }}
            thStyle={{ whiteSpace: "normal", textAlign: "left" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            dataFormat={this.formatoFechaCorta}
          >
            Identificación
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="ColaboradoresNombresCompletos"
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Nombres
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="RubroCodigoString"
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Rubro
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="UnidadString"
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Unidad
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="TipoGastoString"
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{
              type: "TextFilter",
              delay: 500,
            }}
            dataSort={true}
          >
            Tipo Gasto
          </TableHeaderColumn>

          <TableHeaderColumn
            dataField="TotalHoras"
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            dataFormat={this.horasFormat}
          >
            Total Horas
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="TarifaHoras"
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            dataFormat={this.valoresFormat}
          >
            Tarifa
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="MontoTotal"
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            dataFormat={this.valoresFormat}
          >
            Monto Total
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="es500String"
            dataFormat={this.AjusteSpan}
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Es E500
          </TableHeaderColumn>
        </BootstrapTable>
      </>
    );
  }
  AjusteSpan(cell, row) {
    return (
      <>
        {cell === "SI" && (
          <span className="badge bg-info text-dark">{cell}</span>
        )}
        {cell === "NO" && (
          <span className="badge bg-light text-dark">{cell}</span>
        )}
      </>
    );
  }

  horasFormat = (cell, row) => {
    return (
      <CurrencyFormat
        value={cell}
        displayType={"text"}
        thousandSeparator={true}
        decimalScale={2}
        fixedDecimalScale={true}
        prefix={""}
      />
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
        prefix={"$"}
      />
    );
  };

  onEdit = (row) => {
    this.props.mostrarFormularioPocentajes(row);
  };
}
