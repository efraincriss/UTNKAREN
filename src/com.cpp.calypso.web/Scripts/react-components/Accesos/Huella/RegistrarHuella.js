import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';
import { Card } from 'primereact-v2/card';

export default class RegistrarHuella extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            dedo: '',
            principal: false,
            errores: [],
            formIsValid: true,
            estado: false,
            dedos: [],
            message: '',
            huella: '',
            plantilla_base64: '',
            display: 'none',
            loading: false,
            disable: false,
            Id: '',
            load: false,
        }

        this.getFormSelectDedos = this.getFormSelectDedos.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleCheck = this.handleCheck.bind(this);
        this.Guardar = this.Guardar.bind(this);

        /* Captura de Huella */
        this.captureFP = this.captureFP.bind(this);
        this.SuccessFunc = this.SuccessFunc.bind(this);
        this.ErrorFunc = this.ErrorFunc.bind(this);
        this.CallSGIFPGetData = this.CallSGIFPGetData.bind(this);
        this.GetHuellaDigital = this.GetHuellaDigital.bind(this);
        this.ClearStates = this.ClearStates.bind(this);
        this.CargarHuella = this.CargarHuella.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.CargarCatalogos = this.CargarCatalogos.bind(this);
    }

    componentDidMount() {
        this.GetCatalogos();
        this.CargarHuella();
    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.load}>
                <div className="col-sm-12" style={{ margin: 'auto' }}>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <form onSubmit={this.handleSubmit}>
                        <div className="col-sm-12" style={{ margin: 'auto' }}>


                            <div className="row">

                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="label">* Dedo Huella: </label>
                                        <select value={this.state.dedo} onChange={this.handleChange} disabled={this.state.disable} className="form-control" name="dedo">
                                            <option value="">Seleccione...</option>
                                            {this.getFormSelectDedos()}
                                        </select>
                                        <span style={{ color: "red" }}>{this.state.errores["dedo"]}</span>
                                    </div>
                                    <div className="form-group">

                                         <label htmlFor="observacion">Principal: </label>{" "}<input type="checkbox" onChange={this.handleCheck} ref='principal' defaultChecked={this.state.principal} name="principal" />
                                      
                                        <br />
                                    </div>
                                    <div className="form-group">
                                        <button type="button" title="Permite capturar la huella digital, una vez que se encienda la luz roja en el dispositivo, coloque el dedo seleccionado para proceder a registrar" onClick={() => this.captureFP()} className="btn btn-outline-primary">Capturar Huella</button>
                                    </div>

                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label htmlFor="label">* Huella Digital: </label><br />

                                        <br />
                                        <div style={{ width: '140px', height: '180px' }} >
                                            <BlockUi tag="div" blocking={this.state.loading}>
                                                <img id="FPImage1" style={{ display: this.state.display }} height="150" width="100" />
                                            </BlockUi>
                                        </div>
                                        <span style={{ color: "red" }}>{this.state.errores["huella"]}</span>
                                    </div>
                                    <div className="form-group">
                                        <button onClick={() => this.Guardar()} type="button" className="btn btn-outline-primary fa fa-save"> Guardar</button>{" "}
                                        <button type="button" onClick={() => this.ClearStates()} className="btn btn-outline-primary fa fa-arrow-left"> Cancelar</button>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </BlockUi>
        );
    }

    CargarHuella() {
        if (this.props.id_huella != 0) {

            axios.post("/Accesos/Huella/GetHuellaApi/" + this.props.id_huella, {})
                .then((response) => {
                    console.log(response)
                    this.setState({
                        dedo: response.data.catalogo_dedo_id,
                        principal: response.data.principal,
                        huella: response.data.huella,
                        plantilla_base64: response.data.plantilla_base64,
                        display: 'block',
                        disable: true,
                        Id: this.props.id_huella
                    });

                    /* Check */
                    this.refs.principal.checked = response.data.principal;

                    /* Se carga la imagen */
                    document.getElementById("FPImage1").src = "data:image/bmp;base64," + response.data.huella;
                })
                .catch((error) => {
                    this.props.showWarn("Algo salio mal.");
                });
        } else {
            this.setState({ Id: this.props.id_huella });
        }
    }

    Guardar() {

        this.handleValidation();

        if (this.state.formIsValid == true) {
            this.setState({ load: true });
            if (this.state.Id == 0) {
                axios.post("/Accesos/Huella/CrearHuellaApiAsync/", {
                    Id: this.state.Id,
                    dedo: this.state.dedo,
                    colaborador: this.props.id_colaborador,
                    huella: this.state.huella,
                    plantilla_base64: this.state.plantilla_base64,
                    principal: this.state.principal
                })
                    .then((response) => {
                        this.setState({ load: false });


                        if (response.data == "OK") {
                            this.props.actualizarListaHuellas();
                            this.successMessage("Huella registrada!");
                            setTimeout(
                                function () {
                                    this.props.hideForm();
                                }.bind(this), 2000
                            );

                        } else if (response.data === "_PRIMEROPRINCIPAL") {
                            console.log(response.data)
                            this.warnMessage("Inicie Registrando una Huella Principal");
                        }
                        else {
                            this.warnMessage(response.data);
                        }

                    })
                    .catch((error) => {
                        this.setState({ load: false });
                        this.warnMessage("Algo salio mal.");
                    });
            } else {

                axios.post("/Accesos/Huella/UpdateHuellaApiAsync/", {
                    Id: this.state.Id,
                    dedo: this.state.dedo,
                    colaborador: this.props.id_colaborador,
                    huella: this.state.huella,
                    plantilla_base64: this.state.plantilla_base64,
                    principal: this.state.principal
                })
                    .then((response) => {
                        this.setState({ load: false });
                        if (response.data == "OK") {
                            this.props.actualizarListaHuellas();
                            this.successMessage("Huella registrada!");
                            setTimeout(
                                function () {
                                    this.props.hideForm();
                                }.bind(this), 2000
                            );
                        } else {
                            this.warnMessage(response.data);
                        }

                    })
                    .catch((error) => {
                        this.setState({ load: false });
                        this.warnMessage("Algo salio mal.");
                    });

            }
        }

    }

    ClearStates() {
        this.setState({
            dedo: '',
            principal: '',
            huella: ''
        }, this.props.hideForm())
    }

    handleCheck() {
        if(this.state.principal){
            this.setState({ principal: false });
        }else{

            this.setState({ principal: true });
        }
  
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['DEDOS'];

        axios.post("/Accesos/Huella/GetByCodeApi/?code=DEDOS", {})
            .then((response) => {
                console.log(response.data)
  
                this.setState({ dedos: response.data.result })
                this.getFormSelectDedos();
            })
            .catch((error) => {
                console.log(error);
            });
    }

    CargarCatalogos(data) {
      
    }

    getFormSelectDedos() {
        return (
            this.state.dedos.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }
    captureFP() {
        this.setState({ sendData: false })
        this.CallSGIFPGetData(this.SuccessFunc, this.ErrorFunc);
    }

    SuccessFunc(result) {
        console.log('result', result);
        if (result.ErrorCode === 0) {
            /* 	Display BMP data in image tag
                BMP data is in base 64 format
            */
            if (result != null && result.BMPBase64.length > 0) {
                document.getElementById("FPImage1").src = "data:image/bmp;base64," + result.BMPBase64;
                this.setState({
                    huella: result.BMPBase64,
                    plantilla_base64: result.ISOTemplateBase64,
                    display: 'block',
                    loading: false
                })
                console.log('plantilla_base64', result.ISOTemplateBase64);
            }
        }
        else {
            this.setState({ loading: false })

            /* timeout */
            if (result.ErrorCode == 54) {
                this.warnMessage("El tiempo para tomar la huella a terminado, por favor intente nuevamente");
            }
        }
    }

    ErrorFunc() {
        this.setState({ loading: false })
        this.warnMessage("Algo salió mal.");
    }

    CallSGIFPGetData(successCall, failCall) {

        var secugen_lic = "2685338267";

        /* loading icon */
        this.setState({ loading: true })


        axios.get("https://localhost:8443/SGIFPCapture", {
            Timeout: "10000",
            Quality: "100",
            licstr: encodeURIComponent(secugen_lic),
            templateFormat: "ISO"
        })
            .then((response) => {
                successCall(response.data);
            })
            .catch((error) => {
                console.log(error);
                failCall(error)
            });
    }

    GetHuellaDigital() {
        if (this.state.colaborador.huella_digital != null) {
            document.getElementById("FPImage1").src = "data:image/bmp;base64," + this.state.colaborador.huella_digital;
            this.setState({ huella: this.state.colaborador.huella_digital, display: 'block', loading: false })
        } else {
            this.setState({ huella: '', display: 'none' })
        }
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

    handleValidation() {
        let errors = {};
        this.setState({ formIsValid: true });

        if (!this.state.dedo) {
            this.state.formIsValid = false;
            errors["dedo"] = "El campo Dedo Huella es obligatorio.";
        }

        if (!this.state.huella) {
            this.state.formIsValid = false;
            errors["huella"] = "La huella es obligatoria.";
        }

        this.setState({ errores: errors });
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

}