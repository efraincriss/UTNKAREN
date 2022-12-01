import React from "react";
import {
    MODULO_DOCUMENTOS, CONTROLLER_CARPETA, CONTROLLER_DOCUMENTO
} from '../../Base/Strings'
import config from '../../Base/Config';

export class CabeceraDocumento extends React.Component {
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
                                    <h6 className="text-gray-700"><b>Código Contrato:</b> {this.props.carpeta.Codigo ? this.props.carpeta.Codigo : ""}</h6>
                                </div>

                                <div className="col-xs-12 col-md-6">
                                    <h6 className="text-gray-700"><b>Nombre Contrato:</b> {this.props.carpeta.NombreCorto ? this.props.carpeta.NombreCorto : ""}</h6> 
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    redireccionar = (accion, id) => {
        if (accion === "REGRESAR") {
            window.location.href = `${config.appUrl}${MODULO_DOCUMENTOS}/${CONTROLLER_CARPETA}/Index`
        }
    }
}