import React from 'react';
import CreateOrEditActividad from '../forms/CreateOrEditActividad';
import moment from 'moment';
import axios from 'axios';


import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class ActividadTable extends React.Component{
    constructor(props){
        super(props);

        this.state = {
            visible_actividades_form: false,
            key_form: 89247,
         }
        this.onHideVisibleActividadForm = this.onHideVisibleActividadForm.bind(this);
        this.showForm = this.showForm.bind(this);
        this.onDelete = this.onDelete.bind(this);
    }

    generateButton(cell, row){
        return(
            <div>
                <button className="btn btn-outline-primary" onClick={this.showForm} style={{float:'left', marginRight:'0.3em'}}>Editar</button>
                <button className="btn btn-outline-danger" onClick={() => {if (window.confirm('Estás seguro de eliminar este Wbs?')) this.onDelete(row.Id)}} style={{float:'left', marginRight:'0.3em'}}>Eliminar</button>
            </div>
        )
    }

    dateFormat(cell, row){
        if(cell === null){
            return(
                "dd/mm/yy"
            )
        }
        return(
            moment(cell).format('DD-MM-YYYY')
        )
    }

    render(){
        return(
         <div>
             <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>

             <BootstrapTable data={this.props.data} hover={true} pagination={ true } >
                    <TableHeaderColumn dataField="nivel_nombre" filter={ { type: 'TextFilter', delay: 500 } } isKey={true} dataAlign="center" dataSort={true}>Actividad</TableHeaderColumn>
                    <TableHeaderColumn dataField="observaciones" filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}>Observaciones</TableHeaderColumn>
                    <TableHeaderColumn dataField="fecha_inicial" dataFormat={this.dateFormat.bind(this)} filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}>Fecha de Inicio</TableHeaderColumn>
                    <TableHeaderColumn dataField="fecha_final" dataFormat={this.dateFormat.bind(this)} filter={ { type: 'TextFilter', delay: 500 } } dataSort={true}>Fecha de Fin</TableHeaderColumn>
                    <TableHeaderColumn dataField='Operaciones' dataFormat={this.generateButton.bind(this)}>Operaciones</TableHeaderColumn>
                </BootstrapTable>


        

            <Dialog header="Actividades" visible={this.state.visible_actividades_form} width="350px" modal={true} onHide={this.onHideVisibleActividadForm}>
                    <CreateOrEditActividad 
                        updateActividad={this.props.updateActividad} 
                        data={this.props.data}
                        dataItem={this.props.dataItem}
                        key={this.state.key_form}
                        onHide={this.onHideVisibleActividadForm}
                        showSuccess={this.props.showSuccess}
                        showWarn={this.props.showWarn}
                        disciplinas={this.props.disciplinas}
                    />    
            </Dialog>
         </div>   
        )
    }


    onHideVisibleActividadForm(event){
        this.setState({visible_actividades_form: false});
    }

    onDelete(id){
        event.preventDefault();
        axios.post("/proyecto/Wbs/Delete/"+id,{})
        .then((response) => {
            if(response.data == "Ok"){            
                this.props.showSuccess("Se eliminó el registro");
                this.props.updateData();            
                this.props.reset();
            } else {  
                this.props.showWarn("La actividad tiene computos registrados");
            }
        })
        .catch((error) => {
            this.props.showWarn("No se puedo eliminar el registro");
            
        });
    }

    showForm(){
        this.setState({
            visible_actividades_form: true,
            key_form: Math.random()
        })
    }
}