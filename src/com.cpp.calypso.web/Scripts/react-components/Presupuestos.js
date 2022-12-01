import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';

class Presupuestos extends React.Component {
    constructor(props){
        super(props);
        this.state = {
            blocking: true,
            data: [],
            Proyectos: [],
            SR: [],
            visible: false,

            // Inputs del Formulario
            ProyectoId: 0,
            fecha_registro: Date.now(),
            RequerimientoId: 0,
            ClaseId: 0,
            descripcion: '',
            version: 'A',
            codigo: 'Por Generar',
            alcance: '',

        }
        this.successMessage = this.successMessage.bind(this)
        this.warnMessage = this.warnMessage.bind(this);
        this.alertMessage = this.alertMessage.bind(this);
        this.GenerarBotones = this.GenerarBotones.bind(this)
        this.Redireccionar = this.Redireccionar.bind(this)
        this.OcultarFormulario = this.OcultarFormulario.bind(this)
        this.EnviarFormulario = this.EnviarFormulario.bind(this)
        this.handleChangeProyecto = this.handleChangeProyecto.bind(this)
        this.handleChangeSR = this.handleChangeSR.bind(this)
        this.handleChange = this.handleChange.bind(this);
    }

    componentDidMount (){
        this.ConsultarProyectos();
        this.ConsultarPresupuestos();
    }
    

    render(){
        return(
            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <div className="row" style={{marginBottom: '1em'}}>
                        <div className="col" align="right">
                            <button className="btn btn-outline-indigo" onClick={() => this.setState({visible: true})}>Nuevo Presupuesto</button>
                        </div>
                    </div>
                    <BootstrapTable data={ this.state.data } hover={true} pagination={true}>
                        <TableHeaderColumn
                        isKey
                        dataField='contrato_descripcion' 
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal' }}
                        filter={ { type: 'TextFilter', delay: 500 } }
                        dataSort={true}
                        >Contrato</TableHeaderColumn>

                        <TableHeaderColumn 
                        dataField='proyecto_codigo' 
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal' }} 
                        filter={ { type: 'TextFilter', delay: 500 } } 
                        dataSort={true}>Proyecto</TableHeaderColumn>

                        <TableHeaderColumn 
                        dataField='codigo_requerimiento' 
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal' }} 
                        filter={ { type: 'TextFilter', delay: 500 } } 
                        dataSort={true}>SR</TableHeaderColumn>

                        <TableHeaderColumn 
                        dataField='codigo' 
                        tdStyle={{ whiteSpace: 'normal' }} 
                        thStyle={{ whiteSpace: 'normal' }} 
                        filter={ { type: 'TextFilter', delay: 500 } } 
                        dataSort={true}>Presupuesto</TableHeaderColumn>

                        <TableHeaderColumn 
                        dataField='Operaciones'
                        width={'10%'}
                        dataFormat={this.GenerarBotones.bind(this)}></TableHeaderColumn>
                    </BootstrapTable>

                <Dialog header="Generación de Presupuesto" visible={this.state.visible} width="800px" modal={true} onHide={this.OcultarFormulario}>
                    <form onSubmit={this.EnviarFormulario}>
                        <div className="row">

                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label">Proyecto</label>
                                    <select onChange={this.handleChangeProyecto} className="form-control">
                                        <option value="0">-- Selecciona un proyecto --</option>
                                        {this.ProyectosSelect(this.state.Proyectos)}
                                    </select>
                                    
                                </div>
                            </div>

                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label">SR(Service Request)</label>
                                    <select onChange={this.handleChangeSR} className="form-control">
                                        <option value="0">-- Selecciona un SR --</option>
                                        {this.SRSelect(this.state.SR)}
                                    </select>
                                    
                                </div>
                            </div>
                        </div>


                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label>Fecha Registro</label>
                                    <input
                                        type="date"
                                        id="no-filter"
                                        name="fecha_registro"
                                        className="form-control"
                                        onChange={this.handleChange}
                                        value={this.state.fecha_registro}
                                    />

                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label">Clase</label>
                                    <select onChange={this.handleChange} className="form-control" name="ClaseId">
                                        <option value="0">-- Selecciona una Clase --</option>
                                        <option value="1">Budgetario</option>
                                        <option value="2">Clase 1</option>
                                        <option value="3">Clase 2</option>
                                        <option value="4">Clase 3</option>
                                        <option value="5">Clase 4</option>
                                        <option value="6">Clase 5</option>
                                    </select>
                                    
                                </div>
                            </div>
                        </div>
                                

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label>Descripción</label>
                                    <input
                                        type="text"
                                        name="descripcion"
                                        className="form-control"
                                        onChange={this.handleChange}
                                        value={this.state.descripcion}
                                    />
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label>Alcance</label>
                                    <input
                                        type="text"
                                        name="alcance"
                                        className="form-control"
                                        onChange={this.handleChange}
                                        value={this.state.alcance}
                                    />
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col">
                                <p><b>Estado Aprobación: </b>PENDIENTE DE APROBACIÓN</p>
                            </div>
                            <div className="col">
                                <p><b>Estado Emisión: </b>EN PREPARACIÓN</p>
                            </div>
                        </div>

                        <button type="submit"  className="btn btn-outline-primary" icon="fa fa-fw fa-folder-open" style={{marginRight: '0.3em'}}>Guardar</button>
                        <button type="button"  className="btn btn-outline-primary" icon="fa fa-fw fa-ban" onClick={this.OcultarFormulario}>Cancelar</button>
                    </form>
                </Dialog>
            </BlockUi>
        );
    }

