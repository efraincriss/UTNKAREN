import React, { Component } from 'react';
import BlockUi from 'react-block-ui';
import axios from 'axios';
import moment from 'moment';

import http from '../Base/HttpService';
import { Dropdown } from 'primereact/components/dropdown/Dropdown';

//import Field from '../Base/Field';
import Field from "../Base/Field-v2";
import config from '../Base/Config';
import ActionForm from '../Base/ActionForm';
import wrapForm from '../Base/WrapForm';

class SolicitudViandaForm extends Component {

    constructor(props) {
        super(props);


        this.state = {
            data: this.initData(),
            dataExtra: {
                solicitantes: [],
                anotadores: [],
                tiposComidas: [],
                disciplinas: [],
                locaciones: [],
                locaciones_info: [],
                areas: [],
            },
            blocking: true,
            loadDataExtra: false,
            loading: false,
            errors: {}
        };

        this.handleChange = this.handleChange.bind(this);
        this.setData = this.setData.bind(this);

        this.handleSubmit = this.handleSubmit.bind(this);
        this.onChangeValue = this.onChangeValue.bind(this);
    }


    onChangeValue = (name, value) => {
        this.setState({
            [name]: value
        });

    }
    componentDidMount() {
        console.log('SolicitudViandaForm.componentDidMount');
    }

    componentDidUpdate(prevProps) {
        console.log('SolicitudViandaForm.componentDidUpdate');

        if (!this.state.loadDataExtra && this.props.show && !this.state.loading) {
            //Init
            this.loadDataExtra();
        }

        // Typical usage (don't forget to compare props):
        if (this.props.show && (this.props.entityId !== prevProps.entityId ||
            this.props.show !== prevProps.show)) {
            this.loadData();
        }
    }

    initData() {
        let dataInit = {
            anotador_id: 0,
            solicitante_id: 0,
            solicitante_nombre: '',
            LocacionId: 0,
            tipo_comida_id: 0,
            disciplina_id: 0,
            area_id: 0,
            fecha_solicitud: moment().format(config.formatDate),
            fecha_alcancce: '',
            pedido_viandas: 0,
            alcance_viandas: 0,
            total_pedido: 0,
            consumido: 0,
            consumo_justificado: 0,
            total_consumido: 0,
            por_justificar: 0,
            estado: 1,
            referencia_ubicacion: '.',
        };
        return dataInit;
    }

    loadData() {
        console.log('this.props.entityId : ' + this.props.entityId);

        if (this.props.entityId > 0) {

            let url = '';
            url = `/proveedor/SolicitudVianda/GetApi/${this.props.entityId}`;


            http.get(url, {})
                .then((response) => {

                    let data = response.data;

                    if (data.success === true) {

                        console.log(response.data);

                        //Fix Date
                        let dataEntity = data.result;
                        this.normalizingData(dataEntity);

                        this.setState({
                            data: dataEntity
                        });

                    } else {
                        //TODO: 
                        //Presentar errores... 
                        var message = $.fn.responseAjaxErrorToString(data);
                        abp.notify.error(message, 'Error');
                    }


                    this.setState({ blocking: false, errors: {} });

                })
                .catch((error) => {
                    console.log(error);
                });
        } else {

            this.setState({
                data: this.initData(),
                errors: {},
                blocking: false
            });

        }
    }

    normalizingData(dataEntity) {
        dataEntity['fecha_solicitud'] = moment(dataEntity['fecha_solicitud']).format(config.formatDate);
        if (dataEntity['fecha_alcancce'] !== undefined && dataEntity['fecha_alcancce'] !== null && moment(dataEntity['fecha_alcancce']).isValid()) {
            dataEntity['fecha_alcancce'] = moment(dataEntity['fecha_alcancce']).format(config.formatDate);
        } else {
            dataEntity['fecha_alcancce'] = '';
        }

        if (!dataEntity.observaciones) {
            dataEntity.observaciones = "";
        }
    }

