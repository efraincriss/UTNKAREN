import React from 'react';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import { Dialog } from 'primereact/components/dialog/Dialog';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class ContactoEmergencia extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            nro: 0,
            loading: true,
            visible: false,
            contactos: [],
            errores: [],
            formIsValid: false,
            nombres_apellidos: '',
            nro_identificacion: '',
            relacion: '',
            urbanizacion: '',
            direccion: '',
            telefono: '',
            contacto_id: '',
            tiposRelacion: [],
            celular: '',
        }

        this.handleChange = this.handleChange.bind(this);
        this.handleValidation = this.handleValidation.bind(this);
        this.handleChangeUpperCase = this.handleChangeUpperCase.bind(this);
        this.Guardar = this.Guardar.bind(this);

        this.abrirRegistroCarga = this.abrirRegistroCarga.bind(this);
        this.cerrarRegistroCarga = this.cerrarRegistroCarga.bind(this);
        this.AlmacenarContactosEmergencia = this.AlmacenarContactosEmergencia.bind(this);
        this.ClearStates = this.ClearStates.bind(this);


        this.GetCatalogos = this.GetCatalogos.bind(this);
        this.cargarCatalogos = this.cargarCatalogos.bind(this);
        this.getFormSelectRelacion = this.getFormSelectRelacion.bind(this);

    }

    componentDidMount() {
        this.GetCatalogos();
    }

    render() {
        const options = {
            withoutNoDataText: true
        };
        return (
            <BlockUi tag="div" blocking={this.state.loading}>
                <div>
                    <div className="row">
                        <div className="col-xs-12 col-md-12">
                            <form onSubmit={this.handleSubmit}>
                                <div className="row">
                                    <div className="col">
                                        <button type="button" onClick={this.abrirRegistroCarga} className="btn btn-outline-primary fa fa-plus pull-right"> Nuevo</button>
                                    </div>
                                </div>
                                <br />
                                <div className="row">
                                    <div className="col">
                                    <BootstrapTable
                                            data={this.state.contactos}
                                            hover={true}
                                            pagination={true}
                                            options={options}
                                        >
                                            <TableHeaderColumn width={'6%'} dataField='nro' isKey={true} dataAlign="center" headerAlign="center" dataAlign="center" dataSort={true}>N??</TableHeaderColumn>
                                            <TableHeaderColumn width={'13%'} dataField='Identificacion' thStyle={{ whiteSpace: 'normal' }} headerAlign="center" dataSort={true}>N??mero Identificaci??n</TableHeaderColumn>
                                            <TableHeaderColumn dataField="Nombres" thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" dataSort={true}>Apellidos Nombres</TableHeaderColumn>
                                            <TableHeaderColumn dataField='RelacionNombre' thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" dataSort={true}>Relaci??n</TableHeaderColumn>
                                            <TableHeaderColumn dataField="UrbanizacionComuna" thStyle={{ whiteSpace: 'normal' }} tdStyle={{ whiteSpace: 'normal' }} headerAlign="center" dataSort={true}>Urbanizaci??n / Comuna</TableHeaderColumn>
                                            <TableHeaderColumn width={'13%'} dataField="Celular" thStyle={{ whiteSpace: 'normal' }} headerAlign="center" dataSort={true}>Celular</TableHeaderColumn>
                                            <TableHeaderColumn width={'13%'} dataField='Operaciones' dataFormat={this.generateButton.bind(this)} headerAlign="center">Opciones</TableHeaderColumn>
                                        </BootstrapTable>
                                    </div>
                                </div>

                                <br />
                                <div className="row">
                                    <div className="col">
                                        <div className="form-group">
                                            <button onClick={() => this.props.regresar()} type="button" className="btn btn-outline-primary fa fa-arrow-left" style={{ marginLeft: '3px' }}> Cancelar</button>
                                        </div>
                                    </div>
                                </div>

                            </form>


                        </div>
                    </div>
                    <Dialog header="Registro Contacto de Emergencia" visible={this.state.visible} style={{ width: '50%' }} minY={70} onHide={this.cerrarRegistroCarga} maximizable={true}>
                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="observacion">* No. de Identificaci??n: </label>
                                    <input type="text" id="nro_identificacion" className="form-control" value={this.state.nro_identificacion} onChange={this.handleChangeUpperCase} name="nro_identificacion" />
                                    {this.state.errores["nro_identificacion"]}
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="nombres_apellidos">* Apellidos Nombres: </label>
                                    <input type="text" id="nombres_apellidos" className="form-control" value={this.state.nombres_apellidos} onChange={this.handleChangeUpperCase} name="nombres_apellidos" />
                                    {this.state.errores["nombres_apellidos"]}
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="relacion">* Relaci??n: </label>
                                    <select value={this.state.relacion} onChange={this.handleChange} className="form-control" name="relacion">
                                        <option value="">Seleccione...</option>
                                        {this.getFormSelectRelacion()}
                                    </select>
                                    {this.state.errores["relacion"]}
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="urbanizacion">* Urbanizaci??n / Comuna: </label>
                                    <input type="text" id="urbanizacion" className="form-control" value={this.state.urbanizacion} onChange={this.handleChangeUpperCase} name="urbanizacion" />
                                    {this.state.errores["urbanizacion"]}
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="direccion">* Direcci??n: </label>
                                    <input type="text" id="direccion" className="form-control" value={this.state.direccion} onChange={this.handleChangeUpperCase} name="direccion" />
                                    {this.state.errores["direccion"]}
                                </div>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="celular">* Celular: </label>
                                    <input type="text" id="celular" className="form-control" value={this.state.celular} onChange={this.handleChange} name="celular" />
                                    {this.state.errores["celular"]}
                                </div>
                            </div>
                            <div className="col">
                                <div className="form-group">
                                    <label htmlFor="telefono">Tel??fono: </label>
                                    <input type="text" id="telefono" className="form-control" value={this.state.telefono} onChange={this.handleChange} name="telefono" />
                                    {this.state.errores["telefono"]}
                                </div>
                            </div>
                        </div>
                        <div className="form-group">
                            <div className="col">
                                <button type="button" onClick={() => this.Guardar()} className="btn btn-outline-primary" disabled={this.props.disable_button}> Guardar</button>
                                <button onClick={() => this.cerrarRegistroCarga()} type="button" className="btn btn-outline-primary" style={{ marginLeft: '3px' }}> Cancelar</button>

                            </div>
                        </div>

                    </Dialog>
                </div >
            </BlockUi>
        )
    }

    generateButton(cell, row) {
        var Id = 0;
        if (row.contacto_id > 0) {
            Id = row.contacto_id;
        } else {
            Id = row.idContacto;
        }
        return (
            <div>
                <button onClick={() => this.LoadContacto(Id)} type="button" className="btn btn-outline-primary btn-sm">Editar</button>
                <button onClick={() => this.Delete(Id)} type="button" className="btn btn-outline-danger btn-sm" style={{ marginLeft: '0.2em' }}>Eliminar</button>
            </div>
        )
    }

    Delete(id) {
        console.log('id', id);

        let array = this.state.contactos.slice();

        var index = array.findIndex(c => c.contacto_id === id)
        array.splice(index, 1);
        console.log('array', array)
        // this.setState({ cargas: array });
        var i = 1;
        array.forEach(e =>{
            e.nro = i++;
        });
        this.setState({ loading: true });
        axios.post("/RRHH/ContactoEmergencia/DeleteContactoEmergenciaApi/", {
            Id: id
        })
            .then((response) => {
                if (response.data == "OK") {
                    abp.notify.success("Contacto de emergencia eliminado!", "Aviso");
                    this.setState({ contactos: array });
                    console.log(this.state.contactos)
                    this.props.ContactosEmergenciaInfo(this.state.contactos);
                } else {
                    abp.notify.error('Algo sali?? mal', 'Error');
                }
                this.setState({ loading: false });
            })
            .catch((error) => {
                console.log(error);
                this.setState({ loading: false });
                abp.notify.error('Algo sali?? mal', 'Error');
            });
    }

    LoadContacto(id) {
        var contacto = this.state.contactos.filter(c => c.contacto_id === id);
        if (contacto.length == 0) {
            contacto = this.state.contactos.filter(c => c.idContacto === id);
        }
        console.log('contacto', contacto);
        if (contacto.length > 0) {
            this.abrirRegistroCarga();
            this.state.contacto_id = id;
            this.state.nro_identificacion = contacto[0].Identificacion;
            this.state.nombres_apellidos = contacto[0].Nombres;
            this.state.relacion = contacto[0].Relacion;
            this.state.urbanizacion = contacto[0].UrbanizacionComuna;
            this.state.direccion = contacto[0].Direccion;
            this.state.telefono = contacto[0].Telefono;
            this.state.celular = contacto[0].Celular;
        }
    }

    abrirRegistroCarga() {
        this.setState({ visible: true });
    }

    cerrarRegistroCarga() {
        this.ClearStates();
        this.setState({ visible: false });
    }

    ClearStates() {
        this.setState({
            nombres_apellidos: '',
            nro_identificacion: '',
            relacion: '',
            urbanizacion: '',
            direccion: '',
            telefono: '',
            contacto_id: '',
        });
    }

    handleValidation() {
        let errors = {};
        this.state.formIsValid = true;

        if (!this.state.nro_identificacion) {
            this.state.formIsValid = false;
            errors["nro_identificacion"] = <div className="alert alert-danger">El campo No. de Identificaci??n es obligatorio</div>;
        }
        if (!this.state.nombres_apellidos) {
            this.state.formIsValid = false;
            errors["nombres_apellidos"] = <div className="alert alert-danger">El campo Apellidos Nombres es obligatorio</div>;
        }
        if (!this.state.relacion) {
            this.state.formIsValid = false;
            errors["relacion"] = <div className="alert alert-danger">El campo Relaci??n es obligatorio</div>;
        }
        if (!this.state.urbanizacion) {
            this.state.formIsValid = false;
            errors["urbanizacion"] = <div className="alert alert-danger">El campo Urbanizaci??n / Comuna es obligatorio</div>;
        }
        if (!this.state.direccion) {
            this.state.formIsValid = false;
            errors["direccion"] = <div className="alert alert-danger">El campo Direcci??n es obligatorio</div>;
        }
        if (this.state.telefono) {
            if (this.state.telefono.length > 9) {
                this.state.formIsValid = false;
                errors["telefono"] = <div className="alert alert-danger">El campo no puede tener m??s de nueve d??gitos.</div>;
            }
            if (!isFinite(this.state.telefono)) {
                this.state.formIsValid = false;
                errors["telefono"] = <div className="alert alert-danger">El campo permite solo ingreso num??rico</div>;
            }
        }
        if (!this.state.celular) {
            this.state.formIsValid = false;
            errors["celular"] = <div className="alert alert-danger">El campo Celular es obligatorio</div>;
        }else{
            if (this.state.celular.length > 10) {
                this.state.formIsValid = false;
                errors["celular"] = <div className="alert alert-danger">El campo no puede tener m??s de diez d??gitos.</div>;
            }
            if (!isFinite(this.state.celular)) {
                this.state.formIsValid = false;
                errors["celular"] = <div className="alert alert-danger">El campo permite solo ingreso num??rico</div>;
            }
        }

        this.setState({ errores: errors });
    }

    Guardar() {
        this.handleValidation();
        if (this.state.formIsValid) {
            this.setState({ loading: true });
            axios.post("/RRHH/ContactoEmergencia/CreateContactoEmergenciaApi/", {
                Id: this.state.contacto_id,
                ColaboradorId: this.props.id_colaborador,
                Nombres: this.state.nombres_apellidos,
                Identificacion: this.state.nro_identificacion,
                Relacion: this.state.relacion,
                UrbanizacionComuna: this.state.urbanizacion,
                Direccion: this.state.direccion,
                Telefono: this.state.telefono,
                Celular: this.state.celular,
            })
                .then((response) => {
                    console.log('contactoe', response.data)
                    if (response.data > 0) {
                        abp.notify.success("Contacto de emergencia guardado!", "Aviso");
                        this.AlmacenarContactosEmergencia(response.data);
                    } else if (response.data == -1) {
                        abp.notify.error('Contacto de emergencia ya se encuentra registrado', 'Error');
                    } else {
                        abp.notify.error('Algo sali?? mal', 'Error');
                    }
                    this.setState({ loading: false });
                })
                .catch((error) => {
                    console.log(error);
                    this.setState({ loading: false });
                    abp.notify.error('Algo sali?? mal', 'Error');
                });
        } else {
            abp.notify.error('Se ha encontrado errores, por favor revisar el formulario', 'Error');
        }
    }

    AlmacenarContactosEmergencia(id) {

        let contacto = {};
        var relacion = this.state.tiposRelacion.filter(c => c.Id === parseInt(this.state.relacion));

        if (this.state.contacto_id != 0) {
            console.log('existente', this.state.contactos);
            var filterContact = this.state.contactos.filter(c => c.contacto_id === id);
            var indexCarga = this.state.contactos.findIndex(c => c.contacto_id === id);
            if (filterContact.length == 0) {
                filterContact = this.state.contactos.filter(c => c.idContacto === id);
                indexCarga = this.state.contactos.findIndex(c => c.idContacto === id);
            }
            if (indexCarga < 0) {
                indexCarga = 0;
            }
            console.log('cargasAA', indexCarga, filterContact);
            // console.log('indexCarga', indexCarga);

            if (filterContact.length > 0) {
                console.log('entro update');
                contacto.contacto_id = id;
                contacto.nro = filterContact[0].nro;
                contacto.ColaboradorId = this.props.id_colaborador;
                contacto.Nombres = this.state.nombres_apellidos;
                contacto.Identificacion = this.state.nro_identificacion;
                contacto.Relacion = this.state.relacion;
                contacto.RelacionNombre = relacion[0].nombre;
                contacto.UrbanizacionComuna = this.state.urbanizacion;
                contacto.Direccion = this.state.direccion;
                contacto.Telefono = this.state.telefono;
                contacto.Celular = this.state.celular;
                contacto.idContacto = filterContact[0].idContacto;

                this.state.contactos[indexCarga] = contacto;
            }


            console.log('contactos update', this.state.contactos)
            // this.state.cargas[indexCarga] = cargas;

        } else {
            console.log('nuevo');
            contacto = {
                contacto_id: id,
                nro: ++this.state.nro,
                ColaboradorId: this.props.id_colaborador,
                Nombres: this.state.nombres_apellidos,
                Identificacion: this.state.nro_identificacion,
                Relacion: this.state.relacion,
                RelacionNombre: relacion[0].nombre,
                UrbanizacionComuna: this.state.urbanizacion,
                Direccion: this.state.direccion,
                Telefono: this.state.telefono,
                Celular: this.state.celular,
                idContacto: id,
            };
            // this.setState({
            //     cargas: [...this.state.cargas, cargas]
            // });

            this.state.contactos.push(contacto)
            console.log('this.state.contactos', this.state.contactos)
        }

        this.cerrarRegistroCarga();
    }

    GetCatalogos() {
        this.setState({ loading: true })
        let codigos = [];

        codigos = ['RELACION'];

        axios.post("/RRHH/Colaboradores/GetByCodeApiList/", { codigo: codigos })
            .then((response) => {
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
                case 'RELACION':
                    this.setState({ tiposRelacion: catalogo })
                    this.getFormSelectRelacion();
                    return;
            }
        });

        this.setState({ loading: false })
    }

    getFormSelectRelacion() {
        return (
            this.state.tiposRelacion.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }

    handleChangeUpperCase(event) {
        this.setState({ [event.target.name]: event.target.value.toUpperCase() });
    }

}