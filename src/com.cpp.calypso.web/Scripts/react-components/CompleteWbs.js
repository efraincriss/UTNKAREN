import React, { Component } from 'react';
import ReactDOM from 'react-dom';

import BlockUi from 'react-block-ui';

import {Dialog} from 'primereact-v2/dialog';
import {Button} from 'primereact-v2/button';
import {Growl} from 'primereact-v2/growl';

class CompleteWbs extends Component {

    constructor(props) {
        super(props)
        
        this.state = {
            blocking: true,
            data: [],
            actividades: [],
            msg: '',
        }

        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.updateData = this.updateData.bind(this);
        this.getCatalogos = this.getCatalogos.bind(this);
    }

    componentWillMount(){
        this.updateData();
        this.getCatalogos();
    }

    render() {
        return (
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
                        />
                    </Dialog>
                </div>
            </BlockUi>
        )
    }


    


    updateData(){ 
        axios.get("/proyecto/Wbs/ApiWbs/" + document.getElementById('content').className,{})
        .then((response) => {
            this.setState({data: response.data, blocking: false})
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
    <CompleteWbs />,
    document.getElementById('content')
  );
