import React from "react";
import HabitacionTree from "./HabitacionTree";
import EspaciosCards from "./EspaciosCards";
import { Dialog } from 'primereact/components/dialog/Dialog';

export default class HabitacionesPanel extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            selectedFile: {}
        }
    }


    render() {
        return (

            <div className="row">
                <div style={{ width: '100%' }}>
                    <div className="card">
                        <div className="card-body">
                            <div className="row" >
                                <div className="col">
                                    <h3 className="text-gray-400">Habitaciones de {this.props.razonSocial}</h3>
                                </div>
                                <div className="col" align="right">

                                    <button
                                        style={{ marginLeft: '0.3em' }}
                                        className="btn btn-outline-primary"
                                        onClick={() => this.props.switchView(true)}
                                    >Regresar</button>

                                </div>
                            </div>
                            <hr />

                            <div className="row">
                                <div className="col-xs-12 col-md-3">
                                    <HabitacionTree
                                        nodes={this.props.nodes}
                                        onSelect={this.onSelect}
                                    />
                                </div>

                                <div className="col-xs-12 col-md-9">
                                    <EspaciosCards
                                        mostrarForm={this.props.mostrarForm}
                                        espacios={this.props.espacios}
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }


    onSelect = (event) => {
        var id = event.node.data;
        this.setState({ selectedFile: event.node });
        this.props.consultarEspacios(id);
    }
}