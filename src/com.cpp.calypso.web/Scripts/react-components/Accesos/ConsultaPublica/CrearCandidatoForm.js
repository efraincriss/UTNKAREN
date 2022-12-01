import React from "react";
import axios from "axios";
import Field from "../../Base/Field-v2";
import {
    TIPO_IDENTIFICACION,
    REGISTRO_CIVIL_SUCCESS
} from '../../Base/Constantes';
import http from "../../Base/HttpService";
import config from "../../Base/Config";
import {
    TIPO_PROYECTO,

} from "../../Base/Constantes";
import {
    FRASE_ERROR_SELECCIONAR_IDENTIFICACION,
    FRASE_ERROR_SELECCIONAR_TIPO_IDENTIFICACION,
    FRASE_ERROR_SELECCIONAR_PROYECTO,
    FRASE_ERROR_INGRESA_NOMBRE,
    FRASE_ERROR_INGRESAR_CONDICION_CIUDADANO,
    FRASE_ERROR_CONSULTAR_WS_REGISTRO_CIVIL,
    FRASE_ERROR_GLOBAL,
    MODULO_RECURSOS_HUMANOS,
    FRASE_CANDIDATO_ENCONTRADO,
    CONTROLLER_CONSULTA_PUBLICA,
    MODULO_ACCESO,
    FRASE_CANDIDATO_ACTUALIZADO,
    FRASE_CANDIDATO_CREADO,
    CONTROLLER_COLABORADORES
} from "../../Base/Strings";
import dateFormatter from "../../Base/DateFormatter";
import { Messages } from 'primereact-v2/messages';
import { Button } from 'primereact-v2/button';

