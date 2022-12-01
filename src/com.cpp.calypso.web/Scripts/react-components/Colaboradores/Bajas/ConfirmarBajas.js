import React from 'react';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import moment from 'moment';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class ConfirmarBajas extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            bajasEnvio: [],
            filas: [],
            loading: false,
        }

        this.GetColaboradoresEnvioSap = this.GetColaboradoresEnvioSap.bind(this);
        this.DatosEnvio = this.DatosEnvio.bind(this);
        this.onSelectAll = this.onSelectAll.bind(this);
        this.EnvioManual = this.EnvioManual.bind(this);
    }

    componentDidMount() {
    }


    render() {
        const options = {
            withoutNoDataText: true
        };
        const selectRowProp = {
            mode: 'checkbox',
            clickToSelect: true,
            onSelect: this.DatosEnvio,
            onSelectAll: this.onSelectAll
        };
        return (
            <div>
                <BlockUi tag="div" blocking={this.state.loading}>
                    <BootstrapTable data={this.state.bajasEnvio} hover={true} pagination={true} options={options} selectRow={selectRowProp} containerStyle={{ width: '100%', overflowX: 'scroll' }}>
                        <TableHeaderColumn width={'6%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn>
                        <TableHeaderColumn width={'8%'} dataField="id_sap" dataFormat={this.IDSAP.bind(this)} dataAlign="center" thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>ID SAP</TableHeaderColumn>
                        <TableHeaderColumn dataField="nombre_identificacion" dataFormat={this.formatTipoIdentificacion.bind(this)} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Tipo Identificación</TableHeaderColumn>
                        <TableHeaderColumn dataField="numero_identificacion" dataFormat={this.formatNumeroIdentificacion.bind(this)} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>No. Identificación</TableHeaderColumn>
                        <TableHeaderColumn dataField='apellidos_nombres' dataFormat={this.formatNombres.bind(this)} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Apellidos Nombres</TableHeaderColumn>
                        <TableHeaderColumn dataField='encargado_personal' dataFormat={this.formatEncargadoPersonal.bind(this)} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Encargado de Personal</TableHeaderColumn>
                        <TableHeaderColumn dataField='motivo_baja' headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} dataSort={true}>Motivo Baja</TableHeaderColumn>
                        <TableHeaderColumn width={'9%'} /*dataField='fecha_baja'*/ dataFormat={this.formatFechaBaja.bind(this)} thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Fecha Baja</TableHeaderColumn>
                        <TableHeaderColumn dataField='estado_baja' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" filter={{ type: 'TextFilter', delay: 500 }} dataSort={true}>Estado Baja</TableHeaderColumn>
                    </BootstrapTable>

                    <div className="row">
                        <div className="form-group col">
                            <button type="button" onClick={() => this.EnvioManual()} className="btn btn-outline-primary">Guardar</button>
                            <button onClick={() => this.props.onHide()} type="button" className="btn btn-outline-primary" style={{ marginLeft: '3px' }}>Cancelar</button>
                        </div>
                    </div>
                </BlockUi>
            </div>
        )
    }

    EnvioManual() {
        this.setState({ loading: true });
        console.log(this.state.filas)
        axios.post("/RRHH/ColaboradorBaja/EditEstadoBaja/", { ids: this.state.filas })
            .then((response) => {
                this.setState({ loading: false });
                if (response.data == "OK") {
                    abp.notify.success("Bajas Actualizadas", "Aviso");
                    this.props.GetColaboradores();
                    this.props.onHide();
                    this.setState({ filas: [] })
                } else {
                    abp.notify.error("Algo salió mal", 'Error');
                }

            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false });
            });
    }

    GetColaboradoresEnvioSap() {
        this.setState({ loading: true })
        axios.post("/RRHH/ColaboradorBaja/GetBajasEnviadoSapApi/", {})
            .then((response) => {
                console.log(response.data)
                this.setState({ bajasEnvio: response.data, loading: false })
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
            });
    }

    DatosEnvio(row, isSelected, e) {
        var select = this.state.filas.slice();
        console.log('row', row)
        if (isSelected == true) {
            select.push(row.Id);
        } else {
            var i = select.findIndex(c => c == row.Id)
            if (i > -1) {
                select.splice(i, 1)
            }
        }
        console.log(select)
        this.setState({
            filas: select
        });
    }

    onSelectAll(isSelected, rows) {
        var select = [];
        if (isSelected == true) {
            rows.forEach(e => {
                select.push(e.Id)
            });
            this.setState({
                filas: select
            });
        } else {
            this.setState({
                filas: select
            });
        }
        console.log(select)
    }

    IDSAP(cell, row) {
        return row.Colaboradores.empleado_id_sap;
    }

    formatTipoIdentificacion(cell, row) {
        return row.Colaboradores.TipoIdentificacion.nombre;
    }

    formatNumeroIdentificacion(cell, row) {
        return row.Colaboradores.numero_identificacion;
    }

    formatNombres(cell, row) {
        return row.Colaboradores.nombres_apellidos;
    }

    formatEncargadoPersonal(cell, row) {
        return row.Colaboradores.EncargadoPersonal.nombre;
    }

    formatFechaBaja(cell, row) {
        return moment(row.fecha_baja).format("DD-MM-YYYY");
    }

}