import React from "react";
import {
    MODULO_ACCESO,
    CONTROLLER_VALIDACION_REQUISITO
} from '../../../Base/Strings'
import config from '../../../Base/Config';

export default class CabeceraRequisitos extends React.Component {
    constructor(props) {
        super(props)
    }


    render() {
        return (
            <div className="row">
                <div style={{ width: '100%' }}>
                    <div className="card">
                        <div className="card-body">
                            <div className="row" align="right">
                                <div className="col">

                                    <button
                                        style={{ marginLeft: '0.3em' }}
                                        className="btn btn-outline-primary"
                                        onClick={() => this.redireccionar("REGRESAR", 0)}
                                    >Regresar</button>

                                </div>
                            </div>
                            <hr />

                            <div className="row">
                                <div className="col-xs-12 col-md-6">
                                    <h6 className="text-gray-700"><b>Tipo Identificación:</b> {this.props.colaborador.TipoIdentificacionNombre ? this.props.colaborador.TipoIdentificacionNombre : ""}</h6>
                                    <h6 className="text-gray-700"><b>Apellidos y Nombres:</b>  {this.props.colaborador.NombresApellidos ? this.props.colaborador.NombresApellidos : ""}</h6>
                                    <h6 className="text-gray-700"><b>Genero:</b> {this.props.colaborador.Genero ? this.props.colaborador.Genero : ""}</h6>
                                    <h6 className="text-gray-700"><b>Teléfonos:</b> {this.props.colaborador.Telefonos ? this.props.colaborador.Telefonos : ""}</h6>
                                </div>

                                <div className="col-xs-12 col-md-6">
                                    <h6 className="text-gray-700"><b>No. Identificación:</b> {this.props.colaborador.Identificacion ? this.props.colaborador.Identificacion : ""}</h6>
                                    <h6 className="text-gray-700"><b>Agrupación para Requisitos:</b> {this.props.colaborador.GrupoPersonal ? this.props.colaborador.GrupoPersonal : ""}</h6>
                                    <h6 className="text-gray-700"><b>Nacionalidad:</b> {this.props.colaborador.Nacionalidad ? this.props.colaborador.Nacionalidad : ""}</h6>
                                    <h6 className="text-gray-700"><b>Discapacidad:</b> {this.props.colaborador.Discapacidad ? this.props.colaborador.Discapacidad : ""}</h6>
                                </div>
                                {this.props.colaborador.Discapacidad === "SI" &&
                                    <div className="col-xs-12 col-md-6">
                                        <h6 className="text-gray-700"><b>Tipo Discapacidad:</b> {this.props.colaborador.TipoDiscapacidad ? this.props.colaborador.TipoDiscapacidad : ""}</h6>
                                    </div>
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    redireccionar = (accion, id) => {
        if (accion === "REGRESAR") {
            window.location.href = `${config.appUrl}${MODULO_ACCESO}/${CONTROLLER_VALIDACION_REQUISITO}/BuscarColaborador?source=requisitos`;
        }
    }
}