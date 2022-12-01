import React from 'react';
import CurrencyFormat from 'react-currency-format';
export default class CabeceraDetallePrespuesto extends React.Component {
    constructor(props) {
        super(props)
        this.state = {

        }
        this.BotonesDeAprobacion = this.BotonesDeAprobacion.bind(this);
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
                                        onClick={() => this.props.MostrarFormulario()}
                                    >Editar Cabecera Presupuesto</button>

                                    {this.BotonesDeAprobacion()}
                                    {this.props.Oferta != null && this.props.Oferta.NombreEstadoAprobacion != "Emitido" &&
                                        <button
                                            style={{ marginLeft: '0.3em' }}
                                            className="btn btn-outline-primary"
                                            onClick={() => this.Redireccionar("WBS", this.props.OfertaId)}
                                        >WBS</button>
                                    }
                                    {this.props.Oferta != null && this.props.Oferta.NombreEstadoAprobacion != "Emitido" &&
                                        <button
                                            style={{ marginLeft: '0.3em' }}
                                            className="btn btn-outline-primary"
                                            onClick={() => this.Redireccionar("Computos", this.props.OfertaId)}
                                        >Computos</button>
                                    }
                                       <button
                                        style={{ marginLeft: '0.3em' }}
                                        className="btn btn-outline-primary"
                                        onClick={() => this.props.MostrarFormularioEmail()}
                                    >Confirmación Email</button>

                                    <button
                                        style={{ marginLeft: '0.3em' }}
                                        className="btn btn-outline-primary"
                                        onClick={() => this.Redireccionar("Presupuesto", this.props.Oferta.RequerimientoId)}
                                    >Regresar</button>
                                </div>
                            </div>
                            <hr />

                            <div className="row">
                                <div className="col-xs-12 col-md-6">
                                    <h6><b>Proyecto:</b> {this.props.Oferta.Proyecto ? this.props.Oferta.Proyecto.codigo : ""}</h6>
                                    <h6><b>Código:</b>  {this.props.Oferta ? this.props.Oferta.codigo : ""}</h6>
                                    <h6><b>Fecha Registro:</b>  {this.props.Oferta ? this.props.Oferta.fecha_registro : ""}</h6>
                                </div>

                                <div className="col-xs-12 col-md-6">
                                    <h6><b>Requerimiento:</b> {this.props.Oferta.Requerimiento ? this.props.Oferta.Requerimiento.codigo : ""}</h6>
                                    <h6><b>Versión:</b> {this.props.Oferta ? this.props.Oferta.version : ""}</h6>
                                    <h6><b>Tipo:</b> {this.props.Oferta ? this.props.Oferta.NombreClase : ""}</h6>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col-xs-12 col-md-6">
                                    <h6><b>Estado Aprobación:</b> {this.props.Oferta ? this.props.Oferta.NombreEstadoAprobacion : ""}</h6>
                                </div>

                                <div className="col-xs-12 col-md-6">
                                    <h6><b>Estado Emisión:</b> {this.props.Oferta ? this.props.Oferta.NombreEstadoEmision : ""}</h6>
                                </div>

                                <div className="col-xs-12 col-md-6">
                                    <h6><b>Definitivo:</b> {this.props.Oferta ? this.props.Oferta.es_final ? "Si" : "No" : ""}</h6>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <h6><b>Descripción:</b> {this.props.Oferta ? this.props.Oferta.descripcion : ""}</h6>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <div className="callout callout-info">
                                        <small className="text-muted">Ingeniería</small><br />
                                        <strong className="h4">
                                            <CurrencyFormat value={this.props.Oferta.monto_ingenieria ? this.props.Oferta.monto_ingenieria : 0.0} displayType={'text'} thousandSeparator={true} prefix={'$'} />
                                        </strong>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="callout callout-danger">
                                        <small className="text-muted">Construcción (AIU)</small><br />
                                        <strong className="h4">
                                            <CurrencyFormat value={this.props.Oferta.monto_construccion ? this.props.Oferta.monto_construccion : 0.0} displayType={'text'} thousandSeparator={true} prefix={'$'} />

                                        </strong>
                                    </div>
                                </div>
                             
                                <div className="col">
                                    <div className="callout callout-warning">
                                        <small className="text-muted">Procura</small><br />
                                        <strong className="h4">
                                            <CurrencyFormat value={this.props.Oferta.monto_suministros ? this.props.Oferta.monto_suministros : 0.0} displayType={'text'} thousandSeparator={true} prefix={'$'} />
                                        </strong>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="callout callout-primary">
                                        <small className="text-muted">Subcontratos</small><br />
                                        <strong className="h4">
                                            <CurrencyFormat value={this.props.Oferta.monto_subcontratos ? this.props.Oferta.monto_subcontratos : 0.0} displayType={'text'} thousandSeparator={true} prefix={'$'} />
                                        </strong>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="callout callout-success">
                                        <small className="text-muted">Total</small><br />
                                        <strong className="h4">
                                            <CurrencyFormat value={this.props.Oferta.monto_total ? this.props.Oferta.monto_total : 0.0} displayType={'text'} thousandSeparator={true} prefix={'$'} />
                                        </strong>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        )
    }

    Redireccionar(accion, id) {
        if (accion === "WBS") {
            window.location.href = "/Proyecto/WbsPresupuesto/Index/" + id;
        } else if (accion === "Computos") {
            window.location.href = "/Proyecto/ComputoPresupuesto/Index/" + id;
        } else if (accion === "Presupuesto") {
            window.location.href = "/Proyecto/OfertaPresupuesto/Index?RequerimientoId=" + id;
        }
    }

    BotonesDeAprobacion() {
        if (this.props.Oferta) {
            if (this.props.Oferta.estado_aprobacion == 2 || this.props.Oferta.estado_aprobacion === 0) {
                return (
                    <button
                        style={{ marginLeft: '0.3em' }}
                        className="btn btn-outline-primary"
                        onClick={() => this.props.AprobarPresupuesto()}
                    >Enviar</button>
                )
            } else if (this.props.Oferta.estado_aprobacion == 1) {
                return (
                    <button
                        style={{ marginLeft: '0.3em' }}
                        className="btn btn-outline-primary"
                        onClick={() => this.props.DesaprobarPresupuesto()}
                    >A En Proceso</button>
                )
            }
        }
    }


}