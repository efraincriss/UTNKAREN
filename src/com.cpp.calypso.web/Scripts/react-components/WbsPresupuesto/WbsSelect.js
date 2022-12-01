import React, { Component } from 'react';
import axios from 'axios';
import { Tree } from 'primereact-v2/components/tree/Tree';
import { Button } from 'primereact/components/button/Button';
import BlockUi from 'react-block-ui';

export default class WbsSelect extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            data: [],
            expandedKeys: {},
            expandir: true,
            label: '',
            blocking: false
        };

        this.updateData = this.updateData.bind(this);
        this.Expander = this.Expander.bind(this)
        this.onSelect = this.onSelect.bind(this);
        this.Refrescar = this.Refrescar.bind(this);
        this.DesbloquearPantalla = this.DesbloquearPantalla.bind(this);
        this.BloquearPantalla = this.BloquearPantalla.bind(this);
        this.SeleccionarRaiz = this.SeleccionarRaiz.bind(this)
    }

    render() {

        return (
            <BlockUi tag="div" blocking={this.state.blocking}>
                <div className="row">
                    <div className="col">
                        <div className="row">
                            <div className="col">
                                <span><b>Destino: </b>{this.state.label}</span>
                            </div>
                        </div>
                        <Button label="Refrescar" icon="pi pi-refresh" onClick={this.Refrescar} />
                        <Button label="Exp/Contraer" icon="pi pi-external-link" onClick={this.Expander} />
                        {/*<Button label="Raiz" icon="pi pi-external-link" onClick={this.SeleccionarRaiz} />*/}
                        <div className="row" style={{ height: '550px', minHeight: '550px' }}>
                            <div className="col" style={{ overflowX: 'scroll', overflowY: 'scroll' }}>
                                <div>
                                    <Tree value={this.props.data}
                                        selectionMode="single"
                                        style={{ marginTop: '.5em' }}
                                        expandedKeys={this.state.expandedKeys}
                                        onToggle={e => this.setState({ expandedKeys: e.value })}
                                        onSelect={this.onSelect}
                                    />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </BlockUi>
        );
    }

    updateData() {
        this.props.BloquearPantalla();
        axios.get("/proyecto/WbsPresupuesto/ApiWbsL/" + this.props.OfertaId, {})
            .then((response) => {
                this.props.SetearDatosRDO(response.data)
                this.props.DesbloquearPantalla();
            })
            .catch((error) => {
                console.log(error);
                this.props.DesbloquearPantalla();
            });
    }

    Expander() {
        this.props.BloquearPantalla();
        axios.post("/proyecto/WbsPresupuesto/ApiWbsK/" + this.props.OfertaId, {})
            .then((response) => {
                var llaves = response.data;
                if (this.state.expandir) {
                    let expandedKeys = {};
                    llaves.forEach((product) => {
                        if (expandedKeys[product])
                            delete expandedKeys[product];
                        else
                            expandedKeys[product] = true;
                    });
                    this.setState({ expandedKeys: expandedKeys, expandir: false }, this.props.DesbloquearPantalla);
                } else {
                    this.setState({ expandedKeys: {}, expandir: true }, this.props.DesbloquearPantalla);
                }
            })
            .catch((error) => {
                console.log(error);
                this.props.DesbloquearPantalla();
            });
    }

    onSelect(event) {
        var ids = event.node.data.split(",");
        this.props.EstablecerNodoDestino(ids[2])
        this.setState({ label: event.node.label })
    }

    SeleccionarRaiz() {
        this.props.EstablecerNodoDestino(-2)
        this.setState({ label: "Raiz" })
    }

    Refrescar() {
        console.log("Refrescando")
        this.props.BloquearPantalla();
        this.props.updateData();
        this.props.DesbloquearPantalla();
    }

    DesbloquearPantalla() {
        this.setState({ blocking: false })
    }

    BloquearPantalla() {
        this.setState({ blocking: true })
    }
}

