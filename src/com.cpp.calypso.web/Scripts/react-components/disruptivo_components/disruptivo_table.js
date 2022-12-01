import React from "react";
import axios from "axios";
import moment from "moment";

import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact/components/dialog/Dialog";

import DisruptivoForm from "./disruptivoForm";
import EditForm from "./EditForm";

export default class DisruptivoTable extends React.Component {
  constructor(props) {
    super(props);
    this.deleteDisruptivo = this.deleteDisruptivo.bind(this);
    this.state = {
      key_form: 23423,
      Id: 0,
      tipo_improductividad: 0,
      tipo_recurso: 0,
      hora_inicio: "",
      hora_fin: "",
      horas: 0,
      observaciones: "",
      numero_recursos: "",
      numero_horas_hombres: 0,
      fecha_inicio: "",
      fecha_fin: "",
      porcentaje_disruptivo:100,
      visible: false,
    };
    this.LoadDisruptivo = this.LoadDisruptivo.bind(this);
    this.onHide = this.onHide.bind(this);
    this.showForm = this.showForm.bind(this);
  }

  generateButton(cell, row) {
    return (
      <div>
        <button
          onClick={() => this.LoadDisruptivo(row.Id)}
          className="btn btn-outline-primary btn-sm"
        >
          Editar
        </button>
        <button
          onClick={() => this.deleteDisruptivo(row.Id)}
          className="btn btn-outline-danger btn-sm"
          style={{ marginLeft: "0.2em" }}
        >
          Eliminar
        </button>
      </div>
    );
  }

  render() {
    return (
      <div>
        <BootstrapTable data={this.props.data} hover={true} pagination={true}>
          <TableHeaderColumn
            dataField="nombre_improductividad"
            filter={{ type: "TextFilter", delay: 500 }}
            isKey={true}
            dataAlign="center"
            dataSort={true}
          >
            Improductividad
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="nombre_recurso"
            filter={{ type: "TextFilter", delay: 500 }}
            dataAlign="center"
            dataSort={true}
          >
            Recurso
          </TableHeaderColumn>
          <TableHeaderColumn
            hidden
            dataField="hora_inicio"
            width={"10%"}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Hora Inicio
          </TableHeaderColumn>
          <TableHeaderColumn
            hidden
            dataField="hora_fin"
            width={"10%"}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Hora Fin
          </TableHeaderColumn>
          <TableHeaderColumn
            hidden
            dataField="numero_recursos"
            width={"13%"}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Número Recursos
          </TableHeaderColumn>
          <TableHeaderColumn
            hidden
            dataField="numero_horas_hombres"
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Total Horas / Hombre
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="fecha_inicio"
            width={"11%"}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Fecha Inicio
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="fecha_fin"
            width={"11%"}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Fecha Fin
          </TableHeaderColumn>
          <TableHeaderColumn
            
            dataField="numero_dias"
            width={"10%"}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Dias
          </TableHeaderColumn>
          <TableHeaderColumn
            
            dataField="porcentaje_disruptivo"
            width={"10%"}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Porcentaje
          </TableHeaderColumn>
          <TableHeaderColumn
            
            dataField="dias_real"
           
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
          >
            Dias Real
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="Operaciones"
            dataFormat={this.generateButton.bind(this)}
          >
            Operaciones
          </TableHeaderColumn>
        </BootstrapTable>

        <Dialog
          header="Gestión de disruptivos"
          visible={this.state.visible}
          width="850px"
          modal={true}
          onHide={this.onHide}
        >
          <EditForm
            catalogos={this.props.catalogos}
            recursos={this.props.recursos}
            key={this.state.key_form}
            showSuccess={this.props.showSuccess}
            showWarn={this.props.showWarn}
            updateTable={this.props.updateTable}
            onHide={this.onHide}
            Id={this.state.Id}
            tipo_improductividad={this.state.tipo_improductividad}
            hora_inicio={moment(this.state.hora_inicio, "HH:mm:ss")}
            hora_fin={moment(this.state.hora_fin, "HH:mm:ss")}
            horas={this.state.horas}
            observaciones={this.state.observaciones}
            numero_recursos={this.state.numero_recursos}
            numero_horas_hombres={this.state.numero_horas_hombres}
            tipo_recurso={this.state.tipo_recurso}
            fecha_inicio={moment(this.state.fecha_inicio).format("YYYY-MM-DD")}
            fecha_fin={moment(this.state.fecha_fin).format("YYYY-MM-DD")}
            porcentaje_disruptivo={this.state.porcentaje_disruptivo}
          />
        </Dialog>
      </div>
    );
  }

  deleteDisruptivo(id) {
    axios
      .post("/proyecto/ObraDisruptivo/DeleteApi/" + id, {})
      .then((response) => {
        this.props.updateTable();
        this.props.showSuccess("Disruptivo Eliminado.");
      })
      .catch((error) => {
        console.log(error);
        this.props.showWarn("No se ha podido Eliminar.");
      });
  }

  LoadDisruptivo(id) {
    axios
      .post("/proyecto/ObraDisruptivo/DetailsApi/" + id, {})
      .then((response) => {
        console.log(response);
        this.setState(
          {
            Id: id,
            tipo_improductividad: response.data.tipo_improductividad,
            tipo_recurso: response.data.TipoRecursoId,
            hora_inicio: response.data.hora_inicio,
            hora_fin: response.data.hora_fin,
            horas: response.data.numero_horas,
            observaciones: response.data.observaciones,
            numero_recursos: response.data.numero_recursos,
            numero_horas_hombres: response.data.numero_horas_hombres,
            fecha_inicio: response.data.fecha_inicio,
            fecha_fin: response.data.fecha_fin,
            vigente: true,
            key_form: Math.random(),
          },
          this.showForm
        );
      })
      .catch((error) => {
        this.props.showWarn("Algo salio mal.");
      });
  }

  showForm() {
    this.setState({ visible: true });
  }
  onHide() {
    this.setState({ visible: false });
  }
}
