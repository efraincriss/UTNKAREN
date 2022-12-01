import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';

import {Dialog} from 'primereact/components/dialog/Dialog';
import {Button} from 'primereact/components/button/Button';
import {Growl} from 'primereact/components/growl/Growl';


import TreeWbs from './wbs_components/TreeWbs';

import ActividadForm from './forms/ActividadForm';
import ActividadTable from './wbs_components/ActividadTable';
import NivelForm from './wbs_components/NivelForm';

class WbsContainer extends React.Component{

    constructor(props){
        super(props);
        this.state = { 
            data: [],
            key: 5676,
            key_table: 8965,
            key_actividad_form: 6745,
            key_nivel_form: 85735,
            PadreSeleccionado: '',
            selectedFile: [],
            ActividadIdSeleccionada: 0,
            NivelIdSeleccionado: 0,
            NombreNivel: '',

            message: '',
            visible: false, 
            visible_actividades: false,

            
            table_data: [],
            actividades: [],
            data_item: {},

            blocking: true,
            key_actividad_form: 58954,
            disciplinas: [],
        };

        this.onClick = this.onClick.bind(this);
        this.onHide = this.onHide.bind(this);
        this.updateData = this.updateData.bind(this);
        this.onSelectionChange = this.onSelectionChange.bind(this);
        
        this.onHideVisibleActividad = this.onHideVisibleActividad.bind(this);        
        this.updateActividadId = this.updateActividadId.bind(this);
        this.updateTableData = this.updateTableData.bind(this);
        this.getCatalogos = this.getCatalogos.bind(this);
        this.showActividad = this.showActividad.bind(this);

        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.resetTable = this.resetTable.bind(this);
    }


    componentWillMount(){
        this.updateData();
        this.getCatalogos();
    }


    onSelectionChange(e) {
        var ids = e.selection.data.split(",");
        if(e.selection.tipo == 'padre'){
            this.setState({PadreSeleccionado: ids[0], visible: true, NivelIdSeleccionado: ids[2], NombreNivel: e.selection.label})
        } else{
            this.updateActividadId(ids[2]);
            this.successMessage("Wbs Cargado")
        } 
        this.setState({ selectedFile: e.selection });
    }

    render(){

        return(
            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <div className="row" >
                
                    <div className="col-sm-12">
                        <Button label="Ingresar Nivel" icon="pi pi-upload" onClick={this.onClick} />
                        <hr/>
                    </div>


                    <div className="col-sm-12 col-md-12" style={{paddingBottom: '1.5em'}}>
                        <TreeWbs key={this.state.key} onSelectionChange={this.onSelectionChange} data={this.state.data}/>
                    </div>

                    <div className="col-sm-12 col-md-12">
                        <div className="card">
                                <div className="card-body">
                                    <ActividadTable 
                                    updateActividad={this.updateActividadId} 
                                    updateData={this.updateData} 
                                    showForm={this.showForm} 
                                    key={this.state.key_table} 
                                    data={this.state.table_data}
                                    dataItem={this.state.data_item}
                                    showSuccess={this.successMessage}
                                    showWarn={this.warnMessage}
                                    reset={this.resetTable}
                                    disciplinas={this.state.disciplinas}
                                    />
                                </div>
                        </div>
                    </div>
        
                    
        
                    <Dialog header={this.state.NombreNivel} visible={this.state.visible} width="450px" modal={true} onHide={this.onHide}>
                        <NivelForm
                            key={this.props.key_nivel_form}
                            codigo_padre={this.state.PadreSeleccionado}
                            updateData={this.updateData}
                            showActividad={this.showActividad}
                            showSuccess={this.successMessage}
                            showWarn={this.warnMessage}
                            onHide={this.onHide}
                            WbsIdSeleccionado={this.state.NivelIdSeleccionado}
                            NombreNivel={this.state.NombreNivel}
                            OfertaId={document.getElementById('content').className}
                        />
                    </Dialog>

                    <Dialog header="Actividades" visible={this.state.visible_actividades} width="450px" height="300px" modal={true} onHide={this.onHideVisibleActividad}>
                        <ActividadForm
                            ListActividades={this.state.actividades} 
                            updateData={this.updateData}
                            codigo_padre={this.state.PadreSeleccionado}
                            onHide={this.onHideVisibleActividad}
                            key={this.state.key_actividad_form}
                            showSuccess={this.successMessage}
                            showWarn={this.warnMessage}
                            OfertaId={document.getElementById('content').className}
                        />
                    </Dialog>
                </div>
            </BlockUi>
        );
    }

    onClick(event) {
        event.preventDefault();
        this.setState({visible: true, PadreSeleccionado: '.', NivelIdSeleccionado: 0, NombreNivel: ''});
    }
    
    onHide(event) {
        this.setState({visible: false, NombreNivel: ''});
    }

    showActividad(){
        this.setState({visible_actividades: true})
    }


    onHideVisibleActividad(event){
        this.setState({visible_actividades: false, key_actividad_form: Math.random()});
    }

    updateData(){ 
        axios.get("/proyecto/Wbs/ApiWbs/" + document.getElementById('content').className,{})
        .then((response) => {
            this.setState({data: response.data, key: Math.random(), blocking: false})
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    getCatalogos(){
        
        axios.post("/proyecto/catalogo/GetCatalogo/4",{})
        .then((response) => {

            var actividades = response.data.map(item => {
                
                return {label: item.nombre, dataKey: item.Id, value: item.Id}
            })

            this.setState({actividades: actividades})
        })
        .catch((error) => {
            console.log(error);    
        });

        axios.post("/proyecto/catalogo/GetCatalogo/2",{})
        .then((response) => {

            var disciplinas = response.data.map(item => {
                
                return {label: item.nombre, dataKey: item.Id, value: item.Id}
            })

            this.setState({disciplinas: disciplinas})
        })
        .catch((error) => {
            console.log(error);    
        });

    }

    updateTableData(){
        axios.post("/proyecto/Wbs/DetailsApi/"+this.state.ActividadIdSeleccionada,{})
        .then((response) => { 
            console.log(response.data)
            var data = [];
            data.push(response.data); 
                 
            this.setState({table_data: data, data_item: response.data})
            
        })
        .catch((error) => {
            console.log(error);

        });

    }

    resetTable(){
        this.setState({dataItem: {}, table_data: []})
    }
    
    updateActividadId(id){
        
        this.setState({
            ActividadIdSeleccionada: id
        },
        this.updateTableData
        )
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
    <WbsContainer />,
    document.getElementById('content')
  );

