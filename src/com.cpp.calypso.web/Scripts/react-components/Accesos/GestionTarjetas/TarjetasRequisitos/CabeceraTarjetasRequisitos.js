import React from "react";
import {
    MODULO_ACCESO
} from '../../../Base/Strings'
import config from '../../../Base/Config';

export default class CabeceraTarjetasRequisitos extends React.Component{
    constructor(props){
        super(props)
    }


    render() {
        return (
            <div className="row">
                <div style={{width:  '100%'}}>
                    <div className="card">
                        <div className="card-body">
                            <div className="row" align="right">
                                <div className="col">

                                    <button 
                                    style={{marginLeft: '0.3em'}}
                                    className="btn btn-outline-primary" 
                                    onClick={() => this.redireccionar("REGRESAR", 0)}
                                    >Regresar</button>

                                </div>
                            </div>
                            <hr />

                            <div className="row">
                                <div className="col-xs-12 col-md-6">
                                    <h6 className="text-gray-700"><b>Tipo Identificación:</b> {this.props.colaborador.TipoIdentificacionNombre ? this.props.colaborador.TipoIdentificacionNombre : ""}</h6>
                                    <h6 className="text-gray-700"><b>Nombres-Apellidos:</b>  {this.props.colaborador.NombresApellidos ? this.props.colaborador.NombresApellidos: ""}</h6>
                                </div>

                                <div className="col-xs-12 col-md-6">
                                    <h6 className="text-gray-700"><b>Identificación:</b> {this.props.colaborador.Identificacion ? this.props.colaborador.Identificacion: ""}</h6>
                                    <h6 className="text-gray-700"><b>Departamento:</b> {this.props.colaborador.Departamento ? this.props.colaborador.Departamento: ""}</h6>
                                </div>
                            </div>                       
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    redireccionar = (accion, id) => {
        if(accion === "REGRESAR"){
            window.location.href = `/Accesos/TarjetaAcceso/BuscarColaborador?source=tarjetas`;
        }
    }
}