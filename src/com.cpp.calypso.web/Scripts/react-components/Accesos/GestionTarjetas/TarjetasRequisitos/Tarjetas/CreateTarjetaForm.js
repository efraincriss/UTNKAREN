import React from "react";
import Field from "../../../../Base/Field-v2";
import moment from 'moment';

import http from "../../../../Base/HttpService";
import dateFormatter from "../../../../Base/DateFormatter";
import {
    FRASE_ERROR_CODIGO,
    FRASE_ERROR_SOLICITUD_PAM,
    FRASE_ERROR_FECHA_EMISION,
    FRASE_ERROR_FECHA_VENCIMIENTO,
    FRASE_ERROR_FECHAS
} from "../../../../Base/Strings";

export default class CreateTarjetaForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            formData: {
              
                solicitud_pam: '',
                fecha_emision:  moment(new Date()).format("YYYY-MM-DD"),
                fecha_vencimiento:moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))),
                entregada: false,
                observaciones: '',
                estado: 1,
                ColaboradorId: 0,
            },
            errors: {}

        }

    }
    

    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit}>
                    <div className="row">
                        <div className="col">
                            <Field
                                name="solicitud_pam"
                                label="N. Solicitud Pam"
                                required
                                edit={true}
                                readOnly={false}
                                value={this.state.formData.solicitud_pam}
                                onChange={this.handleChange}
                                error={this.state.errors.solicitud_pam}
                            />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            <Field
                                name="fecha_emision"
                                label="Fecha Emisión"
                                required
                                type="date"
                                edit={true}
                                readOnly={false}
                                value={this.state.formData.fecha_emision}
                                onChange={this.handleChange}
                                error={this.state.errors.fecha_emision}
                            />
                        </div>
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
                                name="entregada"
                                label="Entregada?"
                                labelOption="(Si/No)"
                                type={"checkbox"}
                                edit={true}
                                readOnly={false}
                                value={this.state.formData.entregada}
                                onChange={this.handleChange}
                                error={this.state.errors.entregada}
                            />
                        </div>
                    </div>

                    <button type="submit" className="btn btn-outline-primary">Guardar</button>&nbsp;
                </form>
            </div>
        )
    }

    handleSubmit = (event) => {
        event.preventDefault();
        if (this.state.formData.solicitud_pam === '') {
            this.props.showWarn(FRASE_ERROR_SOLICITUD_PAM);
        } else if (this.state.formData.fecha_emision === '') {
            this.props.showWarn(FRASE_ERROR_FECHA_EMISION);
        } else if (this.state.formData.fecha_vencimiento === '') {
            this.props.showWarn(FRASE_ERROR_FECHA_VENCIMIENTO);
        }
        else if (this.state.formData.fecha_vencimiento < this.state.formData.fecha_emision) {
            this.props.showWarn(FRASE_ERROR_FECHAS);
        }
        else {
            this.props.crearTarjeta(this.state.formData);
        }
    }




    resetValues = () => {
        this.setState({
            formData: {
            
                solicitud_pam: '',
                fecha_emision:  moment(new Date()).format("YYYY-MM-DD"),
                fecha_vencimiento:moment(new Date(new Date().setFullYear(new Date().getFullYear() + 1))),
                 entregada: false,
                observaciones: '',
                estado: 1,
                ColaboradorId: 0,
            },
            errors: {}
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

        } 
        else 
        {
            const value = target.type === "checkbox" ? target.checked : target.value;
            const name = target.name;

            const updatedData = {
                ...this.state.formData
            };

            if(name=="fecha_emision"){
            updatedData[name] = value;
            
            updatedData["fecha_vencimiento"] = moment(new Date(new Date(value).setFullYear(new Date(value).getFullYear() + 1))); 
            }else{
            updatedData[name] = value;
            }
            this.setState({
                formData: updatedData,

                
            });
        }

    }

    onChangeValue = (name, value) => {
        this.setState({
            [name]: value
        });
    }



}