import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';

export default class CrearCodigoQR extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            id_colaborador: '',
            nro_identificacion: '',
            tipo_identificacion: '',
            nombre: '',
            permiteValidacion: false,
            codigo_qr: '',
            display: 'none',
            colaborador: []
        }

        this.ConsultaColaborador = this.ConsultaColaborador.bind(this);
        this.CrearQR = this.CrearQR.bind(this);
        this.DescargarQR = this.DescargarQR.bind(this); 
        this.Regresar = this.Regresar.bind(this);
        this.handleCheck = this.handleCheck.bind(this);

        /* Mensajes al usuario */ 
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
    }


    componentDidMount() {
        this.ConsultaColaborador();
    }

    render() {
        return (
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <form onSubmit={this.handleSubmit}>
                    <div className="row">
                        <div className="col-xs-12 col-md-12">
                            <div className="row" >
                            <div className="col-sm-2"></div>
                                <div className="col-sm-4">
                                    <div className="form-group">
                                        <label htmlFor="text"><b>Tipo de Identificación:</b> {this.state.colaborador.nombre_identificacion} </label>
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="text"><b>Colaborador:</b> {this.state.nombres} </label>
                                    </div>
                                </div>
                                <div className="col-sm-4">
                                    <div className="form-group">
                                        <label htmlFor="text"><b>No. de Identificación:</b> {this.state.colaborador.numero_identificacion} </label>
                                    </div>
                                    <div className="form-group" >
                                        <label htmlFor="text"><b>Permite validación por Cédula:</b> 
                                        <input type="checkbox" style={{marginLeft : '15px'}} onChange={this.handleCheck} ref='validacion' defaultChecked={this.state.permiteValidacion} name="validacion"/>
                                        </label>
                                    </div>
                                </div>
                                <div className="col-sm-2"></div>
                            </div>
                            <div className="row" >
                                <div className="col-md-3"></div>
                                <div className="col-md-5">
                                    <div className="form-group" style={{marginLeft : '42px'}} >
                                    <img id="FPImage1" style={{display : this.state.display}} height="260" width="280"/>
                                    </div>
                                </div>
                                <div className="col-md-2"></div>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-2"></div>
                        <div className="col-xs-12 col-md-12">
                        <div className="row" >
                                <div className="col-md-3"></div>
                                <div className="col-md-6">
                                    <div className="form-group">
                                    <button onClick={() => this.Guardar()} type="button" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                        <button onClick={() => this.CrearQR()} type="button" className="btn btn-outline-primary" style={{ marginLeft: '3px' }}> Generar QR</button>
                                        <button onClick={() => this.DescargarQR()} type="button" className="btn btn-outline-primary" style={{ marginLeft: '3px' }}> Imprimir QR</button>
                                        <button onClick={() => this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                                    </div>
                                </div>
                                <div className="col-md-2"></div>
                            </div>
                            <br />
                        </div>
                    </div>
                </form>
            </div>
            
        )
    }

    ConsultaColaborador() {
        axios.post("/RRHH/Colaboradores/GetColaboradorApi/", { id: sessionStorage.getItem('id_colaborador') })
            .then((response) => {
                this.setState({
                    colaborador: response.data,
                    id_colaborador: sessionStorage.getItem('id_colaborador'),
                    permiteValidacion: response.data.validacion_cedula,
                    nombres:  response.data.nombres_apellidos
                })
                /* Check */
                this.refs.validacion.checked = response.data.validacion_cedula;
            })
            .catch((error) => {
                console.log(error);
            });
    }

    Guardar() {
        if(this.state.codigo_qr)
        {
            axios.post("/RRHH/Colaboradores/UpdateColaboradorQR/", 
            { 
                id: sessionStorage.getItem('id_colaborador'),
                validacion: this.state.permiteValidacion
            })
            .then((response) => {
                this.showSuccess("Colaborador guardado con exito!");
                this.Regresar();
            })
            .catch((error) => {
                console.log(error);
                this.warnMessage("Algo salio mal!");
            });

        }else{
            this.warnMessage("Se debe generar el QR antes de guardar!");
        }
    }

    CrearQR() {

        axios.post("/RRHH/Colaboradores/CreateQR/", { id: sessionStorage.getItem('id_colaborador') })
            .then((response) => {
                console.log(response.data);
                this.setState({
                    codigo_qr: response.data,
                    display: 'block'
                });

                /* Se carga la imagen */ 
                document.getElementById("FPImage1").src = "data:image/jpg;base64," + response.data;
            })
            .catch((error) => {
                console.log(error);
            });
    }

    DescargarQR() {

        if(this.state.codigo_qr)
        {
            var element = document.createElement('a');
            element.setAttribute('href', document.getElementById("FPImage1").src);
            element.setAttribute('download', "qr.jpg");
            
            element.style.display = 'none';
            document.body.appendChild(element);
            
            element.click();
            
            document.body.removeChild(element);
        }else{
            this.warnMessage("Se debe generar el QR!");
        }
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/Colaboradores/Servicios/"
        );
    }

    handleCheck() {
        this.setState({permiteValidacion: !this.state.permiteValidacion});
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }

}

ReactDOM.render(
    <CrearCodigoQR />,
    document.getElementById('content-crear-qr')
);