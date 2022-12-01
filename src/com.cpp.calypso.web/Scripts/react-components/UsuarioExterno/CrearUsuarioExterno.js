import React from 'react';
import axios from 'axios';
import moment, { now } from 'moment';
import BlockUi from 'react-block-ui';
import { Dialog } from 'primereact/components/dialog/Dialog';
import { Button } from 'primereact/components/button/Button';
import Field from "../Base/Field-v2";
export default class CrearUsuarioExterno extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            tipo_identificacion: '',
            nro_identificacion: '',
            nombres_apellidos: '',
            primer_apellido: '',
            segundo_apellido: '',
            nombres: '',
            fecha_nacimiento: '',
            edad: '',
            genero: '',
            nacionalidad: '',
            pais: '',
            estado_civil: '',
            fecha_matrimonio: '',
            telefono: '',
            email: '',
            tiposIdentificacion: [],
            generos: [],
            estados: [],
            tiposNacionalidades: [],
            errores: [],
            formIsValid: false,
            id_colaborador: '',
            estado: false,
            loading: true,
            visible_fecha_matrimonio: false,
            codigo_dactilar: '',
            consIsValid: false,
            errCons: [],
            datosConsumo: [],
            disable_consumo: false,
            disable_sexo: false,
            disable_estado_civil: false,
            disable_nacionalidad: false,
            disable_fecha_matrimonio: false,
            sexo_sugerido: '',
            nacionalidad_sugerida: '',
            estado_civil_sugerido: '',
            viene_registro_civil: false,
            fecha_registro_civil: '',
            grupo_personal: '',
            consumo_bdd: false,
            Tipos: [{ label: "Tercero", value: false }, { label: "Visita", value: true }],
            es_visita: true,
            empresa_tercero:'',
            view_terceros:false
        }
        this.titulo = this.titulo.bind(this);

        this.getFormSelectTipoIdent = this.getFormSelectTipoIdent.bind(this);
        this.getFormSelectGenero = this.getFormSelectGenero.bind(this);
        this.getFormSelectEstadoCivil = this.getFormSelectEstadoCivil.bind(this);
        this.getFormSelectNacionalidades = this.getFormSelectNacionalidades.bind(this);

        this.handleChangeFechaNac = this.handleChangeFechaNac.bind(this);
        this.handleChangeEstadoCivil = this.handleChangeEstadoCivil.bind(this);
        this.handleChangeIden = this.handleChangeIden.bind(this);
        this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);

        this.saveEdad = this.saveEdad.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.Guardar = this.Guardar.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.abrirConfirmacion = this.abrirConfirmacion.bind(this);
        this.cerrarConfirmacion = this.cerrarConfirmacion.bind(this);
        this.onChangeValue= this.onChangeValue.bind(this);
        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);

        this.consultarWSHuella = this.consultarWSHuella.bind(this);
        this.validationConsumo = this.validationConsumo.bind(this);
        this.validationConsumoBDD = this.validationConsumoBDD.bind(this);
        this.procesarDatosConsumo = this.procesarDatosConsumo.bind(this);
        this.convertirimagen = this.convertirimagen.bind(this);
        this.b64toBlob = this.b64toBlob.bind(this);

        this.consultaBDD = this.consultaBDD.bind(this);
        this.procesarDatosBDD = this.procesarDatosBDD.bind(this);

    }

    componentDidMount() {
        this.GetCatalogos();
        // this.titulo();
        // this.GetParametroIdentificacion();
    }

    consultaBDD() {
        // this.state.consumo_bdd == true;
        this.validationConsumoBDD()
        if (this.state.consIsValid == true) {
            this.setState({ loading: true })
            axios.post("/RRHH/Colaboradores/GetCompruebaUsuario", {
                numero: this.state.nro_identificacion,
            })
                .then((response) => {
                    if (response.data == "NO") {
                        // this.consultarWSHuella();
                        this.setState({ loading: false })
                        this.props.warnMessage("No se ha encontrado");
                    } else if (response.data.length != undefined) {
                        this.props.warnMessage(response.data);
                        this.setState({ loading: false })
                    } else {
                        console.log('bdd', response.data, response.data.length)
                        this.procesarDatosBDD(response.data);
                    }

                })
                .catch((error) => {
                    console.log(error);
                    this.setState({ loading: false })
                });
        }

    }

    procesarDatosBDD(data) {
        this.setState({
            disable_consumo: true,
            tipo_identificacion: data.catalogo_tipo_identificacion_id,
            nombres_apellidos: data.nombres_apellidos,
            genero: data.catalogo_genero_id,
            disable_sexo: true,
            fecha_nacimiento: moment(data.fecha_nacimiento).format("YYYY-MM-DD"),
            nacionalidad: data.pais_pais_nacimiento_id,
            disable_nacionalidad: true,
            estado_civil: data.catalogo_estado_civil_id,
            disable_estado_civil: true,
            fecha_matrimonio: data.fecha_matrimonio == null ? '' : moment(data.fecha_matrimonio).format("YYYY-MM-DD"),
            visible_fecha_matrimonio: data.fecha_matrimonio == null ? false : true,
            disable_fecha_matrimonio: data.fecha_matrimonio == null ? false : true,
            primer_apellido: data.primer_apellido,
            segundo_apellido: data.segundo_apellido == null ? '' : data.segundo_apellido,
            nombres: data.nombres,
            codigo_dactilar: data.codigo_dactilar == null ? '' : data.codigo_dactilar,
            pais: data.PaisId,
            loading: false
        })
        this.saveEdad();
    }


    consultarWSHuella() {
        this.validationConsumo()
        if (this.state.consIsValid == true) {
            this.setState({
                nombres_apellidos: '',
                primer_apellido: '',
                segundo_apellido: '',
                nombres: '',
                genero: '',
                fecha_nacimiento: '',
                edad: '',
                pais: '',
                nacionalidad: '',
                estado_civil: '',
                fecha_matrimonio: '',
                disable_consumo: false,
                disable_estado_civil: false,
                disable_fecha_matrimonio: false,
                disable_nacionalidad: false,
                disable_sexo: false,
                loading: true
            })

            axios.post("/RRHH/Colaboradores/ConsumirHuella/", {
                cedula: this.state.nro_identificacion,
                huella_dactilar: this.state.codigo_dactilar
            })
                .then((response) => {
                    console.log(response.data.return)
                    this.setState({ loading: false });
                    if (response.data.return.CodigoError != "000") {
                        this.props.warnMessage("" + response.data.return.Error)

                    } else {
                        this.props.successMessage(response.data.return.Error + " : " + this.state.nro_identificacion)
                        this.procesarDatosConsumo(response.data.return)
                    }
                })
                .catch((error) => {
                    console.log(error);
                    this.setState({ loading: false });
                });
        }

    }

    procesarDatosConsumo(datos) {
        this.setState({ datosConsumo: datos, disable_consumo: true })
        // this.state.nombres_apellidos = datos.Nombre
        var fech_nac = moment(this.state.datosConsumo.FechaNacimiento, "DD-MM-YYYY");
        console.log(fech_nac)
        //Seleccionar el SEXO
        if (datos.Sexo == "MUJER") {
            var sexo = this.state.generos.filter(c => c.codigo == "MUJ");
            this.setState({ genero: sexo[0].Id, disable_sexo: true })
        } else if (datos.Sexo == "HOMBRE") {
            var sexo = this.state.generos.filter(c => c.codigo == "VAR");
            this.setState({ genero: sexo[0].Id, disable_sexo: true })
        }
        else {
            this.setState({ sexo_sugerido: datos.Sexo, disable_sexo: false, genero: '' })
        }
        //Seleccionar el ESTADO CIVIL
        if (datos.EstadoCivil == "CASADO") {
            var fecha_m = moment(this.state.datosConsumo.FechaMatrimonio, "DD-MM-YYYY");
            var estado = this.state.estados.filter(c => c.codigo == "CAS");
            this.setState({
                estado_civil: estado[0].Id,
                disable_estado_civil: true,
                fecha_matrimonio: this.state.datosConsumo.FechaMatrimonio == "" ? '' : moment(fecha_m).format("YYYY-MM-DD"),
                visible_fecha_matrimonio: true,
                disable_fecha_matrimonio: this.state.datosConsumo.FechaMatrimonio == "" ? false : true
            })
        } else
            if (datos.EstadoCivil == "DIVORCIADO") {
                var estado = this.state.estados.filter(c => c.codigo == "DIV");
                this.setState({ estado_civil: estado[0].Id, disable_estado_civil: true })
            } else if (datos.EstadoCivil == "SOLTERO") {
                var estado = this.state.estados.filter(c => c.codigo == "SOL");
                this.setState({ estado_civil: estado[0].Id, disable_estado_civil: true })
            } else {
                this.setState({ estado_civil_sugerido: datos.EstadoCivil, disable_estado_civil: false, estado_civil: '' })
            }

        //Seleccionar NACIONALIDAD
        if (datos.Nacionalidad == "ECUATORIANA") {
            var n = this.state.tiposNacionalidades.filter(c => c.codigo == "239");
            this.setState({ nacionalidad: n[0].Id, disable_nacionalidad: true })
        } else {
            this.setState({ nacionalidad_sugerida: datos.Nacionalidad, disable_nacionalidad: false, nacionalidad: '' })
        }

        this.setState({
            nombres_apellidos: this.state.datosConsumo.Nombre,
            fecha_nacimiento: moment(fech_nac).format("YYYY-MM-DD"),
            viene_registro_civil: true,
            fecha_registro_civil: moment().format("YYYY-MM-DD HH:mm:ss"),
            loading: false
        })
        this.saveEdad();
    }

    validationConsumo() {
        let errors = {};
        this.state.consIsValid = true;

        if (!this.state.tipo_identificacion) {
            this.state.consIsValid = false;
            errors["tipo_identificacion"] = "El campo Tipo de Identificación es obligatorio.";
        } else {
            var catalogo = this.state.tiposIdentificacion.filter(c => c.Id == Number.parseInt(this.state.tipo_identificacion));
            console.log('catalogo', catalogo)
            if (catalogo[0].codigo != 'CEDULA') {
                this.state.consIsValid = false;
                errors["tipo_identificacion"] = "Consulta se realiza con CEDULA";
            }
        }
        if (!this.state.nro_identificacion) {
            this.state.consIsValid = false;
            errors["nro_identificacion"] = "El campo No. de Identificación es obligatorio.";
        } else if (catalogo != null && catalogo[0].codigo == 'CEDULA') {


            console.log('lenght', this.state.nro_identificacion.length)
            var cedula_valida = this.props.validarCedula(this.state.nro_identificacion);
            if (!cedula_valida) {
                this.state.consIsValid = false;
                errors["nro_identificacion"] = "No. de Identificación es inválido.";
            }
            if (!isFinite(this.state.nro_identificacion)) {
                this.state.consIsValid = false;
                errors["nro_identificacion"] = "El campo permite solo ingreso numérico";
            }
            if (this.state.nro_identificacion.length != 10) {
                this.state.consIsValid = false;
                errors["nro_identificacion"] = "El campo debe tener 10 dígitos";
            }

        }
        if (!this.state.codigo_dactilar) {
            this.state.consIsValid = false;
            errors["codigo_dactilar"] = "El campo Código Dactilar es obligatorio.";
        } else {
            if (this.state.codigo_dactilar.length < 6 || this.state.codigo_dactilar.length > 10) {
                this.state.consIsValid = false;
                errors["codigo_dactilar"] = "El campo debe tener entre seis y diez dígitos";
            }
        }

        this.setState({ errCons: errors });
    }

    validationConsumoBDD() {
        console.log('BDD validation')
        let errors = {};
        this.state.consIsValid = true;

        if (!this.state.tipo_identificacion) {
            this.state.consIsValid = false;
            errors["tipo_identificacion"] = "El campo Tipo de Identificación es obligatorio.";
        } else {
            var catalogo = this.state.tiposIdentificacion.filter(c => c.Id == Number.parseInt(this.state.tipo_identificacion));
            console.log('catalogo', catalogo)
        }
        if (!this.state.nro_identificacion) {
            this.state.consIsValid = false;
            errors["nro_identificacion"] = "El campo No. de Identificación es obligatorio.";
        } else if (catalogo != null && catalogo[0].codigo == 'CEDULA') {
            console.log('lenght', this.state.nro_identificacion.length)
            var cedula_valida = this.props.validarCedula(this.state.nro_identificacion);
            if (!cedula_valida) {
                this.state.consIsValid = false;
                errors["nro_identificacion"] = "No. de Identificación es inválido.";
            }
            if (!isFinite(this.state.nro_identificacion)) {
                this.state.consIsValid = false;
                errors["nro_identificacion"] = "El campo permite solo ingreso numérico";
            }
            if (this.state.nro_identificacion.length != 10) {
                this.state.consIsValid = false;
                errors["nro_identificacion"] = "El campo debe tener 10 dígitos";
            }

        }

        this.setState({ errCons: errors });
    }

    convertirimagen(binary) {
        if (binary != null) {
            return <img src={`data:image/jpeg;base64,${binary}`} height="140" width="140" />
        } else {

            return ""
        }
    }


    render() {
        const footer = (
            <div>
                <Button label="Yes" icon="pi pi-check" onClick={() => this.Guardar()} />
                <Button label="No" icon="pi pi-times" onClick={() => this.cerrarConfirmacion()} className="p-button-secondary" />
            </div>
        );
        return (
            <div>
                <BlockUi tag="div" blocking={this.state.loading}>
                    {/* <form onSubmit={this.handleSubmit}> */}
                    <div className="row">
                        <div className="col">
                            <div className="row">
                                <div className="col-xs-10 col-md-10" style={{ display: 'inline-block' }}>
                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="label">* Tipo de Identificación: </label>
                                                <select value={this.state.tipo_identificacion} onChange={this.handleChangeIden} className="form-control" name="tipo_identificacion" required>
                                                    <option value="">Seleccione...</option>
                                                    {this.getFormSelectTipoIdent()}
                                                </select>
                                                <span style={{ color: "red" }}>{this.state.errores["tipo_identificacion"]}</span>
                                                <span style={{ color: "red" }}>{this.state.errCons["tipo_identificacion"]}</span>
                                            </div>
                                        </div>
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="observacion">* No. de Identificación: </label>
                                                <input type="text" id="nro_identificacion" className="form-control" value={this.state.nro_identificacion} onChange={this.handleChangeUpperCase} onBlur={()=>this.validarColaboradorExistente()} name="nro_identificacion" required />
                                                <span style={{ color: "red" }}>{this.state.errores["nro_identificacion"]}</span>
                                                <span style={{ color: "red" }}>{this.state.errCons["nro_identificacion"]}</span>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="row">

                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="codigo_dactilar">Código Dactilar: </label><br />
                                                <input type="text" id="codigo_dactilar" className="form-control" value={this.state.codigo_dactilar} onChange={this.handleChangeUpperCase} name="codigo_dactilar" style={{ width: '78%', display: 'inline' }} />
                                                <button type="button" className="btn btn-outline-primary" onClick={this.consultaBDD} style={{ marginTop: '-1%' }}>BDD</button>
                                                <button type="button" className="btn btn-outline-primary" onClick={this.consultarWSHuella} style={{ marginTop: '-1%' }}>WS</button>
                                                <span style={{ color: "red" }}>{this.state.errores["codigo_dactilar"]}</span>
                                                <span style={{ color: "red" }}>{this.state.errCons["codigo_dactilar"]}</span>
                                            </div>
                                        </div>
                                        {/* <div className="col">
                                    <div className="form-group">
                                        <button type="button" className="btn btn-outline-primary" onClick={this.consultaBDD} style={{ marginTop: '6%' }}>BDD</button>
                                        <button type="button" className="btn btn-outline-primary" onClick={this.consultarWSHuella} style={{ marginTop: '6%', marginLeft: '3px' }}>WS</button>
                                    </div>
                                </div> */}
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="nombres_apellidos">Apellidos Nombres: </label>
                                                <input type="text" id="nombres_apellidos" className="form-control" value={this.state.nombres_apellidos} onChange={this.handleChangeUpperCase} name="nombres_apellidos" disabled />
                                                <span style={{ color: "red" }}>{this.state.errores["nombres_apellidos"]}</span>
                                            </div>
                                        </div>
                                    </div>


                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="primer_apellido">* Primer Apellido: </label>
                                                <input type="text" id="primer_apellido" className="form-control" value={this.state.primer_apellido} onChange={this.handleChangeUpperCase} name="primer_apellido" />
                                                <span style={{ color: "red" }}>{this.state.errores["primer_apellido"]}</span>
                                            </div>
                                        </div>
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="segundo_apellido">Segundo Apellido: </label>
                                                <input type="text" id="segundo_apellido" className="form-control" value={this.state.segundo_apellido} onChange={this.handleChangeUpperCase} name="segundo_apellido" />
                                                <span style={{ color: "red" }}>{this.state.errores["segundo_apellido"]}</span>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="observacion">* Nombres: </label>
                                                <input type="text" id="nombres" className="form-control" value={this.state.nombres} onChange={this.handleChangeUpperCase} name="nombres" />
                                                <span style={{ color: "red" }}>{this.state.errores["nombres"]}</span>
                                            </div>
                                        </div>
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="label">* Género: </label><span style={{ color: "red" }}> {this.state.sexo_sugerido}</span>
                                                <select value={this.state.genero} onChange={this.handleChange} className="form-control" name="genero" disabled={this.state.disable_sexo}>
                                                    <option value="">Seleccione...</option>
                                                    {this.getFormSelectGenero()}
                                                </select>
                                                <span style={{ color: "red" }}>{this.state.errores["genero"]}</span>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="horas">* Fecha de Nacimiento: </label>
                                                <input type="date" id="fecha_nacimiento" className="form-control" value={this.state.fecha_nacimiento} onChange={this.handleChangeFechaNac} name="fecha_nacimiento" disabled={this.state.disable_consumo} />
                                                <span style={{ color: "red" }}>{this.state.errores["fecha_nacimiento"]}</span>
                                            </div>
                                        </div>
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="observacion">* Edad: </label>
                                                <input type="text" id="edad" disabled="disabled" className="form-control" value={this.state.edad} onChange={this.handleChange} name="edad" />
                                            </div>
                                        </div>
                                    </div>

                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="pais">* País de Nacimiento: </label>
                                                <select value={this.state.pais} onChange={this.handleChange} className="form-control" name="pais">
                                                    <option value="">Seleccione...</option>
                                                    {this.props.getFormSelectNacionalidad()}
                                                </select>
                                                <span style={{ color: "red" }}>{this.state.errores["pais"]}</span>
                                            </div>
                                        </div>
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="label">* Nacionalidad: </label><span style={{ color: "red" }}> {this.state.nacionalidad_sugerida}</span>
                                                <select value={this.state.nacionalidad} onChange={this.handleChange} className="form-control" name="nacionalidad" disabled={this.state.disable_nacionalidad}>
                                                    <option value="">Seleccione...</option>
                                                    {this.getFormSelectNacionalidades()}
                                                </select>
                                                <span style={{ color: "red" }}>{this.state.errores["nacionalidad"]}</span>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="estado_civil">* Estado Civil: </label><span style={{ color: "red" }}> {this.state.estado_civil_sugerido}</span>
                                                <select value={this.state.estado_civil} onChange={this.handleChangeEstadoCivil} className="form-control" name="estado_civil" disabled={this.state.disable_estado_civil}>
                                                    <option value="">Seleccione...</option>
                                                    {this.getFormSelectEstadoCivil()}
                                                </select>
                                                <span style={{ color: "red" }}>{this.state.errores["estado_civil"]}</span>
                                            </div>
                                        </div>
                                        <div className="col" style={{ visibility: this.state.visible_fecha_matrimonio == true ? 'visible' : 'hidden' }}>
                                            <div className="form-group">
                                                <label htmlFor="fecha_matrimonio">Fecha de Matrimonio: </label>
                                                <input type="date" id="fecha_matrimonio" className="form-control" value={this.state.fecha_matrimonio} onChange={this.handleChange} name="fecha_matrimonio" disabled={this.state.disable_fecha_matrimonio} />
                                                <span style={{ color: "red" }}>{this.state.errores["fecha_matrimonio"]}</span>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="telefono">* Teléfono: </label>
                                                <input type="text" id="telefono" className="form-control" value={this.state.telefono} onChange={this.handleChange} name="telefono" />
                                                <span style={{ color: "red" }}>{this.state.errores["telefono"]}</span>
                                            </div>
                                        </div>
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="email">* Correo Electrónico: </label>
                                                <input type="text" id="email" className="form-control" value={this.state.email} onChange={this.handleChange} name="email" />
                                                <span style={{ color: "red" }}>{this.state.errores["email"]}</span>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="row">
                                        <div className="col">
                                            <Field
                                                name="es_visita"
                                                required
                                                value={this.state.es_visita}
                                                label="Tipo Usuario Externo"
                                                options={this.state.Tipos}
                                                type={"select"}
                                                filter={true}
                                                onChange={this.onChangeValue}
                                                readOnly={false}
                                                placeholder="Seleccione.."
                                                filterPlaceholder="Seleccione.."
                                            />
                                        </div>
                                        <div className="col">
                                            {this.state.view_terceros &&(
                                        <Field
                        name="empresa_tercero"
                        label="Empresa"
                        
                        edit={true}
                        readOnly={false}
                        value={this.state.empresa_tercero}
                        onChange={this.onChangeValue}
                     
                      />)}
                                        </div>
                                    </div>
                                    <br />
                                    <div className="form-group">
                                        <div className="col">
                                            <button type="button" onClick={this.abrirConfirmacion} className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                            <button onClick={() => this.props.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>

                                        </div>
                                    </div>
                                </div>
                                <div style={{ display: 'inline-block' }}>
                                    {this.convertirimagen(this.state.datosConsumo.Fotografia)}
                                </div>
                                <Dialog header="Mensaje de Confirmación" visible={this.state.estado} footer={footer} width="350px" minY={70} onHide={this.cerrarConfirmacion} maximizable={true}>
                                    Está seguro de guardar la información registrada?.
                            </Dialog>
                            </div>
                        </div>
                    </div>
                    {/* </form> */}
                </BlockUi>
            </div>
        )
    }
    onChangeValue = (name, value) => {
        console.log(name);

        if(name=="es_visita"){
            console.log(value)
            if(!value){
                this.setState({view_terceros:true})
            }else{

                this.setState({view_terceros:false})
            }


        }
        this.setState({
          [name]: value
        });
      };
    
    validarColaboradorExistente=()=>{
        this.setState({ loading: true })
        axios.post("/RRHH/Colaboradores/GetColaboradorExistente/", {
        numero_identificacion:this.state.nro_identificacion
        }).then((response)=>{
        console.log('info',response.data);
        if(response.data!=null && response.data.estado){
            this.setState({ loading: false })
            abp.notify.error("No se puede generar un Colaborador Externo con el número de identificación: "+this.state.numero_identificacion+". Existe un Colaborador interno en estado: "+response.data.estado, "Aviso");
        }
        this.setState({ loading: false })
        }).catch((error)=>{
            console.log(error);
            this.setState({ loading: false })
        })
    }

    titulo() {
        document.getElementById("page_title").remove()
        document.getElementById("btntoolbar").remove()

        var a = document.createElement("i");
        a.setAttribute("class", "fa fa-unlock-alt");
        // a.innerHTML = ' Registro Ficha de Colaborador Externo';
        document.getElementsByClassName("card-header")[0].appendChild(a);

        var p = document.createElement("p");
        p.innerHTML = ' Administración Ficha de Colaborador Externo';
        p.style.display = 'inline-block';
        document.getElementsByClassName("card-header")[0].appendChild(p);
    }

    abrirConfirmacion() {

        this.handleValidation();
        if (this.state.formIsValid == true) {
            this.setState({ estado: true });
        } else {
            this.props.warnMessage("Se ha encontrado errores, por favor revisar el formulario");
        }

    }

    cerrarConfirmacion() {
        this.setState({ estado: false });
    }


    handleValidation() {
        let errors = {};
        this.state.formIsValid = true;


        if (!this.state.tipo_identificacion) {
            this.state.formIsValid = false;
            errors["tipo_identificacion"] = "El campo Tipo de Identificación es obligatorio.";
        } else {
            var catalogo = this.state.tiposIdentificacion.filter(c => c.Id == Number.parseInt(this.state.tipo_identificacion));
        }
        if (!this.state.nro_identificacion) {
            this.state.formIsValid = false;
            errors["nro_identificacion"] = "El campo No. de Identificación es obligatorio.";
        } else if (catalogo != null) {
            var cedula_valida = this.props.validarCedula(this.state.nro_identificacion);
            console.log('xxx', cedula_valida);
            // console.log(this.state.nro_identificacion.length);

            if (catalogo[0].codigo == 'CEDULA' && !cedula_valida) {
                if (!isFinite(this.state.nro_identificacion)) {
                    this.state.formIsValid = false;
                    errors["nro_identificacion"] = "El campo permite solo ingreso numérico";
                }
                if (this.state.nro_identificacion.length != 10) {
                    this.state.consIsValid = false;
                    errors["nro_identificacion"] = "El campo debe tener 10 dígitos";
                }
                this.state.formIsValid = false;
                errors["nro_identificacion"] = "No. de Identificación es inválido.";
            } else if (catalogo[0].codigo == 'RUC' && (this.state.nro_identificacion.length != 13 || !cedula_valida)) {
                if (!isFinite(this.state.nro_identificacion)) {
                    this.state.formIsValid = false;
                    errors["nro_identificacion"] = "El campo permite solo ingreso numérico";
                }
                this.state.formIsValid = false;
                errors["nro_identificacion"] = "No. de Identificación es inválido.";
            } else if (catalogo[0].codigo == 'PASAPORTE' && this.state.nro_identificacion.length > 22) {
                this.state.formIsValid = false;
                errors["nro_identificacion"] = "No. de Identificación es inválido.";
            }

        }

        if (!this.state.primer_apellido) {
            this.state.formIsValid = false;
            errors["primer_apellido"] = "El campo Primer Apellido es obligatorio.";
        }
        if (!this.state.nombres) {
            this.state.formIsValid = false;
            errors["nombres"] = "El campo Nombres es obligatorio.";
        }
        if (this.state.primer_apellido && this.state.nombres && this.state.nombres_apellidos) {
            this.state.primer_apellido = this.state.primer_apellido.toUpperCase();
            this.state.segundo_apellido = this.state.segundo_apellido.toUpperCase();
            this.state.nombres = this.state.nombres.toUpperCase();
            var completo = (this.state.primer_apellido + this.state.segundo_apellido + this.state.nombres).toUpperCase();
            var espacios = completo.replace(/ /g, "");
            console.log(espacios)
            if (this.state.nombres_apellidos.replace(/ /g, "") != espacios) {
                this.state.formIsValid = false;
                this.props.warnMessage('Apellidos o nombres no coinciden con datos de Registro Civil')
            }
        }
        if (!this.state.nombres_apellidos) {
            if (this.state.primer_apellido && this.state.nombres && this.state.segundo_apellido) {
                this.state.nombres_apellidos = (this.state.primer_apellido + " " + this.state.segundo_apellido + " " + this.state.nombres).toUpperCase();
            } else if (this.state.primer_apellido && this.state.nombres && this.state.segundo_apellido == '') {
                this.state.nombres_apellidos = (this.state.primer_apellido + " " + this.state.nombres).toUpperCase();
            }
        }
        console.log(this.state.nombres_apellidos.replace(/ /g, ""))
        if (!this.state.genero) {
            this.state.formIsValid = false;
            errors["genero"] = "El campo Género es obligatorio.";
        }
        if (!this.state.fecha_nacimiento) {
            this.state.formIsValid = false;
            errors["fecha_nacimiento"] = "El campo Fecha de Nacimiento es obligatorio.";
        } else {
            if (this.state.edad < 18) {
                this.state.formIsValid = false;
                errors["fecha_nacimiento"] = "El usuario no puede ser menor de 18 años";
            }
        }
        if (!this.state.pais) {
            this.state.formIsValid = false;
            errors["pais"] = "El campo País de Nacimiento es obligatorio.";
        }
        if (!this.state.nacionalidad) {
            this.state.formIsValid = false;
            errors["nacionalidad"] = "El campo Nacionalidad es obligatorio.";
        }
        if (this.state.estado_civil) {
            var catEstadoCivil = this.state.estados.filter(c => c.Id == Number.parseInt(this.state.estado_civil));
            console.log('catEstadoCivil', catEstadoCivil)
            var today = moment().format("YYYY-MM-DD");
            if (catEstadoCivil[0].descripcion == 'CASADO' && !this.state.fecha_matrimonio) {
                this.state.formIsValid = false;
                errors["fecha_matrimonio"] = "El campo Fecha de Matrimonio es obligatorio.";
            } else if (catEstadoCivil[0].descripcion == 'CASADO' && this.state.fecha_matrimonio > today) {
                this.state.formIsValid = false;
                errors["fecha_matrimonio"] = "Fecha de Matrimonio no puede ser mayor a fecha actual.";
            }
        } else {
            this.state.formIsValid = false;
            errors["estado_civil"] = "El campo Estado Civil es obligatorio.";
        }

        if (!this.state.telefono) {
            this.state.formIsValid = false;
            errors["telefono"] = "El campo Teléfono es obligatorio.";
        }
        else {
            if (this.state.telefono.length > 10) {
                this.state.formIsValid = false;
                errors["telefono"] = "El campo no puede tener más de diez dígitos.";
            }
            if (!isFinite(this.state.telefono)) {
                this.state.formIsValid = false;
                errors["telefono"] = "El campo permite solo ingreso numérico";
            }
        }
        if (!this.state.email) {
            this.state.formIsValid = false;
            errors["email"] = "El campo Correo Electrónico es obligatorio.";
        } else {
            let lastAtPos = this.state.email.lastIndexOf('@');
            let lastDotPos = this.state.email.lastIndexOf('.');

            if (!(lastAtPos < lastDotPos && lastAtPos > 0 && this.state.email.indexOf('@@') == -1 && lastDotPos > 2 && (this.state.email.length - lastDotPos) > 2)) {
                this.state.formIsValid = false;
                errors["email"] = "Correo Electrónico ingresado incorrecto";
            }
        }

        this.setState({ errores: errors });

        return this.state.formIsValid;
    }

    Guardar() {
        this.setState({ loading: true });
        axios.post("/RRHH/Colaboradores/CreateUsuarioExterno/", {
            catalogo_tipo_identificacion_id: this.state.tipo_identificacion,
            numero_identificacion: this.state.nro_identificacion,
            primer_apellido: this.state.primer_apellido,
            segundo_apellido: this.state.segundo_apellido,
            nombres: this.state.nombres,
            fecha_nacimiento: this.state.fecha_nacimiento,
            catalogo_genero_id: this.state.genero,
            PaisId: this.state.pais,
            pais_pais_nacimiento_id: this.state.nacionalidad,
            catalogo_estado_civil_id: this.state.estado_civil,
            fecha_matrimonio: this.state.fecha_matrimonio,
            estado: "ACTIVO",
            nombres_apellidos: this.state.nombres_apellidos,
            codigo_dactilar: this.state.codigo_dactilar,
            viene_registro_civil: this.state.viene_registro_civil,
            fecha_registro_civil: this.state.fecha_registro_civil,
            telefono: this.state.telefono,
            email: this.state.email,
            es_visita:this.state.es_visita,
            empresa_tercero:this.state.empresa_tercero
        })
            .then((response) => {
                console.log('Guardar', response.data)
                if (response.data == "Existe") {
                    this.setState({ loading: false });
                    this.props.warnMessage("No. de Identificación ya existe");
                } else if (Number.parseInt(response.data) > 0) {
                    this.cerrarConfirmacion();
                    if (this.state.datosConsumo.Fotografia != null) {
                        this.subirArchivo(Number.parseInt(response.data));
                    } else {
                        this.setState({ loading: false });
                        this.props.successMessage("Usuario Externo Guardado!");
                        setTimeout(
                            function () {
                                this.props.Regresar()
                            }.bind(this), 2000
                        );
                    }

                } else {
                    this.props.warnMessage(response.data);
                }
                this.setState({ loading: false });

            })
            .catch((error) => {
                console.log(error);
                this.props.warnMessage("Algo salió mal.");
                this.setState({ loading: false });
            });
    }

    subirArchivo(id) {
        var file = this.b64toBlob(this.state.datosConsumo.Fotografia, 'image/png');
        console.log(file);
        const formData = new FormData();
        formData.append('idColaborador', id)
        formData.append('UploadedFile', file)
        const config = { headers: { 'content-type': 'multipart/form-data' } }

        axios.post("/RRHH/Colaboradores/CreateArchivoFotografia/", formData, config)
            .then((response) => {
                console.log('subirArchivo', response.data)
                this.setState({ loading: false });
                if (response.data == "OK") {
                    this.props.successMessage("Usuario Externo Guardado!");
                    setTimeout(
                        function () {
                            this.props.Regresar()
                        }.bind(this), 2000
                    );
                } else {
                    this.props.warnMessage("Algo salió mal.");
                }
            })
            .catch((error) => {
                this.setState({ loading: false });
                this.props.warnMessage("Algo salio mal!");
            });
    }

    b64toBlob(b64Data, contentType) {
        var sliceSize = 512;

        var byteCharacters = atob(b64Data);
        var byteArrays = [];

        for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            var slice = byteCharacters.slice(offset, offset + sliceSize);

            var byteNumbers = new Array(slice.length);
            for (var i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }

            var byteArray = new Uint8Array(byteNumbers);

            byteArrays.push(byteArray);
        }

        var blob = new Blob(byteArrays, { type: contentType });
        return blob;
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['TIPOINDENTIFICACION', 'GENERO', 'ESTADOCIVIL', 'NACIONALIDADES'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    cargarCatalogos(data) {
        this.setState({ loading: false })
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
            switch (codigoCatalogo) {
                case 'TIPOINDENTIFICACION':
                    this.setState({ tiposIdentificacion: catalogo })
                    this.getFormSelectTipoIdent();
                    return;
                case 'NACIONALIDADES':
                    this.setState({ tiposNacionalidades: catalogo })
                    this.getFormSelectNacionalidades();
                    return;
                case 'GENERO':
                    this.setState({ generos: catalogo })
                    this.getFormSelectGenero();
                    return;
                case 'ESTADOCIVIL':
                    this.setState({ estados: catalogo })
                    this.getFormSelectEstadoCivil()
                    return;
                default:
                    console.log(codigoCatalogo)
                    return;
            }
        });


    }

    getFormSelectNacionalidades() {
        return (
            this.state.tiposNacionalidades.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectTipoIdent() {
        return (
            this.state.tiposIdentificacion.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectGenero() {
        return (
            this.state.generos.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectEstadoCivil() {
        return (
            this.state.estados.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    handleChangeFechaNac(event) {
        this.setState({ [event.target.name]: event.target.value }, this.saveEdad);
    }

    saveEdad() {
        var age = moment().diff(this.state.fecha_nacimiento, 'years');
        if (!age) {
            age = '';
        }
        this.setState({ edad: age })
    }

    handleChange(event) {
      
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeUpperCase(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }

    
 
    handleChangeIden(event) {
        this.setState({
            [event.target.name]: event.target.value,
            datosConsumo: [],
            nro_identificacion: '',
            codigo_dactilar: '',
            nombres_apellidos: '',
            primer_apellido: '',
            segundo_apellido: '',
            nombres: '',
            genero: '',
            etnia: '',
            fecha_nacimiento: '',
            edad: '',
            pais: '',
            nacionalidad: '',
            estado_civil: '',
            fecha_matrimonio: '',
            disable_consumo: false,
            disable_estado_civil: false,
            disable_fecha_matrimonio: false,
            disable_nacionalidad: false,
            disable_sexo: false,
        });

    }

    handleChangeEstadoCivil(event) {
        this.setState({ [event.target.name]: event.target.value });
        var estado = this.state.estados.filter(c => c.Id == Number.parseInt(event.target.value));
        console.log(estado)
        if (estado[0].descripcion == "CASADO") {
            this.setState({ visible_fecha_matrimonio: true });
        } else {
            this.setState({ visible_fecha_matrimonio: false });
        }

    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

}