import React from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapperApi";
import config from "../../Base/Config";
import http from "../../Base/HttpService";
import { Dialog } from 'primereact-v2/dialog';
import {ScrollPanel} from 'primereact-v2/scrollpanel';
import {
    CONTROLLER_VEHICULOS,
    MODULO_TRANSPORTE,
    FRASE_VEHICULO_ELIMINADO
} from "../../Base/Strings";
import VehiculosTable from "./VehiculosTable";
import VehiculosForm from "./VehiculosForm";


export default class VehiculosContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            vehiculos: [],
            formDialog: false,
            vehiculo: {},
            editForm: false,
            errors:{}
        }
    }

    render() {
        return (
            <div>
                <div className="row" align="right" style={{ marginBottom: '2em' }}>
                    <div className="col">
                        <button className="btn btn-outline-primary" onClick={() => this.mostrarForm({})}>Nuevo</button>
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <VehiculosTable
                            data={this.props.data}
                            mostrarForm={this.mostrarForm}
                            errors={this.state.errors}
                            onDelete={this.onDelete}
                        />

                        <Dialog header="Gestión de Vehículos" visible={this.state.formDialog} width="700px" modal={true} onHide={this.ocultarForm}>
                        <ScrollPanel style={{width: '680px', height: '500px'}}>
    
                            <VehiculosForm
                                vehiculo={this.state.vehiculo}
                                onRefreshData={this.props.onRefreshData}
                                showSuccess={this.props.showSuccess}
                                showWarn={this.props.showWarn}
                                blockScreen={this.props.blockScreen}
                                unlockScreen={this.props.unlockScreen}
                                editForm={this.state.editForm}
                                switchCanReaload={this.switchCanReaload}
                                ocultarForm={this.ocultarForm}
                                setVehiculo={this.setVehiculo}
                                errors={this.state.errors}
                            />
                            </ScrollPanel>
                        </Dialog>
                    </div>
                </div>
            </div>
        )
    }


    ocultarForm = () => {
        this.setState({ formDialog: false, vehiculo: {} ,errors:{}})
    }

    mostrarForm = (vehiculo, editable = false) => {
        this.setState({ vehiculo, formDialog: true, editForm: editable,errors:{}})
    }

    setVehiculo = (vehiculo) => {
        this.setState({ vehiculo })
    }

    onDelete = id => {
        let url;
        url = `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_VEHICULOS}/DeleteApi/${id}`;
        this.props.blockScreen();

        http.post(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.onRefreshData({});
                    this.props.showSuccess(FRASE_VEHICULO_ELIMINADO)
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                }

                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.showWarn(error);
                this.props.unlockScreen();
            })
    }

}

const Container = Wrapper(VehiculosContainer,
    `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_VEHICULOS}/GetAll`,
    {},
    true);

ReactDOM.render(
    <Container />,
    document.getElementById('gestion_vehículos_container')
);