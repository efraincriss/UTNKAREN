import React from 'react';
import axios from 'axios';
import { Button } from 'primereact/components/button/Button';
import { Growl } from 'primereact/components/growl/Growl';
import { Textbox } from 'react-inputs-validation/lib/components';

const estilo = { height: '25px' }

export default class ItemForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            codigo: '',
            nombre: '',
            descripcion: '',
            coordenaday: 0,
            coordenadax: 0,
            IdForm: 0,
            message: '',

        }
        this.showSuccess = this.showSuccess.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.deleteItem = this.deleteItem.bind(this);
        this.getItemData = this.getItemData.bind(this);
        this.handleEdit = this.handleEdit.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);

    }

    componentDidMount() {
        this.getItemData();
    }

    render() {
        return (
            <div>
                <div className="row">
                    <div style={{ width: '100%' }}>
                        <ul className="nav nav-tabs" id="gestion_tabs" role="tablist">
                            <li className="nav-item">
                                <a className="nav-link active" id="jerarquia-tab" data-toggle="tab" href="#jerarquia" role="tab" aria-controls="profile">Modificación</a>
                            </li>
                        </ul>
                        <div className="tab-content" id="myTabContent">
                            <div className="tab-pane fade show active" id="jerarquia" role="tabpanel" aria-labelledby="jerarquia-tab">
                                <form onSubmit={this.handleEdit}>
                                <div className="form-group">
                                        <label htmlFor="label">Código</label> <br />
                                        <div className="row justify-content-md-center">
                                            <div className="col" >
                                                <input
                                                    type="text"
                                                    name="codigo"
                                                    value={this.state.codigo}
                                                    onChange={this.handleChange}
                                                    className="form-control"
                                                    min="1" max="99"
                                                    maxLength="10"
                                                    step="1.0" required />
                                            </div>
                                        </div>
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="label">Nombre</label>
                                        <input
                                            type="text"
                                            name="nombre"
                                            value={this.state.nombre}
                                            onChange={this.handleChange}
                                            className="form-control"
                                            maxLength="60"
                                            required
                                        />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="label">Descripción</label>
                                        <input
                                            type="text"
                                            name="descripcion"
                                            value={this.state.descripcion}
                                            onChange={this.handleChange}
                                            className="form-control"
                                            required
                                        />
                                    </div>
                                    <div className="row">
                                        <div className="col" >
                                            <div className="form-group">
                                                <label htmlFor="label">Coordenada X</label>
                                                <input
                                                    type="text"
                                                    name="coordenadax"
                                                    value={this.state.coordenadax}
                                                    onChange={this.handleChange}
                                                    className="form-control"
                                                    required
                                                />
                                            </div>
                                        </div>
                                        <div className="col" >
                                            <div className="form-group">
                                                <label htmlFor="label">Coordenada Y</label>
                                                <input
                                                    type="text"
                                                    name="coordenaday"
                                                    value={this.state.coordenaday}
                                                    onChange={this.handleChange}
                                                    className="form-control"
                                                    required
                                                />
                                            </div>
                                        </div>
                                    </div>
                                    <Button type="submit" label="Guardar" icon="fa fa-fw fa-folder-open" />
                                    <Button type="button" label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.props.onHide} />
                                    <Button label="Eliminar" icon="fa fa-remove" onClick={() => this.deleteItem(this.state.IdForm)} />
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
            </div>
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
    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }
    handleInputChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }

    deleteItem(id) {
        axios.post("/Proyecto/UbicacionGeografica/DeleteItemFrente/" + this.state.IdForm, {})
            .then((response) => {
                var r = response.data;
                if (r == "Eliminado") {
                    console.log("entro Eliminado");
                    this.successMessage("Eliminado");
                }
                if (r == "ErrorEliminado") {
                    console.log("Entro Error Eliminado");
                    this.warnMessage("No se puede Eliminar Zona con Frentes");
                }
                this.props.updateData();
            })
            .catch((error) => {
                console.log(error);
                this.warnMessage("");
            });
    }

    getItemData() {
        axios.post("/Proyecto/UbicacionGeografica/GetDataApiFrente/", { id: this.props.id })
            .then((response) => {
                console.log(response)
                this.setState({
                    codigo: response.data.codigo,
                    nombre: response.data.nombre,
                    descripcion: response.data.descripcion,
                    IdForm: response.data.Id,
                    coordenaday: response.data.coordenada_y,
                    coordenadax: response.data.coordenada_x,
                })
            })
            .catch((error) => {
                console.log(error);
                this.warnMessage("");
            });
    }

    handleEdit() {
        event.preventDefault();
        axios.post("/proyecto/UbicacionGeografica/EditApiFrente", {
            Id: this.state.IdForm,
            nombre: this.state.nombre,
            descripcion: this.state.descripcion,
            cordenaday: this.state.coordenaday,
            cordenadax: this.state.coordenadax,
        })
            .then((response) => {
                console.log(response)
                this.setState({
                    nombre: '',
                    descripcion: '',
                    coordenaday: 0,
                    coordenadax: 0,
                    IdForm: 0,
                })
                this.props.updateData();
                var r = response.data;

                if (r == "Guardado") {
                    console.log("entro guardado");
                    this.successMessage("Guardado Exitoso");
                }
                if (r == "Error") {
                    console.log("entro error");
                    this.warnMessage("Hubo un Error");
                }
            })
            .catch((error) => {
                console.log(error);
                this.warnMessage("");
            });
    }

}
