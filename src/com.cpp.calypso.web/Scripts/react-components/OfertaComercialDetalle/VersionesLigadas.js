import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import CurrencyFormat from "react-currency-format";
export default class VersionesLigadas extends React.Component {
  constructor(props) {
    super(props);
  }
  ContratoFormato(cell, row) {
    return cell.Codigo;
  }
  EstadoFormato(cell, row) {
    if (cell === null || cell === 0) {
      return "";
    } else {
      return cell.nombre;
    }
  }
  FinalFormato(cell, row) {
    if (cell === 1) {
      return "SI";
    } else {
      return "NO";
    }
  }
  GenerarBotones(cell, row) {
    return (
      <div>
        <button
          className="btn btn-outline-success btn-sm"
          onClick={() => {
            this.VerVersion(row);
          }}
          style={{ float: "left", marginRight: "0.3em" }}
        >
          Ver
        </button>
      </div>
    );
  }
  VerVersion = (cell) => {
    window.location.href = "/Proyecto/OfertaComercial/DetailsOferta/" + cell.Id;
  };

  render() {
    return (
      <div className="row" style={{ marginTop: "1em" }}>
        <div className="col">
          <BootstrapTable
            data={this.props.versiones}
            hover={true}
            pagination={true}
          >
            <TableHeaderColumn
              isKey={true}
              dataField="Id"
              hidden
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Contrato
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Contrato"
              dataFormat={this.ContratoFormato}
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Contrato
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="codigo"
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Oferta
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="version"
              width={"10%"}
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Versión
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="descripcion"
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Descripción
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Catalogo"
              dataFormat={this.EstadoFormato}
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Estado
            </TableHeaderColumn>
            <TableHeaderColumn
              width={"10%"}
              dataField="es_final"
              dataFormat={this.FinalFormato}
              tdStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "12px" }}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Es Final
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Operaciones"
              dataFormat={this.GenerarBotones.bind(this)}
            ></TableHeaderColumn>
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
