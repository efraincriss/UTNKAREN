import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import moment from 'moment';
import BlockUi from 'react-block-ui';
import Formsy from 'formsy-react';
import { Dialog } from 'primereact/components/dialog/Dialog';
import { Growl } from 'primereact/components/growl/Growl';
import { Dropdown } from 'primereact/components/dropdown/Dropdown'
import { Button } from 'primereact/components/button/Button';
import ArbolWbs from '../WbsPresupuesto/ArbolWbs';
import NivelForm from '../wbs_components/NivelForm';
import ActividadForm from '../forms/ActividadForm';

export default class WbsRdo extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            blocking: true,
            visible: false,
            visible_actividades: false,
            PadreSeleccionado: '.',
            NivelIdSeleccionado: 0,
            NombreNivel: '',
            data: [],
            key: 46978,
            disciplinas: [],
            actividades: [],
            ActividadIdSeleccionada: 0,
            visible_reordenar: false,
            llave: 5,
            expandir: true,

            //Parte Derecha
            codigonivel: "",
            disciplinaid: 0,
            nombredisciplina: "",
            observacion: "",
            revision: "",
            fechainicio: null,
            fechafin: null,
            visible_actividades_form: false,
            actividadseleccionada: 0,
            visibleRight: false,
            ofertaid: 0,
            es_actividad: 0,
            estado: 0,
            vigente: 1,
            expandedKeys: {},
        }
        this.onClick = this.onClick.bind(this);
        this.onHide = this.onHide.bind(this);
        this.updateData = this.updateData.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.onDelete = this.onDelete.bind(this);
        this.showActividad = this.showActividad.bind(this);
        this.onHideVisibleActividad = this.onHideVisibleActividad.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.obtenerKeys = this.obtenerKeys.bind(this);
        this.onDragDrop = this.onDragDrop.bind(this);
        this.onSelect = this.onSelect.bind(this);
        this.showForm = this.showForm.bind(this);
        this.onHideVisibleActividad = this.onHideVisibleActividad.bind(this);
        this.getCatalogos = this.getCatalogos.bind(this);
        this.successMessage = this.successMessage.bind(this)
        this.warnMessage = this.warnMessage.bind(this)
        this.Refrescar = this.Refrescar.bind(this)

        this.GuardarDrag = this.GuardarDrag.bind(this)
        this.Expander = this.Expander.bind(this)
        this.confirmacionGuardar = this.confirmacionGuardar.bind(this);
        this.onHideConfirmacionDrag = this.onHideConfirmacionDrag.bind(this);
        this.DragAndDrop = this.DragAndDrop.bind(this)
    }

    componentWillMount() {
        this.updateData();
        this.getCatalogos();
    }

    render() {
        const footer = (
            <div>
                <Button label="Si" icon="pi pi-check" onClick={this.GuardarDrag} />
                <Button label="No" icon="pi pi-times" onClick={this.onHideConfirmacionDrag} className="p-button-secondary" />
            </div>
        );
        return (
            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>

                <div className="row">
                    <div className="col-sm-12">
                        <h3 className="text-blue">WBS RDO</h3>
                        <div className="card" style={{ height: '150px', maxHeight: '150px' }}>
                            <div className="card-body">
                                <span><b>Actividad: </b> {this.state.nombredisciplina}</span>
                                <br/>
                                <span><b>Periodo: </b>{this.state.fechainicio} - {this.state.fechafin}</span>
                                <br/>
                                <span><b>Observación: </b> {this.state.observacion}</span>
                                <br/>
                                <button className="btn btn-outline-primary btn-sm" onClick={this.showForm} style={{ float: 'left', marginRight: '0.3em' }}>Editar</button>
                                <button className="btn btn-outline-danger btn-sm" onClick={() => { if (window.confirm('Estás seguro de eliminar este registro del WBS?')) this.onDelete(this.state.actividadseleccionada) }} style={{ float: 'left', marginRight: '0.3em' }}>Eliminar</button>
                            </div>
                        </div>
                    </div>

                </div>
                
                <div className="row" style={{marginBottom: '0.5em'}}>
                    <div className="col">
                    <Button label="Ingresar Nivel" icon="pi pi-upload" onClick={this.onClick} />
                    <Button label="Guardar" icon="pi pi-external-link" onClick={() => this.confirmacionGuardar()} />
                    <Button label="Exp/Contraer" icon="pi pi-external-link" onClick={this.Expander} />
                    </div>
                </div>

                <div className="row" style={{ height: '500px', maxHeight: '550px' }}>
                    <div className="col-sm-12" style={{ overflowX: 'scroll', overflowY: 'scroll' }}>
                        <ArbolWbs
                            dragdropScope="demo"
                            onSelect={this.onSelect}
                            data={this.state.data}
                            expandedKeys={this.state.expandedKeys}
                            onToggle={e => this.setState({ expandedKeys: e.value })}
                            onDragDrop={event => this.DragAndDrop(event)}
                        />
                    </div>
                    <Dialog header="Actividades" visible={this.state.visible_actividades_form} width="350px" modal={true} onHide={this.onHideVisibleActividad}>

                        <Formsy
                            onValidSubmit={() => this.handleSubmit()}
                            method="post"
                            ref="form"
                        >
                            <div className="form-group">
                                <label htmlFor="label">Disciplina:</label>
                                <Dropdown
                                    value={this.state.disciplinaid}
                                    options={this.state.disciplinas}
                                    onChange={(e) => { this.setState({ disciplinaid: e.value }) }}
                                    filter={true} filterPlaceholder="Selecciona una Disciplina"
                                    filterBy="label,value" placeholder="Selecciona una Disciplina"
                                    style={{ width: '100%', heigh: '18px' }}
                                    required
                                />
                            </div>

                            <div className="form-group">
                                <label>Observación</label>
                                <input
                                    id="no-filter"
                                    name="observacion"
                                    className="form-control"
                                    onChange={this.handleChange}
                                    validations="isText"
                                    value={this.state.observacion}
                                />

                            </div>

                            <div className="form-group">
                                <label>Fecha Inicio</label>
                                <input
                                    type="date"
                                    id="no-filter"
                                    name="fechainicio"
                                    className="form-control"
                                    onChange={this.handleChange}
                                    value={moment(this.state.fechainicio).format("YYYY-MM-DD")}
                                    required
                                />

                            </div>

                            <div className="form-group">
                                <label>Fecha Fin</label>
                                <input
                                    type="date"
                                    id="no-filter"
                                    name="fechafin"
                                    className="form-control"
                                    onChange={this.handleChange}
                                    value={moment(this.state.fechafin).format("YYYY-MM-DD")}
                                    required
                                />
                            </div>

                            <button type="submit" className="btn btn-primary" >Guardar</button>
                            <button style={{ marginLeft: '0.4em' }} type="button" className="btn btn-primary" onClick={this.onHideVisibleActividad}>Cancelar</button>
                        </Formsy>
                    </Dialog>

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
                            OfertaId={this.props.OfertaId}
                        />
                    </Dialog>

                    <Dialog header="Actividades" visible={this.state.visible_actividades} width="660px" height="500px" modal={true} onHide={this.onHideVisibleActividad}>
                        <ActividadForm
                            ListActividades={this.state.actividades}
                            updateData={this.updateData}
                            codigo_padre={this.state.PadreSeleccionado}
                            onHide={this.onHideVisibleActividad}
                            key={this.state.key_actividad_form}
                            showSuccess={this.successMessage}
                            showWarn={this.warnMessage}
                            OfertaId={this.props.OfertaId}
                        />
                    </Dialog>

                    <Dialog header="Reordenar" visible={this.state.visible_reordenar} width="350px" footer={footer} minY={70} onHide={this.onHide} maximizable={true}>
                        Esta Seguro de Guardar?
                    </Dialog>
                </div>
            </BlockUi>
        )
    }



    onClick(event) {
        event.preventDefault();
        this.setState({ visible: true, PadreSeleccionado: '.', NivelIdSeleccionado: 0, NombreNivel: '' });
    }

    DragAndDrop(e){
        this.setState({data: e.value});
    }

    updateData() {
        this.setState({ blocking: true })
        axios.get("/proyecto/Wbs/ApiWbsL/" + this.props.OfertaId, {})
            .then((response) => {
                this.setState({ data: response.data, blocking: false })
                this.props.SetearDatosRDO(response.data)
            })
            .catch((error) => {
                this.setState({ blocking: false })
                console.log(error);
            });
    }

    handleSubmit() {
        event.preventDefault();
        axios.post("/proyecto/Wbs/Edit", {
            OfertaId: this.state.ofertaid,
            Id: this.state.actividadseleccionada,
            estado: this.state.estado,
            observaciones: this.state.observacion,
            fecha_final: moment(this.state.fechafin).format("YYYY-MM-DD"),
            fecha_inicial: moment(this.state.fechainicio).format("YYYY-MM-DD"),
            es_actividad: this.state.es_actividad,
            vigente: this.state.vigente,
            id_nivel_codigo: this.state.PadreSeleccionado,
            id_nivel_padre_codigo: this.state.codigonivel,
            nivel_nombre: this.state.nombredisciplina,
            vigente: true,
            DisciplinaId: this.state.disciplinaid
        })
            .then((response) => {
                if (response.data == "ErrorFechas") {
                    this.warnMessage("La fecha final debe ser mayor a la inicial")
                } else {
                    this.updateData();
                    this.onHide();
                    this.successMessage("Actividad Actualizada")
                }

            })
            .catch((error) => {
                console.log(error);
                this.warnMessage("No se puedo realizar la acción")
            });
    }

    onDelete(id) {
        if (this.state.actividadseleccionada > 0) {
            event.preventDefault();
            axios.post("/proyecto/Wbs/Delete/" + id, {})
                .then((response) => {
                    if (response.data == "Ok") {
                        this.growl.show({ severity: 'success', summary: 'Correcto', detail: 'Se Elimino el registro' });
                        this.updateData();
                    } else {
                        this.warnMessage("La actividad tiene computos registrados");
                    }
                })
                .catch((error) => {
                    this.showWarn("No se puedo eliminar el registro");
                });
        } else {
            this.warnMessage("Debe seleccionar una actividad");
        }
    }

    confirmacionGuardar(event) {
        this.setState({ visible_reordenar: true });
    }

    onHideConfirmacionDrag() {
        this.setState({ visible_reordenar: false })
    }

    Refrescar(event) {
        this.updateData();
    }

    showActividad() {
        this.setState({ visible_actividades: true })
    }

    onHideVisibleActividad(event) {
        this.setState({
            visible_actividades: false, NombreNivel: '',
            PadreSeleccionado: "", visible: false, codigonivel: "", NivelIdSeleccionado: 0, NombreNivel: "", visibleRight: false, visible_actividades_form: false, actividadseleccionada: 0,
            ActividadIdSeleccionada: 0,
            observacion: "", revision: "", fechainicio: null, fechafin: null, es_actividad: 0, estado: 0, nombredisciplina: "", disciplinaid: 0

        });
    }

    handleChange(event) {
        event.stopPropagation();
        this.setState({ [event.target.name]: event.target.value });
    }

    obtenerKeys() {
        axios.post("/proyecto/Wbs/ApiWbsK/" + this.props.OfertaId, {})
            .then((response) => {
                var llaves = response.data;
                this.setState({ expandedKeys: llaves })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    onDragDrop(event) {
        this.setState({ data: event.value, blocking: false })
    }

    onSelect(event) {
        var ids = event.node.data.split(",");
        if (event.node.tipo == 'padre') {
            this.setState({
                PadreSeleccionado: ids[0], visible: true, codigonivel: ids[1], NivelIdSeleccionado: ids[2], NombreNivel: event.node.nivel_nombre, visibleRight: false, visible_actividades_form: false, ofertaid: ids[3], actividadseleccionada: 0,
                ActividadIdSeleccionada: 0, observacion: "", revision: "", fechainicio: null, fechafin: null, es_actividad: 0, estado: 0, nombredisciplina: "", disciplinaid: 0
            })
        } else {
            this.setState({
                PadreSeleccionado: ids[0], codigonivel: ids[1], actividadseleccionada: ids[2], nombredisciplina: event.node.nivel_nombre, ofertaid: ids[3],
                observacion: ids[4], fechainicio: ids[6], fechafin: ids[7], es_actividad: ids[8], estado: ids[9], disciplinaid: ids[10]
            })
            if (ids[5] == "1/1/0001") {
                this.setState({ fechainicio: "dd/mm/aaaa" })
            } else {
                this.setState({ fechainicio: ids[6] })
            }
            if (ids[6] == "1/1/0001") {
                this.setState({ fechafin: "dd/mm/aaaa" })
            } else {
                this.setState({ fechafin: ids[7] })
            }
        }
        this.setState({ selectedFile: event.node });
    }

    showForm() {
        if (this.state.actividadseleccionada > 0) {
            this.setState({ visible_actividades_form: true })
        } else {
            this.growl.show({ severity: 'warn', summary: 'Error', detail: 'Debe seleccionar una actividad' });
        }

    }

    onHide(event) {
        this.setState({ visible: false, NombreNivel: '' });
    }

    onHideVisibleActividad(event) {
        this.setState({
            visible_actividades: false, NombreNivel: '',
            PadreSeleccionado: "", visible: false, codigonivel: "", NivelIdSeleccionado: 0, NombreNivel: "", visibleRight: false, visible_actividades_form: false, actividadseleccionada: 0,
            ActividadIdSeleccionada: 0,
            observacion: "", fechainicio: null, fechafin: null, es_actividad: 0, estado: 0, nombredisciplina: "", disciplinaid: 0

        });
    }

    getCatalogos() {
        axios.post("/proyecto/catalogo/GetCatalogo/4", {})
            .then((response) => {
                var actividades = response.data.map(item => {
                    return { label: item.nombre, dataKey: item.Id, value: item.Id }
                })
                this.setState({ actividades: actividades })
            })
            .catch((error) => {
                console.log(error);
            });

        axios.post("/proyecto/catalogo/GetCatalogo/2", {})
            .then((response) => {
                var disciplinas = response.data.map(item => {
                    return { label: item.nombre, dataKey: item.Id, value: item.Id }
                })
                this.setState({ disciplinas: disciplinas })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    Expander() {
        axios.post("/proyecto/Wbs/ApiWbsK/" + this.props.OfertaId, {})
            .then((response) => {
                var llaves = response.data;
                if(this.state.expandir){
                    let expandedKeys = {  };
                    llaves.forEach((product) => {
                        if (expandedKeys[product])
                            delete expandedKeys[product];
                        else
                            expandedKeys[product] = true;
                    });
                    this.setState({ expandedKeys: expandedKeys, expandir: false });
                }else{
                    this.setState({ expandedKeys:{}, expandir: true });
                }
                
                
            })
            .catch((error) => {
                console.log(error);
            });
    }

    GuardarDrag() {
        this.setState({ blocking: true });
        axios.post("/proyecto/Wbs/ApiWbsD/", {
            data: this.state.data
        })
            .then((response) => {
                if (response.data == "OK") {
                    this.growl.show({ life: 5000, severity: 'info', summary: 'Proceso exitoso!', detail: 'OK' });
                    this.updateData();
                    this.setState({ blocking: false, visible: false });

                } else {

                }
            })
            .catch((error) => {
                console.log(error);
                this.setState({ blocking: false });
            });
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'info', summary: '', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: '', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }
}