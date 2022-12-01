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
                    uploadLabel="Guardar"
                    onUpload={this.props.handleChange}
                    multiple={true}
                    accept=".xlsx,.xls"
                    
                />
            </div>
        )
    }
}