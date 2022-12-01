import React from "react";
import Field from "../../Base/Field-v2";
import http from "../../Base/HttpService";

export default class AtenderProblemaForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            observaciones: props.problema.Observaciones ? props.problema.Observaciones : '',
            solucionado: false,
            errors: {}
        }
    }

    componentWillReceiveProps(prevProps) {
        this.setState({
            observaciones: prevProps.problema.Observaciones ? prevProps.problema.Observaciones : ''
        })
    }


    render() {
        return (
            <div className="row" style={{ marginTop: "1em" }}>
                <div className="col">
                    <div className="row">
                        <div className="col-xs-12 col-md-6">
                            <h6 className="text-gray-700"><b>Fecha:</b> {this.props.problema.Fecha ? this.props.problema.Fecha : ""}</h6>
                            <h6 className="text-gray-700"><b>Fuente:</b>  {this.props.problema.Fuente ? this.props.problema.Fuente : ""}</h6>
                        </div>

                        <div className="col-xs-12 col-md-6">
                            <h6 className="text-gray-700"><b>Entidad:</b> {this.props.problema.Entidad ? this.props.problema.Entidad : ""}</h6>
                            <h6 className="text-gray-700"><b>Usuario Id:</b> {this.props.problema.UsuarioId ? this.props.problema.UsuarioId : ""}</h6>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-xs-12 col-md-6">
                            <h6 className="text-gray-700"><b>UID:</b>  {this.props.problema.Uid ? this.props.problema.Uid : ""}</h6>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-xs-12 col-md-12">
                            <h6 className="text-gray-700"><b>Resumen:</b> {this.props.problema.Resumen ? this.props.problema.Resumen : ""}</h6>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-xs-12 col-md-12">
                            <p>
                                <a class="btn btn-outline-primary" data-toggle="collapse" href="#collapseExample" role="button" aria-expanded="false" aria-controls="collapseExample">
                                    Ver Más
                                </a>
                                
                            </p>
                            <div class="collapse" id="collapseExample">
                                <div class="card card-body">
                                    {this.props.problema.Problema ? this.props.problema.Problema : ""}
                                </div>
                            </div>
                        </div>
                    </div>



                    {/* <div className="row">
                        <div className="col">
                            <Field
                                name="solucionado"
                                label="Solucionado"
                                labelOption="  (Si/No)"
                                type="checkbox"
                                value={this.state.solucionado}
                                edit={!this.state.solucionado}
                                readOnly={this.state.solucionado}
                                error={this.state.errors.solucionado}
                                onChange={this.handleChange}
                            />
                        </div>
                    </div> */}
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
        if (this.props.editando) {
            console.log("Editando")
            this.atenderProblema("Problema de Sincronización Editado")
        } else {
            console.log("atendiendo")
            this.atenderProblema("Problema de Sincronización Atendido")
        }
    }


    atenderProblema = (mensaje) => {
        this.props.blockScreen();
        let url;

        url = `/Seguridad/ProblemaSincronizacion/Solucionar`
        http.post(url, {
            problemaSincronizacionId: this.props.problema.Id,
            observacion: this.state.observaciones
        })
            .then((response) => {
                let data = response.data
                if (data.success === true) {

                    this.props.toggleModal(true);
                    this.props.showSuccess(mensaje);
                    this.props.loadData();
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
}