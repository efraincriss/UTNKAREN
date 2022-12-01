import React from "react";
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class LugaresTable extends React.Component {
    constructor(props) {
        super(props)
    }


    render() {
        return (
            <div>
                <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                    <TableHeaderColumn
                        dataField='Secuencial'
                        width={'8%'}
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
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        dataField="Nombre"
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort>Nombre</TableHeaderColumn>

                    <TableHeaderColumn
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        dataField="Latitud"
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort>Latitud</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='Longitud'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Longitud</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='Descripcion'
                        tdStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                        filter={{ type: 'TextFilter', delay: 500 }}
                        dataSort={true}>Descripción</TableHeaderColumn>

                    <TableHeaderColumn
                        dataField='Id'
                        width={'12%'}
                        dataFormat={this.generarBotones}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}>Opciones</TableHeaderColumn>
                </BootstrapTable>
            </div>
        )
    }

    generarBotones = (cell, row) => {
        return (
            <div>
                <button className="btn btn-outline-info"
                    onClick={() => this.props.mostrarForm(row, true)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Editar Lugar">
                    <i className="fa fa-pencil"></i>
                </button>
                <button className="btn btn-outline-danger" style={{ marginLeft: '0.3em' }}
                    onClick={() => { if (window.confirm(`Esta acción eliminará el lugar con código ${row.Codigo}, ¿Desea continuar?`)) this.props.onDelete(cell) }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Eliminar Lugar">
                    <i className="fa fa-trash"></i>
                </button>
            </div>
        )
    }
}