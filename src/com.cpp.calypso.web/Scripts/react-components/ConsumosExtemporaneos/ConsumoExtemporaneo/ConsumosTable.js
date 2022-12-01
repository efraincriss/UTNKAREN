import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Button, Tooltip } from "shards-react";
import dateFormatter from "../../Base/DateFormatter";
import config from "../../Base/Config";
const PANTALLA_DETALLES = 'Detalles';

export default class ConsumosExtemporaneosTable extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            open: false
        }
    }


    render() {
        return (
            <div className="row" style={{ marginTop: '1em' }}>
                <div className="col">
                    <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                        <TableHeaderColumn
                            dataField='Secuencia'
                            width={'8%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>N°</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='ProveedorNombre'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Proveedor</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Fecha'
                            dataFormat={this.dateFormat}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Fecha Registro</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='TipoComidaNombre'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Tipo Comida</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Id'
                            width={'25%'}
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
                <Button
                    id="TooltipExample"
                    outline
                    onClick={() => this.props.loadDetalles(row)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="VER"
                >
                    Ver
                </Button>



                <Button
                    onClick={() => this.props.setConsumo(row)}
                    style={{ marginLeft: '0.3em' }}
                    outline theme="success"

                    data-toggle="tooltip"
                    data-placement="top"
                    title="EDITAR"
                >
                    Editar
                </Button>
                {row.DocumentoRespaldoId != null &&
                    <Button
                        style={{ marginLeft: '0.3em' }}
                        onClick={() => this.onDownloadPdf(row.DocumentoRespaldoId)}
                        data-toggle="tooltip"
                        data-placement="top"
                        title="DESCARGAR DOCUMENTO"
                        outline pill theme="secondary"
                    >
                        <i className="fa fa-cloud-download"></i>
                    </Button>
                }
            </div>
        )
    }

    onDownloadPdf = DocumentoRespaldoId => {

        return (
            window.location = `${config.appUrl}/proyecto/Archivo/Descargar/${DocumentoRespaldoId}`
        );
    }

    dateFormat = (cell, row) => {
        if (cell === null) {
            return ''
        }
        return dateFormatter(cell);
    }

    toggle = () => {
        this.setState({
            open: !this.state.open
        });
    }
}