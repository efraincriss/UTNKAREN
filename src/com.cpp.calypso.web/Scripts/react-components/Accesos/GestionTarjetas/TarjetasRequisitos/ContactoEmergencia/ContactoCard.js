import React from "react";


export default class ContactoCard extends React.Component {
    constructor(props) {
        super(props)
    }



    render() {
        return (
            <div className="col-sm-12 col-md-6">
                <div className={`card card-accent-${this.props.flagClass} border-${this.props.flagClass}`}>
                    <div className="card-header">
                        <b>{this.props.contacto.Nombres}</b>
                    </div>

                    <div className="card-body">
                        <div className="row">
                            <div className="col-xs-12 col-md-6">
                                <h6 className="text-gray-700"><b>Relación:</b> {this.props.contacto.nombre_relacion}</h6>
                                <h6 className="text-gray-700"><b>Urbanización:</b>  {this.props.contacto.UrbanizacionComuna}</h6>
                                <h6 className="text-gray-700"><b>Dirección:</b>  {this.props.contacto.Direccion}</h6>
                                <h6 className="text-gray-700"><b>Celular:</b>  {this.props.contacto.Celular}</h6>
                                <h6 className="text-gray-700"><b>Identificación:</b>  {this.props.contacto.Identificacion}</h6>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}