import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";

export class DetallesIngenieriaTable extends React.Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  Secuencial(cell, row, enumObject, index) {
    return <div>{index + 1}</div>;
  }

  render() {
    const options = {
      afterColumnFilter: this.props.afterColumnFilter,
    };

    return (
      <BootstrapTable data={this.props.data} hover={true} pagination={true} options={options}>
        <TableHeaderColumn
          isKey={true}
          width={"6%"}
          dataField="Id"
          hidden
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          ID
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="any"
          export={false}
          dataFormat={this.Secuencial}
          width={"8%"}
          tdStyle={{
            whiteSpace: "normal",
            textAlign: "center",
            fontSize: "10px",
          }}
          thStyle={{
            whiteSpace: "normal",
            textAlign: "center",
            fontSize: "10px",
          }}
        >
          Nº
        </TableHeaderColumn>
        
        <TableHeaderColumn
          dataField="Secuencial"
          width={"8%"}
          tdStyle={{
            whiteSpace: "normal",
            textAlign: "center",
            fontSize: "10px",
          }}
          thStyle={{
            whiteSpace: "normal",
            textAlign: "center",
            fontSize: "10px",
          }}
        >
          Seq
        </TableHeaderColumn>

        <TableHeaderColumn
          width={"8%"}
          dataField="formatFechaTrabajo"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Fecha Trabajo
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="CodigoProyecto"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Proyecto
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="NombreEjecutante"
          width={"20%"}
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Colaborador
        </TableHeaderColumn>
        <TableHeaderColumn
            width={"9%"}
          dataField="Identificacion"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Cédula
        </TableHeaderColumn>
        <TableHeaderColumn
          width={"8%"}
          dataField="NumeroHoras"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Horas
        </TableHeaderColumn>
       {/** <TableHeaderColumn
          dataField="nombreLocacion"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Locacion
        </TableHeaderColumn> */}
        <TableHeaderColumn
          dataField="esCargaAutomatica"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Carga
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="formatFechaCarga"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Fecha Carga
        </TableHeaderColumn>
        <TableHeaderColumn
          dataField="nombreEstado"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Estado
        </TableHeaderColumn>
        {/*<TableHeaderColumn
          dataField="Observaciones"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Observaciones
        </TableHeaderColumn>*/}
        <TableHeaderColumn
         width={"10%"}
          dataField="Id"
          dataFormat={this.generarBotones}
          thStyle={{ whiteSpace: "normal", textAlign: "center" }}
        ></TableHeaderColumn>
      </BootstrapTable>
    );
  }

  generarBotones = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-info mr-1 btn-sm"
          onClick={() => this.onEdit(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Editar"
        >
          <i className="fa fa-pencil"></i>
        </button>
        <button
          className="btn btn-outline-danger mr-1 btn-sm"
          onClick={() => this.props.mostrarConfirmacionParaEliminar(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar"
        >
          <i className="fa fa-trash"></i>
        </button>
        <button
          className="btn btn-outline-primary mr-1 btn-sm"
          onClick={() => {
            if (
              window.confirm(
                `Esta acción cambiara de estado el registro  a validado, ¿Desea continuar?`
              )
            )
              this.props.EnviarValidado(row.Id);
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Cambiar Estado"
        >
          <i className="fa fa-check"></i>
        </button>
      </div>
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
