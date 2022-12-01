import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import config from "../../../Base/Config";
import dateFormatter from "../../../Base/DateFormatter";

export default class RequisitosTable extends React.Component {
    constructor(props) {
        super(props)
    }


    render() {
        return (
            <div className="row" style={{ marginTop: '1em' }}>
                <div className="col">
                    <BootstrapTable data={this.props.requisitos} hover={true} pagination={true}>
                        <TableHeaderColumn
                            dataField='Id'
                            width={'7%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>N°</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Codigo'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Código</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Nombre'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Nombre Requisito</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Responsable'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Responsable</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Obligatorio'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Obligatorio</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Cumple'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Cumple</TableHeaderColumn>


                        <TableHeaderColumn
                            dataField='FechaEmision'
                            dataFormat={this.dateFormat}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Fecha Emisión</TableHeaderColumn>


                        <TableHeaderColumn
                            dataField='FechaCaducidad'
                            dataFormat={this.dateFormat}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Fecha Caducidad</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Id'
                            width={'15%'}
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}></TableHeaderColumn>
                    </BootstrapTable>
                </div>
            </div>
        )
    }

    generarBotones = (cell, row) => {
        return (

            <div>
                {row.Editable &&
                    <button className="btn btn-outline-info"
                        onClick={() => this.props.mostrarUploadFile(row)}>
                        <i className="fa fa-pencil"></i>
                    </button>
                }
                {row.ArchivoId != null &&
                    <button className="btn btn-outline-indigo" style={{ marginLeft: '0.3em' }}
                        onClick={() => this.onDownloadPdf(row.ArchivoId)}>
                        <i className="fa fa-cloud-download"></i>
                    </button>
                }
            </div>
        )
    }

    onDownloadPdf = ArchivoId => {
        return (
            window.location = `/Accesos/ValidacionRequisito/Descargar/${ArchivoId}`
        );
    }

    dateFormat = (cell, row) => {
        if (cell === null) {
            return ''
        }
        return dateFormatter(cell);
    }

}