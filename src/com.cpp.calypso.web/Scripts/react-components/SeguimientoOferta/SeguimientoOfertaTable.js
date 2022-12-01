import React from 'react';
import axios from 'axios';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';

export default class SeguimientoOfertaTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            computos: [],
            key_table: 89247,
            blocking: false,
        }
        this.GetComputos = this.GetComputos.bind(this);
        this.updateTableKey = this.updateTableKey.bind(this)
        this.actualizarEAC = this.actualizarEAC.bind(this)
        this.onBeforeSaveCell = this.onBeforeSaveCell.bind(this)
        this.actualizacionEAC = this.actualizacionEAC.bind(this)
        this.obtenerComputos = this.obtenerComputos.bind(this)
        this.bloquearFalso = this.bloquearFalso.bind(this)
        this.computoTemporal = []
        this.json = ''
    }

    render() {
        const cellEditProp = {
            mode: 'click',
            blurToSave: true,
            beforeSaveCell: this.onBeforeSaveCell,
        }
        
        return (
            <div>
                <BlockUi tag="div" blocking={this.state.blocking}>
                    <br />
                    <div className="row">
                        <div className="col-sm-5"></div>
                        <div className="col-sm-2">
                            <button className="btn btn-primary" onClick={this.obtenerComputos} style={{ float: 'left', marginRight: '0.3em' }}>Cargar Computos</button>
                        </div>
                        <div className="col-sm-5"></div>
                    </div>
                    <hr />
                    <div className="row">
                        <div className="col-sm-10"></div>
                        <div className="col-sm-2">
                            <button className="btn btn-primary" onClick={this.actualizacionEAC} style={{ float: 'left', marginRight: '0.3em' }}> Actualizar EAC </button>
                        </div>
                    </div>
                    <br />
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>

                    <BootstrapTable data={this.state.computos} hover={true} pagination={true} cellEdit={cellEditProp}>
                        <TableHeaderColumn dataField="Id" width={'5%'} editable={false} dataAlign="center" isKey={true} filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Id</TableHeaderColumn>
                        <TableHeaderColumn dataField="oferta" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Oferta</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_padre_codigo" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Cod. Padre</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_padre_nombre" width={'20%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Item Padre</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_codigo" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Cod. Item</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_nombre" width={'16%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Item</TableHeaderColumn>
                        <TableHeaderColumn dataField="cantidad" width={'15%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Cant.Presupuestada</TableHeaderColumn>
                        <TableHeaderColumn dataField="cantidad_eac" width={'11%'} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Cant.EAC</TableHeaderColumn>
                    </BootstrapTable>
                </BlockUi>
            </div>
        )
    }

    obtenerComputos() {
        this.setState({ blocking: true}, this.GetComputos)
    }

    GetComputos() {
        if (this.props.id_proyecto == 0) {
            this.props.showWarn("Se debe Seleccionar un Proyecto previamente.")
            this.setState({ blocking: false })
        } else {
            var id = this.props.id_proyecto
            axios.post("/Proyecto/Computo/GetComputosPorProyectoApi/" + id, {})
                .then((response) => {
                    this.setState({ computos: response.data, blocking: false  })
                    this.props.showSuccess("Computos Cargados.")
                    this.computoTemporal = response.data
                    console.log(this.computoTemporal)
                })
                .catch((error) => {
                    this.setState({ blocking: false })
                    console.log(error);
                });
        }
    }

    updateTableKey() {
        this.setState({ key_table: Math.random() })
    }

    onBeforeSaveCell(row, cellName, cellValue) {
        if (isNaN(cellValue) && cellValue > 0) {
            this.props.showWarn("El valor de EAC debe ser númerico.")
            return false;
        } else {
            return true;
        }
    }

    bloquearFalso() {
        this.setState({ blocking: false })
    }

    actualizacionEAC() {
        this.setState({ blocking: true }, this.actualizarEAC)
    }

    actualizarEAC() {
        if (this.props.id_proyecto == 0) {
        } else {
            var id = this.props.id_proyecto
            axios.post("/Proyecto/Computo/GetComputosPorProyectoApi/" + id, {})
                .then((response) => {
                    this.computoTemporal = response.data
                    console.log(this.computoTemporal)
                    console.log("esta ")
                })
                .catch((error) => {
                    this.setState({ blocking: false })
                    console.log(error);
                });
        }

        if (this.state.computos.length == 0) {
            this.props.showWarn("No se han cargado Computos.")
            this.setState({ blocking: false })
        } else {
            var cont = 0;
            for (i = 0; i < (this.state.computos.length - 1); i++) {
                console.log(this.state.computos[i].cantidad_eac + "-computo " +
                    this.computoTemporal[i].cantidad_eac + "-computoTemp")
                if (this.state.computos[i].cantidad_eac != this.computoTemporal[i].cantidad_eac) {
                    cont++
                }
            }

            var i = 0
            var o = 0
            this.json = JSON.stringify(this.state.computos)
            return (
                this.state.computos.map((item) => {
                    if (cont > 0) {
                        if (this.state.computos[o].cantidad_eac != this.computoTemporal[o].cantidad_eac) {
                            axios.post("/Proyecto/Computo/ActualizarValorEac/",
                                { idProyecto: this.props.id_proyecto, valorEac: this.state.computos[o].cantidad_eac, computos: this.json })
                                .then((response) => {
                                    i++
                                    console.log(i + " i " + cont + " cont")
                                    if (i == cont) {
                                        this.setState({ blocking: false },
                                            () => this.props.showSuccess("Se han actualizado " + cont + " registros."))
                                    }
                                })
                                .catch((error) => {
                                    this.setState({ blocking: false })
                                    console.log(error);
                                    this.props.showWarn("Algo salio mal.")
                                });
                        }
                    } else {
                        this.setState({ blocking: false })
                        this.props.showSuccess("No se han hecho Cambios.")
                    }
                    o++
                })
            );
        }
    }
}