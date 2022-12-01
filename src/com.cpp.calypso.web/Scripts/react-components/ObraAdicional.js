import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';

import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';


import TreeWbs from './wbs_components/TreeWbs';
import ItemTable from './adicionales_components/itemsTable';


class ObraAdicional extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            data: [],
            key: 7964,
            blocking: true,
            selectedFile: {parent: {label: ''}, label: ''},
            visible: false,
            table_data: [],
            itemId: 0,
            item_codigo: '',
            WbsOfertaId: 0,
            cantidad: 0,
            precio_unitario: 0.0,
            canSubmit: false,
        }
        this.updateData = this.updateData.bind(this);
        this.onSelectionChange = this.onSelectionChange.bind(this);
        this.onHide = this.onHide.bind(this);
        this.selectItem = this.selectItem.bind(this);
        this.updateItemsData = this.updateItemsData.bind(this);
        this.showDialog = this.showDialog.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
    }

    componentWillMount(){
        this.updateData();
    }

    onSelectionChange(e) {
        if(e.selection.tipo == 'actividad'){
            var ids = e.selection.data.split(",");
            this.setState({ 
                selectedFile: e.selection, 
                WbsOfertaId: ids[2], 
                blocking: true,
                computoId: 0,
                item_codigo: '',
                cantidad: 0,
                precio_unitario: 0.0,
            },
                this.updateItemsData);
        } 
        
    }


    render(){
        let wbs_nombre = this.state.selectedFile.parent.label +  " - " + this.state.selectedFile.label;
        return(

            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <div className="row">
                    <div className="col-sm-6">
                        <TreeWbs onSelectionChange={this.onSelectionChange} data={this.state.data}/>
                    </div>

                    <div className="col-sm-6">
                        <form onSubmit={this.handleSubmit}>
                            <div className="form-group">
                                <label htmlFor="wbs">Wbs</label>
                                <input type="text" value={wbs_nombre} id="wbs" className="form-control" disabled/>
                            </div>

                            <div className="form-group">
                                <label>Item</label>
                                <div className="row">
                                    <div className="col-sm-12 col-md-8">
                                        <input type="text" value={this.state.item_codigo} className="form-control" disabled/>
                                    </div>
                                    <div className="col-sm-12 col-md-4">
                                        <button onClick={this.showDialog}  className="btn btn-outline-primary">Seleccionar</button>
                                    </div>
                                </div>
                            </div>


                            <div className="form-group">
                                <label htmlFor="cantidad">Cantidad</label>
                                <input type="number" step=".01" min="0.0" id="cantidad" value={this.state.cantidad} className="form-control" onChange={this.handleChange} name="cantidad"/>
                            </div>


                            <button type="submit" className="btn btn-outline-primary">Guardar</button>
                        </form>
                        <Dialog header="Items" visible={this.state.visible} width="750px" modal={true} onHide={this.onHide}>
                            <ItemTable data={this.state.table_data} selectItem={this.selectItem} />
                        </Dialog>
                        
                    </div>
                </div>
            </BlockUi>
        );
    }

    handleSubmit(event){
        event.preventDefault();
        axios.post("/proyecto/ObraAdicional/CreateOrUpdateApi",{
            AvanceObraId: document.getElementById('AvanceId').className,
            ItemId: this.state.itemId,
            cantidad: this.state.cantidad,
            precio_unitario: 0,
            total: 0,
            WbsId: this.state.WbsOfertaId,
            costo_total: 0,
            precio_base: 0,
            precio_incrementado: 0,
            precio_ajustado: 0,
            tipo_precio: 0,
            estado: 1,
            vigente: true
        })
        .then((response) => {
            if(response.data === "Ok"){
                this.showSuccess();
                this.setState({
                    blocking: false,
                    selectedFile: {parent: {label: ''}, label: ''},
                    table_data: [],
                    itemId: 0,
                    item_codigo: '',
                    WbsOfertaId: 0,
                    cantidad: 0,
                    
                })
            } else {
                this.showWarn();
            }
            
        })
        .catch((error) => {
            console.log(error);
            this.showWarn();
        });
    }

    
    updateData(){
        
        axios.get("/proyecto/Wbs/ApiWbs/" + document.getElementById('OfertaId').className,{})
        .then((response) => {
            this.setState({data: response.data, blocking: false})
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    updateItemsData(){
        axios.post("/proyecto/ObraAdicional/GetAllItems",{})
        .then((response) => {
            this.setState({table_data: response.data, blocking: false})
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    onHide(event) {
        this.setState({visible: false});
    }

    handleChange(event){
        this.setState({[event.target.name]: event.target.value});
    }

    selectItem(id, codigo){
        this.setState({itemId: id, visible: false, item_codigo: codigo})
    }

    showDialog(e){
        e.preventDefault();
        this.setState({visible: true})
    }

    showSuccess() {
        this.growl.show({  severity: 'success', summary: 'Proceso exitoso!', detail: 'Detalle registrado' });
    }

    showWarn() {
        this.growl.show({  severity: 'error', summary: 'Error', detail: 'No se pudieron guardar los registros' });
    }
}

ReactDOM.render(
    <ObraAdicional />,
    document.getElementById('content-obra-adicional')
  );