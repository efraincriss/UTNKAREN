import React from 'react';
import axios from 'axios';
import { Button } from 'primereact/components/button/Button';
import { Growl } from 'primereact/components/growl/Growl';
import { Textbox } from 'react-inputs-validation/lib/components';
import { MultiSelect } from 'primereact/components/multiselect/MultiSelect';

const estilo = { height: '25px' }

export default class ItemForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            codigo: '',
            nombre: '',
            descripcion: '',
            pais: 0,
            IdForm: 0,
            message: '',
            id_pais: 0,
            provincias: [],
            provinciasIds: [],
            blocking: false,

        }
        this.showSuccess = this.showSuccess.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.handleCreate = this.handleCreate.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.successMessage = this.successMessage.bind(this);
        this.warnMessage = this.warnMessage.bind(this);
        this.getPaisFormSelect = this.getPaisFormSelect.bind(this);
        this.GetProvincias = this.GetProvincias.bind(this);
        this.handlePaisChange = this.handlePaisChange.bind(this);
    }

    componentDidMount() {

    }

    render() {
        return (
            <div>
                <div className="row">
                    <div style={{ width: '100%' }}>
                        <ul className="nav nav-tabs" id="gestion_tabs" role="tablist">
                            <li className="nav-item">
                                <a className="nav-link active" id="jerarquia-tab" data-toggle="tab" href="#jerarquia" role="tab" aria-controls="profile">Creación de Zona</a>
                            </li>
                        </ul>
                        <div className="tab-content" id="myTabContent">
                            <div className="tab-pane fade show active" id="jerarquia" role="tabpanel" aria-labelledby="jerarquia-tab">
                                <form onSubmit={this.handleCreate}>
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
                                    <div className="form-group">
                                        <label htmlFor="label">Pais</label>
                                        <select value={this.state.id_pais} required onChange={this.handlePaisChange} className="form-control" name="id_pais">
                                            <option value="">--- Selecciona un País ---</option>
                                            {this.getPaisFormSelect()}
                                        </select>
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="label">Provincias</label>
                                        <br />
                                        <MultiSelect
                                            value={this.state.provinciasIds}
                                            options={this.state.provincias}
                                            onChange={(e) => this.setState({ provinciasIds: e.value })}
                                            style={{width:'32em'}} filter={true}
                                            defaultLabel="--- Selecciona las Provincias ---"
                                            className="form-control"
                                        />
                                    </div>
                                    <Button type="submit" label="Guardar" icon="fa fa-fw fa-folder-open" />
                                    <Button type="button" label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.props.onHide} />
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

    handleCreate() {
        event.preventDefault();
        axios.post("/Proyecto/UbicacionGeografica/CreateZonaApi", {
            Id: this.state.IdForm,
            codigo: this.state.codigo,
            nombre: this.state.nombre,
            descripcion: this.state.descripcion,
            zonaId: this.state.id_pais,
            provincias: this.state.provinciasIds,
        })
            .then((response) => {
                console.log(response)
                this.setState({
                    codigo: '',
                    nombre: '',
                    descripcion: '',
                    id_pais: 0,
                    provinciasIds: [],
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

    getPaisFormSelect() {
        return (
            this.props.paises.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })

        );
    }

    handlePaisChange(event) {
        this.setState({ [event.target.name]: event.target.value, blocking: true }, this.GetProvincias)
    }

    GetProvincias() {
        var Id = this.state.id_pais;
        this.setState({provinciasIds: []})
        axios.post("/Proyecto/UbicacionGeografica/GetProvinciasApi/", { Id: Id })
            .then((response) => {
                var provincias = response.data.map(item => {
                    return { label: item.nombre, value: item.Id }
                })
                this.setState({ provincias: provincias })
            })
            .catch((error) => {
                console.log(error);
            });
    }

}
