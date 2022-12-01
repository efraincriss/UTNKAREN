import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import dateFormatter from "../../Base/DateFormatter";
export default class ReservasTable extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="row" style={{ marginTop: "1em" }}>
        <div className="col">
          <BootstrapTable data={this.props.data} hover={true} pagination={true}>
            <TableHeaderColumn
              dataField="Id"
              width={"6%"}
              hidden
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
              filter={{ type: "TextFilter", delay: 500 }}
              isKey
            >
              N°
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="colaborador_id_sap"
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Id Sap
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="colaborador_nombres"
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Colaborador
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="colaborador_grupo_personal"
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Colaborador Grupo
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="proveedor_razon_social"
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Proveedor
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="numero_habitacion"
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              N° Habitación
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="tipo_habitacion_nombre"
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Tipo
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="fecha_desde"
              dataFormat={this.dateFormat}
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Fecha Desde
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="fecha_hasta"
              dataFormat={this.dateFormat}
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Fecha Hasta
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="es_extemporaneo"
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Extemporáneo
            </TableHeaderColumn>


            <TableHeaderColumn
              dataField="iniciado_manual"
              hidden
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Inicio Manual
            </TableHeaderColumn>
            <TableHeaderColumn
              hidden
              dataField="finalizado_manual"
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
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Finalizado Manual
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Id"
              width={"18%"}
              dataFormat={this.generarBotones}
              thStyle={{
                whiteSpace: "normal",
                textAlign: "center",
                fontSize: "10px",
              }}
            >
              Opciones
            </TableHeaderColumn>
          </BootstrapTable>
        </div>
      </div>
    );
  }

  generarBotones = (cell, row) => {
    return (
      <div>
        <button
          className="btn btn-outline-success"
          data-toggle="tooltip"
          data-placement="left"
          title="Gestionar Detalles"
          style={{ marginRight: "0.3em" }}
          onClick={() => this.props.verDetalles(row)}
        >
          <i className="fa fa-eye"></i>
        </button>
        {/*<button
          className="btn btn-outline-success"
          data-toggle="tooltip"
          data-placement="left"
          title="Ver Detalles"
          style={{ marginRight: "0.3em" }}
          onClick={() => this.props.consultarDetalles(cell)}
        >
          <i className="fa fa-eye"></i>
    </button>*/}
        <button
          className="btn btn-outline-info"
          data-toggle="tooltip"
          data-placement="left"
          title="Editar Reserva"
          style={{ marginRight: "0.3em" }}
          onClick={() => this.props.seleccionarEditarReserva(row)}
        >
          <i className="fa fa-pencil"></i>
        </button>
        <button
          className="btn btn-outline-danger"
          data-toggle="tooltip"
          data-placement="left"
          title="Eliminar Reserva"
          onClick={() => {
            if (window.confirm(`Esta seguro de eliminar la reserva?`))
              this.props.eliminarReserva(cell);
          }}
        >
          <i className="fa fa-trash"></i>
        </button>
        {row.DocumentoId != null && (
          <button
            className="btn btn-outline-indigo"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.props.onDownload(row.DocumentoId)}
            data-toggle="tooltip"
            data-placement="left"
            title="Archivo Respaldo"
          >
            <i className="fa fa-cloud-download"></i>
          </button>
        )}
        {/*row.inicio_consumo && !row.consumo_finalizado &&
          <button
            className="btn btn-outline-info"
            data-toggle="tooltip"
            data-placement="left"
            title="Editar Fecha Inicio"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.props.onEditarFecha(row, true)}
          >
            <i className="fa fa-history"></i>
          </button>
        }
        {!row.inicio_consumo ? (
          <button
            className="btn btn-outline-info"
            data-toggle="tooltip"
            data-placement="left"
            title="Iniciar Consumo"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.props.onLoad(row, true)}
          >
            <i className="fa fa-clock-o"></i>
          </button>
        ) : (
          <button
            className="btn btn-outline-danger"
            data-toggle="tooltip"
            data-placement="left"
            title="Finalizar Consumo"
            style={{ marginLeft: "0.3em" }}
            onClick={() => this.props.onLoad(row, false)}
          >
            <i className="fa fa-hand-scissors-o"></i>
          </button>
     
        )*/}
      </div>
    );
  };

  dateFormat = (cell, row) => {
    return dateFormatter(cell);
  };
}
