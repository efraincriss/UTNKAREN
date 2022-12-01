import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import Field from "../Base/Field-v2";
import wrapForm from "../Base/BaseWrapper";
import { ScrollPanel } from 'primereact-v2/scrollpanel';
import moment from 'moment';
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import validationRules from '../Base/validationRules';
import { Button } from "primereact-v2/button";
import TreeItem from './TreeItem';
import ItPadreform from './ItPadreform';
import { Checkbox } from 'primereact-v2/checkbox';


export default class Items extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: [],
            ItemPadreSeleccionado: 0,
            key: 8248,
            visible: false,
            visiblepadre: false,
            key_form: 98723,
            id_seleccionado: 0,
            unidades: [],
            label_header: '',
            blocking: true,
            listaitems: [],
            errors: {},



            /*View  */
            viewnew: false,
            viewedit: false,
            viewdelete: false,
            action: "none",
            especialidades: [],
            /*Form Nuevo o Editar*/

            Id: 0,
            codigo: '',
            item_padre: '',
            nombre: '',
            descripcion: '',
            para_oferta: false,
            UnidadId: 0,
            vigente: true,
            GrupoId: 0,
            EspecialidadId: 0,
            PendienteAprobacion: false,
            Seleccionado: null



        }
        this.updateData = this.updateData.bind(this);
        this.onSelect = this.onSelect.bind(this);
        this.onHide = this.onHide.bind(this);
        this.onHidePadre = this.onHidePadre.bind(this);
        this.activarpadre = this.activarpadre.bind(this);
        this.getunidades = this.getunidades.bind(this);
        this.getpadres = this.getpadres.bind(this);
        this.onChangeValue = this.onChangeValue.bind(this);
        this.handleChange = this.handleChange.bind(this);

        /*
         
        */
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleSubmitedit = this.handleSubmitedit.bind(this);
        this.isValid = this.isValid.bind(this);
    }

    componentWillMount() {
        this.updateData();
        this.getunidades();
        this.getpadres();
    }

    onChangeValue = (name, value) => {
        this.setState({
            [name]: value
        });

    }

    isValid() {
        const errors = {};
        if (this.state.codigo === '') {
            errors.codigo = 'Campo requerido';
        }
        if (this.state.para_oferta) {
            if (this.state.PendienteAprobacion) {


                if (this.state.codigo != null && this.state.codigo != '') {
                    const integer = validationRules["isEspecialCode"]([], this.state.codigo);
                    if (!integer) {
                        errors.codigo = 'Item Pendiente debe poseer caracteres especiales al final en mayúsculas';
                    }

                }
            } else {
                if (this.state.codigo != null && this.state.codigo != '') {
                    const integer = validationRules["isInt"]([], this.state.codigo);
                    if (!integer) {
                        errors.codigo = 'Ingrese un número sin caracteres';
                    }

                }

            }
            if (this.state.UnidadId === 0) {
                errors.UnidadId = 'Campo requerido';
            }

        } else {


            if (this.state.codigo != null && this.state.codigo != '') {
                const integer = validationRules["isInt"]([], this.state.codigo);
                if (!integer) {
                    errors.codigo = 'Ingrese un número sin caracteres';
                }

            }
        }
        if (this.state.nombre === '') {
            errors.nombre = 'Campo requerido';
        }
        if (this.state.descripcion === '') {
            errors.descripcion = 'Campo requerido';
        }
        if (this.state.GrupoId === 0) {
            errors.GrupoId = 'Campo requerido';
        }


        console.log(errors);
        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    onSelect(event) {

    }
    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }
    SelectNode = (event) => {

        console.log(event.value);
        if (event.value > 0) {
            this.props.blockScreen();
            axios.post("/proyecto/Item/DetailsApi/" + event.value, {})
                .then((response) => {
                    this.setState({
                        Seleccionado: response.data

                    })
                    this.props.unlockScreen();
                })
                .catch((error) => {
                    console.log(error);
                    this.props.unlockScreen();
                });
        } else {

            abp.notify.error("Debe Seleccionar un Nodo", 'Error');

        }

    }

    render() {
        return (

            <div className="row">

                <div className="col-sm-12">
                    <Button label="Ingresar Padre" icon="pi pi-plus" className="btn btn-sm" onClick={this.activarpadre} /><br />

                    <TreeItem onSelect={this.onSelect}
                        data={this.state.data}
                        onHide={this.onHide}
                        showNew={this.showNew}
                        showEdit={this.showEdit}
                        SelectNode={this.SelectNode}
                        EliminarItem={this.EliminarItem}
                        Id={this.state.Id}
                        Seleccionado={this.state.Seleccionado}
                        showDelete={this.showDelete}
                    />

                </div>

                <Dialog header="Información Padre" visible={this.state.visiblepadre} width="600px" modal={true} onHide={this.onHidePadre}>
                    <ItPadreform
                        updateData={this.updateData}
                        onHidePadre={this.onHidePadre}
                        Grupos_Item={this.state.listaitems}
                        errors={this.state.errors}
                    />
                </Dialog>

                <Dialog header={this.state.label_header} visible={this.state.viewnew} width="600px" modal={true} onHide={this.hideNew}>
                    <div>

                        <form onSubmit={this.handleSubmit}>
                            <div className="row">
                                <div className="col">

                                    <Field
                                        name="item_padre"
                                        label="Padre"
                                        required
                                        edit={true} readOnly={true}
                                        value={this.state.item_padre}
                                        onChange={this.handleChange}

                                        error={this.state.errors.item_padre}
                                    />
                                </div>
                                <div className="col">
                                    <Field
                                        name="codigo"
                                        label="Código Hijo"
                                        required
                                        edit={true}
                                        value={this.state.codigo}
                                        onChange={this.handleChange}

                                        error={this.state.errors.codigo}
                                    />
                                </div>

                            </div>
                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="nombre"
                                        label="Nombre"
                                        required
                                        edit={true}
                                        value={this.state.nombre}
                                        onChange={this.handleChange}
                                        error={this.state.errors.nombre}
                                    />
                                    <Field
                                        name="descripcion"
                                        label="Descripción"
                                        required
                                        edit={true}
                                        value={this.state.descripcion}
                                        onChange={this.handleChange}
                                        error={this.state.errors.descripcion}
                                    />
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">

                                    <label>Para Oferta</label>  < Checkbox checked={this.state.para_oferta} onChange={e => this.setState({ para_oferta: e.checked })} />
                                </div>
                                <div className="col">
                                    {this.state.para_oferta &&
                                        <div className="form-group">
                                            <label>Pendiente Aprobación</label>  < Checkbox checked={this.state.PendienteAprobacion} onChange={e => this.setState({ PendienteAprobacion: e.checked })} />
                                        </div>

                                    }
                                </div>
                            </div>
                            {this.state.para_oferta &&
                                <div className="row">
                                    <div className="col">
                                        <Field
                                            name="UnidadId"
                                            required
                                            value={this.state.UnidadId}
                                            label="Unidad"
                                            options={this.state.unidades}
                                            type={"select"}
                                            filter={true}
                                            onChange={this.onChangeValue}
                                            error={this.state.errors.UnidadId}
                                            readOnly={false}
                                            placeholder="Seleccione.."
                                            filterPlaceholder="Seleccione.."
                                        />
                                    </div>
                                    <div className="col">
                                        <Field
                                            name="EspecialidadId"
                                            value={this.state.EspecialidadId}
                                            label="Especialidad"
                                            options={this.state.especialidades}
                                            type={"select"}
                                            filter={true}
                                            onChange={this.onChangeValue}
                                            error={this.state.errors.EspecialidadId}
                                            readOnly={false}
                                            placeholder="Seleccione.."
                                            filterPlaceholder="Seleccione.."
                                        />
                                    </div>
                                </div>
                            }

                            <br />
                            <Button type="submit" label="Guardar" icon="pi pi-save" /> {" "}
                            <Button type="button" label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.hideNew} />


                        </form>
                    </div>


                </Dialog>
                <Dialog header={this.state.label_header} visible={this.state.viewedit} width="600px" modal={true} onHide={this.hideEdit}>
                    <div>

                        <form onSubmit={this.handleSubmitedit}>
                            <div className="row">
                                <div className="col">

                                    <Field
                                        name="item_padre"
                                        label="Padre"
                                        required
                                        readOnly={true}
                                        value={this.state.item_padre}
                                        onChange={this.handleChange}

                                        error={this.state.errors.item_padre}
                                    />
                                </div>
                                <div className="col">
                                    <Field
                                        name="codigo"
                                        label="Código Hijo"
                                        required
                                        edit={true}
                                        value={this.state.codigo}
                                        onChange={this.handleChange}

                                        error={this.state.errors.codigo}
                                    />
                                </div>

                            </div>
                            <div className="row">
                                <div className="col">
                                    <Field
                                        name="nombre"
                                        label="Nombre"
                                        required
                                        edit={true}
                                        value={this.state.nombre}
                                        onChange={this.handleChange}
                                        error={this.state.errors.nombre}
                                    />
                                    <Field
                                        name="descripcion"
                                        label="Descripción"
                                        required
                                        edit={true}
                                        value={this.state.descripcion}
                                        onChange={this.handleChange}
                                        error={this.state.errors.descripcion}
                                    />
                                </div>
                            </div>
                            {this.state.Seleccionado != null && this.state.Seleccionado.tieneHijos === 0 &&
                                this.state.Seleccionado.item_padre != "." &&
                                <div className="row">
                                    <div className="col">

                                        <label>Para Oferta</label>  < Checkbox checked={this.state.para_oferta} onChange={e => this.setState({ para_oferta: e.checked })} />

                                    </div>
                                    <div className="col">
                                        {this.state.para_oferta &&
                                            <div className="form-group">
                                                <label>Pendiente Aprobación</label>  < Checkbox checked={this.state.PendienteAprobacion} onChange={e => this.setState({ PendienteAprobacion: e.checked })} />
                                            </div>

                                        }
                                    </div>
                                </div>
                            }
                            {this.state.para_oferta &&
                                <div className="row">
                                    <div className="col">
                                        <Field
                                            name="UnidadId"
                                            required
                                            value={this.state.UnidadId}
                                            label="Unidad"
                                            options={this.state.unidades}
                                            type={"select"}
                                            filter={true}
                                            onChange={this.onChangeValue}
                                            error={this.state.errors.UnidadId}
                                            readOnly={false}
                                            placeholder="Seleccione.."
                                            filterPlaceholder="Seleccione.."
                                        />
                                    </div>
                                    <div className="col">
                                        <Field
                                            name="EspecialidadId"
                                            value={this.state.EspecialidadId}
                                            label="Especialidad"
                                            options={this.state.especialidades}
                                            type={"select"}
                                            filter={true}
                                            onChange={this.onChangeValue}
                                            error={this.state.errors.EspecialidadId}
                                            readOnly={false}
                                            placeholder="Seleccione.."
                                            filterPlaceholder="Seleccione.."
                                        />
                                    </div>
                                </div>
                            }

                            <br />
                            <Button type="submit" label="Editar" icon="pi pi-pencil" /> {" "}
                            <Button type="button" label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.hideEdit} />


                        </form>
                    </div>


                </Dialog>
                <Dialog header="Eliminar" visible={this.state.viewdelete} width="400px" modal={true} onHide={this.hideDelete}>
                    <div>

                        <b>Está seguro de eliminar el Item?</b>
                        <br />
                        <br />
                        <Button type="submit" label="Eliminar" onClick={this.EliminarItem} icon="pi pi-save" /> {" "}
                        <Button type="button" label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.hideDelete} />


                    </div>
                </Dialog>

            </div>

        );
    }

    handleSubmit(event) {
        event.preventDefault();
        if (!this.isValid()) {
            abp.notify.error("No ha ingresado los campos obligatorios  o existen datos inválidos.", 'Validación');
            return;
        }

        else {
            this.props.blockScreen();
            axios.post("/proyecto/Item/CrearItem", {
                codigo: this.state.codigo,
                item_padre: this.state.item_padre,
                nombre: this.state.nombre,
                descripcion: this.state.descripcion,
                UnidadId: this.state.UnidadId,
                para_oferta: this.state.para_oferta,
                vigente: 1,
                GrupoId: this.state.Seleccionado != null ? this.state.Seleccionado.GrupoId : 0,
                EspecialidadId: this.state.EspecialidadId,
                PendienteAprobacion: this.state.PendienteAprobacion
            })
                .then((response) => {

                    var r = response.data;

                    if (r == "Guardado") {
                        console.log("entro guardado");
                        abp.notify.success("Guardado Correctamente", 'Aviso');
                        this.setState({ viewnew: false });
                        this.updateData();
                    }
                    if (r == "Error") {
                        console.log("entro error");
                        abp.notify.error("No se pudo Completar Transaccion", 'Error');
                        this.props.unlockScreen();
                    }
                    if (r == "Movimiento") {
                        console.log("entro error movimiento");
                        abp.notify.error("Error el Item Padre Es Item de Movimiento", 'Error');
                        this.props.unlockScreen();
                    }
                    if (r == "Existe") {

                        abp.notify.error("El Código del Item ya se encuentra registrado", 'Error');
                        this.props.unlockScreen();
                    }


                })
                .catch((error) => {
                    abp.notify.error(error, 'Error');
                    this.props.unlockScreen();
                });
        }
    }
    handleSubmitedit(event) {


        event.preventDefault();
        if (!this.isValid()) {
            abp.notify.error("No ha ingresado los campos obligatorios  o existen datos inválidos.", 'Validación');
            return;
        }

        else {
            this.props.blockScreen();
            if (this.state.Seleccionado != null && this.state.Seleccionado.Id > 0) {
                axios.post("/proyecto/Item/EditApi", {
                    Id: this.state.Seleccionado.Id,
                    codigo: this.state.codigo,
                    item_padre: this.state.item_padre,
                    nombre: this.state.nombre,
                    descripcion: this.state.descripcion,
                    UnidadId: this.state.UnidadId,
                    para_oferta: this.state.para_oferta,
                    vigente: 1,
                    GrupoId: this.state.Seleccionado != null ? this.state.Seleccionado.GrupoId : 0,
                    EspecialidadId: this.state.EspecialidadId,
                    PendienteAprobacion: this.state.PendienteAprobacion
                })
                    .then((response) => {

                        var r = response.data;
                        if (r == "EXISTE") {
                            abp.notify.error("El Código del Item ya se encuentra registrado", 'Error');
                            this.props.unlockScreen();
                        }
                        if (r === "GUARDADO") {
                            abp.notify.success("Guardado Correctamente", 'Aviso');
                            this.setState({ viewedit: false });
                            this.updateData();
                        }
                        if (r === "ERROR") {
                            abp.notify.error("Existió un inconveniente inténtelo más tarde", 'Error');
                            this.props.unlockScreen();
                        }
                        if (r === "MOVIMIENTO") {
                            abp.notify.error("Error el Item Padre Es Item de Movimiento", 'Error');
                            this.props.unlockScreen();
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                        this.props.unlockScreen();
                    });
            } else {

                abp.notify.error("No se ha Seleccionado ningún item", 'Error');
            }
        }
    }

    EliminarItem = () => {
        if (this.state.Seleccionado != null && this.state.Seleccionado.Id > 0) {
            this.props.blockScreen();
            axios.post("/proyecto/Item/DeleteItem/" + this.state.Seleccionado.Id, {})
                .then((response) => {

                    var r = response.data;

                    if (r == "ELIMINADO") {
                        abp.notify.success("Item Eliminado", 'Aviso');
                        this.updateData();
                        this.setState({viewdelete:false});
                    }
                    if (r == "TIENEHIJOS") {
                        abp.notify.error("No se puede Eliminar Item tiene hijos relacionados", 'Error');
                        this.props.unlockScreen();
                    }
                    if (r == "ErrorEliminado") {
                        abp.notify.error("Ocurrió un error inesperado intentelo más tarde", 'Error');
                        this.props.unlockScreen();
                    }


                })
                .catch((error) => {
                    console.log(error);
                    this.props.unlockScreen();
                });

        }
        else {
            abp.notify.error("Debe Seleccionar un Item", 'Error');
        }

    }

    activarpadre(event) {
        this.setState({ visiblepadre: true, errors: {} });
    }

    onHide(event) {
        this.setState({ visible: false });
    }
    onHidePadre(event) {
        this.setState({ visiblepadre: false, errors: {} });
    }

    showNew = (event) => {
        console.log("CREAR")
        if (this.state.Seleccionado != null) {
            this.setState({
                viewnew: true,
                errors: {},
                action: "create",
                Id: 0,
                codigo: '',
                item_padre: this.state.Seleccionado.codigo,
                nombre: '',
                descripcion: '',
                para_oferta: false,
                UnidadId: 0,
                vigente: true,
                GrupoId: this.state.Seleccionado.GrupoId,
                EspecialidadId: 0,
                PendienteAprobacion: false,
                label_header: this.state.Seleccionado.codigo + " " + this.state.Seleccionado.nombre
            })
        }
    }
    showEdit = (event) => {
        console.log("EDITAR")
        if (this.state.Seleccionado != null) {
            console.log(this.state.Seleccionado)
            this.setState({
                viewedit: true,
                errors: {},
                action: "edit",
                Id: this.state.Seleccionado.Id,
                codigo: this.state.Seleccionado.apicodigo,
                item_padre: this.state.Seleccionado.item_padre,
                nombre: this.state.Seleccionado.nombre,
                descripcion: this.state.Seleccionado.descripcion,
                para_oferta: this.state.Seleccionado.para_oferta,
                UnidadId: this.state.Seleccionado.UnidadId,
                vigente: true,
                GrupoId: this.state.Seleccionado.GrupoId,
                EspecialidadId: this.state.Seleccionado.EspecialidadId,
                PendienteAprobacion: this.state.Seleccionado.PendienteAprobacion,
                label_header: this.state.Seleccionado.codigo + " " + this.state.Seleccionado.nombre
            })
        }
    }
    showDelete = (event) => {
        this.setState({ viewdelete: true })
    }
    hideNew = (event) => {
        this.setState({ viewnew: false, errors: {} })
    }
    hideEdit = (event) => {
        this.setState({ viewedit: false, errors: {} })
    }
    hideDelete = (event) => {
        this.setState({ viewdelete: false, errors: {} })
    }


    updateData() {
        this.props.blockScreen();
        axios.get("/Proyecto/Item/ItemsFormato2", {})
            .then((response) => {
                this.setState({ data: response.data, blocking: false, key: Math.random() })
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Error al Consultar Items Nuevos");
                this.props.unlockScreen();
            });
    }

    getunidades() {

        axios.post("/Proyecto/Computo/CatalogoUnidades")
            .then((response) => {

                var uns = response.data.map(i => {
                    return { label: i.nombre, value: i.Id }
                })

                this.setState({ unidades: uns })


            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Error al consultar catálogos de unidades");
            });


    }

    getpadres() {
        axios.post("/Proyecto/Item/DetailsGrupos")
            .then((response) => {

                var uns = response.data.map(i => {
                    return { label: i.descripcion, dataKey: i.Id, value: i.Id }
                })

                this.setState({ listaitems: uns })


            })
            .catch((error) => {
                console.log(error);
            });

        axios
            .get("/Proyecto/Item/DetailsGruposEspecialidades/?code=TDICIPLINAS", {})
            .then(response => {
                console.log(response.data.result)
                var items = response.data.result.map(item => {
                    return { label: item.nombre, dataKey: item.Id, value: item.Id };
                });
                this.setState({ especialidades: items });

            })
            .catch(error => {
                console.log(error);

            });

    }
}
const Container = wrapForm(Items);
ReactDOM.render(

    < Container />,
    document.getElementById('content')
);