import React from 'react';
import axios from 'axios';
import Field from "../Base/Field-v2";
export default class UsuariosForm extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            Id: '',
            correo: '',
            nombres: '',
            usuario: '',
            errors: {}
        }

        this.getFormSelect = this.getFormSelect.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }


    render() {
        return (
            <div>
                <form onSubmit={this.handleSubmit} style={{ height: "450px" }}>
                    <Field
                        name="usuario"
                        required
                        value={this.state.usuario}
                        label="Usuario"
                        options={this.props.data}
                        type={"select"}
                        filter={true}
                        onChange={this.onChangeValue}
                        error={this.state.errors.usuario}
                        readOnly={false}
                        placeholder="Seleccione.."
                        filterPlaceholder="Seleccione.."

                    />
                    <br />

                    <button type="submit" label="Guardar" className="btn btn-outline-primary" style={{ marginRight: '0.4em' }}>Guardar</button>
                    <button type="button" label="Cancelar" className="btn btn-outline-primary" onClick={this.props.onHide}>Cancelar</button>
                </form>
            </div>
        )
    }

    handleSubmit(event) {
        event.preventDefault();
        if (this.state.id == '') {
            this.props.showWarn();
        } else {
            axios.post("/proyecto/CorreoLista/CreateApi/", {
                ListaDistribucionId: document.getElementById('content').className,
                externo: this.props.externo,
                nombres: this.state.nombres,
                UsuarioId: this.state.Id,
                correo: this.state.correo,
                vigente: true,
            })
                .then((response) => {
                    if (response.data == "Ok") {
                        this.props.updateData();
                        this.props.showSuccess();
                        this.props.UpdateCorreos();
                        this.props.onHide();
                    } else {
                        this.props.showWarn();
                    }
                })
                .catch((error) => {
                    console.log(error);
                    this.props.showWarn();
                });
        }

    }

    getFormSelect(list) {
        return (
            list.map((item) => {
                return (
                    <option value={item.Id + "," + item.nombres + "," + item.correo} key={item.Id}>{item.nombres} - {item.correo}</option>
                )
            })

        );
    }
    onChangeValue = (name, value) => {
        this.setState({
            [name]: value
        });
        var data = value.split(",");
        this.setState({ Id: data[0], nombres: data[1], correo: data[2] });

    };
    handleChange(event) {
        console.log(event);
        //var data = event.target.value.split(",");
        // this.setState({ Id: data[0], nombres: data[1], correo: data[2] });
    }
}