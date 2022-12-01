import React from "react";
import Field from "../../../../Base/Field-v2";
import moment from 'moment';
import config from "../../../../Base/Config";
import {
    FRASE_ERROR_FECHA_VENCIMIENTO,
    FRASE_ERROR_OBSERVACION,
    FRASE_ERROR_ARCHIVO
} from "../../../../Base/Strings";

export default class AnularTarjetaForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            formData: {
                Id: props.tarjeta.Id ? props.tarjeta.Id : 0,
                fecha_vencimiento: moment().format(config.formatDate),
                observaciones: props.tarjeta.observaciones ? props.tarjeta.observaciones : '',
            },
            errors: {},
            uploadFile: '',

        }
    }


    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit}>
                    <div className="row">
                        <div className="col">
                            <Field
                                name="observaciones"
                                label="Observaciones"
                                required
                                edit={true}
                                readOnly={false}
                                value={this.state.formData.observaciones}
                                onChange={this.handleChange}
                                error={this.state.errors.observaciones}
                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="fecha_vencimiento"
                                label="Fecha Vencimiento"
                                required
                                type="date"
                                edit={true}
                                readOnly={false}
                                value={this.state.formData.fecha_vencimiento}
                                onChange={this.handleChange}
                                error={this.state.errors.fecha_vencimiento}
                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="uploadFile"
                                label="Documento de Respaldo"
                                required
                                type={"file"}
                                edit={true}
                                readOnly={false}
                                onChange={this.handleChange}
                                error={this.state.errors.uploadFile}
                            />
                        </div>
                    </div>

                    <button type="submit" className="btn btn-outline-primary">Guardar</button>&nbsp;
                </form>
            </div>
        )
    }

    normalizingDataSubmit = (dataEntity) => {
        //Add File
        dataEntity.uploadFile = this.state.uploadFile;
    }

    handleSubmit = (event) => {
        event.preventDefault();
        if (this.state.formData.fecha_vencimiento === '') {
            this.props.showWarn(FRASE_ERROR_FECHA_VENCIMIENTO);
        } else if (this.state.formData.observaciones === '') {
            this.props.showWarn(FRASE_ERROR_OBSERVACION);
        } else {
            let data = Object.assign({}, this.state.formData);
            this.normalizingDataSubmit(data);
            console.log(data);

            const formData = new FormData();
            for (var key in data) {
                if (data[key] !== null)
                    formData.append(key, data[key]);
                else
                    formData.append(key, '');
            }
            console.log(formData)

            this.props.anularTarjeta(formData);
        }
    }




    resetValues = () => {
        this.setState({
            formData: {
                Id: 0,
                fecha_vencimiento: moment().format(config.formatDate),
                observaciones: '',
            },
            errors: {},
            uploadFile: '',
        })
    }

    handleChange = (event) => {
        const target = event.target;
        if (event.target.files) {
            let files = event.target.files || event.dataTransfer.files;
            if (files.length > 0) {
                let uploadFile = files[0];
                this.setState({
                    uploadFile: uploadFile
                });
            }

        } else {
            const value = target.type === "checkbox" ? target.checked : target.value;
            const name = target.name;

            const updatedData = {
                ...this.state.formData
            };

            updatedData[name] = value;

            this.setState({
                formData: updatedData
            });
        }

    }

}