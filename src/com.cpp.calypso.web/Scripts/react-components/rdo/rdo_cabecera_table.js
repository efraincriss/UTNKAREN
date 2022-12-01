import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import moment from 'moment';
import axios from 'axios';
import {Growl} from 'primereact/components/growl/Growl';
export default class RdoCabeceraTable extends React.Component{
    constructor(props){
        super(props);
        this.handleRedirect = this.handleRedirect.bind(this);
        this.CodigoProyectoFormat = this.CodigoProyectoFormat.bind(this);
        this.NombreProyectoFormat = this.NombreProyectoFormat.bind(this);
        this.DescripcionProyectoFormat = this.DescripcionProyectoFormat.bind(this);
        this.FechaFormat = this.FechaFormat.bind(this);
        this.DefinitivaFormat = this.DefinitivaFormat.bind(this);
    }

    generateButton(cell, row){
        return(
            <div>
                <button onClick={() => this.handleRedirect(row.Id)} className="btn btn-sm btn-outline-indigo">Ver</button>{" "}
                <button onClick={() =>{if (window.confirm(`Esta acción eliminará el registro y todo su contenido, ¿Desea continuar?`))this.EliminarRDO(row.Id);}} className="btn btn-sm btn-outline-danger">Eliminar</button>
            </div>
        )
    }

    render(){
        return(
            <div>
                  <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <BootstrapTable data={this.props.data} hover={true} pagination={ true } >
                    <TableHeaderColumn
                    isKey={true}
                    dataField="Proyecto"
                    width={'20%'}
                    filter={ { type: 'TextFilter', delay: 500 } }
                    dataFormat={this.NombreProyectoFormat}
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                    dataSort={true}>Nombre Proyecto</TableHeaderColumn>

                    <TableHeaderColumn 
                    dataField="Proyecto" 
                    filter={ { type: 'TextFilter', delay: 500 } }
                    dataFormat={this.DescripcionProyectoFormat}
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                    dataSort={true}>Descripción Proyecto</TableHeaderColumn>

                    <TableHeaderColumn 
                    dataField="fecha_rdo"
                    width={'12%'}
                    dataFormat={this.FechaFormat}
                    filter={ { type: 'TextFilter', delay: 500 } } 
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                    dataSort={true}>Fecha RDO</TableHeaderColumn>

                    <TableHeaderColumn 
                    dataField="version"
                    width={'8%'}
                    filter={ { type: 'TextFilter', delay: 500 } } 
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                    dataSort={true}>Versión</TableHeaderColumn>

                    <TableHeaderColumn 
                    dataField="FormatEstado"
                    width={'8%'}                
                    filter={ { type: 'TextFilter', delay: 500 } } 
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}
                    dataSort={true}>Definitiva</TableHeaderColumn>

                    <TableHeaderColumn 
                    dataField='Operaciones'
                    width={'13%'}
                    dataFormat={this.generateButton.bind(this)}></TableHeaderColumn>
                </BootstrapTable> 
            </div>
        )
    }

    CodigoProyectoFormat(cell, row){
        return cell.codigo
    }

    NombreProyectoFormat(cell, row){
        return cell.nombre_proyecto
    }

    DescripcionProyectoFormat(cell, row){
        return cell.nombre_proyecto
    }

    handleRedirect(id){
        window.location.href = "/Proyecto/RdoCabecera/Details/" + id;
    }
    EliminarRDO=(id)=>{
        this.props.Block();
        axios.post("/Proyecto/RdoCabecera/Delete/"+id,{
       
        })
        .then((response) => {
            if(response.data == "OK"){
                this.growl.show({life: 5000,  severity: 'success', summary: 'Proceso exitoso!', detail: 'Eliminado Correctamente' });
                this.props.updateData();
                this.props.Unlock();
            } 
        })
        .catch((error) => {
            console.log(error)
            this.props.showWarn();
        });
    }


    FechaFormat(cell, row){
        
        return moment(cell).format("DD/MM/YYYY")
    }

    DefinitivaFormat(cell, row){
        if(cell === false){
            return "NO"
        }
        return "SI"
    }
}