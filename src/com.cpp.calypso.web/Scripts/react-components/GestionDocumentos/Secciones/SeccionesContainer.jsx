import React from "react"
import ReactDOM from "react-dom"
import Wrapper from "../../Base/BaseWrapper"
import http from "../../Base/HttpService"
import {
  MODULO_DOCUMENTOS,
  CONTROLLER_USUARIO_AUTORIZADO,
  FRASE_USUARIO_DESASIGNADO,
  FRASE_ERROR_GLOBAL,
  CONTROLLER_SECCION,
  FRASE_SECCION_ELIMINADA,
  CONTROLLER_DOCUMENTO,
} from "../../Base/Strings"
import { Button } from "primereact-v3.3/button"
import { Dialog } from "primereact-v3.3/dialog"
import { Fragment } from "react"
import { Tree } from "primereact-v3.3/components/tree/Tree"
import { ContextMenu } from "primereact-v3.3/contextmenu"
import { SeccionForm } from "./SeccionForm.jsx"
import CabeceraSeccion from "./CabeceraSeccion.jsx"
import axios from "axios"
import { GestionImagenes } from "./GestionImagenes.jsx"
import { ContentEditor } from "./ContentEditor"

import { ReadOnlyEditor } from "./ReadOnlyEditor"
import { ScrollPanel } from 'primereact-v3.3/scrollpanel';
import { Accordion, AccordionTab } from 'primereact-v3.3/accordion';

