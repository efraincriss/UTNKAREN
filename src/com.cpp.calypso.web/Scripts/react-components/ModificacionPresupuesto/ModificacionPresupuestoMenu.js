import React from 'react';
import axios from 'axios';
import ModificacionPresupuestoTable from './ModificacionPresupuestoTable';
import ModificacionPresupuestoTableA from './ModificacionPresupuestoTableA';

import BlockUi from 'react-block-ui';

export default class ModificacionPresupuestoMenu extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            clase: '',
            monto_total: '',
            monto_construccion: '',
            monto_ingenieria: '',
            monto_procura: '',
            id_requerimiento: 0,
            id_contrato: 0,
            id_proyecto: 0,
            id_oferta: 0,
            proyectos: [],
            computos: [],
            requerimientos: [],
            computosTemporal: [],
            ofertas: [],
            blocking: false,
            versionPresupuesto: false
        }
        this.handleContratoChange = this.handleContratoChange.bind(this);
        this.handleProyectoChange = this.handleProyectoChange.bind(this);
        this.handleRequerimientoChange = this.handleRequerimientoChange.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.getRequeriFormSelect = this.getRequeriFormSelect.bind(this);
        this.getContratoFormSelect = this.getContratoFormSelect.bind(this);
        this.changeProyectos = this.changeProyectos.bind(this);
        this.changeContratos = this.changeContratos.bind(this);
        this.changeOferta = this.changeOferta.bind(this);
        this.GetOfertas = this.GetOfertas.bind(this);
        this.GetProyectos = this.GetProyectos.bind(this);
        this.GetComputos = this.GetComputos.bind(this);
        this.NuevaVersion = this.NuevaVersion.bind(this);
        this.GetNuevaVersion = this.GetNuevaVersion.bind(this);
        this.UpdateData = this.UpdateData.bind(this);
        this.GetSession = this.GetSession.bind(this);
        this.GetRequerimientos = this.GetRequerimientos.bind(this);
        this.SetSession = this.SetSession.bind(this);
        this.UpdateCombos = this.UpdateCombos.bind(this);
    }

    componentDidMount() {
        if (document.getElementById('estado').className == "true") {
            this.setState({
                id_contrato: sessionStorage.getItem('id_contrato'),
                id_proyecto: sessionStorage.getItem('id_proyecto'),
                id_requerimiento: sessionStorage.getItem('id_requerimiento'),
                id_oferta: sessionStorage.getItem('id_oferta')
            }, this.UpdateCombos)
            this.GetSession();
        }
        //sessionStorage.removeItem('id_empresa')
    }

    render() {
        return (
            <div>
                <BlockUi tag="div" blocking={this.state.blocking}>
                    <div className="row">
                        <div className="col-sm-9"></div>
                        <div className="col-sm-3">
                            <button className="btn btn-primary" disabled={this.state.computosTemporal.length != 0 ? true : false} onClick={this.NuevaVersion} style={{ float: 'right', marginRight: '0.3em' }}> Nueva versión Presupuesto </button>
                        </div>
                    </div>
                    <hr />
                    <div className="row">
                        <div className="col-sm-2">
                        </div>
                        <div className="col-sm-3">
                            <label htmlFor="label">Contrato</label>
                            <select value={this.state.id_contrato} required onChange={this.handleContratoChange} onClick={this.changeContratos} className="form-control" name="id_contrato">
                                <option value="">--- Selecciona un Contrato ---</option>
                                {this.getContratoFormSelect()}
                            </select>
                        </div>
                        <div className="col-sm-2">
                        </div>
                        <div className="col-sm-3">
                            <label htmlFor="label">Proyecto</label>
                            <select value={this.state.id_proyecto} required onChange={this.handleProyectoChange} onClick={this.changeProyectos} className="form-control" name="id_proyecto">
                                <option value="">--- Selecciona un Proyecto ---</option>
                                {this.getProyectoFormSelect()}
                            </select>
                        </div>
                        <div className="col-sm-2">
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-sm-2">
                        </div>
                        <div className="col-sm-3">
                            <label htmlFor="label">Requerimiento</label>
                            <select value={this.state.id_requerimiento} required onChange={this.handleRequerimientoChange} onClick={this.changeRequerimientos} className="form-control" name="id_requerimiento">
                                <option value="">--- Selecciona un Requerimiento ---</option>
                                {this.getRequeriFormSelect()}
                            </select>
                        </div>
                        <div className="col-sm-2">
                        </div>
                        <div className="col-sm-3">
                            <label htmlFor="label">Oferta</label>
                            <select value={this.state.id_oferta} required onChange={this.handleChange} onClick={this.changeOferta} className="form-control" name="id_oferta">
                                <option value="">--- Selecciona una Oferta ---</option>
                                {this.getOfertaFormSelect()}
                            </select>
                        </div>
                        <div className="col-sm-2">
                        </div>
                    </div>

                    <hr />
                    <div className="row">
                        <div className="col">
                            <div>
                                <ul className="nav nav-tabs" id="empresa_tabs" role="tablist">
                                    <li className="nav-item">
                                        <a className="nav-link active" id="actual-tab" data-toggle="tab" href="#actual" role="tab" aria-controls="home" aria-expanded="true">Presupuesto Actual</a>
                                    </li>
                                    <li className="nav-item">
                                        <a className="nav-link" id="actualizado-tab" data-toggle="tab" href="#actualizado" role="tab" aria-controls="home" aria-expanded="true">Presupuesto Actualizado</a>
                                    </li>
                                </ul>

                                <div className="tab-content" id="myTabContent">
                                    <div className="tab-pane fade show active" id="actual" role="tabpanel" aria-labelledby="actual-tab">
                                        <ModificacionPresupuestoTableA
                                            id_proyecto={this.state.id_proyecto}
                                            showSuccess={this.props.showSuccess}
                                            showWarn={this.props.showWarn}
                                            blocking={this.state.blocking}
                                            computos={this.state.computos}
                                            clase={this.state.clase}
                                            monto_total={this.state.monto_total}
                                            monto_construccion={this.state.monto_construccion}
                                            monto_ingenieria={this.state.monto_ingenieria}
                                            monto_procura={this.state.monto_procura}
                                        />
                                    </div>
                                    <div className="tab-pane fade" id="actualizado" role="tabpanel" aria-labelledby="actualizado-tab">
                                        <ModificacionPresupuestoTable
                                            SetSession={this.SetSession}
                                            id_oferta={this.state.id_oferta}
                                            showSuccess={this.props.showSuccess}
                                            showWarn={this.props.showWarn}
                                            blocking={this.state.blocking}
                                            computos={this.state.computos}
                                            clase={this.state.clase}
                                            monto_total={this.state.monto_total}
                                            monto_construccion={this.state.monto_construccion}
                                            monto_ingenieria={this.state.monto_ingenieria}
                                            monto_procura={this.state.monto_procura}
                                            computosTemporal={this.state.computosTemporal}
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </BlockUi>
            </div>
        );
    }

    handleRequerimientoChange(event) {
        this.setState({ [event.target.name]: event.target.value, blocking: true }, this.GetOfertas)
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value, blocking: true }, this.GetComputos);
    }

    handleContratoChange(event) {
        this.setState({ [event.target.name]: event.target.value, blocking: true }, this.GetProyectos)
    }

    handleProyectoChange(event) {
        this.setState({ [event.target.name]: event.target.value, blocking: true }, this.GetRequerimientos)
    }

    NuevaVersion(event) {
        this.setState({ blocking: true }, this.GetNuevaVersion)
    }

    getRequeriFormSelect() {
        return (
            this.state.requerimientos.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.codigo}</option>
                )
            })

        );
    }

    getContratoFormSelect() {
        return (
            this.props.contratos.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.Codigo}</option>
                )
            })
        );
    }

    getProyectoFormSelect() {
        return (
            this.state.proyectos.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.codigo} - {item.descripcion_proyecto}</option>
                )
            })
        );
    }

    getOfertaFormSelect() {
        return (
            this.state.ofertas.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.codigo} - {item.version} - {item.descripcion}</option>
                )
            })
        );
    }

    UpdateCombos() {
        axios.post("/Proyecto/Proyecto/GetProyectosporContratoApi/" + this.state.id_contrato, {})
            .then((response) => {
                this.setState({ proyectos: response.data, blocking: false })
            })
            .catch((error) => {
                this.setState({ blocking: false })
                console.log(error);
            });
        axios.post("/Proyecto/Requerimiento/GetReqPorProyecto/" + this.state.id_proyecto, )
            .then((response) => {
                this.setState({ requerimientos: response.data, blocking: false })
            })
            .catch((error) => {
                this.setState({ blocking: false })
                console.log(error);
            });
        axios.post("/Proyecto/Oferta/GetOfertasPorReqDefinitivasApi/" + this.state.id_requerimiento, {})
            .then((response) => {
                this.setState({ ofertas: response.data, blocking: false })
            })
            .catch((error) => {
                this.setState({ blocking: false })
                console.log(error);
            });
    }

    GetRequerimientos() {
        this.setState({ id_oferta: 0 })
        if (this.state.id_proyecto > 0) {
            axios.post("/Proyecto/Requerimiento/GetReqPorProyecto/" + this.state.id_proyecto, )
                .then((response) => {
                    this.setState({ requerimientos: response.data, blocking: false })
                })
                .catch((error) => {
                    this.setState({ blocking: false })
                    console.log(error);
                });
        } else {
            this.setState({ blocking: false })
        }
    }

    GetProyectos() {
        this.setState({ id_proyecto: 0 })
        var id = this.state.id_contrato
        axios.post("/Proyecto/Proyecto/GetProyectosporContratoApi/" + id, {})
            .then((response) => {
                this.setState({ proyectos: response.data, blocking: false })
            })
            .catch((error) => {
                this.setState({ blocking: false })
                console.log(error);
            });
    }

    GetOfertas() {
        if (this.state.id_requerimiento == 0) {

        } else {
            this.setState({ id_oferta: 0 })
            axios.post("/Proyecto/Oferta/GetOfertasPorReqDefinitivasApi/" + this.state.id_requerimiento, {})
                .then((response) => {
                    this.setState({ ofertas: response.data, blocking: false })
                })
                .catch((error) => {
                    this.setState({ blocking: false })
                    console.log(error);
                });
        }
    }

    GetComputos() {
        this.UpdateData();
        if (this.state.id_oferta == 0) {
            this.setState({
                computos: [], clase: '', monto_total: '', monto_construccion: '',
                monto_ingenieria: '', monto_procura: '', blocking: false
            })
        } else {
            axios.post("/Proyecto/Computo/GetComputosPorProyectoApi/" + this.state.id_oferta, {})
                .then((response) => {
                    this.setState({ computos: response.data })
                })
                .catch((error) => {
                    this.setState({ blocking: false })
                    console.log(error);
                });
            axios.post("/Proyecto/Computo/GetCabeceraApi/" + this.state.id_oferta, {})
                .then((response) => {
                    this.setState({
                        clase: response.data.clase,
                        monto_total: response.data.monto_total,
                        monto_construccion: response.data.monto_construccion,
                        monto_ingenieria: response.data.monto_ingenieria,
                        monto_procura: response.data.monto_procura,
                        blocking: false
                    })
                })
                .catch((error) => {
                    this.setState({ blocking: false })
                    console.log(error);
                });
        }
    }

    GetSession() {
        this.setState({
            blocking: false,
            id_contrato: sessionStorage.getItem('id_contrato'),
            id_proyecto: sessionStorage.getItem('id_proyecto'),
            id_requerimiento: sessionStorage.getItem('id_requerimiento'),
            id_oferta: sessionStorage.getItem('id_oferta')
        }, this.UpdateData)
    }

    SetSession() {
        sessionStorage.setItem('id_contrato', this.state.id_contrato);
        sessionStorage.setItem('id_proyecto', this.state.id_proyecto);
        sessionStorage.setItem('id_requerimiento', this.state.id_requerimiento);
        sessionStorage.setItem('id_oferta', this.state.id_oferta);
    }

    UpdateData() {
        if (this.state.id_oferta == 0) {
        } else {
            axios.post("/Proyecto/ComputosTemporal/GetComputosTemporal/" + this.state.id_oferta, {})
                .then((response) => {
                    this.setState({ computosTemporal: response.data })
                })
                .catch((error) => {
                    console.log(error);
                });
            axios.post("/Proyecto/Computo/GetComputosPorProyectoApi/" + this.state.id_oferta, {})
                .then((response) => {
                    this.setState({ computos: response.data })
                })
                .catch((error) => {
                    console.log(error);
                });
            axios.post("/Proyecto/Computo/GetCabeceraApi/" + this.state.id_oferta, {})
                .then((response) => {
                    this.setState({
                        clase: response.data.clase,
                        monto_total: response.data.monto_total,
                        monto_construccion: response.data.monto_construccion,
                        monto_ingenieria: response.data.monto_ingenieria,
                        monto_procura: response.data.monto_procura,
                        blocking: false
                    })
                })
                .catch((error) => {
                    this.setState({ blocking: false })
                    console.log(error);
                });
        }
    }

    changeContratos() {
        if (this.state.id_cliente == 0 || this.state.id_empresa == 0) {
            this.props.showWarn("Se debe Seleccionar un Cliente y Empresa previamente.")
        }
    }

    changeProyectos() {
        if (this.state.id_contrato == 0) {
            this.props.showWarn("Se debe Seleccionar un Contrato previamente.")
        }
    }

    changeOferta() {
        if (this.state.id_proyecto == 0) {
            this.props.showWarn("Se debe Seleccionar un Proyecto previamente.")
        }
    }

    GetNuevaVersion() {
        axios.post("/Proyecto/ComputosTemporal/GenerarNuevaVersion/" + this.state.id_oferta, {})
            .then((response) => {

                axios.post("/Proyecto/ComputosTemporal/GetComputosTemporal/" + this.state.id_oferta, {})
                    .then((response) => {
                        this.setState({ computosTemporal: response.data, blocking: false })
                        this.props.showSuccess("Nueva versión generada.")
                    })
                    .catch((error) => {
                        this.setState({ blocking: false })
                        console.log(error);
                        this.props.showWarn("No se pudo crear la nueva versión.")
                    });

            })
            .catch((error) => {
                this.setState({ blocking: false })
                console.log(error);
                this.props.showWarn("No se pudo crear la nueva versión.")
            });
    }

}