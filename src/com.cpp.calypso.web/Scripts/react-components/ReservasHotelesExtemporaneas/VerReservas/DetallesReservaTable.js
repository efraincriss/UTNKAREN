import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import dateFormatter from "../../Base/DateFormatter";
import { DataTable } from 'primereact-v2/datatable';
import { Column } from 'primereact-v2/column';

export default class DetallesReservaTable extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            detallesseleccionados: []

        };
        this.RegistrarConsumo = this.RegistrarConsumo.bind(this);
    }

    handleRowSelect(isSelected, rows) {
        console.log(rows)
    }

    render() {

        return (
            <div>
                <div align="right">
                    <button
                        type="button"
                        className="btn btn-outline-primary"
                        icon="fa fa-fw fa-ban"
                        onClick={() => { if (window.confirm(`¿Está seguro en registrar el consumo de hospedaje?`)) this.RegistrarConsumo(); }}
                    >
                        Registrar Consumos Hospedaje
                    </button>&nbsp;
                    <button
                        type="button"
                        className="btn btn-outline-primary"
                        icon="fa fa-fw fa-ban"
                        onClick={() => { if (window.confirm(`¿Está seguro en anular el consumo de hospedaje?`)) this.CancelarConsumo(); }}
                    >
                        Anular Hospedaje
                    </button>&nbsp;
                    <button
                        type="button"
                        className="btn btn-outline-primary"
                        icon="fa fa-fw fa-ban"
                        onClick={() => { if (window.confirm(`¿Está seguro en registrar el consumo de lavandería?`)) this.RegistrarLavanderia(); }}
                    >
                        Registrar Consumos Lavandería
                    </button>&nbsp;
                    <button
                        type="button"
                        className="btn btn-outline-primary"
                        icon="fa fa-fw fa-ban"
                        onClick={() => { if (window.confirm(`¿Está seguro en anular el consumo de lavandería?`)) this.CancelarLavanderia(); }}
                    >
                        Anular Lavandería
                    </button>
                </div>
                <br />
                <div className="row" style={{ marginTop: '1em' }}>
                    <div className="col">
                        <DataTable value={this.props.data}
                            selection={this.state.detallesseleccionados} paginator={true}

                            rows={10}
                            rowsPerPageOptions={[5, 10, 20]}
                            onSelectionChange={e => this.setState({ detallesseleccionados: e.value })}>
                            <Column selectionMode="multiple" style={{ width: '3em' }} />
                            <Column field="Id" header="N°" filter={true} style={{ width: '5em' }} />
                            <Column field="fecha_reserva_format" header="Fecha Reserva" filter={true} filterMatchMode="contains" />
                            <Column field="fecha_consumo_format" header="Fecha Consumo" filter={true} filterMatchMode="contains" />
                            <Column field="consumido_nombre" header="Consumido Hospedaje" filter={true} filterMatchMode="contains" />
                            <Column field="aplica_lavanderia_nombre" header="Consumido Lavandería" filter={true} filterMatchMode="contains" />
                            <Column field="facturado_nombre" header="Facturado" filter={true} />


                        </DataTable>

                    </div>
                </div>
            </div>
        )
    }

    RegistrarConsumo() {
        console.log(this.state.detallesseleccionados)
        if (this.state.detallesseleccionados.length > 0) {
            this.props.ActualizarDetalles(this.state.detallesseleccionados)
            //this.props.consultarDetalles(this.props.reservaId)
            this.setState({ detallesseleccionados: [] })
        } else {
            this.props.showWarn("Seleccione al menos una fila de la tabla")
        }
    }
    CancelarConsumo() {
        if (this.state.detallesseleccionados.length > 0) {
            console.log(this.state.detallesseleccionados)
            this.props.ActualizarDetallesNoConsumido(this.state.detallesseleccionados)

           // this.props.consultarDetalles(this.props.reservaId)
            this.setState({ detallesseleccionados: [] })
        } else {
            this.props.showWarn("Seleccione al menos una fila de la tabla")
        }
    }

    RegistrarLavanderia() {
        console.log(this.state.detallesseleccionados)
        if (this.state.detallesseleccionados.length > 0) {
            this.props.ActualizarLavanderiaSi(this.state.detallesseleccionados)
            //this.props.consultarDetalles(this.props.reservaId)
            this.setState({ detallesseleccionados: [] })
        } else {
            this.props.showWarn("Seleccione al menos una fila de la tabla")
        }
    }
    CancelarLavanderia() {
        if (this.state.detallesseleccionados.length > 0) {
            console.log(this.state.detallesseleccionados)
            this.props.ActualizarLavanderiaNo(this.state.detallesseleccionados)

          //  this.props.consultarDetalles(this.props.reservaId)
            this.setState({ detallesseleccionados: [] })
        } else {
            this.props.showWarn("Seleccione al menos una fila de la tabla")
        }
    }

}