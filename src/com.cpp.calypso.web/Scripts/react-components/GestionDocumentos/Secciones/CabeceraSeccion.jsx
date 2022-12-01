import React from "react";
import {
    MODULO_DOCUMENTOS, CONTROLLER_CARPETA, CONTROLLER_DOCUMENTO
} from '../../Base/Strings'
import config from '../../Base/Config';

export default class CabeceraSeccion extends React.Component {
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
                                    <h6 className="text-gray-700"><b>Código Contrato:</b> {this.props.documento.Carpeta ? this.props.documento.Carpeta.Codigo : ""}</h6>
                                    <h6 className="text-gray-700"><b>Código Documento:</b> {this.props.documento.Codigo ? this.props.documento.Codigo : ""}</h6>
                                    <h6 className="text-gray-700"><b>Tipo Documento:</b> {this.props.documento.TipoDocumentoNombre ? this.props.documento.TipoDocumentoNombre : ""}</h6>
                                </div>

                                <div className="col-xs-12 col-md-6">
                                    <h6 className="text-gray-700"><b>Nombre Contrato:</b> {this.props.documento.Carpeta ? this.props.documento.Carpeta.NombreCorto : ""}</h6>
                                    <h6 className="text-gray-700"><b>Nombre Documento:</b> {this.props.documento.Nombre ? this.props.documento.Nombre : ""}</h6>
                                    <h6 className="text-gray-700"><b>Número de páginas:</b> {this.props.documento.CantidadPaginas ? this.props.documento.CantidadPaginas : ""}</h6>
                                    
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
            window.location.href = `${config.appUrl}${MODULO_DOCUMENTOS}/${CONTROLLER_DOCUMENTO}/Detalles?contratoId=${this.props.documento.Carpeta.Id}`
        }
    }
}