import React from 'react';
import axios from 'axios';
import { Button } from 'primereact-v2/components/button/Button';
import { Growl } from 'primereact-v2/components/growl/Growl';
import { Textbox } from 'react-inputs-validation/lib/components';
import Field from "../Base/Field-v2";
import validationRules from '../Base/validationRules';
const estilo = { height: '25px' }

export default class ItPadreform extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            codigo: '',
            nombre: '',
            descripcion: '',
            UnidadId: 0,
            para_oferta: 0,
            codigoForm: '',
            nombreForm: '',
            descripcionForm: '',
            UnidadIdForm: 0,
            para_ofertaForm: 0,
            IdForm: 0,
            vigente: true,
            itemPadreForm: '.',

            /* Grupos Items*/
            GrupoId: 0,
            errors: {},


        }
        this.showSuccess = this.showSuccess.bind(this);
        this.handleSubmitPadre = this.handleSubmitPadre.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showWarn = this.showWarn.bind(this);

        /* Change Items */
        this.isValid = this.isValid.bind(this);
        this.onChangeValue = this.onChangeValue.bind(this);
    }

    componentWillReceiveProps(nextProps) {
        this.setState({
            errors: {},
        })
    }

    isValid() {
        const errors = {};
        if (this.state.codigo === '') {
            errors.codigo = 'Campo requerido';
        }
        if (this.state.codigo != null && this.state.codigo != '') {
            const integer = validationRules["isInt"]([], this.state.codigo);
            if (!integer) {
                errors.codigo = 'Ingrese un número sin caracteres';
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



        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

    render() {
        return (
            <div>

                <form onSubmit={this.handleSubmitPadre}>
                    <div className="row">
                        <div className="col">

                            <Field
                                name="codigo"
                                label="Código"
                                required
                                edit={true}
                                value={this.state.codigo}
                                onChange={this.handleChange}

                                error={this.state.errors.codigo}
                            />
                        </div>
                        <div className="col">
                            <Field
                                name="GrupoId"
                                required
                                value={this.state.GrupoId}
                                label="Grupo"
                                options={this.props.Grupos_Item}
                                type={"select"}
                                filter={true}
                                onChange={this.onChangeValue}
                                error={this.state.errors.GrupoId}
                                readOnly={false}
                                placeholder="Seleccione.."
                                filterPlaceholder="Seleccione.."
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


                    <Button type="submit" label="Guardar" icon="pi pi-save" /> {" "}
                    <Button type="button" label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.props.onHidePadre} />


                </form>
            </div>


        );
    }

    handleSubmitPadre(event) {
        event.preventDefault();
        if (!this.isValid()) {
            abp.notify.error("No ha ingresado los campos obligatorios  o existen datos inválidos.", 'Validación');
            return;
        }

        else {



            axios.post("/proyecto/Item/CrearItemPadre", {
                codigo: this.state.codigo,
                item_padre: this.state.itemPadreForm,
                nombre: this.state.nombre,
                descripcion: this.state.descripcion,
                UnidadId: this.state.UnidadId,
                para_oferta: false,
                vigente: 1,
                GrupoId: this.state.GrupoId
            })
                .then((response) => {
                    console.log(response.data);
                    var r = response.data;

                    if (r == "Guardado") {
                        console.log("entro guardado");
                        abp.notify.success("Item Guardado", 'Aviso');
                        this.setState({
                            codigo: '',
                            nombre: '',
                            descripcion: '',
                            UnidadIdForm: 0,
                            GrupoId: 0


                        })
                        this.props.onHidePadre();
                        this.props.updateData();
                    }
                    if (r == "Existe") {
                        console.log("entro existe");
                        abp.notify.error("El Código del Item ya Existe", 'Error');
                    }


                })
                .catch((error) => {
                    abp.notify.error("Ocurrió un error intentelo más tarde", 'Error');
                });
        }
    }
    onChangeValue = (name, value) => {
        this.setState({
            [name]: value
        });

    }
    showSuccess() {
        this.growl.show({ severity: 'success', summary: 'Realizao correctamente', detail: 'Item Creado Correctamente' });
    }

    showWarn() {
        this.growl.show({ severity: 'error', summary: 'Error', detail: 'El Item Ya Existe' });
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }
}

