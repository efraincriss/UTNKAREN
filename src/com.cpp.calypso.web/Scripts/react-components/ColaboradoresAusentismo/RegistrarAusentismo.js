import React from 'react';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import moment from 'moment';
import BlockUi from 'react-block-ui';
import { ToggleButton } from 'primereact/components/togglebutton/ToggleButton';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import {
    AUSENTISMO_PATERNIDAD, AUSENTISMO_MATERNIDAD, GENERO_MUJER, GENERO_VARON,
    PARAMETRO_TIEMPO_PATERNIDAD
} from '../Colaboradores/Codigos';


export default class RegistrarAusentismo extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            colaboradoresAusentismo: [],
            tipo_identificacion_list: [],
            tipo_genero: [],
            tipo_grupo_personal: [],
            tipo_discapacidad: [],
            tipo_ausentismo: [],
            requisitos: [],
            colaborador: [],
            requisitos_cumplimiento: [],
            errores: [],
            archivos: [],
            tipo_identificacion: '',
            colaborador_id: 0,
            apellidos: '',
            nombres: '',
            telefono: '',
            genero: '',
            grupo_personal: '',
            inputKey: '',
            isDiscapacidad: false,
            discapacidad: '',
            ausentismo: '',
            fecha_desde: '',
            fecha_hasta: '',
            errores: [],
            nro_identificacion: '',
            formIsValid: true,
            loading: true,
            archivos: [],
            vista_discapacidad: 'none',
            cumple: [],
            req_table: [],
            table: [],
            errtable: [],
            es_nuevo: false,
            tableIsValid: '',
            requisitos_cumplimiento: [],
            ausentismos: [],
            ausentismo_id: 0,
            disable_tipoAusentismo: false,
            nombre_identificacion: '',
            nombre_genero: '',
            tiempo_paternidad: '',
            es_paternidad: false,
            observacion: ''
        }

        this.delete = this.delete.bind(this);
        this.dateFormat = this.dateFormat.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleCheck = this.handleCheck.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.changeAusentismo = this.changeAusentismo.bind(this);
        this.handleChangeFecha = this.handleChangeFecha.bind(this);
        this.validaPaternidad = this.validaPaternidad.bind(this);

        /* Guardar Ausentismo */
        this.guardarAusentismo = this.guardarAusentismo.bind(this);
        this.buscarColaborador = this.buscarColaborador.bind(this);
        this.getDatosTable = this.getDatosTable.bind(this);

        /* Archivos */
        this.onBasicUploadAuto = this.onBasicUploadAuto.bind(this);
        this.guardarArchivoRequisito = this.guardarArchivoRequisito.bind(this);

        /* Selects */
        this.getFormSelectTipoIdent = this.getFormSelectTipoIdent.bind(this);
        this.getFormSelectGenero = this.getFormSelectGenero.bind(this);
        this.getFormSelectGrupo = this.getFormSelectGrupo.bind(this);
        this.getFormSelectDiscapacidad = this.getFormSelectDiscapacidad.bind(this);
        this.getFormSelectAusentismo = this.getFormSelectAusentismo.bind(this);


        /* Mensajes al usuario */
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
    }

    componentWillReceiveProps(nextProps) {
        this.GetCatalogos();
    }

    componentDidMount() {
        this.GetCatalogos();
        this.buscarColaborador();
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <div>
                        <div className="row">

                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Tipo de Identificación:</b> {this.state.nombre_identificacion} </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>No. de Identificación:</b> {this.state.nro_identificacion} </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Apellidos Nombres:</b> {this.state.nombres} </label>
                                </div>
                            </div>

                        </div>
                        <div className="row">

                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Género:</b> {this.state.nombre_genero} </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Teléfono:</b> {this.state.telefono} </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Agrupación para Requisitos:</b> {this.state.nombre_grupo_personal}</label>
                                </div>
                            </div>

                        </div>
                        <div className="row">

                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label"><b>* Tipo de Ausentismo:</b> </label>
                                    <select value={this.state.ausentismo} style={{ width: '95%' }} onChange={this.changeAusentismo} className="form-control" id="ausentismo" name="ausentismo" disabled={this.state.disable_tipoAusentismo}>
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectAusentismo()}
                                    </select>
                                    <span style={{ color: "red" }}>{this.state.errores["ausentismo"]}</span>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label"><b>* Fecha Desde:</b> </label>
                                    <input type="date" id="fecha_desde" style={{ width: '95%' }} className="form-control" value={this.state.fecha_desde} onChange={this.handleChangeFecha} name="fecha_desde" />
                                    <span style={{ color: "red" }}>{this.state.errores["fecha_desde"]}</span>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label"><b>* Fecha Hasta:</b> </label>
                                    <input type="date" id="fecha_hasta" style={{ width: '95%' }} className="form-control" value={this.state.fecha_hasta} onChange={this.handleChange} name="fecha_hasta" />
                                    <span style={{ color: "red" }}>{this.state.errores["fecha_hasta"]}</span>
                                </div>
                            </div>

                        </div>
                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label"><b>Observación</b> </label>
                                    <input type="text" className="form-control" value={this.state.observacion} onChange={this.handleChange} name="observacion" />
                                </div>
                            </div>   </div> <br />
                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label" style={{ fontSize: '16px' }}><b>Requisitos para Ausentismo:</b> </label>
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-12">
                                <BootstrapTable
                                    id="requisitostable"
                                    data={this.state.req_table}
                                    hover={true}
                                    pagination={false}
                                    striped={false}
                                    condensed={true}
                                    options={options}
                                >
                                    <TableHeaderColumn
                                        dataField="any"
                                        dataFormat={this.Secuencial}
                                        width={"8%"}
                                        tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                        thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                                    >
                                        Nº
            </TableHeaderColumn>
                                    <TableHeaderColumn width={'10%'} dataField="codigo" isKey={true} tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Código</TableHeaderColumn>
                                    <TableHeaderColumn width={'20%'} dataField='nombre' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Nombre de Requisito</TableHeaderColumn>
                                    <TableHeaderColumn width={'18%'} dataField='responsable' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Responsable</TableHeaderColumn>
                                    <TableHeaderColumn width={'10%'} dataField='cumple' dataFormat={this.formatCumple.bind(this)} headerAlign="center" dataAlign="center">Cumple</TableHeaderColumn>
                                    <TableHeaderColumn dataField='archivo' tdStyle={{ whiteSpace: 'normal' }} dataFormat={this.formatArchivo.bind(this)} headerAlign="center">Archivo</TableHeaderColumn>
                                </BootstrapTable>
                            </div>
                        </div>
                        <hr />
                        <div className="row">
                            <div className="col">
                                <button type="button" onClick={() => this.guardarAusentismo()} className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                <button type="button" onClick={() => this.clearStates()} className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                            </div>
                        </div>


                    </div >
                </div>
            </BlockUi >
        )
    }

    Secuencial(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }

    formatCumple(cell, row) {
        var nombre = "selected" + row.Id;
        var id = row.codigo;

        if (row.obligatorio == true) {
            return (
                <div>
                    * <ToggleButton id={nombre} checked={this.state.table["selected" + (row.Id)]} onChange={(e) => this.handleInputTable(nombre, e)} onLabel="SI" offLabel="NO" disabled={row.disabled} />
                    <span style={{ color: "red", display: "inherit" }}>{this.state.errtable["cumple" + id]}</span>
                </div>
            )
        }
        else {
            return (
                <div>
                    <ToggleButton id={nombre} checked={this.state.table["selected" + (row.Id)]} onChange={(e) => this.handleInputTable(nombre, e)} onLabel="SI" offLabel="NO" disabled={row.disabled} />
                    <span style={{ color: "red", display: "inherit" }}>{this.state.errtable["cumple" + id]}</span>
                </div>
            )
        }

    }

    formatArchivo(cell, row) {
        var nombre = "archivo_" + row.Id;
        var id = row.codigo

        var req = this.state.req_table.filter(c => c.Id == row.Id);
        // console.log(nombre,req[0].cumple)
        if (row.archivo != null) {

            return (
                <div>
                    <input type="file" id={nombre} accept=".xls,.png,.jpg,.doc,.xlsx,.docx,.pdf" /*value={this.b64toBlob(this.state.table["archivo" + (row.Id)])}*/ onChange={(e) => this.onBasicUploadAuto(e, nombre)} key={''} disabled={row.disabled} />
                    <div style={{ marginTop: '10px' }}><a href="#" onClick={() => this.descargarFile(row.archivo.hash, row.archivo.tipo_contenido, row.archivo.nombre)}> {row.archivo.nombre}</a> </div>
                    <span style={{ color: "red", display: "inherit" }}>{this.state.errtable["archivo" + id]}</span>
                </div>
            )
        } else {
            return (
                <div>
                    <input type="file" id={nombre} accept=".xls,.png,.jpg,.doc,.xlsx,.docx,.pdf" onChange={(e) => this.onBasicUploadAuto(e, nombre)} key={''} disabled={row.disabled} />
                    <span style={{ color: "red", display: "inherit" }}>{this.state.errtable["archivo" + id]}</span>
                </div>
            )
        }

    }

    buscarColaborador() {
        axios.post("/RRHH/Colaboradores/GetDataColaboradorAusentismos/", {
            id: this.props.colaborador.Id
        })
            .then((response) => {
                console.log('datos_col', response.data);
                var apellido = response.data.segundo_apellido == null ? '' : response.data.segundo_apellido;
                this.setState({
                    colaborador_id: this.props.id,
                    nro_identificacion: response.data.numero_identificacion,
                    colaborador: response.data,
                    nombre_identificacion: response.data.nombre_identificacion,
                    nombres: response.data.nombres_apellidos,
                    genero: response.data.codigo_genero,
                    nombre_genero: response.data.nombre_genero,
                    nombre_grupo_personal: response.data.nombre_grupo_personal,
                    grupo_personal: response.data.catalogo_grupo_personal_id,
                    isDiscapacidad: response.data.discapacidad == null ? '' : response.data.discapacidad,
                    telefono: response.data.telefono == null ? '' : response.data.telefono,
                    discapacidad: response.data.tipo_discapacidad == null ? '' : response.data.tipo_discapacidad,
                    requisitos: response.data.requisitos,
                    ausentismos: response.data.ausentismos,
                    observacion: response.data.observacion,
                    loading: false
                })

                if (response.data.tipo_discapacidad != null) {
                    this.setState({ vista_discapacidad: 'block' })
                }

                /*if (response.data.ausentismos != null) {
                    this.setState({
                        ausentismo: response.data.ausentismos[0].catalogo_tipo_ausentismo_id,
                        ausentismo_id: response.data.ausentismos[0].Id,
                        fecha_desde: moment(response.data.ausentismos[0].fecha_inicio).format("YYYY-MM-DD"),
                        fecha_hasta: moment(response.data.ausentismos[0].fecha_fin).format("YYYY-MM-DD"),
                        cumple: response.data.ausentismos[0].requisitos,
                        //disable_tipoAusentismo: true,
                        disable_tipoAusentismo: false
                    });
                    this.arrayTabla(response.data.requisitos, response.data.ausentismos[0].catalogo_tipo_ausentismo_id)
                }*/
            })
            .catch((error) => {
                console.log(error);
            });
    }

    arrayTabla(requisitos, ausentismo) {
        // console.log('requisitos', requisitos)

        let array = [];
        let table = [];
        let cumple = null;
        let a = Object.assign({}, this.state.table);
        var i = 1;

        // console.log('cumple', this.state.cumple.length);

        if (this.state.cumple.length > 0) {
            this.setState({ es_nuevo: false });

            this.state.cumple.forEach(e => {
                let result = {};

                result.nro = i++;
                result.codigo = e.Requisitos.codigo;
                result.nombre = e.Requisitos.nombre;
                result.Id = e.Id;
                result.obligatorio = false;
                result.cumple = e.cumple;
                result.archivo = e.Archivo;
                a["selected" + (e.Id)] = e.cumple;
                if (e.Archivo != null) {
                    a["archivo_" + (e.Id)] = this.b64toBlob(e.Archivo);
                    // document.getElementById("archivo_"+e.Id).value = this.b64toBlob(e.Archivo);
                } else {
                    a["archivo_" + (e.Id)] = '';
                }

                result.responsable = e.Requisitos.Responsable.nombre;

                table.push(result);

            });
        } else if (requisitos != null) {
            this.setState({ es_nuevo: true });

            if (this.state.disable_tipoAusentismo == false) {
                requisitos.forEach(e => {
                    if (e.catalogo_tipo_ausentismo_id == ausentismo) {
                        array.push(e);
                    }
                });
            } else {
                this.warnMessage('Ya existe un ausentismo registrado!');
            }


            array.forEach(e => {
                let result = {};

                result.nro = e.nro;
                result.codigo = e.Requisitos.codigo;
                result.nombre = e.Requisitos.nombre;
                result.Id = e.Id;
                result.obligatorio = e.obligatorio;
                result.cumple = null;
                result.archivo = null;
                a["selected" + (e.Id)] = false;
                a["archivo_" + (e.Id)] = '';
                result.responsable = e.Requisitos.Responsable.nombre;
                table.push(result);

            });
        }

        // console.log('array', table);
        this.setState({ req_table: table, table: a });
    }

    b64toBlob(archivo) {
        // console.log('aa', archivo)
        var sliceSize = 512;
        var b64Data = archivo.hash;
        var contentType = archivo.tipo_contenido;

        var byteCharacters = atob(b64Data);
        var byteArrays = [];

        for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            var slice = byteCharacters.slice(offset, offset + sliceSize);

            var byteNumbers = new Array(slice.length);
            for (var i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }

            var byteArray = new Uint8Array(byteNumbers);

            byteArrays.push(byteArray);
        }

        var blob = new File(byteArrays, archivo.nombre, { type: contentType });
        return blob;
    }

    descargarFile(b64Data, contentType, filename) {

        // var c = this.b64toBlob(archivo.hash, archivo.tipo_contenido, archivo.nombre);

        var sliceSize = 512;

        var byteCharacters = atob(b64Data);
        var byteArrays = [];

        for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            var slice = byteCharacters.slice(offset, offset + sliceSize);

            var byteNumbers = new Array(slice.length);
            for (var i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }

            var byteArray = new Uint8Array(byteNumbers);

            byteArrays.push(byteArray);
        }

        var blob = new Blob(byteArrays, { type: contentType });
        //
        var url = URL.createObjectURL(blob);

        var element = document.createElement('a');
        element.setAttribute('href', url);
        element.setAttribute('download', filename);

        element.style.display = 'none';
        document.body.appendChild(element);

        element.click();

        document.body.removeChild(element);
    }

    clearStates() {

        this.setState({
            nro_identificacion: '',
            tipo_identificacion: '',
            nombres: '',
            apellidos: '',
            telefono: '',
            genero: '',
            grupo_personal: '',
            isDiscapacidad: false,
            discapacidad: '',
            colaborador_id: '',
            ausentismo: '',
            fecha_desde: '',
            fecha_hasta: '',
            loading: false,
            requisitos: [],
            requisitos_cumplimiento: [],
            archivos: [],
            colaboradoresAusentismo: [],
            tipo_identificacion_list: [],
            tipo_genero: [],
            tipo_grupo_personal: [],
            tipo_discapacidad: [],
            tipo_ausentismo: [],
            errores: [],
            colaborador: [],
            vista_discapacidad: 'none'
        }, this.props.onHide())
    }

    cargarCatalogos(data) {
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
            if (codigoCatalogo == 'TIPOINDENTIFICACION') {
                this.setState({ tipo_identificacion_list: catalogo })
                this.getFormSelectTipoIdent();
                return;
            }
            if (codigoCatalogo == 'GENERO') {
                this.setState({ tipo_genero: catalogo })
                this.getFormSelectGenero();
                return;
            }
            if (codigoCatalogo == 'GRUPOPERSONAL') {
                this.setState({ tipo_grupo_personal: catalogo })
                this.getFormSelectGrupo();
                return;
            }
            if (codigoCatalogo == 'TIPODISCAPACIDAD') {
                this.setState({ tipo_discapacidad: catalogo })
                this.getFormSelectDiscapacidad();
                return;
            }
            if (codigoCatalogo == 'TIPOAUSENTISMO') {
                this.setState({ tipo_ausentismo: catalogo })
                this.getFormSelectGrupo();
                return;
            }
        });
    }

    handleValidation() {
        let errors = {};
        var fecha = moment().format("YYYY-MM-DD");
        this.setState({ formIsValid: true });

        if (!this.state.ausentismo) {
            this.state.formIsValid = false;
            errors["ausentismo"] = "El campo Tipo de Ausentismo es obligatorio.";
        } else {
            var ausentismo = this.state.tipo_ausentismo.filter(c => c.Id == Number.parseInt(this.state.ausentismo));
            if (ausentismo.length > 0) {
                var codigo = ausentismo[0].codigo.replace(/ /g, "");
                if (codigo == AUSENTISMO_MATERNIDAD && this.state.genero != GENERO_MUJER) {
                    this.state.formIsValid = false;
                    errors["ausentismo"] = "El Tipo de Ausentismo no esta permitido.";
                } else if (codigo == AUSENTISMO_PATERNIDAD && this.state.genero != GENERO_VARON) {
                    this.state.formIsValid = false;
                    errors["ausentismo"] = "El Tipo de Ausentismo no esta permitido.";
                }
            }
            console.log(ausentismo);
        }

        if (!this.state.fecha_desde) {
            this.state.formIsValid = false;
            errors["fecha_desde"] = "El campo Fecha Desde es obligatorio.";
        }

        if (!this.state.fecha_hasta) {
            this.state.formIsValid = false;
            errors["fecha_hasta"] = "El campo Fecha Hasta es obligatorio.";
        }

        if ((new Date(this.state.fecha_desde).getTime() > new Date(this.state.fecha_hasta).getTime())) {
            this.state.formIsValid = false;
            errors["fecha_desde"] = "Fecha Desde no puede ser mayor que Fecha Hasta!";
        }

        if ((new Date(this.state.colaborador.fecha_ingreso).getTime() > new Date(this.state.fecha_desde).getTime())) {
            this.state.formIsValid = false;
            this.warnMessage("Las fechas ingresadas no pueden ser menores a la fecha de ingreso del  trabajador!");
        }
        this.setState({ errores: errors });
    }

    getFormSelectAusentismo() {
        return (
            this.state.tipo_ausentismo.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectDiscapacidad() {
        return (
            this.state.tipo_discapacidad.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectGrupo() {
        return (
            this.state.tipo_grupo_personal.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectGenero() {
        return (
            this.state.tipo_genero.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectTipoIdent() {
        return (
            this.state.tipo_identificacion_list.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }


    GetCatalogos() {

        var x = this.props.ColaboradorId;
        let codigos = [];

        codigos = ['TIPOINDENTIFICACION', 'GENERO', 'TIPODISCAPACIDAD', 'GRUPOPERSONAL', 'TIPOAUSENTISMO'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                this.cargarCatalogos(response.data);

            })
            .catch((error) => {
                console.log(error);
            });
    }


    getListado() {
        axios.post("/RRHH/ColaboradoresAusentismo/GetListado/", {})
            .then((response) => {
                this.setState({ colaboradoresAusentismo: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    validateTable() {
        let t = Object.assign({}, this.state.table);;
        let errors = [];
        this.state.tableIsValid = true;
        var modificar = false;


        // console.log('validateTable', this.state.table);

        if (this.state.es_nuevo == false) {
            this.state.cumple.forEach(e => {
                var req = this.state.requisitos.filter(c => c.RequisitosId == e.requisito_id);
                // console.log('req', req);

                var codigo = e.Requisitos.codigo;
                var cumple = t['selected' + e.Id]
                var archivo = t['archivo_' + e.Id]
                // console.log(req);

                //Valida si el requisito es obligatorio
                // console.log(req[0].obligatorio == true, cumple == true);
                if (req[0].obligatorio == true && cumple == false) {
                    this.state.tableIsValid = false;
                    errors["cumple" + codigo] = "Obligatorio";
                }

                //Valida si se requiere archivo
                if (req[0].requiere_archivo == true && archivo == "" && cumple == true) {
                    this.state.tableIsValid = false;
                    errors["archivo" + codigo] = "Obligatorio";
                }

            });
        } else {
            console.log('nuevo !')
            this.state.requisitos.forEach(e => {

                if (e.catalogo_tipo_ausentismo_id == this.state.ausentismo) {
                    var codigo = e.Requisitos.codigo;
                    var cumple = t['selected' + e.Id]
                    var archivo = t['archivo_' + e.Id]

                    //Valida si el requisito es obligatorio
                    // console.log(e.obligatorio, cumple, codigo);
                    if (e.obligatorio == true && cumple != true) {
                        this.state.tableIsValid = false;
                        errors["cumple" + codigo] = "Obligatorio";
                    }

                    //Valida si se requiere archivo
                    if (e.requiere_archivo == true && archivo == "" && cumple == true) {
                        this.state.tableIsValid = false;
                        errors["archivo" + codigo] = "Obligatorio";
                    }
                }



            });
        }


        this.setState({ errtable: errors });
    }

    getDatosTable() {
        let t = Object.assign({}, this.state.table);;
        // console.log('archivos', this.state.archivos);
        let req = [];


        if (this.state.es_nuevo == true) {
            this.state.requisitos.forEach(e => {
                if (e.catalogo_tipo_ausentismo_id == this.state.ausentismo) {

                    let result = {};

                    var cumple = t['selected' + e.Id]
                    var archivo = t['archivo_' + e.Id]


                    /* Objeto */
                    result.ColaboradoresId = this.state.colaborador_id;
                    result.RequisitosId = e.Requisitos.Id;
                    result.cumple = cumple;
                    result.observacion = '';
                    result.archivo_id = '';
                    if (archivo != "") {
                        result.archivo_usuario = archivo;
                    }
                    req.push(result)
                }



            });
        } else {
            this.state.cumple.forEach(e => {

                let result = {};

                var cumple = t['selected' + e.Id]
                var archivo = t['archivo_' + e.Id]

                /* Objeto */
                result.Id = e.Id;
                result.ColaboradoresId = this.state.colaborador_id;
                result.RequisitosId = e.requisito_id;
                result.cumple = cumple;
                result.observacion = '';
                result.archivo_id = e.archivo_id;
                if (archivo != "") {
                    result.archivo_usuario = archivo;
                }
                req.push(result)


            });
        }

        // console.log(req)

        this.state.requisitos_cumplimiento = req;

    }

    guardarAusentismo() {
        var Id = this.props.colaborador.Id
        console.log(Id);
        // this.getDatosTable();
        this.handleValidation();

        if (this.state.formIsValid == true) {
            if (Object.keys(this.state.table).length > 0) {
                this.validateTable();
                if (this.state.tableIsValid == true) {
                    console.log(this.state.colaborador_id);
                    this.setState({ loading: true });
                    this.getDatosTable();
                    console.log('requisitos_cumplimiento', this.state.requisitos_cumplimiento)

                    axios.post("/RRHH/ColaboradoresAusentismo/CrearAusentismo/",
                        {
                            idColaborador: this.props.colaborador.Id,
                            inTipoAusentismo: this.state.ausentismo,
                            fecha_desde: this.state.fecha_desde,
                            fecha_hasta: this.state.fecha_hasta,
                            idAusentismo: this.state.ausentismo_id,
                            observacion: this.state.observacion
                        })
                        .then((response) => {

                            if (response.data == "EXISTE") {
                                abp.notify.error(
                                    "Ya existe un ausentismo activo en las misma fechas",
                                    "ALERTA"
                                );
                                this.setState({ loading: false })

                            } else {


                                this.setState({ loading: false })
                                this.successMessage("Ausentismo Registrado!");
                                this.guardarArchivoRequisito(response.data);
                                setTimeout(
                                    function () {
                                        this.clearStates();
                                    }.bind(this), 2000
                                );
                            }


                        })
                        .catch((error) => {
                            console.log("ERROR", error);
                            this.setState({ loading: false })
                        });

                }
            } else {
                this.setState({ loading: true })
                axios.post("/RRHH/ColaboradoresAusentismo/CrearAusentismo/",
                    {
                        idColaborador: this.props.colaborador.Id,
                        inTipoAusentismo: this.state.ausentismo,
                        fecha_desde: this.state.fecha_desde,
                        fecha_hasta: this.state.fecha_hasta,
                        idAusentismo: this.state.ausentismo_id,
                        observacion: this.state.observacion
                    })
                    .then((response) => {
                        if (response.data == "EXISTE") {
                            abp.notify.error(
                                "Ya existe un ausentismo activo en las misma fechas",
                                "ALERTA"
                            );
                            this.setState({ loading: false })

                        } else {

                        this.setState({ loading: false })
                        this.successMessage("Ausentismo Registrado!");
                        setTimeout(
                            function () {
                                this.clearStates();
                            }.bind(this), 2000
                        );
                        // this.props.listado();

                        // this.clearStates();
                        }
                    })
                    .catch((error) => {
                        console.log("ERROR", error);
                        this.setState({ loading: false })
                    });
            }
        } else {
            this.warnMessage("Se ha encontrado errores, por favor revisar el formulario");
            this.setState({ loading: false })
        }
    }

    guardarArchivoRequisito(idAusentismo) {

        if (this.state.requisitos_cumplimiento != null) {

            this.state.requisitos_cumplimiento.forEach(e => {
                const formData = new FormData();
                var ruta = "";

                formData.append('Id', e.Id)
                formData.append('archivo_id', e.archivo_id)
                formData.append('idAusentismo', idAusentismo)
                formData.append('idRequisito', e.RequisitosId)
                formData.append('cumple', e.cumple)

                if (e.archivo_usuario != null) {
                    formData.append('UploadedFile', e.archivo_usuario)
                    ruta = "/RRHH/ColaboradoresAusentismo/CrearRequisitoAusentismo/";
                } else {
                    ruta = "/RRHH/ColaboradoresAusentismo/CrearRequisitoAusentismoNoFile/";
                }

                const config = { headers: { 'content-type': 'multipart/form-data' } }

                axios.post(ruta, formData, config)
                    .then((response) => {
                        console.log("DATA ", response.data);

                    })
                    .catch((error) => {
                        console.log("ERROR", error);
                    });

            });
        }
    }

    delete(id) {
        axios.post("/RRHH/ColaboradoresAusentismo/getListado/" + id, {})
            .then((response) => {
                console.log(response.data);
                if (response.data == "OK") {
                    this.setState({ colaboradoresAusentismo: response.data })
                }
            })
            .catch((error) => {
                console.log(error);
            });
    }

    dateFormat(cell, row) {
        if (cell === null) {
            return (
                "dd/mm/yy"
            )
        }
        return (
            moment(cell).format('DD-MM-YYYY')
        )
    }

    onBasicUploadAuto(event, nombre) {
        let obj = Object.assign({}, this.state.table);
        // console.log(event.target.files[0])
        var file = event.target.files[0];
        // console.log('type', file.type)
        if (file != null) {
            obj[nombre] = file;
            this.setState({ table: obj })

            if (file >= 2 * 1024 * 1024) {
                this.warnMessage("El archivo solo puede ser de máximo 2MB");
                return;
            } /*else if (!file.type.match('application/vnd') && !file.type.match('image/png') && !file.type.match('image/jpeg')) {
                this.warnMessage("No puede subir archivos de ese formato");
                return;
            } */else {
                var archivo = {};

                archivo.nombre = nombre;
                archivo.file = file;

                if (this.state.archivos.length != 0) {

                    this.state.archivos.forEach(function (item, index, object) {
                        if (item.nombre == archivo.name) {
                            object.splice(index, 1);
                            return true;
                        }
                    });

                    this.successMessage("Archivo Procesado con Exito");
                    this.state.archivos.push(archivo);

                } else {
                    this.successMessage("Archivo Procesado con Exito");
                    this.state.archivos.push(archivo);
                }

            }

        } else {
            console.log("error llamada");
        }
    }

    handleCheck() {
        this.setState({ isDiscapacidad: !this.state.isDiscapacidad });
    }

    changeAusentismo(event) {
        this.setState({ [event.target.name]: event.target.value });
        this.arrayTabla(this.state.requisitos, event.target.value);
        this.validaPaternidad(event.target.value);
    }

    validaPaternidad(id) {
        console.log('paternidad');
        var ausentismo = this.state.tipo_ausentismo.filter(c => c.Id == Number.parseInt(id));
        if (ausentismo.length > 0) {
            var codigo = ausentismo[0].codigo.replace(/ /g, "");
            if (codigo == AUSENTISMO_PATERNIDAD) {
                this.setState({ es_paternidad: true });
                axios.post("/RRHH/Colaboradores/GetParametroAPI/",
                    {
                        codigo: PARAMETRO_TIEMPO_PATERNIDAD,
                    })
                    .then((response) => {
                        console.log('parametro', response.data)
                        this.setState({ tiempo_paternidad: response.data });
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            } else {
                this.setState({ es_paternidad: false });
            }
        }
    }

    handleChangeFecha(event) {
        this.setState({ [event.target.name]: event.target.value });
        if (this.state.es_paternidad == true) {
            var dias = Number.parseInt(this.state.tiempo_paternidad) - 1;
            var add_days = moment(event.target.value, "YYYY-MM-DD").add(dias, 'days');
            this.state.fecha_hasta = moment(add_days).format("YYYY-MM-DD");
        }
    }

    handleChange(event) {

        this.setState({ [event.target.name]: event.target.value });
    }

    handleInputTable(nombre, event) {
        let obj = Object.assign({}, this.state.table);
        obj[nombre] = event.value;
        this.setState({
            table: obj
        });
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
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
}