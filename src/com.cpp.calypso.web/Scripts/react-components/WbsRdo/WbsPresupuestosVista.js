import React, { Component } from 'react';
import axios from 'axios';
import { Tree } from 'primereact-v2/components/tree/Tree';
import { ContextMenu } from 'primereact-v2/contextmenu';
import { Button } from 'primereact/components/button/Button';


export default class WbsPresupuestosVista extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            data: [],
            expandedKeys: {},
            NombreNivel: '',
            FechaInicio: '',
            FechaFin: '',
            observacion: '',
            expandir: true,
            menu: [
                {
                    label: 'Copiar a',
                    icon: 'pi pi-clone',
                    command: () => {
                        this.props.MostrarDialogo();
                    }
                }
            ]
        };

        this.updateData = this.updateData.bind(this);
        this.Expander = this.Expander.bind(this)
        this.onSelect = this.onSelect.bind(this);
        this.Refrescar = this.Refrescar.bind(this);
    }

    componentWillMount() {
        this.updateData();
    }

    render() {

        return (
            <div className="col">
                <h3 className="text-blue">Presupuesto</h3>
                <div className="row" >
                    <div className="col-sm-12">
                        <div className="card" style={{ paddingBottom: '0.5em', height: '150px', maxHeight: '150px' }} >
                            <div className="card-body">
                                <span><b>Actividad: </b> {this.state.NombreNivel}</span>
                                <br />
                                <span><b>Periodo: </b>{this.state.FechaInicio} - {this.state.FechaFin}</span>
                                <br />
                                <span><b>Observación: </b> {this.state.observacion}</span>
                                <br />
                                <br />
                            </div>
                        </div>

                    </div>

                    <div className="col-sm-12">
                        <Button label="Refrescar" icon="pi pi-refresh" onClick={this.Refrescar} />
                        <Button label="Exp/Contraer" icon="pi pi-external-link" onClick={this.Expander} />
                        <div className="row" style={{ height: '500px', maxHeight: '550px' }}>
                            <div className="col" style={{ overflowX: 'scroll', overflowY: 'scroll' }}>
                                <ContextMenu appendTo={document.body} model={this.state.menu} ref={el => this.cm = el} />
                                <div>
                                    
                                    <Tree value={this.state.nodes} selectionMode="single"
                                        expandedKeys={this.state.expandedKeys}
                                        onToggle={e => this.setState({ expandedKeys: e.value })}
                                        onSelect={this.onSelect}
                                        style={{ marginTop: '.5em' }}
                                        dragdropScope="demo"
                                        onDragDrop={event => this.setState({ nodes: event.value })}
                                        onContextMenuSelectionChange={event => this.props.EstablecerNodoOrigen(event)}
                                        onContextMenu={event => this.cm.show(event.originalEvent)}
                                        contextMenuSelectionKey={event => console.log(event)}
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    updateData() {
        axios.get("/proyecto/WbsPresupuesto/ApiWbsL/" + this.props.PresupuestoId, {})
            .then((response) => {
                this.setState({ nodes: response.data })
                this.props.DesbloquearPantalla();
            })
            .catch((error) => {
                console.log(error);
                this.props.DesbloquearPantalla();
            });
    }

    Expander() {
        this.props.BloquearPantalla();
        axios.post("/proyecto/WbsPresupuesto/ApiWbsK/" + this.props.PresupuestoId, {})
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
        if (event.node.tipo == 'padre') {
            this.setState({
                NombreNivel: event.node.label, FechaInicio: null, FechaFin: null, observacion: ""
            })
        } else {
            this.setState({
                NombreNivel: event.node.label, FechaInicio: ids[5], FechaFin: ids[6], observacion: ids[4]
            })
            if (ids[5] === "1/1/0001") {
                this.setState({ FechaInicio: "dd/mm/aaaa" })
            } else {
                this.setState({ FechaInicio: ids[5] })
            }
            if (ids[6] === "1/1/0001") {
                this.setState({ FechaFin: "dd/mm/aaaa" })
            } else {
                this.setState({ fechafin: ids[6] })
            }
        }
        this.setState({ selectedFile: event.node });
    }

    Refrescar() {
        this.props.BloquearPantalla();
        this.updateData();
        this.props.DesbloquearPantalla();
    }
}

