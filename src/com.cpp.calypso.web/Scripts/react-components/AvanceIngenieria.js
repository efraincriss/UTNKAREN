import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import moment from 'moment';

import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';

import DetalleAvanceTable from './DetalleAvanceIngenieria/DetalleAvanceTable';
import DetalleIngenieriaForm from './DetalleAvanceIngenieria/DetalleIngenieriaForm';
import DetalleItemsTable from './DetalleAvanceIngenieria/DetalleItemsTable';
import ItemIngenieriaForm from './DetalleAvanceIngenieria/ItemIngenieriaForm';
import ListadoDetallesIngenieriaTable from './DetalleAvanceIngenieria/ListadoDetallesIngenieriaTable';

class AvanceIngenieria extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            blocking: true,
            detalles_data: [],
            visible_detalles: false,
            visible_listado: false,
            tipo_registros: [],
            computos_list: [],
            message: "",
            detalleIngenieriaFormKey: 4568,

            DetalleAvanceIngenieriaId: 0,
            items_data: [],
            visible_items: false,
            especialidad_catalogo: [],

            registros: 0, 
            cantidad_horas: 0,
            computo: 0,
            valor_real: 0,
            DetalleIngenieriaId: 0,

            ColaboradorId: 0,
            cantidad_horas_item: 0,
            etapa: 0,
            fecha_registro: '',
            especialidad: 0,
            ItemIngenieriaFormKey: 9857,
            ItemId: 0,
            colaboradores_catalogo: []
        }
        this.GetDetallesData = this.GetDetallesData.bind(this);
        this.onHideDetallesAvanceIngenieria = this.onHideDetallesAvanceIngenieria.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.onHideItemsAvanceIngenieria = this.onHideItemsAvanceIngenieria.bind(this);
        this.GetItemsData = this.GetItemsData.bind(this);
        this.SetDataDetalleIngenieria = this.SetDataDetalleIngenieria.bind(this);
        this.showFormItems = this.showFormItems.bind(this);
        this.SetDataItemIngenieria = this.SetDataItemIngenieria.bind(this);
        this.Delete = this.Delete.bind(this);
        this.onHideDetallesListado = this.onHideDetallesListado.bind(this);
    }

    componentDidMount(){
        this.GetDetallesData();
        this.GetCatalogos();
        this.setState({blocking: false})
    }
    render(){
        return(
            <div>
                <BlockUi tag="div" blocking={this.state.blocking}>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    

                    <div className="row">
                        <div className="col">
                            <button className="btn btn-outline-primary" onClick={() => this.setState({visible_detalles: true})}>Nuevo</button>
                            <button className="btn btn-outline-primary" style={{marginLeft: '0.3em'}} onClick={() => this.setState({visible_listado: true})}>Ver todos</button>
                            <hr/>
                            <DetalleAvanceTable 
                                    data={this.state.detalles_data} 
                                    update_items={this.GetItemsData}
                                    SetDataDetalleIngenieria={this.SetDataDetalleIngenieria}
                                    DeleteItem={this.Delete}
                                />                    
                        </div>
                        <div className="col">
                            <button className="btn btn-outline-primary" onClick={this.showFormItems}>Nuevo</button>
                            <hr/>
                            <DetalleItemsTable 
                            data={this.state.items_data}
                            showSuccess={this.successMessage}
                            showWarn={this.warnMessage}
                            SetDataItem={this.SetDataItemIngenieria}
                            DeleteItem={this.Delete}
                            />
                        
                        </div>
                    </div>
                    

                    

                    


                    <Dialog header="Detalles Avance de Ingeniería" visible={this.state.visible_detalles} width="670px" modal={true} onHide={this.onHideDetallesAvanceIngenieria}>
                        <DetalleIngenieriaForm
                            key={this.state.detalleIngenieriaFormKey}
                            tipo_registros_catalogo={this.state.tipo_registros} 
                            computos_list={this.state.computos_list} 
                            updateData={this.GetDetallesData}
                            showSuccess={this.successMessage}
                            showWarn={this.warnMessage}
                            onHide={this.onHideDetallesAvanceIngenieria}
                            registro={this.state.registro} 
                            cantidad_horas={this.state.cantidad_horas}
                            computo={this.state.computo}
                            valor_real={this.state.valor_real}
                            Id={this.state.DetalleIngenieriaId}
                        />
                    </Dialog>

                    <Dialog header="Avance de ingeniería" visible={this.state.visible_items} width="670px" modal={true} onHide={this.onHideItemsAvanceIngenieria}>
                        <ItemIngenieriaForm
                        key={this.state.ItemIngenieriaFormKey}
                        colaboradores_catalogo={this.state.colaboradores_catalogo}
                        especialidad_catalogo={this.state.especialidad_catalogo}
                        updateData={this.GetItemsData}
                        showSuccess={this.successMessage}
                        showWarn={this.warnMessage}
                        onHide={this.onHideItemsAvanceIngenieria}
                        DetalleAvanceIngenieriaId={this.state.DetalleAvanceIngenieriaId}
                        ColaboradorId={this.state.ColaboradorId}
                        cantidad_horas_item={this.state.cantidad_horas_item}
                        etapa={this.state.etapa}
                        fecha_registro={this.state.fecha_registro}
                        especialidad={this.state.especialidad}
                        ItemId={this.state.ItemId}
                        registros={this.state.registros}
                        tipo_registros_catalogo={this.state.tipo_registros} 
                        />
                    </Dialog>

                    <Dialog header="Detalles Avance de ingeniería" visible={this.state.visible_listado} width="900px" modal={true} onHide={this.onHideDetallesListado}>                        
                        <ListadoDetallesIngenieriaTable data={this.state.detalles_data} />
                    </Dialog>
                </BlockUi>
            </div>
        )
    }
    GetDetallesData(){
        axios.post("/proyecto/DetalleAvanceIngenieria/GetDetallesAvanceIngenieriaApi/" + document.getElementById('AvanceIngenieriaId').className,{})
        .then((response) => {
            console.log(response.data);
            this.setState({detalles_data: response.data})
            
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    GetItemsData(id){
        this.setState({blocking: true})
        axios.post("/proyecto/DetalleItemIngenieria/GetItemsPorDetalleApi/"+id,{})
        .then((response) => {
            console.log(response.data);
            this.setState({items_data: response.data, DetalleAvanceIngenieriaId: id, blocking: false })
            
        })
        .catch((error) => {
            this.setState({blocking: false})
            console.log(error);    
        });
    }

    SetDataDetalleIngenieria(id){
        axios.post("/Proyecto/DetalleAvanceIngenieria/Details/"+id,{})
        .then((response) => {
            
            this.setState({
                registro: response.data.tipo_registro,
                cantidad_horas: response.data.cantidad_horas,
                computo: response.data.ComputoId,
                valor_real: response.data.valor_real,
                DetalleIngenieriaId: response.data.Id,
                visible_detalles: true,
                detalleIngenieriaFormKey: Math.random(),
                
            })
        })
        .catch((error) => {
            console.log(error);
            
        });
    }

    SetDataItemIngenieria(id){
        axios.post("/Proyecto/DetalleItemIngenieria/DetailsApi/"+id,{})
        .then((response) => {
        
            this.setState({
                ColaboradorId: response.data.ColaboradorId,
                cantidad_horas_item: response.data.cantidad_horas,
                etapa: response.data.etapa,
                fecha_registro: moment(response.data.fecha_registro).format('YYYY-MM-DD'),
                especialidad: response.data.especialidad,
                ItemIngenieriaFormKey: Math.random(),
                ItemId: response.data.Id,
                DetalleAvanceIngenieriaId: response.data.DetalleAvanceIngenieriaId,
                visible_items: true,
                precio: response.data.precio_unitario,
                registros: response.data.tipo_registro
            })
        })
        .catch((error) => {
            console.log(error);
            
        });
    }

    Delete(id, type, detalleAvanceId){
        if(type === "DETALLE"){
            axios.post("/Proyecto/DetalleAvanceIngenieria/Delete/"+id,{})
            .then((response) => {
                this.GetDetallesData();
                this.setState({
                    DetalleAvanceIngenieriaId: 0,
                    items_data: [],
                })
            })
            .catch((error) => {
                console.log(error);
            });
        } else {
            axios.post("/Proyecto/DetalleItemIngenieria/Delete/"+id,{})
            .then((response) => {
                this.GetItemsData(detalleAvanceId);
            })
            .catch((error) => {
                console.log(error);
            });
        }
        
    }


    onHideDetallesAvanceIngenieria(){
        this.setState({
            visible_detalles: false, 
            detalleIngenieriaFormKey: Math.random(),
            registro: 0, 
            cantidad_horas: 0,
            computo: 0,
            DetalleIngenieriaId: 0,
        })
    }

    onHideDetallesListado(){
        this.setState({
            visible_listado: false
        })
    }

    onHideItemsAvanceIngenieria(){
        this.setState({
            visible_items: false, 
            ItemIngenieriaFormKey: Math.random(),
            registros: 0,
            ColaboradorId: 0,
            cantidad_horas_item: 0,
            etapa: 0,
            fecha_registro: '',
            especialidad: 0,
            ItemIngenieriaFormKey: Math.random(),
            ItemId: 0,

        
        })
    }

    GetCatalogos(){
        console.log("Catalogos")
        axios.post("/proyecto/Catalogo/GetCatalogo/2006",{})
        .then((response) => {
            this.setState({tipo_registros: response.data})
        })
        .catch((error) => {
            console.log(error);
            
        });

        axios.post("/proyecto/Colaborador/ObtainbyOferta/"+document.getElementById('OfertaId').className,{})
        .then((response) => {
            console.log(response)
            
            var colaboradores = response.data.map(i => {
                return {label: i.numero_identificacion + " - " + i.nombres, value: i.Id}
            })
            this.setState({colaboradores_catalogo: colaboradores})
        })
        .catch((error) => {
            console.log(error);
            
        });

        axios.post("/proyecto/Catalogo/GetCatalogo/3006",{})
        .then((response) => {
            this.setState({especialidad_catalogo: response.data})
        })
        .catch((error) => {
            console.log(error);
            
        });

        axios.post("/proyecto/DetalleAvanceIngenieria/GetComputosIngenieriaApi/"+document.getElementById('OfertaId').className,{})
        .then((response) => {
            var computos = response.data.map(i => {
                return {label: i.Id + " - " + i.item_codigo + " - " + i.item_nombre, value: i.Id }
            })
            this.setState({computos_list: computos})
        })
        .catch((error) => {
            console.log(error);
            
        });
        this.setState({blocking: false})
    }

    showFormItems(){
        if(this.state.DetalleAvanceIngenieriaId == 0){
            this.warnMessage("Selecciona un detalle")
        } else {
            this.setState({visible_items: true})
        }
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
    <AvanceIngenieria />,
    document.getElementById('content')
  );