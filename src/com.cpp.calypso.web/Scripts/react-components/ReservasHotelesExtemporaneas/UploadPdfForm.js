import React from "react";
import { FileUpload } from "primereact-v2/fileupload";

export default class UploadPdfForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            uploadFile: '',
            errors: {}
        }
    }


    render() {
        return (
            <div>
                <FileUpload
                    name="uploadFile"
                    chooseLabel="Seleccionar"
                    cancelLabel="Cancelar"
                    uploadLabel="Crear Reserva Extemporánea"
                    onUpload={this.props.handleChange}
                    multiple={true}
                    accept="file_extension|media_type"
                    maxFileSize={1000000}
                    url=""
                />
            </div>
        )
    }
}