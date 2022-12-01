import React from "react";


export default class CabeceraDetalleHabitacion extends React.Component {
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
                                <h3 className="">Stock de Habitaciones</h3>
                                </div>
                                <div className="col" align="right">
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
                                    <h6><b>Identificación:</b> {this.props.proveedor ? this.props.proveedor.identificacion : ""}</h6>
                                    <h6><b>Razón Social:</b>  {this.props.proveedor ? this.props.proveedor.razon_social : ""}</h6>
                                </div>

                                <div className="col-xs-12 col-md-6">
                                    <h6><b>Código SAP:</b> {this.props.proveedor ? this.props.proveedor.codigo_sap : ""}</h6>
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
            window.location.href = "/Proveedor/Habitacion/IndexProveedores/";
        }
    }
}