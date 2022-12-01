import React from "react"
import ReactDOM from "react-dom"
import axios from "axios"
import BlockUi from "react-block-ui"
import wrapForm from "./Base/BaseWrapper"
import { Growl } from "primereact/components/growl/Growl"
import { Dialog } from "primereact-v2/dialog"
import UsuariosTable from "./ListasDeDistribucion/UsuariosTable"
import UsuariosForm from "./ListasDeDistribucion/UsuariosForm"
import { CorreosListaOrden } from "./Pos/CorreosListaOrden.jsx"

class ListaDeDistribucion extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      externos: [],
      internos: [],
      blocking: true,

      externos_pendientes: [],
      internos_pendientes: [],

      visible_usuarios: false,
      visible_externos: false,
      mostrarOrdenarLista: false,
      mostrarOrdenarListaExternos: false,
    }
    this.showSuccess = this.showSuccess.bind(this)
    this.showWarn = this.showWarn.bind(this)
    this.GetData = this.GetData.bind(this)
    this.GetCorreosIngresar = this.GetCorreosIngresar.bind(this)
    this.ToggleUsuarios = this.ToggleUsuarios.bind(this)
    this.ToggleExternos = this.ToggleExternos.bind(this)
    this.onHideUsuarios = this.onHideUsuarios.bind(this)
    this.onHideExternos = this.onHideExternos.bind(this)
  }

  componentWillMount() {
    this.props.unlockScreen()
    this.GetData()
    this.GetCorreosIngresar()
  }

  render() {
    return (
      <div>
        <Growl
          ref={(el) => {
            this.growl = el
          }}
          position="bottomright"
          baseZIndex={1000}
        ></Growl>

        <BlockUi tag="div" blocking={this.state.blocking}>
          <div className="row">
            <div style={{ width: "100%" }}>
              <ul className="nav nav-tabs" id="usuarios_tabs" role="tablist">
                <li className="nav-item">
                  <a
                    className="nav-link active"
                    id="usuarios-tab"
                    data-toggle="tab"
                    href="#usuarios"
                    role="tab"
                    aria-controls="home"
                    aria-expanded="true"
                  >
                    Usuarios Internos
                  </a>
                </li>
                <li className="nav-item">
                  <a
                    className="nav-link"
                    id="externos-tab"
                    data-toggle="tab"
                    href="#externos"
                    role="tab"
                    aria-controls="profile"
                  >
                    Usuarios Externos
                  </a>
                </li>
              </ul>

              <div className="tab-content" id="myTabContent">
                <div
                  className="tab-pane fade show active"
                  id="usuarios"
                  role="tabpanel"
                  aria-labelledby="usuarios-tab"
                >
                  <div className="row" align="right">
                    <div className="col" align="right">
                      <button
                        className="btn btn-outline-primary"
                        onClick={this.ToggleUsuarios}
                        style={{ marginLeft: "0.3em" }}
                      >
                        Nuevo
                      </button>
                      <button
                        className="btn btn-outline-primary"
                        style={{ marginLeft: "0.3em" }}
                        onClick={this.toggleOrdenarLista}
                      >
                        Ordenar
                      </button>
                    </div>
                  </div>
                  <hr />

                  <UsuariosTable
                    data={this.state.internos}
                    getData={this.GetData}
                    showSuccess={this.props.showSuccess}
                    showWarning={this.props.showWarning}
                    showWarn={this.props.showWarn}
                    blockScreen={this.props.blockScreen}
                    unlockScreen={this.props.unlockScreen}
                    EliminarCorreosLista={this.EliminarCorreosLista}
                  />
                </div>

                <div
                  className="tab-pane fade"
                  id="externos"
                  role="tabpanel"
                  aria-labelledby="externos-tab"
                >
                  <div className="col" align="right">
                    <button
                      className="btn btn-outline-primary"
                      style={{ marginLeft: "0.3em" }}
                      onClick={this.ToggleExternos}
                    >
                      Nuevo
                    </button>
                    <button
                        className="btn btn-outline-primary"
                        style={{ marginLeft: "0.3em" }}
                        onClick={this.toggleOrdenarListaExternos}
                      >
                        Ordenar
                      </button>
                  </div>
                  <hr />
                  <UsuariosTable
                    showSuccess={this.props.showSuccess}
                    showWarning={this.props.showWarning}
                    showWarn={this.props.showWarn}
                    blockScreen={this.props.blockScreen}
                    unlockScreen={this.props.unlockScreen}
                    data={this.state.externos}
                    getData={this.GetData}
                    EliminarCorreosLista={this.EliminarCorreosLista}
                  />
                </div>
              </div>
            </div>

            <Dialog
              header="Usuario Internos"
              visible={this.state.visible_usuarios}
              width="600px"
              modal={true}
              onHide={this.onHideUsuarios}
            >
              <UsuariosForm
                data={this.state.internos_pendientes}
                showSuccess={this.showSuccess}
                showWarn={this.showWarn}
                updateData={this.GetData}
                onHide={this.onHideUsuarios}
                UpdateCorreos={this.GetCorreosIngresar}
                externo={false}
              />
            </Dialog>

            <Dialog
              header="Usuarios Externos"
              visible={this.state.visible_externos}
              width="600px"
              modal={true}
              onHide={this.onHideExternos}
            >
              <UsuariosForm
                data={this.state.externos_pendientes}
                showSuccess={this.showSuccess}
                showWarn={this.showWarn}
                updateData={this.GetData}
                onHide={this.onHideExternos}
                UpdateCorreos={this.GetCorreosIngresar}
                externo={true}
              />
            </Dialog>
            <Dialog
              header="Ordenar Lista de Distribución"
              visible={this.state.mostrarOrdenarLista}
              width="600px"
              modal={true}
              onHide={this.toggleOrdenarLista}
            >
              <CorreosListaOrden
                unlockScreen={this.props.unlockScreen}
                blockScreen={this.props.blockScreen}
                correos={this.state.internos}
                guardarOrden={this.guardarOrden}
                toggleOrdenarLista={this.toggleOrdenarLista}
                getData={this.GetData}
              />
            </Dialog>
            <Dialog
              header="Ordenar Lista de Distribución Externos"
              visible={this.state.mostrarOrdenarListaExternos}
              width="600px"
              modal={true}
              onHide={this.toggleOrdenarListaExternos}
            >
              <CorreosListaOrden
                unlockScreen={this.props.unlockScreen}
                blockScreen={this.props.blockScreen}
                correos={this.state.externos}
                guardarOrden={this.guardarOrden}
                toggleOrdenarLista={this.toggleOrdenarListaExternos}
                getData={this.GetData}
              />
            </Dialog>
          </div>
        </BlockUi>
      </div>
    )
  }

  toggleOrdenarLista = () => {
    this.setState({ mostrarOrdenarLista: !this.state.mostrarOrdenarLista })
  }

  toggleOrdenarListaExternos = () => {
    this.setState({ mostrarOrdenarListaExternos: !this.state.mostrarOrdenarListaExternos })
  }

  GetData() {
    axios
      .post(
        "/Proyecto/ListaDistribucion/GetCorreosExternosApi/" +
          document.getElementById("content").className,
        {}
      )
      .then((response) => {
        console.log("Externos", response.data)

        this.setState({ externos: response.data })

        axios
          .post(
            "/Proyecto/ListaDistribucion/GetCorreosInternosApi/" +
              document.getElementById("content").className,
            {}
          )
          .then((response) => {
            console.log("Internos", response.data)
            this.props.unlockScreen()
            this.setState({ internos: response.data, blocking: false })
          })
          .catch((error) => {
            console.log(error)
            this.setState({ blocking: false })
          })
      })
      .catch((error) => {
        console.log(error)
        this.setState({ blocking: false })
      })
  }

  GetCorreosIngresar = () => {
    axios
      .post(
        "/Proyecto/ListaDistribucion/GetCorreosExternosParaIngresarApi/" +
          document.getElementById("content").className,
        {}
      )
      .then((response) => {
        console.log(response.data)
        var items = response.data.map((item) => {
          return {
            label: item.nombres + "," + item.correo,
            dataKey: item.Id,
            value: item.Id + "," + item.nombres + "," + item.correo,
          }
        })
        this.setState({ externos_pendientes: items, blocking: false })
      })
      .catch((error) => {
        console.log(error)
      })
    axios
      .post(
        "/Proyecto/ListaDistribucion/GetCorreosInternosParaIngresarApi/" +
          document.getElementById("content").className,
        {}
      )
      .then((response) => {
        console.log(response.data)
        var items = response.data.map((item) => {
          return {
            label: item.nombres + "," + item.correo,
            dataKey: item.Id,
            value: item.Id + "," + item.nombres + "," + item.correo,
          }
        })
        this.setState({ internos_pendientes: items, blocking: false })
      })
      .catch((error) => {
        console.log(error)
      })

    /* axios.post("/Proyecto/ListaDistribucion/GetCorreosExternosParaIngresarApi/" + document.getElementById('content').className, {})
             .then((response) => {
                   this.setState({ externos_pendientes: response.data })
                 axios.post("/Proyecto/ListaDistribucion/GetCorreosInternosParaIngresarApi/" + document.getElementById('content').className, {})
                     .then((response) => {
                         this.setState({ internos_pendientes: response.data })
                     })
                     .catch((error) => {
                         console.log(error);
                         this.setState({ blocking: false })
                     });
 
             })
             .catch((error) => {
                 console.log(error);
                 this.setState({ blocking: false })
             });
 
             console.log(this.state.externos_pendientes)
             console.log(this.state.internos_pendientes)*/
  }

  EliminarCorreosLista = (Id) => {
    console.log(Id)
    axios
      .post("/Proyecto/ListaDistribucion/GetEliminar/" + Id, {})
      .then((response) => {
        console.log(response.data)
        if (response.data === "OK") {
          this.GetData()
        } else {
          this.showWarn()
        }
      })
      .catch((error) => {
        console.log(error)
      })
  }

  ToggleUsuarios() {
    this.setState({ visible_usuarios: !this.state.visible_usuarios })
  }

  ToggleExternos() {
    this.setState({ visible_externos: !this.state.visible_externos })
  }

  onHideUsuarios() {
    this.setState({ visible_usuarios: false })
  }

  onHideExternos() {
    this.setState({ visible_externos: false })
  }
  showSuccess() {
    this.growl.show({
      life: 5000,
      severity: "success",
      summary: "Proceso exitoso!",
      detail: "La operación se realizó con éxito",
    })
  }

  showWarn() {
    this.growl.show({
      life: 5000,
      severity: "error",
      summary: "Error",
      detail: "Ocurrio un inconveniente",
    })
  }
}
const Container = wrapForm(ListaDeDistribucion)
ReactDOM.render(<Container />, document.getElementById("content"))
