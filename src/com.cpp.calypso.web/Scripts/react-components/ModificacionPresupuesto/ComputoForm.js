import React from 'react';
import axios from 'axios';
import { Button } from 'primereact/components/button/Button';
import { Growl } from 'primereact/components/growl/Growl';
import { Textbox } from 'react-inputs-validation/lib/components';
import { Dropdown } from 'primereact/components/dropdown/Dropdown'

const estilo = { height: '25px' }

export default class ComputoForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Item_Id: 0,
            cantidadc: 1,
            vigente: true,
            estado: true,
            message: '',
            nitem: '',
            nidescripcion: '',
            ncantidad: '',
            paraoferta: true,
            nunidad: 0,
            npadre: 0,
            item: props.item,


        }
        this.getFormSelect = this.getFormSelect.bind(this);
        this.getProcura = this.getProcura.bind(this);
        this.getUnidades = this.getUnidades.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }
    render() {
        return (
            <div>

                <div className="row">
                    <div style={{ width: '90%', height: '300px', margin: 'auto' }}>
                        <form onSubmit={this.handleSubmit}>
                            <div className="form-group">
                                <label htmlFor="label">Item:</label>

                                <Dropdown
                                    value={this.state.item}
                                    options={this.props.item_list}
                                    onChange={(e) => { this.setState({ item: e.value }) }}
                                    filter={true} filterPlaceholder="Selecciona un item"
                                    filterBy="label,value" placeholder="Selecciona un item"
                                    style={{ width: '100%', heigh: '18px' }}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="label">Cantidad:</label> <br />
                                <input
                                    type="number"
                                    name="cantidadc"
                                    value={this.state.cantidadc}
                                    onChange={this.handleChange}
                                    className="form-control"
                                    required />
                            </div>
                            <div className="form-group">
                                <label>
                                    Estado :<br />
                                    <input
                                        name="estado"
                                        type="checkbox"
                                        checked={this.state.estado}
                                        onChange={this.handleInputChange} />
                                </label>
                            </div>
                            <Button type="submit" label="Guardar" icon="fa fa-fw fa-folder-open" />
                            <Button type="button" label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.props.onHide} />
                        </form>
                    </div>
                </div>

                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>

            </div>
        );
    }

    handleSubmit(event) {
        event.preventDefault();

        if (this.state.item == 0) {
            this.setState({ message: 'Selecciona un item' }, this.showWarn)

        } else {
            axios.post("/proyecto/ComputosTemporal/CreateComputosTemporalArbol", {
                OfertaId: document.getElementById('OfertaId').className,
                WbsId: this.props.WbsOfertaId,
                ItemId: this.state.item,
                estado: this.state.estado,
                vigente: true,
                codigo_primavera: "a",
                cantidad: this.state.cantidadc,
                nuevonombrei: "nuevonombre"
            })
                .then((response) => {
                    var r = response.data;
                    if (r == "OK") {
                        console.log("entro guardado");
                        this.setState({
                            message: 'Guardado Correctamente',
                            cantidadc: 1,
                            item: 0

                        },
                            this.showSuccess)
                        this.props.updateData();

                    } else if (r == "Repetido") {
                        console.log("repetido");
                        this.setState({
                            message: 'El Item ya se encuentra registrado',
                            cantidadc: 1,
                            item: 0
                        },
                            this.showWarn)
                    } else {
                        console.log("entro error");
                        this.setState({ message: 'No se pudo Completar Transaccion' },
                            this.showWarn)
                    }

                })
                .catch((error) => {

                });
        }
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
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

    getFormSelect(list) {
        return (

            list.map((item) => {
                return (
                    <option key={item.Id} value={item.Id}>{item.codigo} {item.nombre}</option>
                )

            })

        );
    }

    getProcura(list) {
        return (

            list.map((item) => {
                return (
                    <option key={item.value} value={item.value}>{item.label}</option>
                )

            })

        );
    }

    getUnidades(list) {
        return (

            list.map((item) => {
                return (
                    <option key={item.value} value={item.value}>{item.label}</option>
                )

            })

        );
    }
}