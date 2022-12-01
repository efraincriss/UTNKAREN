import React, { Component } from 'react';
 

class ProveedorInfo extends Component {

    constructor() {
        super();
 
    }

    render() {

       
        return (

            <div className="row">
                <div className="col">
                    <h6><b>Tipo Identificación:</b> {this.props.data.tipo_identificacion_nombre}</h6>
                    <h6><b>No. Identificación:</b> {this.props.data.identificacion}</h6>
                    <h6><b>Razón Social:</b> {this.props.data.razon_social}</h6>
                 
                </div>

                <div className="col">
                    <h6><b>Código SAP:</b> {this.props.data.codigo_sap}</h6>
                    <h6><b>Estado:</b> {this.props.data.estado_nombre}</h6>
                </div>
            </div>

        );
    }
}



export default ProveedorInfo;