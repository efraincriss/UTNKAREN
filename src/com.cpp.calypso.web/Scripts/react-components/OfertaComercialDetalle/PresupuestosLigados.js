import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import CurrencyFormat from "react-currency-format";
export default class PresupuestosLigados extends React.Component {
  constructor(props) {
    super(props);
  }

  CodigoFormato(cell, row) {
    if (cell == null) {
      return "";
    } else {
      return cell.codigo;
    }
  }
  FechaPresupuestoFormat(cell, row) {
    if (cell == null) {
      return "dd/mm/yy";
    } else {
      var x = cell.fecha_registro;
      return moment(x).format("DD/MM/YYYY");
    }
  }
  DescripcionPresupuesto(cell, row) {
    if (cell == null) {
      return "";
    } else {
      return cell.descripcion;
    }
  }
  MontosContruccion(cell, row) {
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
          value={cell.monto_construccion}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  }
  MontosIngenieria(cell, row) {
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
          value={cell.monto_ingenieria}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  }
  MontosSuministros(cell, row) {
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
          value={cell.monto_suministros}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  }
  MontosSubContratos = (cell, row) => {
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
          value={cell.monto_subcontratos}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  };
  MontosTotal(cell, row) {
    if (row == null) {
      return (
        <CurrencyFormat
          value={0}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    } else {
      var total = row.Presupuesto != null ? row.Presupuesto.monto_total : 0;
      return (
        <CurrencyFormat
          value={total}
          displayType={"text"}
          thousandSeparator={true}
          prefix={"$"}
        />
      );
    }
  }
  VersionFormato(cell, row) {
    if (cell == null) {
      return "";
    } else {
      return cell.version;
    }
  }
  CodigoFormatoProyecto(cell, row) {
    if (cell == null) {
      return "";
    } else {
      if(cell.Proyecto!=null){
        return cell.Proyecto.codigo;
      }else{
        return "";
      }
      
    }
  }
  GenerarBotonesPresupuestos(cell, row) {
    return (
      <div>
        <button
          className="btn btn-outline-info btn-sm"
          onClick={() => {
            if (window.confirm("Estás segur@?")) this.props.Eliminar(row.Id);
          }}
          style={{ float: "left", marginRight: "0.3em" }}
          data-toggle="tooltip"
          data-placement="top"
          title="Eliminar Presupuesto"
        >
            <i className="fa fa-trash"></i>
        </button>
        <button
          className="btn btn-outline-info btn-sm"
          style={{ marginRight: "0.2em" }}
          onClick={() => this.props.mostrarInfoPresupue(row)}
          data-toggle="tooltip"
          data-placement="top"
          title="Ver confirmación mail"
        >
          <i className="fa fa-search"></i>
        </button>
      </div>
    );
  }
  render() {
    return (
      <div className="row" style={{ marginTop: "1em" }}>
        <div className="col">
          <BootstrapTable
            data={this.props.presupuestos}
            hover={true}
            pagination={true}
          >
            <TableHeaderColumn
              dataField="Id"
              hidden
              width={"8%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
              filter={{ type: "TextFilter", delay: 500 }}
              isKey
            >
              No.
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Requerimiento"
              dataFormat={this.CodigoFormatoProyecto}
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              filter={{ type: "TextFilter", delay: 500 }}
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
              width={"10%"}
            >
              Proyecto
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Requerimiento"
              dataFormat={this.CodigoFormato}
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              filter={{ type: "TextFilter", delay: 500 }}
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
              width={"10%"}
            >
              Requerimiento
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Presupuesto"
              filter={{ type: "TextFilter", delay: 500 }}
              dataFormat={this.FechaPresupuestoFormat}
              dataSort={true}
              width={"10%"}
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
            >
              Fecha Presupuesto
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Presupuesto"
              dataFormat={this.DescripcionPresupuesto}
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              filter={{ type: "TextFilter", delay: 500 }}
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
            >
              Descripción
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Presupuesto"
              dataFormat={this.MontosContruccion}
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              filter={{ type: "TextFilter", delay: 500 }}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                textAlign: "right",
              }}
              width={"10%"}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
            >
              M. Construcción
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Presupuesto"
              dataFormat={this.MontosSuministros}
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              filter={{ type: "TextFilter", delay: 500 }}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                textAlign: "right",
              }}
              width={"10%"}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
            >
              M. Suministros
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Presupuesto"
              dataFormat={this.MontosIngenieria}
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              filter={{ type: "TextFilter", delay: 500 }}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                textAlign: "right",
              }}
              width={"10%"}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
            >
              M. Ingeniería
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Presupuesto"
              dataFormat={this.MontosSubContratos}
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              filter={{ type: "TextFilter", delay: 500 }}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                textAlign: "right",
              }}
              width={"10%"}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
            >
              M. Sub Contratos
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Presupuesto"
              dataFormat={this.MontosTotal}
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              filter={{ type: "TextFilter", delay: 500 }}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                textAlign: "right",
              }}
              width={"10%"}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
            >
              M. Total
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Presupuesto"
              dataFormat={this.VersionFormato}
              tdStyle={{ whiteSpace: "normal" }}
              thStyle={{ whiteSpace: "normal" }}
              width={"6%"}
              filter={{ type: "TextFilter", delay: 500 }}
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
            >
              Versión
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Operaciones"
              dataFormat={this.GenerarBotonesPresupuestos.bind(this)}
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
