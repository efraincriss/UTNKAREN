import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";

import { Growl } from "primereact/components/growl/Growl";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";

// Tabla con GRID editable
class AvanceDeObra extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      blocking: true,
      data: [
        {
          Id: 0,
          Actividad: "",
          CodigoItem: "",
          NombreItem: "",
          CantidadAnterior: 0,
          CantidadAcumulada: 0,
          ComputoId: 0,
          Editado:false
        },
      ],
    };

    this.GetData = this.GetData.bind(this);
    this.onBeforeSaveCell = this.onBeforeSaveCell.bind(this);
    this.successMessage = this.successMessage.bind(this);
    this.warnMessage = this.warnMessage.bind(this);
    this.alertMessage = this.alertMessage.bind(this);
    this.Submit = this.Submit.bind(this);
    this.handleKeyDown = this.handleKeyDown.bind(this);
    this.DeleteButton = this.DeleteButton.bind(this);
    this.AjusteSpan = this.AjusteSpan.bind(this);
    this.Delete = this.Delete.bind(this);
  }

  componentDidMount() {
    this.GetData();
  }

  render() {
    const cellEditProp = {
      mode: "click",
      blurToSave: true,
      beforeSaveCell: this.onBeforeSaveCell,
    };

    return (
      <BlockUi tag="div" blocking={this.state.blocking}>
        <div onKeyDown={this.handleKeyDown}>
          <Growl
            ref={(el) => {
              this.growl = el;
            }}
            position="bottomright"
            baseZIndex={1000}
          ></Growl>
          <div className="row">
            <div className="col">
              <button
                onClick={this.Submit}
                className="btn btn-outline-indigo"
                style={{ marginBottom: "1em", float: "right" }}
              >
                Guardar
              </button>
            </div>
          </div>

          <BootstrapTable
            data={this.state.data}
            hover={true}
            pagination={true}
            scrollTop={"Bottom"}
            cellEdit={cellEditProp}
            
          >
            <TableHeaderColumn
              dataField="ComputoId"
              hidden
              width={"5%"}
              isKey
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              editable={false}
            >
              Id
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="padre_principal"
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              editable={false}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Nivel
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="padre_superior"
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              editable={false}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              Nivel
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Actividad"
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              editable={false}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              
            >
              Actividad
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="CodigoItem"
              editable={false}
              filter={{ type: "TextFilter", delay: 500 }}
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              width={"8%"}
              dataSort={true}
            >
              Cód Item
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="NombreItem"
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              editable={false}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              
            >
              Nombre Item
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="tienecantidadAjustada"
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "13px",
                alignContent: "center",
                alignItems: "center",
              }}
              thStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                alignContent: "center",
              }}
              editable={false}
              dataFormat={this.AjusteSpan}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
            >
              C. Ajustada{" "}
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="tipoCantidadAjustada"
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              editable={false}
              filter={{ type: "TextFilter", delay: 500 }}
              dataSort={true}
              width={"8%"}
            >
              Tipo{" "}
            </TableHeaderColumn>
            <TableHeaderColumn
              dataField="Budget"
              width={"9%"}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                textAlign: "right",
              }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              editable={false}
              dataSort={true}
            >
              Budget
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="CantidadEAC"
              width={"9%"}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                textAlign: "right",
              }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
              filter={{ type: "TextFilter", delay: 500 }}
            >
              C.EAC
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="CantidadAnterior"
              width={"9%"}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                textAlign: "right",
              }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              editable={false}
              dataSort={true}
              filter={{ type: "TextFilter", delay: 500 }}
            >
              C.Anterior
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="CantidadAcumulada"
              width={"9%"}
              tdStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                textAlign: "right",
              }}
              thStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              dataSort={true}
              filter={{ type: "TextFilter", delay: 500 }}
            >
              C.Acumulada
            </TableHeaderColumn>

            <TableHeaderColumn
              dataField="Id"
              width={"9%"}
              editable={false}
              dataFormat={this.DeleteButton}
              tdStyle={{ whiteSpace: "normal", fontSize: "11px" }}
              thStyle={{
                whiteSpace: "normal",
                fontSize: "11px",
                textAlign: "center",
              }}
            >
              Operaciones
            </TableHeaderColumn>
          </BootstrapTable>
        </div>
      </BlockUi>
    );
  }
  DeleteButton(cell, row) {
    return (
      <>
        {row.CantidadEAC === 0 && row.CantidadAnterior > 0 && (
          <button
            className="btn btn-outline-danger"
            title="Actualizar Cantidad Acumulado a Cero"
            onClick={() => {
              if (
                window.confirm(
                  "Está seguro de actualizar la cantidad acumulada del rubro " +
                    row.CodigoItem +
                    " con valor cero?"
                )
              )
                this.Delete(row);
            }}
          >
            <i className="fa fa-eraser"></i>
          </button>
        )}
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
  Delete(row) {
    console.log("row", row);
    this.setState({ blocking: true });
    axios
      .post("/proyecto/DetalleAvanceObra/CreateAvancesNegativos", {
        data: row,
        AvanceObraId: document.getElementById("AvanceObraId").className,
      })
      .then((response) => {
        this.GetData();
        this.successMessage("Se insertaron los registros.");
      })
      .catch((error) => {
        console.log(error);
        this.setState({ blocking: false });
        this.warnMessage("Vuelve a intentar más tarde");
      });
  }
  GetData() {
    axios
      .post("/proyecto/AvanceObra/GetComputosDetalles", {
        ofertaId: document.getElementById("OfertaId").className,
        fecha: document.getElementById("Fecha").className,
        AvanceObraId: document.getElementById("AvanceObraId").className,
      })
      .then((response) => {
        console.log(response.data);
        this.setState({ data: response.data, blocking: false });
      })
      .catch((error) => {
        console.log(error);
      });
  }

  onBeforeSaveCell(row, cellName, cellValue) {
    if (isNaN(cellValue)) {
      this.warnMessage("La cantidad debe ser un número.");
      return false;
    } else if (cellValue === 0) {
      this.warnMessage("La cantidad debe ser mayor a 0.");
      return false;
    }
    if (cellValue < parseFloat(row.CantidadAnterior)) {
      this.alertMessage("La cantidad acumulada es menor a la anterior");
    }
    if (cellName !== "CantidadEAC") {
      if (cellValue > parseFloat(row.CantidadEAC)) {
        this.warnMessage("La cantidad acumulada es mayor al EAC");
        return false;
      }
    }
    if (cellName === "CantidadEAC") {
      if (cellValue < parseFloat(row.CantidadAcumulada)) {
        this.warnMessage("La cantidad EAC es menor a la acumulada");
        return false;
      }
      if (cellValue < parseFloat(row.CantidadAnterior)) {
        this.warnMessage(
          "La cantidad EAC no puede ser menor a la Cantidad Anterior"
        );
        return false;
      }
    }
    row.Editado=true;
    return true;
  }

  Submit() {
  
    let data=this.state.data;
    console.log('data',this.state.data);
    let datafilter=this.state.data.filter(x=>x.Editado===true);
    console.log('datafilter',datafilter);
    this.setState({ blocking: true });

    axios
      .post("/proyecto/DetalleAvanceObra/Create", {
        data: datafilter,
        AvanceObraId: document.getElementById("AvanceObraId").className,
      })
      .then((response) => {
        this.GetData();
        this.successMessage("Se insertaron los registros.");
      })
      .catch((error) => {
        console.log(error);
        this.setState({ blocking: false });
        this.warnMessage("Vuelve a intentar más tarde");
      });
  }

  handleKeyDown(event) {
    let charCode = String.fromCharCode(event.which).toLowerCase();
    if (event.ctrlKey && charCode === "c") {
      this.Submit();
    }

    if (event.metaKey && charCode === "c") {
      this.Submit();
    }
  }

  showSuccess() {
    this.growl.show({
      life: 5000,
      severity: "success",
      summary: "Proceso exitoso!",
      detail: this.state.message,
    });
  }

  showWarn() {
    this.growl.show({
      life: 5000,
      severity: "error",
      summary: "Error",
      detail: this.state.message,
    });
  }

  showAlert() {
    this.growl.show({
      severity: "warn",
      summary: "Alerta",
      detail: this.state.message,
    });
  }

  successMessage(msg) {
    this.setState({ message: msg }, this.showSuccess);
  }

  warnMessage(msg) {
    this.setState({ message: msg }, this.showWarn);
  }

  alertMessage(msg) {
    this.setState({ message: msg }, this.showAlert);
  }
}

ReactDOM.render(<AvanceDeObra />, document.getElementById("content"));
