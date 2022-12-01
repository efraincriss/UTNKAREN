import React from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import Field from "./Base/Field-v2";
import wrapForm from "./Base/BaseWrapper";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Growl } from "primereact/components/growl/Growl";
import { FileUpload } from "primereact/components/fileupload/FileUpload";
import BlockUi from "react-block-ui";
import RdoForm from "./RdoForm";

class RdoPresupuesto extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visible: false,
      message: "",
      blocking: false,
      file: null
    };

    this.successMessage = this.successMessage.bind(this);
    this.warnMessage = this.warnMessage.bind(this);
    this.showForm = this.showForm.bind(this);
    this.onHide = this.onHide.bind(this);
    this.handleBlocking = this.handleBlocking.bind(this);
    this.showSuccess = this.showSuccess.bind(this);
    this.showWarn = this.showWarn.bind(this);
    this.onBasicUpload = this.onBasicUpload.bind(this);
  }

  componentDidMount() {
    this.props.unlockScreen();
  }
  render() {
    return (
        <BlockUi tag="div" blocking={this.state.blocking}>
        <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
      <div>
        <button onClick={this.showForm} className="btn btn-outline-primary">
          Generar Rdo
        </button>
        <Dialog
          header="Generar Base Rdo"
          visible={this.state.visible}
          width="350px"
          modal={true}
          onHide={this.onHide}
        >
          <RdoForm
            onHide={this.onHide}
            showWarn={this.showWarn}
            showSuccess={this.showSuccess}
            handleBlocking={this.handleBlocking}
          />
        </Dialog>
      </div>
      </BlockUi>
    );
  }
  handleBlocking(){
    this.setState({blocking: !this.state.blocking})
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
          "content-type": "multipart/form-data"
        }
      };
      axios
        .post("/proyecto/Computo/SubirExcelEAC/", formData, config)
        .then(response => {
          console.log(response);
          this.successMessage("Cargado Correctamente");
          this.setState({ blocking: false, visible: false });
        })
        .catch(error => {
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
      detail: this.state.message
    });
  }

  showWarn() {
    this.growl.show({
      life: 5000,
      severity: "error",
      summary: "Error",
      detail: this.state.message
    });
  }
  successMessage(msg) {
    this.setState({ message: msg }, this.showSuccess);
  }

  warnMessage(msg) {
    this.setState({ message: msg }, this.showWarn);
  }
}
const Container = wrapForm(RdoPresupuesto);
ReactDOM.render(<Container />, document.getElementById("content-rdo"));
