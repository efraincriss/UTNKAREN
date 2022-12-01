import React from 'react';
import axios from 'axios';

export default class InformacionAusentismos extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            tipo_identificacion: '',
            numero_identificacion: '',
            nombres_apellidos: '',
            id_sap: '',
            estado: '',
            grupo_personal: '',
            encargado_personal: '',
            tipo_ausentismo: '',
            fecha_inicio_desde: '',
            fecha_inicio_hasta: '',
            fecha_fin_desde: '',
            fecha_fin_hasta: '',
        }

        this.handleChange = this.handleChange.bind(this);
        this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);

        this.GenerarReporte = this.GenerarReporte.bind(this);

    }

    componentDidMount() {

    }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="label">Tipo de Identificación: </label>
                            <select value={this.state.tipo_identificacion} onChange={this.handleChange} className="form-control" name="tipo_identificacion">
                                <option value="">Seleccione...</option>
                                {this.props.getFormSelectIdentificaciones()}
                            </select>
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="numero_identificacion">No. de Identificación: </label>
                            <input type="text" id="numero_identificacion" className="form-control" value={this.state.numero_identificacion} onChange={this.handleChangeUpperCase} name="numero_identificacion" />
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="nombres_apellidos">Apellidos Nombres: </label>
                            <input type="text" id="nombres_apellidos" className="form-control" value={this.state.nombres_apellidos} onChange={this.handleChangeUpperCase} name="nombres_apellidos" />
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="id_sap">ID SAP: </label>
                            <input type="text" id="id_sap" className="form-control" value={this.state.id_sap} onChange={this.handleChangeUpperCase} name="id_sap" />
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="estado">Estado: </label>
                            <select value={this.state.estado} onChange={this.handleChange} className="form-control" name="estado">
                                <option value="">Seleccione...</option>
                                {this.props.getFormSelectEstados()}
                            </select>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="grupo_personal">Agrupación para Requisitos: </label>
                            <select value={this.state.grupo_personal} onChange={this.handleChange} className="form-control" name="grupo_personal">
                                <option value="">Seleccione...</option>
                                {this.props.getFormSelectGrupoPersonal()}
                            </select>
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="encargado_personal">Encargado de Personal: </label>
                            <select value={this.state.encargado_personal} onChange={this.handleChange} className="form-control" name="encargado_personal">
                                <option value="">Seleccione...</option>
                                {this.props.getFormSelectEncargadoPersonal()}
                            </select>
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="tipo_ausentismo">Tipo Ausentismo: </label>
                            <select value={this.state.tipo_ausentismo} onChange={this.handleChange} className="form-control" name="tipo_ausentismo">
                                <option value="">Seleccione...</option>
                                {this.props.getFormSelectAusentismos()}
                            </select>
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="fecha_inicio_desde">Fecha Inicio Desde: </label>
                            <input type="date" id="fecha_inicio_desde" className="form-control" value={this.state.fecha_inicio_desde} onChange={this.handleChange} name="fecha_inicio_desde" />
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="fecha_inicio_hasta">Fecha Inicio Hasta: </label>
                            <input type="date" id="fecha_inicio_hasta" className="form-control" value={this.state.fecha_inicio_hasta} onChange={this.handleChange} name="fecha_inicio_hasta" />
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="fecha_fin_desde">Fecha Fin Desde: </label>
                            <input type="date" id="fecha_fin_desde" className="form-control" value={this.state.fecha_fin_desde} onChange={this.handleChange} name="fecha_fin_desde" />
                        </div>
                    </div>
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="fecha_fin_hasta">Fecha Fin Hasta: </label>
                            <input type="date" id="fecha_fin_hasta" className="form-control" value={this.state.fecha_fin_hasta} onChange={this.handleChange} name="fecha_fin_hasta" />
                        </div>
                    </div>
                    <div className="col">
                    </div>
                    <div className="col"></div>
                    <div className="col"></div>
                </div>
                <br />
                <div className="row">
                    <div className="col">
                        <div className="form-group">
                            <button type="button" onClick={() => this.GenerarReporte()} className="btn btn-outline-primary"> Generar Reporte</button>
                            <button onClick={() => this.props.onHide()} type="button" className="btn btn-outline-primary" style={{ marginLeft: '3px' }}> Cancelar</button>
                        </div>
                    </div>
                </div>
            </div >
        )
    }



    GenerarReporte() {
        this.props.onLoad();
        axios.get("/RRHH/ColaboradoresAusentismo/GetReporteAusentismosApi/", {
            params: {
                tipo_identificacion: this.state.tipo_identificacion,
                numero_identificacion: this.state.numero_identificacion,
                nombres_apellidos: this.state.nombres_apellidos,
                id_sap: this.state.id_sap,
                estado: this.state.estado,
                grupo_personal: this.state.grupo_personal,
                encargado_personal: this.state.encargado_personal,
                tipo_ausentismo: this.state.tipo_ausentismo,
                fecha_inicio_desde: this.state.fecha_inicio_desde,
                fecha_inicio_hasta: this.state.fecha_inicio_hasta,
                fecha_fin_desde: this.state.fecha_fin_desde,
                fecha_fin_hasta: this.state.fecha_fin_hasta,
            },
            responseType: 'arraybuffer',
        })
            .then((response) => {
                this.props.offLoad();
                console.log(response.data, response.data.byteLength)
                if (response.data.byteLength > 0) {
                    var nombre = response.headers["content-disposition"].split('=');

                    const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
                    const link = document.createElement('a');
                    link.href = url;
                    link.setAttribute('download', nombre[1]);
                    document.body.appendChild(link);
                    link.click();
                    abp.notify.success("Reporte generado!", "Aviso");
                    this.props.onHide();
                }else{
                    abp.notify.error('No existen registros!', 'Error');
                }
            })
            .catch((error) => {
                this.props.offLoad();
                console.log(error);
                this.setState({ loading: false });
                abp.notify.error('Algo salió mal', 'Error');
            });
    }


    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeUpperCase(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }

}