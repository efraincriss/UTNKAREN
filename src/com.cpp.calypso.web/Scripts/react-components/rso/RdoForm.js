import React from 'react';
import axios from "axios/index";
import moment from 'moment';
import { Dropdown } from "primereact-v2/dropdown";
export default class RdoForm extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            fecha_rdo: '',
            fecha_inicio: 0,
            block: false,
            tieneFechaInicio: false,
        };
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }
    componentDidMount() {
        console.log('props', this.props)

    }

    render() {
        return (
            <div>
                <div>
                    <form onSubmit={this.handleSubmit}>
                        {this.props.rdoCabecerasDefinitivas != null && this.props.rdoCabecerasDefinitivas.length > 0 && <div className="form-group">
                            <label htmlFor="label">Fecha Inicial </label>
                            <Dropdown
                                value={this.state.fecha_inicio}
                                options={this.props.rdoCabecerasDefinitivas}
                                onChange={(e) => {
                                    this.setState({ fecha_inicio: e.value, tieneFechaInicio: true });
                                }}
                                required
                                filter={true}
                                filterPlaceholder="Seleccione.."
                                filterBy="label,value"
                                placeholder="Seleccione.."
                                style={{ width: "100%" }}
                            />
                        </div>
                        }
                        <div className="form-group">
                            <label>Fecha Final RSO</label>
                            <input
                                type="date"
                                id="no-filter"
                                name="fecha_rdo"
                                className="form-control"
                                onChange={this.handleChange}
                                value={this.state.fecha_rdo}
                                required
                            />

                        </div>

                        <button className="btn btn-outline-primary" type="submit">Generar</button>
                    </form>
                </div>
            </div>
        )
    }

    handleSubmit(event) {
        event.preventDefault();
        this.props.Block();
        let _this = this;
        if (this.props.rdoCabecerasDefinitivas != null && this.props.rdoCabecerasDefinitivas.length > 0) {
            if (this.state.fecha_inicio === 0) {
                abp.notify.error('Seleccione Fecha Inicial', 'Error');
                _this.props.Unlock();

                return;
            }
        }

        abp.message.confirm(
            "Se  procederá a generar el RSO " + this.state.fecha_rdo + ". Si existen RSOs posteriores generados  a la Fecha Inicial y Final se eliminarán. ¿Desea Continuar?",
            "¿Generar RSO?",
            function (isConfirmed) {
                if (isConfirmed) {
                    _this.props.Block();
                    axios.post("/Proyecto/RsoCabecera/CreateRso", {
                        fecha_registro: _this.state.fecha_rdo,
                        ProyectoId: document.getElementById('content').className,

                        Id: _this.state.fecha_inicio,
                        tieneFechaInicio:_this.state.tieneFechaInicio
                    })
                        .then((response) => {
                            if (response.data == "OK") {
                                _this.setState({ fecha_rdo: '', fecha_inicio:0,tieneFechaInicio:false })
                                _this.props.showSuccess();
                                _this.props.updateData();
                                _this.props.Unlock();
                            } else if (response.data == "MENOR") {
                                abp.notify.error('La Fecha Final no puede ser menor a la Fecha de un RSO anterior', 'Error');
                                _this.props.Unlock();
                            } else {
                                _this.props.showWarn();
                            }
                            _this.props.Unlock();
                        })
                        .catch((error) => {
                            console.log(error)
                            _this.props.showWarn();
                            _this.props.Unlock();
                        });
                } else {
                    _this.props.Unlock();
                }
            }
        );

    }

    handleChange(event) {
        event.stopPropagation();
        this.setState({ [event.target.name]: event.target.value });
    }
}