import React from "react";
import ReactDOM from 'react-dom';
import axios from "axios";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/dialog";
import { Checkbox } from 'primereact-v2/checkbox';
import wrapForm from "../../../Base/BaseWrapper";
import Field from "../../../Base/Field-v2";


export default class LugaresContainer extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            lugares: [],
            formDialog: false,
            lugar: {},
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
                        <LugaresTable
                            data={this.props.data}
                            mostrarForm={this.mostrarForm}
                            onDelete={this.onDelete}
                        />

                        <Dialog header="Gestión de Lugares" visible={this.state.formDialog} width="700px" modal={true} onHide={this.ocultarForm}>
                            <LugaresForm
                                lugar={this.state.lugar}
                                onRefreshData={this.props.onRefreshData}
                                showSuccess={this.props.showSuccess}
                                showWarn={this.props.showWarn}
                                blockScreen={this.props.blockScreen}
                                unlockScreen={this.props.unlockScreen}
                                editForm={this.state.editForm}
                                ocultarForm={this.ocultarForm}
                                setLugar={this.setLugar}
                                showValidation={this.props.showValidation}
                                errors={this.state.errors}
                            />
                        </Dialog>
                    </div>
                </div>
            </div>
        )
    }


    ocultarForm = () => {
        this.setState({ formDialog: false, lugar: {},errors:{}})
    }

    mostrarForm = (lugar, editable = false) => {
        this.setState({ lugar, formDialog: true, editForm: editable })
    }

    setLugar = (lugar) => {
        this.setState({ lugar })
    }

    onDelete = id => {
        let url;
        url = `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_LUGAR}/DeleteApi/${id}`;
        this.props.blockScreen();

        http.post(url, {})
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    if (data.deleted == true) {
                        this.props.onRefreshData({});
                        this.props.showSuccess(FRASE_LUGAR_ELIMINADO)
                    } else {
                        this.props.showWarn(data.errors)
                    }
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

const Container = Wrapper(LugaresContainer,
    `${config.appUrl}${MODULO_TRANSPORTE}/${CONTROLLER_LUGAR}/GetAllApi`,
    {},
    true);

ReactDOM.render(
    <Container />,
    document.getElementById('gestion_lugares_container')
);