import React from 'react';
import {Button} from 'primereact/components/button/Button';
import axios from 'axios';

export default class OrdenServicioForm extends React.Component{
    constructor(props){
        super(props);

        this.state ={

            //nombre: '',
            //nombre_nuevo: this.props.NombreNivel

            codigo_orden_servicio:'',
            fecha_orden_servicio:new Date(),
            monto_aprobado_os:0,
            monto_aprobado_suministros:0,
            monto_aprobado_construccion:0,
            monto_aprobado_ingenieria:0,
            version_os:'',


            cabecerapo: null,
            detalleseleccionado: null,
            detallepo: null,
            displayDialog: false,

            proyectoid:0,
            grupoid:1,
            valor:0,

            grupos_item:[
                {label: 'Ingenieria', value: 1},
                {label: 'Contruccion', value: 2},
                {label: 'Suministros', value: 3},
            ]
        }
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showActividadButton = this.showActividadButton.bind(this);
        this.DeleteNivel = this.DeleteNivel.bind(this);
        this.handleSubmitNivel = this.handleSubmitNivel.bind(this)

        this.save = this.save.bind(this);
        this.delete = this.delete.bind(this);
        this.onCarSelect = this.onCarSelect.bind(this);
        this.addNew = this.addNew.bind(this);

    }

    componentWillReceiveProps(prevProps){
        this.setState({nombre_nuevo: prevProps.NombreNivel})
    }



    save() {
        let cars = [...this.state.cars];
        if(this.newCar)
            cars.push(this.state.car);
        else
            cars[this.findSelectedCarIndex()] = this.state.car;

        this.setState({cars:cars, selectedCar:null, car: null, displayDialog:false});
    }

    delete() {
        let index = this.findSelectedCarIndex();
        this.setState({
            cars: this.state.cars.filter((val,i) => i !== index),
            selectedCar: null,
            car: null,
            displayDialog: false});
    }

    findSelectedCarIndex() {
        return this.state.cars.indexOf(this.state.selectedCar);
    }

    updateProperty(property, value) {
        let po = this.state.detallepo;
        po[property] = value;
        this.setState({po: po});
    }

    onCarSelect(e){
        this.newCar = false;
        this.setState({
            displayDialog:true,
            car: Object.assign({}, e.data)
        });
    }

    addNew() {
        this.setState({
            displayDialog: true
        });
    }


    render(){
        let header = <div className="p-clearfix" style={{lineHeight:'1.87em'}}>Montos</div>;

        let footer = <div className="p-clearfix" style={{width:'100%'}}>
            <Button style={{float:'left'}} label="Nuevo" icon="pi pi-plus" onClick={this.addNew}/>
        </div>;

        let dialogFooter = <div className="ui-dialog-buttonpane p-clearfix">
                <Button label="Cancelar" icon="pi pi-times" onClick={this.delete}/>
                <Button label="Guardar" icon="pi pi-check" onClick={this.handleSubmit}/>
            </div>;
        return (

            <div>


                <div className="content-section introduction">
                    <div className="feature-intro">
                    <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                            <label htmlFor="label">Codigó OS</label>
                                            <input
                                            type="text"
                                            name="codigo_orden_servicio"
                                            className="form-control"
                                            onChange={this.handleChange}
                                            value={this.state.codigo_orden_servicio}
                                            />

                                    </div>




                                    <div className="form-group">
                                            <label htmlFor="label">Versión OS</label>
                                            <input
                                            type="text"
                                            name="version_os"
                                            className="form-control"
                                            onChange={this.handleChange}
                                            value={this.state.version_os}
                                            />

                                    </div>

                                </div>
                                    <div className="col">
                                            <div className="form-group">
                                            <label>Fecha Os</label>
                                            <input
                                            type="date"
                                            id="no-filter"
                                            name="fecha_orden_servicio"
                                            className="form-control"
                                            onChange={this.handleChange}
                                            value={this.state.fecha_orden_servicio}
                                    />

                                </div>
                                <div className="col">
                                            <div className="form-group">
                                            <label>Archivo</label>

                                <input type="file" id="docpicker"
                                   accept=".doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document"/>
                                 </div>
                                </div>
                                </div>
                                </div>


                    </div>
                </div>



            </div>
        );
    }

    showActividadButton(){
        if(this.props.codigo_padre != "."){
            return(
                <Button type="button"  label="Actividades" icon="fa fa-fw fa-calendar" onClick={this.props.showActividad}/>
            )
        }
    }

    handleSubmit(event){
       event.preventDefault();
    axios.post("/proyecto/OfertaComercial/CrearOS",{
        OfertaComercialId:this.props.OfertaIdActual,
        codigo_orden_servicio:this.state.codigo_orden_servicio,
        fecha_orden_servicio:this.state.fecha_orden_servicio,
        monto_aprobado_os:this.state.monto_aprobado_os,
        monto_aprobado_suministros:this.state.monto_aprobado_suministros,
        monto_aprobado_construccion:this.state.monto_aprobado_construccion,
        monto_aprobado_ingenieria:this.state.monto_aprobado_ingenieria,
        version_os:this.state.version_os,
        vigente:true,
        ProyectoId:this.state.proyectoid,
        GrupoItemId:this.state.grupoid,
        valor_os:this.state.valor


               })
               .then((response) => {
                   if(response.data ="o"){

                       this.setState({

                        visibleorden:false,
                        codigo_orden_servicio:'',
                        fecha_orden_servicio:null,
                        monto_aprobado_os:0,
                        monto_aprobado_suministros:0,
                        monto_aprobado_construccion:0,
                        monto_aprobado_ingenieria:0,
                        version_os:'',

                    });
                   this.successMessage("OP Creada Correctamente");
                       this.ConsultarOS();
                       this.calcularmontos();
                   } else {
                       this.alertMessage("Ocurrió un Error")
                   }
               })
               .catch((error) => {
                   console.log(error);
                   this.alertMessage("Ocurrió un Error")

               });

        }


    handleSubmitNivel(event){
        event.preventDefault();
        if(this.props.WbsIdSeleccionado == 0){
            this.props.showWarn("No hay nada que editar");
        } else if(this.state.nombre_nuevo === ""){
            this.props.showWarn("Ingresa el nombre del nivel");
        } else {
            axios.post("/proyecto/Wbs/EditarNivel/"+ this.props.WbsIdSeleccionado,{
                nombre: this.state.nombre_nuevo
            })
            .then((response) => {
                if(response.data == "Ok"){
                    this.props.updateData();
                    this.setState({nombre: ''});
                    this.props.onHide();
                    this.props.showSuccess("Nivel actualizado");
                } else {
                    this.props.showWarn("No se pudo editar el nivel");
                }
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Ocurrio un error");
            });
        }
    }

    DeleteNivel(event){
        event.preventDefault();
        axios.post("/proyecto/Wbs/DeleteNivel/"+ this.props.WbsIdSeleccionado,{})
        .then((response) => {
            if(response.data == "Ok"){
                this.props.updateData();
                this.props.showSuccess("Nivel eliminado");
                this.setState({nombre: ''});
                this.props.onHide();
            } else {
                this.props.showWarn("Las actividades tienen items registrados");
            }


        })
        .catch((error) => {
            console.log(error);
            this.props.showWarn("Ocurrio un error");
        });
    }


    handleChange(event) {
        this.setState({[event.target.name]: event.target.value});
    }
}
