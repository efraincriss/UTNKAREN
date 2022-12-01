import React from 'react';
import axios from 'axios';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';

export default class ModificacionPresupuestoTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            key_table: 89247,
            blocking: false,
            selected: [],
            bandera: false,
        }
        this.updateTableKey = this.updateTableKey.bind(this)
        this.actualizarEAC = this.actualizarEAC.bind(this)
        this.onBeforeSaveCell = this.onBeforeSaveCell.bind(this)
        this.actualizacionEAC = this.actualizacionEAC.bind(this)
        this.bloquearFalso = this.bloquearFalso.bind(this)
        this.getSelectedRowKeys = this.getSelectedRowKeys.bind(this)
        this.agregarItemsComputo = this.agregarItemsComputo.bind(this)
        this.emitirPresupuesto = this.emitirPresupuesto.bind(this)
        this.CommaFormatted = this.CommaFormatted.bind(this)
        this.json = ''
    }

    componentWillReceiveProps(nextProps) {
        
        if (!this.state.bandera) {
            this.setState({ computosTemporal: nextProps.computosTemporal, bandera: false });
        }
        this.setState({ bandera: false });
    }

    render() {

        const cellEditProp = {
            mode: 'click',
            blurToSave: true,
            beforeSaveCell: this.onBeforeSaveCell,
        }

        const selectRowProp = {
            mode: 'checkbox',
            clickToSelect: true,
            bgColor: '#fefefe'
        }

        function activeFormatter(cell, row) {
            return (
                <input type='checkbox' checked={row.diferente} />
            );
        }

        return (
            <div>
                <div className="row">
                    <div className="col-sm-4"></div>
                    <div className="col-sm-8">
                        <button className="btn btn-primary" onClick={this.actualizacionEAC} style={{ float: 'right', marginRight: '0.3em' }}>
                            <i className="	fa fa-envelope"></i>
                        </button>
                        <button className="btn btn-primary" onClick={this.emitirPresupuesto} style={{ float: 'right', marginRight: '0.3em' }}>Aprobar</button>
                        <button className="btn btn-primary" onClick={this.agregarItemsComputo} style={{ float: 'right', marginRight: '0.3em' }}>Cargar Nuevos Ítems</button>
                        <button className="btn btn-primary" onClick={this.actualizacionEAC} style={{ float: 'right', marginRight: '0.3em' }}>Actualizar EAC</button>
                    </div>
                </div>
                <hr />
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

                    <BootstrapTable data={this.state.computosTemporal} selectRow={selectRowProp} hover={true} pagination={true} cellEdit={cellEditProp} ref='table'>
                        <TableHeaderColumn dataField="Id" editable={false} dataAlign="center" isKey={true} filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Id</TableHeaderColumn>
                        <TableHeaderColumn dataField="actividad_nombre" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Actividad<br />WBS</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_padre_codigo" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Cód.<br />Padre</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_padre_nombre" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Ítem<br /> Padre</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_codigo" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Cod.<br />Item</TableHeaderColumn>
                        <TableHeaderColumn dataField="item_nombre" width={'11%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Ítem</TableHeaderColumn>
                        <TableHeaderColumn dataField="cantidad"  tdStyle={{ background: '#99bbff' }} width={'9%'} editable={true} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true} dataAlign='right'>Cant.<br />Budget</TableHeaderColumn>
                        <TableHeaderColumn dataField="cantidad_eac"  tdStyle={{ background: '#99bbff' }} width={'9%'} editable={true} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true} dataAlign='right'>Cant.<br />EAC</TableHeaderColumn>
                        <TableHeaderColumn dataField="precio_unitario"  width={'9%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true} dataAlign='right'>P.U.<br />Preciario</TableHeaderColumn>
                        <TableHeaderColumn dataField="precio_incrementado"  width={'9%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true} dataAlign='right'>P.U <br />+ AUI</TableHeaderColumn>
                        <TableHeaderColumn dataField="precio_ajustado"  tdStyle={{ background: '#99bbff' }} width={'9%'} editable={true} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true} dataAlign='right'>P.U.<br />Ajustado</TableHeaderColumn>
                        <TableHeaderColumn dataField="total_pu"  width={'9%'} style={{ float: 'right' }} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true} dataAlign='right'>Total.<br />P.U.</TableHeaderColumn>
                        <TableHeaderColumn dataField="total_pu_aui"  width={'9%'} editable={false} dataAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true} dataAlign='right'>Total.<br />P.U + AUI</TableHeaderColumn>
                        <TableHeaderColumn dataField="diferente" dataFormat={activeFormatter} width={'6%'} editable={false} dataAlign="center" dataSort={true}>DIF</TableHeaderColumn>
                    </BootstrapTable>
                </BlockUi>
            </div>
        )
    }

    getSelectedRowKeys() {
        console.log(this.refs.table.state.selectedRowKeys)
    }

    updateTableKey() {
        this.setState({ key_table: Math.random() })
    }

    onBeforeSaveCell(row, cellName, cellValue) {
        this.updateTableKey()
        if (isNaN(cellValue) || cellValue < 0) {
            this.props.showWarn("El valor debe ser númerico o mayor a Cero.")
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

    agregarItemsComputo() {
        this.props.SetSession()
        return (
            window.location.href = "/Proyecto/ComputosTemporal/AgregarItems/"+this.props.id_oferta
        );
    }

    emitirPresupuesto(){
        return (
            axios.post("/Proyecto/ComputosTemporal/EmitirPresupuesto/",
                {
                    idOferta: this.props.id_oferta
                })
                .then((response) => {
                    this.props.showSuccess("Presupuesto Emitido.")
                })
                .catch((error) => {
                    console.log(error);
                    this.props.showWarn("Algo salio mal.")
                })
        );
    }

    actualizarEAC() {
        if (this.refs.table.state.selectedRowKeys.length != 0) {
            if (this.props.computosTemporal.length == 0) {
                this.props.showWarn("No se han cargado Computos.")
                this.setState({ blocking: false })
            } else {
                return (
                    axios.post("/Proyecto/ComputosTemporal/ActualizarValorEacAsync/",
                        {
                            idOferta: this.props.id_oferta, computos: this.refs.table.state.selectedRowKeys,
                            lista: this.state.computosTemporal
                        })
                        .then((response) => {
                            console.log(response.data)
                            var i = this.refs.table.state.selectedRowKeys.length
                            this.refs.table.setState({selectedRowKeys: []})
                            this.setState({ bandera: true, computosTemporal: response.data, blocking: false, key_table: Math.random() },
                                () => this.props.showSuccess("Se han actualizado "+i+" registros."))
                        })
                        .catch((error) => {
                            this.setState({ blocking: false })
                            console.log(error);
                            this.props.showWarn("Algo salio mal.")
                        })
                );
            }
        } else {
            this.props.showWarn("No se han seleccionado computos.")
            this.setState({ blocking: false })
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