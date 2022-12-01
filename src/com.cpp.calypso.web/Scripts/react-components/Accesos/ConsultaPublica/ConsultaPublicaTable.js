import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import config from "../../Base/Config";
import dateFormatter from "../../Base/DateFormatter";

export default class ConsultaPublicaTable extends React.Component {
    constructor(props) {
        super(props)
    }


    render() {
        return (
            <div>
                <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                    <TableHeaderColumn
                        dataField='Id'
                        width={'8%'}
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        isKey>N°</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='tipo_identificacion_nombre'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Tipo Identificación</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='identificacion'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Identificación</TableHeaderColumn>



                    <TableHeaderColumn
                        dataField='nombres_completos'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Nombres</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='condicion_cedulado'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Condición</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='fecha_consulta'
                        dataFormat={this.dateFormat}
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Fecha de Consulta</TableHeaderColumn>


                    <TableHeaderColumn
                        dataField='Id'
                        width={'20%'}
                        dataFormat={this.generarBotones}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}></TableHeaderColumn>
                </BootstrapTable>
            </div>
        )
    }

    generarBotones = (cell, row) => {
        return (

            <div>

                <button className="btn btn-outline-success"
                    onClick={() => this.props.mostrarCambiarFoto()}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Gestionar Firma">
                    <i className="fa fa-vcard"></i>
                </button>
                &nbsp;
                <button className="btn btn-outline-info"
                    onClick={() => this.descargarPlantilla(cell)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Descargar Documento">
                    <i className="fa fa-file-word-o"></i>
                </button>
                <button className="btn btn-outline-indigo" style={{ marginLeft: '0.3em' }}
                    onClick={() => this.props.mostrarListaDistribucion(cell)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Subir Documento">
                    <i className="fa fa-cloud-upload"></i>
                </button>
                {row.ArchivoPdfId != null &&
                    <button className="btn btn-outline-indigo" style={{ marginLeft: '0.3em' }}
                        onClick={() => this.onDownloadPdf(row.ArchivoPdfId)}
                        data-toggle="tooltip"
                        data-placement="top"
                        title="Descargar Documento">
                        <i className="fa fa-cloud-download"></i>
                    </button>
                }
            </div>
        )
    }

    onDownloadPdf = ArchivoPdfId => {

        return (
            window.location = `${config.appUrl}/proyecto/Archivo/Descargar/${ArchivoPdfId}`
        );
    }


    descargarPlantilla = (consultaPublicaId) => {
        if (this.props.Usuario != null) {
            window.location.href = `${config.appUrl}/Accesos/ConsultaPublica/DescargarPlantilla/${consultaPublicaId}`
        } else {

            this.props.showWarn("Debe actualizar la firma del Usuario para poder descargar el documento")
        }
    }

    dateFormat = (cell, row) => {
        if (cell === null) {
            return ''
        }
        return dateFormatter(cell);
    }
}