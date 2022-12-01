import React from "react";
import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";

export default class AtenderProblemasMasivosForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            observaciones: '',
            errors: {}
        }
    }


    render() {
        return (
            <div className="row" style={{ marginTop: "1em" }}>
                <div className="col">
                    <div className="row">
                        <div className="col-xs-12 col-md-6">
                            <h6 className="text-gray-700"><b>Cantidad Seleccionados:</b> {this.props.problemasSeleccionados.length}</h6>
                        </div>
                    </div>
                    
                    <div className="row">
                        <div className="col">
                            <Field
                                name="observaciones"
                                value={this.state.observaciones}
                                label="Observaciones"
                                type="textarea"
                                onChange={this.handleChange}
                                error={this.state.errors.observaciones}
                                edit={true}
                                readOnly={false}
                            />
                        </div>
                    </div>

                    <div className="row" align="right">
                        <div className="col">
                            <div>
                                <button
                                    className="btn btn-outline-primary mr-4"
                                    type="button" aria-expanded="false"
                                    aria-controls="collapseExample"
                                    onClick={() => this.submitForm()}
                                >Solucionar</button>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
        );
    }

    handleChange = (event) => {
        const value = event.target.type === "checkbox" ? event.target.checked : event.target.value;
        this.setState({ [event.target.name]: value });
    }

    submitForm = () => {
        this.props.blockScreen();
        let url;
        
        url = `/Seguridad/ProblemaSincronizacion/SolucionarMultiple`
        http.post(url, {
            ids: this.props.problemasSeleccionados,
            observacion: this.state.observaciones
        })
            .then((response) => {
                let data = response.data
                if (data.success === true) {
                    
                    this.props.ocultarFormularioMasivo();
                    this.props.showSuccess("Problemas de Sincronización Atendidos");
                    this.props.loadData();
                }  else {
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
}