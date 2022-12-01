import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import moment from 'moment';
import CurrencyFormat from 'react-currency-format';

import {Growl} from 'primereact/components/growl/Growl';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import {Dialog} from 'primereact/components/dialog/Dialog';
import NuevosCertificados from './GR/NuevosCertificados';

class DetalleGR extends Component {
    constructor(props) {
        super(props)
        
        this.state = {
            blocking: true,
            visible: false,
            data: [], // DetallesGR,
            certificados: [], //CertificadosNuevos
            key: 6523,
            monto_total: 0,
        }
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.GetDetalles = this.GetDetalles.bind(this);
        this.CodigoFormat = this.CodigoFormat.bind(this);
        this.NumeroCertificadoFormat = this.NumeroCertificadoFormat.bind(this);
        this.FechaFormat = this.FechaFormat.bind(this);
        this.MontoFormat = this.MontoFormat.bind(this);
        this.onHide = this.onHide.bind(this);
        this.GetCertificados = this.GetCertificados.bind(this);
        this.DescripcionFormat = this.DescripcionFormat.bind(this);
        this.Block = this.Block.bind(this)
        this.ClearCertificados = this.ClearCertificados.bind(this);
        this.GetMonto = this.GetMonto.bind(this);
        this.DeleteButton = this.DeleteButton.bind(this);
        this.Delete = this.Delete.bind(this);
    }

    componentDidMount() {
        this.GetDetalles();
        this.GetCertificados();
        this.GetMonto();
    }
    

    render() {
        return (
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>

                <BlockUi tag="div" blocking={this.state.blocking}>
                    <div className="row">
                        <div className="col">
                        <h6><b>Monto Total: </b>{this.state.monto_total}</h6>
                        </div>
                        <div className="col" align="right">
                            <button onClick={() => this.setState({visible:true})} className="btn btn-outline-primary">Nuevo</button>
                        </div>
                    </div>
                    <hr/>
                    <BootstrapTable data={ this.state.data } hover={true} pagination={true} >
                        

                        <TableHeaderColumn 
                        dataField='Certificado'
                        width={'12%'}
                        dataFormat={this.CodigoFormat}
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }} 
                        filter={ { type: 'TextFilter', delay: 500 } } 
                        dataSort={true}>Proyecto</TableHeaderColumn>

                        <TableHeaderColumn 
                        dataField='Certificado'
                        dataFormat={this.DescripcionFormat}
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }} 
                        filter={ { type: 'TextFilter', delay: 500 } } 
                        dataSort={true}>Descripción</TableHeaderColumn>


                        <TableHeaderColumn 
                        dataField='Certificado'
                        width={'20%'}
                        dataFormat={this.NumeroCertificadoFormat}
                        filter={ { type: 'TextFilter', delay: 500 } } 
                        isKey
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }} 
                        dataSort={true}> # Certificado</TableHeaderColumn>

                        <TableHeaderColumn 
                        dataField='Certificado'
                        width={'12%'}
                        dataFormat={this.FechaFormat}
                        tdStyle={{ whiteSpace: 'normal' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }} 
                        filter={ { type: 'TextFilter', delay: 500 } } 
                        dataSort={true}>Fecha</TableHeaderColumn>


                        <TableHeaderColumn 
                        dataField='Certificado'
                        width={'20%'}
                        dataFormat={this.MontoFormat}
                        tdStyle={{ textAlign: 'right' }}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }} 
                        filter={ { type: 'TextFilter', delay: 500 } } 
                        dataSort={true}>Monto Certificado</TableHeaderColumn>

                        <TableHeaderColumn 
                        dataField='Id'
                        width={'8%'}
                        dataFormat={this.DeleteButton}
                        thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}></TableHeaderColumn>

                    </BootstrapTable>

                    <Dialog header="Ingreso GR" visible={this.state.visible} width="900px" modal={true} onHide={this.onHide}>
                        <NuevosCertificados 
                        key={this.state.key}
                        certificados={this.state.certificados}
                        successMessage={this.successMessage}
                        warnMessage={this.warnMessage} 
                        GetCertificados={this.GetCertificados}
                        GetDetalles={this.GetDetalles}
                        GetMonto={this.GetMonto}
                        onHide={this.onHide}
                        Block={this.Block}
                        ClearCertificados={this.ClearCertificados}/>
                    </Dialog>
                </BlockUi>
            </div>
        );
    }

    onHide(){
        this.setState({visible: false})
    }
    CodigoFormat(cell, row){
        return cell.Proyecto.codigo
    }

    NumeroCertificadoFormat(cell, row){
        return cell.numero_certificado
    }

    FechaFormat(cell, row){
        return moment(cell.fecha_emision).format("DD/MM/YYYY")
    }

    MontoFormat(cell, row){
        return <CurrencyFormat value={cell.monto_certificado} displayType={'text'} thousandSeparator={true} prefix={'$'} />
    }

    DescripcionFormat(cell, row){
        return cell.Proyecto.descripcion_proyecto
    }

    DeleteButton(cell, row){
        return(
            <button className="btn btn-outline-danger"
            onClick={() => { if (window.confirm('Estás segur@?')) this.Delete(cell) } }>
                <i className="fa fa-trash"></i>
            </button>
        )
    }

    Block(){
        this.setState({BlockUi: true})
    }

    ClearCertificados(){
        this.setState({key: Math.random()})
    }



    GetDetalles(){
        axios.post("/proyecto/DetalleGR/GetDetalles/" + document.getElementById('GRId').className,{
        })
        .then((response) => {
            this.setState({data: response.data})
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    GetCertificados(){
        axios.post("/proyecto/DetalleGR/GetCertificados/" + document.getElementById('ProyectoId').className,{
        })
        .then((response) => {
            this.setState({certificados: response.data})
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    GetMonto(){
        axios.post("/proyecto/GR/GetGrMonto/" + document.getElementById('GRId').className,{
        })
        .then((response) => {
            this.setState({monto_total: response.data, blocking: false})
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    Delete(id){
        this.setState({blocking: true})
        axios.post("/proyecto/DetalleGR/Delete/" + id,{
        })
        .then((response) => {
            this.GetDetalles();
            this.GetCertificados();
            this.GetMonto();
            this.successMessage("Registro eliminado");
        })
        .catch((error) => {
            console.log(error);    
        });
    }



    showSuccess() {
        this.growl.show({life: 5000,  severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({life: 5000,  severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg){
        this.setState({message: msg}, this.showSuccess)
    }

    warnMessage(msg){
        this.setState({message: msg}, this.showWarn)
    }
    
}

ReactDOM.render(
    <DetalleGR />,
    document.getElementById('content')
  );