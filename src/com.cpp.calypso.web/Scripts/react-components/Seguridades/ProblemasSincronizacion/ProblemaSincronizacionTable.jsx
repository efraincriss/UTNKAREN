import React from "react";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import moment from 'moment';
import axios from "axios";

export class ProblemaSincronizacionTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            selected: []
        }
    }

    render() {

        const selectRow = {
            mode: 'checkbox',
            clickToSelect: true,
            selected: this.state.selected,
            onSelect: this.handleOnSelect,
            onSelectAll: this.handleOnSelectAll
        };
        return (
            <div className="row" style={{ marginTop: "1em" }}>
                <div className="col">
                    <div className="row" align="right">
                        <div className="col">
                            <div>
                                <button
                                    className="btn btn-outline-primary mr-4"
                                    type="button" aria-expanded="false"
                                    aria-controls="collapseExample"
                                    onClick={() => this.mostrarFormularioMasivo()}
                                >Atender Masivamente</button>
                                <button
                                    className="btn btn-outline-primary mr-4"
                                    type="button" aria-expanded="false"
                                    aria-controls="collapseExample"
                                    onClick={() => this.descargarExcel()}
                                >Descargar Excel</button>
                            </div>
                        </div>
                        
                    </div>
                    <div className="row" style={{ marginTop: "0.5em" }}>
                        <div className="col">
                            <BootstrapTable data={this.props.data} hover={true} pagination={true} selectRow={selectRow}>

                                <TableHeaderColumn
                                    dataField="Id"
                                    dataFormat={this.index}
                                    width={"8%"}
                                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    filter={{ type: "TextFilter", delay: 500 }}
                                    isKey
                                >
                                    N°
                                </TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField="Fecha"
                                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    filter={{ type: "TextFilter", delay: 500 }}
                                    dataSort={true}
                                    dataFormat={this.formatoFecha}
                                >
                                    Fecha
                                </TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField="Fuente"
                                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    filter={{ type: "TextFilter", delay: 500 }}
                                    dataSort={true}
                                    width={"25%"}
                                >
                                    Fuente
                                </TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField="Entidad"
                                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    filter={{ type: "TextFilter", delay: 500 }}
                                    dataSort={true}
                                >
                                    Entidad
                                </TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField="UsuarioId"
                                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    filter={{ type: "TextFilter", delay: 500 }}
                                    dataSort={true}
                                >
                                    Usuario
                                </TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField="Solucionado"
                                    tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    filter={{ type: "TextFilter", delay: 500 }}
                                    dataSort={true}
                                    dataFormat={this.formatoBoolean}
                                >
                                    Solucionado?
                                </TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField="Id"
                                    width={"18%"}
                                    dataFormat={this.generarBotones}
                                    thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                ></TableHeaderColumn>
                            </BootstrapTable>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    handleOnSelect = (row, isSelect) => {
        if (isSelect) {
            this.setState(() => ({
                selected: [...this.state.selected, row.Id]
            }));
        } else {
            this.setState(() => ({
                selected: this.state.selected.filter(x => x !== row.Id)
            }));
        }
    }

    handleOnSelectAll = (isSelect, rows) => {
        const ids = rows.map(r => r.Id);
        if (isSelect) {
            this.setState(() => ({
                selected: ids
            }));
        } else {
            this.setState(() => ({
                selected: []
            }));
        }
    }

    descargarExcel = () => {
        if (this.state.selected.length === 0) {
            this.props.showWarn("Debes seleccionar al menos 1 registro");
            return;
        }
        this.props.blockScreen()
        axios
            .post(
                `/Seguridad/ProblemaSincronizacion/DescargarPlantillaCargaMasivaDeJornales`, {
                ids: this.state.selected
            },
                { responseType: "arraybuffer" }
            )
            .then((response) => {
                console.log(response)
                var nombre = response.headers["content-disposition"].split("=");

                const url = window.URL.createObjectURL(
                    new Blob([response.data], {
                        type:
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    })
                );
                const link = document.createElement("a");
                link.href = url;
                link.setAttribute("download", nombre[1]);
                document.body.appendChild(link);
                link.click();
                this.props.showSuccess("Formato descargado exitosamente")
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Ocurrió un error al descargar el archivo, intentalo nuevamente");
                this.props.unlockScreen();
            });
    }

    generarBotones = (cell, row) => {
        if (row.Solucionado) {
            return (
                <div>
                    <button
                        className="btn btn-outline-info mr-2"
                        onClick={() => this.onEdit(row)}
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Editar"
                    >
                        <i className="fa fa-pencil"></i>
                    </button>
                    <button
                        className="btn btn-outline-info mr-2"
                        onClick={() => this.props.mostrarConfirmacion(row)}
                        data-toggle="tooltip"
                        data-placement="top"
                        title="No Solucionado"
                    >
                        <i className="fa fa-warning "></i>
                    </button>
                </div>
            )
        }
        return (

            <div>
                <button
                    className="btn btn-outline-info mr-2"
                    onClick={() => this.onWatch(row)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Atender"
                >
                    <i className="fa fa-exclamation"></i>
                </button>
            </div>
        )
    };

    onWatch = (row) => {
        console.log(row);
        this.props.mostrarModal(row)
    }

    onEdit = (row) => {
        console.log(row);
        this.props.editarErrorSincronizacion(row)
    }


    index(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }

    formatoFecha = (cell, row) => {
        if (cell != null) {
            var informacion = cell.split("T")
            return informacion[0] + "  " + informacion[1].substring(0, 5);
        } else {
            return null;
        }

    }

    formatoBoolean = (cell, row) => {
        if (cell) {
            return "SI"
        } else {
            return "NO";
        }
    }

    formatoFechaCorta = (cell, row) => {
        if (cell != null) {
            var informacion = cell.split("T")
            return informacion[0];
        } else {
            return null;
        }
    }

    mostrarFormularioMasivo = () => {
        if (this.state.selected.length === 0) {
            this.props.showWarn("Debes seleccionar al menos un problema de sincronización")
        } else {
            this.props.mostrarFormularioMasivo(this.state.selected);
        }
    }
}