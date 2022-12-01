import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import { FileUpload } from "primereact/components/fileupload/FileUpload";
import BlockUi from "react-block-ui";

class SubirExcelEAC extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visible: false,
      message: "",
      blocking: false,
      file: null,
    };

    this.successMessage = this.successMessage.bind(this);
    this.warnMessage = this.warnMessage.bind(this);
    this.showForm = this.showForm.bind(this);
    this.onHide = this.onHide.bind(this);
    this.showSuccess = this.showSuccess.bind(this);
    this.showWarn = this.showWarn.bind(this);
    this.onBasicUpload = this.onBasicUpload.bind(this);
  }

  componentDidMount() {}
  render() {
    return (
      <div>
        <Growl
          ref={(el) => {
            this.growl = el;
          }}
          position="bottomright"
          baseZIndex={1000}
        ></Growl>
        <input
          type="button"
          className="btn btn-outline-primary btn-sm"
          onClick={this.showForm}
          value="Subir Excel EAC"
        />

        <Dialog
          header="Subir Excel EAC"
          visible={this.state.visible}
          width="45%"
          modal={true}
          onHide={this.onHide}
        >
          <BlockUi tag="div" blocking={this.state.blocking}>
            <FileUpload
              name="UploadedFile"
              chooseLabel="Seleccionar"
              cancelLabel="Cancelar"
              uploadLabel="Guardar"
              onUpload={this.onBasicUpload}
              multiple={true}
              accept="file_extension|media_type"
              maxFileSize={1000000}
            />
          </BlockUi>
        </Dialog>
      </div>
    );
  }

  onBasicUpload(event) {
    this.setState({ file: event.files[0] });

    if (event.files[0] != null) {
      const formData = new FormData();
      formData.append("UploadedFile", event.files[0]);
      formData.append("Id", document.getElementById("OfertaId").className);
      this.setState({ blocking: true });
      const config = {
        headers: {
          "content-type": "multipart/form-data",
        },
      };

      axios
        .post("/proyecto/Computo/SubirExcelEAC/", formData, config)
        .then((response) => {
          console.log(response);
          this.successMessage("Cargado Correctamente");
          this.setState({ blocking: false, visible: false });
        })
        .catch((error) => {
          console.log(error);
          this.warnMessage("Error Subida");
          this.setState({ blocking: false });
        });
    } else {
      console.log("error llamada");
      this.setState({ blocking: false });
    }
  }

  showForm() {
    this.setState({ visible: true });
  }

  onHide(event) {
    this.setState({ visible: false });
  }
  showSuccess() {
    this.growl.show({
      life: 5000,
      severity: "success",
      summary: "",
      detail: this.state.message,
    });
  }

  showWarn() {
    this.growl.show({
      life: 5000,
      severity: "error",
      summary: "Error",
      detail: this.state.message,
    });
  }
  successMessage(msg) {
    this.setState({ message: msg }, this.showSuccess);
  }

  warnMessage(msg) {
    this.setState({ message: msg }, this.showWarn);
  }
}

ReactDOM.render(
  <SubirExcelEAC />,
  document.getElementById("content-subir-excel")
);
