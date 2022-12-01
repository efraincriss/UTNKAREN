import React from 'react';
import axios from 'axios';
import moment from 'moment';
import BlockUi from 'react-block-ui';
import {
    ENCARGADO_PERSONAL_JORNALES, ENCARGADO_PERSONAL_MENSUALES, ENCARGADO_PERSONAL_EXPATRIADOS,
    TIPO_NOMINA_JORNALES, TIPO_NOMINA_MENSUALES, TIPO_NOMINA_EXPATRIADOS,
    PERIODO_NOMINA_JORNALES, PERIODO_NOMINA_MENSUALES, VIA_PAGO_EFECTIVO, VIA_PAGO_CHEQUE
} from './Codigos';

export default class EditarNomina extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            empresa: 0,
            fecha_ingreso: '',
            posicion: '',
            division_personal: '',
            subdivision_personal: '',
            funcion: '',
            tipo_contrato: '',
            clase_contrato: '',
            caducidad_contrato: '',
            ejecutor_obra: '',
            tipo_nomina: '',
            periodo_nomina: '',
            proyecto: '',
            forma_pago: '',
            grupo: '',
            subgrupo: '',
            remuneracion: '',
            banco: '',
            tipo_cuenta: '',
            numero_cuenta: '',
            errores: [],
            formIsValid: '',
            tiposEmpresas: [],
            visibleSubGrupo: false,
            via_pago: '',
            loading: true,
            encargado_personal: '',
            disable_pago: false
        }

        this.handleChange = this.handleChange.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChangeFechaNac = this.handleChangeFechaNac.bind(this);
        this.saveEdad = this.saveEdad.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleChangeContrato = this.handleChangeContrato.bind(this);
        this.handleChangeNomina = this.handleChangeNomina.bind(this);
        this.handleChangeViaPago = this.handleChangeViaPago.bind(this);
        this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);
        this.handleChangeCategoria = this.handleChangeCategoria.bind(this);

        this.getFormSelectEmpresas = this.getFormSelectEmpresas.bind(this);
        this.GetEmpresas = this.GetEmpresas.bind(this);

        this.CargaDatosNomina = this.CargaDatosNomina.bind(this);
        this.ConfiguracionPago = this.ConfiguracionPago.bind(this);
    }

    componentDidMount() {
        this.GetEmpresas();
    }

    componentWillReceiveProps(nextProps) {
        // console.log('Nomina');
        this.setState({
            empresa: nextProps.colaborador.empresa_id==null ? 0 : nextProps.colaborador.empresa_id,
            posicion: nextProps.colaborador.posicion == null ? '' : nextProps.colaborador.posicion,
            division_personal: nextProps.colaborador.catalogo_division_personal_id == null ? '' : nextProps.colaborador.catalogo_division_personal_id,
            subdivision_personal: nextProps.colaborador.catalogo_subdivision_personal_id == null ? '' : nextProps.colaborador.catalogo_subdivision_personal_id,
            funcion: nextProps.colaborador.catalogo_funcion_id == null ? '' : nextProps.colaborador.catalogo_funcion_id,
            tipo_contrato: nextProps.colaborador.catalogo_tipo_contrato_id == null ? '' : nextProps.colaborador.catalogo_tipo_contrato_id,
            clase_contrato: nextProps.colaborador.catalogo_clase_contrato_id == null ? '' : nextProps.colaborador.catalogo_clase_contrato_id,
            caducidad_contrato: nextProps.colaborador.fecha_caducidad_contrato == null ? '' : moment(nextProps.colaborador.fecha_caducidad_contrato).format("YYYY-MM-DD"),
            ejecutor_obra: nextProps.colaborador.ejecutor_obra == null ? '' : nextProps.colaborador.ejecutor_obra,
            tipo_nomina: nextProps.colaborador.catalogo_tipo_nomina_id == null ? '' : nextProps.colaborador.catalogo_tipo_nomina_id,
            periodo_nomina: nextProps.colaborador.catalogo_periodo_nomina_id == null ? '' : nextProps.colaborador.catalogo_periodo_nomina_id,
            // proyecto: nextProps.colaborador.ContratoId == 0 ? '' : nextProps.colaborador.ContratoId,
            forma_pago: nextProps.colaborador.catalogo_forma_pago_id == null ? '' : nextProps.colaborador.catalogo_forma_pago_id,
            grupo: nextProps.colaborador.catalogo_grupo_id == null ? '' : nextProps.colaborador.catalogo_grupo_id,
            subgrupo: nextProps.colaborador.catalogo_subgrupo_id == null ? '' : nextProps.colaborador.catalogo_subgrupo_id,
            remuneracion: nextProps.colaborador.remuneracion_mensual == 0 ? '' : nextProps.colaborador.remuneracion_mensual,
            banco: nextProps.colaborador.catalogo_banco_id == null ? '' : nextProps.colaborador.catalogo_banco_id,
            tipo_cuenta: nextProps.colaborador.catalogo_tipo_cuenta_id == null ? '' : nextProps.colaborador.catalogo_tipo_cuenta_id,
            numero_cuenta: nextProps.colaborador.numero_cuenta == null ? '' : nextProps.colaborador.numero_cuenta,
            via_pago: nextProps.colaborador.catalogo_via_pago_id == null ? '' : nextProps.colaborador.catalogo_via_pago_id,
            encargado_personal: nextProps.colaborador.catalogo_encargado_personal_id,
            loading: false
        })
        // console.log('nomina', nextProps.colaborador.catalogo_tipo_nomina_id, nextProps.colaborador.catalogo_periodo_nomina_id)
        if (nextProps.colaborador.catalogo_tipo_nomina_id == null && nextProps.colaborador.catalogo_periodo_nomina_id == null) {
            this.CargaDatosNomina(nextProps.colaborador.catalogo_encargado_personal_id);
        }
        if (nextProps.colaborador.catalogo_via_pago_id != null) {
            this.ConfiguracionPago(nextProps.colaborador.catalogo_via_pago_id);
        }

    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <div className="row">
                        <div className="col-xs-12 col-md-12">
                            <form onSubmit={this.handleSubmit}>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="empresa">* Empresa: </label>
                                            <select value={this.state.empresa} onChange={this.handleChange} className="form-control" name="empresa">
                                                <option value="">Seleccione...</option>
                                                {this.getFormSelectEmpresas()}
                                            </select>
                                            {this.state.errores["empresa"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="fecha_ingreso">* Fecha de Ingreso: </label>
                                            <input type="date" id="fecha_ingreso" className="form-control" value={moment(this.props.colaborador.fecha_ingreso).format("YYYY-MM-DD")} onChange={this.handleChangeFechaNac} name="fecha_ingreso" disabled />
                                            {this.state.errores["fecha_ingreso"]}
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="posicion">* Posición: </label>
                                            <input type="text" id="posicion" className="form-control" value={this.state.posicion} onChange={this.handleChangeUpperCase} name="posicion" />
                                            {this.state.errores["posicion"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="division_personal">* División Personal: </label>
                                            <select value={this.state.division_personal} onChange={this.handleChange} className="form-control" name="division_personal">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectDivisionPersonal()}
                                            </select>
                                            {this.state.errores["division_personal"]}
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="subdivision_personal">* Subdivisión Personal (para donde va): </label>
                                            <select value={this.state.subdivision_personal} onChange={this.handleChange} className="form-control" name="subdivision_personal">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectSubDivisionP()}
                                            </select>
                                            {this.state.errores["subdivision_personal"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="funcion">* Función: </label>
                                            <select value={this.state.funcion} onChange={this.handleChange} className="form-control" name="funcion">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectFuncion()}
                                            </select>
                                            {this.state.errores["funcion"]}
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="tipo_contrato">* Tipo de Contrato: </label>
                                            <select value={this.state.tipo_contrato} onChange={this.handleChangeContrato} className="form-control" name="tipo_contrato">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectTipoContrato()}
                                            </select>
                                            {this.state.errores["tipo_contrato"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="clase_contrato">* Clase de Contrato: </label>
                                            <select value={this.state.clase_contrato} onChange={this.handleChange} className="form-control" name="clase_contrato">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectClaseContrato()}
                                            </select>
                                            {this.state.errores["clase_contrato"]}
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="caducidad_contrato">* Caducidad Contrato: </label>
                                            <input type="date" id="caducidad_contrato" className="form-control" value={this.state.caducidad_contrato} onChange={this.handleChangeFechaNac} name="caducidad_contrato" />
                                            {this.state.errores["caducidad_contrato"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '32px' }}>
                                            <label htmlFor="ejecutor_obra" style={{ width: '285px' }}>Es Ejecutor de Obra?: </label>
                                            <input type="checkbox" id="ejecutor_obra" className="form-control" checked={this.state.ejecutor_obra} onChange={this.handleInputChange} name="ejecutor_obra" style={{ marginTop: '5px', marginLeft: '-30%' }} />
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="tipo_nomina">* Tipo de Nómina: </label>
                                            <select value={this.state.tipo_nomina} onChange={this.handleChangeNomina} className="form-control" name="tipo_nomina">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectTipoNomina()}
                                            </select>
                                            {this.state.errores["tipo_nomina"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="periodo_nomina">* Período de Nómina: </label>
                                            <select value={this.state.periodo_nomina} onChange={this.handleChange} className="form-control" name="periodo_nomina">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectPeriodoNomina()}
                                            </select>
                                            {this.state.errores["periodo_nomina"]}
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="proyecto">* Proyecto: </label>
                                            <select value={this.props.colaborador.ContratoId} onChange={this.handleChange} className="form-control" name="proyecto" disabled>
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectProyecto()}
                                            </select>
                                            {this.state.errores["proyecto"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="forma_pago">* Forma de Pago: </label>
                                            <select value={this.state.forma_pago} onChange={this.handleChange} className="form-control" name="forma_pago">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectFormaPago()}
                                            </select>
                                            {this.state.errores["forma_pago"]}
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="grupo">Grupo (Categoría O PC): </label>
                                            <select value={this.state.grupo} onChange={this.handleChangeCategoria} className="form-control" name="grupo">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectGrupo()}
                                            </select>
                                            {this.state.errores["grupo"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="subgrupo">Sub Grupo (Cuartil): </label>
                                            <select value={this.state.subgrupo} onChange={this.handleChange} className="form-control" name="subgrupo" disabled={this.state.visibleSubGrupo}>
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectSubGrupo()}
                                            </select>
                                            {this.state.errores["subgrupo"]}
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="remuneracion">* Remuneración Mensual:</label>
                                            <input type="number" id="remuneracion" className="form-control currency" value={this.state.remuneracion} onChange={this.handleChange} name="remuneracion"
                                                min="0" step="0.01" data-number-to-fixed="2" data-number-stepfactor="100" />
                                            {this.state.errores["remuneracion"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                    </div>
                                </div>
                                <h5><b>Información Bancaria:</b></h5>
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="via_pago">* Vía de Pago: </label>
                                            <select value={this.state.via_pago} onChange={this.handleChangeViaPago} className="form-control" name="via_pago">
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectViaPago()}
                                            </select>
                                            {this.state.errores["via_pago"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="banco">* Banco: </label>
                                            <select value={this.state.banco} onChange={this.handleChange} className="form-control" name="banco" disabled={this.state.disable_pago}>
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectBancos()}
                                            </select>
                                            {this.state.errores["banco"]}
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="tipo_cuenta">* Tipo de Cuenta: </label>
                                            <select value={this.state.tipo_cuenta} onChange={this.handleChange} className="form-control" name="tipo_cuenta" disabled={this.state.disable_pago}>
                                                <option value="">Seleccione...</option>
                                                {this.props.getFormSelectTipoCuenta()}
                                            </select>
                                            {this.state.errores["tipo_cuenta"]}
                                        </div>
                                    </div>
                                    <div className="col">
                                        <div className="form-group">
                                            <label htmlFor="numero_cuenta">* Número de Cuenta:</label>
                                            <input type="text" id="numero_cuenta" className="form-control" value={this.state.numero_cuenta} onChange={this.handleChange} name="numero_cuenta" disabled={this.state.disable_pago} />
                                            {this.state.errores["numero_cuenta"]}
                                        </div>
                                    </div>
                                </div>

                                <br />
                                <div className="form-group">
                                    <div className="col">
                                        <button type="submit" className="btn btn-outline-primary fa fa-save" disabled={this.props.disable_button}> Guardar</button>
                                        <button onClick={() => this.props.regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                                    </div>
                                </div>

                            </form>
                        </div>
                    </div>
                </div >
            </BlockUi>
        )
    }

    handleValidation() {
        console.log('handleValidation');
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.empresa) {
            this.state.formIsValid = false;
            errors["empresa"] = <div className="alert alert-danger">El campo Empresa es obligatorio.</div>;
        }
        if (!this.state.posicion) {
            this.state.formIsValid = false;
            errors["posicion"] = <div className="alert alert-danger">El campo Posición es obligatorio.</div>;
        }
        if (!this.state.division_personal) {
            this.state.formIsValid = false;
            errors["division_personal"] = <div className="alert alert-danger">El campo División Personal es obligatorio.</div>;
        }
        if (!this.state.subdivision_personal) {
            this.state.formIsValid = false;
            errors["subdivision_personal"] = <div className="alert alert-danger">El campo Subdivisión Personal es obligatorio.</div>;
        }
        if (!this.state.funcion) {
            this.state.formIsValid = false;
            errors["funcion"] = <div className="alert alert-danger">El campo Función es obligatorio.</div>;
        }
        if (!this.state.tipo_contrato) {
            this.state.formIsValid = false;
            errors["tipo_contrato"] = <div className="alert alert-danger">El campo Tipo de Contrato es obligatorio.</div>;
        }
        if (!this.state.clase_contrato) {
            this.state.formIsValid = false;
            errors["clase_contrato"] = <div className="alert alert-danger">El campo Clase de Contrato es obligatorio.</div>;
        }
        if (!this.state.caducidad_contrato) {
            this.state.formIsValid = false;
            errors["caducidad_contrato"] = <div className="alert alert-danger">El campo Caducidad Contrato es obligatorio.</div>;
        } else {
            if (this.state.caducidad_contrato > this.props.fecha_ingreso) {
                this.state.formIsValid = false;
                errors["caducidad_contrato"] = <div className="alert alert-danger">No se permite el registro de una fecha menos a la Fecha de Ingreso.</div>;
            }
        }
        if (!this.state.tipo_nomina) {
            this.state.formIsValid = false;
            errors["tipo_nomina"] = <div className="alert alert-danger">El campo Tipo de Nómina es obligatorio.</div>;
        }
        if (!this.state.periodo_nomina) {
            this.state.formIsValid = false;
            errors["periodo_nomina"] = <div className="alert alert-danger">El campo Período de Nómina es obligatorio.</div>;
        }
        if (!this.state.forma_pago) {
            this.state.formIsValid = false;
            errors["forma_pago"] = <div className="alert alert-danger">El campo Forma de Pago es obligatorio.</div>;
        }
        // if (!this.state.grupo) {
        //     this.state.formIsValid = false;
        //     errors["grupo"] = <div className="alert alert-danger">El campo Grupo es obligatorio.</div>;
        // }
        // if (!this.state.subgrupo) {
        //     this.state.formIsValid = false;
        //     errors["subgrupo"] = <div className="alert alert-danger">El campo Sub Grupo es obligatorio.</div>;
        // }
        if (!this.state.remuneracion) {
            this.state.formIsValid = false;
            errors["remuneracion"] = <div className="alert alert-danger">El campo Remuneración Mensual es obligatorio.</div>;
        } else {
            if (!isFinite(this.state.remuneracion)) {
                this.state.formIsValid = false;
                errors["remuneracion"] = <div className="alert alert-danger">El campo permite solo ingreso numérico</div>;
            }
        }

        if (!this.state.forma_pago) {
            this.state.formIsValid = false;
            errors["forma_pago"] = <div className="alert alert-danger">El campo Forma de Pago es obligatorio.</div>;
        }

        if (!this.state.via_pago) {
            this.state.formIsValid = false;
            errors["via_pago"] = <div className="alert alert-danger">El campo Vía de Pago es obligatorio.</div>;
        } else {
            var tipos = this.props.tiposViaPago.filter(c => c.Id == Number.parseInt(this.state.via_pago));
            if (tipos.length > 0) {
                var codigo = tipos[0].codigo.replace(/ /g, "");
                if (codigo == VIA_PAGO_CHEQUE || codigo == VIA_PAGO_EFECTIVO) { }
                else {
                    if (!this.state.numero_cuenta) {
                        this.state.formIsValid = false;
                        errors["numero_cuenta"] = <div className="alert alert-danger">El campo Número de Cuenta es obligatorio.</div>;
                    } else {
                        if (!isFinite(this.state.numero_cuenta)) {
                            this.state.formIsValid = false;
                            errors["numero_cuenta"] = <div className="alert alert-danger">El campo permite solo ingreso numérico</div>;
                        }
                        if (this.state.numero_cuenta.length > 50) {
                            this.state.formIsValid = false;
                            errors["numero_cuenta"] = <div className="alert alert-danger">El campo no puede tener más de 50 dígitos</div>;
                        }
                    }
                    if (!this.state.tipo_cuenta) {
                        this.state.formIsValid = false;
                        errors["tipo_cuenta"] = <div className="alert alert-danger">El campo Tipo de Cuenta es obligatorio.</div>;
                    }
                    if (!this.state.banco) {
                        this.state.formIsValid = false;
                        errors["banco"] = <div className="alert alert-danger">El campo Banco es obligatorio.</div>;
                    }
                }
            }
        }

        this.setState({ errores: errors });
        return this.state.formIsValid;
    }

    handleSubmit(event) {
        event.preventDefault();
        this.handleValidation();

        if (this.state.formIsValid) {
            this.setState({ loading: true });
            this.props.colaborador.Id = this.props.id_colaborador;
            this.props.colaborador.empresa_id = this.state.empresa;
            this.props.colaborador.posicion = this.state.posicion;
            this.props.colaborador.catalogo_division_personal_id = this.state.division_personal;
            this.props.colaborador.catalogo_subdivision_personal_id = this.state.subdivision_personal;
            this.props.colaborador.catalogo_funcion_id = this.state.funcion;
            this.props.colaborador.catalogo_tipo_contrato_id = this.state.tipo_contrato;
            this.props.colaborador.catalogo_clase_contrato_id = this.state.clase_contrato;
            this.props.colaborador.fecha_caducidad_contrato = this.state.caducidad_contrato;
            this.props.colaborador.ejecutor_obra = this.state.ejecutor_obra;
            this.props.colaborador.catalogo_tipo_nomina_id = this.state.tipo_nomina;
            this.props.colaborador.catalogo_periodo_nomina_id = this.state.periodo_nomina;
            // this.props.colaborador.ContratoId = this.state.proyecto;
            this.props.colaborador.catalogo_forma_pago_id = this.state.forma_pago;
            this.props.colaborador.catalogo_grupo_id = this.state.grupo;
            this.props.colaborador.catalogo_subgrupo_id = this.state.subgrupo;
            this.props.colaborador.remuneracion_mensual = this.state.remuneracion;
            this.props.colaborador.catalogo_banco_id = this.state.banco;
            this.props.colaborador.catalogo_tipo_cuenta_id = this.state.tipo_cuenta;
            this.props.colaborador.numero_cuenta = this.state.numero_cuenta;
            this.props.colaborador.catalogo_via_pago_id = this.state.via_pago;

            axios.post("/RRHH/Colaboradores/CreateEmpleoApi/", {
                colaborador: this.props.colaborador
            })
                .then((response) => {
                    this.setState({ loading: false });
                    if (response.data == 'OK') {
                        abp.notify.success("Nómina Guardada!", "Aviso");
                    } else {
                        abp.notify.error(response.data, 'Error');
                    }
                    console.log('nomina', response.data);
                })
                .catch((error) => {
                    this.setState({ loading: false });
                    console.log(error);
                    abp.notify.error('Algo salió mal.', 'Error');
                });
        } else {
            abp.notify.error('Se ha encontrado errores, por favor revisar el formulario', 'Error');
        }

    }

    CargaDatosNomina(id) {
        var encargado = this.props.tiposEncargadosPersonal.filter(c => c.Id == Number.parseInt(id));
        // console.log('encargado', encargado)
        if (encargado.length > 0) {
            switch (encargado[0].codigo.replace(/ /g, "")) {
                case ENCARGADO_PERSONAL_JORNALES:
                    var tipo = this.props.tiposNomina.filter(c => c.codigo.replace(/ /g, "") == TIPO_NOMINA_JORNALES);
                    var periodo = this.props.tiposPeriodoNomina.filter(c => c.codigo.replace(/ /g, "") == PERIODO_NOMINA_JORNALES);
                    if (tipo.length > 0 && periodo.length > 0) {
                        this.setState({ tipo_nomina: tipo[0].Id, periodo_nomina: periodo[0].Id })
                        this.state.visibleSubGrupo = true;
                    }
                    return;
                case ENCARGADO_PERSONAL_MENSUALES:
                    var tipo = this.props.tiposNomina.filter(c => c.codigo.replace(/ /g, "") == TIPO_NOMINA_MENSUALES);
                    var periodo = this.props.tiposPeriodoNomina.filter(c => c.codigo.replace(/ /g, "") == PERIODO_NOMINA_MENSUALES);
                    if (tipo.length > 0 && periodo.length > 0) {
                        this.setState({ tipo_nomina: tipo[0].Id, periodo_nomina: periodo[0].Id })
                        this.state.visibleSubGrupo = false;
                    }
                    return;
                case ENCARGADO_PERSONAL_EXPATRIADOS:
                    var tipo = this.props.tiposNomina.filter(c => c.codigo.replace(/ /g, "") == TIPO_NOMINA_MENSUALES);
                    var periodo = this.props.tiposPeriodoNomina.filter(c => c.codigo.replace(/ /g, "") == PERIODO_NOMINA_MENSUALES);
                    if (tipo.length > 0 && periodo.length > 0) {
                        this.setState({ tipo_nomina: tipo[0].Id, periodo_nomina: periodo[0].Id })
                        this.state.visibleSubGrupo = false;
                    }
                    return;
            }
        }
    }

    getFormSelectEmpresas() {
        return (
            this.state.tiposEmpresas.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.razon_social}</option>
                )
            })
        );
    }

    GetEmpresas() {
        axios.post("/RRHH/Colaboradores/GetEmpresasApi/", {})
        .then((response) => {
            console.log(response.data)
            this.setState({ tiposEmpresas: response.data });
            this.getFormSelectEmpresas();
        })
        .catch((error) => {
            console.log(error);
        });
    }

    handleChangeFechaNac(event) {
        this.setState({ [event.target.name]: event.target.value }, this.saveEdad);
    }

    saveEdad() {
        var age = moment().diff(this.state.fecha_nacimiento, 'years');
        if (!age) {
            age = '';
        }
        this.setState({ edad: age })
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeViaPago(event) {
        this.setState({ [event.target.name]: event.target.value });
        if (event.target.value != '') {
            this.ConfiguracionPago(event.target.value);
        }

    }

    ConfiguracionPago(value) {
        var tipos = this.props.tiposViaPago.filter(c => c.Id == Number.parseInt(value));
        if (tipos.length > 0) {
            var codigo = tipos[0].codigo.replace(/ /g, "");
            if (codigo == VIA_PAGO_CHEQUE || codigo == VIA_PAGO_EFECTIVO) {
                this.setState({
                    disable_pago: true,
                    banco: '',
                    tipo_cuenta: '',
                    numero_cuenta: ''
                });
            } else {
                this.setState({ disable_pago: false });
            }
        }
    }

    handleChangeContrato(event) {
        this.setState({ [event.target.name]: event.target.value });
        var catalogo = this.props.tiposContrato.filter(c => c.Id == Number.parseInt(event.target.value));
        if (catalogo[0].codigo == '01') { // Verificar si tipo contrato en Indeterminado con período de prueba
            var today = moment().format("YYYY-MM-DD");
            var add_days = moment(this.props.colaborador.fecha_ingreso, "YYYY-MM-DD").add(89, 'days');

            this.state.caducidad_contrato = moment(add_days).format("YYYY-MM-DD");
            if (today == add_days) {
                this.state.caducidad_contrato = moment('2044-12-31').format("YYYY-MM-DD");
            }
        }
        else if (catalogo[0].codigo == '02') { // Verificar si tipo contrato en Indeterminado sin período de prueba

            this.state.caducidad_contrato = moment('2044-12-31').format("YYYY-MM-DD");
        } else { // Verificar si es otro tipo de contrato
            this.state.caducidad_contrato = '';
        }

    }

    handleChangeCategoria(event) {
        this.setState({ [event.target.name]: event.target.value });
        if (event.target.value != '') {
            var encargado = this.props.tiposEncargadosPersonal.filter(c => c.Id == Number.parseInt(this.state.encargado_personal));
            if (encargado.length > 0) {
                if (encargado[0].codigo.replace(/ /g, "") == ENCARGADO_PERSONAL_JORNALES) {
                    var categoria = this.props.tiposCategorias.filter(c => c.CategoriaId == Number.parseInt(event.target.value));
                    if (categoria.length > 0) {
                        this.setState({ remuneracion: categoria[0].Categoria.valor_numerico });
                    }
                }
            }
        }

    }

    handleChangeNomina(event) {
        this.setState({ [event.target.name]: event.target.value });
        if (event.target.value != '') {
            var catalogo = this.props.tiposNomina.filter(c => c.Id == Number.parseInt(event.target.value));
            if (catalogo[0].codigo.replace(/ /g, "") == TIPO_NOMINA_MENSUALES || catalogo[0].codigo == TIPO_NOMINA_EXPATRIADOS) {
                this.state.visibleSubGrupo = false;
            } else {
                this.state.visibleSubGrupo = true;
            }
        }

    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

    handleChangeUpperCase(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }
}