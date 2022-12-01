import React from 'react';
import axios from 'axios';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';

export default class ModificacionPresupuestoTableA extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            key_table: 89247,
            blocking: false,
        }
        this.updateTableKey = this.updateTableKey.bind(this)
        this.actualizarEAC = this.actualizarEAC.bind(this)
        this.onBeforeSaveCell = this.onBeforeSaveCell.bind(this)
        this.actualizacionEAC = this.actualizacionEAC.bind(this)
        this.bloquearFalso = this.bloquearFalso.bind(this)
        this.CommaFormatted = this.CommaFormatted.bind(this)
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

                <div className="row">
                    <div className="col-sm-1"></div>

                    <div className="col-sm-2">
                        <div className="row">
                            <b>Clase: </b>
                        </div>
                        <div className="row">
                            <label htmlFor="clase">{this.props.clase}</label>
                        </div>
                    </div>
                    <div className="col-sm-2">
                        <div className="row">
                            <b>Monto Total:</b>
                        </div>
                        <div className="row">
                            <label htmlFor="clase">{this.CommaFormatted(this.props.monto_total)}</label>
                        </div>
                    </div>
                    <div className="col-sm-2">
                        <div className="row">
                            <b>Monto Construcción:</b>
                        </div>
                        <div className="row">
                            <label htmlFor="clase">{this.CommaFormatted(this.props.monto_construccion)}</label>
                        </div>
                    </div>
                    <div className="col-sm-2">
                        <div className="row">
                            <b>Monto Ingeniería:</b>
                        </div>
                        <div className="row">
                            <label htmlFor="clase">{this.CommaFormatted(this.props.monto_ingenieria)}</label>
                        </div>
                    </div>
                    <div className="col-sm-2">
                        <div className="row">
                            <b>Monto Procura:</b>
                        </div>
                        <div className="row">
                            <label htmlFor="clase">{this.CommaFormatted(this.props.monto_procura)}</label>
                        </div>
                    </div>

                    <div className="col-sm-1"></div>

                </div>
                <hr />
                <BlockUi tag="div" blocking={this.state.blocking}>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>

                    <BootstrapTable data={this.props.computos} hover={true} pagination={true} cellEdit={cellEditProp}>
                        <TableHeaderColumn dataField="actividad_nombre" width={'5%'} editable={false} dataAlign="center" isKey={true} filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Actividad WBS</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_padre_codigo" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Cód. Padre</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_padre_nombre" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Ítem Padre</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_codigo" width={'20%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Cód. Ítem</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_nombre" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Ítem</TableHeaderColumn>
                        <TableHeaderColumn dataField="cantidad" width={'16%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true} dataAlign='right'>Cantidad</TableHeaderColumn>
                        <TableHeaderColumn dataField="costo_total" width={'15%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true} dataAlign='right'>P.U.</TableHeaderColumn>
                        <TableHeaderColumn dataField="precio_unitario" width={'15%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true} dataAlign='right'>Total</TableHeaderColumn>
                    </BootstrapTable>
                </BlockUi>
            </div>
        )
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

    CommaFormatted(amount) {
        var delimiter = ","; // reemplazar coma
        var a = amount.split('.', 2)
        var d = a[1];
        var i = parseInt(a[0]);
        if (isNaN(i)) { return ''; }
        var minus = '';
        if (i < 0) { minus = '-'; }
        i = Math.abs(i);
        var n = new String(i);
        var a = [];
        while (n.length > 3) {
            var nn = n.substr(n.length - 3);
            a.unshift(nn);
            n = n.substr(0, n.length - 3);
        }
        if (n.length > 0) { a.unshift(n); }
        n = a.join(delimiter);
        if (d != null && d != '') {
            if (d.length < 1) { amount = n; }
            else { amount = n + '.' + d; }
            amount = minus + amount;
            return amount;
        }
        return '0.00';
    }

}