import React from 'react';
import ReactDOM from 'react-dom';
import Moment from 'moment';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import { Dialog } from 'primereact/components/dialog/Dialog';
import { Growl } from 'primereact/components/growl/Growl';
import { Dropdown } from 'primereact/components/dropdown/Dropdown';
import { Button } from 'primereact/components/button/Button';
import { Calendar } from 'primereact/components/calendar/Calendar';
import { InputText } from 'primereact-v2/components/inputtext/InputText';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import CurrencyFormat from 'react-currency-format';

class RegistroCivil extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            colaboradores: [],
            obtenido: [],
            cedula: '',

            huella_dactilar: ''

        }
        this.GetColaboradores = this.GetColaboradores.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.consultarWS = this.consultarWS.bind(this);
        this.consultarWSHuella = this.consultarWSHuella.bind(this);
        this.convertirimagen = this.convertirimagen.bind(this);
    }

    componentDidMount() {

    }

    render() {
        return (
            <div>

                <Growl ref={(el) => { this.growl = el; }}
                    position="bottomright"

                    baseZIndex={1000}

                ></Growl>
                <div className="row">
                    <div style={{ width: '100%' }}>

                        <ul className="nav nav-tabs">
                            <li className="nav-item">
                                <a className="nav-link active" data-toggle="tab" href="#home">Búsqueda por NUI</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" data-toggle="tab" href="#menu1">Búsqueda por Huella Dactilar</a>
                            </li>

                        </ul>


                        <div className="tab-content">
                            <div className="tab-pane container active" id="home">
                                <form onSubmit={this.consultarWS}>
                                    <div className="form-group">
                                        <label htmlFor="label" >Cédula : </label>
                                        <InputText value={this.state.cedula}
                                            onChange={(e) => this.setState({ cedula: e.target.value })}


                                        />
                                    </div>

                                    <Button type="submit" label="Obtener" icon="fa fa-fw fa-search" />

                                </form>

                                <br />

                                <b><label>Respuesta:</label></b><br />
                                <br />
                                <div className="row" >
                                    <div className="col-sm-6">
                                        <b><label>Calle :</label></b>{this.state.obtenido.Calle}<br />
                                        <b><label>CodigoError :</label></b>{this.state.obtenido.CodigoError}<br />
                                        <b><label>CondicionCedulado :</label></b>{this.state.obtenido.CondicionCedulado}<br />
                                        <b><label>Conyuge :</label></b>{this.state.obtenido.Conyuge}<br />
                                        <b><label>Domicilio :</label></b>{this.state.obtenido.Domicilio}<br />
                                        <b><label>Error :</label></b>{this.state.obtenido.Error}<br />
                                        <b><label>EstadoCivil :</label></b>{this.state.obtenido.EstadoCivil}<br />
                                        <b><label>FechaCedulacion :</label></b>{this.state.obtenido.FechaCedulacion}<br />

                                        <b><label>FechaFallecimiento :</label></b>{this.state.obtenido.FechaFallecimiento}<br />
                                        <b><label>FechaMatrimonio :</label></b>{this.state.obtenido.FechaMatrimonio}<br />
                                    </div>
                                    <div className="col-sm-6">
                                        <b><label>FechaNacimiento :</label></b>{this.state.obtenido.FechaNacimiento}<br />
                                        <b><label>Instruccion :</label></b>{this.state.obtenido.Instruccion}<br />

                                        <b><label>LugarNacimiento :</label></b>{this.state.obtenido.LugarNacimiento}<br />
                                        <b><label>NUI :</label></b>{this.state.obtenido.NUI}<br />
                                        <b><label>Nacionalidad :</label></b>{this.state.obtenido.Nacionalidad}<br />
                                        <b><label>Nombre :</label></b>{this.state.obtenido.Nombre}<br />
                                        <b><label>NumeroCasa :</label></b>{this.state.obtenido.NumeroCasa}<br />
                                        <b><label>Profesion :</label></b>{this.state.obtenido.Profesion}<br />
                                        <b><label>Sexo :</label></b>{this.state.obtenido.Sexo}<br />
                                    </div>
                                </div>



                            </div>
                            <div className="tab-pane container fade" id="menu1">
                                <form onSubmit={this.consultarWSHuella}>
                                    <div className="form-group">
                                        <label htmlFor="label" >Cédula : </label>
                                        <InputText value={this.state.cedula}
                                            onChange={(e) => this.setState({ cedula: e.target.value })}


                                        />
                                        <label htmlFor="label" >Huella Dactilar : </label>
                                        <InputText value={this.state.huella_dactilar}
                                            onChange={(e) => this.setState({ huella_dactilar: e.target.value })}


                                        />
                                    </div>

                                    <Button type="submit" label="Obtener" icon="fa fa-fw fa-search" />

                                </form>

                                <br />

                                <b><label>Respuesta:</label></b><br />
                                <br />
                                <div className="row" >
                                    <div className="col-sm-4">
                                        <b><label>Calle :</label></b>{this.state.obtenido.Calle}<br />
                                        <b><label>CodigoError :</label></b>{this.state.obtenido.CodigoError}<br />
                                        <b><label>CondicionCedulado :</label></b>{this.state.obtenido.CondicionCedulado}<br />
                                        <b><label>Conyuge :</label></b>{this.state.obtenido.Conyuge}<br />
                                        <b><label>Domicilio :</label></b>{this.state.obtenido.Domicilio}<br />
                                        <b><label>Error :</label></b>{this.state.obtenido.Error}<br />
                                        <b><label>EstadoCivil :</label></b>{this.state.obtenido.EstadoCivil}<br />
                                        <b><label>FechaCedulacion :</label></b>{this.state.obtenido.FechaCedulacion}<br />

                                        <b><label>FechaFallecimiento :</label></b>{this.state.obtenido.FechaFallecimiento}<br />
                                        <b><label>FechaMatrimonio :</label></b>{this.state.obtenido.FechaMatrimonio}<br />
                                    </div>
                                    <div className="col-sm-4">
                                        <b><label>FechaNacimiento :</label></b>{this.state.obtenido.FechaNacimiento}<br />

                                        <b><label>Instruccion :</label></b>{this.state.obtenido.Instruccion}<br />

                                        <b><label>LugarNacimiento :</label></b>{this.state.obtenido.LugarNacimiento}<br />
                                        <b><label>NUI :</label></b>{this.state.obtenido.NUI}<br />
                                        <b><label>Nacionalidad :</label></b>{this.state.obtenido.Nacionalidad}<br />
                                        <b><label>Nombre :</label></b>{this.state.obtenido.Nombre}<br />
                                        <b><label>NumeroCasa :</label></b>{this.state.obtenido.NumeroCasa}<br />
                                        <b><label>Profesion :</label></b>{this.state.obtenido.Profesion}<br />
                                        <b><label>Sexo :</label></b>{this.state.obtenido.Sexo}<br />
                                    </div>
                                    <div className="col-sm-4">
                                        <b><label>Firma :</label></b><br />
                                        {this.convertirimagen(this.state.obtenido.Firma)}

                                        <br />
                                        <b><label>Fotografia :</label></b>
                                        <br />
                                        {this.convertirimagen(this.state.obtenido.Fotografia)}
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

            </div>





        )
    }
    convertirimagen(binary) {
        if (binary != null) {
            return <img src={`data:image/jpeg;base64,${binary}`} height="140" width="140" />
        } else {

            return ""
        }
    }



    consultarWS() {
        event.preventDefault();
        axios.post("/RRHH/Colaboradores/Consumir/", {
            cedula: this.state.cedula
        })
            .then((response) => {

                console.log(response)
                this.setState({ obtenido: response.data.return })

                if (response.data.return.CodigoError != "000") {
                    this.warnMessage("" + response.data.return.Error)

                } else {
                    this.successMessage(response.data.return.Error + " : " + this.state.cedula)

                }
            })
            .catch((error) => {
                console.log(error);
            });
    }

    consultarWSHuella() {
        event.preventDefault();
        axios.post("/RRHH/Colaboradores/ConsumirHuella/", {
            cedula: this.state.cedula,
            huella_dactilar: this.state.huella_dactilar

        })
            .then((response) => {

                console.log(response)
                this.setState({ obtenido: response.data.return })

                if (response.data.return.CodigoError != "000") {
                    this.warnMessage("" + response.data.return.Error)

                } else {
                    this.successMessage(response.data.return.Error + " : " + this.state.cedula)

                }
            })
            .catch((error) => {
                console.log(error);
            });


    }

    GetColaboradores() {
        axios.post("/RRHH/Colaboradores/GetColaboradoresApi/", {})
            .then((response) => {
                this.setState({ colaboradores: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
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
    <RegistroCivil />,
    document.getElementById('content-registrocivil')
);