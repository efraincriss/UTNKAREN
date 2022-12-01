import axios from "axios";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import React from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapper";
import http from "../../Base/HttpService";
import { ProblemaSincronizacionTable } from "./ProblemaSincronizacionTable.jsx"
import AtenderProblemaForm from "./AtenderProblemaForm.jsx"
import AtenderProblemasMasivosForm from "./AtenderProblemasMasivosForm.jsx";
import Field from "../../Base/Field-v2";

class ProblemaSincronizacionContainer extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            errors: {},
            FechaInicio: new Date().toISOString().split('T')[0],
            Observaciones: '',
            solucionado: false,
            fechaInicio: null,
            fechaFin: null,
            solucionadoOpciones: [
                { label: "SI", value: true },
                { label: "NO", value: false },
            ],
            problemasSincronizacionList: [],
            mostrarModal: false,
            problemaSeleccionado: {},
            mostrarFormularioMasivo: false,
            problemasSeleccionados: []
        }
    }

    componentDidMount() {
        this.loadData();
    }

    render() {
        const footer = (
            <div className="row" align="right">
                <div className="col">
                    <div>
                        <button className="btn btn-outline-primary mr-4"
                            onClick={() => this.marcarNoSolucionado()}
                        >
                            Continuar
                        </button>
                        <button
                            className="btn btn-outline-primary mr-4"
                            type="button" aria-expanded="false"
                            aria-controls="collapseExample"
                            onClick={() => this.ocultarConfirmacion()}
                        >
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>

        );
        return (
            <div>
                <div className="card">
                    <div className="card-body">

                        <div className="row">
                            <div className="col">
                                <div>

                                </div>
                            </div>
                        </div>
                        <div className="row pt-4">
                            <div className="col">
                                <div className="card card-body">
                                    <div className="row">
                                        <div className="col">
                                            <Field
                                                name="fechaInicio"
                                                value={this.state.fechaInicio}
                                                label="Fecha Inicio"
                                                type="date"
                                                onChange={this.handleChange}
                                                error={this.state.errors.fechaInicio}
                                                edit={true}
                                                readOnly={false}
                                                max={new Date().toISOString().split('T')[0]}
                                            />
                                        </div>
                                        <div className="col">
                                            <Field
                                                name="fechaFin"
                                                value={this.state.fechaFin}
                                                label="Fecha Finalización"
                                                type="date"
                                                onChange={this.handleChange}
                                                error={this.state.errors.fechaFin}
                                                edit={true}
                                                readOnly={false}
                                                max={new Date().toISOString().split('T')[0]}
                                            />
                                        </div>
                                        <div className="col">
                                            <Field
                                                name="solucionado"
                                                label="Solucionado"
                                                type="select"
                                                options={this.state.solucionadoOpciones}
                                                edit={true}
                                                readOnly={false}
                                                value={this.state.solucionado}
                                                onChange={this.onChangeDropdown}
                                                error={this.state.errors.solucionado}
                                                placeholder="Seleccionar..."
                                            />
                                        </div>
                                    </div>

                                    <div className="row" align="right">
                                        <div className="col">
                                            <div>
                                                <button className="btn btn-outline-primary mr-4"
                                                    onClick={() => this.limpiarFiltros()}
                                                >
                                                    Limpiar Filtros
                                                </button>
                                                <button
                                                    className="btn btn-outline-primary mr-4"
                                                    type="button" aria-expanded="false"
                                                    aria-controls="collapseExample"
                                                    onClick={() => this.loadData()}
                                                >
                                                    Buscar
                                                </button>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div className="card">
                    <div className="card-body">
                        <div className="row">
                            <div className="col">
                                <ProblemaSincronizacionTable
                                    data={this.state.problemasSincronizacionList}
                                    mostrarModal={this.mostrarModal}
                                    mostrarFormularioMasivo={this.mostrarFormularioMasivo}
                                    showWarn={this.props.showWarn}
                                    blockScreen={this.props.blockScreen}
                                    unlockScreen={this.props.unlockScreen}
                                    showSuccess={this.props.showSuccess}
                                    editarErrorSincronizacion={this.editarErrorSincronizacion}
                                    mostrarConfirmacion={this.mostrarConfirmacion}                                />
                            </div>
                        </div>
                    </div>
                </div>

                <Dialog
                    header="Solucionar Problema"
                    modal={true}
                    visible={this.state.mostrarModal}
                    style={{ width: "700px", overflow: "auto", "maxHeight": "560px" }}
                    onHide={this.toggleModal}
                >
                    <AtenderProblemaForm
                        problema={this.state.problemaSeleccionado}
                        toggleModal={this.toggleModal}
                        blockScreen={this.props.blockScreen}
                        unlockScreen={this.props.unlockScreen}
                        showSuccess={this.props.showSuccess}
                        showWarn={this.props.showWarn}
                        loadData={this.loadData}
                        editando={this.state.editando}
                    />
                </Dialog>

                <Dialog
                    header="Atender Problemas Masivamente"
                    modal={true}
                    visible={this.state.mostrarFormularioMasivo}
                    style={{ width: "700px" }}
                    onHide={this.ocultarFormularioMasivo}
                >
                    <AtenderProblemasMasivosForm
                        problemasSeleccionados={this.state.problemasSeleccionados}
                        ocultarFormularioMasivo={this.ocultarFormularioMasivo}
                        blockScreen={this.props.blockScreen}
                        unlockScreen={this.props.unlockScreen}
                        showSuccess={this.props.showSuccess}
                        showWarn={this.props.showWarn}
                        loadData={this.loadData}
                        mostrarFormularioMasivo={this.mostrarFormularioMasivo}
                    />
                </Dialog>
                <Dialog header="Confirmación" visible={this.state.mostrarConfirmacion} width="350px" footer={footer} minY={70} onHide={this.ocultarConfirmacion} maximizable={true}>
                    Esta Seguro de actualizar el problema a No Solucionado?
                </Dialog>
            </div>
        )
    }

    loadData = () => {
        this.props.blockScreen();

        axios
            .get("/seguridad/ProblemaSincronizacion/Search", {
                params: {
                    fechaInicio: this.state.fechaInicio,
                    fechaFin: this.state.fechaFin,
                    solucionado: this.state.solucionado
                }
            })
            .then(response => {
                console.log(response);
                let data = response.data;
                if (data.success === true) {
                    console.log(data)
                    this.setState({ problemasSincronizacionList: data.result })
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn("Ocurrió un error, intentalo más tarde.");
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }

    marcarNoSolucionado = () => {
        this.props.blockScreen();
        let url;

        url = `/Seguridad/ProblemaSincronizacion/MarcarNoSolucionado`
        axios.post(url, {
            problemaSincronizacionId: this.state.problemaSeleccionado.Id
        })
            .then((response) => {
                let data = response.data
                if (data.success === true) {
                    this.ocultarConfirmacion();
                    this.props.showSuccess("Problema de sincronización actualizado correctamente");
                    this.loadData();
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



    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }

    onChangeDropdown = (name, value) => {
        this.setState({
            [name]: value,
        });
    };


    limpiarFiltros = () => {
        this.setState({
            solucionado: false,
            fechaInicio: '',
            fechaFin: ''
        })
    }

    toggleModal = (limpiarFiltros = false) => {
        this.setState({ mostrarModal: false, editando: false });
        if (limpiarFiltros) {
            this.limpiarFiltros();
        }

    }

    mostrarModal = (row) => {
        this.setState({
            mostrarModal: true,
            problemaSeleccionado: row,
            editando: false
        })
    }

    mostrarFormularioMasivo = (ids) => {
        this.setState({
            mostrarFormularioMasivo: true,
            problemasSeleccionados: ids
        });
    }

    ocultarFormularioMasivo = () => {
        this.setState({
            mostrarFormularioMasivo: false,
            problemasSeleccionados: []
        });
    }

    
    ocultarConfirmacion = () => {
        this.setState({
            mostrarConfirmacion: false,
            problemaSeleccionado: {}
        });
    }

    mostrarConfirmacion = (problema) => {
        this.setState({
            mostrarConfirmacion: true,
            problemaSeleccionado: problema
        });
    }

    editarErrorSincronizacion = (row) => {
        this.setState({
            mostrarModal: true,
            problemaSeleccionado: row,
            editando: true
        })
    }

}

const Container = Wrapper(ProblemaSincronizacionContainer);

ReactDOM.render(
    <Container />,
    document.getElementById('problema_sincronizacion_container')
);