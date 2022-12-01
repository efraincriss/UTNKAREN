import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';

import InformacionGeneral from './Colaboradores/InformacionGeneral';
import Contacto from './Colaboradores/Contacto';
import CondicionesEmpleo from './Colaboradores/CondicionesEmpleo';
import Servicios from './Colaboradores/Servicios';
import CargaSocial from './Colaboradores/CargaSocial';
import Nomina from './Colaboradores/Nomina';
import ContactoEmergencia from './Colaboradores/ContactoEmergencia';

export default class CrearColaborador extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            activeIndex: 0,
            id_colaborador: '',
            colaborador: [],
            contacto: [],
            nacionalidades: [],
            disabled: false,
            nro_sap: "No asignado",
            visible: false,
            active: false,
            info_tab: true,
            contacto_tab: false,
            empleo_tab: false,
            servicio_tab: false,
            carga_tab: false,
            nomina_tab: false,
            emergencia_tab: false,
            loading: false,
            tiposCategorias: [],
        }
        this.childInfo = React.createRef();
        this.childContacto = React.createRef();
        this.childEmpleo = React.createRef();
        this.childServicios = React.createRef();
        this.childNomina = React.createRef();

        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.Regresar = this.Regresar.bind(this);
        this.ChangeTab = this.ChangeTab.bind(this);
        this.ColaboradorId = this.ColaboradorId.bind(this);
        this.ConsultaColaborador = this.ConsultaColaborador.bind(this);
        this.VerInfoColaborador = this.VerInfoColaborador.bind(this);
        this.ContantoInfo = this.ContantoInfo.bind(this);
        this.Siguiente = this.Siguiente.bind(this);

        this.GetNacionalidades = this.GetNacionalidades.bind(this);
        this.getFormSelectNacionalidad = this.getFormSelectNacionalidad.bind(this);
        this.GetProvincias = this.GetProvincias.bind(this);
        this.GetCiudades = this.GetCiudades.bind(this);
        this.GetParroquias = this.GetParroquias.bind(this);

        this.validarCedula = this.validarCedula.bind(this);
        this.validarEnvio = this.validarEnvio.bind(this);
        this.VerificaCedula = this.VerificaCedula.bind(this);
        this.VerificaPersonaJuridica = this.VerificaPersonaJuridica.bind(this);
        this.VerificaSectorPublico = this.VerificaSectorPublico.bind(this);

        this.consultarWSHuella = this.consultarWSHuella.bind(this);
        this.consultarWS = this.consultarWS.bind(this);

        this.CargaCategoriaEncargado = this.CargaCategoriaEncargado.bind(this);
        this.getFormSelectGrupo = this.getFormSelectGrupo.bind(this);

        this.CargaNomina = this.CargaNomina.bind(this);
    }

    componentDidMount() {
        this.GetNacionalidades();
        this.GetProvincias();
        this.GetCiudades();
        this.GetParroquias();
    }

    GetProvincias() {
        axios.get("/RRHH/Colaboradores/GetListaProvinciasApi")
            .then((response) => {
                this.setState({ provincias: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    GetCiudades() {
        axios.get("/RRHH/Colaboradores/GetListaCiudadesApi")
            .then((response) => {
                this.setState({ ciudades: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    GetParroquias() {
        axios.get("/RRHH/Colaboradores/GetListaParroquiasApi")
            .then((response) => {
                this.setState({ parroquias: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    consultarWSHuella(nro_identificacion, codigo_dactilar) {
        event.preventDefault();
        axios.post("/RRHH/Colaboradores/ConsumirHuella/", {
            cedula: nro_identificacion,
            huella_dactilar: codigo_dactilar
        })
            .then((response) => {
                console.log(response.data.return)

                if (response.data.return.CodigoError != "000") {
                    this.warnMessage("" + response.data.return.Error)
                    this.childInfo.current.loading = false;

                } else {
                    this.successMessage(response.data.return.Error + " : " + nro_identificacion)
                    this.childInfo.current.procesarDatosConsumo(response.data.return);
                }
            })
            .catch((error) => {
                console.log(error);
                this.childInfo.current.loading = false;
            });

    }

    consultarWS(cedula) {
        event.preventDefault();
        axios.post("/RRHH/Colaboradores/Consumir/", {
            cedula: cedula
        })
            .then((response) => {
                console.log(response.data.return)
                // this.setState({ obtenido: response.data.return})

                if (response.data.return.CodigoError != "000") {
                    this.warnMessage("" + response.data.return.Error)

                } else {
                    this.successMessage(response.data.return.Error + " : " + cedula)
                    this.childCargas.current.procesarDatosConsumo(response.data.return);
                }
            })
            .catch((error) => {
                console.log(error);
            });
    }


    render() {
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <div className="row" style={{ visibility: this.state.visible == true ? 'visible' : 'hidden', marginTop: this.state.visible == true ? '0%' : '-10%' }}>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="text"><b>Tipo de Identificación:</b> {this.state.colaborador.nombre_identificacion} </label>
                            </div>
                            <div className="form-group">
                                <label htmlFor="text"><b>Apellidos Nombres:</b> {this.state.colaborador.nombres_apellidos} </label>
                            </div>
                            <div className="form-group">
                                <label htmlFor="text"><b>ID Legajo:</b> {this.state.colaborador.numero_legajo_definitivo ? this.state.colaborador.numero_legajo_definitivo : this.state.colaborador.numero_legajo_temporal} </label>
                            </div>
                        </div>
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="text"><b>No. de Identificación:</b> {this.state.colaborador.numero_identificacion} </label>
                            </div>
                            <div className="form-group">
                                <label htmlFor="text"><b>Estado:</b> {this.state.colaborador.estado} </label>
                            </div>
                            <div className="form-group">
                                <label htmlFor="text"><b>ID SAP:</b> {this.state.colaborador.nro_sap ? this.state.colaborador.nro_sap : 'No asignado'} </label>
                            </div>
                        </div>
                    </div>

                    <ul className="nav nav-tabs" id="detalles_tabs" role="tablist">
                        <li className="nav-item">
                            <a className={this.state.info_tab == true ? "nav-link active show" : "nav-link"} id="info_general-tab" tabIndex="0" data-toggle="tab" href="#info_general" role="tab" aria-controls="home" aria-expanded="true">Información General</a>
                        </li>
                        <li className="nav-item" style={{ pointerEvents: this.state.active == true ? 'auto' : 'none' }}>
                            <a className={this.state.contacto_tab == true ? "nav-link active show" : "nav-link"} id="contactos-tab" tabIndex="1" data-toggle="tab" href="#contactos" role="tab" aria-controls="home" aria-expanded="true">Direcciones</a>
                        </li>
                        <li className="nav-item" style={{ pointerEvents: this.state.active == true ? 'auto' : 'none' }}>
                            <a className={this.state.empleo_tab == true ? "nav-link active show" : "nav-link"} id="empleo-tab" tabIndex="2" data-toggle="tab" href="#empleo" role="tab" aria-controls="home" aria-expanded="true">Condiciones de Empleo</a>
                        </li>
                        {/*<li className="nav-item" style={{ pointerEvents: this.state.active == true ? 'auto' : 'none' }}>
                            <a className={this.state.servicio_tab == true ? "nav-link active show" : "nav-link"} id="servicios-tab" tabIndex="3" data-toggle="tab" href="#servicios" role="tab" aria-controls="home" aria-expanded="true">Servicios</a>
        </li>*/}
                        <li className="nav-item" style={{ pointerEvents: this.state.active == true ? 'auto' : 'none' }}>
                            <a className={this.state.carga_tab == true ? "nav-link active show" : "nav-link"} id="cargas_sociales-tab" tabIndex="4" data-toggle="tab" href="#cargas_sociales" role="tab" aria-controls="home" aria-expanded="true">Familiares</a>
                        </li>
                        <li className="nav-item" style={{ pointerEvents: this.state.active == true ? 'auto' : 'none' }}>
                            <a className={this.state.nomina_tab == true ? "nav-link active show" : "nav-link"} id="nomina-tab" tabIndex="5" data-toggle="tab" href="#nomina" role="tab" aria-controls="home" aria-expanded="true">Nómina</a>
                        </li>
                        <li className="nav-item" style={{ pointerEvents: this.state.active == true ? 'auto' : 'none' }}>
                            <a className={this.state.emergencia_tab == true ? "nav-link active show" : "nav-link"} id="emergencia-tab" tabIndex="5" data-toggle="tab" href="#emergencia" role="tab" aria-controls="home" aria-expanded="true">Contactos de Emergencia</a>
                        </li>
                    </ul>

                    <div className="tab-content" id="myTabContent">
                        <div className={this.state.info_tab == true ? "tab-pane fade active show" : "tab-pane fade"} id="info_general" role="tabpanel" aria-labelledby="info_general-tab">
                            <InformacionGeneral ref={this.childInfo}
                                regresar={this.Regresar}
                                ChangeTab={this.ChangeTab}
                                ColaboradorId={this.ColaboradorId}
                                VerInfoColaborador={this.VerInfoColaborador}
                                validarCedula={this.validarCedula}
                                Siguiente={this.Siguiente}
                                getFormSelectNacionalidad={this.getFormSelectNacionalidad}
                                consultarWSHuella={this.consultarWSHuella}
                                CargaCategoriaEncargado={this.CargaCategoriaEncargado}
                                CargaNomina={this.CargaNomina}
                            />
                        </div>
                        <div className={this.state.contacto_tab == true ? "tab-pane fade active show" : "tab-pane fade"} id="contactos" role="tabpanel" aria-labelledby="contactos-tab">
                            <Contacto ref={this.childContacto}
                                regresar={this.Regresar}
                                ChangeTab={this.ChangeTab}
                                id_colaborador={this.state.id_colaborador}
                                ContantoInfo={this.ContantoInfo}
                                Siguiente={this.Siguiente}
                                getFormSelectNacionalidad={this.getFormSelectNacionalidad}
                                provincias={this.state.provincias}
                                ciudades={this.state.ciudades}
                                parroquias={this.state.parroquias}
                            />
                        </div>
                        <div className={this.state.empleo_tab == true ? "tab-pane fade active show" : "tab-pane fade"} id="empleo" role="tabpanel" aria-labelledby="empleo-tab">
                            <CondicionesEmpleo ref={this.childEmpleo}
                                regresar={this.Regresar}
                                successMessage={this.successMessage}
                                warnMessage={this.warnMessage}
                                ChangeTab={this.ChangeTab}
                                id_colaborador={this.state.id_colaborador}
                                GenerarLegajoTemporal={this.GenerarLegajoTemporal}
                                colaborador={this.state.colaborador}
                                Siguiente={this.Siguiente}
                            />
                        </div>
                       {/* <div className={this.state.servicio_tab == true ? "tab-pane fade active show" : "tab-pane fade"} id="servicios" role="tabpanel" aria-labelledby="servicios-tab">
                            <Servicios ref={this.childServicios}
                                regresar={this.Regresar}
                                successMessage={this.successMessage}
                                warnMessage={this.warnMessage}
                                ChangeTab={this.ChangeTab}
                                id_colaborador={this.state.id_colaborador}
                                colaborador={this.state.colaborador}
                                contacto={this.state.contacto}
                                Siguiente={this.Siguiente}
                            />
                        </div>* */}
                        <div className={this.state.carga_tab == true ? "tab-pane fade active show" : "tab-pane fade"} id="cargas_sociales" role="tabpanel" aria-labelledby="cargas_sociales-tab">
                            <CargaSocial
                                regresar={this.Regresar}
                                successMessage={this.successMessage}
                                warnMessage={this.warnMessage}
                                ChangeTab={this.ChangeTab}
                                id_colaborador={this.state.id_colaborador}
                                validarCedula={this.validarCedula}
                                Siguiente={this.Siguiente}
                                getFormSelectNacionalidad={this.getFormSelectNacionalidad}
                                consultarWS={this.consultarWS}
                                colaborador={this.state.colaborador}
                            />
                        </div>
                        <div className={this.state.nomina_tab == true ? "tab-pane fade active show" : "tab-pane fade"} id="nomina" role="tabpanel" aria-labelledby="nomina-tab">
                            <Nomina ref={this.childNomina}
                                regresar={this.Regresar}
                                successMessage={this.successMessage}
                                warnMessage={this.warnMessage}
                                ChangeTab={this.ChangeTab}
                                id_colaborador={this.state.id_colaborador}
                                colaborador={this.state.colaborador}
                                getFormSelectGrupo={this.getFormSelectGrupo}
                                tiposCategorias={this.state.tiposCategorias}
                                Siguiente={this.Siguiente}
                            />
                        </div>
                        <div className={this.state.emergencia_tab == true ? "tab-pane fade active show" : "tab-pane fade"} id="emergencia" role="tabpanel" aria-labelledby="emergencia-tab">
                            <ContactoEmergencia
                                regresar={this.Regresar}
                                id_colaborador={this.state.id_colaborador}
                                colaborador={this.state.colaborador}
                            />
                        </div>
                    </div>
                    <br />
                    <div className="form-group">
                        <div className="col">
                            <button type="button" onClick={this.validarEnvio} className="btn btn-outline-primary"> Enviar</button>
                        </div>
                    </div>
                </div>
            </BlockUi>
        )
    }

    CargaCategoriaEncargado(id) {
        axios.post("/RRHH/CategoriasEncargado/GetCategoriasPorEncargadoPersonalApi/", { id: id })
            .then((response) => {
                if (response.data == []) {
                    // this.warnMessage('No existe !!!');
                } else {
                    this.setState({ tiposCategorias: response.data });
                    this.getFormSelectGrupo();
                }

            })
            .catch((error) => {
                console.log(error);
            });

    }

    getFormSelectGrupo() {
        return (
            this.state.tiposCategorias.map((item) => {
                // console.log(item.Categoria.Id, item.Categoria.nombre)
                return (
                    <option key={Math.random()} value={item.Categoria.Id}>{item.Categoria.nombre}</option>
                )
            })
        );

    }

    validarEnvio() {
        console.log('envio');
        // axios.post("/RRHH/Colaboradores/GetExcelApi/")
        //     .then((response) => {
        //         console.log(response)
        //         if(response.data == "OK"){
        //             this.props.showSuccess("Excel generado");
        //         }else{
        //             this.props.showWarn("Algo salió mal");
        //         }
        //     })
        //     .catch((error) => {
        //         console.log(error);
        //     });
        var info = this.childInfo.current.handleValidation();
        var contacto = this.childContacto.current.handleValidation();
        var empleo = this.childEmpleo.current.handleValidation();
        var nomina = this.childNomina.current.handleValidation();
        console.log(info, contacto, empleo, servicios, nomina)
        if (info == false) {
            this.warnMessage("Complete Información General");
        }
        else if (contacto == false) {
            this.warnMessage("Complete Direcciones");
        }
        else if (empleo == false) {
            this.warnMessage("Complete Condiciones de Empleo");
        }
        else if (nomina == false) {
            this.warnMessage("Complete Nómina");
        }
        else if (info == true && contacto == true && empleo == true && nomina == true) {
            // this.successMessage("Datos completos");
            this.setState({ loading: true })
            axios.post("/RRHH/Colaboradores/EditEstadoColaboradorApi/", {
                id: this.state.id_colaborador,
                estado: "INFORMACION COMPLETA",
            })
                .then((response) => {
                    console.log(response)
                    this.successMessage("Cambio estado a 'INFORMACION COMPLETA'");
                    this.setState({ loading: false })
                })
                .catch((error) => {
                    console.log(error);
                });
        }
        else {
            this.warnMessage("Algo salio mal.");
        }
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/Colaboradores/Vista/"
        );
    }

    GetNacionalidades() {
        axios.post("/RRHH/Colaboradores/GetPaisesApi", {})
            .then((response) => {
                this.setState({ nacionalidades: response.data })
                this.getFormSelectNacionalidad()
            })
            .catch((error) => {
                console.log(error);
            });
    }

    getFormSelectNacionalidad() {
        return (
            this.state.nacionalidades.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    Siguiente(id) {
        this.setState({ active: true });
        console.log('empleo_tab', this.state.empleo_tab);
        switch (id) {
            case 1:
                this.setState({
                    info_tab: false,
                    contacto_tab: true,
                    empleo_tab: false,
                    servicio_tab: false,
                    carga_tab: false,
                    nomina_tab: false,
                });

                // window.scrollTo = "contactos";

                return;
            case 2:
                this.setState({
                    info_tab: false,
                    contacto_tab: false,
                    empleo_tab: true,
                    servicio_tab: false,
                    carga_tab: false,
                    nomina_tab: false,
                });
                // window.scrollTo = "condiciones_empleo";
                return;
            case 3:
                this.state.empleo_tab = true;
                this.setState({
                    empleo_tab: false,
                    info_tab: false,
                    contacto_tab: false,
                    servicio_tab: true,
                    carga_tab: false,
                    nomina_tab: false,
                    emergencia_tab: false,
                });
                console.log('empleo_tab333', this.state.empleo_tab);
                return;
            case 4:
                this.setState({
                    info_tab: false,
                    contacto_tab: false,
                    empleo_tab: false,
                    servicio_tab: false,
                    carga_tab: true,
                    nomina_tab: false,
                    emergencia_tab: false,
                });
                // window.scrollTo = "cargas_sociales";
                return;
            case 5:
                this.setState({
                    info_tab: false,
                    contacto_tab: false,
                    empleo_tab: false,
                    servicio_tab: false,
                    carga_tab: false,
                    nomina_tab: true,
                    emergencia_tab: false,
                });
                // window.scrollTo = "nomina";
                return;
            case 6:
                this.setState({
                    info_tab: false,
                    contacto_tab: false,
                    empleo_tab: false,
                    servicio_tab: false,
                    carga_tab: false,
                    nomina_tab: false,
                    emergencia_tab: true,
                });
                // window.scrollTo = "nomina";
                return;
        }
    }

    ChangeTab(destino) {
      //  this.childServicios.current.ConsultaServicios(destino);
    }

    validarCedula(identificacion) {
        var estado = false;
        var valced = [];
        var provincia;
        // console.log(identificacion.length)
        if (identificacion == "2222222222") {
            estado = false
        }
        else if (identificacion.length >= 10) {
            valced = identificacion.split('');
            // console.log('valced', valced)
            provincia = Number.parseInt(valced[0] + valced[1]);
            // console.log('provincia', provincia)
            if (provincia > 0 && provincia < 25) {
                if (Number.parseInt(valced[2]) < 6) {
                    estado = this.VerificaCedula(valced);
                    // console.log('VerificaCedula')
                }
                else if (Number.parseInt(valced[2]) == 6) {
                    estado = this.VerificaSectorPublico(valced);
                }
                else if (Number.parseInt(valced[2]) == 9) {

                    estado = this.VerificaPersonaJuridica(valced);
                }
            }
        }
        return estado;
    }

    VerificaCedula(validarCedula) {
        var aux = 0, par = 0, impar = 0, verifi;
        for (var i = 0; i < 9; i += 2) {
            aux = 2 * Number.parseInt(validarCedula[i]);
            if (aux > 9)
                aux -= 9;
            par += aux;
        }
        for (var i = 1; i < 9; i += 2) {
            impar += Number.parseInt(validarCedula[i]);
        }

        aux = par + impar;
        if (aux % 10 != 0) {
            verifi = 10 - (aux % 10);
        }
        else
            verifi = 0;
        if (verifi == Number.parseInt(validarCedula[9]))
            return true;
        else
            return false;
    }

    VerificaPersonaJuridica(validarCedula) {
        var aux = 0, prod, veri;
        veri = Number.parseInt(validarCedula[10]) + Number.parseInt(validarCedula[11]) + Number.parseInt(validarCedula[12]);
        if (veri > 0) {
            let coeficiente = [4, 3, 2, 7, 6, 5, 4, 3, 2];
            for (var i = 0; i < 9; i++) {
                prod = Number.parseInt(validarCedula[i]) * coeficiente[i];
                aux += prod;
            }
            if (aux % 11 == 0) {
                veri = 0;
            }
            else if (aux % 11 == 1) {
                return false;
            }
            else {
                aux = aux % 11;
                veri = 11 - aux;
            }

            if (veri == Number.parseInt(validarCedula[9])) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }

    VerificaSectorPublico(validarCedula) {
        var aux = 0, prod, veri;
        veri = Number.parseInt(validarCedula[9]) + Number.parseInt(validarCedula[10]) + Number.parseInt(validarCedula[11]) + Number.parseInt(validarCedula[12]);
        if (veri > 0) {
            let coeficiente = [3, 2, 7, 6, 5, 4, 3, 2];

            for (var i = 0; i < 8; i++) {
                prod = Number.parseInt(validarCedula[i]) * coeficiente[i];
                aux += prod;
            }

            if (aux % 11 == 0) {
                veri = 0;
            }
            else if (aux % 11 == 1) {
                return false;
            }
            else {
                aux = aux % 11;
                veri = 11 - aux;
            }

            if (veri == Number.parseInt(validarCedula[8])) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }



    VerInfoColaborador() {
        this.setState({
            visible: true
        })
        // this.state.visible = true;
    }

    ColaboradorId(id) {
        console.log('entro a colaborador id');
        this.setState({ id_colaborador: id });
        this.ConsultaColaborador(id);
    }

    ConsultaColaborador(id) {
        console.log(id);
        axios.post("/RRHH/Colaboradores/GetColaboradorApi/", { id: id })
            .then((response) => {
                console.log('consulta col', response.data)
                //var data = JSON.parse(response.data);
                // this.setState({
                //     colaborador: response.data
                // })
                this.state.colaborador = response.data;
                this.VerInfoColaborador();
            })
            .catch((error) => {
                console.log(error);
            });
    }

    ContantoInfo(data, id) {
        var d = JSON.parse(data);
        console.log('entro a contacto');
        this.setState({ contacto: d });
        this.state.colaborador.ContactoId = id;
        console.log(this.state.colaborador.ContactoId);
    }

    CargaNomina(id) {
        this.childNomina.current.CargaDatosNomina(id);
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
    <CrearColaborador />,
    document.getElementById('content-crear-colaborador')
);