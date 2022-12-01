import React from 'react';
import axios from 'axios';
import {Button} from 'primereact/components/button/Button';
import {Growl} from 'primereact/components/growl/Growl';
import { Textbox } from 'react-inputs-validation/lib/components';

const estilo = { height: '25px'}

export default class ItPadreform extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            codigo: '',
            nombre: '',
            descripcion: '',
            UnidadId: 0,
            para_oferta: 0,
            codigoForm: '',
            nombreForm: '',
            descripcionForm: '',
            UnidadIdForm: 0,
            para_ofertaForm: 0,
            IdForm: 0,
            vigente: true,
            itemPadreForm: '.',

        }
        this.showSuccess = this.showSuccess.bind(this);
        this.handleSubmitPadre = this.handleSubmitPadre.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showWarn = this.showWarn.bind(this);
            }


    render(){
        return(
            <div>

            <div className="row">
                <div style={{width:  '100%'}}>

                    <ul className="nav nav-tabs" id="gestion_tabs" role="tablist">
                        <li className="nav-item">
                            <a className="nav-link active" id="gestion-tab" data-toggle="tab" href="#gestion" role="tab" aria-controls="home" aria-expanded="true">Crear</a>
                        </li>
                     
                    </ul>

                    <div className="tab-content" id="myTabContent">
                        <div className="tab-pane fade show active" id="gestion" role="tabpanel" aria-labelledby="gestion-tab">
                        <form onSubmit={this.handleSubmitPadre}>
                            <div className="form-group">
                                <label htmlFor="label">Código</label> 

                                 <input style={estilo}
                                type="number"
                                name="codigo"
                                value={this.state.codigo}
                                onChange={this.handleChange}
                                className="form-control"
                                min="1"
                                max="99"
                                maxLength="3"
                                step="1.0"
                                required/> 
                                                                                        
                            </div>

                            <div className="form-group">
                                <label htmlFor="label">Nombre</label>

                                <input 
                                type="text"
                                name="nombre"
                                value={this.state.nombre}
                                onChange={this.handleChange}
                                className="form-control"
                                required
                                />   
                            </div>

                            <div className="form-group">
                                <label htmlFor="label">Descripción</label>

                                <input 
                                type="text"
                                name="descripcion"
                                value={this.state.descripcion}
                                onChange={this.handleChange}
                                className="form-control"
                                required
                                />   
                            </div>
                                   <Button type="submit" label="Guardar" icon="fa fa-fw fa-folder-open" />
                                    <Button type="button"  label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.props.onHidePadre}/>
                                 
                            </form>
                        </div>

                    </div>
                </div>
            </div>

                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                     
            </div>
        );
    }

    handleSubmitPadre(event){
        event.preventDefault();
        axios.post("/proyecto/Item/CrearItemPadre",{
            codigo: this.state.codigo,
            item_padre: this.state.itemPadreForm,
            nombre: this.state.nombre,
            descripcion: this.state.descripcion,
            UnidadId: this.state.UnidadId,
            para_oferta: false,
            vigente: 1,
        })
        .then((response) => {
            this.props.updateData();
           var r= response.data;
           
            if(r=="Guardado"){
                console.log("entro guardado");
                this.showSuccess(); 
            }
            if(r=="Error"){
                console.log("entro error");
                this.showWarn(); 
            }
            this.setState({
                codigo: '',
                nombre: '',
                descripcion: '',
                UnidadIdForm: 0,
                
             })
           
                    })
        .catch((error) => {
               this.showWarn();
        });
    }
      
    showSuccess() {
        this.growl.show({ severity: 'success', summary: 'Realizao correctamente', detail: 'Item Creado Correctamente' });
    }

    showWarn() {
        this.growl.show({ severity: 'error', summary: 'Error', detail: 'El Item Ya Existe' });
    }

    handleChange(event) {
        this.setState({[event.target.name]: event.target.value});
    }
}

