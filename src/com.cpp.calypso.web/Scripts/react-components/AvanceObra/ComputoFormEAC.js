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
            
            item: props.item,
            EAC: 0,
            estado: true,


        }
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }
    render() {
        return (
            <div>
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
                        <label htmlFor="label">Cantidad EAC:</label> <br />

                        <input
                            type="number"
                            name="EAC"
                            value={this.state.EAC}
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
        );
    }

    handleSubmit(event) {

        event.preventDefault();

        if (this.state.item == 0) {
            this.setState({ message: 'Selecciona un item' }, this.props.showWarn)

        } else {
            axios.post("/proyecto/Computo/CreateComputoArbol", {
                WbsId: this.props.WbsOfertaId,
                ItemId: this.state.item,
                estado: this.state.estado,
                vigente: true,
                codigo_primavera: "a",
                cantidad: 0,
                cantidad_eac: this.state.EAC,
                presupuestado: false,
                nuevonombrei: "nuevonombre"
            })
                .then((response) => {

                    var r = response.data;
                    if (r == "OK") {
                        
                        this.setState({
                            message: 'Guardado Correctamente',
                            cantidadc: 1,
                            item: 0,
                            EAC: 0,

                        })
                        this.props.successMessage("Computo Registrado")
                        this.props.updateData();

                    } else if (r == "Repetido") {
                        
                        this.setState({
                            message: 'El Item ya se encuentra registrado',
                            cantidadc: 1,
                            item: 0
                        })
                        this.props.warnMessage('El Item ya se encuentra registrado')
                    } else {

                       
                        this.setState({ message: 'No se pudo Completar Transaccion' })
                        this.props.warnMessage('El Item ya se encuentra registrado')
                    }

                })
                .catch((error) => {

                });
        }
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

    
}