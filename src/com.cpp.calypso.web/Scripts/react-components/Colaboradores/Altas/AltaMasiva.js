import React from 'react';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import moment from 'moment';
export default class Alta extends React.Component {

    constructor(props) {

        super(props);
        this.state = {
            errores: [],
            id_empleado: '',
            meta4: '',
            archivo: [],
            lista_errores: [],
        }

        this.handleChange = this.handleChange.bind(this);
        this.DescargarFormato = this.DescargarFormato.bind(this);
        this.onBasicUploadAuto = this.onBasicUploadAuto.bind(this);
        this.EnvioAltaMasiva = this.EnvioAltaMasiva.bind(this);
        this.clearCarga = this.clearCarga.bind(this);
    }

    componentDidMount() {
    }


    render() {
        return (
            <div>
                <BlockUi tag="div" blocking={this.state.loading}>
                    <div className="row">
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="text"><b>Descargar Formato Excel</b></label><br />
                                <button type="button" onClick={() => this.DescargarFormato()} className="btn btn-outline-primary btn-sm">Descargar</button>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <div className="form-group">
                                <label htmlFor="text"><b>Subir Excel Alta Masiva</b></label>
                                <br />
                                <input type="file" id="fileAltaMasiva" accept=".xls,.xlsx" onChange={(e) => this.onBasicUploadAuto(e)} key={''} />
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            <br />
                            {this.ListaErrores()}
                            <br />
                        </div>
                    </div>


                    <div className="row">
                        <div className="form-group col">
                            <button type="button" onClick={() => this.EnvioAltaMasiva()} className="btn btn-outline-primary">Subir Excel</button>
                            <button onClick={() => this.props.onHide()} type="button" className="btn btn-outline-primary" style={{ marginLeft: '3px' }}> Cancelar</button>

                        </div>
                    </div>
                </BlockUi>
            </div>
        )
    }

    DescargarFormato() {
        axios.get("/RRHH/Colaboradores/CreateAltaMasiva/", {
            params: {
            },
            responseType: 'arraybuffer',
        })
            .then((response) => {
                this.setState({ loading: false });
                console.log(response)
                console.log(response.data, response.data.byteLength)
                if (response.data.byteLength > 0) {
                    var nombre = response.headers["content-disposition"].split('=');

                    const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
                    const link = document.createElement('a');
                    link.href = url;
                    link.setAttribute('download', nombre[1]);
                    document.body.appendChild(link);
                    link.click();
                    abp.notify.success("Archivo Generado!", "Aviso");
                }else{
                    abp.notify.error('No existen registros!', 'Error');
                }

                
                // this.props.GetColaboradores();
                // this.props.onHide();
            })
            .catch((error) => {
                this.setState({ loading: false });
                
                console.log("ERROR", error);
            });
    }

    onBasicUploadAuto(event) {
        console.log(event.target.files[0])
        var file = event.target.files[0];
        var a = {};
        console.log('type', file.type)
        if (file != null) {
            if (file >= 2 * 1024 * 1024) {
                abp.notify.error('El archivo solo puede ser de máximo 2MB', 'Error');
                this.setState({ archivo: a });
                document.getElementById("fileAltaMasiva").value = "";
                return;
            } else if (!file.type.match('application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') && !file.type.match('application/vnd.ms-excel')) {
                abp.notify.error('No puede subir archivos de ese formato', 'Error');
                // event.target.files = new FileList();
                this.setState({ archivo: a });
                document.getElementById("fileAltaMasiva").value = "";
                return;
            } else {

                const formData = new FormData();
                // const formData = {};
                // formData.append('UploadedFile', event.files[0])
                formData['UploadedFile'] = file;
                //this.setState({blocking: true})
                const config = {
                    headers: {
                        'content-type': 'multipart/form-data'
                    }
                }
                a.file = formData;
                a.config = config;

                // this.state.archivo.push(a);
                this.setState({ archivo: file });
                console.log("formData", formData);

                abp.notify.success("Archivo Procesado con Exito", "Aviso");
                console.log("this.state.archivo", this.state.archivo);

            }

        } else {
            console.log("error llamada");
        }
    }

    EnvioAltaMasiva() {
        console.log(this.state.archivo);
        if (this.state.archivo.length == 0) {
            abp.notify.error('Seleccione un archivo!', 'Error');
        } else {
            this.setState({ loading: true });
            console.log('lleno');
            const config = { headers: { 'content-type': 'multipart/form-data' } }
            const formData = new FormData();
            formData.append('UploadedFile', this.state.archivo)

            axios.post("/RRHH/Colaboradores/GetExcelAltaMasiva/", formData, config)
                .then((response) => {
                    console.log("DATA ", response.data);
                    if (response.data[0] == "OK") {
                        this.props.onHide();
                        abp.notify.success("Altas Guardadas!", "Aviso");
                        this.clearCarga();
                        this.props.GetColaboradores();
                    } else {
                        this.setState({ lista_errores: response.data });
                        abp.notify.error('Se encontraron errores!', 'Error');
                        this.ListaErrores();
                    }
                    this.setState({ loading: false });
                })
                .catch((error) => {
                    this.setState({ loading: false });
                    console.log("ERROR", error);
                });
        }


    }

    ListaErrores() {
        return (
            this.state.lista_errores.map((item) => {
                return (
                    <div className="alert alert-danger">{item}</div>
                )
            })
        );
    }

    clearCarga() {
        this.setState({
            lista_errores: [],
            archivo: []
        });
        document.getElementById("fileAltaMasiva").value = "";
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }
}