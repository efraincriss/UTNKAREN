import React, { Component } from 'react';
import BlockUi from 'react-block-ui';


import http from '../Base/HttpService';
import ActionForm from '../Base/ActionForm';
import Field from '../Base/Field';
import config from '../Base/Config';
import wrapForm from '../Base/WrapForm';
import validationRules from '../Base/validationRules';

class ProveedorForm extends Component {

    constructor(props) {
        super(props);

        this.state = {
            data: this.initData(),
            uploadFile: '',
            dataExtra: {
                tipo_identificaciones: [],
                tipo_proveedores: [],
                paises: [],
                provincias: [],
                ciudades: [],
                parroquias: []
            },
            blockingItem: {
                provincias: true,
                ciudades: true,
                parroquias: true
            },
            blocking: true,
            loadDataExtra: false,
            loading: false,
            errors: {}
        };

        this.handleChange = this.handleChange.bind(this);
        this.setData = this.setData.bind(this);

        this.handleSubmit = this.handleSubmit.bind(this);
    }


    componentDidUpdate(prevProps) {
        console.log('ProveedorForm.componentDidUpdate');

        if (!this.state.loadDataExtra && this.props.show && !this.state.loading) {
            //Init
            this.loadDataExtra();
        }

        // Typical usage (don't forget to compare props):
        if (this.props.show && (this.props.entityId !== prevProps.entityId ||
            this.props.show !== prevProps.show)) {
            this.loadData();
        }
    }

    initData() {
        let dataInit = {
            Id: 0,
            tipo_identificacion: 0,
            identificacion: '',
            razon_social: '',
            contacto_id: 0,
            calle_principal: '',
            calle_secundaria: '',
            numero: '',
            referencia: '',
            correo_electronico: '',
            telefono_convencional: '',
            celular: '',
            estado: true,
            estado_nombre: '',
            es_externo: false,
            coordenadas: '',
            usuario: '',
            PaisId: 0,
            ProvinciaId: 0,
            CiudadId: 0,
            ParroquiaId: 0,
            tipo_proveedor_id: 0,
            tipo_proveedor_nombre: '',
            codigo_sap: ''
        };
        return dataInit;
    }

    loadData() {

        this.setState({ blocking: true });

        if (this.props.entityId > 0) {

            let url = '';
            url = `${this.props.urlApiBase}/GetApi/${this.props.entityId}`;

            http.get(url, {})
                .then((response) => {

                    let data = response.data;

                    if (data.success === true) {

                        //Fix 
                        let dataEntity = data.result;
                        this.normalizingData(dataEntity);

                        this.setState({ data: dataEntity });
                        this.loadDataPost(dataEntity);

                    } else {
                        //TODO: 
                        //Presentar errores... 
                        var message = $.fn.responseAjaxErrorToString(data);
                        abp.notify.error(message, 'Error');
                    }

                    this.setState({ blocking: false, errors: {} });

                })
                .catch((error) => {
                    //TODO: Mejorar gestion errores
                    self.setState({ blocking: false });
                    console.log(error);
                });
        } else {

            this.setState({
                data: this.initData(),
                errors: {},
                blocking: false
            });
        }
    }

    normalizingData(dataEntity) {

    }

    getTipoIdentificacion() {
        let url = '';
        url = `/Proveedor/Proveedor/GetTipoIdentificadorApi/`;

        return http.post(url);

    }

    getTipoProveedor() {
        let url = '';
        url = `/Proveedor/Proveedor/SearchByCodeApi/?code=TIPOPROVEEDOR`;

        return http.get(url);
    }

    getPaises() {
        let url = '';
        url = `/Proveedor/Proveedor/SearchPaisesApi/`;
        return http.post(url);
    }

    getProvincias(padreId) {
        if (padreId) {
            let url = '';
            url = `/Proveedor/Proveedor/SearchProvinciasApi`;
            return http.post(url, { id: padreId });
        } else {
            new Promise((resolve, reject) => {
                let data = [];
                resolve(data);
            });
        }
    }

    getCiudades(padreId) {

        if (padreId) {
            let url = '';
            url = `/Proveedor/Proveedor/SearchCantonesApi`;
            return http.post(url, { id: padreId });
        } else {
            new Promise((resolve, reject) => {
                resolve([]);
            });
        }
    }

    getParroquias(padreId) {
        if (padreId) {
            let url = '';
            url = `/Proveedor/Proveedor/SearchParroquiasApi`;
            return http.post(url, { id: padreId });
        } else {
            new Promise((resolve, reject) => {
                resolve([]);
            });
        }
    }

