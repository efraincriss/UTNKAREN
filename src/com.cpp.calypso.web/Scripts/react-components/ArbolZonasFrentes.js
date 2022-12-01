import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';

import { Dialog } from 'primereact/components/dialog/Dialog';
import { Button } from 'primereact/components/button/Button';

import TreeFull from './ArbolZonaFrente/Arbol';
import ItemForm from './ArbolZonaFrente/ItemForm';
import ItemFormFrente from './ArbolZonaFrente/ItemFormFrente';
import NuevaZona from './ArbolZonaFrente/NuevaZona';

export default class CreateTreeItems extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: [],
            ItemPadreSeleccionado: 0,
            key: 8248,
            visible: false,
            visibleFrente: false,
            visibleZona: false,
            key_form: 98723,
            id_seleccionado: 0,
            label_header: '',
            paises:[],
            id_pais: 0,
        }
        this.updateData = this.updateData.bind(this);
        this.onSelectionChange = this.onSelectionChange.bind(this);
        this.onHide = this.onHide.bind(this);
        this.onHideFrente = this.onHideFrente.bind(this);
        this.onHideZona = this.onHideZona.bind(this);
        this.GetPaises = this.GetPaises.bind(this);
        this.activarCrearZona = this.activarCrearZona.bind(this);
        this.getPaisesForSelect = this.getPaisesForSelect.bind(this);
    }

    componentWillMount() {
        this.updateData();
        this.GetPaises();
    }

    render() {
        return (
            <div className="row">
                <div className="col-sm-12">
                    <Button
                        label="Nueva Zona"
                        icon="fa fa-plus-square-o"
                        onClick={this.activarCrearZona}
                    />
                    <label htmlFor="label">País</label>
                            <select value={this.state.id_pais} required onChange={this.changePais} className="form-control" name="id_pais">
                                <option value="">--- Selecciona un País ---</option>
                                {this.getPaisesForSelect()}
                            </select>
                    <br /><hr />
                    <TreeFull
                        key={this.state.key}
                        onSelectionChange={this.onSelectionChange}
                        data={this.state.data} onHide={this.onHide}
                    />
                    <Dialog header={this.state.label_header}
                        visible={this.state.visible}
                        width="500px" modal={true}
                        onHide={this.onHide}>
                        <ItemForm
                            id={this.state.id_seleccionado}
                            key={this.state.key_form}
                            itemPadre={this.state.ItemPadreSeleccionado}
                            updateData={this.updateData}
                            onHide={this.onHide}
                            tipo={this.state.label_header} />
                    </Dialog>
                    <Dialog header={this.state.label_header}
                        visible={this.state.visibleFrente}
                        width="500px" modal={true}
                        onHide={this.onHideFrente}>
                        <ItemFormFrente
                            id={this.state.id_seleccionado}
                            key={this.state.key_form}
                            itemPadre={this.state.ItemPadreSeleccionado}
                            updateData={this.updateData}
                            onHide={this.onHideFrente}
                            tipo={this.state.label_header} />
                    </Dialog>
                    <Dialog header={this.state.label_header}
                        visible={this.state.visibleZona}
                        width="500px" modal={true}
                        onHide={this.onHideZona}>
                        <NuevaZona
                            id={this.state.id_seleccionado}
                            key={this.state.key_form}
                            updateData={this.updateData}
                            onHide={this.onHideZona} 
                            paises={this.state.paises}/>
                    </Dialog>
                </div>
            </div>
        )
    }

    onSelectionChange(e) {
        if (e.selection.labelcompleto.substring(0, 1) == 'F') {
            this.setState({
                ItemPadreSeleccionado: e.selection.data,
                visibleFrente: true, key_form: Math.random(),
                id_seleccionado: e.selection.id,
                label_header: e.selection.labelcompleto + " ",
            })
        } else {
            if (e.selection.labelcompleto.substring(0, 1) == 'Z') {
                this.setState({
                    ItemPadreSeleccionado: e.selection.data,
                    visible: true, key_form: Math.random(),
                    id_seleccionado: e.selection.id,
                    label_header: e.selection.labelcompleto + " ",
                })
            }
        }
    }

    onHide(event) {
        this.setState({ visible: false });
    }

    onHideFrente(event) {
        this.setState({ visibleFrente: false });
    }

    onHideZona(event) {
        this.setState({ visibleZona: false });
    }

    activarCrearZona(event){
        this.setState({ visibleZona: true });
    }

    handlePaisChange(event) {
        this.setState({ [event.target.name]: event.target.value, blocking: true }, this.updateData)
    }

    updateData() {
        if (true) {
            axios.get("http://localhost:7090/Proyecto/UbicacionGeografica/GetZonasArbol", {id: this.state.id_pais})
            .then((response) => {
                console.log(response)
                this.setState({ data: response.data, key: Math.random() })
            })
            .catch((error) => {
                console.log(error);
            });
        } else {
            console.log("id_pais == 0")
        }
    }

    GetPaises() {
        axios.get("/Proyecto/UbicacionGeografica/GetPaisesApi")
            .then((response) => {
                this.setState({ paises: response.data })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    getPaisesForSelect() {
        return (
            this.state.paises.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                )
            })
        );
    }

}

ReactDOM.render(
    <CreateTreeItems />,
    document.getElementById('content-tree')
);