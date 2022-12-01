import React from "react";
import { Card } from 'primereact-v2/card'
import { Button } from 'primereact-v2/button';

export default class EspaciosCards extends React.Component {

    constructor(props) {
        super(props)
    }

    render() {
        return (
            <div className="row">
                <div style={{ width: '100%' }}>
                    <div className="card">
                        <div className="card-body">
                            <div className="row">
                                {this.renderApp()}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    renderApp = () => {
        if (this.props.espacios.length === 0) {
            
            return (
                <div className="col">
                    <h3 className="text-gray-400">Selecciona una Habitación con Espacios Registrados</h3>
                </div>
            )
        } else {
            return (
                this.renderCards()
            )
        }
    }

    espacioCard = espacio => {
        console.log(espacio.Id)
        const footer = (
            <span>
                <Button label="Reservar" icon="pi pi-check" onClick={() =>this.props.mostrarForm(espacio.Id)} />
            </span>
        );

        if (espacio.ocupado) {
            return (
                <Card
                    title={espacio.codigo_espacio}
                    className="ui-card-shadow"
                    key={`${espacio.Id}_key}`}
                    subTitle={`Espacio Ocupado`}
                >
                    <h4>{espacio.nombres_colaborador}</h4>
                </Card>
            )
        } else {
            return (
                <Card
                    title={espacio.codigo_espacio}
                    className="ui-card-shadow"
                    footer={footer}
                    key={`${espacio.Id}_key`}
                    subTitle={`Espacio Libre`}
                >
                    
                </Card>
            )
        }

    }

    renderCards = () => {
        return this.props.espacios.map(espacio => {
            return (
                <div
                    className="col-xs-11 col-md-3"
                    style={{ marginTop: '1em' }}
                    key={`${Math.random()}_key_parent`}
                >
                    {this.espacioCard(espacio)}
                </div>
            )
        })
    }



}