import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import moment, { now } from 'moment';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Growl } from 'primereact/components/growl/Growl';
import { ToggleButton } from 'primereact/components/togglebutton/ToggleButton';
import BlockUi from 'react-block-ui';

export default class CrearCumplimientoRequisitos extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            colaborador_id: '',
            tipo_identificacion: '',
            nro_identificacion: '',
            nombres: '',
            telefono: '',
            genero: '',
            nacionalidad: '',
            discapacidad: '',
            tipo_discapacidad: '',
            tiposIdentificacion: [],
            tipoDiscapacidad: [],
            gruposPersonal: [],
            generos: [],
            tiposNacionalidades: [],
            requisitos: [],
            requisitos_cumplimiento: [],
            archivos: [],
            errores: [],
            errtable: [],
            formIsValid: '',
            tableIsValid: '',
            loading: true,
            req_table: [],
            table: {},
            es_nuevo: false,
            responsabilidades: [],
            tiposRoles: [],
            tiposAusentismo: [],
            accion: [],
            tipo_ausentismo: [],
            es_externo: '',
            nombre_grupo_personal: '',
            grupo_personal: '',
            accion_id: 0,
        }

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChangeAccion = this.handleChangeAccion.bind(this);
        this.validarCedula = this.validarCedula.bind(this);

        this.getFormSelectTipoIdent = this.getFormSelectTipoIdent.bind(this);
        this.getFormSelectTipoDiscapacidad = this.getFormSelectTipoDiscapacidad.bind(this);
        this.getFormSelectGrupoPersonal = this.getFormSelectGrupoPersonal.bind(this);
        this.getFormSelectGenero = this.getFormSelectGenero.bind(this);
        this.getFormSelectNacionalidades = this.getFormSelectNacionalidades.bind(this);
        this.getFormSelectAccion = this.getFormSelectAccion.bind(this);
        this.getFormSelectAusentismo = this.getFormSelectAusentismo.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);

        this.consultarDatosColaborador = this.consultarDatosColaborador.bind(this);

        this.getDatosTable = this.getDatosTable.bind(this);
        this.validateTable = this.validateTable.bind(this);

        this.onBasicUploadAuto = this.onBasicUploadAuto.bind(this);
        this.GetData = this.GetData.bind(this);

        this.arrayTabla = this.arrayTabla.bind(this);
        this.descargarFile = this.descargarFile.bind(this);

        this.consultarUser = this.consultarUser.bind(this);
    }

    componentDidMount() {
        this.GetCatalogos();
        this.consultarDatosColaborador();
    }

    consultarUser() {
        axios.post("/RRHH/ColaboradorResponsabilidad/GetListaApi/", {})
            .then((response) => {
                console.log('User', response.data);
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
                abp.notify.error("Algo salió mal.", 'Error');
            });
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <form onSubmit={this.handleSubmit}>
                        <div className="row">
                            <div className="col-1"></div>
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
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Apellidos Nombres:</b> {this.state.nombres} </label>
                                </div>
                            </div>
                            <div className="col-1"></div>
                        </div>
                        <div className="row">
                            <div className="col-1"></div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="text"><b>Género:</b> {this.state.genero} </label>
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
                            <div className="col-1"></div>
                        </div>
              {/*
                        <div className="row">
                            <div className="col-1"></div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="accion">* Acción: </label>
                                    <select value={this.state.accion} onChange={this.handleChangeAccion} className="form-control" name="accion">
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectAccion()}
                                    </select>
                                    <span style={{ color: "red" }}>{this.state.errores["accion"]}</span>
                                </div>
                            </div>
                             
                            <div className="col"></div>
                            <div className="col"></div>
                            <div className="col-1"></div>
                        </div>
                    */}
                        <br />
                        <h5>Requisitos del Colaborador</h5>
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
              width={"5%"}
              tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
              thStyle={{ whiteSpace: "normal", textAlign: "center" }}
            >
              Nº
            </TableHeaderColumn>
                                    {/* <TableHeaderColumn width={'6%'} dataField="nro" isKey={true} dataAlign="center" headerAlign="center" dataSort={true}>No.</TableHeaderColumn> */}
                                    <TableHeaderColumn width={'7%'} dataField="codigo" isKey={true} tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Código</TableHeaderColumn>
                                    <TableHeaderColumn width={'8%'} dataField="nombre_accion" tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataAlign="center" dataSort={true}>Acción</TableHeaderColumn>
                                    <TableHeaderColumn width={'12%'} dataField='nombre' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Nombre de Requisito</TableHeaderColumn>
                                    <TableHeaderColumn width={'11%'} dataField='responsable' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} filter={{ type: 'TextFilter', delay: 500 }} headerAlign="center" dataSort={true}>Responsable</TableHeaderColumn>
                                    <TableHeaderColumn width={'9%'} dataField='cumple' dataFormat={this.formatCumple.bind(this)} headerAlign="center" dataAlign="center">Cumple</TableHeaderColumn>
                                    <TableHeaderColumn dataField='archivo' tdStyle={{ whiteSpace: 'normal' }} dataFormat={this.formatArchivo.bind(this)} headerAlign="center">Archivo</TableHeaderColumn>
                                    <TableHeaderColumn width={'15%'} dataField='fecha_emision' dataFormat={this.formatFechaEmision.bind(this)} headerAlign="center">Fecha Emisión</TableHeaderColumn>
                                    <TableHeaderColumn width={'15%'} dataField='fecha_caducidad' dataFormat={this.formatFechaCaducidad.bind(this)} headerAlign="center">Fecha Caducidad</TableHeaderColumn>
                                </BootstrapTable>
                                <br />
                                <div className="form-group">
                                    <div className="col">
                                        <button type="submit" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                        <button onClick={() => this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </form>
                </div >
            </BlockUi>
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
                abp.notify.error("El archivo solo puede ser de máximo 2MB", 'Error');
                return;
            } else if (!file.type.match('application/vnd') && !file.type.match('image/png') && !file.type.match('image/jpeg')) {
                abp.notify.error("No puede subir archivos de ese formato", 'Error');
                return;
            } else {
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

                    abp.notify.success("Archivo Procesado con Exito", "Aviso");
                    this.state.archivos.push(archivo);

                } else {
                    abp.notify.success("Archivo Procesado con Exito", "Aviso");
                    this.state.archivos.push(archivo);
                }

            }

        } else {
            console.log("error llamada");
        }
    }

    GetData(event, nombre) {
        var a = {};
        if (event.files[0] != null) {

            const formData = new FormData();
            // const formData = {};
            // formData.append('UploadedFile', event.files[0])
            formData['UploadedFile'] = event.files[0];
            //this.setState({blocking: true})
            const config = {
                headers: {
                    'content-type': 'multipart/form-data'
                }
            }
            a.nombre = nombre;
            a.file = formData;
            a.config = config;

            this.state.archivos.push(a);
            console.log("formData", formData);

        } else {

            console.log("error llamada");
        }
        console.log(this.state.archivos);
    }

    formatCodigo(cell, row) {
        return row.Requisitos.codigo;
    }

    formatNombre(cell, row) {
        return row.Requisitos.nombre;
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
                    <input type="file" id="file_reintegro" accept=".xls,.png,.jpg,.doc,.xlsx,.docx" /*value={this.b64toBlob(this.state.table["archivo" + (row.Id)])}*/ onChange={(e) => this.onBasicUploadAuto(e, nombre)} key={''} disabled={row.disabled} />
                    <div style={{ marginTop: '10px' }}><a href="#" onClick={() => this.descargarFile(row.archivo.hash, row.archivo.tipo_contenido, row.archivo.nombre)}> {row.archivo.nombre}</a> </div>
                    <span style={{ color: "red", display: "inherit" }}>{this.state.errtable["archivo" + id]}</span>
                </div>
            )
        } else {
            return (
                <div>
                    <input type="file" id="file_reintegro" accept=".xls,.png,.jpg,.doc,.xlsx,.docx" onChange={(e) => this.onBasicUploadAuto(e, nombre)} key={''} disabled={row.disabled} />
                    <span style={{ color: "red", display: "inherit" }}>{this.state.errtable["archivo" + id]}</span>
                </div>
            )
        }

    }

    formatFechaEmision(cell, row) {
        var nombre = "emision" + row.Id;
        var id = row.codigo;

        return (
            <div className="form-group">
                <input type="date" id={nombre} className="form-control" value={this.state.table["emision" + (row.Id)]} onChange={(e) => this.handleTable(nombre, e)} name={nombre} disabled={row.disabled} />
                <span style={{ color: "red", display: "inline-block" }}>{this.state.errtable["emision" + id]}</span>
            </div>
        )
    }

    handleTable(nombre, value) {
        let obj = Object.assign({}, this.state.table);
        obj[nombre] = value.target.value;
        this.setState({ table: obj })
    }

    handleInputTable(nombre, event) {
        let obj = Object.assign({}, this.state.table);
        obj[nombre] = event.value;
        this.setState({
            table: obj
        });
    }

    Secuencial(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
      }

    formatFechaCaducidad(cell, row) {
        var nombre = "fecha" + row.Id;
        var id = row.codigo;
        if (row.fecha_caducidad != null) {
            this.state["fecha" + (row.Id)] = moment(row.fecha_caducidad).format("YYYY-MM-DD");
        }
        return (
            <div className="form-group">
                <input type="date" id={nombre} className="form-control" value={this.state.table["fecha" + (row.Id)]} onChange={(e) => this.handleTable(nombre, e)} name={nombre} disabled={row.disabled} />
                <span style={{ color: "red", display: "inline-block" }}>{this.state.errtable["fecha" + id]}</span>
            </div>
        )

    }

    validateTable() {
        let t = Object.assign({}, this.state.table);;
        let errors = [];
        this.state.tableIsValid = true;
        var modificar = false;

        this.state.responsabilidades.forEach(e => {
            if (e.acceso == "M") {
                modificar = true;
                console.log('modificar', modificar);
            }
        });



        console.log('validateTable', this.state.table);

        if (this.state.es_nuevo == false) {
            this.state.cumple.forEach(e => {
                if (modificar == true && e.Requisitos.responsableId == this.state.responsabilidades[0].catalogo_responsable_id && e.Requisitos.activo == true) {
                    var codigo = e.Requisitos.codigo;
                    var cumple = t['selected' + e.Id]
                    var fechaEmision = t['emision' + e.Id]
                    var fechaCaducidad = t['fecha' + e.Id]
                    var today = moment().format("YYYY-MM-DD");
                    var archivo = t['archivo_' + e.Id]

                    var req = this.state.requisitos.filter(c => c.RequisitosId == e.RequisitosId);
                    // console.log(req);

                    //Valida si el requisito es obligatorio
                    // console.log(req[0].obligatorio == true, cumple == true);
                    if (req[0].obligatorio == true && cumple == false) {
                        this.state.tableIsValid = false;
                        errors["cumple" + codigo] = "Obligatorio";
                    }

                    //Valida la fecha de emision no sea mayor a la actual
                    if (fechaEmision != undefined && fechaEmision != "") {
                        if (fechaEmision > today) {
                            this.state.tableIsValid = false;
                            errors["emision" + codigo] = "Fecha mayor a la actual";
                        }
                    }

                    //Valida la fecha de caducidad no sea anterior a la actual
                    if (fechaCaducidad != undefined && fechaCaducidad != "") {
                        if (fechaCaducidad < today) {
                            this.state.tableIsValid = false;
                            errors["fecha" + codigo] = "Fecha menor a la actual";
                        }
                    }

                    //Valida la fecha de caducidad del requisito
                    if (req[0].Requisitos.tiempo_vigencia != null && req[0].Requisitos.tiempo_vigencia > 0) {
                        var vigencia = moment(fechaEmision, "YYYY-MM-DD").add(Number.parseInt(req[0].Requisitos.tiempo_vigencia), 'M');

                        var fv = moment(vigencia).format("YYYY-MM-DD");
                        console.log(fv)
                        if (fechaCaducidad != undefined && fechaCaducidad != "") {
                            if (fechaCaducidad < today || fechaCaducidad > fv) {
                                this.state.tableIsValid = false;
                                errors["fecha" + codigo] = "Fecha mayor a " + req[0].Requisitos.tiempo_vigencia + " mes/es";
                            }
                        }

                        if (fechaCaducidad == "" && cumple == true) {
                            this.state.tableIsValid = false;
                            errors["fecha" + codigo] = "Obligatorio";
                        }

                        if (fechaEmision == "" && cumple == true) {
                            this.state.tableIsValid = false;
                            errors["emision" + codigo] = "Obligatorio";
                        }
                    }

                    //Valida si se requiere archivo
                    if (req[0].requiere_archivo == true && archivo == "" && cumple == true) {
                        this.state.tableIsValid = false;
                        errors["archivo" + codigo] = "Obligatorio";
                    }
                }

            });
        } else {
            console.log('nuevo !')
            this.state.requisitos.forEach(e => {
                if (modificar == true && e.Requisitos.responsableId == this.state.responsabilidades[0].catalogo_responsable_id && e.Requisitos.activo == true) {
                    var codigo = e.Requisitos.codigo;
                    var cumple = t['selected' + e.Id]
                    var fechaEmision = t['emision' + e.Id]
                    var fechaCaducidad = t['fecha' + e.Id]
                    var today = moment().format("YYYY-MM-DD");
                    var archivo = t['archivo_' + e.Id]

                    //Valida si el requisito es obligatorio
                    console.log(e.obligatorio, cumple);
                    if (e.obligatorio == true && cumple != true) {
                        this.state.tableIsValid = false;
                        errors["cumple" + codigo] = "Obligatorio";
                    }

                    //Valida si se requiere archivo
                    if (e.requiere_archivo == true && archivo == "" && cumple == true) {
                        this.state.tableIsValid = false;
                        errors["archivo" + codigo] = "Obligatorio";
                    }

                    //Valida la fecha de emision no sea mayor a la actual
                    if (fechaEmision != undefined && fechaEmision != "") {
                        if (fechaEmision > today) {
                            this.state.tableIsValid = false;
                            errors["emision" + codigo] = "Fecha mayor a la actual";
                        }
                    }

                    //Valida la fecha de caducidad no sea anterior a la actual
                    if (fechaCaducidad != undefined && fechaCaducidad != "") {
                        if (fechaCaducidad < today) {
                            this.state.tableIsValid = false;
                            errors["fecha" + codigo] = "Fecha menor a la actual";
                        }
                    }

                    //Valida la fecha de caducidad del requisito
                    if (e.Requisitos.tiempo_vigencia != null && e.Requisitos.tiempo_vigencia > 0) {
                        var vigencia = moment(fechaEmision, "YYYY-MM-DD").add(Number.parseInt(e.Requisitos.tiempo_vigencia), 'M');

                        var fv = moment(vigencia).format("YYYY-MM-DD");
                        console.log(fv)
                        if (fechaCaducidad != undefined && fechaCaducidad != "") {
                            if (fechaCaducidad < today || fechaCaducidad > fv) {
                                this.state.tableIsValid = false;
                                errors["fecha" + codigo] = "Fecha mayor a " + e.Requisitos.tiempo_vigencia + " mes/es";
                            }
                        }

                        if (fechaCaducidad == "" && cumple == true) {
                            this.state.tableIsValid = false;
                            errors["fecha" + codigo] = "Obligatorio";
                        }

                        if (fechaEmision == "" && cumple == true) {
                            this.state.tableIsValid = false;
                            errors["emision" + codigo] = "Obligatorio";
                        }
                    }


                }

            });
        }


        this.setState({ errtable: errors });
    }

    getDatosTable() {
        let t = Object.assign({}, this.state.table);;
        console.log('archivos', this.state.archivos);
        let req = [];
        let array = [];
        var modificar = false;

        this.state.responsabilidades.forEach(e => {
            if (e.acceso == "M") {
                modificar = true;
                console.log('modificar', modificar);
            }
        });

        

        if (this.state.es_nuevo == true) {
            console.log('this.state.accion_id', this.state.accion_id)
            this.state.requisitos.forEach(e => {
                if (e.rolId == this.state.accion_id && e.Requisitos.activo == true) {
                    array.push(e);
                }
            });
            array.forEach(e => {
               // if (modificar == true && e.Requisitos.responsableId == this.state.responsabilidades[0].catalogo_responsable_id) {
                    let result = {};

                    var cumple = t['selected' + e.Id]
                    var fechaEmision = t['emision' + e.Id]
                    var fechaCaducidad = t['fecha' + e.Id]
                    var archivo = t['archivo_' + e.Id]

                    /* Objeto */
                    result.ColaboradoresId = this.state.colaborador_id;
                    result.RequisitosId = e.Requisitos.Id;
                    result.cumple = cumple;
                    result.accion = e.rolId;
                    result.fecha_caducidad = fechaCaducidad;
                    result.fecha_emision = fechaEmision;
                    result.observacion = '';
                    result.archivo_id = '';
                    if (archivo != "") {
                        result.archivo_usuario = archivo;
                    }
                    req.push(result)
               // }

            });
        } else {
            this.state.cumple.forEach(e => {
                if (modificar == true && e.Requisitos.responsableId == this.state.responsabilidades[0].catalogo_responsable_id && e.Requisitos.activo == true) {
                    let result = {};

                    var cumple = t['selected' + e.Id]
                    var fechaEmision = t['emision' + e.Id]
                    var fechaCaducidad = t['fecha' + e.Id]
                    var archivo = t['archivo_' + e.Id]

                    /* Objeto */
                    result.Id = e.Id;
                    result.ColaboradoresId = this.state.colaborador_id;
                    result.RequisitosId = e.RequisitosId;
                    result.accion = e.catalogo_accion_id;
                    result.cumple = cumple;
                    result.fecha_caducidad = fechaCaducidad;
                    result.fecha_emision = fechaEmision;
                    result.observacion = '';
                    result.archivo_id = e.ArchivoId;
                    if (archivo != "") {
                        result.archivo_usuario = archivo;
                    }
                    req.push(result)
                }

            });
        }

        console.log(req)

        this.state.requisitos_cumplimiento = req;

    }

    consultarDatosColaborador() {
        this.setState({ loading: true })
        axios.post("/RRHH/Colaboradores/GetColaboradorRequisito/", {
            id: sessionStorage.getItem('id_colaborador'),
        }).then((response) => {
            console.log('DatosColaborador', response.data);
            this.setState({
                table: {},
                colaborador_id: response.data.Id,
                tipo_identificacion: response.data.nombre_identificacion,
                nro_identificacion: response.data.numero_identificacion,
                nombres: response.data.primer_apellido + ' ' + response.data.segundo_apellido + ' ' + response.data.nombres,
                telefono: response.data.telefono == null ? '' : response.data.telefono,
                genero: response.data.nombre_genero,
                nacionalidad: response.data.pais_pais_nacimiento_id,
                discapacidad: response.data.discapacidad == null ? '' : response.data.discapacidad,
                tipo_discapacidad: response.data.tipo_discapacidad == null ? '' : response.data.tipo_discapacidad,
                grupo_personal: response.data.catalogo_grupo_personal_id,
                nombre_grupo_personal: response.data.nombre_grupo_personal,
                requisitos: response.data.requisitos,
                cumple: response.data.req_cumple,
                responsabilidades: response.data.responsabilidades,
                es_externo: response.data.es_externo,
                loading: false
            })
            // console.log('req', this.state.requisitos);
            // this.arrayTabla(response.data.requisitos, response.data.req_cumple, response.data.responsabilidades);
            //this.consultarUser();
        })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false })
                abp.notify.error("Algo salió mal.", 'Error');
            });
    }

    arrayTabla(requisitos, cumple, responsabilidades, accion, ausentismo) {
        console.log('cumple', cumple)
        console.log('requisitos', requisitos)

        let array = [];
        let table = [];
        let a = Object.assign({}, this.state.table);
        var i = 1;
        var visualizar = false;
        var modificar = false;

        if (responsabilidades == null) {
            abp.notify.error("El usuario no tiene asignadas responsabilidades !", 'Error');
        } else {
            responsabilidades.forEach(e => {
                // console.log('accesos', e.acceso);
                if (e.acceso == "R") {
                    visualizar = true;
                    // console.log('visualizar',visualizar);
                } else if (e.acceso == "M") {
                    modificar = true;
                }
            });
        }

        if (cumple != null) {
            this.setState({ es_nuevo: false });

            console.log('accion', accion);
            if (accion == null) {
                cumple.forEach(e => {

                    var req = requisitos.filter(c => c.RequisitosId == e.RequisitosId);
                    console.log('req', req);

                    if (req[0].catalogo_tipo_ausentismo_id == ausentismo) {
                        array.push(e);
                    }
                });
            } else {
                cumple.forEach(e => {

                    var req = requisitos.filter(c => c.RequisitosId == e.RequisitosId);
                    console.log('req', req);

                    if (req[0].rolId == accion && e.Requisitos.activo == true) {
                        array.push(e);
                    }
                });
            }



            if (visualizar == true) {
                array.forEach(e => {
                    let result = {};

                    result.nro = i++;
                    result.codigo = e.Requisitos.codigo;
                    result.nombre_accion = e.nombre_accion;
                    result.nombre = e.Requisitos.nombre;
                    result.Id = e.Id;
                    result.obligatorio = false;
                    result.cumple = e.cumple;
                    result.fecha_emision = e.fecha_emision;
                    result.fecha_caducidad = e.fecha_caducidad;
                    result.archivo = e.Archivo;
                    a["emision" + (e.Id)] = e.fecha_emision == null ? '' : moment(e.fecha_emision).format("YYYY-MM-DD");
                    a["fecha" + (e.Id)] = e.fecha_caducidad == null ? '' : moment(e.fecha_caducidad).format("YYYY-MM-DD");
                    a["selected" + (e.Id)] = e.cumple;
                    if (e.Archivo != null) {
                        a["archivo_" + (e.Id)] = this.b64toBlob(e.Archivo);
                    } else {
                        a["archivo_" + (e.Id)] = '';
                    }
                    if (modificar == true && e.Requisitos.responsableId == responsabilidades[0].catalogo_responsable_id) {
                        result.disabled = false;
                    } else {
                        result.disabled = true;
                    }
                    result.responsable = e.Requisitos.Responsable.nombre;

                    table.push(result);

                });
            } else {
                array.forEach(e => {

                    if (e.Requisitos.responsableId == responsabilidades[0].catalogo_responsable_id) {
                        let result = {};

                        result.nro = i++;
                        result.codigo = e.Requisitos.codigo;
                        result.nombre_accion = e.nombre_accion;
                        result.nombre = e.Requisitos.nombre;
                        result.Id = e.Id;
                        result.obligatorio = false;
                        result.cumple = e.cumple;
                        result.fecha_emision = e.fecha_emision;
                        result.fecha_caducidad = e.fecha_caducidad;
                        result.archivo = e.Archivo;
                        a["emision" + (e.Id)] = e.fecha_emision == null ? '' : moment(e.fecha_emision).format("YYYY-MM-DD");
                        a["fecha" + (e.Id)] = e.fecha_caducidad == null ? '' : moment(e.fecha_caducidad).format("YYYY-MM-DD");
                        a["selected" + (e.Id)] = e.cumple;
                        if (e.Archivo != null) {
                            a["archivo_" + (e.Id)] = this.b64toBlob(e.Archivo);
                        } else {
                            a["archivo_" + (e.Id)] = '';
                        }
                        result.responsable = e.Requisitos.Responsable.nombre;

                        table.push(result);
                    }


                });
            }

        } else if (requisitos != null) {
            this.setState({ es_nuevo: true });

            console.log('accion', accion);
            if (accion == null) {
                requisitos.forEach(e => {
                    if (e.catalogo_tipo_ausentismo_id == ausentismo) {
                        array.push(e);
                    }
                });
            } else {
                requisitos.forEach(e => {
                    if (e.rolId == accion && e.Requisitos.activo == true) {
                        array.push(e);
                    }
                });
            }


            //Verificar si puede visualizar todos los requisitos
            if (visualizar == true) {
                array.forEach(e => {
                    let result = {};

                    result.nro = e.nro;
                    result.codigo = e.Requisitos.codigo;
                    result.nombre_accion = e.nombre_accion;
                    result.nombre = e.Requisitos.nombre;
                    result.Id = e.Id;
                    result.obligatorio = e.obligatorio;
                    result.cumple = null;
                    result.fecha_emision = null;
                    result.fecha_caducidad = null;
                    result.archivo = null;
                    a["emision" + (e.Id)] = '';
                    a["fecha" + (e.Id)] = '';
                    a["selected" + (e.Id)] = false;
                    a["archivo_" + (e.Id)] = '';
                    if (modificar == true && e.Requisitos.responsableId == responsabilidades[0].catalogo_responsable_id) {
                        result.disabled = false;
                    } else {
                        result.disabled = true;
                    }
                    result.responsable = e.Requisitos.Responsable.nombre;

                    table.push(result);

                });
            } else {
                array.forEach(e => {
                    //Mostrar los requisitos para los cuales tiene permiso
                    if (e.Requisitos.responsableId == responsabilidades[0].catalogo_responsable_id) {
                        let result = {};

                        result.nro = e.nro;
                        result.codigo = e.Requisitos.codigo;
                        result.nombre_accion = e.nombre_accion;
                        result.nombre = e.Requisitos.nombre;
                        result.Id = e.Id;
                        result.obligatorio = e.obligatorio;
                        result.cumple = null;
                        result.fecha_emision = null;
                        result.fecha_caducidad = null;
                        result.archivo = null;
                        a["emision" + (e.Id)] = '';
                        a["fecha" + (e.Id)] = '';
                        a["selected" + (e.Id)] = false;
                        a["archivo_" + (e.Id)] = '';
                        result.responsable = e.Requisitos.Responsable.nombre;

                        table.push(result);
                    }

                });
            }


        }

        console.log('array', table);
        this.setState({ req_table: table, table: a, accion_id: accion });
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

    handleSubmit(event) {
        event.preventDefault();
        // console.log('this.state.table',Object.keys(this.state.table).length )
        if (Object.keys(this.state.table).length > 0) {
            this.validateTable();
            if (this.state.tableIsValid == true) {
                this.setState({ loading: true });
                this.getDatosTable();
                console.log('requisitos_cumplimiento', this.state.requisitos_cumplimiento)
                var sucess = this.state.requisitos_cumplimiento.length;
                // console.log('sucess',sucess)
                var cont = 0;
                this.state.requisitos_cumplimiento.forEach(e => {
                    const formData = new FormData();
                    console.log('archivo', e.archivo_id)
                    formData.append('id', e.Id)
                    formData.append('archivo', e.archivo_id)
                    formData.append('idColaborador', e.ColaboradoresId)
                    formData.append('idRequisito', e.RequisitosId)
                    formData.append('cumple', e.cumple)
                    //formData.append('accion', e.accion)
                    formData.append('accion', 0)
                    formData.append('fecha_caducidad', e.fecha_caducidad)
                    formData.append('fecha_emision', e.fecha_emision)
                    formData.append('UploadedFile', e.archivo_usuario)

                    const config = { headers: { 'content-type': 'multipart/form-data' } }

                    axios.post("/RRHH/ColaboradorRequisito/CreateApi/", formData, config)
                        .then((response) => {
                            console.log("DATA ", response.data);
                            if (response.data == "OK") {
                                cont++;
                                // console.log('cont',cont)
                                console.log('sucess == cont', sucess == cont)
                                if (sucess == cont) {
                                    this.setState({ loading: false });
                                    abp.notify.success("Cumplimiento de Requisitos Guardado!", "Aviso");
                                }
                            } else {
                                abp.notify.error("Algo salió mal.", 'Error');
                                this.setState({ loading: false });
                            }


                        })
                        .catch((error) => {
                            this.setState({ loading: false });
                            console.log("ERROR", error);
                        });

                });
            } else {
                abp.notify.error("Se ha encontrado errores, por favor revisar el formulario", 'Error');
            }
        }
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['TIPOINDENTIFICACION', 'TIPODISCAPACIDAD', 'GRUPOPERSONAL', 'NACIONALIDADES', 'GENERO',
            'ACCIONCOL', 'TIPOAUSENTISMO'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                //console.log(response.data);
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    cargarCatalogos(data) {
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
            switch (codigoCatalogo) {
                case 'TIPOINDENTIFICACION':
                    this.setState({ tiposIdentificacion: catalogo })
                    this.getFormSelectTipoIdent();
                    return;
                case 'TIPODISCAPACIDAD':
                    this.setState({ tipoDiscapacidad: catalogo })
                    this.getFormSelectTipoDiscapacidad()
                    return;
                case 'GRUPOPERSONAL':
                    this.setState({ gruposPersonal: catalogo })
                    this.getFormSelectGrupoPersonal()
                    return;
                case 'NACIONALIDADES':
                    this.setState({ tiposNacionalidades: catalogo })
                    this.getFormSelectTipoIdent();
                    return;
                case 'GENERO':
                    this.setState({ generos: catalogo })
                    this.getFormSelectGenero();
                    return;
                case 'ACCIONCOL':
                    this.setState({ tiposRoles: catalogo })
                    this.getFormSelectAccion()
                    return;
                case 'TIPOAUSENTISMO':
                    this.setState({ tiposAusentismo: catalogo })
                    this.getFormSelectAusentismo()
                    return;
            }
        });
        //this.setState({ loading: false })

    }

    getFormSelectAccion() {
        return (
            this.state.tiposRoles.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectAusentismo() {
        return (
            this.state.tiposAusentismo.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectTipoIdent() {
        return (
            this.state.tiposIdentificacion.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectTipoDiscapacidad() {
        return (
            this.state.tipoDiscapacidad.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectGrupoPersonal() {
        return (
            this.state.gruposPersonal.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectGenero() {
        return (
            this.state.generos.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    getFormSelectNacionalidades() {
        return (
            this.state.tiposNacionalidades.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    validarCedula(cedula) {
        var cad = cedula;
        var total = 0;
        var i = 0;
        var longitud = cad.length;
        var longcheck = longitud - 1;

        if (cad !== "" && longitud === 10) {
            for (i = 0; i < longcheck; i++) {
                if (i % 2 === 0) {
                    var aux = cad.charAt(i) * 2;
                    if (aux > 9) aux -= 9;
                    total += aux;
                } else {
                    total += parseInt(cad.charAt(i)); // parseInt o concatenará en lugar de sumar
                }
            }

            total = total % 10 ? 10 - total % 10 : 0;

            if (cad.charAt(longitud - 1) == total) {
                this.setState({
                    cedula_valida: true
                })
                //console.log("Cedula Válida");
                return true;
            } else {
                this.setState({
                    cedula_valida: false
                })
                //console.log("Cedula Inválida");
                return false;
            }
        }
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
        if (event.target.name == 'tipo_ausentismo') {
            this.arrayTabla(this.state.requisitos, this.state.cumple, this.state.responsabilidades, null, event.target.value);
        }
    }

    handleChangeAccion(event) {
        this.setState({ [event.target.name]: event.target.value, req_table: [], table: {} });
        if (event.target.value != '') {
            var accion = this.state.tiposRoles.filter(c => c.Id == Number.parseInt(event.target.value));
            console.log(accion)
            if (accion[0].codigo == "AUS") {
                this.setState({ visible_ausentismo: true });
            } else {
                this.setState({ visible_ausentismo: false });

                this.arrayTabla(this.state.requisitos, this.state.cumple, this.state.responsabilidades, event.target.value, null);
            }
        } else {
            this.setState({ visible_ausentismo: false });
        }
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/ColaboradorRequisito/Index/"
        );
    }


}
ReactDOM.render(
    <CrearCumplimientoRequisitos />,
    document.getElementById('content-cumple-requisitos')
);