export default class CrearCandidatoForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            tipoIdentificacion: [],
            //paises: [],
            //provincias: [],
            //ciudades: [],
            // paisId: 0,
            //ciudadId: 0,
            //provinciaId: 0,
            tipoIdentificacionId: 0,
            errors: {},
            identificacion: '',
            proyectos: [],
            proyectoid: 0,
            wsResult: {
                Nombre: '',
                CondicionCedulado: '',
                Fotografia: ''
            },
            showExtraInputs: false,
            canSave: false,
            canUpdate: false,
            canManualCreate: false,
            candidato: {},

            //ES Nueva Consulta
            huella: ''

        }
        this.convertirimagen = this.convertirimagen.bind(this);
    }

    componentDidMount() {
        this.consultarDatos();
    }


    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <Messages ref={(el) => this.messages = el} />
                    </div>
                </div>
                <div>
                    <div className="row">
                        <div className="col-10">
                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="tipoIdentificacionId"
                                        required
                                        value={this.state.tipoIdentificacionId}
                                        label="Tipo Identificación"
                                        options={this.state.tipoIdentificacion}
                                        type={"select"}
                                        onChange={this.onChangeValue}
                                        error={this.state.errors.tipoIdentificacionId}
                                        readOnly={false}
                                        placeholder="Seleccione.."
                                        filterPlaceholder="Seleccione.."

                                    />
                                </div>

                                <div className="col">
                                    <Field
                                        name="identificacion"
                                        label="Identificación"
                                        required
                                        edit={true}
                                        readOnly={false}
                                        value={this.state.identificacion}
                                        onChange={this.handleChange}
                                        error={this.state.errors.identificacion}
                                    />

                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="huella"
                                        label="Código Dactilar"
                                        edit={true}
                                        readOnly={false}
                                        value={this.state.huella}
                                        onChange={this.handleChange}
                                        error={this.state.errors.huella}
                                    />

                                </div>

                                <div className="col">
                                    {/*  <Field
                                name="provinciaId"
                                required
                                value={this.state.provinciaId}
                                label="Provincia"
                                options={this.state.provincias}
                                type={"select"}
                                filter={true}
                                onChange={this.onChangeProvincias}
                                error={this.state.errors.provincias}
                                readOnly={false}
                                placeholder="Seleccione.."
                                filterPlaceholder="Seleccione.."

                            />
                             */}
                                </div>
                            </div>

                            {this.state.showExtraInputs ?
                                <div className="row">
                                    <div className="col">
                                        <Field
                                            name="Nombre"
                                            label="Nombres Completos"
                                            required
                                            edit={true}
                                            readOnly={false}
                                            value={this.state.wsResult.Nombre}
                                            onChange={this.handleChangeWsResult}
                                            error={this.state.errors.Nombre}
                                        />

                                    </div>
                                    <div className="col">
                                        <Field
                                            name="CondicionCedulado"
                                            label="Condición Cedulado"
                                            required
                                            edit={true}
                                            readOnly={false}
                                            value={this.state.CondicionCedulado}
                                            onChange={this.handleChangeWsResult}
                                            error={this.state.errors.CondicionCedulado}
                                        />
                                    </div>
                                </div>
                                :
                                this.state.wsResult.Nombre ?
                                    <div className="row">
                                        <div className="col">
                                            <h6><b>Nombres: </b>{this.state.wsResult.Nombre}</h6>
                                        </div>
                                        <div className="col">
                                            <h6><b>Condición: </b>{this.state.wsResult.CondicionCedulado}</h6>
                                        </div>
                                    </div>
                                    :
                                    <div className="row">
                                    </div>
                            }
                            <div className="row">

                                <div className="col">
                                    <Field
                                        name="proyectoid"
                                        required
                                        value={this.state.proyectoid}
                                        label="Proyecto"
                                        options={this.state.proyectos}
                                        type={"select"}
                                        filter={true}
                                        onChange={this.onChangeValue}
                                        error={this.state.errors.proyectoid}
                                        readOnly={false}
                                        placeholder="Seleccione.."
                                        filterPlaceholder="Seleccione.."

                                    />
                                </div>
                                <div className="col">
                                </div>
                            </div>
                        </div>
                        {this.state.wsResult.Fotografia != null && this.state.wsResult.Fotografia.length > 0 &&
                            <div className="col-2">
                                <label>Fotografía :</label>
                                <br />
                                {this.convertirimagen(this.state.wsResult.Fotografia)}
                                <br />
                            </div>
                        }
                    </div>


                    <div className="row" style={{ marginTop: '0.4em' }}>
                        {this.renderButtons()}
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
    renderButtons = () => {
        return (
            <div className="col">
                <Button
                    label="Consultar"
                    icon="pi pi-check"
                    onClick={this.handleSubmit}
                    className="p-button-rounded"
                    style={{ marginRight: '0.3em' }}
                />

                {this.state.canSave &&
                    <Button label="Ingresar"
                        icon="pi pi-check"
                        onClick={this.crearCandidato}
                        className="p-button-rounded"
                        style={{ marginRight: '0.3em' }} />
                }

                {this.state.canUpdate &&
                    <Button
                        label="Actualizar Datos"
                        icon="pi pi-check"
                        onClick={this.actualizarCandidato}
                        className="p-button-rounded"
                        style={{ marginRight: '0.3em' }}
                    />
                }

                {this.state.canUpdate &&
                    <Button
                        label="Actualizar Registro Civil"
                        icon="pi pi-check"
                        onClick={this.actualizarConRegistroCivil}
                        className="p-button-rounded"
                        style={{ marginRight: '0.3em' }}
                    />
                }
                {this.state.canManualCreate &&
                    <Button
                        label="Ingreso Manual"
                        icon="pi pi-check"
                        onClick={this.ingresoManual}
                        className="p-button-rounded"
                        style={{ marginRight: '0.3em' }}
                    />
                }
            </div>
        )
    }

    handleSubmit = () => {
        if (this.state.identificacion === '') {
            this.props.showWarn(FRASE_ERROR_SELECCIONAR_IDENTIFICACION)
        } else {
            this.props.blockScreen();
            this.setState({
                wsResult: {},
                econtrado: false,
                canSave: false,
                canUpdate: false,
                canManualCreate: false,
                candidato: {}
            }, this.comprobarExistenciaCandidato(this.state.identificacion))
        }
    }

    comprobarExistenciaCandidato = (identificacion) => {
        let url = '';
        url = `${config.appUrl}${MODULO_ACCESO}/${CONTROLLER_CONSULTA_PUBLICA}/ExisteCandidato?identificacion=${identificacion}`;
        http.get(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        showExtraInputs: false,
                        canUpdate: true,
                        canSave: false,
                        canManualCreate: false,
                        candidato: data.result,
                        paisId: data.result.PaisTrabajoId,
                        provinciaId: data.result.ProvinciaTrabajoId,
                        tipoIdentificacionId: data.result.TipoIdentificacionId,
                        // ciudadId: data.result.CiudadTrabajoId,
                        proyectoid: data.result.ProyectoId,
                        wsResult: {
                            Nombre: data.result.nombres_completos,
                            CondicionCedulado: data.result.condicion_cedulado
                        }
                    }/*, this.consultarProvinciasCiudades*/)
                    this.props.showSuccess(FRASE_CANDIDATO_ENCONTRADO)
                    this.props.unlockScreen();

                } else if (data.success === false) {
                    this.consultarWS();
                } else {
                    console.log(data)
                    this.props.showWarn(FRASE_ERROR_GLOBAL);
                    this.props.unlockScreen();
                }
            })
            .catch((error) => {
                this.props.showWarn(FRASE_ERROR_GLOBAL);
                console.log(error);
                this.props.unlockScreen();
            });
    }

    crearCandidato = () => {
        let validForm = this.validarEnvioDeDatos();
        if (validForm) {
            this.props.blockScreen();
            var entity = {
                // CiudadTrabajoId: this.state.ciudadId,
                ProyectoId: this.state.proyectoid,
                TipoIdentificacionId: this.state.tipoIdentificacionId,
                identificacion: this.state.identificacion,
                nombres_completos: this.state.wsResult.Nombre ? this.state.wsResult.Nombre : ' ',
                condicion_cedulado: this.state.wsResult.CondicionCedulado ? this.state.wsResult.CondicionCedulado : ' ',
                calle: this.state.wsResult.Calle ? this.state.wsResult.Calle : ' ',
                codigo_error: this.state.wsResult.CodigoError ? this.state.wsResult.CodigoError : ' ',
                conyugue: this.state.wsResult.Conyuge && this.state.wsResult.Conyuge != "" ? this.state.wsResult.Conyuge : ' ',
                domicilio: this.state.wsResult.Domicilio && this.state.wsResult.Domicilio != "" ? this.state.wsResult.Domicilio : ' ',
                error: this.state.wsResult.Error ? this.state.wsResult.Error : ' ',
                estado_civil: this.state.wsResult.EstadoCivil ? this.state.wsResult.EstadoCivil : 0,
                fecha_cedulacion: this.state.wsResult.FechaCedulacion ? dateFormatter(this.state.wsResult.FechaCedulacion).props.date : null,
                fecha_fallecimiento: this.state.wsResult.FechaFallecimiento ? dateFormatter(this.state.wsResult.FechaFallecimiento).props.date : null,
                fecha_matrimonio: this.state.wsResult.FechaMatrimonio ? dateFormatter(this.state.wsResult.FechaMatrimonio).props.date : null,
                fecha_nacimiento: this.state.wsResult.FechaNacimiento ? dateFormatter(this.state.wsResult.FechaNacimiento).props.date : null,
                instruccion: this.state.wsResult.Instruccion ? this.state.wsResult.Instruccion : ' ',
                lugar_nacimiento: this.state.wsResult.LugarNacimiento ? this.state.wsResult.LugarNacimiento : ' ',
                nacionalidad: this.state.wsResult.Nacionalidad ? this.state.wsResult.Nacionalidad : ' ',
                numero_casa: this.state.wsResult.NumeroCasa && this.state.wsResult.NumeroCasa != "" ? this.state.wsResult.NumeroCasa : ' ',
                profesion: this.state.wsResult.Profesion && this.state.wsResult.Profesion != "" ? this.state.wsResult.Profesion : ' ',
                sexo: this.state.wsResult.Sexo ? this.state.wsResult.Sexo : ' ',
                fecha_consulta: new Date(),
                ArchivoPdfId: null
            }

            this.crear(entity);
        }


    }


    // Actualizo datos solo del formulario
    actualizarCandidato = () => {
        // En el state.candidato tengo el candidato recuperado
        // Cojo el candidato en el state y sobrescribo solo los datos del formulario
        // No tengo wsResult
        let validForm = this.validarEnvioDeDatos();
        if (validForm) {
            const updatedData = {
                ...this.state.candidato
            };
            updatedData["TipoIdentificacionId"] = this.state.tipoIdentificacionId;
            updatedData["identificacion"] = this.state.identificacion;
            updatedData["ProyectoId"] = this.state.proyectoid;
            //updatedData["CiudadTrabajoId"] = this.state.ciudadId;

            this.actualizar(updatedData);
        }
    }

    // Actializacion que consulta los datos en el registro civil y lo completa con el formulario
    actualizarConRegistroCivil = () => {
        // Hacer la consulta al registro civil
        let validForm = this.validarEnvioDeDatos();
        if (validForm) {
            this.props.blockScreen();
            let url;
            url = `/Accesos/ConsultaPublica/ObtenerDatosWs`;
            http.post(url, {
                cedula: this.state.identificacion
            })
                .then((response) => {
                    if (response.data.return.CodigoError != REGISTRO_CIVIL_SUCCESS) {
                        this.props.showWarn(FRASE_ERROR_CONSULTAR_WS_REGISTRO_CIVIL);
                        this.props.unlockScreen();
                    } else {
                        let wsResult = response.data.return;
                        let entity = this.generarEntidadActualizacion(wsResult);
                        this.actualizar(entity);
                    }

                })
                .catch((error) => {
                    this.props.showWarn(FRASE_ERROR_GLOBAL);
                    console.log(error);
                    this.props.unlockScreen();
                });
        }

    }

    // Ingreso solo los datos del formulario
    ingresoManual = () => {
        var validForm = this.validarEnvioDeDatos();
        if (validForm) {
            var entity = {
                //CiudadTrabajoId: this.state.ciudadId,
                ProyectoId: this.state.proyectoid,
                TipoIdentificacionId: this.state.tipoIdentificacionId,
                identificacion: this.state.identificacion,
                nombres_completos: this.state.wsResult.Nombre,
                condicion_cedulado: this.state.wsResult.CondicionCedulado
            }

            this.crear(entity);
        }
    }

    crear = entity => {
        this.props.blockScreen();
        let url = '';
        url = `${config.appUrl}${MODULO_ACCESO}/${CONTROLLER_CONSULTA_PUBLICA}/CreateApi`;
        http.post(url, entity)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        //provincias: [],
                        // ciudades: [],
                        // paisId: 0,
                        //provinciaId: 0,
                        //ciudadId: 0,
                        proyectoid: 0,
                        tipoIdentificacionId: 0,
                        errors: {},
                        identificacion: '',
                        wsResult: {},
                        canSave: false,
                        showExtraInputs: false,
                        canUpdate: false,
                        canManualCreate: false,
                        candidato: {}
                    })

                    this.props.ocultarCreateForm()
                    this.props.showSuccess(FRASE_CANDIDATO_CREADO)
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }

    actualizar = entity => {
        this.props.blockScreen();
        let url = '';
        url = `${config.appUrl}${MODULO_ACCESO}/${CONTROLLER_CONSULTA_PUBLICA}/UpdateApi`;
        http.post(url, entity)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        // provincias: [],
                        // ciudades: [],
                        //paisId: 0,
                        // provinciaId: 0,
                        //  ciudadId: 0,
                        proyectoid: 0,
                        tipoIdentificacionId: 0,
                        errors: {},
                        identificacion: '',
                        wsResult: {},
                        canSave: false,
                        showExtraInputs: false,
                        canUpdate: false,
                        canManualCreate: false,
                        candidato: {}
                    })
                    this.props.ocultarCreateForm()
                    this.props.showSuccess(FRASE_CANDIDATO_ACTUALIZADO)
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }

    consultarWS = () => {

        if (this.state.huella != null && this.state.huella.length > 0) {
            if (this.state.huella.length >= 6) {
                this.props.blockScreen();
                let url = '';
                url = `/Accesos/ConsultaPublica/ObtenerWsHuella`;
                http.post(url, {
                    cedula: this.state.identificacion,
                    huella: this.state.huella
                })
                    .then((response) => {
                        if (response.data === "SIN_RESPUESTA") {
                            abp.notify.error('Sin respuesta al Web Service verifique los datos de conección', 'Error');
                            this.props.unlockScreen();
                            const updatedWs = {
                                ...this.state.wsResult
                            };
                            updatedWs["Nombre"] = ''
                            updatedWs["CondicionCedulado"] = ''
                            updatedWs["Fotografia"] = ''
                            this.setState({
                                showExtraInputs: true,
                                canManualCreate: true,
                                canSave: false,
                                canUpdate: false,
                                wsResult: updatedWs
                            })
                        } else {
                            if (response.data.return.CodigoError != REGISTRO_CIVIL_SUCCESS) {
                                this.props.showWarn(FRASE_ERROR_CONSULTAR_WS_REGISTRO_CIVIL);

                                const updatedWs = {
                                    ...this.state.wsResult
                                };
                                updatedWs["Nombre"] = ''
                                updatedWs["CondicionCedulado"] = ''
                                updatedWs["Fotografia"] = ''
                                this.setState({
                                    showExtraInputs: true,
                                    canManualCreate: true,
                                    canSave: false,
                                    canUpdate: false,
                                    wsResult: updatedWs
                                })
                            } else {
                                this.setState({
                                    wsResult: response.data.return,
                                    encontrado: false,
                                    canUpdate: false,
                                    canSave: true
                                })
                                this.props.showSuccess(`Candidato con identificación ${this.state.identificacion} encontrado en el registro civil.`)
                                console.log(this.state.wsResult);
                                console.log(dateFormatter(this.state.wsResult.FechaMatrimonio).props.date)
                            }
                            this.props.unlockScreen();
                        }
                    })
                    .catch((error) => {
                        this.props.showWarn(FRASE_ERROR_GLOBAL);
                        console.log(error);
                        this.props.unlockScreen();
                    });
            } else {

                this.props.showWarn("El Código Dactilar debe tener al menos 6 Dígitos");
                this.props.unlockScreen();
            }

        } else {

            this.props.blockScreen();
            let url = '';
            url = `/Accesos/ConsultaPublica/ObtenerDatosWs`;
            http.post(url, {
                cedula: this.state.identificacion
            })
                .then((response) => {
                    if (response.data === "SIN_RESPUESTA") {
                        abp.notify.error('Sin respuesta al Web Service verifique los datos de conección', 'Error');
                        this.props.unlockScreen();
                    } else {
                        if (response.data.return.CodigoError != REGISTRO_CIVIL_SUCCESS) {
                            this.props.showWarn(FRASE_ERROR_CONSULTAR_WS_REGISTRO_CIVIL);

                            const updatedWs = {
                                ...this.state.wsResult
                            };
                            updatedWs["Nombre"] = ''
                            updatedWs["CondicionCedulado"] = ''

                            this.setState({
                                showExtraInputs: true,
                                canManualCreate: true,
                                canSave: false,
                                canUpdate: false,
                                wsResult: updatedWs
                            })
                        } else {
                            this.setState({
                                wsResult: response.data.return,
                                encontrado: false,
                                canUpdate: false,
                                canSave: true
                            })
                            this.props.showSuccess(`Candidato con identificación ${this.state.identificacion} encontrado en el registro civil.`)
                            console.log(this.state.wsResult);
                            console.log(dateFormatter(this.state.wsResult.FechaMatrimonio).props.date)
                        }
                        this.props.unlockScreen();
                    }
                })
                .catch((error) => {
                    this.props.showWarn(FRASE_ERROR_GLOBAL);
                    console.log(error);
                    this.props.unlockScreen();
                });
        }

    }

    generarEntidadActualizacion = (wsResult) => {
        var entity = {
            Id: this.state.candidato.Id,
            //CiudadTrabajoId: this.state.ciudadId,
            ProyectoId: this.state.proyectoid,
            TipoIdentificacionId: this.state.tipoIdentificacionId,
            identificacion: this.state.identificacion,
            nombres_completos: wsResult.Nombre ? wsResult.Nombre : ' ',
            condicion_cedulado: wsResult.CondicionCedulado ? wsResult.CondicionCedulado : ' ',
            calle: wsResult.Calle ? wsResult.Calle : ' ',
            codigo_error: wsResult.CodigoError ? wsResult.CodigoError : ' ',
            conyugue: wsResult.Conyuge && wsResult.Conyuge != "" ? wsResult.Conyuge : ' ',
            domicilio: wsResult.Domicilio && wsResult.Domicilio != "" ? wsResult.Domicilio : ' ',
            error: wsResult.Error ? wsResult.Error : ' ',
            estado_civil: wsResult.EstadoCivil ? wsResult.EstadoCivil : 0,
            fecha_cedulacion: wsResult.FechaCedulacion ? dateFormatter(wsResult.FechaCedulacion).props.date : null,
            fecha_fallecimiento: wsResult.FechaFallecimiento ? dateFormatter(wsResult.FechaFallecimiento).props.date : null,
            fecha_matrimonio: wsResult.FechaMatrimonio ? dateFormatter(wsResult.FechaMatrimonio).props.date : null,
            fecha_nacimiento: wsResult.FechaNacimiento ? dateFormatter(wsResult.FechaNacimiento).props.date : null,
            instruccion: wsResult.Instruccion ? wsResult.Instruccion : ' ',
            lugar_nacimiento: wsResult.LugarNacimiento ? wsResult.LugarNacimiento : ' ',
            nacionalidad: wsResult.Nacionalidad ? wsResult.Nacionalidad : ' ',
            numero_casa: wsResult.NumeroCasa && wsResult.NumeroCasa != "" ? wsResult.NumeroCasa : ' ',
            profesion: wsResult.Profesion && wsResult.Profesion != "" ? wsResult.Profesion : ' ',
            sexo: wsResult.Sexo ? wsResult.Sexo : ' ',
            fecha_consulta: new Date(),
            ArchivoPdfId: this.state.candidato.ArchivoPdfId
        }
        console.log(entity);

        return entity;
    }

    catalogoTiposIdentificacion = () => {
        axios
            .get(`/Accesos/ConsultaPublica/ObtenerCatalogos/?code=${TIPO_IDENTIFICACION}`, {})
            .then(response => {
                console.log(response.data)
                var items = response.data.map(item => {
                    return { label: item.nombre, dataKey: item.Id, value: item.Id };
                });
                this.setState({ tipoIdentificacion: items });
                this.props.unlockScreen()
            })
            .catch(error => {
                console.log(error);
                this.props.unlockScreen()
            });
    }

    /*obtenerPaises = () => {
        let url = '';
        url = `${config.appUrl}${MODULO_ACCESO}/${CONTROLLER_CONSULTA_PUBLICA}/ObtenerPaises`;
        return http.get(url);
    }
*/
    /*obtenerProvincias = (paisId = this.state.paisId) => {
        let url = '';
        url = `${config.appUrl}${MODULO_ACCESO}/${CONTROLLER_CONSULTA_PUBLICA}/ObtenerProvinciasPorPais/${paisId}`;
        return http.get(url);
    }*/
    obtenerproyectos() {
        axios
            .get("/Accesos/ConsultaPublica/ObtenerCatalogos/?code=PROYECTO", {})
            .then(response => {
                console.log(response.data)
                var items = response.data.map(item => {
                    return { label: item.nombre, dataKey: item.Id, value: item.Id };
                });
                this.setState({ proyectos: items });
                this.props.unlockScreen()
            })
            .catch(error => {
                console.log(error);
                this.props.unlockScreen()
            });

    }
    /*obtenerCiudades = () => {
        let url = '';
        url = `${config.appUrl}${MODULO_ACCESO}/${CONTROLLER_CONSULTA_PUBLICA}/ObtenerCiudadesPorProvincia/${this.state.provinciaId}`;
        return http.get(url);
    }*/

    consultarDatos = () => {
        this.props.blockScreen();
        this.obtenerproyectos()
        this.catalogoTiposIdentificacion()
    }

    validarEnvioDeDatos = () => {
        let flag = true;
        if (this.state.tipoIdentificacionId === 0) {
            flag = false;
            this.props.showWarn(FRASE_ERROR_SELECCIONAR_TIPO_IDENTIFICACION)
        } else if (this.state.proyectoid === 0) {
            flag = false;
            this.props.showWarn(FRASE_ERROR_SELECCIONAR_PROYECTO)
        } else if (this.state.identificacion === '') {
            flag = false;
            this.props.showWarn(FRASE_ERROR_SELECCIONAR_IDENTIFICACION)
        } else if (this.state.wsResult.Nombre === '') {
            flag = false;
            this.props.showWarn(FRASE_ERROR_INGRESA_NOMBRE)
        } else if (this.state.wsResult.CondicionCedulado === '') {
            flag = false;
            this.props.showWarn(FRASE_ERROR_INGRESAR_CONDICION_CIUDADANO)
        }
        return flag;
    }

    handleChange = (event) => {
        if (event.target.name === 'identificacion') {
            this.setState({
                [event.target.name]: event.target.value,
                wsResult: {},
                canSave: false,
                showExtraInputs: false,
                canUpdate: false,
                canManualCreate: false,
                candidato: {},
                //provincias: [],
                // ciudades: [],
                //paisId: 0,
                // provinciaId: 0,
                // ciudadId: 0,
                //tipoIdentificacionId: 0,
                proyectoid: 0,
                errors: {},
            });
        } else {
            this.setState({ [event.target.name]: event.target.value });
        }
    }

    handleChangeWsResult = (event) => {
        const name = event.target.name;
        const value = event.target.value;
        const updatedData = {
            ...this.state.wsResult
        };
        updatedData[name] = value.toUpperCase();
        this.setState({
            wsResult: updatedData
        });
    }

    onChangeValue = (name, value) => {
        this.setState({
            [name]: value
        });
    }

    onChangeTipoIdentificacion = (name, value) => {
        this.setState({
            tipoIdentificacionId: value
        });
    }


    showSticky = (message) => {
        this.messages.show({ severity: 'info', summary: 'Información', detail: message, sticky: true });
    }

    resetDatos = () => {
        this.setState({
            provincias: [],
            ciudades: [],
            paisId: 0,
            provinciaId: 0,
            //ciudadId: 0,
            tipoIdentificacionId: 0,
            errors: {},
            identificacion: '',
            wsResult: {},
            canSave: false,
            showExtraInputs: false,
            canUpdate: false,
            canManualCreate: false,
            candidato: {},
            proyectoid: 0,
        })
    }

}