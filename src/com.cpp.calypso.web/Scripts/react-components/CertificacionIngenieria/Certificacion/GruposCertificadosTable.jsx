import React, { Fragment } from "react";
import { Button } from "primereact-v2/button";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";

export class GruposCertificadosTable extends React.Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  render() {
    return (
      <BootstrapTable data={this.props.data} hover={true} pagination={true}>
        <TableHeaderColumn
          dataField="FechaInicioDate"
          tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
          thStyle={{ whiteSpace: "normal", textAlign: "right" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Fecha Inicial
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="FechaFinDate"
          tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
          thStyle={{ whiteSpace: "normal", textAlign: "right" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Fecha Final
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="NombreCliente"
          width={"20%"}
          tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Cliente
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="FechaCertificadoDate"
          tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
          thStyle={{ whiteSpace: "normal", textAlign: "right" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Fecha Certificado
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="EstadoString"
          tdStyle={{ whiteSpace: "normal", textAlign: "right" }}
          thStyle={{ whiteSpace: "normal", textAlign: "right" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Estado
        </TableHeaderColumn>
        <TableHeaderColumn
          isKey
          dataField="Id"
          width={"20%"}
          dataFormat={this.generarBotones}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
        ></TableHeaderColumn>
      </BootstrapTable>
    );
  }

  generarBotones = (cell, row) => {
    return (
      <Fragment>
        {/**  <Button
          style={{ marginLeft: "0.2em" }}
          label=""
          className="p-button-outlined"
      
          icon="pi pi-eye"
        />
        */}
        <button
          className="btn btn-outline-success btn-sm"
          style={{ marginLeft: "0.2em" }}
          onClick={() => this.props.mostrarDetalleGrupo(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Ver"
        >
          <i className="fa fa-eye"></i>
        </button>

        <button
          className="btn btn-outline-indigo btn-sm"
          style={{ marginLeft: "0.2em" }}
          onClick={() => this.props.descargarGrupodeCertificados(row.Id)}
          data-toggle="tooltip"
          data-placement="top"
          title="Certificado"
        >
          <i className="fa fa-download"></i>
        </button>

        <button
          className="btn btn-outline-danger btn-sm"
          style={{ marginLeft: "0.2em" }}
          onClick={() => this.props.mostrarConfirmacionParaEliminar(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
        >
          <i className="fa fa-trash" />
        </button>


        <button
          className="btn btn-outline-primary btn-sm"
          style={{ marginLeft: "0.2em" }}
          onClick={() => this.props.mostrarDistribucionParametros(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Mostrar Distribución Proyectos"
        >
          <i className="fa fa-database"></i>
        </button>
        <button
          className="btn btn-outline-success btn-sm"
          style={{ marginLeft: "0.2em" }}
          onClick={() => this.props.descargarGrupodeCertificados2(row.Id)}
          data-toggle="tooltip"
          data-placement="top"
          title="Simplificado"
        >
          <i className="fa fa-download"></i>
        </button>
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
