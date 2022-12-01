import React from 'react';
import axios from 'axios';
import moment from 'moment';
import BlockUi from 'react-block-ui';
import Field from "../../Base/Field-v2";

export default class EditarBaja extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            baja_id: '',
            tipo_identificacion: '',
            nro_identificacion: '',
            nombres_apellidos: '',
            nro_legajo: '',
            id_sap: '',
            motivo_baja: '',
            fecha_baja: '',
            motivo_edicion: '',
            formIsValid: '',
            errores: [],
            loading: false,

            //*ES: Actualización **/
            MotivoBajaId: 0,
            detalle_baja: '',
            errors: {},
            fecha_ingreso: ''
        }

        this.handleValidation = this.handleValidation.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.Guardar = this.Guardar.bind(this);
        this.clearStates = this.clearStates.bind(this);
        this.onChangeValue = this.onChangeValue.bind(this);
    }

    componentDidMount() {

    }

    componentWillReceiveProps(nextProps) {
        console.log('nextProps', nextProps);
        this.setState({
            baja_id: nextProps.baja_id,
            tipo_identificacion: nextProps.tipo_identificacion,
            nro_identificacion: nextProps.nro_identificacion,
            nombres_apellidos: nextProps.nombres_apellidos,
            nro_legajo: nextProps.nro_legajo,
            id_sap: nextProps.id_sap,
            motivo_baja: nextProps.motivo_baja,
            fecha_baja: moment(nextProps.fecha_baja).format("YYYY-MM-DD"),
            detalle_baja: nextProps.detalle_baja,
            MotivoBajaId: nextProps.MotivoBajaId,
            motivo_edicion: nextProps.motivo_edicion,
            fecha_ingreso: moment(nextProps.fecha_ingreso).format("YYYY-MM-DD"),
            errores: [],
        })
    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <form onSubmit={this.handleSubmit}>
                        <div className="row" >
                            <div className="col-xs-12 col-md-12" >
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="text"><b>Tipo de Identificación:</b> {this.state.tipo_identificacion} </label>
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="text"><b>No. de Identificación:</b> {this.state.nro_identificacion} </label>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="text"><b>Apellidos Nombres:</b> {this.state.nombres_apellidos} </label>
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="text"><b>ID Legajo:</b> {this.state.nro_legajo} </label>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="text"><b>ID SAP:</b> {this.state.id_sap} </label>
                                        </div>
                                    </div>
                                    <div className="col">

                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="fecha_baja">* Fecha de Baja: </label>
                                            <input type="date" id="fecha_baja" className="form-control" value={this.state.fecha_baja} onChange={this.handleChange} name="fecha_baja" />
                                            {this.state.errores["fecha_baja"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <Field
                                            name="MotivoBajaId"
                                            required
                                            value={this.state.MotivoBajaId}
                                            label="Motivo Baja"
                                            options={this.props.CatalogoBajas}
                                            type={"select"}
                                            filter={true}
                                            onChange={this.onChangeValue}
                                            error={this.state.errors.MotivoBajaId}
                                            readOnly={false}

                                            placeholder="Seleccione.."
                                            filterPlaceholder="Seleccione.."
                                        />

                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="motivo_edicion">* Motivo de Edición: </label>
                                            <textarea type="text" id="motivo_edicion" /*rows="1"*/ maxLength="200" className="form-control" value={this.state.motivo_edicion} onChange={this.handleChange} name="motivo_edicion" />
                                            {this.state.errores["motivo_edicion"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="detalle_baja">* Detalle de Baja: </label>
                                            <input type="text" id="motivo_edicion" /*rows="1"*/ maxLength="200" className="form-control" value={this.state.detalle_baja} onChange={this.handleChange} name="detalle_baja" />
                                            {this.state.errores["detalle_baja"]}
                                        </div>
                                    </div>
                                </div>



                                <br />
                                <div className="form-group">
                                    <div className="col">
                                        <button type="button" onClick={this.Guardar} className="btn btn-outline-primary"> Guardar</button>
                                        <button onClick={() => this.props.onHide()} type="button" className="btn btn-outline-primary" style={{ marginLeft: '3px' }}> Cancelar</button>

                                    </div>
                                </div>
                            </div>
                        </div>


                    </form>
                </div >
            </BlockUi>
        )
    }
    onChangeValue = (name, value) => {
        this.setState({
            [name]: value
        });
    };
    clearStates() {
        this.setState({
            baja_id: '',
            tipo_identificacion: '',
            nro_identificacion: '',
            nombres_apellidos: '',
            nro_legajo: '',
            id_sap: '',
            motivo_baja: '',
            fecha_baja: '',
            motivo_edicion: '',
            errores: []
        }, this.props.onHide())
    }

    handleValidation() {
        let errors = {};
        this.state.formIsValid = true;
        var today = moment().format("YYYY-MM-DD");
        var check = moment(today, 'YYYY/MM/DD');
        var month = check.format('M');
        var last = moment().endOf('month').format('D');

        console.log(this.props.fecha_ingreso);
        if (!this.state.fecha_baja) {
            this.state.formIsValid = false;
            errors["fecha_baja"] = <div className="alert alert-danger">El campo Fecha de Baja es obligatorio.</div>;
        } else {
            console.log('Edit')
            var fecha = moment(this.state.fecha_baja, 'YYYY/MM/DD');
            console.log('month', month, fecha.format('M'), last)
            var fechabaja = moment(this.state.fecha_baja).format("YYYY-MM-DD");
            var fechaIngreso = moment(this.props.fecha_ingreso).format("YYYY-MM-DD");
            console.log('fechabaja', fechabaja);
            console.log('fechaIngreso', fechaIngreso);
           /* if (fechabaja < fechaIngreso) {
                console.log('True')
                this.state.formIsValid = false;
                errors["fecha_baja"] = <div className="alert alert-danger">Fecha no puede ser menor a Fecha Ingreso.</div>;
            }*/
            /*if (month != fecha.format('M')) {
                if (check.format('D') != last) {
                    this.state.formIsValid = false;
                    errors["fecha_baja"] = <div className="alert alert-danger">La Fecha de Baja debe estar dentro del mes actual</div>;
                } else if (fecha.format('D') > 5) {
                    this.state.formIsValid = false;
                    errors["fecha_baja"] = <div className="alert alert-danger">Debe ingresar una fecha hasta el 5 del próximo mes</div>;
                }

            }*/
        }
        if (!this.state.motivo_edicion) {
            this.state.formIsValid = false;
            errors["motivo_edicion"] = <div className="alert alert-danger">El campo Motivo de Edición es obligatorio.</div>;
        }
        if (!this.state.detalle_baja) {
            this.state.formIsValid = false;
            errors["detalle_baja"] = <div className="alert alert-danger">El campo Detalle de Baja es obligatorio.</div>;
        }


        this.setState({ errores: errors });
    }

    Guardar() {
        console.log('Guardar')
        this.handleValidation();
        if (this.state.formIsValid == false) {
            abp.notify.error('Se ha encontrado errores, por favor revisar el formulario', 'Error');
        }
        else {
            this.setState({ loading: true });
            axios.post("/RRHH/ColaboradorBaja/EditBaja/", {
                Id: this.state.baja_id,
                fecha_baja: this.state.fecha_baja,
                motivo_edicion: this.state.motivo_edicion,
                catalogo_motivo_baja_id: this.state.MotivoBajaId,
                detalle_baja: this.state.detalle_baja
            })
                .then((response) => {
                    this.setState({ loading: false });
                    abp.notify.success("Baja editada!", "Aviso");
                    this.clearStates();
                    this.props.GetColaboradores();
                })
                .catch((error) => {
                    this.setState({ loading: false });
                    console.log(error);
                    abp.notify.error("SAlgo salió mal.", 'Error');
                });

        }
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }




}