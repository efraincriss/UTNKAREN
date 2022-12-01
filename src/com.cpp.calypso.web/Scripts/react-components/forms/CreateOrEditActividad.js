import React from 'react';
import Formsy from 'formsy-react';
import axios from 'axios';
import Moment from 'moment';

import { Dropdown } from 'primereact/components/dropdown/Dropdown'
import {Button} from 'primereact/components/button/Button';
import {Growl} from 'primereact/components/growl/Growl';


export default class CreateOrEditActividad extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            canSubmit: false,
            fecha_inicial: Moment(this.props.fecha_inicial).format("YYYY-MM-DD"),
            fecha_final: Moment(this.props.fecha_final).format("YYYY-MM-DD"),
            observaciones: this.props.observacion,
            disciplinas: this.props.disciplinas,
            referencia:this.props.revision,
            disciplinaid:this.props.disciplinaid,
        }
        this.enableButton = this.enableButton.bind(this);
        this.disableButton = this.disableButton.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.onChangeFechaInicio = this.onChangeFechaInicio.bind(this);
        this.onChangeFechaFin = this.onChangeFechaFin.bind(this);
        this.getDisciplinas=this.getDisciplinas.bind(this);
    }
    componentWillMount(){
        this.getDisciplinas();
    }
    enableButton() {
        this.setState({
          canSubmit: true,
        });
    }

    disableButton() {
        this.setState({
          canSubmit: false
        });
    }

    handleSubmit(){
        event.preventDefault();
        axios.post("/proyecto/Wbs/Edit",{
            OfertaId: this.props.ofertaid,
            Id: this.props.Id,
            estado: this.props.estado,
            observaciones: this.state.observaciones,
            fecha_final: this.state.fecha_final,
            fecha_inicial: this.state.fecha_inicial,
            es_actividad: this.props.dataItem.es_actividad,
            vigente: this.props.dataItem.vigente,
            id_nivel_codigo: this.props.dataItem.id_nivel_codigo,
            id_nivel_padre_codigo: this.props.dataItem.id_nivel_padre_codigo,
            nivel_nombre: this.props.dataItem.nivel_nombre,
            vigente: true,
            revision:this.state.referencia,
            DisciplinaId:this.state.disciplinaid
        })
        .then((response) => {
            if(response.data == "ErrorFechas"){
                this.props.showWarn("Error de fechas")
            } else {
                this.props.updateActividad(this.props.data[0].Id);
                this.props.onHide();
                this.props.showSuccess();
            }
            
        })
        .catch((error) => {           
            this.props.showWarn();
        });
    }

    handleChange(event) {
        event.stopPropagation();
        
        this.setState({[event.target.name]: event.target.value});
    }


    render(){
        return(
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <Formsy 
                onValidSubmit={() => this.handleSubmit()} 
                onValid={() => this.enableButton()} 
                onInvalid={() => this.disableButton()}
                method="post"
                ref="form"
                >       
                       <div className="form-group">
                         <label htmlFor="label">Disciplina:</label>
                                         <Dropdown
                                            value={this.state.disciplinaid}
                                            options={this.props.disciplinas}
                                            onChange={(e) => { this.setState({ disciplinaid: e.value }) }}
                                            filter={true} filterPlaceholder="Selecciona una Disciplina"
                                            filterBy="label,value" placeholder="Selecciona una Disciplina"
                                            style={{ width: '100%', heigh: '18px' }}
                                            required
                                        />
                        </div>
                      <div className="form-group">
                        <label>Revisión</label>
                        <input
                            id="no-filter"                                                                            
                            name="referencia"                            
                            className="form-control"
                            onChange={this.handleChange}
                            validations="isText"
                            value={this.state.referencia}
                        />

                    </div>

                    <div className="form-group">
                        <label>Observación</label>
                        <input
                            id="no-filter"                                                                            
                            name="observaciones"                            
                            className="form-control"
                            onChange={this.handleChange}
                            validations="isText"
                            value={this.props.observacion}
                        />

                    </div>

                    <div className="form-group">
                        <label>Fecha Inicio</label>
                        <input
                            type="date"
                            id="no-filter"                                                                            
                            name="fecha_inicial"                        
                            className="form-control"
                            onChange={this.handleChange}
                            value={this.props.fecha_inicio}
                            required
                        />

                    </div>

                    <div className="form-group">
                        <label>Fecha Fin</label>
                        <input
                            type="date"
                            id="no-filter"                                                                            
                            name="fecha_final"
                            className="form-control"
                            onChange={this.handleChange}
                            value={this.props.fecha_final}
                            required
                        />


                    </div>



                    <button type="submit" className="btn btn-primary" >Guardar</button>
                    <button style={{marginLeft: '0.4em'}} type="button" className="btn btn-primary" onClick={this.props.onHide}>Cancelar</button>
                </Formsy>
            </div>
        )
    }
    getDisciplinas(){
        
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

    onChangeFechaInicio(fecha_inicio){
        this.setState({ fecha_inicio })
    }
    onChangeFechaFin(fecha_inicio){
        this.setState({ fecha_inicio })
    }
}