    loadDataPost(dataEntity) {

        var self = this;

        self.setState({ blocking: true, loading: true });

        Promise.all([this.getProvincias(dataEntity.PaisId), this.getCiudades(dataEntity.ProvinciaId), this.getParroquias(dataEntity.CiudadId)])
            .then(function ([provincias, ciudades, parroquias]) {

                let dataExtraLocal = { ...self.state.dataExtra };

                dataExtraLocal.provincias = self.props.mapDropdown(provincias.data, 'nombre', 'Id');
                dataExtraLocal.ciudades = self.props.mapDropdown(ciudades.data, 'nombre', 'Id');
                dataExtraLocal.parroquias = self.props.mapDropdown(parroquias.data, 'nombre', 'Id');


                let blockingItemLocal = {
                    provincias: provincias.data.length > 0,
                    ciudades: ciudades.data.length > 0,
                    parroquias: parroquias.data.length > 0
                };

                self.setState({ bloblockingItem: blockingItemLocal, dataExtra: dataExtraLocal, blocking: false, loading: false });
            })
            .catch((error) => {
                //TODO: Mejorar gestion errores
                self.setState({ blocking: false, loading: false });
                console.log(error);
            });
    }

    loadDataExtra() {

        var self = this;

        self.setState({ blocking: true, loading: true });

        Promise.all([this.getTipoIdentificacion(), this.getTipoProveedor(), this.getPaises()])
            .then(function ([tipoIdentificador, tipoProveedor, paises]) {
                console.log(paises)
                console.log(tipoIdentificador)
                console.log(tipoProveedor)

                self.setState({
                    dataExtra: {
                        paises: self.props.mapDropdown(paises.data, 'nombre', 'Id'),
                        tipo_identificaciones: self.props.mapDropdown(tipoIdentificador.data, 'nombre', 'Id'),
                        tipo_proveedores: self.props.mapDropdown(tipoProveedor.data, 'nombre', 'Id'),
                        provincias: [],
                        ciudades: [],
                        parroquias: []
                    }
                });

                self.setState({ loadDataExtra: true, blocking: false, loading: false });
            })
            .catch((error) => {
                //TODO: Mejorar gestion errores
                self.setState({ blocking: false, loading: false });
                console.log(error);
            });
    }

    isValid() {
        const errors = {};

        if (!this.state.data.tipo_identificacion || this.state.data.tipo_identificacion <= 0) {
            errors.tipo_identificacion = 'Debe seleccionar un tipo de Identificación';
        }

        if (!this.state.data.identificacion || this.state.data.identificacion.length <= 1) {
            errors.identificacion = 'Identificación es requerido';
        }

        if (!this.state.data.razon_social || this.state.data.razon_social.length <= 1) {
            errors.razon_social = 'Razón social es requerido';
        }

        if (this.state.data.PaisId === undefined || this.state.data.PaisId <= 0) {
            errors.PaisId = 'Debe seleccionar un País';
        }

        if (this.state.data.ProvinciaId === undefined || this.state.data.ProvinciaId <= 0) {
            errors.ProvinciaId = 'Debe seleccionar una provincia';
        }
        if (this.state.data.correo_electronico === undefined || this.state.data.correo_electronico === '') {
            errors.correo_electronico = 'Correo es requerido';
        }

        if (this.state.data.CiudadId === undefined || this.state.data.CiudadId <= 0) {
            errors.CiudadId = 'Debe seleccionar una ciudad';
        }
        if (this.state.data.ParroquiaId === undefined || this.state.data.ParroquiaId <= 0) {
            errors.ParroquiaId = 'Debe seleccionar una parroquia';
        }

        /*if (this.state.data.tipo_proveedor_id === undefined || this.state.data.tipo_proveedor_id <= 0) {
            errors.tipo_proveedor_id = 'Debe seleccionar un Tipo Proveedor';
        }
        */
        if (validationRules["isExisty"]([], this.state.data.correo_electronico) &&
            !validationRules["isEmptyString"]([], this.state.data.correo_electronico)
        ) {
            const emailValidation = validationRules["isEmail"]([], this.state.data.correo_electronico);
            if (!emailValidation) {
                errors.correo_electronico = 'El Correo Electrónico es inválido';
            }
        }


        /*if (this.state.data.es_externo) {
            if (!this.state.data.coordenadas || this.state.data.coordenadas.length <= 1) {
                errors.coordenadas = 'Coordenadas es requerido, si proveedor es Externo';
            }
        }*/


        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    handleSubmit(event) {
        event.preventDefault();

        if (!this.isValid()) {
            abp.notify.error("No ha ingresado los campos obligatorios  o existen datos inválidos.", 'Validación');
            return;
        }

        this.setState({ blocking: true });

        let url = '';
        if (this.props.entityAction === 'edit')
            url = `${this.props.urlApiBase}/EditApi`;
        else
            url = `${this.props.urlApiBase}/CreateApi`;


        //creating copy of object
        let data = Object.assign({}, this.state.data);
        this.normalizingDataSubmit(data);

        const formData = new FormData();
        for (var key in data) {
            if (data[key] !== null)
                formData.append(key, data[key]);
            else
                formData.append(key, '');
        }

        const config = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };


        http.post(url, formData, config)
            .then((response) => {

                let data = response.data;
                console.log(data);

                if (data.success === true) {

                    this.setState({
                        data: this.initData()
                    });


                    if (this.props.entityId <= 0) {
                        this.props.onAdded(response.data.result.id);
                    }
                    else {
                        this.props.onUpdated(response.data.result.id);
                    }

                } else {

                    console.log(data.errors)
                    abp.notify.error(data.errors, 'Alerta');

                    /*if (data.errors === 'UNIQUECORREO') {
                        abp.notify.error("Ya Existe un Proveedor con el mismo corre electrónico", 'Validación');

                    } else {
                        //TODO: 
                        //Presentar errores... 
                        var message = $.fn.responseAjaxErrorToString(data);
                        abp.notify.error(message, 'Error');
                    }*/
                }

                this.setState({ blocking: false });

            })
            .catch((error) => {

                console.log(error);

                this.setState({ blocking: false });
            });

    }

