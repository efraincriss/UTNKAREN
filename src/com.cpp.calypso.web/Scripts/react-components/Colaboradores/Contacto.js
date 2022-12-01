import React from 'react';
import axios from 'axios';
import BlockUi from 'react-block-ui';

export default class Contacto extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            amazonia: '',
            pais: '',
            provincia: '',
            canton: '',
            parroquia: '',
            detalle_parroquia: '',
            comunidad: '',
            detalle_comunidad: '',
            calle_principal: '',
            numero: '',
            codigo_postal: '',
            telefono: '',
            celular: '',
            email: '',
            pais_lab: '',
            provincia_lab: '',
            canton_lab: '',
            parroquia_lab: '',
            comunidad_lab: '',
            errores: [],
            formIsValid: '',
            provinciasR: [],
            provinciasL: [],
            cantonesR: [],
            cantonesL: [],
            parroquiasR: [],
            parroquiasL: [],
            comunidadesR: [],
            comunidadesL: [],
            loading: false,
        }

        this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);

        this.getFormSelectCodigoPostal = this.getFormSelectCodigoPostal.bind(this);
        this.GetProvinciasR = this.GetProvinciasR.bind(this);
        this.getFormSelectProvinciasR = this.getFormSelectProvinciasR.bind(this);
        this.GetProvinciasL = this.GetProvinciasL.bind(this);
        this.getFormSelectProvinciasL = this.getFormSelectProvinciasL.bind(this);
        this.GetCantonR = this.GetCantonR.bind(this);
        this.getFormSelectCantonR = this.getFormSelectCantonR.bind(this);
        this.GetCantonL = this.GetCantonL.bind(this);
        this.getFormSelectCantonL = this.getFormSelectCantonL.bind(this);
        this.GetParroquiasR = this.GetParroquiasR.bind(this);
        this.getFormSelectParroquiasR = this.getFormSelectParroquiasR.bind(this);
        this.GetParroquiasL = this.GetParroquiasL.bind(this);
        this.getFormSelectParroquiasL = this.getFormSelectParroquiasL.bind(this);
        this.GetComunidadR = this.GetComunidadR.bind(this);
        this.getFormSelectComunidadR = this.getFormSelectComunidadR.bind(this);
        this.GetComunidadL = this.GetComunidadL.bind(this);
        this.getFormSelectComunidadL = this.getFormSelectComunidadL.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleChangeSelect = this.handleChangeSelect.bind(this);
        this.handleChangeR = this.handleChangeR.bind(this);
        this.handleChangeL = this.handleChangeL.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);


    }

    componentDidMount() {
    }

    render() {
        return (
            <div className="row">
                <div className="col-xs-12 col-md-12">
                    <form onSubmit={this.handleSubmit}>

                        <div className="row">
                            <div className="col">
                                <div className="row">
                                    <div className="col">
                                        <h5>Ubicación Geográfica de Residencia:</h5>
                                    </div>
                                    <div className="col">
                                        <div className="form-group checkbox" /*style={{ display: 'inline-flex', marginTop: '32px' }}*/>
                                            <label htmlFor="amazonia" style={{ width: '285px' }}><b>Es Región Amazónica?:</b> </label>
                                            <input type="checkbox" id="amazonia" className="form-control" checked={this.state.amazonia} onChange={this.handleInputChange} name="amazonia" style={{ marginTop: '-22px', marginLeft: '-20%' }} disabled />
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="pais">* Pais: </label>
                                            <select value={this.state.pais} onChange={this.handleChangeSelect} onClick={this.handleChangeR} className="form-control" name="pais">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectNacionalidad()}
                                            </select>
                                            {this.state.errores["pais"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="provincia">* Región: </label>
                                            <select value={this.state.provincia} onChange={this.handleChangeSelect} onClick={this.handleChangeR} className="form-control" name="provincia">
                                                <option value="">Seleccione...</option>
                                                {this.getFormSelectProvinciasR()}
                                            </select>
                                            {this.state.errores["provincia"]}
                                        </div>
                                    </div>
                                    {/* <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="canton">* Cantón/Ciudad/Subregión: </label>
                                            <select value={this.state.canton} onChange={this.handleChangeSelect} onClick={this.handleChangeR} className="form-control" name="canton">
                                                <option value="">Seleccione...</option>
                                                {this.getFormSelectCantonR()}
                                            </select>
                                            <span style={{ color: "red" }}>{this.state.errores["canton"]}</span>
                                        </div>
                                    </div> */}
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="parroquia">* Subregión: </label>
                                            <select value={this.state.parroquia} onChange={this.handleChangeSelect} onClick={this.handleChangeR} className="form-control" name="parroquia">
                                                <option value="">Seleccione...</option>
                                                {this.getFormSelectParroquiasR()}
                                            </select>
                                            {this.state.errores["parroquia"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="detalle_parroquia">Detalle Parroquia: </label>
                                            <input type="text" id="detalle_parroquia" className="form-control" value={this.state.detalle_parroquia} onChange={this.handleChange} name="detalle_parroquia" />
                                            {this.state.errores["detalle_parroquia"]}
                                        </div>
                                    </div>
                                </div>

                                <div className="row" style={{ visibility: this.state.visibleComunidadR == true ? 'visible' : 'hidden' }}>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="comunidad">Comunidad: </label>
                                            <select value={this.state.comunidad} onChange={this.handleChange} className="form-control" name="comunidad">
                                                <option value="">Seleccione...</option>
                                                {this.getFormSelectComunidadR()}
                                            </select>
                                            {this.state.errores["comunidad"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="detalle_comunidad">Detalle Comunidad: </label>
                                            <input type="text" id="detalle_comunidad" className="form-control" value={this.state.detalle_comunidad} onChange={this.handleChange} name="detalle_comunidad" />
                                            {this.state.errores["detalle_comunidad"]}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <h5 style={{ marginTop: this.state.visibleComunidadR == true ? '0' : '-6.5%' }}>Domicilio:</h5>
                        <div className="row">
                            <div className="col">
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="calle_principal">* Calle y número: </label>
                                            <input type="text" id="calle_principal" className="form-control" value={this.state.calle_principal} onChange={this.handleChangeUpperCase} name="calle_principal" />
                                            {this.state.errores["calle_principal"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="numero">* Número: </label>
                                            <input type="text" id="numero" className="form-control" value={this.state.numero} onChange={this.handleChangeUpperCase} name="numero" />
                                            {this.state.errores["numero"]}
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="observacion">Teléfono Convencional: </label>
                                            <input type="text" id="telefono" className="form-control" value={this.state.telefono} onChange={this.handleChange} name="telefono" />
                                            {this.state.errores["telefono"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="codigo_postal"><b>Código Postal:</b> </label>
                                            <select value={this.state.codigo_postal} onChange={this.handleChange} className="form-control" name="codigo_postal" disabled>
                                                <option value=""></option>
                                                {this.getFormSelectCodigoPostal()}
                                            </select>
                                            {this.state.errores["codigo_postal"]}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <h5>Contacto Personal:</h5>
                        <div className="row">
                            <div className="col">
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="celular">* Celular: </label>
                                            <input type="text" id="celular" className="form-control" value={this.state.celular} onChange={this.handleChange} name="celular" />
                                            {this.state.errores["celular"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="email">* Email Personal: </label>
                                            <input type="text" id="email" className="form-control" value={this.state.email} onChange={this.handleChange} name="email" />
                                            {this.state.errores["email"]}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        {/* <h5>Ubicación Geográfica Laboral:</h5>
                        <div className="row">
                            <div className="col">
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="pais_lab">* Pais: </label>
                                            <select value={this.state.pais_lab} onChange={this.handleChange} onClick={this.handleChangeL} className="form-control" name="pais_lab">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectNacionalidad()}
                                            </select>
                                            <span style={{ color: "red" }}>{this.state.errores["pais_lab"]}</span>
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="provincia_lab">* Provincia/Región: </label>
                                            <select value={this.state.provincia_lab} onChange={this.handleChange} onClick={this.handleChangeL} className="form-control" name="provincia_lab">
                                                <option value="">Seleccione...</option>
                                                {this.getFormSelectProvinciasL()}
                                            </select>
                                            <span style={{ color: "red" }}>{this.state.errores["provincia_lab"]}</span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="canton_lab">* Cantón/Ciudad/Subregión: </label>
                                            <select value={this.state.canton_lab} onChange={this.handleChange} onClick={this.handleChangeL} className="form-control" name="canton_lab">
                                                <option value="">Seleccione...</option>
                                                {this.getFormSelectCantonL()}
                                            </select>
                                            <span style={{ color: "red" }}>{this.state.errores["canton_lab"]}</span>
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="parroquia_lab">Parroquia: </label>
                                            <select value={this.state.parroquia_lab} onChange={this.handleChange} onClick={this.handleChangeL} className="form-control" name="parroquia_lab">
                                                <option value="">Seleccione...</option>
                                                {this.getFormSelectParroquiasL()}
                                            </select>
                                            <span style={{ color: "red" }}>{this.state.errores["parroquia_lab"]}</span>
                                        </div>
                                    </div>
                                </div>

                                <div className="row" style={{ visibility: this.state.visibleComunidadL == true ? 'visible' : 'hidden' }}>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="comunidad_lab">Comunidad: </label>
                                            <select value={this.state.comunidad_lab} onChange={this.handleChange} className="form-control" name="comunidad_lab">
                                                <option value="">Seleccione...</option>
                                                {this.getFormSelectComunidadL()}
                                            </select>
                                            <span style={{ color: "red" }}>{this.state.errores["comunidad_lab"]}</span>
                                        </div>
                                    </div>
                                    <div className="col">
                                    </div>
                                </div>
                            </div>
                        </div> */}

                        <br />
                        <div className="form-group" /*style={{ marginTop: this.state.visibleComunidadL == true ? '0' : '-6.5%' }}*/>
                            <div className="col">
                                <button onClick={() => this.handleSubmit()} type="button" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                <button onClick={() => this.props.regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                            </div>
                        </div>

                    </form>
                </div>
            </div>
        )
    }


    handleValidation() {
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.pais) {
            this.state.formIsValid = false;
            errors["pais"] = <div className="alert alert-danger">El campo País es obligatorio.</div>;
        }
        if (!this.state.provincia) {
            this.state.formIsValid = false;
            errors["provincia"] = <div className="alert alert-danger">El campo Provincia es obligatorio.</div>;
        }
        if (!this.state.calle_principal) {
            this.state.formIsValid = false;
            errors["calle_principal"] = <div className="alert alert-danger">El campo Calle y Número es obligatorio.</div>;
        }
        if (!this.state.numero) {
            this.state.formIsValid = false;
            errors["numero"] = <div className="alert alert-danger">El campo Número es obligatorio.</div>;
        }
        // if (!this.state.codigo_postal) {
        //     this.state.formIsValid = false;
        //     errors["codigo_postal"] = <div className="alert alert-danger">El campo Código Postal es obligatorio.</div>;
        // }
        if (!this.state.celular) {
            this.state.formIsValid = false;
            errors["celular"] = <div className="alert alert-danger">El campo Celular es obligatorio.</div>;
        } else {
            if (this.state.celular.length > 10) {
                this.state.formIsValid = false;
                errors["celular"] = <div className="alert alert-danger">El campo no puede tener más de diez dígitos.</div>;
            }
            if (!isFinite(this.state.celular)) {
                this.state.formIsValid = false;
                errors["celular"] = <div className="alert alert-danger">El campo permite solo ingreso numérico</div>;
            }
        }
        if (this.state.telefono) {
            if (this.state.telefono.length > 9) {
                this.state.formIsValid = false;
                errors["telefono"] = <div className="alert alert-danger">El campo no puede tener más de nueve dígitos.</div>;
            }
            if (!isFinite(this.state.telefono)) {
                this.state.formIsValid = false;
                errors["telefono"] = <div className="alert alert-danger">El campo permite solo ingreso numérico</div>;
            }
        }
        if (!this.state.email) {
            this.state.formIsValid = false;
            errors["email"] = <div className="alert alert-danger">El campo Email Personal es obligatorio.</div>;
        } else {
            let lastAtPos = this.state.email.lastIndexOf('@');
            let lastDotPos = this.state.email.lastIndexOf('.');

            if (!(lastAtPos < lastDotPos && lastAtPos > 0 && this.state.email.indexOf('@@') == -1 && lastDotPos > 2 && (this.state.email.length - lastDotPos) > 2)) {
                this.state.formIsValid = false;
                errors["email"] = <div className="alert alert-danger">Email ingresado incorrecto</div>;
            }
        }
        // if (!this.state.pais_lab) {
        //     this.state.formIsValid = false;
        //     errors["pais_lab"] = <div className="alert alert-danger">El campo País es obligatorio.</div>;
        // }
        // if (!this.state.provincia_lab) {
        //     this.state.formIsValid = false;
        //     errors["provincia_lab"] = <div className="alert alert-danger">El campo Provincia/Región es obligatorio.</div>;
        // }
        // if (!this.state.canton_lab) {
        //     this.state.formIsValid = false;
        //     errors["canton_lab"] = <div className="alert alert-danger">El campo Cantón/Ciudades/Subregión es obligatorio.</div>;
        // }
        if (!this.state.parroquia) {
            this.state.formIsValid = false;
            errors["parroquia"] = <div className="alert alert-danger">El campo Subregión es obligatorio.</div>;
        }
        // if (!this.state.parroquia_lab) {
        //     this.state.formIsValid = false;
        //     errors["parroquia_lab"] = <div className="alert alert-danger">El campo Parroquia es obligatorio.</div>;
        // }

        this.setState({ errores: errors });
        return this.state.formIsValid;
    }

    handleSubmit() {
        this.handleValidation();
        if (this.state.formIsValid) {
            axios.post("/RRHH/Colaboradores/CreateContactoApi/", {
                ColaboradoresId: this.props.id_colaborador,
                // Id: this.state.contacto.Id,
                calle_principal: this.state.calle_principal,
                numero: this.state.numero,
                codigo_postal: this.state.codigo_postal,
                telefono_convencional: this.state.telefono,
                celular: this.state.celular,
                correo_electronico: this.state.email,
                ComunidadId: this.state.amazonia ? this.state.comunidad : '',
                // comunidad_comunidad_laboral_id: this.state.comunidad_lab,
                detalle_comunidad: this.state.amazonia ? this.state.detalle_comunidad : '',
                ParroquiaId: this.state.parroquia,
                // parroquia_parroquia_laboral_id: this.state.parroquia_lab,
                detalle_parroquia: this.state.detalle_parroquia,
                PaisId: this.state.pais,
                // pais_lab: this.state.pais_lab,
                ProvinciaId: this.state.provincia,
                // provincia_lab: this.state.provincia_lab,
                CiudadId: this.state.canton,
                // canton_lab: this.state.canton_lab,
                amazonica: this.state.amazonia
            })
                .then((response) => {
                    if (response.data > 0) {
                        abp.notify.success("Contacto Guardado!", "Aviso");
                        console.log(response.data);
                        this.props.ContantoInfo(response.config.data, response.data);
                        this.props.Siguiente(2);
                    } else {
                        abp.notify.error('Algo salió mal.', 'Error');
                    }
                })
                .catch((error) => {
                    console.log(error);
                    abp.notify.error('Algo salió mal.', 'Error');
                });
        } else {
            abp.notify.error('Se ha encontrado errores, por favor revisar el formulario', 'Error');
        }

    }

    getFormSelectCodigoPostal() {
        return (
            this.state.parroquiasR.map((item) => {
                return (
                    <option key={Math.random()} value={item.codigo_postal}>{item.codigo_postal}</option>
                )
            })
        );
    }

    GetProvinciasR(id) {
        let p = [];
        this.props.provincias.forEach(e => {
            if (e.PaisId == id) {
                p.push(e);
            }
        });
        this.setState({ provinciasR: p });
        this.getFormSelectProvinciasR()
    }

    getFormSelectProvinciasR() {

        // if (!this.state.amazonia) {
        return (
            this.state.provinciasR.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )

            })
        );
        // } else {
        //     return (
        //         this.state.provinciasR.map((item) => {
        //             if (item.region_amazonica == true) {
        //                 return (
        //                     <option key={Math.random()} value={item.Id}>{item.nombre}</option>
        //                 )
        //             }

        //         })
        //     );
        // }

    }

    GetProvinciasL(id) {
        let p = [];
        this.props.provincias.forEach(e => {
            if (e.PaisId == id) {
                p.push(e);
            }
        });
        this.setState({ provinciasL: p });
        this.getFormSelectProvinciasL()
    }

    getFormSelectProvinciasL() {

        return (
            this.state.provinciasL.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    GetCantonR(id) {
        let p = [];
        this.props.ciudades.forEach(e => {
            if (e.ProvinciaId == id) {
                p.push(e);
            }
        });
        this.setState({ cantonesR: p });
        this.GetParroquiasR(p[0].Id);
    }

    getFormSelectCantonR() {
        return (
            this.state.cantonesR.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    GetCantonL(id) {
        let p = [];
        this.props.ciudades.forEach(e => {
            if (e.ProvinciaId == id) {
                p.push(e);
            }
        });
        this.setState({ cantonesL: p });
        this.getFormSelectCantonL()
    }

    getFormSelectCantonL() {

        return (
            this.state.cantonesL.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    GetParroquiasR(id) {
        let p = [];
        this.props.parroquias.forEach(e => {
            if (e.CiudadId == id) {
                p.push(e);
            }
        });
        // console.log('p', p);
        this.setState({ parroquiasR: p });
        this.getFormSelectParroquiasR()
        this.getFormSelectCodigoPostal()
    }

    getFormSelectParroquiasR() {

        return (
            this.state.parroquiasR.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    GetParroquiasL(id) {
        let p = [];
        this.props.parroquias.forEach(e => {
            if (e.CiudadId == id) {
                p.push(e);
            }
        });
        this.setState({ parroquiasL: p });
        this.getFormSelectParroquiasL()
    }

    getFormSelectParroquiasL() {

        return (
            this.state.parroquiasL.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    GetComunidadR() {
        axios.post("/RRHH/Colaboradores/GetComunidadesApi/", { id: this.state.parroquia })
            .then((response) => {
                // console.log('comunidad', response.data);
                this.setState({ comunidadesR: response.data })
                if (response.data.length > 0) {
                    this.setState({ visibleComunidadR: true })
                    this.getFormSelectComunidadR()
                }
                else {
                    this.setState({ visibleComunidadR: false })
                }
            })
            .catch((error) => {
                console.log(error);
            });
    }

    getFormSelectComunidadR() {
        return (
            this.state.comunidadesR.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    GetComunidadL() {
        axios.post("/RRHH/Colaboradores/GetComunidadesApi/", { id: this.state.parroquia_lab })
            .then((response) => {
                this.setState({ comunidadesL: response.data })
                if (response.data.length > 0) {
                    this.setState({ visibleComunidadL: true })
                    this.getFormSelectComunidadL()
                }
                else {
                    this.setState({ visibleComunidadL: false })
                }

            })
            .catch((error) => {
                console.log(error);
            });
    }

    getFormSelectComunidadL() {
        return (
            this.state.comunidadesL.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeSelect(event) {
        this.setState({ [event.target.name]: event.target.value });
        // console.log([event.target.name], event.target.value)

        switch (event.target.name) {
            case 'pais':
                this.setState({
                    provincia: '',
                    canton: '',
                    parroquia: '',
                    comunidad: '',
                    provinciasR: [],
                    cantonesR: [],
                    parroquiasR: [],
                    comunidadesR: []
                });
                return;
            case 'provincia':
                this.setState({
                    canton: '',
                    parroquia: '',
                    comunidad: '',
                    cantonesR: [],
                    parroquiasR: [],
                    comunidadesR: []
                });
                return;
            case 'canton':
                this.setState({
                    parroquia: '',
                    comunidad: '',
                    parroquiasR: [],
                    comunidadesR: []
                });
                return;
            case 'parroquia':
                this.setState({
                    comunidad: '',
                    comunidadesR: []
                });
                return;
        }
    }

    handleChangeR(event) {

        switch (event.target.name) {
            case 'pais':
                this.state.pais ? this.GetProvinciasR(this.state.pais) : '';
                return;
            case 'provincia':
                this.state.provincia ? this.GetCantonR(this.state.provincia) : '';
                if (event.target.value == '') {
                    this.setState({
                        visibleComunidadR: false,
                        amazonia: false,
                        codigo_postal: ''
                    });
                } else {
                    var p = this.state.provinciasR.filter(c => c.Id == Number.parseInt(this.state.provincia));
                    // console.log(p)
                    if (p != '') {
                        //this.state.codigo_postal = p[0].codigo_postal;
                        if (p[0].region_amazonica == true) {
                            this.setState({
                                visibleComunidadR: true,
                                amazonia: true
                            });
                        } else {
                            this.setState({
                                visibleComunidadR: false,
                                amazonia: false
                            });
                        }
                    }
                }
                return;
            case 'canton':
                this.state.canton ? this.GetParroquiasR(this.state.canton) : '';
                return;
            case 'parroquia':
                this.state.parroquia ? this.GetComunidadR(this.state.parroquia) : '';
                // console.log('parroquia', event.target.value );
                if (event.target.value != '') {
                    var p = this.state.parroquiasR.filter(c => c.Id == Number.parseInt(this.state.parroquia));
                    // console.log('parro', p)
                    if (p != '') {
                        this.state.codigo_postal = p[0].codigo_postal;
                    }

                }
                return;
            default:
                // this.setState({ amazonia: false });
                return;

        }

    }

    handleChangeL(event) {
        // console.log('event', event.target.name)

        switch (event.target.name) {
            case 'pais_lab':
                this.state.pais_lab ? this.GetProvinciasL(this.state.pais_lab) : '';
                return;
            case 'provincia_lab':
                this.state.provincia_lab ? this.GetCantonL(this.state.provincia_lab) : '';
                return;
            case 'canton_lab':
                this.state.canton_lab ? this.GetParroquiasL(this.state.canton_lab) : '';
                return;
            case 'parroquia_lab':
                // this.state.parroquia_lab ? this.GetComunidadL(this.state.parroquia_lab) : '';
                return;
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

    handleChangeUpperCase(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }

}