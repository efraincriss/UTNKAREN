import React, { Component } from "react"
import { FileUpload } from "primereact-v3.3/fileupload"
import { ProgressBar } from "primereact-v3.3/progressbar"
import { Button } from "primereact-v3.3/button"
import { Tooltip } from "primereact-v3.3/tooltip"
import {
  FRASE_ERROR_GLOBAL,
  FRASE_IMAGEN_SUBIDA,
  MODULO_DOCUMENTOS,
  CONTROLLER_IMAGEN_SECCION,
} from "../../Base/Strings"
import config from "../../Base/Config"
import http from "../../Base/HttpService"
import axios from "axios"

export class GestionImagenes extends Component {
  constructor(props) {
    super(props)

    this.state = {
      totalSize: 0,
    }

    this.onTemplateRemove = this.onTemplateRemove.bind(this)
    this.onTemplateClear = this.onTemplateClear.bind(this)

    this.headerTemplate = this.headerTemplate.bind(this)
    this.itemTemplate = this.itemTemplate.bind(this)
    this.emptyTemplate = this.emptyTemplate.bind(this)
  }

  onUpload() {
    console.log("Subido")
  }

  fileListToBase64 = async (fileList) => {
    // create function which return resolved promise
    // with data:base64 string
    function getBase64(file) {
      const reader = new FileReader()
      return new Promise(resolve => {
        reader.onload = ev => {
          resolve(ev.target.result)
        }
        reader.readAsDataURL(file)
      })
    }
    // here will be array of promisified functions
    const promises = []
  
    // loop through fileList with for loop
    for (let i = 0; i < fileList.length; i++) {
      promises.push(getBase64(fileList[i]))
    }
  
    // array with base64 strings
    return await Promise.all(promises)
  }

  onTemplateUpload = async (e) => {
    let imagenes = []
    let seccionId = this.props.SeccionId

    /*e.files.forEach((file) => {
      var reader = new FileReader()
      reader.readAsDataURL(file)
      reader.onloadend = function () {
        var base64data = reader.result
        let imagen = {
          NombreImagen: "Imagen.",
          SeccionId: seccionId,
          ImagenBase64: base64data,
        }
        imagenes.push(imagen)
      }
    })
    console.log(imagenes)*/

  
    const arrayOfBase64 = await this.fileListToBase64(e.files)
    console.log('Imagen',arrayOfBase64)

    this.props.blockScreen()
    let url = ""
    url = `${config.apiUrl}${MODULO_DOCUMENTOS}/${CONTROLLER_IMAGEN_SECCION}/CrearImagenes`

    arrayOfBase64.forEach(img => {
      let imagen = {
        NombreImagen: "Imagen.",
        SeccionId: seccionId,
        ImagenBase64: img,
      }
      imagenes.push(imagen)
    });
    let body = {
      input: imagenes,
    }
    console.log('body',body)

    const bodyFormData = new FormData()

    arrayOfBase64.forEach((item) => {
      bodyFormData.append("input[]", item)
      console.log("iterando")
    })

    axios
      .post(url, body)
      .then((response) => {
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_IMAGEN_SUBIDA)
          this.props.onHideFormulario(true);
          this.fileUploadRef.clear();
          
        } else {
         // var message = $.fn.responseAjaxErrorToString(data)
         var message =response.data.errors
          this.props.showWarn(message)
          this.props.unlockScreen()
        }
        this.props.unlockScreen()
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(FRASE_ERROR_GLOBAL)
        this.props.unlockScreen()
      })
  }

  onTemplateRemove(file, callback) {
    this.setState(
      (prevState) => ({
        totalSize: prevState.totalSize - file.size,
      }),
      callback
    )
  }

  onTemplateClear() {
    this.setState({ totalSize: 0 })
  }

  headerTemplate(options) {
    const { className, chooseButton, uploadButton, cancelButton } = options
    const value = this.state.totalSize / 10000
    const formatedValue = this.fileUploadRef
      ? this.fileUploadRef.formatSize(this.state.totalSize)
      : "0 B"

    return (
      <div
        className={className}
        style={{
          backgroundColor: "transparent",
          display: "flex",
          alignItems: "center",
        }}
      >
        {chooseButton}
        {uploadButton}
        {cancelButton}
        <ProgressBar
          value={value}
          displayValueTemplate={() => `${formatedValue} / 1 MB`}
          style={{ width: "300px", height: "20px", marginLeft: "auto" }}
        ></ProgressBar>
      </div>
    )
  }

  itemTemplate(file, props) {
    return (
      <div className="p-d-flex p-ai-center p-flex-wrap">
        <div className="p-d-flex p-ai-center" style={{ width: "40%" }}>
          <img
            alt={file.name}
            role="presentation"
            src={file.objectURL}
            width={100}
          />
          <span className="p-d-flex p-dir-col p-text-left p-ml-3">
            {file.name}
            <small>{new Date().toLocaleDateString()}</small>
          </span>
        </div>
        <Button
          type="button"
          icon="pi pi-times"
          className="p-button-outlined p-button-rounded p-button-danger p-ml-auto"
          onClick={() => this.onTemplateRemove(file, props.onRemove)}
        />
      </div>
    )
  }

  emptyTemplate() {
    return (
      <div className="p-d-flex p-ai-center p-dir-col">
        <i
          className="pi pi-image p-mt-3 p-p-5"
          style={{
            fontSize: "5em",
            borderRadius: "50%",
            backgroundColor: "var(--surface-b)",
            color: "var(--surface-d)",
          }}
        ></i>
        <span
          style={{ fontSize: "1.2em", color: "var(--text-color-secondary)" }}
          className="p-my-5"
        >
          Drag and Drop Image Here
        </span>
      </div>
    )
  }

  myUploader = (event) => {
    //event.files == files to upload
    console.log(event)
    console.log(event.files)
  }

  render() {
    const chooseOptions = {
      icon: "pi pi-fw pi-images",
      iconOnly: true,
      className: "custom-choose-btn p-button-rounded p-button-outlined",
    }
    const uploadOptions = {
      icon: "pi pi-fw pi-cloud-upload",
      iconOnly: true,
      className:
        "custom-upload-btn p-button-success p-button-rounded p-button-outlined",
    }
    const cancelOptions = {
      icon: "pi pi-fw pi-times",
      iconOnly: true,
      className:
        "custom-cancel-btn p-button-danger p-button-rounded p-button-outlined",
    }

    return (
      <div>
        <FileUpload
          ref={(el) => (this.fileUploadRef = el)}
          name="demo[]"
          multiple
          accept="image/*"
          maxFileSize={231000}
          onUpload={this.onTemplateUpload}
          //onSelect={this.onTemplateSelect}
          onError={this.onTemplateClear}
          onClear={this.onTemplateClear}
          
          headerTemplate={this.headerTemplate}
          itemTemplate={this.itemTemplate}
          emptyTemplate={this.emptyTemplate}
          chooseOptions={chooseOptions}
          uploadOptions={uploadOptions}
          cancelOptions={cancelOptions}
          chooseLabel="Seleccionar Imágenes"
          uploadLabel="Subir Imágenes"
          cancelLabel="Cancelar"
          customUpload
          uploadHandler={this.onTemplateUpload}
        />
      </div>
    )
  }
}
