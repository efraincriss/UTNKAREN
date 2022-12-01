import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import moment from 'moment';


export default class DetalleRequerimiento extends React.Component{
    constructor(props){
        super(props)
        this.state = {
        }
        this.BotonesGenerarPresupuesto = this.BotonesGenerarPresupuesto.bind(this);
    }
    componentDidMount(){
    console.log('this.props',this.props)
    console.log('PropsSr',this.props.sr)
    }
    render(){
        return(
            <div>
                <div className="row">
                    <div className="col">
                        </div>

                    <div className="col" align="right">
                        <button className="btn btn-outline-primary" onClick={() => this.props.Regresar()}>Regresar</button>
                    </div>
                </div>
                <hr />

                <div className="row">
                    <div className="col-xs-12 col-md-6">
                        <h6><b>Cliente: </b>{this.props.sr ? this.props.sr.cliente : ""}</h6>
                        <h6><b>Proyecto: </b>{this.props.sr ? this.props.sr.Proyecto.descripcion_proyecto : ""}</h6>
                   
                    </div>

                    <div className="col-xs-12 col-md-6">
                    <h6><b>Fecha máxima de entrega oferta: </b>{this.props.sr && this.props.sr.fecha_maxima_presupuesto!=null? moment(this.props.sr.fecha_maxima_presupuesto).format("DD-MM-YYYY") : ""}</h6>
                    <h6><b>Última Versión: </b>{this.props.sr ? this.props.sr.ultima_version : ""}</h6>
                    </div>
                    </div>
                    <div className="row">
                    <div className="col-xs-12 col-md-6">
                        <h6><b>Descripción: </b>{this.props.sr ? this.props.sr.descripcion : ""}</h6>
                        <h6><b>Estado Presupuesto: </b>{this.props.sr ? this.props.sr.estado_presupuesto_actual : ""}</h6>
                    </div>
                    <div className="col-xs-12 col-md-6">
                      
                        <h6><b>Último Origen: </b>{this.props.sr ? this.props.sr.ultimo_origen : ""}</h6>
                        <h6><b>Última Clase: </b>{this.props.sr ? this.props.sr.ultima_clase : ""}</h6>
                    </div>
                </div>

                {this.BotonesGenerarPresupuesto()}
                
                <div className="row" >
                    <div className="col">
                    <BootstrapTable data={ this.props.presupuestos } hover={true} pagination={true}>

                        <TableHeaderColumn
                        isKey
                        dataField='codigo' 
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal' }} 
                        filter={ { type: 'TextFilter', delay: 500 } } 
                        dataSort={true}>Código</TableHeaderColumn>

                        <TableHeaderColumn 
                        dataField='fecha_registro'
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal' }} 
                        filter={ { type: 'TextFilter', delay: 500 } }
                        dataFormat={this.FechaFormat} 
                        dataSort={true}>Fecha Registro</TableHeaderColumn>

                        <TableHeaderColumn 
                        dataField='descripcion' 
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal' }} 
                        filter={ { type: 'TextFilter', delay: 500 } } 
                        dataSort={true}>Descripción</TableHeaderColumn>

                        <TableHeaderColumn
                        dataField='version' 
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal' }}
                        filter={ { type: 'TextFilter', delay: 500 } }
                        dataSort={true}
                        >Versión</TableHeaderColumn>

                        <TableHeaderColumn
                        dataField='es_final' 
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal' }}
                        filter={ { type: 'TextFilter', delay: 500 } }
                        dataSort={true}
                        dataFormat={this.EsFinal}
                        >Definitivo</TableHeaderColumn>

                        <TableHeaderColumn 
                        dataField='Operaciones'
                        width={'10%'}
                        dataFormat={this.GenerarBotones.bind(this)}></TableHeaderColumn>
                    </BootstrapTable>
                    </div>
                </div>
            </div>
        )
    }

    EsFinal(cell, row){
        return cell === true ? "SI" : "NO"
    }

    FechaFormat(cell, row){
        return moment(cell).format("YYYY-MM-DD")
    }


    GenerarBotones(cell, row){
        return(
            <div>
                <button onClick={() => this.Redireccionar(row.Id)} className="btn btn-sm btn-outline-indigo">Ver</button>
            </div>
        )
    }

    Redireccionar(id){
        window.location.href = "/Proyecto/OfertaPresupuesto/Details/" + id;
    }

    BotonesGenerarPresupuesto(){
        if (this.props.presupuestos.length > 0) {
            return(
                <div className="row" style={{marginTop: '2em', marginBottom: '1.3em'}}>
                    <div className="col" align="right">
                        <button onClick={() => this.props.NuevaVersion()} className="btn btn-outline-primary">Nueva Versión</button>
                    </div>
                </div>
            )
        } else {
            return(
                <div className="row" style={{marginTop: '2em', marginBottom: '1.3em'}}>
                    <div className="col" align="right">
                        <button onClick={() => this.props.MostrarFormulario()} className="btn btn-outline-primary">Nuevo Presupuesto</button>
                    </div>
                </div>
            )
        }
    }
}