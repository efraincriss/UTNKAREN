import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import config from "../../../Base/Config";
import {
    CONTROLLER_TARJETA_ACCESO,
    MODULO_ACCESO,
    CONTROLLER_VALIDACION_REQUISITO
} from '../../../Base/Strings';

export default class ColaboradorTable extends React.Component {
    constructor(props) {
        super(props)
    }


    render() {
        return (
            <div className="row" style={{ marginTop: '1em' }}>
                <div className="col">
                    <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                        <TableHeaderColumn
                            dataField='Id'
                            width={'8%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>No.</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='numero_identificacion'
                            width={'25%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>No. Identificacion</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='nombres_apellidos'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Apellidos Nombres</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='grupo_personal'
                            dataFormat={this.dateFormat}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Agrupación para Requisitos</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='estado'
                            dataFormat={this.dateFormat}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Estado</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='Id'
                            width={'8%'}
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}></TableHeaderColumn>
                    </BootstrapTable>
                </div>
            </div>
        )
    }

    generarBotones = (cell, row) => {
        return (
            <button className="btn btn-outline-info" data-toggle="tooltip" data-placement="left" title="VER"
                onClick={
                    () => this.onRedirect(cell)
                }>
                <i className="fa fa-eye"></i>
            </button>
        )
    }

    onRedirect = colaboradorId => {
        switch (this.props.source) {
            case "requisitos":
                window.location.href = `/Accesos/ValidacionRequisito/Index?colaboradorId=${colaboradorId}`
                console.log("aaa")
                break;
            case "tarjetas":
                window.location.href = `${config.appUrl}Accesos/${CONTROLLER_TARJETA_ACCESO}/Index?colaboradorId=${colaboradorId}`
                break;

            default:
                window.location.href = `${config.appUrl}Accesos/${CONTROLLER_TARJETA_ACCESO}/Index?colaboradorId=${colaboradorId}`
        }

    }
}