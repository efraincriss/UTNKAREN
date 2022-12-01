import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import {Dialog} from 'primereact/components/dialog/Dialog';
import {Button} from 'primereact/components/button/Button';

import TreeItem from './item_components/TreeItem';
import ItemForm from './forms/ItemForm';
import ItPadreform from './forms/ItPadreform';

export default class CreateTreeItems extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            data: [],
            ItemPadreSeleccionado: 0,
            key: 8248,
            visible: false,
            visiblepadre: false,
            key_form: 98723,
            id_seleccionado: 0,
            unidades: [],
            label_header: '',
            blocking: true,
            listaitems:[]
        }
        this.updateData = this.updateData.bind(this);
        this.onSelectionChange = this.onSelectionChange.bind(this);
        this.onHide = this.onHide.bind(this);
        this.onHidePadre = this.onHidePadre.bind(this);
        this.activarpadre = this.activarpadre.bind(this);
        this.getunidades=this.getunidades.bind(this);
        this.getpadres=this.getpadres.bind(this);
    }

    componentWillMount(){
        this.updateData();
        this.getunidades();
        this.getpadres();
    }


    onSelectionChange(e) {
        this.setState({
            ItemPadreSeleccionado: e.selection.data, 
            visible: true, key_form: Math.random(), 
            id_seleccionado: e.selection.id,
            label_header: e.selection.labelcompleto+" ",
        })
    }

    render(){
        return(
            <div className="row">
         
                <div className="col-sm-12">
                <Button label="Ingresar Padre" icon="pi pi-upload" onClick={this.activarpadre} />
                <BlockUi tag="div" blocking={this.state.blocking}>
                    <TreeItem key={this.state.key} onSelectionChange={this.onSelectionChange} data={this.state.data} onHide={this.onHide}/>
                    </BlockUi>
                </div>  

            <Dialog header={this.state.label_header} visible={this.state.visible} width="500px" modal={true} onHide={this.onHide}>
                <ItemForm id={this.state.id_seleccionado} 
                unidades={this.state.unidades} 
                key={this.state.key_form} 
                itemPadre={this.state.ItemPadreSeleccionado} 
                updateData={this.updateData} 
                onHide={this.onHide}
                listaitems={this.state.listaitems}
                />    
            </Dialog>
            
            <Dialog header="Información Padre" visible={this.state.visiblepadre} width="350px" modal={true} onHide={this.onHidePadre}>
                        <ItPadreform
                            updateData={this.updateData}
                            onHidePadre={this.onHidePadre}
                        />    
                    </Dialog>

            </div>
            
        )
    }

    activarpadre(event) {
              this.setState({visiblepadre: true});
    }
    
    onHide(event) {
        this.setState({visible: false});
    }
    onHidePadre(event) {
        this.setState({visiblepadre: false});
    }

    updateData(){
        
        axios.get("/Proyecto/Item/Notas",{})
        .then((response) => {
            this.setState({data: response.data,blocking:false, key: Math.random()})
        })
        .catch((error) => {
            console.log(error);
            
        });
    }

    getunidades(){
        axios.post("/Proyecto/Computo/CatalogoUnidades")
        .then((response) => {

            var uns = response.data.map(i => {
                return {label:i.nombre, value: i.Id}
            })
           
            this.setState({unidades: uns})

            
        })
           .catch((error) => {
            console.log(error);    
        });
    }

    getpadres(){
        axios.post("/Proyecto/Item/DetailsGrupos")
        .then((response) => {

            var uns = response.data.map(i => {
                return {label:i.descripcion, value: i.Id}
            })
           
            this.setState({listaitems: uns})

            
        })
           .catch((error) => {
            console.log(error);    
        });
    }
}

ReactDOM.render(
    <CreateTreeItems />,
    document.getElementById('content-item')
  );