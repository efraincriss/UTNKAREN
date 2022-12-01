import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import http from "../../Base/HttpService"
import { Button } from "primereact/components/button/Button";

export default class EspaciosTable extends React.Component {
    constructor(props) {
        super(props)
    }



    render() {
        return (
            <>
                <div className="row alert alert-warning">
                    <div className="col">
                 <p><strong>Nota:</strong> El campo <strong> Disponibilidad, </strong> lo define el proveedor desde el app móvil.</p>   
                
                    </div></div>
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
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>No. Habitación</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='tipo_habitacion_nombre'
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Tipo Habitación</TableHeaderColumn>

                            <TableHeaderColumn
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                dataField="codigo_espacio"
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort>Espacio</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='estado_nombre'
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Disponibilidad</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='activo_nombre'
                                tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Activo/Inactivo</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='Id'
                                width={'10%'}
                                dataFormat={this.generarBotones}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}></TableHeaderColumn>
                        </BootstrapTable>
                    </div>
                </div>
            </>
        )
    }

    generarBotones = (cell, row) => {
        return (
            <>
                {row.activo === true ?
                    <button className="btn btn-outline-danger" data-toggle="tooltip" data-placement="left" title="Inactivar"
                        onClick={() => this.activarDesactivarEspacioI(row)}>
                        <i className="fa fa-minus-square-o"></i>
                    </button>
                    :

                    <button className="btn btn-outline-info" data-toggle="tooltip" data-placement="left" title="Activar"
                        onClick={() => this.activarDesactivarEspacio(row)}>
                        <i className="fa fa-check-square-o"></i>
                    </button>}
            </>
        )
    }

    activarDesactivarEspacio = (entity) => {
        if (entity.EspaciosHabitacionConfig < entity.capacidadHabitacionConfig) {
            console.log("Capacidad dentro de rango",entity)

        } else {
            this.props.showWarn("La habitación tiene capacidad de "+entity.capacidadHabitacionConfig+ " espacios, no se puede activar uno más. Dirigirse a la viñeta “Stock de Habitaciones” para aumentar espacios.");
            return;
        }



        this.props.blockScreen();


        http.post('/Proveedor/Habitacion/ActivarDesactivarEspacio/' + entity.Id, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.getEspacios();
                    this.props.consultarEspaciosYHabitaciones();
                } else if (data.success === false) {
                    var message = data.error;
                    this.props.showWarn(message);
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }

    activarDesactivarEspacioI = (entity) => {
        console.log('Espacio', entity);
        console.log('Capacidad', entity.capacidadHabitacionConfig);
        console.log('Espacios', entity.EspaciosHabitacionConfig);
        if (entity.EspaciosHabitacionConfig > entity.capacidadHabitacionConfig) {

        } else {
            this.props.showWarn("No se puede Inactivar. El número de espacios activos debe estar acorde a la capacidad registrada");
            return;
        }


        this.props.blockScreen();


        http.post('/Proveedor/Habitacion/ActivarDesactivarEspacio/' + entity.Id, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.getEspacios();
                    this.props.consultarEspaciosYHabitaciones();
                } else if (data.success === false) {
                    var message = data.error;
                    this.props.showWarn(message);
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }
}