class SeccionesContainer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      documentoId: 0,
      estructura: [],
      expandedKeys: {},
      documento: {},
      mostrarConfirmacion: false,
      mostrarFormulario: false,
      mostrarImagenes: false,
      seccionSeleccionada: { Contenido: "" },
      menu: [
        {
          label: "Crear Contenido",
          icon: "pi pi-plus",
          command: () => {
            this.mostrarCrearSeccionesHijas()
          },
        },
        {
          label: "Editar",
          icon: "pi pi-pencil",
          command: () => {
            this.mostrarFormulario()
          },
        },
        {
          label: "Subir Imágenes",
          icon: "pi pi-images",
          command: () => {
            this.mostrarFormularioImagenes()
          },
        },
        {
          label: "Eliminar",
          icon: "pi pi-trash",
          command: () => {
            this.mostrarConfirmacionParaEliminar()
          },
        },
      ],
      keyEditor: 123,
      accionSeccion:"nuevo",

    }
  }

  componentWillMount() {
    this.setState(
      {
        documentoId: this.props.getParameterByName("documentoId"),
      },
      () => console.log(this.state.contratoId)
    )
  }

  componentDidMount() {
    this.consultarDatos()
  }

  render() {
    return (
      <div>
        <CabeceraSeccion documento={this.state.documento} />

        <div className="row">
          <div style={{ width: "100%" }}>
            <div className="card">
              <div className="card-body">
                <div className="row" style={{ marginTop: "1em" }}>
                  <div className="col" align="right">
                    <Button
                      type="button"
                      icon="pi pi-plus"
                      label="Nuevo"
                      onClick={() => this.mostrarFormulario({})}
                      className="mr-2"
                    />
                    <Button
                      type="button"
                      icon="pi pi-plus"
                      label="Expandir"
                      onClick={this.expandAll}
                      className="mr-2"
                    />
                    <Button
                      type="button"
                      icon="pi pi-minus"
                      label="Contraer"
                      onClick={this.collapseAll}
                    />
                  </div>
                </div>
                <hr />

                <div className="row" style={{ marginTop: "1em" }}>
                  <div className="col-7">
                    <ContextMenu
                      appendTo={document.body}
                      model={this.state.menu}
                      ref={(el) => (this.cm = el)}
                    />
                    <Tree
                      filter={true}
                      filterMode="strict"
                      style={{ width: "100%" }}
                      value={this.state.estructura}
                      expandedKeys={this.state.expandedKeys}
                      dragdropScope="demo"
                      onToggle={(e) => this.setState({ expandedKeys: e.value })}
                      onContextMenuSelectionChange={(event) =>
                        this.seleccionarNodo(event)
                      }
                      onDragDrop={(event) => this.onDragDrop(event.value)}
                      onContextMenu={(event) =>
                        this.cm.show(event.originalEvent)
                      }
                      contextMenuSelectionKey={(event) => console.log(event)}
                      onSelect={(event) => this.onSelect(event)}
                      selectionMode="single"
                      
                    />
                  </div>
                  <div className="col">
                    <Accordion>
                      <AccordionTab header="Contenido">
                        <ReadOnlyEditor
                          updatedContent={(text) => console.log(texy)}
                          Contenido={this.state.seccionSeleccionada.Contenido}
                          toolbarHidden={true}
                          key={this.state.keyEditor}
                        />
                      </AccordionTab>
                      <AccordionTab header="Imágenes">
                        <ScrollPanel style={{ width: '100%', height: '300px' }}>
                          {this.state.seccionSeleccionada.Contenido != null && this.state.seccionSeleccionada.Contenido.length > 0 && (
                            <div>
                              {this.state.seccionSeleccionada.Imagenes != undefined && this.state.seccionSeleccionada.Imagenes.map((item) => (
                                <div className="card" key={item.Id}>
                                  <img className="card-img-top" src={item.ImagenBase64} alt={item.NombreImagen} key={item.Id}
                                    id={item.Id} />
                                  <div className="card-body" align="center">
                                    <a style={{ marginLeft: '0.3em' }} href={item.ImagenBase64} download={item.NombreImagen} className="btn btn-outline-indigo">   <i className="fa fa-cloud-download"></i></a>

                                    <button
                                      className="btn btn-outline-danger"
                                      style={{ marginLeft: "0.3em" }}
                                      onClick={() => {
                                        if (
                                          window.confirm(
                                            `Esta acción eliminará la imagen registro, ¿Desea continuar?`
                                          )
                                        )
                                          this.eliminarImagen(item);
                                      }}
                                      data-toggle="tooltip"
                                      data-placement="top"
                                      title="Eliminar "
                                    >
                                      <i className="fa fa-trash" />
                                    </button>


                                  </div>
                                </div>

                              ))}
                            </div>
                          )}
                        </ScrollPanel>
                      </AccordionTab>
                    </Accordion>


                  </div>
                </div>

                <Dialog
                  header="Gestión de Secciones"
                  modal={true}
                  
                  visible={this.state.mostrarFormulario}
                  style={{ width: "1000px" }}
                  onHide={this.onHideFormulario}
                >
                 <ScrollPanel style={{width: '100%', height: '480px'}}>
                    <SeccionForm
                      seccion={this.state.seccionSeleccionada}
                      actualizarSeccionSeleccionada={
                        this.actualizarSeccionSeleccionada
                      }
                      onHideFormulario={this.onHideFormulario}
                      showSuccess={this.props.showSuccess}
                      showWarn={this.props.showWarn}
                      blockScreen={this.props.blockScreen}
                      unlockScreen={this.props.unlockScreen}
                      documentoId={this.state.documentoId}
                      accionSeccion={this.state.accionSeccion}
                      consultarDatos={this.consultarDatos}
                    />
                 </ScrollPanel>
                </Dialog>
                <Dialog
                  header="Gestión de Imágenes"
                  modal={true}
                  visible={this.state.mostrarImagenes}
                  style={{ width: "850px" }}
                  onHide={this.onHideImagenes}
                >
                  <GestionImagenes
                    onHideFormulario={this.onHideImagenes}
                    showSuccess={this.props.showSuccess}
                    showWarn={this.props.showWarn}
                    blockScreen={this.props.blockScreen}
                    unlockScreen={this.props.unlockScreen}
                    SeccionId={this.state.seccionSeleccionada.Id}
                  />
                </Dialog>

                <Dialog
                  header="Confirmación"
                  visible={this.state.mostrarConfirmacion}
                  modal={true}
                  style={{ width: "500px" }}
                  footer={this.construirBotonesDeConfirmacion()}
                  onHide={this.onHideFormulario}
                >
                  <div className="confirmation-content">
                    <div className="p-12">
                      <i
                        className="pi pi-exclamation-triangle p-mr-3"
                        style={{ fontSize: "2rem" }}
                      />
                      <p>
                        Se eliminará la sección seleccionada{" "}
                        {this.state.seccionSeleccionada.NommbreSeccion}, está
                        seguro presione <b>ELIMINAR</b>, caso contrato{" "}
                        <b>CANCELAR</b>
                      </p>
                    </div>
                  </div>
                </Dialog>
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }

  obtenerEstructuraArbol = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_SECCION}/ObtenerEstructuraArbol/${this.state.documentoId}`
    return http.get(url)
  }

  obtenerSeccionPorId = (seccionId) => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_SECCION}/ObtenerSeccionPorId/${seccionId}`
    return http.get(url)
  }

  obtenerDocumento = () => {
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_DOCUMENTO}/ObtenerDetallesDocumento/${this.state.documentoId}`
    return http.get(url)
  }

  consultarDatos = () => {
    this.props.blockScreen()
    var self = this
    Promise.all([this.obtenerEstructuraArbol(), this.obtenerDocumento()])
      .then(function ([estructura, documento]) {
        self.setState(
          {
            estructura: estructura.data.result,
            documento: documento.data.result,
            keyEditor: Math.random(),
          },
          self.props.unlockScreen
        )
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  onDragDrop = (arbol) => {
    let this_=this;
    abp.message.confirm(
      'Esta acción reubicará la sección.',
      '¿Está seguro?',
      function (isConfirmed) {
          if (isConfirmed) {
            this_.setState(
              {
                estructura: arbol,
              },
              
              this_.guardarArbol(arbol)
            )
          }
      }
  );

   
  }

  guardarArbol = (arbol) => {
    console.clear();
    console.log('estructura',arbol);
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_SECCION}/GuardarArbol/`

    axios
      .post(url, {
        arbol: arbol,
      })
      .then((response) => {
        let data = response.data
        if (data.success === true) {
          this.props.unlockScreen();
         // this.obtenerEstructuraArbol();
        } else {
          var message = data.result
          this.props.showWarn(message)
          this.props.unlockScreen()
        }
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(FRASE_ERROR_GLOBAL)
        this.props.unlockScreen()
      })
  }

  eliminarUsuarioAsignado = () => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_SECCION}/EliminarSeccion/${this.state.seccionSeleccionada.Id}`

    http
      .delete(url, {})
      .then((response) => {
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_SECCION_ELIMINADA)
          this.onHideFormulario(true)
        } else {
          var message = data.result
          this.props.showWarn(message)
          this.props.unlockScreen()
        }
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(FRASE_ERROR_GLOBAL)
        this.props.unlockScreen()
      })
  }
  eliminarImagen = (item) => {
    this.props.blockScreen()
    let url = ""
    url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_SECCION}/EliminarImagen/${item.Id}`

    http
      .delete(url, {})
      .then((response) => {
        let data = response.data
        if (data.success === true) {
          this.props.showSuccess(FRASE_SECCION_ELIMINADA)
          this.consultarDatosSeccion(item.SeccionId);
        } else {
          var message = data.result
          this.props.showWarn(message)
          this.props.unlockScreen()
        }
      })
      .catch((error) => {
        console.log(error)
        this.props.showWarn(FRASE_ERROR_GLOBAL)
        this.props.unlockScreen()
      })
  }
  mostrarFormulario = (seccion) => {
    if (seccion) {
      this.setState({
        seccionSeleccionada: seccion,
        mostrarFormulario: true,
        accionSeccion:"seleccion"
      })

      console.log('Editars')
    } else {
      this.setState({
        mostrarFormulario: true,
        accionSeccion:"nuevo"
      })
    }
  }

  onHideImagenes = () => {
    this.setState({
      mostrarImagenes: false,
      seccionSeleccionada: {},
    })
  }

  mostrarFormularioImagenes = () => {
    this.setState({
      mostrarImagenes: true,
    })
  }

  mostrarConfirmacionParaEliminar = () => {
    this.setState({
      mostrarConfirmacion: true,
    })
  }

  onHideFormulario = (recargar = false) => {
    this.setState({
      seccionSeleccionada: {},
      mostrarFormulario: false,
      mostrarConfirmacion: false,
    })
    if (recargar) {
      this.consultarDatos()
    }
  }

  construirBotonesDeConfirmacion = () => {
    return (
      <Fragment>
        <Button
          label="Eliminar"
          className="p-button-danger p-button-outlined"
          onClick={() => this.eliminarUsuarioAsignado()}
          icon="pi pi-save"
        />
        <Button
          style={{ marginLeft: "0.4em" }}
          label="Cancelar"
          className="p-button-outlined"
          onClick={() => this.onHideFormulario()}
          icon="pi pi-ban"
        />
      </Fragment>
    )
  }

  collapseAll = () => {
    this.setState({ expandedKeys: {} })
  }

  expandAll = () => {
    let expandedKeys = {}
    for (let node of this.state.estructura) {
      this.expandNode(node, expandedKeys)
    }

    this.setState({ expandedKeys })
  }

  expandNode(node, expandedKeys) {
    if (node.children && node.children.length) {
      expandedKeys[node.key] = true

      for (let child of node.children) {
        this.expandNode(child, expandedKeys)
      }
    }
  }

  collapseAll() {
    this.setState({ expandedKeys: {} })
  }

  mostrarCrearSeccionesHijas = () => {
    const SeccionPadreId = this.state.seccionSeleccionada.Id
    this.setState({
      seccionSeleccionada: { SeccionPadreId },
      mostrarFormulario: true,
    })
  }

  seleccionarNodo = (event) => {
    this.consultarDatosSeccion(event.value)
  }

  onSelect = (event) => {
    this.consultarDatosSeccion(event.node.key)
  
  }

  consultarDatosSeccion = (id) => {
    console.log('COnsultando DAtos')
    this.props.blockScreen()
    var self = this
    Promise.all([this.obtenerSeccionPorId(id)])
      .then(function ([seccion]) {
        self.setState(
          {
            seccionSeleccionada: seccion.data.result,
            accionSeccion:"seleccion"
            
          },
          self.props.unlockScreen
        )
      })
      .catch((error) => {
        self.props.unlockScreen()
        console.log(error)
      })
  }

  actualizarSeccionSeleccionada = (seccion) => {
    this.setState({ seccionSeleccionada: seccion })
  }
}

const Container = Wrapper(SeccionesContainer)
ReactDOM.render(
  <Container />,
  document.getElementById("gestion_secciones_container")
)
