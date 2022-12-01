import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';

import {Dialog} from 'primereact/components/dialog/Dialog';
import {Button} from 'primereact/components/button/Button';

import AreaForm from './forms/AreaForm';
import DisciplinaForm from './forms/DisciplinaForm';
import TreeWbs from './wbs_components/TreeWbs';
import ElementoForm from './forms/ElementoForm';
import ActividadForm from './forms/ActividadForm';
import ActividadTable from './wbs_components/ActividadTable';



class CreateWbsContainer extends React.Component{

    constructor(props){
        super(props);
        this.state = { 
            data: [],
            AreaIdSelecionada: 0,
            DisciplinaIdSeleccionada: 0,
            ElementoIdSeleccionado: 0,
            ActividadIdSeleccionada: 0,
            visible: false, 
            visible_disciplina: false,
            visible_elemento: false,
            visible_actividades: false,
            key: 5676,
            key_table: 8965,
            table_data: {},
            areas: [],
            disciplinas: [],
            elementos: [],
            actividades: [],
            blocking: true,
            key_actividad_form: 58954
        };
 
        this.onClick = this.onClick.bind(this);
        this.onHide = this.onHide.bind(this);
        this.updateData = this.updateData.bind(this);
        this.onHideVisibleDisciplina = this.onHideVisibleDisciplina.bind(this);
        this.onSelectionChange = this.onSelectionChange.bind(this);
        this.onHideVisibleElemento = this.onHideVisibleElemento.bind(this);
        this.onHideVisibleActividad = this.onHideVisibleActividad.bind(this);        
        this.updateActividadId = this.updateActividadId.bind(this);
        this.updateTableData = this.updateTableData.bind(this);
        this.getCatalogos = this.getCatalogos.bind(this);
    }

    

    componentWillMount(){
        this.updateData();
        this.getCatalogos();
    }


    onSelectionChange(e) {
        console.log(e.target)
        if(e.selection.tipo == 'area'){
            this.setState({
                visible_disciplina: true, 
                AreaIdSelecionada: parseInt(e.selection.data)})
        } else if(e.selection.tipo == 'disciplina'){
            var ids = e.selection.data.split(",");
            this.setState({
                visible_elemento: true,
                AreaIdSelecionada: ids[0],
                DisciplinaIdSeleccionada: ids[1]
            })
        } else if(e.selection.tipo == 'elemento'){
            var ids = e.selection.data.split(",");
            this.setState({
                visible_actividades: true,
                AreaIdSelecionada: ids[0],
                DisciplinaIdSeleccionada: ids[1],
                ElementoIdSeleccionado: ids[2]
            })
        } else if(e.selection.tipo == 'actividad'){
            var ids = e.selection.data.split(",");
            this.updateActividadId(ids[4]);           
        }
        this.setState({ selectedFile: e.selection });
    }


    render(){

        return(
            <BlockUi tag="div" blocking={this.state.blocking}>
                <div className="row" >
                
                    <div className="col-sm-12">
                        <Button label="Ingresar Área" icon="pi pi-upload" onClick={this.onClick} />
                        <hr/>
                    </div>


                    <div className="col-sm-12 col-md-12" style={{paddingBottom: '1.5em'}}>
                        <TreeWbs key={this.state.key} onSelectionChange={this.onSelectionChange} data={this.state.data}/>
                    </div>
        
                    <div className="col-sm-12 col-md-12">
                    <div className="card">
                            <div className="card-body">
                                <ActividadTable updateActividad={this.updateActividadId} updateData={this.updateData} showForm={this.showForm} key={this.state.key_table} data={this.state.table_data}/>
                            </div>
                    </div>
                    </div>
        
                    <Dialog header="Áreas" visible={this.state.visible} width="350px" modal={true} onHide={this.onHide}>
                        <AreaForm 
                            ListAreas={this.state.areas} 
                            updateData={this.updateData}
                            onHide={this.onHide}
                        />    
                    </Dialog>

                    <Dialog header="Discplinas" visible={this.state.visible_disciplina} width="350px" modal={true} onHide={this.onHideVisibleDisciplina}>
                        <DisciplinaForm 
                            AreaId={this.state.AreaIdSelecionada} 
                            ListDisciplinas={this.state.disciplinas} 
                            updateData={this.updateData}
                            onHide={this.onHideVisibleDisciplina}
                        />    
                    </Dialog>

                    <Dialog header="Elementos" visible={this.state.visible_elemento} width="350px" modal={true} onHide={this.onHideVisibleElemento}>
                        <ElementoForm 
                            AreaId={this.state.AreaIdSelecionada} 
                            DisciplinaId={this.state.DisciplinaIdSeleccionada} 
                            ListElementos={this.state.elementos} 
                            updateData={this.updateData}
                            onHide={this.onHideVisibleElemento}
                        />    
                    </Dialog>

                    <Dialog header="Actividades" visible={this.state.visible_actividades} width="350px" height="300px" modal={true} onHide={this.onHideVisibleActividad}>
                        <ActividadForm 
                            AreaId={this.state.AreaIdSelecionada} 
                            DisciplinaId={this.state.DisciplinaIdSeleccionada} 
                            ListActividades={this.state.actividades} 
                            updateData={this.updateData}
                            ElementoId={this.state.ElementoIdSeleccionado}
                            onHide={this.onHideVisibleActividad}
                            key={this.state.key_actividad_form}
                        />    
                    </Dialog>
                </div>
            </BlockUi>
        );
    }


    onClick(event) {
        event.preventDefault();
        this.setState({visible: true});
    }
    
    onHide(event) {
        this.setState({visible: false});
    }

    onHideVisibleDisciplina(event){
        this.setState({visible_disciplina: false});
    }

    onHideVisibleElemento(event){
        this.setState({visible_elemento: false});
    }

    onHideVisibleActividad(event){
        this.setState({visible_actividades: false});
    }

    updateData(){
        
        axios.get("/proyecto/WbsOferta/ApiWbsOferta/" + document.getElementById('content').className,{})
        .then((response) => {
            this.setState({data: response.data, key: Math.random(), blocking: false})
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    getCatalogos(){
        axios.post("/proyecto/catalogo/GetCatalogo/1",{})
        .then((response) => {
            this.setState({areas: response.data})
        })
        .catch((error) => {
            console.log(error);    
        });
        axios.post("/proyecto/catalogo/GetCatalogo/2",{})
        .then((response) => {
            this.setState({disciplinas: response.data})
        })
        .catch((error) => {
            console.log(error);    
        });
        axios.post("/proyecto/catalogo/GetCatalogo/3",{})
        .then((response) => {
            this.setState({elementos: response.data})
        })
        .catch((error) => {
            console.log(error);    
        });
        axios.post("/proyecto/catalogo/GetCatalogo/4",{})
        .then((response) => {

            var actividades = response.data.map(item => {
                return {label: item.nombre, value: item.Id}
            })

            this.setState({actividades: actividades})
        })
        .catch((error) => {
            console.log(error);    
        });
    }
    
    updateActividadId(id){
        this.setState({
            ActividadIdSeleccionada: id
        },
        this.updateTableData
        )
    }



    updateTableData(){
        
        axios.get("/proyecto/WbsOferta/ApiDetails/"+this.state.ActividadIdSeleccionada,{})
        .then((response) => {            
            this.setState({table_data: response.data, key_actividad_form: Math.random()})
            
        })
        .catch((error) => {
            console.log(error);
            
        });
        
        
    }



}

ReactDOM.render(
    <CreateWbsContainer />,
    document.getElementById('content')
  );

