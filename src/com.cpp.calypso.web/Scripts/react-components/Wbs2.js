import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Moment from 'moment';
import Formsy from 'formsy-react';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import { Dialog } from 'primereact/components/dialog/Dialog';
import { Button } from 'primereact/components/button/Button';
import { Growl } from 'primereact/components/growl/Growl';
import { Tree } from 'primereact-v2/components/tree/Tree';
import ActividadForm from './forms/ActividadForm';
import NivelForm from './wbs_components/NivelForm';
import { Dropdown } from 'primereact/components/dropdown/Dropdown'

class WbsContainer extends React.Component {

    constructor(props) {
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

            selectedFile: null,

        };
        this.Refrescar = this.Refrescar.bind(this);
        this.onClick = this.onClick.bind(this);
        this.onHide = this.onHide.bind(this);
        this.updateData = this.updateData.bind(this);
        this.onSelectionChange = this.onSelectionChange.bind(this);
        this.onHideVisibleActividad = this.onHideVisibleActividad.bind(this);
        this.updateActividadId = this.updateActividadId.bind(this);
        this.updateTableData = this.updateTableData.bind(this);
        this.getCatalogos = this.getCatalogos.bind(this);
        this.showActividad = this.showActividad.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.resetTable = this.resetTable.bind(this);

