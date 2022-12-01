import React from 'react';
import axios from 'axios';
import BlockUi from 'react-block-ui';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import {Sidebar} from 'primereact/components/sidebar/Sidebar';



export default class DetalleItemsTable extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            visibleBottom: false,
            colaborador: '',
            horas: 0,
            fecha_registro: '',
            especialidad: '',
            blocking: false,
        }
        this.generateButton = this.generateButton.bind(this);
        this.GetDetallesData = this.GetDetallesData.bind(this);
    }


    generateButton(cell, row){
        return(
            <div>
                <button style={{marginRight: '0.5em'}} onClick={() => this.GetDetallesData(row.Id, row.nombre_colaborador, row.nombre_especialidad)} className="btn btn-outline-success btn-sm">
                    <i className="fa fa-eye"></i>
                </button>
                <button style={{marginRight: '0.5em'}} onClick={() => this.props.SetDataItem(row.Id)} className="btn btn-outline-primary btn-sm">
                    <i className="fa fa-edit"></i>
                </button>
                <button 
                    onClick={() => {if (window.confirm('Estás seguro?')) this.props.DeleteItem(row.Id,"ITEM",row.DetalleAvanceIngenieriaId)}}
                    className="btn btn-outline-danger btn-sm">
                    <i className="fa fa-trash-o"></i>
                </button>
            </div>
        )
    }


    render(){
        return(
            <div>
                <BlockUi tag="div" blocking={this.state.blocking}>
                    
                    <BootstrapTable data={this.props.data}  hover={true} pagination={ true } >
                        
                        <TableHeaderColumn width={'20%'}  dataField="nombre_colaborador"  filter={ { type: 'TextFilter', delay: 500 } } isKey={true} dataAlign="center" dataSort={true}>Nombre Colaborador</TableHeaderColumn>
                        <TableHeaderColumn width={'20%'}  dataField="cantidad_horas" filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}># Horas</TableHeaderColumn>
                        
                        <TableHeaderColumn width={'20%'}  dataField='nombre_especialidad' filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}>Especialidad</TableHeaderColumn>
                        <TableHeaderColumn width={'20%'}  dataField='especialidad' dataFormat={this.generateButton.bind(this)}>Operaciones</TableHeaderColumn>
                    </BootstrapTable>

                    <Sidebar visible={this.state.visibleBottom} position="bottom" baseZIndex={1000000} onHide={(e) => this.setState({visibleBottom: false})} className="ui-sidebar-sm">
                        <h1 style={{fontWeight:'normal'}} className="text-gray-400" >Detalle Avance Ingeniería</h1>
                        <div className="row">
                            <div className="col-xs-12 col-md-8">
                                <h6 className="text-blue"><b className="text-gray-400">Colaborador:</b> {this.state.colaborador}</h6>
                                <h6 className="text-blue"><b className="text-gray-400">Cantidad Horas:</b> {this.state.horas}</h6>
                                

                            </div>

                            <div className="col-xs-12 col-md-3">
                                <h6 className="text-blue"><b className="text-gray-400">Especialidad:</b> {this.state.especialidad}</h6>
                                <h6 className="text-blue"><b className="text-gray-400">Fecha Registro:</b> {this.state.fecha_registro}</h6>
                            </div>
                        </div>
                    </Sidebar>
                </BlockUi>
            </div>
        )
    }

    GetDetallesData(id, colaborador, especialidad){
        this.setState({blocking: true})
        axios.post("/Proyecto/DetalleItemIngenieria/DetailsApi/" + id,{})
        .then((response) => {
            this.setState({
                colaborador: colaborador,
                horas: response.data.cantidad_horas,
                fecha_registro: response.data.fecha_registro,
                especialidad: response.data.especialidad,
                blocking: false,
                visibleBottom: true,
            })
            
        })
        .catch((error) => {
            console.log(error);    
        });
    }
}