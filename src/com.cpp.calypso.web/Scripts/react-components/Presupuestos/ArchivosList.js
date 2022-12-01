import React from "react";
import ReactDOM from "react-dom";
import BlockUi from 'react-block-ui';
import axios from "axios";
import Field from "../Base/Field-v2";
import { BootstrapTable, TableHeaderColumn } from "react-bootstrap-table";
import { Dialog } from "primereact-v2/components/dialog/Dialog";
import { Growl } from "primereact-v2/components/growl/Growl";
 export default class  ArchivosList extends React.Component {
    constructor(props) {
        super(props);
        this.state = {

            data: [],
            visible: false,
            errors: {},
            uploadFile: '',
            descripcion: '',
            editable: true,
            imagen: '',
            Id: 0,
            action: "create"



        };

        this.handleChange = this.handleChange.bind(this);
        this.onChangeValue = this.onChangeValue.bind(this);
        this.mostrarForm = this.mostrarForm.bind(this);
        this.OcultarFormulario = this.OcultarFormulario.bind();
        this.isValid = this.isValid.bind();
        this.EnviarFormulario = this.EnviarFormulario.bind(this);

    }

    componentDidMount() {
        this.ObtenerArchivos();
        this.props.unlockScreen();
    }

    isValid = () => {
        const errors = {};
        if (this.state.editable) {
            if (this.state.uploadFile == null || this.state.uploadFile === '') {
                errors.uploadFile = "Campo Requerido";
            }
        }
       


        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }
    ObtenerArchivos = () => {
        console.log('InitArchivos');
        this.props.blockScreen();
        axios
            .post("/Proyecto/OfertaPresupuesto/UploadFileList/" +  this.props.OfertaId, {})
            .then(response => {
                console.log(response.data);
                this.setState({ data: response.data });
                this.props.unlockScreen();
            })
            .catch(error => {
                console.log(error);
                this.props.unlockScreen();
            });
    }


    Secuencial(cell, row, enumObject, index) {
        return (<div>{index + 1}</div>)
    }

    onChangeValue = (name, value) => {
        this.setState({
            [name]: value
        });
    };

    Eliminar = (Id) => {
        console.log(Id);
        axios
            .post("/Proyecto/OfertaPresupuesto/DeleteArchivo", {
                Id: Id
            })
            .then(response => {
                if (response.data == "OK") {
                    this.props.showSuccess("Eliminado Correctamente");
                    this.ObtenerArchivos();
                } else if (response.data == "NOPUEDE") {
                    this.props.showWarn("No se puedo Eliminar");

                }
            })
            .catch(error => {
                console.log(error);
                this.props.showWarn("Ocurrió un Error");
            });
    }

    mostrarForm = (row) => {
        if (row != null && row.Id > 0) {
            this.setState({
                Id: row.Id,
                descripcion: row.descripcion,
                imagen: row.filebase64,
                action: "edit",
                visible: true,
                editable: false,

            });
        } else {
            this.setState({
                Id: 0,
                descripcion: "",
                imagen: "",
                action: "create",
                visible: true,
                editable: true,
            });
        }


    }


    generarBotones = (cell, row) => {
        return (
            <div>
               {/*  <button
                    className="btn btn-outline-info"
                    style={{ marginLeft: "0.3em" }}
                    onClick={() => this.mostrarForm(row)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Editar Descripción"
                >
                    <i className="fa fa-edit" />
                </button>
                */}
                <button
                    className="btn btn-outline-danger"
                    style={{ marginLeft: "0.3em" }}
                    onClick={() => {
                        if (
                            window.confirm(
                                `Esta acción eliminará el registro, ¿Desea continuar?`
                            )
                        )
                            this.Eliminar(row.Id);
                    }}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Eliminar "
                >
                    <i className="fa fa-trash" />
                </button>
                <button className="btn btn-outline-indigo" style={{ marginLeft: '0.3em' }}
                    onClick={() => this.onDownloaImagen(row.Id)}
                    data-toggle="tooltip"
                    data-placement="top"
                    title="Descargar ">
                    <i className="fa fa-cloud-download"></i>
                </button>
            </div>
        );
    };
    render() {
        return (
            <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl
                    ref={el => {
                        this.growl = el;
                    }}
                    baseZIndex={1000}
                />
                <div align="right">
                    <button
                        type="button"
                        className="btn btn-outline-primary"
                        icon="fa fa-fw fa-ban"
                        onClick={this.mostrarForm}
                    >
                        Nuevo
          </button>
                </div>
                <br />
                <div>
                    <BootstrapTable data={this.state.data} hover={true} pagination={true}>
                        <TableHeaderColumn
                            dataField="any"
                            dataFormat={this.Secuencial}
                            width={"8%"}
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                        >
                            Nº
            </TableHeaderColumn>
                        <TableHeaderColumn
                            dataField="nombre"
                            width={"12%"}
                            tdStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                            filter={{ type: "TextFilter", delay: 500 }}
                            dataSort={true}
                        >
                            Descripción
            </TableHeaderColumn>


                        <TableHeaderColumn
                            dataField="Id"
                            isKey
                            width={"10%"}
                            dataFormat={this.generarBotones}
                            thStyle={{ whiteSpace: "normal", textAlign: "center" }}
                        >
                            Opciones
            </TableHeaderColumn>
                    </BootstrapTable>
                </div>

                <Dialog
                    header="Nuevo"
                    visible={this.state.visible}
                    onHide={this.OcultarFormulario}
                    modal={true}
                    style={{ width: '500px', overflow: 'auto' }}
                >
                    <div>
                        <form onSubmit={this.EnviarFormulario}>
                            <div className="row">
                                <div className="col">
                                    {this.state.editable &&
                                        <Field
                                            name="uploadFile"
                                            label="Archivo Presupuesto"
                                            type={"file"}
                                            edit={true}
                                            readOnly={false}
                                            onChange={this.handleChange}
                                            error={this.state.errors.uploadFile}
                                        />
                                    }
                                   
                                </div>
                            </div>

                            <button type="submit" className="btn btn-outline-primary">
                                Guardar
              </button>
                            &nbsp;
              <button
                                type="button"
                                className="btn btn-outline-primary"
                                icon="fa fa-fw fa-ban"
                                onClick={this.OcultarFormulario}
                            >
                                Cancelar
              </button>
                        </form>
                    </div>
                </Dialog>

            </BlockUi>
        )
    }

    onDownloaImagen = Id => {

        return (
            window.location = `/Proyecto/OfertaPresupuesto/DescargarArchivo/${Id}`
        );
    }

    EnviarFormulario(event) {
        event.preventDefault();

        if (!this.isValid()) {
            abp.notify.error(
                "No ha ingresado los campos obligatorios  o existen datos inválidos.",
                "Validación"
            );
            return;
        } else {
            if (this.state.action == 'create') {
                const formData = new FormData();
                formData.append('descripcion', this.state.descripcion);
                formData.append('Id', this.props.OfertaId);
                formData.append('UploadedFile', this.state.uploadFile);
                const multipart = {
                    headers: {
                        'content-type': 'multipart/form-data'
                    }
                };
                axios
                    .post("/Proyecto/OfertaPresupuesto/CreateArchivo",formData ,multipart)
                    .then(response => {

                        if (response.data == "OK") {
                            this.props.showSuccess("Archivo Guardado Correctamente");
                            this.setState({visible:false});
                            this.ObtenerArchivos();
                        }else{

                            this.props.showWarn("Ocurrió un Error");
                        }
                    })
                    .catch(error => {
                        console.log(error);
                        this.props.showWarn("Ocurrió un Error");
                         });

            }
            else {
           
            }

        }
    }
    
    MostrarFormulario() {
        this.setState({ visible: true });
    }

    handleChange(event) {


        if (event.target.files) {

            console.log(event.target.files)
            let files = event.target.files || event.dataTransfer.files;
            if (files.length > 0) {
                let uploadFile = files[0];
                this.setState({
                    uploadFile: uploadFile
                });
            }

        } else {

            this.setState({ [event.target.name]: event.target.value });

        }
    }

    OcultarFormulario = () => {
        this.setState({ visible: false });

    }


 }