        this.onUnselect = this.onUnselect.bind(this);
        this.onSelect = this.onSelect.bind(this);
        this.showForm = this.showForm.bind(this);
        this.onDragDrop = this.onDragDrop.bind(this);
        this.getDisciplinas = this.getDisciplinas.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.obtenerKeys = this.obtenerKeys.bind(this);

    }


    obtenerKeys() {

        axios.post("/proyecto/Wbs/ApiWbsK/" + document.getElementById('OfertaId').className, {
            data: this.state.data
        })
            .then((response) => {
                var llaves = response.data;
                let expandedKeys = { ...this.state.expandedKeys };
                llaves.forEach((product) => {
                    if (expandedKeys[product])
                        delete expandedKeys[product];

                    else
                        expandedKeys[product] = true;

                });
                this.setState({ expandedKeys: expandedKeys });
            })
            .catch((error) => {
                console.log(error);
            });


    }
    showForm() {
        if (this.state.actividadseleccionada > 0) {
            this.setState({
                visible_actividades_form: true,

            })

        } else {
            this.growl.show({ severity: 'warn', summary: 'Error', detail: 'Debe seleccionar una actividad' });
        }

    }

    onDragDrop(event) {

        this.setState({ data: event.value, blocking: false })

    }
    onUnselect(event) {
        console.log("Nodo que solte");
        console.log(event);
    }
    onSelectionChange(event) {
        console.log("Nodo Actual");
        console.log(event);

    }

    onDelete(id) {
        if (this.state.actividadseleccionada > 0) {
            event.preventDefault();
            axios.post("/proyecto/Wbs/Delete/" + id, {})
                .then((response) => {
                    if (response.data == "Ok") {
                        this.growl.show({ severity: 'success', summary: 'Correcto', detail: 'Se Elimino el registro' });
                        this.updateData();
                        this.reset();
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

    onSelect(event) {
        var ids = event.node.data.split(",");

        if (event.node.tipo == 'padre') {
            this.setState({
                PadreSeleccionado: ids[0], visible: true, codigonivel: ids[1], NivelIdSeleccionado: ids[2], NombreNivel: event.node.label, visibleRight: false, visible_actividades_form: false, ofertaid: ids[3], actividadseleccionada: 0,
                ActividadIdSeleccionada: 0,
                observacion: "", revision: "", fechainicio: null, fechafin: null, es_actividad: 0, estado: 0, nombredisciplina: "", disciplinaid: 0
            }
            )
        } else {

            this.updateActividadId(ids[2]);

            this.setState({
                PadreSeleccionado: ids[0], codigonivel: ids[1], actividadseleccionada: ids[2], nombredisciplina: event.node.label, ofertaid: ids[3],
                observacion: ids[4], revision: ids[5], fechainicio: ids[6], fechafin: ids[7], es_actividad: ids[8], estado: ids[9], disciplinaid: ids[10]


            })

            if (ids[6] == "1/1/0001") {

                this.setState({ fechainicio: "dd/mm/aaaa" })

            } else {
                this.setState({ fechainicio: ids[6] })

            }
            if (ids[7] == "1/1/0001") {

                this.setState({ fechafin: "dd/mm/aaaa" })

            } else {
                this.setState({ fechafin: ids[7] })

            }


        }
        this.setState({ selectedFile: event.node });
        //this.growl.show({severity: 'info', summary: 'Seleccionado', detail: event.node.label});
    }
    componentWillMount() {
        this.updateData();
        this.getCatalogos();
    }

    handleChange(event) {
        event.stopPropagation();

        this.setState({ [event.target.name]: event.target.value });
    }

    render() {

        return (
            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>




                <div className="row">
                    <div className="col-sm-12">

                        <Button label="Ingresar Nivel" icon="pi pi-upload" onClick={this.onClick} />
                        <Button label="Refrescar" icon="pi pi-refresh" onClick={this.Refrescar} />
                        <Button onClick={this.obtenerKeys} label="Exp/Contraer" icon="pi pi-upload" />
                        <hr />
                    </div>
                    <div className="col-sm-8" style={{ overflowX: 'scroll', overflowY: 'scroll' }}>
                        <Tree value={this.state.data}
                            expandedKeys={this.state.expandedKeys}
                            onToggle={e => this.setState({ expandedKeys: e.value })}

                            selectionMode="single"
                            selectionKeys={this.state.selectedFile}
                            onSelect={this.onSelect}></Tree>


                    </div>

                    <div className="col-sm-4">
                        <div className="form-group">

                            <input type="hidden" value={this.state.actividadseleccionada} className="form-control" disabled />
                        </div>
                        <div className="form-group" >
                            <b><label style={{ fontSize: '12px' }} >Actividad :&nbsp;</label></b>
                            <label style={{ fontSize: '12px' }}>{this.state.nombredisciplina}</label>
                        </div>
                        <div className="form-group" >
                            <b><label style={{ fontSize: '12px' }} >Revisión :&nbsp;</label></b>

                            <label style={{ fontSize: '12px' }}>{this.state.revision}</label>
                        </div>
                        <div className="form-group">
                            <b><label style={{ fontSize: '12px' }}> Observación :&nbsp;</label></b>
                            <label style={{ fontSize: '12px' }}>{this.state.observacion}</label>
                        </div>
                        <div className="form-group">
                            <b><label style={{ fontSize: '12px' }}>Fecha Inicio :&nbsp;</label></b>

                            <label style={{ fontSize: '12px' }}>{this.state.fechainicio}</label>

                        </div>
                        <div className="form-group">
                            <b><label style={{ fontSize: '12px' }}>Fecha Fin :&nbsp;</label></b>
                            <label style={{ fontSize: '12px' }}>{this.state.fechafin}</label>
                        </div>

                        <button className="btn btn-outline-primary" onClick={this.showForm} style={{ float: 'left', marginRight: '0.3em' }}>Editar</button>
                        <button className="btn btn-outline-danger" onClick={() => { if (window.confirm('Estás seguro de eliminar este registro del WBS?')) this.onDelete(this.state.actividadseleccionada) }} style={{ float: 'left', marginRight: '0.3em' }}>Eliminar</button>


                    </div>
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
                            <label>Revisión</label>
                            <input
                                id="no-filter"
                                name="revision"
                                className="form-control"
                                onChange={this.handleChange}
                                validations="isText"
                                value={this.state.revision}
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
                                value={Moment(this.state.fechainicio).format("YYYY-MM-DD")}
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
                                value={Moment(this.state.fechafin).format("YYYY-MM-DD")}
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

            </BlockUi>
        );
    }

    onClick(event) {
        event.preventDefault();
        this.setState({ visible: true, PadreSeleccionado: '.', NivelIdSeleccionado: 0, NombreNivel: '' });
    }

    onHide(event) {
        this.setState({ visible: false, NombreNivel: '' });
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

    updateData() {
        axios.get("/proyecto/Wbs/ApiWbs/" + document.getElementById('content').className, {})
            .then((response) => {
                this.setState({ data: response.data, key: Math.random(), blocking: false })
            })
            .catch((error) => {
                console.log(error);
            });
    }
    Refrescar(event) {
        this.updateData();
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

    updateTableData() {
        axios.post("/proyecto/Wbs/DetailsApi/" + this.state.ActividadIdSeleccionada, {})
            .then((response) => {
                console.log(response.data)
                var data = [];
                data.push(response.data);

                this.setState({ table_data: data, data_item: response.data })

            })
            .catch((error) => {
                console.log(error);

            });

    }

    resetTable() {
        this.setState({ dataItem: {}, table_data: [] })
    }

    updateActividadId(id) {

        this.setState({
            ActividadIdSeleccionada: id
        },
            this.updateTableData
        )
    }
    getDisciplinas() {

        axios.post("/proyecto/catalogo/GetCatalogo/2", {})
            .then((response) => {

                var disciplina = response.data.map(item => {

                    return { label: item.nombre, dataKey: item.Id, value: item.Id }
                })

                this.setState({ disciplinas: [], disciplinas: disciplina })
            })
            .catch((error) => {
                console.log(error);
            });


    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'info', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }



    handleSubmit() {
        event.preventDefault();
        axios.post("/proyecto/Wbs/Edit", {
            OfertaId: this.state.ofertaid,
            Id: this.state.ActividadIdSeleccionada,
            estado: this.state.estado,
            observaciones: this.state.observacion,
            fecha_final: Moment(this.state.fechafin).format("YYYY-MM-DD"),
            fecha_inicial: Moment(this.state.fechainicio).format("YYYY-MM-DD"),
            es_actividad: this.state.es_actividad,
            vigente: this.state.vigente,
            id_nivel_codigo: this.state.PadreSeleccionado,
            id_nivel_padre_codigo: this.state.codigonivel,
            nivel_nombre: this.state.nombredisciplina,
            vigente: true,
            revision: this.state.revision,
            DisciplinaId: this.state.disciplinaid
        })
            .then((response) => {
                if (response.data == "ErrorFechas") {
                    this.growl.show({ severity: 'warn', summary: 'Error', detail: 'fechas' });
                } else {
                    console.log(reponse.data);
                    this.updateActividad(this.state.data[0].Id);
                    this.onHide();
                    this.growl.show({ severity: 'success', summary: 'Ok', detail: 'Correcto' });
                }

            })
            .catch((error) => {
                console.log(error);
                this.growl.show({ severity: 'warn', summary: 'error', detail: 'No se Guardo' });
            });
    }
}

ReactDOM.render(
    <WbsContainer />,
    document.getElementById('content')
);

