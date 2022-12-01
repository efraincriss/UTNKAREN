import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Dialog } from 'primereact/components/dialog/Dialog';

import InformacionColaborador from './InformacionColaborador';
import InformacionFamiliares from './InformacionFamiliares';
import InformacionAusentismos from './InformacionAusentismos';
import { InformacionCapacitaciones } from './InformacionCapacitaciones.jsx';

export default class ReportesContainer extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            tiposAusentismos: [],
            tiposEncargadoPersonal: [],
            tiposEstados: [],
            tiposGrupoPersonal: [],
            tiposIdentificaciones: [],
            tiposMotivoBaja: [],
            reportes: [
                { nombre: 'Información General de Colaboradores', codigo: 'COL' },
                { nombre: 'Información de Cargas y Familiares', codigo: 'FAM' },
                { nombre: 'Información de Ausentismos', codigo: 'AUS' },
                { nombre: 'Información de Capacitaciones', codigo: 'CAP' },
            ],
            visible_colaboradores: false,
            visible_familiares: false,
            visible_ausentismos: false,
            loading: false,
        }

        this.mostrarDialog = this.mostrarDialog.bind(this);

        this.showFormColaboradores = this.showFormColaboradores.bind(this);
        this.onHideColaboradores = this.onHideColaboradores.bind(this);
        this.showFormAusentismos = this.showFormAusentismos.bind(this);
        this.onHideAusentismos = this.onHideAusentismos.bind(this);
        this.showFormFamiliares = this.showFormFamiliares.bind(this);
        this.onHideFamiliares = this.onHideFamiliares.bind(this);
        this.onLoad = this.onLoad.bind(this);
        this.offLoad = this.offLoad.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.getFormSelectAusentismos = this.getFormSelectAusentismos.bind(this);
        this.getFormSelectEncargadoPersonal = this.getFormSelectEncargadoPersonal.bind(this);
        this.getFormSelectEstados = this.getFormSelectEstados.bind(this);
        this.getFormSelectGrupoPersonal = this.getFormSelectGrupoPersonal.bind(this);
        this.getFormSelectIdentificaciones = this.getFormSelectIdentificaciones.bind(this);
        this.getFormSelectMotivoBaja = this.getFormSelectMotivoBaja.bind(this);

    }

    componentDidMount() {
        this.GetCatalogos();
    }

    render() {
        return (

            <div>
                <div className="row">
                    <div className="col-3"></div>
                    <div className="col">
                        <BootstrapTable data={this.state.reportes} hover={true}>
                            <TableHeaderColumn
                                dataField="any"
                                dataFormat={this.Secuencial}
                                width={"8%"}
                                tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            >
                                Nº
                    </TableHeaderColumn>
                            <TableHeaderColumn
                                dataField="nombre"
                                headerAlign="center"
                                isKey={true}
                            >
                                Nombre Reporte
                    </TableHeaderColumn>
                            <TableHeaderColumn
                                dataField='Operaciones'
                                headerAlign="center"
                                dataAlign="center"
                                dataFormat={this.generateButton.bind(this)}
                                width={"25%"}
                            >
                                Opciones
                    </TableHeaderColumn>
                        </BootstrapTable>
                    </div>
                    <div className="col-3"></div>
                </div>

                <Dialog header="Reporte de Información General del Colaborador" visible={this.state.visible_colaboradores} modal={true} onHide={this.onHideColaboradores}>
                    <BlockUi tag="div" blocking={this.state.loading}>
                        <InformacionColaborador
                            onHide={this.onHideColaboradores}
                            onLoad={this.onLoad}
                            offLoad={this.offLoad}
                            getFormSelectIdentificaciones={this.getFormSelectIdentificaciones}
                            getFormSelectEstados={this.getFormSelectEstados}
                            getFormSelectEncargadoPersonal={this.getFormSelectEncargadoPersonal}
                            getFormSelectGrupoPersonal={this.getFormSelectGrupoPersonal}
                            getFormSelectMotivoBaja={this.getFormSelectMotivoBaja}
                        />
                    </BlockUi>
                </Dialog>
                <Dialog header="Reporte de Información de Cargas Familiares" visible={this.state.visible_familiares} width="850px" modal={true} onHide={this.onHideFamiliares}>
                    <BlockUi tag="div" blocking={this.state.loading}>
                        <InformacionFamiliares
                            onHide={this.onHideFamiliares}
                            onLoad={this.onLoad}
                            offLoad={this.offLoad}
                            getFormSelectIdentificaciones={this.getFormSelectIdentificaciones}
                            getFormSelectEstados={this.getFormSelectEstados}
                            getFormSelectEncargadoPersonal={this.getFormSelectEncargadoPersonal}
                        />
                    </BlockUi>
                </Dialog>
                <Dialog header="Reporte de Información de Ausentismos del Colaborador" visible={this.state.visible_ausentismos} width="850px" modal={true} onHide={this.onHideAusentismos}>
                    <BlockUi tag="div" blocking={this.state.loading}>
                        <InformacionAusentismos
                            onHide={this.onHideAusentismos}
                            onLoad={this.onLoad}
                            offLoad={this.offLoad}
                            getFormSelectIdentificaciones={this.getFormSelectIdentificaciones}
                            getFormSelectEstados={this.getFormSelectEstados}
                            getFormSelectEncargadoPersonal={this.getFormSelectEncargadoPersonal}
                            getFormSelectGrupoPersonal={this.getFormSelectGrupoPersonal}
                            getFormSelectAusentismos={this.getFormSelectAusentismos}
                        />
                    </BlockUi>
                </Dialog>
                <Dialog header="Reporte de Capacitacion De Colaboradores" visible={this.state.visible_capacitaciones} width="750px" modal={true} onHide={this.hideFormularioCapacitaciones}>
                    <BlockUi tag="div" blocking={this.state.loading}>
                        <InformacionCapacitaciones
                            unlockScreen={this.offLoad}
                            blockScreen={this.onLoad}
                        />
                    </BlockUi>
                </Dialog>

            </div >

        )
    }

    Secuencial(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }

    generateButton(cell, row) {
        // console.log('row', row)
        return (
            <div>
                <button onClick={() => this.mostrarDialog(row.codigo)} data-toggle="tooltip" data-placement="top" title="Generar reporte" className="btn btn-outline-primary btn-sm fa fa-download"></button>
            </div>
        )
    }

    mostrarDialog(codigo) {
        console.log('codigo', codigo)
        switch (codigo) {
            case 'COL':
                this.showFormColaboradores();
                return;
            case 'FAM':
                this.showFormFamiliares();
                return;
            case 'AUS':
                this.showFormAusentismos();
                return;
            case 'CAP':
                this.showFormularioCapacitaciones();
                return;

        }
    }

    onHideColaboradores() {
        this.setState({ visible_colaboradores: false })
    }

    showFormColaboradores() {
        this.setState({ visible_colaboradores: true })
    }

    onHideFamiliares() {
        this.setState({ visible_familiares: false })
    }

    showFormFamiliares() {
        this.setState({ visible_familiares: true })
    }

    onHideAusentismos() {
        this.setState({ visible_ausentismos: false })
    }

    showFormAusentismos() {
        this.setState({ visible_ausentismos: true })
    }

    showFormularioCapacitaciones = () => {
        this.setState({ visible_capacitaciones: true })
    }

    hideFormularioCapacitaciones = () => {
        this.setState({ visible_capacitaciones: false })
    }

    onLoad() {
        this.setState({ loading: true })
    }

    offLoad() {
        this.setState({ loading: false })
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['TIPOINDENTIFICACION', 'ESTADOSCOL', 'ENCARGADO', 'GRUPOPERSONAL',
            'MOTIVOBAJA', 'TIPOAUSENTISMO'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    cargarCatalogos(data) {
        //console.log('data',data);
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
            switch (codigoCatalogo) {
                case 'TIPOINDENTIFICACION':
                    this.setState({ tiposIdentificaciones: catalogo })
                    this.getFormSelectIdentificaciones();
                    return;
                case 'ESTADOSCOL':
                    this.setState({ tiposEstados: catalogo })
                    this.getFormSelectEstados();
                    return;
                case 'ENCARGADO':
                    this.setState({ tiposEncargadoPersonal: catalogo })
                    this.getFormSelectEncargadoPersonal();
                    return;
                case 'GRUPOPERSONAL':
                    this.setState({ tiposGrupoPersonal: catalogo })
                    this.getFormSelectGrupoPersonal();
                    return;
                case 'MOTIVOBAJA':
                    this.setState({ tiposMotivoBaja: catalogo })
                    this.getFormSelectMotivoBaja();
                    return;
                case 'TIPOAUSENTISMO':
                    this.setState({ tiposAusentismos: catalogo })
                    this.getFormSelectAusentismos();
                    return;
            }
        });
    }

    getFormSelectIdentificaciones() {
        return (
            this.state.tiposIdentificaciones.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }
    getFormSelectEstados() {
        return (
            this.state.tiposEstados.map((item) => {
                return (
                    <option key={Math.random()} value={item.nombre}>{item.nombre}</option>
                )
            })
        );
    }
    getFormSelectEncargadoPersonal() {
        return (
            this.state.tiposEncargadoPersonal.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }
    getFormSelectGrupoPersonal() {
        return (
            this.state.tiposGrupoPersonal.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }
    getFormSelectMotivoBaja() {
        return (
            this.state.tiposMotivoBaja.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }
    getFormSelectAusentismos() {
        return (
            this.state.tiposAusentismos.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }
}

ReactDOM.render(
    <ReportesContainer />,
    document.getElementById('content-reportes')
);