    EnviarFormulario(event){
        event.preventDefault();
        if(this.state.ProyectoId === 0){
            this.alertMessage("Selecciona un Proyecto")
        } else if(this.state.RequerimientoId === 0){
            this.alertMessage("Selecciona un Requerimiento")
        } else {
            axios.post("/proyecto/Oferta/CreatePresupuesto",{
                ProyectoId: this.state.ProyectoId,
                fecha_registro: this.state.fecha_registro,
                RequerimientoId: this.state.RequerimientoId,
                ClaseId: this.state.ClaseId,
                descripcion: this.state.descripcion,
                version: this.state.version,
                codigo: this.state.codigo,
                alcance: this.state.alcance,
            })
            .then((response) => {
                if(response.data === "Error"){
                    this.alertMessage("Validación de Datos")
                } else {
                    this.Redireccionar(response.data);
                }
            })
            .catch((error) => {
                console.log(error);
                this.warnMessage("Ocurrió un Error")
            });
        }
    }

    
    
    

    handleChangeProyecto(event) {
        this.setState(
            { ProyectoId: event.target.value, RequerimientoId: 0, SR: [], blocking: true },
            this.ConsultarSR
        );
    }

    handleChangeSR(event) {
        this.setState({ RequerimientoId: event.target.value });
    }

    handleChange(event){
        this.setState({[event.target.name]: event.target.value});
    }

    ConsultarProyectos(){
        axios.post("/Proyecto/Proyecto/RecuperarProyectos/", {})
            .then((response) => {
                this.setState({ Proyectos: response.data})
                console.log(response.data);
            })
            .catch((error) => {
                this.setState({blocking: false })
                this.alertMessage("Ocurrió un error al consultar los proyectos")
                console.log(error);
            });
    }

    ConsultarSR(){
        axios.post("/Proyecto/Presupuestos/GetRequerimientosProyectoApi/" + this.state.ProyectoId, {})
        .then((response) => {
            this.setState({ SR: response.data, blocking: false })
        })
        .catch((error) => {
            this.setState({blocking: false })
            this.alertMessage("Ocurrió un error al consultar las SR")
            console.log(error);
        });
    }

    GenerarBotones(cell, row){
        return(
            <div>
                <button onClick={() => this.Redireccionar(row.Id)} className="btn btn-sm btn-outline-indigo">Ver</button>
            </div>
        )
    }

    ProyectosSelect(list) {
        return (

            list.map((item) => {
                return (
                    <option key={item.Id} value={item.Id}>{item.codigo}</option>
                )
            })

        );
    }

    SRSelect(list) {
        return (

            list.map((item) => {
                return (
                    <option key={item.Id} value={item.Id}>{item.codigo}</option>
                )
            })

        );
    }

    Redireccionar(id){
        window.location.href = "/Proyecto/Oferta/Detalle/" + id;
    }



    OcultarFormulario(){
        this.setState({visible: false})
    }

    showSuccess() {
        this.growl.show({life: 5000,  severity: 'success', summary: 'Correcto', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({life: 5000,  severity: 'error', summary: 'Error', detail: this.state.message });
    }

    showAlert() {
        this.growl.show({severity: 'warn', summary: 'Alerta', detail: this.state.message});
    }

    successMessage(msg){
        this.setState({message: msg}, this.showSuccess)
    }

    warnMessage(msg){
        this.setState({message: msg}, this.showWarn)
    }

    alertMessage(msg){
        this.setState({message: msg}, this.showAlert)
    }
}
ReactDOM.render(
    <Presupuestos />,
    document.getElementById('content')
  );