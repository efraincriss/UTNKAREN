import React from "react";
import dateFormatter from "../../Base/DateFormatter";


export default class CabeceraTarifas extends React.Component {
    constructor(props) {
        super(props)
    }

    render() {
        return (
            <div className="row">
                <div style={{ width: '100%' }}>
                    <div className="card">
                        <div className="card-body">
                            <div className="row" >
                                <div className="col">
                                <h2 className="text-gray-500">Gestión de Tarifas Hospedaje</h2>
                                </div>
                                <div className="col" align="right">

                                    <button
                                        style={{ marginLeft: '0.3em' }}
                                        className="btn btn-outline-primary"
                                        onClick={() => this.props.redireccionar("REGRESAR", this.props.data.ProveedorId)}
                                    >Regresar</button>

                                </div>
                            </div>
                            <hr />

                            <div className="row">
                                <div className="col-xs-12 col-md-6">
                                    <h6><b>Empresa:</b> {this.props.data ? this.props.data.empresa_nombre : ""}</h6>
                                    <h6><b>Proveedor:</b>  {this.props.data ? this.props.data.proveedor_nombre : ""}</h6>
                                    <h6><b>Descripción:</b>  {this.props.data ? this.props.data.objeto : ""}</h6>
                                </div>

                                <div className="col-xs-12 col-md-6">
                                    <h6><b>Fecha de Inicio:</b> {this.props.data ? dateFormatter(this.props.data.fecha_inicio) : ""}</h6>
                                    <h6><b>Fecha de Finalización:</b> {this.props.data ? dateFormatter(this.props.data.fecha_fin) : ""}</h6>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}