    normalizingDataSubmit(dataEntity) {

        //Fix. Claves foraneas a 0
        if (dataEntity.PaisId <= 0)
            dataEntity.PaisId = null;

        if (dataEntity.ProvinciaId <= 0)
            dataEntity.ProvinciaId = null;

        if (dataEntity.CiudadId <= 0)
            dataEntity.CiudadId = null;

        if (dataEntity.ParroquiaId <= 0)
            dataEntity.ParroquiaId = null;

        //Fix. Boolean to Enum
        if (dataEntity.estado)
            dataEntity.estado = 1;
        else
            dataEntity.estado = 0;

        if (dataEntity.es_externo)
            dataEntity.es_externo = 1;
        else
            dataEntity.es_externo = 0;


        //Fix value null



        //Add File
        dataEntity.uploadFile = this.state.uploadFile;

    }

    handleChange(event) {
        const target = event.target;

        if (event.target.files) {

            let files = event.target.files || event.dataTransfer.files;
            if (files.length > 0) {
                let uploadFile = files[0];
                this.setState({
                    uploadFile: uploadFile
                });
            }

        } else {
            const value = target.type === "checkbox" ? target.checked : target.value;


            const name = target.name;

            const updatedData = {
                ...this.state.data
            };

            updatedData[name] = value;


            this.setState({
                data: updatedData
            });

            //Fix
            if (name === "PaisId") {
                this.handleChangeProvincias(value);
            }

            if (name === "ProvinciaId") {
                this.handleChangeCiudades(value);
            }

            if (name === "CiudadId") {
                this.handleChangeParroquias(value);
            }
        }

    }

    handleChangeProvincias(padreId) {
        var self = this;

        self.setState({ blocking: true, loading: true });

        Promise.all([this.getProvincias(padreId)])
            .then(function ([provincias]) {

                let dataExtraLocal = { ...self.state.dataExtra };

                if (provincias)
                    dataExtraLocal.provincias = self.props.mapDropdown(provincias.data, 'nombre', 'Id');
                else
                    dataExtraLocal.provincias = [];

                dataExtraLocal.ciudades = [];
                dataExtraLocal.parroquias = [];

                let blockingItemLocal = {
                    provincias: true,
                    ciudades: false,
                    parroquias: false
                };

                let updatedData = {
                    ...self.state.data
                };

                updatedData["ProvinciaId"] = null;
                updatedData["CiudadId"] = null;
                updatedData["ParroquiaId"] = null;

                self.setState({ data: updatedData, blockingItem: blockingItemLocal, dataExtra: dataExtraLocal, loadDataExtra: true, blocking: false, loading: false });
            })
            .catch((error) => {
                //TODO: Mejorar gestion errores
                self.setState({ blocking: false, loading: false });
                console.log(error);
            });
    }

