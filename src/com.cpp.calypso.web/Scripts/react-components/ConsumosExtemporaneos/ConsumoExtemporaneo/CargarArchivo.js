import React from "react";
import Field from "../../Base/Field-v2";
import { FileUpload } from "primereact-v2/fileupload";

export default class CargarArchivo extends React.Component {
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
                    mode="basic"
                    chooseLabel="Seleccionar"
                    cancelLabel="Cancelar"
                    uploadLabel="Guardar"
                    
                    onSelect={this.props.handleChange}
                    onClear={this.props.onClear}
                    multiple={true}
                    accept="file_extension|media_type"
                    maxFileSize={1000000}
                />
            </div>
        )
    }
}