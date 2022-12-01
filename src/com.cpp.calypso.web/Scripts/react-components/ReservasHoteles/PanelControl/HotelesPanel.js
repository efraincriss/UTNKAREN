import React from "react";
import Field from "../../Base/Field-v2";
import { Card } from 'primereact-v2/card'
import { Button } from 'primereact-v2/button';


export default class HotelesPanel extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            fecha: '',
            errors: {},
        }
    }


    render() {
        return (
            <div className="row">
                <div style={{ width: '100%' }}>
                    <div className="card">
                        <div className="card-body">
                            <div className="row">
                                <div className="col-md-5">
                                    <form onSubmit={() => this.props.consultarHoteles(this.state.fecha)}>
                                        <div className="row">
                                            <div className="col">
                                                <Field
                                                    name="fecha"
                                                    label="Fecha"
                                                    required
                                                    type="date"
                                                    edit={true}
                                                    readOnly={false}
                                                    value={this.state.fecha}
                                                    onChange={this.handleChange}
                                                    error={this.state.errors.fecha}
                                                />
                                            </div>

                                            <div className="col-md-1" style={{ paddingTop: '34px' }}>
                                                <button type="submit" className="btn btn-outline-primary">Buscar</button>&nbsp;

                                            </div>
                                        </div>

                                    </form>

                                </div>
                            </div>
                            <hr />
                            <div className="row">
                                {this.renderCards()}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }


    hotelCard = hotel => {

        const footer = (
            <span>
                <Button label="Habitaciones" icon="pi pi-check" onClick={() => this.props.consultarHabitaciones(hotel.Id, hotel.razon_social)} />
            </span>
        );
        return (
            <Card
                title={hotel.razon_social}
                subTitle={`Espacios Totales ${hotel.espacios_totales}`}

                className="ui-card-shadow"
                footer={footer}
                key={`${hotel.Id}_key`}
            >
                <p><b>Espacios Ocupados:</b> {hotel.espacio_ocupados}</p>
                <p><b>Espacios Libres:</b> {hotel.espacios_libres}</p>
            </Card>
        )
    }

    renderCards = () => {
        if (this.props.hoteles.length === 0) {
            return (
                <div className="col">
                    <h3 className="text-gray-400">Ingresa una fecha para mostrar el listado de hoteles</h3>
                </div>

            )
        }
        return this.props.hoteles.map(hotel => {
            return (
                <div
                    className="col-xs-11 col-md-3"
                    style={{ marginTop: '1em' }}
                    key={`${hotel.Id}_key_parent`}
                >
                    {this.hotelCard(hotel)}
                </div>
            )
        })
    }

    handleChange = event => {
        this.setState({ [event.target.name]: event.target.value });
    }

}