    handleChangeCiudades(padreId) {

        var self = this;

        self.setState({ blocking: true, loading: true });

        Promise.all([this.getCiudades(padreId)])
            .then(function ([ciudades]) {

                let dataExtraLocal = { ...self.state.dataExtra };
                if (ciudades)
                    dataExtraLocal.ciudades = self.props.mapDropdown(ciudades.data, 'nombre', 'Id');
                else
                    dataExtraLocal.ciudades = [];
                dataExtraLocal.parroquias = [];

                let blockingItemLocal = {
                    provincias: true,
                    ciudades: true,
                    parroquias: false
                };

                let updatedData = {
                    ...self.state.data
                };

                updatedData["CiudadId"] = null;
                updatedData["ParroquiaId"] = null;

                self.setState({ data: updatedData, blockingItem: blockingItemLocal, dataExtra: dataExtraLocal, loadDataExtra: true, blocking: false, loading: false });
            })
            .catch((error) => {
                //TODO: Mejorar gestion errores
                self.setState({ blocking: false, loading: false });
                console.log(error);
            });
    }

    handleChangeParroquias(padreId) {

        var self = this;

        self.setState({ blocking: true, loading: true });

        Promise.all([this.getParroquias(padreId)])
            .then(function ([parroquias]) {

                let dataExtraLocal = { ...self.state.dataExtra };
                if (parroquias)
                    dataExtraLocal.parroquias = self.props.mapDropdown(parroquias.data, 'nombre', 'Id');
                else
                    dataExtraLocal.parroquias = [];

                let blockingItemLocal = {
                    provincias: true,
                    ciudades: true,
                    parroquias: true
                };

                self.setState({ blockingItem: blockingItemLocal, dataExtra: dataExtraLocal, loadDataExtra: true, blocking: false, loading: false });
            })
            .catch((error) => {
                //TODO: Mejorar gestion errores
                self.setState({ blocking: false, loading: false });
                console.log(error);
            });
    }

    setData(name, value) {

        const updatedData = {
            ...this.state.data
        };

        updatedData[name] = value;

        this.setState({
            data: updatedData
        });

        //Fix
        if (name === "PaisId") {
            this.handleChangeProvincias(value);
        }

        if (name === "ProvinciaId") {
            this.handleChangeCiudades(value);
        }

        if (name === "CiudadId") {
            this.handleChangeParroquias(value);
        }
    }

    render() {


        let blocking = this.state.blocking || this.state.loading || this.props.blocking;

        return (
            (!this.props.show) ? (<div>...</div>) :
                (
                    <BlockUi tag="div" blocking={blocking}>

                        <form onSubmit={this.handleSubmit}>
                            <div className="row">
                                <div className="col">

                                    <Field
                                        name="tipo_identificacion"
                                        value={this.state.data.tipo_identificacion}
                                        label="Tipo Identificación"
                                        required={true}
                                        options={this.state.dataExtra.tipo_identificaciones}
                                        type={"select"}
                                        onChange={this.setData}
                                        error={this.state.errors.tipo_identificacion}
                                        readOnly={(this.props.entityAction === 'show')}
                                    />

                                </div>
                                <div className="col">

                                    <Field
                                        name="identificacion"
                                        label="Identificación"
                                        required
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.identificacion}
                                        onChange={this.handleChange}
                                        error={this.state.errors.identificacion}
                                    />
                                </div>
                            </div>
                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="razon_social"
                                        label="Razón social"
                                        required
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.razon_social}
                                        onChange={this.handleChange}
                                        error={this.state.errors.razon_social}
                                    />
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="codigo_sap"
                                        label="Código SAP"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.codigo_sap}
                                        onChange={this.handleChange}
                                        error={this.state.errors.codigo_sap}
                                    />
                                </div>

