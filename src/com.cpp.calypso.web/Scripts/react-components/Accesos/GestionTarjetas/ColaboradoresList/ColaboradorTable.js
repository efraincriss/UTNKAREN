import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import config from "../../../Base/Config";
import {CONTROLLER_TARJETA_ACCESO} from '../../../Base/Strings';

export default class ColaboradorTable extends React.Component {
    constructor(props){
        super(props)
    }


    render(){
        return(
            <div className="row" style={{ marginTop: '1em' }}>
                <div className="col">
                    <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                        <TableHeaderColumn
                            dataField='Id'
                            width={'8%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            isKey>N°</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='numero_identificacion'
                            width={'25%'}
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Identificacion</TableHeaderColumn>

                        <TableHeaderColumn
                            dataField='nombres_apellidos'
                            tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                            filter={{ type: 'TextFilter', delay: 500 }}
                            dataSort={true}>Nombres</TableHeaderColumn>

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
            <button className="btn btn-outline-info"
                onClick={
                    () => this.onRedirect(cell)
                }>
                <i className="fa fa-eye"></i>
            </button>
        )
    }

    onRedirect = proveedorId => {
        window.location.href = `/Accesos/TarjetaAcceso/Index?colaboradorId=${proveedorId}`
    }
}