import React from 'react';
import axios from 'axios';


import {Button} from 'primereact/components/button/Button';
import {Growl} from 'primereact/components/growl/Growl';
import {Dropdown} from 'primereact/components/dropdown/Dropdown'

export default class DisciplinaForm extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            DisciplinaId: 4,                
        }
        this.showSuccess = this.showSuccess.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.handleChangeDisciplina = this.handleChangeDisciplina.bind(this);
    }

    render(){
        return(
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <form onSubmit={this.handleSubmit}>

                    <div className="form-group">
                        <label htmlFor="label">Disciplina</label>

                        <select onChange={this.handleChangeDisciplina} className="form-control">
                            {this.getFormSelect(this.props.ListDisciplinas)}
                        </select>
                        
                    </div>
                    <Button type="submit"  label="Guardar" icon="fa fa-fw fa-folder-open"/>
                    <Button type="button"  label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.props.onHide}/>
                </form>
            </div>
        );
    }

    handleSubmit(event){
        event.preventDefault();
        axios.post("/proyecto/WbsOferta/CreateWbs",{
            AreaId: this.props.AreaId,
            DisciplinaId: this.state.DisciplinaId,
            ElementoId: 0,
            ActividadId: 0,
            estado: 1,
            observaciones: "inicial",
            fecha_inicio: "01/01/2018",
            fecha_fin: "01/01/2018",
            es_estructura: 1,
            OfertaId: document.getElementById('content').className,
            vigente: 1,
        })
        .then((response) => {
            this.props.updateData();
            this.showSuccess();
        })
        .catch((error) => {
            console.log(error);
            this.showWarn();
        });
    }

    showSuccess() {
        this.growl.show({ severity: 'success', summary: 'Realizao correctamente', detail: 'Creación de Wbs' });
    }

    showWarn() {
        this.growl.show({ severity: 'error', summary: 'Error', detail: 'No se pudieron guardar los registros' });
    }

    handleChange(event) {
        this.setState({AreaId: event.target.value});
    }

    handleChangeDisciplina(event) {
        this.setState({DisciplinaId: event.target.value});
    }

    getFormSelect(list){      
        return(
            
            list.map((item) => {
                return (
                    <option key={item.Id} value={item.Id}>{item.nombre}</option>
                )         
            })
            
        );
    }
}