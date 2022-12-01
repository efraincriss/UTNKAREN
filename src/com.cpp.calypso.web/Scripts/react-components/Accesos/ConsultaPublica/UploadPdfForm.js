import React from "react";
import Field from "../../Base/Field-v2";
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
                    uploadLabel="Guardar"
                    
                    onUpload={this.props.handleChange}
                    multiple={true}
                    accept="file_extension|media_type"
                    maxFileSize={1000000}
                />
            </div>
        )
    }
}