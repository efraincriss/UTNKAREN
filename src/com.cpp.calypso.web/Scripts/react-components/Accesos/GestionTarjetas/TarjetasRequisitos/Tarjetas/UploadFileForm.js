import React from "react";
import Field from "../../../../Base/Field-v2";
import {
    FRASE_ERROR_GLOBAL,
    FRASE_ARCHIVO_SUBIDO,
    MODULO_ACCESO,
    CONTROLLER_TARJETA_ACCESO
} from "../../../../Base/Strings";
import http from "../../../../Base/HttpService";
import config from "../../../../Base/Config";

export default class UploadFileForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            uploadFile: '',
            errors:{},
        }
    }


    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit}>
                    <div className="row">
                        <div className="col">
                            <Field
                                name="uploadFile"
                                label={this.props.label}
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

    handleSubmit = event => {
        event.preventDefault();
        this.props.blockScreen();

        let url = '';
       // url = `${config.appUrl}${MODULO_ACCESO}/${CONTROLLER_TARJETA_ACCESO}/SubirArchivo/${this.props.tarjetaId}`
       url = `/Accesos/TarjetaAcceso/SubirArchivo/${this.props.tarjetaId}`

        const formData = new FormData();
        formData.append('uploadFile', this.state.uploadFile);
 
        const configuration = {
            headers: {
                'content-type': 'multipart/form-data'
            }
        };

        http.post(url, formData, configuration)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({
                        uploadFile: ''
                    });
                    this.props.reloadTarjetas();
                    this.props.showSuccess(FRASE_ARCHIVO_SUBIDO)
                    this.props.ocultarUploadDialog()
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.props.showWarn(message);
                    this.props.unlockScreen();
                }
                
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn(FRASE_ERROR_GLOBAL);
                this.props.unlockScreen();
            });

    }


    handleChange = event => {
        const target = event.target;
        if (event.target.files) {
            let files = event.target.files || event.dataTransfer.files;
            if (files.length>0) {
                let uploadFile = files[0];
                this.setState({
                    uploadFile: uploadFile
                });
            }          
        } 
    }
}