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

  PorcentajeFormat(cell, row) {
    return `${(cell * 100 ).toFixed(2)} %`;
  }

  render() {
    return (
      <BootstrapTable data={this.props.data} hover={true} pagination={true}>
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
          width={"8%"}
          dataField="formatFechaCertificado"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Fecha
        </TableHeaderColumn>
        <TableHeaderColumn
          width={"12%"}
          dataField="nombreContrato"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Contrato
        </TableHeaderColumn>

        <TableHeaderColumn
          dataField="nombreProyecto"
          width={"15%"}
          tdStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px" }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataSort={true}
        >
          Proyecto
        </TableHeaderColumn>
        <TableHeaderColumn
          width={"10%"}
          dataField="AvancePrevistoActualIB"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px", textAlign: "center"  }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px", textAlign: "center"  }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataFormat={this.PorcentajeFormat}
          dataSort={true}
        >
          Avance Prev IB
        </TableHeaderColumn>

        <TableHeaderColumn
          width={"10%"}
          dataField="AvancePrevistoActualID"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px", textAlign: "center"  }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px", textAlign: "center"  }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataFormat={this.PorcentajeFormat}
          dataSort={true}
        >
          Avance Prev ID
        </TableHeaderColumn>

        <TableHeaderColumn
          width={"10%"}
          dataField="AvanceRealActualIB"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px", textAlign: "center"  }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px", textAlign: "center"  }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataFormat={this.PorcentajeFormat}
          dataSort={true}
        >
          Avance Real IB
        </TableHeaderColumn>
        <TableHeaderColumn
          width={"10%"}
          dataField="AvanceRealActualID"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px", textAlign: "center"  }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px", textAlign: "center"  }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataFormat={this.PorcentajeFormat}
          dataSort={true}
        >
          Avance Real ID
        </TableHeaderColumn>
        <TableHeaderColumn
          width={"10%"}
          dataField="AsbuiltActual"
          tdStyle={{ whiteSpace: "normal", fontSize: "10px", textAlign: "center"  }}
          thStyle={{ whiteSpace: "normal", fontSize: "10px", textAlign: "center"  }}
          filter={{ type: "TextFilter", delay: 500 }}
          dataFormat={this.PorcentajeFormat}
          dataSort={true}
        >
          AsBuilt
        </TableHeaderColumn>
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
