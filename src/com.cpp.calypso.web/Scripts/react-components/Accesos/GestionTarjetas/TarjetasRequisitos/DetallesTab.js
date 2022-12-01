import React from "react";

export default class DetallesTab extends React.Component {

    constructor(props) {
        super(props)

        this.convertirimagen = this.convertirimagen.bind(this);
    }


    convertirimagen(binary) {
        if (binary != null) {
            return <img src={`data:image/jpeg;base64,${binary}`} height="140" width="140" />
        } else {

            return ""
        }
    }

    render() {
        return (
            <div className="row" style={{ marginTop: '2em' }}>
                <div className="col">

                    <div className="row">
                        <div className="col-sm-12">
                            <div className="card card-accent-primary border-primary">
                                <div className="card-header">
                                    <b>Datos Personales</b>
                                </div>

                                <div className="card-body">
                                    <div className="row">
                                        <div className="col-xs-12 col-md-4">
                                            <h6 className="text-gray-700"><b>Nombres:</b> {this.props.colaborador.PrimerNombre ? this.props.colaborador.PrimerNombre : ""}</h6>
                                            <h6 className="text-gray-700"><b>Cargo:</b>  {this.props.colaborador.CargoNombre ? this.props.colaborador.CargoNombre : ""}</h6>
                                            <h6 className="text-gray-700"><b>Nacionalidad:</b>  {this.props.colaborador.Nacionalidad ? this.props.colaborador.Nacionalidad : ""}</h6>
                                        </div>

                                        <div className="col-xs-12 col-md-4">
                                            <h6 className="text-gray-700"><b>Primer Apellido:</b> {this.props.colaborador.PrimerApellido ? this.props.colaborador.PrimerApellido : ""}</h6>
                                            <h6 className="text-gray-700"><b>Segundo Apellido:</b> {this.props.colaborador.SegundoApellido ? this.props.colaborador.SegundoApellido : ""}</h6>
                                        </div>
                                        
                                        <div className="col-xs-12 col-md-4">
                                        {this.convertirimagen(this.props.Fotografia)}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div className="col-sm-12 col-md-6">
                            <div className="card card-accent-warning border-warning">
                                <div className="card-header">
                                    <b>Datos Residencia</b>
                                </div>

                                <div className="card-body">
                                    <div className="row">
                                        <div className="col-xs-12 col-md-6">
                                            <h6 className="text-gray-700"><b>Pais:</b> {this.props.colaborador.PaisNombre ? this.props.colaborador.PaisNombre : ""}</h6>
                                            <h6 className="text-gray-700"><b>Provincia:</b>  {this.props.colaborador.ProvinciaNombre ? this.props.colaborador.ProvinciaNombre : ""}</h6>
                                            <h6 className="text-gray-700"><b>Calle:</b>  {this.props.colaborador.Calle ? this.props.colaborador.Calle : ""}</h6>
                                            <h6 className="text-gray-700"><b>Intersección:</b>  {this.props.colaborador.Interseccion ? this.props.colaborador.Interseccion : ""}</h6>
                                        </div>

                                        <div className="col-xs-12 col-md-6">
                                            <h6 className="text-gray-700"><b>Ciudad:</b> {this.props.colaborador.Ciudad ? this.props.colaborador.Ciudad : ""}</h6>
                                            <h6 className="text-gray-700"><b>Parroquia:</b>  {this.props.colaborador.Parroquia ? this.props.colaborador.Parroquia : ""}</h6>
                                            <h6 className="text-gray-700"><b>N° Casa:</b>  {this.props.colaborador.NumeroCasa ? this.props.colaborador.NumeroCasa : ""}</h6>
                                            <h6 className="text-gray-700"><b>Teléfono Domicilio:</b>  {this.props.colaborador.TelefonoDomicilio ? this.props.colaborador.TelefonoDomicilio : ""}</h6>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div className="col-sm-12 col-md-6">
                            <div className="card card-accent-danger border-danger">
                                <div className="card-header">
                                    <b>Datos de Contácto</b>
                                </div>

                                <div className="card-body">
                                    <div className="row">
                                        <div className="col-xs-12 col-md-6">
                                            <h6 className="text-gray-700"><b>Teléfono Celular:</b> {this.props.colaborador.TelefonoCelular ? this.props.colaborador.TelefonoCelular : ""}</h6>
                                            <h6 className="text-gray-700"><b>Fecha Inducción:</b>  {this.props.colaborador.FechaInduccion ? this.props.colaborador.FechaInduccion : ""}</h6>
                                            <h6 className="text-gray-700"><b>Correo:</b>  {this.props.colaborador.Correo ? this.props.colaborador.Correo : ""}</h6>
                                            <h6 className="text-gray-700"><b>Ingreso BPM:</b>  {this.props.colaborador.IngresoBpm ? this.props.colaborador.IngresoBpm : ""}</h6>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>
        )
    }
}