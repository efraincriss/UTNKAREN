import React from 'react';
import axios from 'axios';
import { Button } from 'primereact/components/button/Button';
import { Growl } from 'primereact/components/growl/Growl';
import { Dropdown } from 'primereact/components/dropdown/Dropdown'

const estilo = { height: '25px' }

export default class ComputoPresupuestoForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Item_Id: 0,
            cantidadc: 1,
            vigente: true,
            estado: true,
            message: '',
            nitem: '',
            nidescripcion: 0.0,
            ncantidad: '',
            paraoferta: true,
            nunidad: 26,
            npadre: 0,
            item: props.item,

            codigo:'',

        }
        this.getFormSelect = this.getFormSelect.bind(this);
        this.getProcura = this.getProcura.bind(this);
        this.getUnidades = this.getUnidades.bind(this);
        this.handleSubmitAdicional = this.handleSubmitAdicional.bind(this);
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
                    <div style={{ width: '100%' }}>

                        <ul className="nav nav-tabs" id="gestion_tabs" role="tablist">
                            <li className="nav-item">
                                <a className="nav-link active" id="gestion-tab" data-toggle="tab" href="#gestion" role="tab" aria-controls="home" aria-expanded="true">Nuevo Computo</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link " id="jerarquia-tab" data-toggle="tab" href="#jerarquia" role="tab" aria-controls="profile">Item Adicional Procura</a>
                            </li>

                        </ul>

                        <div className="tab-content" id="myTabContent">

                            <div className="tab-pane fade show active" id="gestion" role="tabpanel" aria-labelledby="gestion-tab">
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


                            <div className="tab-pane fade" id="jerarquia" role="tabpanel" aria-labelledby="jerarquia-tab">
                                <form onSubmit={this.handleSubmitAdicional}>

                                    <div className="form-group">
                                        <label htmlFor="label">Grupo</label>

                                        <select value={this.state.npadre} onChange={this.handleChange} className="form-control" name="npadre" required>
                                            <option value="0" >--- Seleccione Grupo ---</option>
                                            {this.getProcura(this.props.itemsprocura)}

                                        </select>

                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="label">Nombre</label>

                                        <input
                                            type="text"
                                            name="nitem"
                                            value={this.state.nitem}
                                            onChange={this.handleChange}
                                            className="form-control"
                                            required
                                        />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="label">Código</label>

                                        <input
                                            type="text"
                                            name="codigo"
                                            value={this.state.codigo}
                                            onChange={this.handleChange}
                                            className="form-control"
                                            required
                                        />
                                    </div>


                                    

                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="label">Cantidad:</label> <br />

                                                <input
                                                    type="number"
                                                    name="ncantidad"
                                                    value={this.state.ncantidad}
                                                    onChange={this.handleChange}
                                                    className="form-control"

                                                    required />

                                            </div></div>

                                            <div className="form-group">
                                        <label htmlFor="label">Precio</label>

                                        <input
                                            type="number"
                                            name="nidescripcion"
                                            value={this.state.nidescripcion}
                                            onChange={this.handleChange}
                                            className="form-control"
                                            required
                                        />
                                    </div>
                                        <div className="col">
                                            <div className="form-group">
                                                <label htmlFor="label">Unidad</label>
                                                <Dropdown
                                                    value={this.state.nunidad}
                                                    options={this.props.unidades}
                                                    onChange={(e) => { this.setState({ nunidad: e.value }) }}
                                                    filter={true} filterPlaceholder="Selecciona una Unidad"
                                                    filterBy="label,value" placeholder="Selecciona una Unidad"
                                                    style={{ width: '100%', heigh: '18px' }}
                                                    required
                                                />
                                            </div>
                                        </div>
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

    handleSubmit(event) {

        event.preventDefault();

        if (this.state.item == 0) {
            this.setState({ message: 'Selecciona un item' }, this.showWarn)

        } else {
            axios.post("/proyecto/ComputoPresupuesto/CreateComputoArbol", {
                WbsPresupuestoId: this.props.WbsOfertaId,
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

    handleSubmitAdicional(event) {

        event.preventDefault();

        if (this.state.nunidad == 0) {
            this.setState({ message: 'Selecciona una unidad' }, this.showWarn)

        } else {
            axios.post("/Proyecto/ComputoPresupuesto/CreateAdicional", {
                WbsId: this.props.WbsOfertaId,
                nombre: this.state.nitem,
                precio: this.state.nidescripcion,
                cantidad: this.state.ncantidad,
                unidadid: this.state.nunidad,
                npadre: this.state.npadre,
                codigo:this.state.codigo

            })
                .then((response) => {
                    var r = response.data;
                    console.log(r);
                    if (r == "Guardado") {
                        console.log("entro guardado");
                        this.setState({
                            message: 'Guardado Computo Correctamente',
                            ncantidad: 1,
                            nitem: "",
                            nidescripcion: "",
                            nunidad: 26,
                            npadre: 0,
                            codigo:''
                        },
                            this.showSuccess),
                            this.props.updateData();

                    }
                    if (r == "Error") {
                        console.log("entro error");
                        this.setState({ message: 'No se pudo Completar Transaccion' },
                            this.showWarn)
                    }
                    if (r == "noitem") {
                        console.log("entro error movimiento");
                        this.setState({ message: 'Error el Item No se Creo' },
                            this.showWarn)
                    }
                })
                .catch((error) => {

                });
        }
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