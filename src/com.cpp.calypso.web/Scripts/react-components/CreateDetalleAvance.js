import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';

import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';

import TreeWbs from './wbs_components/TreeWbs';
import ComputosTable from './detalles_avance_components/ComputosTable';


class CreateDetalleAvance extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            data: [],
            key: 7964,
            blocking: true,
            selectedFile: {nombres: ''},
            visible: false,
            table_data: [],
            computoId: 0,
            item_codigo: '',
            WbsOfertaId: 0,
            cantidad_diaria: 0,
            cantidad_acumulada_anterior: 0,
            cantidad_acumulada_actual: 0,
            precio_unitario: 0.0,
            canSubmit: false,
        }
        this.updateData = this.updateData.bind(this);
        this.onSelectionChange = this.onSelectionChange.bind(this);
        this.onHide = this.onHide.bind(this);
        this.selectComputo = this.selectComputo.bind(this);
        this.updateComputosData = this.updateComputosData.bind(this);
        this.showDialog = this.showDialog.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.showWarnDuplicado = this.showWarnDuplicado.bind(this);
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
                cantidad_diaria: 0,
                cantidad_acumulada_anterior: 0,
                cantidad_acumulada_actual: 0,
                precio_unitario: 0.0,
                cantidad_eac: 0,
            },
                this.updateComputosData);
        } 
        
    }

    render(){
        return(

            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <div className="row">
                    <div className="col-sm-6">
                        <TreeWbs key={this.state.key} onSelectionChange={this.onSelectionChange} data={this.state.data}/>
                    </div>

                    <div className="col-sm-6">
                        <form onSubmit={this.handleSubmit}>
                            <div className="form-group">
                                <label htmlFor="wbs">Wbs</label>
                                <input type="text" value={this.state.selectedFile.label} id="wbs" className="form-control" disabled/>
                            </div>

                            <div className="form-group">
                                <label>Computo</label>
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
                                <label htmlFor="cantidad_acumulada_actual">Cantidad Acumulada</label>
                                <input type="number" step=".01" id="cantidad_acumulada_actual" value={this.state.cantidad_acumulada_actual} min="0.0" className="form-control" onChange={this.handleChange} name="cantidad_acumulada_actual"/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="cantidad_eac">Cantidad EAC</label>
                                <input type="number" step=".01" id="cantidad_eac" value={this.state.cantidad_eac} min="0.0" className="form-control" onChange={this.handleChange} name="cantidad_eac"/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="cantidad_acumulada_anterior">Cantidad Acumulada Anterior</label>
                                <input type="text" disabled id="cantidad_acumulada_anterior" className="form-control" value={this.state.cantidad_acumulada_anterior}/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="cantidad_diaria">Cantidad Diaria</label>
                                <input type="text" disabled id="cantidad_diaria" className="form-control" value={(this.state.cantidad_acumulada_actual - this.state.cantidad_acumulada_anterior)}/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="PU">Precio Unitario</label>
                                <input type="text" disabled id="PU" className="form-control" value={this.state.precio_unitario}/>
                            </div>

                            <div className="form-group">
                                <label htmlFor="Total">Total</label>
                                <input type="text" disabled id="total" className="form-control" value={(this.state.precio_unitario * ((this.state.cantidad_acumulada_actual - this.state.cantidad_acumulada_anterior)))}/>
                            </div>



                            <button type="submit" className="btn btn-outline-primary">Guardar</button>
                        </form>
                        <Dialog header="Computos" visible={this.state.visible} width="750px" modal={true} onHide={this.onHide}>
                            <ComputosTable data={this.state.table_data} selectComputo={this.selectComputo}/>
                        </Dialog>
                        
                    </div>
                </div>
            </BlockUi>
        );
    }

    handleSubmit(event){
        event.preventDefault();
        axios.post("/proyecto/DetalleAvanceObra/CreateDetalleAvance",{
            AvanceObraId: document.getElementById('AvanceId').className,
            ComputoId: this.state.computoId,
            cantidad_diaria: this.state.cantidad_acumulada_actual - this.state.cantidad_acumulada_anterior,
            cantidad_acumulada_anterior: this.state.cantidad_acumulada_anterior,
            cantidad_acumulada: this.state.cantidad_acumulada_actual,
            precio_unitario: this.state.precio_unitario,
            total: (this.state.precio_unitario * ((this.state.cantidad_acumulada_actual - this.state.cantidad_acumulada_anterior))),
            vigente: true,
            cantidad_eac: this.state.cantidad_eac
        })
        .then((response) => {
            if(response.data === "OK"){
                this.showSuccess();
                this.setState({
                    blocking: false,
                    selectedFile: {label: ''},
                    table_data: [],
                    computoId: 0,
                    item_codigo: '',
                    WbsOfertaId: 0,
                    cantidad_diaria: 0,
                    cantidad_acumulada_anterior: 0,
                    cantidad_acumulada_actual: 0,
                    precio_unitario: 0.0,
                    canSubmit: false,
                })
            } else if(response.data === "ErrorDuplicate"){
                this.showWarnDuplicado();
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

    updateComputosData(){
        axios.post("/proyecto/DetalleAvanceObra/GetComputos/" + this.state.WbsOfertaId,{
            fecha_presentacion: document.getElementById('fecha_presentacion').className
        })
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

    selectComputo(id, codigo, precio_unitario, cantidad_acumulada_anterior, cantidad_eac){
        this.setState({computoId: id, visible: false, item_codigo: codigo, precio_unitario: precio_unitario, cantidad_acumulada_anterior: cantidad_acumulada_anterior, cantidad_eac: cantidad_eac})
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

    showWarnDuplicado() {
        this.growl.show({  severity: 'error', summary: 'Error', detail: 'El registro ya existe' });
    }

}

ReactDOM.render(
    <CreateDetalleAvance />,
    document.getElementById('content')
  );