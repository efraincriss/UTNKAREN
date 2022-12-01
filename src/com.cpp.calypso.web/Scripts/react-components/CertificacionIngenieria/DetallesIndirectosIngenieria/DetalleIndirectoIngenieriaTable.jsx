import React, { Fragment } from "react";
import { Button } from "primereact-v2/button";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";

export class DetalleIndirectoIngenieriaTable extends React.Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  render() {
    const options = {
      afterColumnFilter: this.props.afterColumnFilter,
    };

    return (
      <BootstrapTable data={this.props.data} hover={true} pagination={true}  options={options}>
        <TableHeaderColumn
          dataField="FechaDesde"
          tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
          thStyle={{ whiteSpace: "normal", textAlign: "right" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
          dataFormat={this.formatoFechaCorta}
        >
          Fecha Inicial
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="FechaHasta"
          tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
          thStyle={{ whiteSpace: "normal", textAlign: "right" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
          dataFormat={this.formatoFechaCorta}
        >
          Fecha Final
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="ColaboradorIdentificacion"
          width={"14%"}
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Identificación
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="ColaboradorNombres"
          width={"20%"}
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Colaborador
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="HorasLaboradas"
          tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
          thStyle={{ whiteSpace: "normal", textAlign: "right" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Horas
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="DiasLaborados"
          tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
          thStyle={{ whiteSpace: "normal", textAlign: "right" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Días
        </TableHeaderColumn>

        <TableHeaderColumn
          width={"13%"}
          dataField="CertificadoNombre"
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Certificado
        </TableHeaderColumn>

        <TableHeaderColumn
          isKey
          dataField="Id"
          dataFormat={this.generarBotones}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
        ></TableHeaderColumn>
      </BootstrapTable>
    );
  }

  generarBotones = (cell, row) => {
    return (
      <Fragment>
        <Button
          label=""
          className="p-button-outlined"
          onClick={() => this.onEdit(row)}
          icon="pi pi-pencil"
        />
        <Button
          style={{ marginLeft: "0.2em" }}
          label=""
          className="p-button-outlined"
          onClick={() => this.props.mostrarDetalleGastoIndirecto(row)}
          icon="pi pi-eye"
        />
        <Button
          style={{ marginLeft: "0.2em" }}
          label=""
          className="p-button-danger p-button-outlined"
          onClick={() => this.props.mostrarConfirmacionParaEliminar(row)}
          icon="pi pi-ban"
        />
      </Fragment>
    );
  };

  onEdit = (row) => {
    this.props.mostrarFormulario(row);
  };

  index(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  formatoFechaCorta = (cell, row) => {
    if (cell != null) {
      var informacion = cell.split("T");
      return informacion[0];
    } else {
      return null;
    }
  };
}