                                <div className="col">
                                    <Field
                                        name="usuario"
                                        label="Usuario"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.usuario}
                                        onChange={this.handleChange}
                                        error={this.state.errors.usuario}
                                        readOnly={true}
                                    />
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">

                                    <Field
                                        name="PaisId"
                                        value={this.state.data.PaisId}
                                        label="País"
                                        required={true}
                                        options={this.state.dataExtra.paises}
                                        type={"select"}
                                        onChange={this.setData}
                                        error={this.state.errors.PaisId}
                                        readOnly={(this.props.entityAction === 'show')}
                                    />

                                </div>

                                <div className="col">

                                    <Field
                                        name="ProvinciaId"
                                        value={this.state.data.ProvinciaId}
                                        label="Provincia"
                                        required={true}
                                        options={this.state.dataExtra.provincias}
                                        type={"select"}
                                        onChange={this.setData}
                                        error={this.state.errors.ProvinciaId}
                                        readOnly={(this.props.entityAction === 'show')}
                                    />

                                </div>
                            </div>

                            <div className="row">
                                <div className="col">

                                    <Field
                                        name="CiudadId"
                                        value={this.state.data.CiudadId}
                                        label="Ciudad"
                                        required={true}
                                        options={this.state.dataExtra.ciudades}
                                        type={"select"}
                                        onChange={this.setData}
                                        error={this.state.errors.CiudadId}
                                        readOnly={(this.props.entityAction === 'show')}
                                    />

                                </div>

                                <div className="col">

                                    <Field
                                        name="ParroquiaId"
                                        value={this.state.data.ParroquiaId}
                                        label="Parroquia"
                                        options={this.state.dataExtra.parroquias}
                                        type={"select"}
                                        onChange={this.setData}
                                        error={this.state.errors.ParroquiaId}
                                        readOnly={(this.props.entityAction === 'show')}
                                    />

                                </div>
                            </div>


                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="calle_principal"
                                        label="Calle Principal"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.calle_principal}
                                        onChange={this.handleChange}
                                        error={this.state.errors.calle_principal}
                                    />
                                </div>
                                <div className="col">
                                    <Field
                                        name="calle_secundaria"
                                        label="Calle Secundaria"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.calle_secundaria}
                                        onChange={this.handleChange}
                                        error={this.state.errors.calle_secundaria}
                                    />
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="referencia"
                                        label="Referencia"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.referencia}
                                        onChange={this.handleChange}
                                        error={this.state.errors.referencia}
                                    />
                                </div>

                                <div className="col">
                                    <Field
                                        name="numero"
                                        label="Número"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.numero}
                                        onChange={this.handleChange}
                                        error={this.state.errors.numero}
                                    />
                                </div>
                            </div>


                            <div className="row">
                                <div className="col">
                                    <Field
                                        required
                                        name="correo_electronico"
                                        label="Correo Electrónico Principal"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.correo_electronico}
                                        onChange={this.handleChange}
                                        error={this.state.errors.correo_electronico}
                                    />
                                </div>

                                <div className="col">
                                    <Field
                                        name="coordenadas"
                                        label="Coordenadas"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        //readOnly={(this.props.entityAction === 'show' || !this.state.data.es_externo)}
                                        readOnly={true}
                                        value={this.state.data.coordenadas}
                                        onChange={this.handleChange}
                                        error={this.state.errors.coordenadas}
                                    />
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="telefono_convencional"
                                        label="Teléfono"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.telefono_convencional}
                                        onChange={this.handleChange}
                                        error={this.state.errors.telefono_convencional}
                                    />
                                </div>
                                <div className="col">
                                    <Field
                                        name="celular"
                                        label="Celular"
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.celular}
                                        onChange={this.handleChange}
                                        error={this.state.errors.celular}
                                    />
                                </div>

                            </div>

                            <div className="row">
                                {/* <div className="col">

                                    <Field
                                        name="tipo_proveedor_id"
                                        required
                                        value={this.state.data.tipo_proveedor_id}
                                        label="Tipo Proveedor"
                                        options={this.state.dataExtra.tipo_proveedores}
                                        type={"select"}
                                        onChange={this.setData}
                                        error={this.state.errors.tipo_proveedor_id}
                                        readOnly={(this.props.entityAction === 'show')}
                                    />

                                </div>
                                */}
                                <div className="col">
                                    <Field
                                        name="estado"
                                        label="Estado"
                                        labelOption="(Activo/Inactivo)"
                                        type={"checkbox"}
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.estado}
                                        onChange={this.handleChange}
                                        error={this.state.errors.estado}
                                    />
                                </div>
                                <div className="col">
                                    <Field
                                        name="uploadFile"
                                        label="Documentación"
                                        type={"file"}
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        onChange={this.handleChange}
                                        error={this.state.errors.uploadFile}
                                    />
                                </div>
                            </div>


                            <div className="row">

                                <div className="col">
                                    <Field
                                        name="es_externo"
                                        label="Externo"
                                        labelOption="(Si/No)"
                                        type={"checkbox"}
                                        edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                        readOnly={(this.props.entityAction === 'show')}
                                        value={this.state.data.es_externo}
                                        onChange={this.handleChange}
                                        error={this.state.errors.es_externo}
                                    />
                                </div>

                            </div>


                            {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                                <ActionForm onCancel={this.props.onHide} onSave={this.handleSubmit} />
                            }

                            {this.props.entityAction === 'show' &&
                                <ActionForm onAccept={this.props.onHide} />
                            }

                        </form>
                    </BlockUi>
                )
        );
    }
}

export default wrapForm(ProveedorForm);