import React from "react";
import ReactDOM from 'react-dom';
import Wrapper from "../../Base/BaseWrapperApi";
import config from "../../Base/Config";
import http from "../../Base/HttpService";
import { Dialog } from 'primereact-v2/dialog';
import {
    MODULO_TRANSPORTE,
    CONTROLLER_PARADA,
    FRASE_PARADA_ELIMINADA
} from "../../Base/Strings";
import ParadasTable from "./ParadasTable";
import ParadasForm from "./ParadasForm";



export default class ParadasContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            paradas: [],
            formDialog: false,
            parada: {},
            editForm: false,

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
                        <ParadasTable
                            data={this.props.data}
                            mostrarForm={this.mostrarForm}
                            onDelete={this.onDelete}
                        />

                        <Dialog header="Gestión de Paradas" visible={this.state.formDialog} width="700px" modal={true} onHide={this.ocultarForm}>
                            <ParadasForm
                                parada={this.state.parada}
                                onRefreshData={this.props.onRefreshData}
                                showSuccess={this.props.showSuccess}
                                showWarn={this.props.showWarn}
                                showValidation={this.props.showValidation}
                                blockScreen={this.props.blockScreen}
                                unlockScreen={this.props.unlockScreen}
                                editForm={this.state.editForm}
                                ocultarForm={this.ocultarForm}
                                setParada={this.setParada}
                            />
                        </Dialog>
                    </div>
                </div>
            </div>
        )
    }


    ocultarForm = () => {
        this.setState({ formDialog: false, parada: {} })
    }

    mostrarForm = (parada, editable = false) => {
        this.setState({ parada, formDialog: true, editForm: editable })
    }

    setParada = (parada) => {
        this.setState({ parada })
    }

    onDelete = id => {
        let url;
        url = `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_PARADA}/DeleteApi/${id}`;
        this.props.blockScreen();

        http.post(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.props.onRefreshData({});
                    this.props.showSuccess(FRASE_PARADA_ELIMINADA)
                } else {
                    this.props.showWarn(data.errors)
                    //var message = $.fn.responseAjaxErrorToString(data);
                    //this.props.showWarn(message);
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

const Container = Wrapper(ParadasContainer,
    `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_PARADA}/GetAllApi`,
    {},
    true);

ReactDOM.render(
    <Container />,
    document.getElementById('gestion_paradas_container')
);