import React from 'react';
import axios from 'axios';
import moment from 'moment';
import BlockUi from 'react-block-ui';

export default class PagoLiquidacion extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            baja_id: '',
            tipo_identificacion: '',
            nro_identificacion: '',
            nombres_apellidos: '',
            nro_legajo: '',
            id_sap: '',
            motivo_baja: '',
            fecha_baja: '',
            fecha_pago: '',
            tiposMotivosBajas: [],
            errores: [],
            formIsValid: false,
            loading: false,
            archivoNombre: '',
            file: [],
        }

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.getFormMotivosBajas = this.getFormMotivosBajas.bind(this);

        this.handleChange = this.handleChange.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.Guardar = this.Guardar.bind(this);
        this.clearStates = this.clearStates.bind(this);
        this.onUpload = this.onUpload.bind(this);
        this.descargarFile = this.descargarFile.bind(this);
    }

    componentDidMount() {

    }

    componentWillReceiveProps(nextProps) {
        // console.log('nextProps', nextProps);
        this.setState({
            baja_id: nextProps.baja_id,
            tipo_identificacion: nextProps.tipo_identificacion,
            nro_identificacion: nextProps.nro_identificacion,
            nombres_apellidos: nextProps.nombres_apellidos,
            nro_legajo: nextProps.nro_legajo,
            id_sap: nextProps.id_sap,
            motivo_baja: nextProps.motivo_baja,
            fecha_baja: moment(nextProps.fecha_baja).format("YYYY-MM-DD")
        })
        this.GetCatalogos();
    }

    componentDidMount() {
        // this.GetTiposItentificacion();
        // this.setState({
        //     tipo_identificacion: this.props.tipo_identificacion,
        //     apellidos: this.props.apellidos,
        //     nombres: this.props.nombres,
        //     nro_identificacion: this.props.nro_identificacion
        // })
    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <form onSubmit={this.handleSubmit}>
                        <div className="row" >
                            <div className="col-xs-12 col-md-12" >
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="text"><b>Tipo de Identificación:</b> {this.state.tipo_identificacion} </label>
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="text"><b>No. de Identificación:</b> {this.state.nro_identificacion} </label>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="text"><b>Apellidos Nombres:</b> {this.state.nombres_apellidos} </label>
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="text"><b>ID Legajo:</b> {this.state.nro_legajo} </label>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="text"><b>ID SAP:</b> {this.state.id_sap} </label>
                                        </div>
                                    </div>
                                    <div className="col">
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="motivo_baja">* Motivo de Baja: </label>
                                            <select value={this.state.motivo_baja} onChange={this.handleChange} className="form-control" name="motivo_baja" disabled>
                                                <option value="">Seleccione...</option>
                                                {this.getFormMotivosBajas()}
                                            </select>
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="fecha_baja">* Fecha de Baja: </label>
                                            <input type="date" id="fecha_baja" className="form-control" value={this.state.fecha_baja} onChange={this.handleChange} name="fecha_baja" disabled />

                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="fecha_pago">* Fecha de Pago: </label>
                                            <input type="date" id="fecha_pago" className="form-control" value={this.state.fecha_pago} onChange={this.handleChange} name="fecha_pago" />
                                            <span style={{ color: "red" }}>{this.state.errores["fecha_pago"]}</span>
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="archivo">Archivo de Respaldo: </label><br />
                                            <input type="file" id="file_respaldo" accept=".xls,.png,.jpg,.doc,.xlsx,.docx" onChange={(e) => this.onUpload(e)} />
                                            <br />
                                            <div style={{ marginTop: '10px' }}><a href="#" onClick={() => this.descargarFile()}> {this.state.archivoNombre}</a> </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </form>
                </div >
            </BlockUi>
        )
    }

    clearStates() {
        this.setState({
            baja_id: '',
            tipo_identificacion: '',
            nro_identificacion: '',
            nombres_apellidos: '',
            nro_legajo: '',
            id_sap: '',
            motivo_baja: '',
            fecha_baja: '',
            fecha_pago: '',
        }, this.props.onHidePago())
    }

    onUpload(event) {
        console.log('e', event.target.files[0]);
        var file = event.target.files[0];
        if (file != null) {

            if (file >= 2 * 1024 * 1024) {
                this.props.warnMessage("El archivo solo puede ser de máximo 2MB");
                document.getElementById("file_respaldo").value = "";
                this.setState({ file: [], archivoNombre: '' });
                return;
            } else if (!file.type.match('application/vnd') && !file.type.match('image/png') && !file.type.match('image/jpeg')) {
                this.props.warnMessage("No puede subir archivos de ese formato");
                document.getElementById("file_respaldo").value = "";
                this.setState({ file: [], archivoNombre: '' });
                return;
            } else {
                this.props.successMessage("Archivo Cargado!");
                this.setState({ file: file, archivoNombre: file.name });

            }

        } else {
            console.log("error llamada");
        }
    }

    descargarFile() {

        var file = this.state.file;
        var filename = file.name;
        var blob = new Blob([file]);
        var url = URL.createObjectURL(blob);

        var element = document.createElement('a');
        element.setAttribute('href', url);
        element.setAttribute('download', filename);

        element.style.display = 'none';
        document.body.appendChild(element);

        element.click();

        document.body.removeChild(element);

    }

    handleValidation() {
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.fecha_pago) {
            this.state.formIsValid = false;
            errors["fecha_pago"] = "El campo Fecha de Pago es obligatorio.";
        } else {
            if (this.state.fecha_pago < this.state.fecha_baja) {
                this.state.formIsValid = false;
                errors["fecha_pago"] = "Fecha de Pago no puede ser menor a Fecha de Baja.";
            }
        }


        this.setState({ errores: errors });
    }

    Guardar() {
        console.log('Guardar')
        this.handleValidation();
        console.log(this.state.formIsValid)
        if (this.state.formIsValid == true) {
            this.setState({ loading: true });
            console.log('file', this.state.file);

            const formData = new FormData();
            formData.append('idBaja', this.state.baja_id)
            formData.append('fecha_pago_liquidacion', this.state.fecha_pago)
            if (this.state.file == '') {
                formData.append('UploadedFile', null)
            } else {
                formData.append('UploadedFile', this.state.file)
            }
            const config = { headers: { 'content-type': 'multipart/form-data' } }
            axios.post("/RRHH/ColaboradorBaja/CreatePagoApi/", formData, config)
                .then((response) => {
                    this.setState({ loading: false });
                    this.props.successMessage("Pago de Liquidación Guardado!");
                    this.clearStates();
                    this.props.GetColaboradores();
                })
                .catch((error) => {
                    this.setState({ loading: false });
                    console.log(error);
                    this.props.warnMessage("Algo salió mal.");
                });

            // axios.post("/RRHH/ColaboradorBaja/CreatePagoApi/", {
            //     Id: this.state.baja_id,
            //     fecha_pago_liquidacion: this.state.fecha_pago,
            // })
            //     .then((response) => {
            //         this.setState({ loading: false });
            //         this.props.successMessage("Pago de Liquidación Guardado!");
            //         this.clearStates();
            //         this.props.GetColaboradores();
            //     })
            //     .catch((error) => {
            //         this.setState({ loading: false });
            //         console.log(error);
            //         this.props.warnMessage("Algo salió mal.");
            //     });
        }
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['MOTIVOBAJA'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    cargarCatalogos(data) {
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
            switch (codigoCatalogo) {
                case 'MOTIVOBAJA':
                    this.setState({ tiposMotivosBajas: catalogo })
                    this.getFormMotivosBajas();
                    return;
            }

        });
    }

    getFormMotivosBajas() {
        return (
            this.state.tiposMotivosBajas.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }




}