import React from 'react';
import axios from 'axios';
import {Button} from 'primereact/components/button/Button';
import {Growl} from 'primereact/components/growl/Growl';
import { Textbox } from 'react-inputs-validation/lib/components';
import { Dropdown } from 'primereact/components/dropdown/Dropdown'

const estilo = { height: '25px'}

export default class ItemForm extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            codigo: '',
            nombre: '',
            descripcion: '',
            UnidadId: 0,
            para_oferta: false,
            codigoForm: '',
            nombreForm: '',
            descripcionForm: '',
            UnidadIdForm: 0,
            para_ofertaForm: false,
            IdForm: 0,
            vigente: true,
            itemPadreForm: '',
            message: '',
            GrupoId:1,

        }
        this.showSuccess = this.showSuccess.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.deleteItem = this.deleteItem.bind(this);
        this.getItemData = this.getItemData.bind(this);
        this.handleEdit = this.handleEdit.bind(this);
        this.showSuccessE = this.showSuccess.bind(this);
        this.showWarnE = this.showWarn.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    componentDidMount(){
        this.getItemData();
    }

    render(){
        return(
            <div>

            <div className="row">
                <div style={{width:  '100%'}}>

                    <ul className="nav nav-tabs" id="gestion_tabs" role="tablist">
                        
                        <li className="nav-item">
                            <a className="nav-link active" id="jerarquia-tab" data-toggle="tab" href="#jerarquia" role="tab" aria-controls="profile">Editar</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" id="gestion-tab" data-toggle="tab" href="#gestion" role="tab" aria-controls="home" aria-expanded="true">Crear Hijo</a>
                        </li>
                    </ul>

                    <div className="tab-content" id="myTabContent">
                    
                        <div className="tab-pane fade" id="gestion" role="tabpanel" aria-labelledby="gestion-tab">
                        <form onSubmit={this.handleSubmit}>
                            <div className="form-group">
                                <label htmlFor="label">Código</label> <br />

                                 <div className="row justify-content-md-center">
                              <div className="col-sm-2">
                              {this.props.itemPadre}
                              </div>
                              <div className="col" >
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
                              </div>
                                                            
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
                            <div className="form-group">
                            <label>
          Es Para Oferta:<br/>
          <input
            name="para_oferta"
            type="checkbox"
            checked={this.state.para_oferta}
            onChange={this.handleInputChange} />
        </label>
                            </div>

                            <div className="form-group">
                                <label htmlFor="label">Unidad</label>
                                            
                         

                                <Dropdown
                                                        value={this.state.UnidadId}
                                                        options={this.props.unidades}
                                                        onChange={(e) => { this.setState({ UnidadId: e.value }) }}
                                                        filter={true} filterPlaceholder="Selecciona una Unidad"
                                                        filterBy="label,value" placeholder="Selecciona una Unidad"
                                                        style={{ width: '100%', heigh: '18px' }}
                                                        required
                                                    />
                                    </div>
                                    <div className="form-group">
                                <label htmlFor="label">Grupo Item</label>
                                            
                         

                                <Dropdown
                                                        value={this.state.GrupoId}
                                                        options={this.props.listaitems}
                                                        onChange={(e) => { this.setState({ GrupoId: e.value }) }}
                                                        filter={true} filterPlaceholder="Selecciona un Grupo"
                                                        filterBy="label,value" placeholder="Selecciona un Grupo"
                                                        style={{ width: '100%', heigh: '18px' }}
                                                        required
                                                    />
                                    </div>
                                    <Button type="submit" label="Guardar" icon="fa fa-fw fa-folder-open" />
                                    <Button type="button"  label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.props.onHide}/>
                                 
                            </form>
                        </div>


                        <div className="tab-pane fade show active" id="jerarquia" role="tabpanel" aria-labelledby="jerarquia-tab">
                        <form onSubmit={this.handleEdit}>
                            <div className="form-group">
                                <label htmlFor="label">Código</label> <br />
                              <div className="row justify-content-md-center">
                              <div className="col-sm-2">
                              {this.state.itemPadreForm}
                              </div>
                              <div className="col">
                              <input  style={estilo}
                                type="number"
                                name="codigoForm"
                                value={this.state.codigoForm}
                                onChange={this.handleChange}
                                className="form-control"
                                min="1"
                                max="99"
                                maxLength="3"
                                step="1.0"
                                required/> 
                              </div>
                              </div>
                              
                                
                            </div>

                            <div className="form-group">
                                <label htmlFor="label">Nombre</label>

                                <input 
                                type="text"
                                name="nombreForm"
                                value={this.state.nombreForm}
                                onChange={this.handleChange}
                                className="form-control"
                                required
                                />   
                            </div>

                            <div className="form-group">
                                <label htmlFor="label">Descripción</label>

                                <input 
                                type="text"
                                name="descripcionForm"
                                value={this.state.descripcionForm}
                                onChange={this.handleChange}
                                className="form-control"
                                required
                                />   
                            </div>
                            <div className="form-group">
                            <label>
          Es Para Oferta: <br/>
          <input
            name="para_ofertaForm"
            type="checkbox"
            checked={this.state.para_ofertaForm}
            onChange={this.handleInputChange} />
        </label>
                            </div>
                            <div className="form-group">
                                <label htmlFor="label">Unidad</label>
                                <Dropdown
                                                        value={this.state.UnidadIdForm}
                                                        options={this.props.unidades}
                                                        onChange={(e) => { this.setState({ UnidadIdForm: e.value }) }}
                                                        filter={true} filterPlaceholder="Selecciona una Unidad"
                                                        filterBy="label,value" placeholder="Selecciona una Unidad"
                                                        style={{ width: '100%', heigh: '18px' }}
                                                        required
                                                    />
                                                            </div>

                                    <Button type="submit" label="Guardar" icon="fa fa-fw fa-folder-open" />
                                    <Button type="button"  label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.props.onHide}/>
                                    <Button label="Eliminar" icon="fa fa-remove" onClick={() => this.deleteItem(this.state.IdForm)} />
                            </form>
                        </div>


                    </div>
                </div>
            </div>

                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                     
            </div>
        );
    }

    handleSubmit(event){
        event.preventDefault();
        axios.post("/proyecto/Item/CrearItem",{
            codigo: this.state.codigo,
            item_padre: this.props.itemPadre,
            nombre: this.state.nombre,
            descripcion: this.state.descripcion,
            UnidadId: this.state.UnidadId,
            para_oferta: this.state.para_oferta,
            vigente: 1,
            GrupoId:this.state.GrupoId
        })
        .then((response) => {
            this.props.updateData();
           var r= response.data;
           
            if(r=="Guardado"){
                console.log("entro guardado");
                this.setState({message: 'Guardado Correctamente'},
            this.showSuccess)
            }
            if(r=="Error"){
                console.log("entro error");
                this.setState({message: 'No se pudo Completar Transaccion'},
            this.showWarn)
            }
            if(r=="Movimiento"){
                console.log("entro error movimiento");
                this.setState({message: 'Error el Item Padre Es Item de Movimiento'},
                this.showWarn) 
            }
            if(r=="Existe"){
            
                this.setState({message: 'Error el codigo de Item ya Existe'},
                this.showWarn) 
            }
           
           
                    })
        .catch((error) => {
               this.showWarn();
        });
    }

    showSuccess() {
        this.growl.show({life: 5000,  severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({life: 5000,  severity: 'error', summary: 'Error', detail: this.state.message });
    }
    showSuccessE() {
        this.growl.show({ severity: 'successE', summary: 'Eliminado', detail: 'Eliminado Correctamente' });
    }

    showWarnE() {
        this.growl.show({ severity: 'errorE', summary: 'Error', detail: 'No se Elimino' });
    }
    handleChange(event) {
        this.setState({[event.target.name]: event.target.value});
    }
    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });
    }
    deleteItem(id){
        axios.post("/proyecto/Item/DeleteItem/"+this.state.IdForm,{})
        .then((response) => {
          
            var r= response.data;
           
            if(r=="Eliminado"){
                console.log("entro Eliminado");
              
            }
            if(r=="ErrorEliminado"){
                console.log("Entro Error Eliminado");
                            }
         this.props.updateData();
           
        })
        .catch((error) => {
            console.log(error);
            this.showWarnE();
        });
    }

    getItemData(){
        if(this.props.id>0){
        axios.post("/proyecto/Item/DetailsApi/"+this.props.id,{})
        .then((response) => {
            console.log(response)
            this.setState({
                codigoForm: response.data.apicodigo,
                nombreForm: response.data.nombre,
                descripcionForm: response.data.descripcion,
                UnidadIdForm: response.data.UnidadId,
                para_ofertaForm: response.data.para_oferta,
                vigente: response.data.vigente,
                IdForm: response.data.Id,
                itemPadreForm: response.data.item_padre,
            })
            
        })
        .catch((error) => {
            console.log(error);
            this.showWarn();
        });
    }else{

        
    }
    }

    handleEdit(){
        event.preventDefault();
        axios.post("/proyecto/Item/EditApi",{
            Id: this.state.IdForm,
            codigo: this.state.codigoForm,
            item_padre: this.state.itemPadreForm,
            nombre: this.state.nombreForm,
            descripcion: this.state.descripcionForm,
            UnidadId: this.state.UnidadIdForm,
            para_oferta: this.state.para_ofertaForm,
            vigente: this.state.vigente,
        })
        .then((response) => {
            console.log(response)

            this.setState({
                codigoForm: '',
                nombreForm: '',
                descripcionForm: '',
                UnidadIdForm: 0,
                para_ofertaForm: false,
                vigente: 1,
                IdForm: 0,
            })
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
                   
        })
        .catch((error) => {
            console.log(error);
            this.showWarn();
        });
    }

}