    getSolicitantes() {

        let url = '';
        url = `/Proveedor/SolicitudVianda/SearchColaboradores`;

        return http.get(url);
    }
    getAnotadores() {

        let url = '';
        url = `/Proveedor/SolicitudVianda/SearchAnotadores`;

        return http.get(url);
    }

    getTipoComida() {
        let url = '';
        url = `/Proveedor/SolicitudVianda/SearchTipoComida/?code=TIPOCOMIDA`;

        return http.get(url);
    }

    getDisciplina() {
        let url = '';
        url = `/Proveedor/SolicitudVianda/SearchByCodeApi/?code=DISCIPLINA`;

        return http.get(url);
    }

    getArea() {
        let url = '';
        url = `/Proveedor/SolicitudVianda/SearchByCodeApi/?code=TAREAS`;

        return http.get(url);
    }

    getLocacion() {
        let url = '';

        // url = `${config.apiUrl}/proveedor/SolicitudVianda/GetLocacionApi`;
        url = `/Proveedor/SolicitudVianda/SearchByCodeApi/?code=LOCACIONDETRABAJO`;
        return http.get(url);
    }

    loadDataExtra() {

        var self = this;

        self.setState({ blocking: true, loading: true });

        Promise.all([this.getSolicitantes(), this.getAnotadores(), this.getTipoComida(), this.getDisciplina(), this.getLocacion(), this.getArea()])
            .then(function ([solicitantes, anotadores, tiposComidas, disciplinas, locaciones, areas]) {
                self.setState({ blocking: false, loadDataExtra: true });


                self.setState({
                    dataExtra: {
                        solicitantes: solicitantes.data.result.map(item => {
                            return { label: item.nombres, dataKey: item.Id, value: item.Id };
                        }),
                        anotadores: anotadores.data.result.map(item => {
                            return { label: item.nombres, dataKey: item.Id, value: item.Id };
                        }),
                        tiposComidas: tiposComidas.data.result.map(item => {
                            return { label: item.nombre, dataKey: item.Id, value: item.Id };
                        }),
                        disciplinas: disciplinas.data.result.map(item => {
                            return { label: item.nombre, dataKey: item.Id, value: item.Id };
                        }),
                        locaciones: self.props.mapDropdown(locaciones.data, 'nombre', 'Id'),
                        locaciones_info: locaciones.data.result,
                        areas: self.props.mapDropdown(areas.data, 'nombre', 'Id')

                        /* solicitantes: self.props.mapDropdown(solicitantes.data, 'nombres', 'Id'),
                         anotadores: self.props.mapDropdown(anotadores.data, 'nombres', 'Id'),
                         tiposComidas: self.props.mapDropdown(tiposComidas.data, 'nombre', 'Id'),
                         disciplinas: self.props.mapDropdown(disciplinas.data, 'nombre', 'Id'),
                         locaciones: self.props.mapDropdown(locaciones.data, 'nombre', 'Id'),
                         locaciones_info: locaciones.data.result,
                         areas: self.props.mapDropdown(areas.data, 'nombre', 'Id')
                     */
                    }
                });

                self.setState({ blocking: false });
                self.setState({ loading: false });

            })
            .catch((error) => {
                self.setState({ blocking: false });
                self.setState({ loading: false });
                console.log(error);
            });
    }


