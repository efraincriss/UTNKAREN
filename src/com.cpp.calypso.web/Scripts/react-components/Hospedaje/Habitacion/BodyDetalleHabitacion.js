import React from "react";
import HabitacionesTable from "./HabitacionesTable";
import EspaciosTable from "./EspaciosTable";
import { Dialog } from 'primereact-v2/dialog';
import CrearHabitacionForm from "./CrearHabitacionForm";
import http from "../../Base/HttpService";

export default class BodyDetalleHabitacion extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            visible: false,
            habitacion: {},
            editable: false,
        }

        this.formRef = React.createRef();
    }


    render() {
        return (
            <div className="row">
                <div style={{ width: '100%' }}>
                    <div className="card">
                        <div className="card-body">
                            <div className="row">
                                <div className="col">

                                    <ul className="nav nav-tabs" id="hospedaje_tabs" role="tablist">
                                        <li className="nav-item">
                                            <a className="nav-link active" id="habitaciones-tab" data-toggle="tab" href="#habitaciones" role="tab" aria-controls="home" aria-expanded="true">Stock de Habitaciones</a>
                                        </li>
                                        <li className="nav-item">
                                            <a className="nav-link" id="espacios-tab" data-toggle="tab" href="#espacios" role="tab" aria-controls="profile">Stock de Espacios</a>
                                        </li>
                                    </ul>

                                    <div className="tab-content" id="myTabContent">
                                        <div className="tab-pane fade show active" id="habitaciones" role="tabpanel" aria-labelledby="habitaciones-tab">
                                           
                                           
                                            <HabitacionesTable
                                                data={this.props.habitaciones}
                                                mostrarForm={this.mostrarForm}
                                                unlockScreen={this.props.unlockScreen}
                                                blockScreen={this.props.blockScreen}
                                                showSuccess={this.props.showSuccess}
                                                showWarn={this.props.showWarn}
                                                consultarEspaciosYHabitaciones={this.props.consultarEspaciosYHabitaciones}
                                                getHabitacionDetalle={this.getHabitacionDetalle}
                                            />
                                        </div>

                                        <div className="tab-pane fade" id="espacios" role="tabpanel" aria-labelledby="espacios-tab">
                                            <EspaciosTable
                                                data={this.props.espacios}
                                                unlockScreen={this.props.unlockScreen}
                                                blockScreen={this.props.blockScreen}
                                                getEspacios={this.props.getEspacios}
                                                consultarEspaciosYHabitaciones={this.props.consultarEspaciosYHabitaciones}
                                                showSuccess={this.props.showSuccess}
                                                showWarn={this.props.showWarn}
                                            />
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                    <Dialog header="Crear Habitaciones" contentStyle={{maxHeight: '450px', overflowY: 'auto', overflowX: 'hidden'}} rtl={true} visible={this.state.visible} width="550px" modal={true} onHide={this.ocultarForm}>
                        <CrearHabitacionForm
                            ref={this.formRef}
                            unlockScreen={this.props.unlockScreen}
                            blockScreen={this.props.blockScreen}
                            showSuccess={this.props.showSuccess}
                            showWarn={this.props.showWarn}
                            ocultarForm={this.ocultarForm}
                            consultarEspaciosYHabitaciones={this.props.consultarEspaciosYHabitaciones}
                            proveedorId={this.props.proveedorId}
                            habitacion={this.state.habitacion}
                            setHabitacion={this.setHabitacion}
                            editable={this.state.editable}
                        />
                    </Dialog>
                </div>
            </div>
        )
    }

    resetFormData = () => {
        this.formRef.current.resetErrors();
    }


    ocultarForm = () => {
        this.setState({ visible: false, habitacion: {}, editable: false}, this.resetFormData)
    }

    mostrarForm = (editable) => {
        this.setState({ visible: true, editable: editable })
    }

    setHabitacion = (habitacion) => {
        this.setState({ habitacion })
    }

    getHabitacionDetalle = (id) => {
        this.props.blockScreen();
        let url;
        url = `/Proveedor/Habitacion/DetailsApi/${id}`
        http.get(url, {})
            .then((response) => {
                let data = response.data
                this.setState({ habitacion: data.result }, this.mostrarForm(true))
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
    }


}