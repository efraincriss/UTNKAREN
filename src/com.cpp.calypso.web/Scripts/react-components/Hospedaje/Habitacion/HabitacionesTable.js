import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { 
    FRASE_HABITACION_ELIMINADA, 
    FRASE_ERROR_GLOBAL,
    MODULO_PROVEEDOR,
    CONTROLLER_HABITACION 
} from '../../Base/Strings';
import http from "../../Base/HttpService";
import config from "../../Base/Config";



export default class HabitacionesTable extends React.Component {
    constructor(props) {
        super(props)
    }



    render() {
        return (
            <div>
                <div className="row" align="right">
                    <div className="col">
                        <button className="btn btn-outline-primary" onClick={() => this.props.mostrarForm(false)}>Nuevo</button>
                    </div>
                </div>
                <div className="row" style={{ marginTop: '1em' }}>
                    <div className="col">
                        <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                            <TableHeaderColumn
                                dataField='secuencial'
                                width={'8%'}
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                isKey>N°</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='numero_habitacion'
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>No. Habitación</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='tipo_habitacion_nombre'
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Tipo Habitación</TableHeaderColumn>

                            <TableHeaderColumn
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'right' }}
                                dataField="capacidad"
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort>Capacidad</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='estado_nombre'
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Estado</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='Id'
                                width={'13%'}
                                dataFormat={this.generarBotones}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>Opciones</TableHeaderColumn>
                        </BootstrapTable>
                    </div>
                </div>
            </div>
        )
    }

    generarBotones = (cell, row) => {
        return (
            <div>
                <button className="btn btn-outline-info" data-toggle="tooltip" data-placement="left" title="Editar"
                    style={{ marginRight: '0.3em' }}
                    onClick={() => { this.props.getHabitacionDetalle(cell) }}>
                    <i className="fa fa-pencil"></i>
                </button>

                {row.estado === true ?
                    <button className="btn btn-outline-danger" data-toggle="tooltip" data-placement="left" title="Inactivar"
                        onClick={() => { if (window.confirm(`Esta acción inactivará la habitación ${row.numero_habitacion} y sus espacios, ¿Desea continuar?`)) this.deleteHabitacion(cell, false) }}>
                        <i className="fa fa-minus-square"></i>
                    </button>
                    :
                    <button className="btn btn-outline-success" data-toggle="tooltip" data-placement="left" title="Activar"
                        onClick={() => { if (window.confirm(`Esta acción activará la habitación ${row.numero_habitacion} y sus espacios, ¿Desea continuar?`)) this.deleteHabitacion(cell, true) }}>
                        <i className="fa fa-check-square"></i>
                    </button>
                }


            </div>
        )
    }

    deleteHabitacion = (id, estado) => {
        this.props.blockScreen();
        let url = '';
        var entity = {
            habitacionId: id,
            estado: estado
        }
        url = `${config.appUrl}${MODULO_PROVEEDOR}/${CONTROLLER_HABITACION}/SwitchHabitacionEstado`
        http.post(url, entity)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.deleteSuccess();
                } else {
                    this.props.unlockScreen();
                    this.props.showWarn(FRASE_ERROR_GLOBAL)
                    console.log(data.errors)
                }
            })
            .catch((error) => {
                this.props.unlockScreen();
                this.props.showWarn(FRASE_ERROR_GLOBAL)
                console.log(error)
            })
    }

    deleteSuccess = () => {
        this.props.showSuccess("Acción procesada satisfactoriamente")
        this.props.consultarEspaciosYHabitaciones();
    }


}