    isValid() {
        const errors = {};

        if (this.state.data.fecha_solicitud === undefined ||
            !moment(this.state.data.fecha_solicitud).isValid()) {
            errors.fecha_solicitud = 'Debe ingresar una Fecha';
        }


        if (this.state.data.tipo_comida_id === undefined || this.state.data.tipo_comida_id <= 0) {
            errors.tipo_comida_id = 'Debe seleccionar un tipo de Comida';
        }

        if (this.state.data.solicitante_id === undefined || this.state.data.solicitante_id <= 0) {
            errors.solicitante_id = 'Debe seleccionar un solicitante';
        }
        if (this.state.data.anotador_id === undefined || this.state.data.anotador_id <= 0) {
            errors.anotador_id = 'Debe seleccionar un Anotador';
        }

        if (this.state.data.LocacionId === undefined || this.state.data.LocacionId <= 0) {
            errors.LocacionId = 'Debe seleccionar una localización';
        }

        /*if (this.state.data.area_id === undefined || this.state.data.area_id <= 0) {
            errors.area_id = 'Debe seleccionar una área';
        }
        */
        if (this.state.data.disciplina_id === undefined || this.state.data.disciplina_id <= 0) {
            errors.disciplina_id = 'Debe seleccionar una disciplina';
        }

        if (this.state.data.pedido_viandas === undefined || this.state.data.pedido_viandas <= 0) {
            errors.pedido_viandas = 'Debe ingresar un valor mayor a cero';
        }

        /*if (this.props.entityAction === 'edit' && (this.state.data.alcance_viandas === undefined || this.state.data.alcance_viandas <= 0)) {
            errors.alcance_viandas = 'Debe ingresar un valor mayor a cero';
        }*/

        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    handleSubmit(event) {
        event.preventDefault();

        if (!this.isValid()) {
            abp.notify.error("No ha ingresado los campos obligatorios  o existen datos inválidos.", 'Validación');
            return;
        }

        console.log(this.state);

        this.setState({ blocking: true });

        let url = '';
        if (this.props.entityAction === 'edit')
            url = `/proveedor/SolicitudVianda/EditApi`;
        else
            url = `/proveedor/SolicitudVianda/CreateApi`;



        //creating copy of object
        let data = Object.assign({}, this.state.data);
        data.Id = this.props.entityId;                        //updating value

        http.post(url, data)
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    this.setState({
                        data: this.initData()
                    });


                    if (this.props.entityId <= 0) {
                        this.props.onAdded(response.data.result.id);
                    }
                    else {
                        this.props.onUpdated(response.data.result.id);
                    }

                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(data);
                    abp.notify.error(message, 'Error');
                }


                this.setState({ blocking: false });

            })
            .catch((error) => {
                console.log(error);

                this.setState({ blocking: false });
            });

    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === "checkbox" ? target.checked : target.value;
        const name = target.name;

        const updatedData = {
            ...this.state.data
        };

        updatedData[name] = value;

        //Compute
        this.updateFieldCompute(updatedData);

        this.setState({
            data: updatedData
        });
    }

    updateFieldCompute(updatedData) {
        let pedido_viandas = parseInt(updatedData["pedido_viandas"]);
        if (isNaN(pedido_viandas))
            pedido_viandas = 0;

        let alcance_viandas = parseInt(updatedData["alcance_viandas"]);
        if (isNaN(alcance_viandas))
            alcance_viandas = 0;

        updatedData["total_pedido"] = pedido_viandas + alcance_viandas;
    }

    setData(name, value) {

        const updatedData = {
            ...this.state.data
        };

        updatedData[name] = value;

        /*if (name === "LocacionId") {
            var locacion = this.state.dataExtra.locaciones_info.filter(item => item.Id === value);
            if (locacion.length === 1) {
                updatedData["referencia_ubicacion"] = "";
            }
        }
        */

        this.setState({
            data: updatedData
        });
        console.log(this.state.data)

    }

    render() {


        let blocking = this.state.blocking || this.state.loading || this.props.blocking;

        return (

            <BlockUi tag="div" blocking={blocking}>

                <form >
                    <div className="row">
                        <div className="col">

                            <Field
                                name="fecha_solicitud"
                                label="Fecha"
                                required
                                type="date"
                                edit={(this.props.entityAction === 'create' || this.props.entityAction === 'edit')}
                                readOnly={(this.props.entityAction === 'show')}
                                value={this.state.data.fecha_solicitud}
                                onChange={this.handleChange}
                                error={this.state.errors.fecha_solicitud}
                            />

                        </div>


                        <div className="col">

                            <Field
                                name="tipo_comida_id"
                                required
                                value={this.state.data.tipo_comida_id}
                                label="Tipo Comida"
                                options={this.state.dataExtra.tiposComidas}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.tipo_comida_id}
                                readOnly={(this.props.entityAction === 'show')}
                                filter={true} filterPlaceholder="Seleccione"
                                filterBy="label,value" placeholder="Seleccione"
                            />

                        </div>
                    </div>
                    <div className="row">
                        <div className="col">

                            <Field
                                name="solicitante_id"
                                required
                                value={this.state.data.solicitante_id}
                                label="Solicitante"
                                options={this.state.dataExtra.solicitantes}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.solicitante_id}
                                readOnly={(this.props.entityAction === 'show')}
                                filter={true} 
                                filterBy="label,value"
                                placeholder="Seleccione"

                            />
                        </div>
                        <div className="col">

                            <Field
                                name="anotador_id"
                                required
                                value={this.state.data.anotador_id}
                                label="Anotador"
                                options={this.state.dataExtra.anotadores}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.anotador_id}
                                readOnly={(this.props.entityAction === 'show')}
                                filter={true} filterPlaceholder="Seleccione"
                                filterBy="label,value" placeholder="Seleccione"

                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">

                            <Field
                                name="LocacionId"
                                required
                                value={this.state.data.LocacionId}
                                label="Locación"
                                options={this.state.dataExtra.locaciones}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.LocacionId}
                                readOnly={(this.props.entityAction === 'show')}
                                filter={true} filterPlaceholder="Seleccione"
                                filterBy="label,value" placeholder="Seleccione"

                            />

                        </div>
                        <div className="col">

                            <Field
                                name="area_id"
                                value={this.state.data.area_id}
                                label="Área"
                                options={this.state.dataExtra.areas}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.area_id}
                                readOnly={(this.props.entityAction === 'show')}
                                filter={true} filterPlaceholder="Seleccione"
                                filterBy="label,value" placeholder="Seleccione"

                            />


                        </div>
                        <div className="col">

                            <Field
                                name="disciplina_id"
                                required
                                value={this.state.data.disciplina_id}
                                label="Disciplina"
                                options={this.state.dataExtra.disciplinas}
                                type={"select"}
                                onChange={this.setData}
                                error={this.state.errors.disciplina_id}
                                readOnly={(this.props.entityAction === 'show')}
                                filter={true} filterPlaceholder="Seleccione"
                                filterBy="label,value" placeholder="Seleccione"
                            />
                        </div>
                    </div>


                    <div className="row">
                        <div className="col">

                            <Field
                                name="pedido_viandas"
                                label="Viandas Originales"
                                required
                                type="number"
                                min="0"
                                edit={(this.props.entityAction === 'create')}
                                readOnly={(this.props.entityAction === 'show' || this.props.entityAction === 'edit')}
                                value={this.state.data.pedido_viandas}
                                onChange={this.handleChange}
                                error={this.state.errors.pedido_viandas}
                            />

                        </div>
                        <div className="col">

                            <Field
                                name="alcance_viandas"
                                label="Viandas Adicionales"
                                type="number"
                                min="0"
                                edit={(this.props.entityAction === 'edit')}
                                readOnly={(this.props.entityAction === 'show' || this.props.entityAction === 'create')}
                                value={this.state.data.alcance_viandas}
                                onChange={this.handleChange}
                                error={this.state.errors.alcance_viandas}
                            />

                        </div>
                        <div className="col">
                            <Field
                                name="total_pedido"
                                label="Viandas Totales"
                                type="number"
                                min="0"
                                edit={false}
                                readOnly
                                value={this.state.data.total_pedido}
                                onChange={this.handleChange}
                                error={this.state.errors.total_pedido}
                            />

                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                        </div>
                    </div>
                    {(this.props.entityAction === 'create' || this.props.entityAction === 'edit') &&
                        <ActionForm onCancel={this.props.onHide} onSave={this.handleSubmit} />
                    }

                    {this.props.entityAction === 'show' &&
                        <ActionForm onAccept={this.props.onHide} />
                    }

                </form>
            </BlockUi>
        );
    }
}

export default wrapForm(SolicitudViandaForm);