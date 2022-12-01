import React from "react";
import { Button } from "primereact-v2/button";
import Field from "../../../Base/Field-v2";
import {
    NOMBRE_AUSENTISMO
} from "../../../Base/Constantes";
export default class RequisitosBusqueda extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            accion: 0,
            baja: 0,
            errors: {},
            showBajasDropDown: false
        }
    }



    render() {
        return (
            <div className="row">
                <div className="col">

                    <div>
                        <div className="row">
                            <div className="col">
                                <Field
                                    name="accion"
                                    required
                                    value={this.state.accion}
                                    label="Acción"
                                    options={this.props.catalogo_acciones}
                                    type={"select"}
                                    onChange={this.onChange}
                                    error={this.state.errors.accion}
                                    readOnly={false}
                                    placeholder="Seleccione..."
                                    filterPlaceholder="Seleccione..."
                                />
                            </div>

                            <div className="col">
                                <Field
                                    name="baja"
                                    value={this.state.baja}
                                    label="Motivo de Baja"
                                    options={this.props.catalogo_bajas}
                                    type={"select"}
                                    onChange={this.onChangeAusentismos}
                                    error={this.state.errors.baja}
                                    readOnly={!this.state.showBajasDropDown}
                                    placeholder="Seleccione..."
                                    filterPlaceholder="Seleccione..."

                                />
                            </div>

                            <div className="col-xs-12 col-md-3" style={{ paddingTop: '33px' }}>
                                <button className="btn btn-outline-primary" onClick={() => this.submit()} icon="pi pi-search">
                                    <span>
                                        <i className="fa fa-search"></i> Buscar
                                    </span>
                                </button>{" "}
                                <button className="btn btn-outline-primary" onClick={() => this.vaciar()} icon="pi pi-close">
                                    <span>
                                        <i className="fa fa-close"></i> Cancelar
                                    </span>
                                </button>
                            </div>
                        </div>

                        <hr />
                    </div>
                </div>
            </div>
        )
    }

    submit = () => {
        let accion = this.state.accion;
        let baja = this.state.baja;
        this.props.submitBusqueda(accion, baja);
    }

    vaciar = () => {
        this.setState({
            accion: 0,
            baja: 0,
        });
    }


    onChange = (name, value) => {
        if (value === this.props.bajaId) {
            this.setState({
                [name]: value,
                showBajasDropDown: true
            });
        } else {
            this.setState({
                [name]: value,
                ausentismo: 0,
                showBajasDropDown: false
            });
        }

    }

    onChangeAusentismos = (name, value) => {
        this.setState({
            [name]: value,
        });

    }
}