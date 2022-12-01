import React from 'react';
import axios from 'axios';
import SeguimientoOfertaTable from './SeguimientoOfertaTable';
import BlockUi from 'react-block-ui';

export default class SeguimientoOfertaMenu extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            id_empresa: 0,
            id_cliente: 0,
            id_contrato: 0,
            id_proyecto: 0,
            contratos: [],
            proyectos: [],
            blocking: false,
        }
        this.handleContratoChange = this.handleContratoChange.bind(this);
        this.getEmpresaFormSelect = this.getEmpresaFormSelect.bind(this);
        this.getClienteFormSelect = this.getClienteFormSelect.bind(this);
        this.getContratoFormSelect = this.getContratoFormSelect.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleProyectoChange = this.handleProyectoChange.bind(this);
        this.GetProyectos = this.GetProyectos.bind(this);
        this.GetContratos = this.GetContratos.bind(this);
        this.changeProyectos = this.changeProyectos.bind(this);
        this.changeContratos = this.changeContratos.bind(this);
    }

    render() {
        return (
            <div>
                <BlockUi tag="div" blocking={this.state.blocking}>
                    <div className="row">
                        <div className="col-sm-2">
                        </div>
                        <div className="col-sm-3">
                            <label htmlFor="label">Empresa</label>
                            <select value={this.state.id_empresa} required onChange={this.handleContratoChange} className="form-control" name="id_empresa">
                                <option value="">--- Selecciona una Empresa ---</option>
                                {this.getEmpresaFormSelect()}
                            </select>
                        </div>
                        <div className="col-sm-2">
                        </div>
                        <div className="col-sm-3">
                            <label htmlFor="label">Cliente</label>
                            <select value={this.state.id_cliente} required onChange={this.handleContratoChange} className="form-control" name="id_cliente">
                                <option value="">--- Selecciona un Cliente ---</option>
                                {this.getClienteFormSelect()}
                            </select>
                        </div>
                        <div className="col-sm-2">
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-sm-2">
                        </div>
                        <div className="col-sm-3">
                            <label htmlFor="label">Contrato</label>
                            <select value={this.state.id_contrato} required onChange={this.handleProyectoChange} onClick={this.changeContratos} className="form-control" name="id_contrato">
                                <option value="">--- Selecciona un Contrato ---</option>
                                {this.getContratoFormSelect()}
                            </select>
                        </div>
                        <div className="col-sm-2">
                        </div>
                        <div className="col-sm-3">
                            <label htmlFor="label">Proyecto</label>
                            <select value={this.state.id_proyecto} required onChange={this.handleChange} onClick={this.changeProyectos} className="form-control" name="id_proyecto">
                                <option value="">--- Selecciona un Proyecto ---</option>
                                {this.getProyectoFormSelect()}
                            </select>
                        </div>
                        <div className="col-sm-2">
                        </div>
                    </div>
                    <SeguimientoOfertaTable
                        id_proyecto={this.state.id_proyecto}
                        showSuccess={this.props.showSuccess}
                        showWarn={this.props.showWarn}
                        blocking={this.state.blocking}
                    />
                </BlockUi>
            </div>

        );
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleContratoChange(event) {
        this.setState({ [event.target.name]: event.target.value, blocking: true }, this.GetContratos)
    }

    handleProyectoChange(event) {
        this.setState({ [event.target.name]: event.target.value, blocking: true }, this.GetProyectos)
    }

    getEmpresaFormSelect() {
        return (
            this.props.empresas.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.razon_social}</option>
                )
            })

        );
    }

    getClienteFormSelect() {
        return (
            this.props.clientes.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.razon_social}</option>
                )
            })

        );
    }

    getContratoFormSelect() {
        return (
            this.state.contratos.map((item) => {
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
                    <option key={Math.random()} value={item.Id}>{item.nombre_proyecto}</option>
                )
            })
        );
    }

    GetContratos() {
        this.setState({ id_contrato: 0, id_proyecto: 0 })
        var id = this.state.id_empresa + "," + this.state.id_cliente;
        if (this.state.id_empresa > 0 && this.state.id_cliente > 0) {
            axios.post("/Proyecto/Contrato/GetContratosPorClienteEmpresaApi/", { ids: id })
                .then((response) => {
                    this.setState({ contratos: response.data, blocking: false })
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
        if (this.state.id_cliente == 0 || this.state.id_empresa == 0) {

        } else {
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

}