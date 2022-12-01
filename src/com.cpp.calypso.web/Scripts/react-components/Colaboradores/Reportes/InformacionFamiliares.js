import React from 'react';
import axios from 'axios';

export default class InformacionFamiliares extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            tipo_identificacion: '',
            numero_identificacion: '',
            nombres_apellidos: '',
            id_sap: '',
            estado: '',
            encargado_personal: '',
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
                </div>
                <div className="row">
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
                    <div className="col">
                        <div className="form-group">
                            <label htmlFor="encargado_personal">Encargado de Personal: </label>
                            <select value={this.state.encargado_personal} onChange={this.handleChange} className="form-control" name="encargado_personal">
                                <option value="">Seleccione...</option>
                                {this.props.getFormSelectEncargadoPersonal()}
                            </select>
                        </div>
                    </div>
                </div>
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


        axios.get("/RRHH/Colaboradores/GetReporteCargasFamiliaresApi/", {
            params: {
                tipo_identificacion: this.state.tipo_identificacion,
                numero_identificacion: this.state.numero_identificacion,
                nombres_apellidos: this.state.nombres_apellidos,
                id_sap: this.state.id_sap,
                estado: this.state.estado,
                encargado_personal: this.state.encargado_personal
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
                abp.notify.error('Existe un inconveniente inténtelo más tarde!', 'Error');
            });
    }


    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeUpperCase(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }

}