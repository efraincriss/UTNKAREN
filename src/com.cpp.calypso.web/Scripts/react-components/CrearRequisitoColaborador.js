import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';

export default class CrearRequisitoColaborador extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            tipo_usuario: '',
            requisito: '',
            descripcion: '',
            rol: '',
            obligatorio: true,
            requiere_archivo: '',
            tiposUsuario: [],
            tiposRequisito: [],
            roles: [],
            errores: [],
            frentes: [],
            id_frentes: [],
            sel: [],
            formIsValid: '',
            loading: true,
            tiposAusentismo: [],
            tipo_ausentismo: '',
            visible_ausentismo: false,
        }

        this.Regresar = this.Regresar.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.Select = this.Select.bind(this);
        this.Delete = this.Delete.bind(this);
        this.handleChangeAccion = this.handleChangeAccion.bind(this);

        this.getFormSelectAusentismo = this.getFormSelectAusentismo.bind(this);
        this.getFormSelectTipoUsuario = this.getFormSelectTipoUsuario.bind(this);
        this.GetRequisitos = this.GetRequisitos.bind(this);
        this.getFormSelectRequisito = this.getFormSelectRequisito.bind(this);
        this.getFormSelectRol = this.getFormSelectRol.bind(this);
        this.GetFrentes = this.GetFrentes.bind(this);

        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);

    }

    componentDidMount() {
        this.GetCatalogos();
        this.GetRequisitos();
        // this.GetFrentes();
    }

    render() {

        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <form onSubmit={this.handleSubmit}>
                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label">* Agrupación para Requisitos: </label>
                                    <select value={this.state.tipo_usuario} onChange={this.handleChange} className="form-control" name="tipo_usuario">
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectTipoUsuario()}
                                    </select>
                                    {this.state.errores["tipo_usuario"]}
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="rol">* Acción: </label>
                                    <select value={this.state.rol} onChange={this.handleChangeAccion} className="form-control" name="rol">
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectRol()}
                                    </select>
                                    {this.state.errores["rol"]}
                                </div>
                            </div>

                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="label">* Requisito: </label>
                                    <select value={this.state.requisito} onChange={this.handleChange} className="form-control" name="requisito">
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectRequisito()}
                                    </select>
                                    {this.state.errores["requisito"]}
                                </div>
                            </div>
                            <div className="col" style={{ visibility: this.state.visible_ausentismo == true ? 'visible' : 'hidden' }}>
                                <div className="form-group">
                                    <label htmlFor="tipo_ausentismo">* Tipo de Ausentismo: </label>
                                    <select value={this.state.tipo_ausentismo} onChange={this.handleChange} className="form-control" name="tipo_ausentismo">
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectAusentismo()}
                                    </select>
                                    {this.state.errores["tipo_ausentismo"]}
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="descripcion">* Descripción: </label>
                                    <textarea type="text" id="descripcion" /*rows="1"*/ maxLength="200" className="form-control" value={this.state.descripcion} onChange={this.handleChange} name="descripcion" />
                                    {this.state.errores["descripcion"]}
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '15px' }}>
                                    <label htmlFor="obligatorio" style={{ width: '294px' }}>Obligatorio: </label>
                                    <input type="checkbox" id="obligatorio" className="form-control" checked={this.state.obligatorio} onChange={this.handleInputChange} name="obligatorio" style={{ marginTop: '5px', marginLeft: '-160px' }} />
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group checkbox" style={{ display: 'inline-flex', marginTop: '15px' }}>
                                    <label htmlFor="requiere_archivo" style={{ width: '294px' }}>Requiere Archivo: </label>
                                    <input type="checkbox" id="requiere_archivo" className="form-control" checked={this.state.requiere_archivo} onChange={this.handleInputChange} name="requiere_archivo" style={{ marginTop: '5px', marginLeft: '-110px' }} />
                                </div>
                            </div>
                        </div>

                        {/* <div className="row">
                            <div className="col-2">
                                <div className="form-group">
                                    <label htmlFor="frentes" style={{ width: '294px' }}>Frente / Locación: </label>
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <div className="content-section implementation">
                                        <MultiSelect value={this.state.sel} optionLabel="nombre" options={this.state.frentes} dataKey="Id" defaultLabel="Seleccione..." onChange={(e) => this.setState({ sel: e.value })}
                                            style={{ width: '50%', marginLeft: '5%' }} filter={true} />
                                    </div>
                                    <span style={{ color: "red", marginLeft: '5%' }}>{this.state.errores["frentes"]}
                                </div>
                            </div>
                        </div> */}

                        {/* <br />
                        <div className="row">
                            <div className="col" style={{ visibility: this.state.sel != '' ? 'visible' : 'hidden' }}>
                                <BootstrapTable
                                    data={this.state.sel}
                                    hover={true}
                                    pagination={true}
                                    striped={false}
                                    condensed={true}
                                // key={Math.random()}
                                >
                                    <TableHeaderColumn dataField="ZonaId" isKey={true} dataAlign="center" headerAlign="center" dataAlign="center" dataSort={true}>Id Zona</TableHeaderColumn>
                                    <TableHeaderColumn dataField="codigo" headerAlign="center" dataAlign="center" dataSort={true}>Código</TableHeaderColumn>
                                    <TableHeaderColumn dataField='nombre' headerAlign="center" dataSort={true}>Frente/Locación</TableHeaderColumn>
                                    <TableHeaderColumn dataField='Operaciones' dataFormat={this.generateButton.bind(this)} headerAlign="center" dataAlign="center">Opciones</TableHeaderColumn>
                                </BootstrapTable>
                            </div>
                        </div> */}

                        <div className="form-group" /*style={{ marginTop: this.state.sel == '' ? '-20%' : '0%' }}*/>
                            <div className="col">
                                <button type="submit" className="btn btn-outline-primary fa fa-save"> Guardar</button>
                                <button onClick={() => this.Regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                            </div>
                        </div>

                    </form>
                </div >
            </BlockUi>
        )
    }

    generateButton(cell, row) {
        return (
            <div>
                <button onClick={() => this.Delete(row.Id)} type="button" className="btn fa fa-trash" style={{ marginLeft: '0.2em' }}></button>
            </div>
        )
    }


    handleValidation() {
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.tipo_usuario) {
            this.state.formIsValid = false;
            errors["tipo_usuario"] = <div className="alert alert-danger">El campo Agrupación para Requisitos es obligatorio</div>;
        }
        if (!this.state.requisito) {
            this.state.formIsValid = false;
            errors["requisito"] = <div className="alert alert-danger">El campo Requisito es obligatorio</div>;
        }
        if (!this.state.rol) {
            this.state.formIsValid = false;
            errors["rol"] = <div className="alert alert-danger">El campo Acción es obligatorio</div>;
        } else {
            var accion = this.state.roles.filter(c => c.Id == Number.parseInt(this.state.rol));
            console.log(accion)
            if (accion[0].codigo == "AUSENTISMO" && !this.state.tipo_ausentismo) {
                this.state.formIsValid = false;
                errors["tipo_ausentismo"] = <div className="alert alert-danger">El campo Tipo de Ausentismo es obligatorio</div>;
            }
        }
        if (!this.state.descripcion) {
            this.state.formIsValid = false;
            errors["descripcion"] = <div className="alert alert-danger">El campo Descripción es obligatorio</div>;
        }
        // if (this.state.sel == '') {
        //     this.state.formIsValid = false;
        //     errors["frentes"] = <div className="alert alert-danger">El campo Frente / Locación es obligatorio</div>;
        // }

        this.setState({ errores: errors });
    }

    handleSubmit(event) {
        event.preventDefault();
        // if (this.state.sel != '') {
        //     this.Select();
        // }
        this.handleValidation();
        //console.log(this.state.id_frentes);
        if (this.state.formIsValid) {

            axios.post("/RRHH/RequisitoColaborador/CreateApiAsync/", {
                tipo_usuarioId: this.state.tipo_usuario,
                RequisitosId: this.state.requisito,
                descripcion: this.state.descripcion,
                rolId: this.state.rol,
                obligatorio: this.state.obligatorio,
                requiere_archivo: this.state.requiere_archivo,
                catalogo_tipo_ausentismo_id: this.state.tipo_ausentismo,
                // frentes: this.state.id_frentes
            })
                .then((response) => {
                    if (response.data == "OK") {
                        this.setState({ loading: true })
                        abp.notify.success("Requisito guardado!", "Aviso");
                        setTimeout(
                            function () {
                                this.Regresar()
                            }.bind(this), 2000
                        );
                    } else if (response.data == "SI") {
                        abp.notify.error('Requisito ya existe!', 'Error');
                    }
                })
                .catch((error) => {
                    console.log(error);
                    abp.notify.error('Algo salió mal.', 'Error');
                });

        }else {
            abp.notify.error('Se ha encontrado errores, por favor revisar el formulario', 'Error');
        }
    }

    GetCatalogos() {
        let codigos = [];

        codigos = ['GRUPOPERSONAL', 'ACCIONCOL', 'TIPOAUSENTISMO'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
                //console.log('catalogos',response.data);
                this.cargarCatalogos(response.data);
            })
            .catch((error) => {
                console.log(error);
            });
    }

    cargarCatalogos(data) {
        //console.log('data',data);
        data.forEach(e => {
            var catalogo = JSON.parse(e);
            var codigoCatalogo = catalogo[0].TipoCatalogo.codigo;
            switch (codigoCatalogo) {
                case 'GRUPOPERSONAL':
                    this.setState({ tiposUsuario: catalogo })
                    this.getFormSelectTipoUsuario()
                    return;
                case 'ACCIONCOL':
                    this.setState({ roles: catalogo })
                    this.getFormSelectRol()
                    return;
                case 'TIPOAUSENTISMO':
                    this.setState({ tiposAusentismo: catalogo })
                    this.getFormSelectAusentismo()
                    return;
                default:
                    console.log(codigoCatalogo)
                    return;
            }


        });
        this.setState({ loading: false })

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

    getFormSelectTipoUsuario() {
        return (
            this.state.tiposUsuario.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    GetRequisitos() {
        axios.post("/RRHH/Requisitos/GetRequisitosApi", {})
            .then((response) => {
                console.log('reqlist', response.data)
                this.setState({ tiposRequisito: response.data })
                this.getFormSelectRequisito()

            })
            .catch((error) => {
                console.log(error);
            });
    }

    getFormSelectRequisito() {
        return (
            this.state.tiposRequisito.map((item) => {
                //console.log(item.nombre_requisito);
                if (item.nombre_requisito == "COLABORADORESREQ") {
                    return (
                        <option key={item.Id} value={item.Id}>{item.nombre}</option>
                    )
                }
            })
        );
    }

    getFormSelectRol() {
        return (
            this.state.roles.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    GetFrentes() {
        axios.post("/RRHH/RequisitoColaborador/GetFrentesApi/", {})
            .then((response) => {
                this.setState({ frentes: response.data })

            })
            .catch((error) => {
                console.log(error);
            });


    }

    Select() {
        let selectedFrentes = this.state.sel.slice();
        let ids = [];
        selectedFrentes.forEach(element => {
            ids.push(element.Id)
        });
        this.state.id_frentes = ids.slice();
    }

    Delete(id) {
        //console.log(id);
        let selectedFrentes = this.state.sel.slice();

        var index = selectedFrentes.findIndex(selectedFrentes => selectedFrentes.Id === id)
        selectedFrentes.splice(index, 1);
        //console.log(selectedFrentes);

        this.setState({ sel: selectedFrentes });
    }

    Regresar() {
        return (
            window.location.href = "/RRHH/RequisitoColaborador/Index/"
        );
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeAccion(event) {
        this.setState({ [event.target.name]: event.target.value });
        if (event.target.value != '') {
            var accion = this.state.roles.filter(c => c.Id == Number.parseInt(event.target.value));
            console.log(accion)
            if (accion[0].codigo == "AUS") {
                this.setState({ visible_ausentismo: true });
            } else {
                this.setState({ visible_ausentismo: false });
            }
        } else {
            this.setState({ visible_ausentismo: false });
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

}


ReactDOM.render(
    <CrearRequisitoColaborador />,
    document.getElementById('content-crear-requisito')
);