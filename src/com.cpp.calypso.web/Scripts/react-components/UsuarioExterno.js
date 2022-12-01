import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import { Growl } from 'primereact/components/growl/Growl';
import UsuarioExternoTable from './UsuarioExterno/UsuarioExternoTable';
import CrearUsuarioExterno from './UsuarioExterno/CrearUsuarioExterno';
import EditarUsuarioExterno from './UsuarioExterno/EditarUsuarioExterno';
import CrearVisita from './UsuarioExterno/CrearVisita';

export default class UsuarioExterno extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            table: true,
            crear: false,
            editar: false,
            visita: false,
            nacionalidades: [],
        }

        this.childCrear = React.createRef();
        this.childEditar = React.createRef();
        this.childVisita = React.createRef();
        this.childTable = React.createRef();
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.click = this.click.bind(this);

        this.Regresar = this.Regresar.bind(this);
        this.Siguiente = this.Siguiente.bind(this);

        this.GetNacionalidades = this.GetNacionalidades.bind(this);
        this.getFormSelectNacionalidad = this.getFormSelectNacionalidad.bind(this);

        this.validarCedula = this.validarCedula.bind(this);
        this.VerificaCedula = this.VerificaCedula.bind(this);
    }

    componentDidMount() {
        this.click();
        this.GetNacionalidades();
    }

    render() {
        return (
            <div className="row">
                <div className="col">
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    <div className="tab-content" id="myTabContent" style={{ border: 'none' }}>
                        <div className={this.state.table == true ? "tab-pane fade show active" : "tab-pane fade show"} id="table" role="tabpanel">
                            <UsuarioExternoTable ref={this.childTable}
                                successMessage={this.successMessage}
                                warnMessage={this.warnMessage}
                                Siguiente={this.Siguiente}
                                validarCedula={this.validarCedula}
                            />
                        </div>
                        <div className={this.state.crear == true ? "tab-pane fade show active" : "tab-pane fade show"} id="crear" role="tabpanel">
                            <CrearUsuarioExterno ref={this.childCrear}
                                Regresar={this.Regresar}
                                successMessage={this.successMessage}
                                warnMessage={this.warnMessage}
                                getFormSelectNacionalidad={this.getFormSelectNacionalidad}
                                validarCedula={this.validarCedula}
                            />
                        </div>
                        <div className={this.state.editar == true ? "tab-pane fade show active" : "tab-pane fade show"} id="crear" role="tabpanel">
                            <EditarUsuarioExterno ref={this.childEditar}
                                Regresar={this.Regresar}
                                successMessage={this.successMessage}
                                warnMessage={this.warnMessage}
                                getFormSelectNacionalidad={this.getFormSelectNacionalidad}
                                validarCedula={this.validarCedula}
                            />
                        </div>
                        <div className={this.state.visita == true ? "tab-pane fade show active" : "tab-pane fade show"} id="crear" role="tabpanel">
                            <CrearVisita ref={this.childVisita}
                                Regresar={this.Regresar}
                                successMessage={this.successMessage}
                                warnMessage={this.warnMessage}
                            />
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    click() {
        var a = document.createElement("button")
        a.setAttribute("class", "btn btn-primary pull-right fa fa-plus");
        a.setAttribute("id", "nuevo_usuario");
        a.setAttribute("type", "button");
        a.addEventListener("click", e => this.Siguiente(1))
        // a.attachEvent('onClick',this.showFormEnvioManual);
        a.textContent = " Nuevo";
        document.getElementById("btntoolbar").prepend(a);
    }

    Siguiente(id) {
        console.log(id);
        switch (id) {
            case 1:
                this.setState({
                    table: false,
                    crear: true,
                    editar: false,
                    visita: false
                });

                window.scrollTo = "crear";
                this.childCrear.current.titulo();
                return;
            case 2:
                this.setState({
                    table: false,
                    crear: false,
                    editar: true,
                    visita: false
                });
                window.scrollTo = "editar";
                this.childEditar.current.ConsultaUsuarioExterno();
                this.childEditar.current.titulo();
                return;
            case 3:
                this.setState({
                    visita: true,
                    table: false,
                    crear: false,
                    editar: false
                });
                window.scrollTo = "visita";
                this.childVisita.current.ConsultaUsuarioExterno();
                this.childVisita.current.titulo();
                return;

        }
    }

    validarCedula(identificacion) {
        var estado = false;
        var valced = [];
        var provincia;
        // console.log(identificacion.length)
        if (identificacion == "2222222222") {
            estado = false
        }
        else if (identificacion.length >= 10) {
            valced = identificacion.split('');
            // console.log('valced', valced)
            provincia = Number.parseInt(valced[0] + valced[1]);
            // console.log('provincia', provincia)
            if (provincia > 0 && provincia < 25) {
                if (Number.parseInt(valced[2]) < 6) {
                    estado = this.VerificaCedula(valced);
                    // console.log('VerificaCedula')
                }
                else if (Number.parseInt(valced[2]) == 6) {
                    // estado = this.VerificaSectorPublico(valced);
                    estado = true;
                }
                else if (Number.parseInt(valced[2]) == 9) {
                    estado = true;
                    // estado = this.VerificaPersonaJuridica(valced);
                }
            }
        }
        return estado;
    }

    VerificaCedula(validarCedula) {
        var aux = 0, par = 0, impar = 0, verifi;
        for (var i = 0; i < 9; i += 2) {
            aux = 2 * Number.parseInt(validarCedula[i]);
            if (aux > 9)
                aux -= 9;
            par += aux;
        }
        for (var i = 1; i < 9; i += 2) {
            impar += Number.parseInt(validarCedula[i]);
        }

        aux = par + impar;
        if (aux % 10 != 0) {
            verifi = 10 - (aux % 10);
        }
        else
            verifi = 0;
        if (verifi == Number.parseInt(validarCedula[9]))
            return true;
        else
            return false;
    }

    GetNacionalidades() {
        axios.post("/RRHH/Colaboradores/GetPaisesApi", {})
            .then((response) => {
                this.setState({ nacionalidades: response.data })
                this.getFormSelectNacionalidad()
            })
            .catch((error) => {
                console.log(error);
            });
    }

    getFormSelectNacionalidad() {
        return (
            this.state.nacionalidades.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

    Regresar() {
        this.childTable.current.GetVisitas();
        return (
            window.location.href = "/RRHH/Colaboradores/CrearUsuarioExterno"
        );
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }

}

ReactDOM.render(
    <UsuarioExterno />,
    document.getElementById('content-usuarios')
);