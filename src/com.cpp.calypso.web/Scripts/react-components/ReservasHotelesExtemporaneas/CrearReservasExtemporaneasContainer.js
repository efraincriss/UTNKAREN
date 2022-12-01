import React from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../Base/BaseWrapper";
import EspaciosDisponiblesTable from "./EspaciosDisponiblesTable";
import http from "../Base/HttpService";
import { Dialog } from 'primereact/components/dialog/Dialog';
import {
    FRASE_ERROR_SELECCIONA_FECHA_INICIO,
    FRASE_ERROR_SELECCIONA_FECHA_FIN,
    FRASE_RESERVA_CREADA,
    MODULO_PROVEEDOR,
    CONTROLLER_RESERVA_HOTEL
} from "../Base/Strings";
import config from "../Base/Config";
import Field from "../Base/Field-v2";
import BuscarColaboradorContainer from "./BuscarColaborador/BuscarColaboradorContainer";
import UploadPdfForm from "./UploadPdfForm";
import moment from "moment";

export class CrearReservaContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            data: [],
            fechaInicio: "",
            fechaFin: "",
            colaborador: {},
            visible: false,
            espacioId: 0,
            errors: {},
            colaboradorId: 0,
            UploadedFile: null,


            //archivos

            upload: false,
            uploadFile: '',
            seleccionado: null


        }

        this.busquedaRef = React.createRef();
        //SUBIDA DE ARCHIVOS
        this.handleChangeUploadFile = this.handleChangeUploadFile.bind(this);

        this.mostrarUpload = this.mostrarUpload.bind(this);
        this.ocultarUpload = this.ocultarUpload.bind(this);
    }

    componentDidMount() {
        this.props.unlockScreen();
    }
    ocultarUpload = () => {
        this.setState({ upload: false, uploadFile: '' })
    }

    mostrarUpload = (cell) => {

        this.setState({ seleccionado: cell, upload: true })
    }
    submit = (cell, uploadFile) => {
          this.seleccionarColaborador(cell, uploadFile)
    }

    handleChangeUploadFile = event => {
        if (event.files) {
            if (event.files.length > 0) {
                let uploadFile = event.files[0];
                this.setState({
                    uploadFile: uploadFile
                }, this.submit(this.state.seleccionado, uploadFile));
            } else {
                this.props.showWarn("Error al Seleccionar Archivo")

            }
        } else {
            this.props.showWarn("Error al Seleccionar Archivo")
        }
    }


    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <form onSubmit={this.consultarEspaciosDisponibles}>
                            <div className="row">
                                <div className="col-xs-6 col-md-5">
                                    <Field
                                        name="fechaInicio"
                                        label="Fecha inicio"
                                        required
                                        type="date"
                                        edit={true}
                                        readOnly={false}
                                        value={this.state.fechaInicio}
                                        onChange={this.handleChange}
                                        error={this.state.errors.fechaInicio}
                                    />
                                </div>
                                <div className="col-xs-6 col-md-5">
                                    <Field
                                        name="fechaFin"
                                        label="Fecha fin"
                                        required
                                        type="date"
                                        edit={true}
                                        readOnly={false}
                                        value={this.state.fechaFin}
                                        onChange={this.handleChange}
                                        error={this.state.errors.fechaFin}
                                    />
                                </div>

                                <div className="col" style={{ paddingTop: '35px' }}>
                                    <button type="submit" className="btn btn-outline-primary">Buscar</button>&nbsp;
                                </div>
                            </div>
                        </form>
                        <hr />
                    </div>
                </div>

                <div className="row" style={{ marginTop: '1em' }}>
                    <div className="col">

                        <EspaciosDisponiblesTable
                            data={this.state.data}
                            seleccionarEspacio={this.seleccionarEspacio}
                        />

                        <Dialog header="Colaborador" visible={this.state.visible} width="830px" modal={true} onHide={this.ocultarForm}>
                            <BuscarColaboradorContainer
                                ref={this.busquedaRef}
                                seleccionarColaborador={this.seleccionarColaborador}
                                unlockScreen={this.props.unlockScreen}
                                blockScreen={this.props.blockScreen}
                                showSuccess={this.props.showSuccess}
                                showWarn={this.props.showWarn}
                                mostrarUpload={this.mostrarUpload}
                                ocultarUpload={this.ocultarUpload}
                            />
                        </Dialog>
                        <Dialog header="Subir Archivo de Reserva Extemporáneo" visible={this.state.upload} modal={true} width="600px" onHide={this.ocultarUpload}>
                            <UploadPdfForm
                                handleChange={this.handleChangeUploadFile}
                                label="Reserva Extemporáneo"
                            />
                        </Dialog>
                    </div>
                </div>
            </div>


        )
    }

    crearEspacio = (ColaboradorId, uploadFile) => {
        this.props.blockScreen();
        if (this.state.fechaInicio === "") {
            this.props.showWarn(FRASE_ERROR_SELECCIONA_FECHA_INICIO)
            this.props.unlockScreen();
        } else if (this.state.fechaFin === "") {
            this.props.showWarn(FRASE_ERROR_SELECCIONA_FECHA_FIN)
            this.props.unlockScreen();
        } else {
            this.crearEspacioApi(ColaboradorId, uploadFile);
        }
    }

    crearEspacioApi = (ColaboradorId, uploadFile) => {
        var entity = {
            EspacioHabitacionId: this.state.espacioId,
            ColaboradorId: ColaboradorId,
            fecha_desde: this.state.fechaInicio,
            fecha_hasta: this.state.fechaFin,
            fecha_registro: new Date(),
            extemporaneo: true,
            DocumentoId: null,
        }
        console.log(entity)
        const formData = new FormData();
        formData.append('EspacioHabitacionId', this.state.espacioId);
        formData.append('ColaboradorId', ColaboradorId);
        formData.append('fecha_desde', this.state.fechaInicio);
        formData.append('fecha_hasta', this.state.fechaFin);
        formData.append('fecha_registro', this.state.fechaInicio);
        formData.append('extemporaneo', true);
        formData.append('uploadFile', uploadFile);
        formData.append('DocumentoId', 0);
        formData.append('es_extemporaneo', "_");
        const multipart = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };
        let url = '';
        url = `${config.appUrl}${MODULO_PROVEEDOR}/${CONTROLLER_RESERVA_HOTEL}/CrearReservaExtemporaneaApi`

        http.post(url, formData, multipart)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    if (data.created == true) {
                        this.props.showSuccess("La reserva extemporánea fue creada correctamente")
                        this.setState({
                            fechaInicio: "",
                            fechaFin: "",
                            colaborador: {},
                            colaboradorId: 0,
                            visible: false,
                            data: []
                        }, this.resetValues);
                        
                this.props.unlockScreen();
                    } else {
                        this.props.showWarn(data.errors);
                        
                this.props.unlockScreen();
                    }
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                    
                this.props.unlockScreen();
                }

            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })

    }

    resetValues = () => {
        this.busquedaRef.current.resetValues();
        this.ocultarUpload();
    }

    consultarEspaciosDisponibles = event => {
        event.preventDefault();
        if (this.state.fechaInicio === "") {
            this.props.showWarn(FRASE_ERROR_SELECCIONA_FECHA_INICIO)
            this.props.unlockScreen();
        } else if (this.state.fechaFin === "") {
            this.props.showWarn(FRASE_ERROR_SELECCIONA_FECHA_FIN)
            this.props.unlockScreen();
        } else if (moment(this.state.fechaInicio) > moment(this.state.fechaFin)) {
            this.props.showWarn("Fecha de inicio debe ser menor a la fecha de fin")
            this.props.unlockScreen();
        } else {
            this.props.blockScreen();
            let url = '';
            url = '/Proveedor/ReservaHotel/EspciosDisponibles'
            http.post(url, {
                fechaInicio: this.state.fechaInicio,
                fechaFin: this.state.fechaFin
            })
                .then((response) => {
                    let data = response.data;
                    if (data.success === true) {
                        this.setState({ data: data.result });
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
    }

    handleChange = event => {
        this.setState({
            [event.target.name]: event.target.value,
            data: []
        });
    }

    seleccionarColaborador = (ColaboradorId, uploadFile) => {

        this.crearEspacio(ColaboradorId, uploadFile)
    }

    seleccionarEspacio = espacioId => {
        this.setState({ espacioId, visible: true })
    }

    ocultarForm = () => {
        this.setState({ visible: false, habitacion: {} }, this.resetValues)
    }

    mostrarForm = () => {
        this.setState({ visible: true })
    }
}

const Container = Wrapper(CrearReservaContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('crear_reserva_container')
);

