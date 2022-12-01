import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import config from "../../Base/Config";
import {
    CONTROLLER_TARJETA_ACCESO,
} from '../../Base/Strings';
import { Button } from "primereact-v2/button"
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
                            dataField='fechaIngreso'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Fecha Ingreso</TableHeaderColumn>
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
            <Button
                label=""
                className="p-button-outlined"
                onClick={() => this.props.seleccionarColaborador(row)}
                icon="pi pi-users"
            />
        )
    }


}