import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import moment from 'moment';
import { Dropdown } from 'primereact/components/dropdown/Dropdown'
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';
import ListaRequerimientos from './Presupuestos/ListaRequerimientos';
import DetalleRequerimiento from './Presupuestos/DetalleRequerimiento';

class Requerimientos extends React.Component {
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
            fecha_registro: moment(new Date()).format("YYYY-MM-DD"),
            RequerimientoId: 0,
            ClaseId: 0,
            descripcion: '',
            version: 'A',
            codigo: 'Por Generar',
            alcance: '',
            origenes:[],
            origen:0,
            descuento:0.0,
            justificacion_descuento:'',

            // Inputs de Navegacion
            MostrarLista: true,
            SrSeleccionada: {},
            presupuestos: [],

            // Saber si se genera el primer presupuesto,
            // o es una nueva version
            nueva_version: false,


            //filtro
            contratos:[],
            ContratoId:0,

        }

        this.handleChange = this.handleChange.bind(this);
        this.OcultarFormulario= this.OcultarFormulario.bind(this);
        this.MostrarFormulario = this.MostrarFormulario.bind(this);
        this.alertMessage = this.alertMessage.bind(this);
        this.infoMessage = this.infoMessage.bind(this);
        this.EnviarFormulario = this.EnviarFormulario.bind(this);

        this.EscogerRender = this.EscogerRender.bind(this);
        this.Loading = this.Loading.bind(this);
        this.StopLoading = this.StopLoading.bind(this);
        this.CambiarRender = this.CambiarRender.bind(this);
        this.SeleccionarSr = this.SeleccionarSr.bind(this);
        this.ConsultarPresupuestos = this.ConsultarPresupuestos.bind(this);
        this.MostrarComponenteDetalleRequerimiento = this.MostrarComponenteDetalleRequerimiento.bind(this);
        this.NuevaVersion = this.NuevaVersion.bind(this);
        this.getContratos=this.getContratos.bind(this);
        
        
    }

    componentDidMount (){
        this.ConsultarSR();
        this.ConsultarOrigen();
        this.getContratos();
        if(document.getElementById('content').className > 0){
            this.MostrarComponenteDetalleRequerimiento(document.getElementById('content').className);
        }
    }

    render(){
        return(
            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
               
              
                {this.EscogerRender()}
                

                <Dialog header="Generación de Presupuesto" visible={this.state.visible} width="800px" modal={true} onHide={this.OcultarFormulario}>
                    <form onSubmit={this.EnviarFormulario}>
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
                                                <label htmlFor="label">Origen</label>
                                                <Dropdown
                                            value={this.state.origen}
                                            options={this.state.origenes}
                                            onChange={(e) => { this.setState({ origen: e.value })}}
                                            filter={true} filterPlaceholder="Seleccione un origen"
                                            filterBy="label,value" placeholder="Seleccione un origen"
                                            style={{ width: '100%', heigh: '18px' }}
                                            
                                             />
                                                
                                     </div>
                             </div>
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
                                            <label htmlFor="label">Valor Descuento</label>
                                            <input
                                            type="number"
                                            name="descuento"
                                            className="form-control"
                                            onChange={this.handleChange}
                                            min="0" value="0" step="any"
                                            value={this.state.descuento}
                                    />
                                         </div>
                                </div>
                            <div className="col">
                                <div className="form-group">
                                    <label>Justificacion Descuento</label>
                                    <input
                                        type="text"
                                        name="justificacion_descuento"
                                        className="form-control"
                                        onChange={this.handleChange}
                                        value={this.state.justificacion_descuento}
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
        this.setState({blocking: true})
        if(this.state.nueva_version){
            if(this.state.origen>0){

            
            axios.post("/proyecto/OfertaPresupuesto/NuevaVersion",{
                fecha_registro: this.state.fecha_registro,
                RequerimientoId: this.state.SrSeleccionada.Id,
                ProyectoId: this.state.SrSeleccionada.Proyecto.Id,
                Clase: this.state.ClaseId,
                descripcion: this.state.descripcion,
                version: this.state.version,
                codigo: this.state.codigo,
                alcance: this.state.alcance,
                origen:this.state.origen,
                descuento:this.state.descuento,
                justificacion_descuento:this.state.justificacion_descuento
            })
            .then((response) => {
                if(response.data === "NO_EXISTE_RDO_DEFINITIVO"){
                    this.alertMessage("No existe un RDO definitivo")
                    this.setState({blocking: false})
                } else {
                    this.infoMessage("Versión creada");
                    this.ConsultarPresupuestos();
                    this.ConsultarSR();
                    this.setState({visible: false})
                }
            })
            .catch((error) => {
                console.log(error);
                this.alertMessage("Ocurrió un Error")
                this.setState({blocking: false})
            });
        }else{
            this.alertMessage("Escoja un Origen de Datos")   
            this.setState({blocking: false})
        }
        } else {
            if(this.state.origen>0){
            axios.post("/proyecto/OfertaPresupuesto/CreatePresupuesto",{
                fecha_registro: this.state.fecha_registro,
                RequerimientoId: this.state.SrSeleccionada.Id,
                Clase: this.state.ClaseId,
                descripcion: this.state.descripcion,
                version: this.state.version,
                codigo: this.state.codigo,
                alcance: this.state.alcance,
                origen:this.state.origen, 
                descuento:this.state.descuento,
                justificacion_descuento:this.state.justificacion_descuento
            })
            .then((response) => {
                if(response.data === "Error"){
                    this.alertMessage("Validación de Datos")
                    this.setState({blocking: false})
                } else {
                    this.infoMessage("Presupuesto Creado");
                    this.ConsultarPresupuestos();
                    this.ConsultarSR();
                    this.setState({visible: false})
                }
            })
            .catch((error) => {
                console.log(error);
                this.alertMessage("Ocurrió un Error")
                this.setState({blocking: false})
            });

        }else{
            this.alertMessage("Escoja un Origen de Datos")   
            this.setState({blocking: false})
        }
        }
        
    }

    EscogerRender(){
        if(this.state.MostrarLista){
            return(<div>
                <div className="row">
                <div className="col">
                                  <div className="form-group">
                                                 <label htmlFor="label">Contrato</label>
                                                 <Dropdown
                                             value={this.state.ContratoId}
                                             options={this.state.contratos}
                                             onChange={(e) => { this.ConsultarSRC( e.value)  }}
                                             filter={true} filterPlaceholder="Selecciona un Contrato"
                                             filterBy="label,value" placeholder="Selecciona una Contrato"
                                             style={{ width: '100%', heigh: '18px' }}
                                           
                                              />
                                                 
                                     </div>
                    </div>
                    <div className="col"></div>
                 </div>
                <ListaRequerimientos
                data={this.state.SR}
                Loading={this.Loading}
                StopLoading={this.StopLoading}
                SeleccionarSr={this.SeleccionarSr}
                />
                </div>
            )
        } else {
            return(
                <DetalleRequerimiento
                Regresar={this.CambiarRender}
                sr={this.state.SrSeleccionada}
                presupuestos={this.state.presupuestos}
                NuevaVersion={this.NuevaVersion}
                MostrarFormulario={this.MostrarFormulario}
                />
            )
        }
    }

    ConsultarSR(){
        axios.post("/Proyecto/Requerimiento/Listar", {})
        .then((response) => {
            this.setState({ SR: response.data, blocking: false })
         
        })
        .catch((error) => {
            this.setState({blocking: false })
            this.alertMessage("Ocurrió un error al consultar las SR")
            console.log(error);
        });
    }
    ConsultarSRC(id){
        console.log(id);
        this.setState({ ContratoId: id, blocking: true })
        axios.post("/Proyecto/Requerimiento/ListarporContrato/", {
            Id:id
        })
        .then((response) => {
            this.setState({ SR: response.data, blocking: false })
         
        })
        .catch((error) => {
            this.setState({blocking: false })
            this.ConsultarSR();
            console.log(error);
        });
    }
    ConsultarOrigen(){
            axios.post("/Proyecto/Catalogo/GetByCodeApi/?code=origen_presupuesto",{})
        .then((response) => {
            var items = response.data.result.map(item => {          
                return {label: item.nombre, dataKey: item.Id, value: item.Id}
            })
            this.setState({origenes:items, blocking: false })
       

        })
        .catch((error) => {
            this.setState({blocking: false })
            this.alertMessage("Ocurrió un error al consultar los origenes")
            console.log(error);
        });
    }

    ConsultarPresupuestos(){
        axios.post("/Proyecto/OfertaPresupuesto/ListarPorRequerimiento/"+ this.state.SrSeleccionada.Id, {})
        .then((response) => {
            this.setState({presupuestos: response.data, blocking: false})
        })
        .catch((error) => {
            this.setState({blocking: false })
            this.alertMessage("Ocurrió un error al consultar los Presupuestos")
            console.log(error);
        });
    }

    NuevaVersion(){
        this.setState({
            fecha_registro:moment(new Date()).format("YYYY-MM-DD"),
            ClaseId: 0,
            descripcion: '',
            version: 'Indefinida',
            codigo: 'Por Generar',
            alcance: '',
            nueva_version: true,
        }, this.MostrarFormulario)
    }

    SeleccionarSr(Sr){
        this.setState({blocking: true})
        axios.post("/Proyecto/OfertaPresupuesto/ListarPorRequerimiento/"+ Sr.Id, {})
        .then((response) => {
            this.setState({presupuestos: response.data,
                         SrSeleccionada: Sr, 
                         alcance:Sr.alcance?Sr.alcance:'',
                         blocking: false}, this.CambiarRender)
        })
        .catch((error) => {
            this.setState({blocking: false })
            this.alertMessage("Ocurrió un error al consultar las SR")
            console.log(error);
        });
        
    }
    getContratos() {
        axios.post("/Proyecto/Contrato/GetContratosApi", {})
            .then((response) => {
                console.log(response.data);
                var items = response.data.map(item => {          
                    return {label: item.Codigo +' ' + item.descripcion, dataKey: item.Id, value: item.Id}
                })
                this.setState({contratos: items})

            })
            .catch((error) => {
                console.log(error);
            });
    }
    CambiarRender(){
        this.setState({MostrarLista: !this.state.MostrarLista})
    }

    // Cuando se regresar de los detalles de Presupuesto
    // se debe cargar el componente de DetallesRequerimiento
    // en lugar del componente ListaRequerimientos
    MostrarComponenteDetalleRequerimiento(RequerimientoId){
        axios.post("/Proyecto/Requerimiento/DetailsApi/"+ RequerimientoId, {})
        .then((response) => {
            this.SeleccionarSr(response.data);
        })
        .catch((error) => {
            this.setState({blocking: false })
            this.alertMessage("Ocurrió un error al consultar la SR")
            console.log(error);
        });
    }

    handleChange(event){
        this.setState({[event.target.name]: event.target.value});
    }

    Redireccionar(id){
        window.location.href = "/Proyecto/Oferta/Detalle/" + id;
    }

    Loading(){
        this.setState({blocking: true})
    }

    StopLoading(){
        this.setState({blocking: false})
    }

    OcultarFormulario(){
        this.setState({visible: false})
    }

    MostrarFormulario(){
        this.setState({visible: true})
    }

    showInfo() {
        this.growl.show({severity: 'info', summary: 'Información', detail: this.state.message});
    }

    infoMessage(msg){
        this.setState({message: msg}, this.showInfo)
    }

    showAlert() {
        this.growl.show({severity: 'warn', summary: 'Alerta', detail: this.state.message});
    }

    alertMessage(msg){
        this.setState({message: msg}, this.showAlert)
    }
}

ReactDOM.render(
    <Requerimientos />,
    document.getElementById('content')
  );