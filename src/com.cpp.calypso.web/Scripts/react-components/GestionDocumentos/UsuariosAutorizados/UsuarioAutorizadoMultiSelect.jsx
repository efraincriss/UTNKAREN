import React from "react"
import Field from "../../Base/Field-v2"
import { Button } from "primereact-v2/button"
import { CONTROLLER_USUARIO_AUTORIZADO, FRASE_USUARIOS_ASIGNADOS, MODULO_DOCUMENTOS } from "../../Base/Strings"
import http from "../../Base/HttpService";

export class UsuariosAutorizadoMultiSelect extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      data: {},
      errors: {},
    }
  }

  render() {
    return (
      <div className="row align-items-center">
        <div className="col">
          <Field
            name="Users"
            value={this.state.data.Users}
            defaultLabel="Seleccione.."
            label="Seleccione usuarios a autorizar: "
            options={this.props.usuariosDisponibles}
            type={"MULTI-SELECT"}
            onChange={this.onDropdownChangeValue}
            error={this.state.errors.Users}
            readOnly={false}
            placeholder="Seleccione.."
            fixedPlaceholder="Seleccione.."
            filter={true}
          />
        </div>
        <div className="col-3">
          <Button
            label="Agregar"
            className="p-button-outlined"
            onClick={() => this.handleSubmit()}
            icon="pi pi-plus"
          />
        </div>
      </div>
    )
  }

  handleSubmit = () => {
    this.props.blockScreen();
        
        var body = {
            usuarios: this.state.data.Users,
            carpetaId: this.props.contratoId
        }
        console.log(body)
        let url = '';
        url = `/${MODULO_DOCUMENTOS}/${CONTROLLER_USUARIO_AUTORIZADO}/CrearUsuarioAutorizados`
        http.post(url, body)
            .then((response) => {
                let data = response.data;
                if (data.success === true) {
                    this.setState({ data: {} })
                    this.props.showSuccess(FRASE_USUARIOS_ASIGNADOS)
                    this.props.consultarDatos();
                } else {
                    var message =  data.result
                    this.props.showWarn(message);
                }
                this.props.unlockScreen();
            })
            .catch((error) => {
                console.log(error)
                this.props.unlockScreen();
            })
  }

  onDropdownChangeValue = (name, value) => {
    const data = {
      ...this.state.data,
      [name]: value,
    }
    this.setState({
      data,
    })
  }
}
