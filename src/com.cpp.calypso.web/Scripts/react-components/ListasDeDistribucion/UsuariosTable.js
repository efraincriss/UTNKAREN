import React from "react";
import axios from "axios";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
export default class UsuariosTable extends React.Component {
  constructor(props) {
    super(props);
    this.handleRedirect = this.handleRedirect.bind(this);
    this.actionBodyTemplate = this.actionBodyTemplate.bind(this);
  }

  generateButton(cell, row) {
    return (
      <div>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          onClick={() => {
            if (
              window.confirm(
                `Esta acción elimanará el registro, ¿Desea continuar?`
              )
            )
              this.props.EliminarCorreosLista(row.Id);
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Anular Liquidación"
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  }
  actionBodyTemplate(rowData) {
    console.log(rowData);
    return (
      <div>
        <button
          className="btn btn-outline-danger"
          style={{ marginLeft: "0.3em" }}
          onClick={() => {
            if (
              window.confirm(
                `Esta acción elimanará el registro, ¿Desea continuar?`
              )
            )
              this.props.EliminarCorreosLista(rowData.Id);
          }}
          data-toggle="tooltip"
          data-placement="top"
          title="Anular Liquidación"
        >
          <i className="fa fa-trash" />
        </button>
      </div>
    );
  }
  onAfterSaveCell(row, cellName, cellValue) {
    console.log(row);
    console.log('Valor', cellValue);
    if (cellValue === 'dirigido') {
      row.seccion=1;
    }
    if (cellValue === 'copia') {
      row.seccion=2;
    }
    console.log("Row Actrualizado",row);
    this.props.blockScreen();
    axios
      .post("/Proyecto/ListaDistribucion/GetUpdateSeccion", {
        correo: row,
      })
      .then((response) => {
        if (response.data == "OK") {
          this.props.GetData();
          this.props.unlockScreen();
        }
      })
      .catch((error) => {
        console.log(error);
        this.props.unlockScreen();
      });
  }

  render() {
    const cellEditProp = {
      mode: "click",
      blurToSave: true,
      afterSaveCell: this.onAfterSaveCell.bind(this), // a hook for after saving cell
    };

    const jobTypes = ['dirigido', 'copia'];
    return (
      <div>
        <BootstrapTable data={this.props.data} hover={true} pagination={true} cellEdit={cellEditProp}>
          <TableHeaderColumn
            dataField="nombres"
            isKey={true}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            editable={false}
          >
            Nombre
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="externo"
            hidden
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            editable={false}
          >
            Tipo
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="correo"
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            editable={false}
          >
            Correo
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="nombre_seccion"
            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            filter={{ type: "TextFilter", delay: 500 }}
            dataSort={true}
            // dataFormat={this.readFormatter}
            editable={{ type: 'select', options: { values: jobTypes } }}
          >
            Sección
          </TableHeaderColumn>
          <TableHeaderColumn
            dataField="Id"
            dataFormat={this.generateButton.bind(this)}
          >
            Operaciones
          </TableHeaderColumn>
        </BootstrapTable>
      </div>
    );
  }

  handleRedirect(id) {
    window.location.href = "/Proyecto/RdoCabecera/Details/" + id;
  }
}
