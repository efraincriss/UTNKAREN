import React from 'react';
import axios from 'axios';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import CurrencyFormat from 'react-currency-format';
import { Growl } from 'primereact/components/growl/Growl';
import { Dialog } from 'primereact/components/dialog/Dialog';

export default class ComputosPresupuestosTable extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            Id: 0, // Id Computo Seleccionado
            visibleajustado: false,
            item_nombre: '',
            item_padre_nombre: '',
            cantidad: 0,
            precio_base: 0.0,
            precio_ajustado: 0.0,


        }
        this.EditarAjustado = this.EditarAjustado.bind(this);
        this.MostrarFormularioAjustado = this.MostrarFormularioAjustado.bind(this);
        this.OcultarFormularioAjustado = this.OcultarFormularioAjustado.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    render() {
        return (

            <div className="row">
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <div style={{ width: '100' }}>
                    <div className="card">
                        <div className="card-body">
                            <div className="row" style={{ marginBottom: '1em' }}>
                                <div className="col" align="right">
                                    {this.props.Oferta != null && this.props.Oferta.NombreEstadoAprobacion != "Emitido" &&
                                        <button
                                            className="btn btn-outline-primary"
                                            style={{ marginRight: '0.3em' }}
                                            onClick={() => this.GenerarPresupuesto()}
                                        >Generar Presupuesto</button>
                                    }

                                    <button
                                        className="btn btn-outline-primary"
                                        style={{ marginRight: '0.3em' }}
                                        onClick={() => this.DescargarMatriz()}
                                    >Matriz Presupuesto</button>
                                </div>
                            </div>


                            <BootstrapTable data={this.props.data} hover={true} pagination={true}>
                                <TableHeaderColumn
                                    isKey
                                    dataField='wbs_actividad'
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}
                                >Actividad</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='item_padre_codigo'
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}>Código Padre</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='item_padre_nombre'
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}>Ítem Padre</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='item_codigo'
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}>Código Ítem</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='item_nombre'
                                    tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}>Ítem</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='precio_base'
                                    dataFormat={this.PreciosFormato}
                                    tdStyle={{ whiteSpace: 'normal', textAlign: 'right', fontSize: '11px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}>P.U Costo Directo</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='precio_ajustado'
                                    dataFormat={this.PreciosFormato}
                                    tdStyle={{ whiteSpace: 'normal', textAlign: 'right', fontSize: '11px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}>P.U Ajustado</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='precio_incrementado'
                                    dataFormat={this.PreciosFormato}
                                    tdStyle={{ whiteSpace: 'normal', textAlign: 'right', fontSize: '11px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}>P.U AIU</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='cantidad'
                                    dataFormat={this.CantidadFormato}
                                    tdStyle={{ whiteSpace: 'normal', textAlign: 'right', fontSize: '11px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}>Cantidad</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='precio_unitario'
                                    dataFormat={this.PreciosFormato}
                                    tdStyle={{ whiteSpace: 'normal', textAlign: 'right', fontSize: '11px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}>P.U</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='costo_total'
                                    dataFormat={this.PreciosFormato}
                                    tdStyle={{ whiteSpace: 'normal', textAlign: 'right', fontSize: '11px' }}
                                    thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                    filter={{ type: 'TextFilter', delay: 500 }}
                                    dataSort={true}>Total</TableHeaderColumn>

                                <TableHeaderColumn
                                    dataField='Id'

                                    dataFormat={this.GenerarBotonesPresupuestos.bind(this)}></TableHeaderColumn>
                            </BootstrapTable>

                            <Dialog header="Edición Computo" visible={this.state.visibleajustado} width="800px" modal={true} onHide={this.OcultarFormularioAjustado}>



                                <form onSubmit={this.EditarAjustado}>

                                    <div className="row">
                                        <div className="col">
                                            <div className="form-group">
                                                <b><label htmlFor="label">Item Padre:</label></b><br />
                                                <label align="right" htmlFor="label">{this.state.item_padre_nombre}</label><br />
                                            </div>

                                        </div>
                                        <div className="col">

                                            <div className="form-group">
                                                <b> <label htmlFor="label">Descripción:</label></b><br />

                                                <label align="right" htmlFor="label">{this.state.item_nombre}</label><br />


                                            </div>

                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col">

                                            <div className="form-group">
                                                <b> <label htmlFor="label">Cantidad:</label></b><br />

                                                <label align="right" htmlFor="label">{this.state.cantidad}</label><br />


                                            </div>
                                        </div>
                                        <div className="col">
                                            <div className="form-group">
                                                <b> <label htmlFor="label">Precio Base:</label></b><br />

                                                <label align="right" htmlFor="label"><CurrencyFormat value={this.state.precio_base} displayType={'text'} thousandSeparator={true} prefix={'$'} /></label><br />


                                            </div>
                                        </div>

                                    </div>
                                    <div className="row">

                                        <div className="col">

                                            <div className="form-group">
                                                <b> <label htmlFor="label">Precio  Ajustado</label></b>
                                                $<input
                                                    type="number"
                                                    name="precio_ajustado"
                                                    className="form-control"
                                                    onChange={this.handleChange}
                                                    min="0" value="0" step="any"
                                                    value={this.state.precio_ajustado}
                                                />
                                            </div>
                                        </div>
                                        <div className="col">
                                        </div>

                                    </div>


                                    <button type="submit" className="btn btn-outline-primary" style={{ marginLeft: '0.3em' }}>Guardar</button>
                                    <button type="button" className="btn btn-outline-primary" icon="fa fa-fw fa-ban" style={{ marginLeft: '0.3em' }} onClick={this.OcultarFormularioAjustado}>Cancelar</button>
                                </form>





                            </Dialog>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    OcultarFormularioAjustado() {

        this.setState({
            visibleajustado: false,
            ComputoSeleccionado: null,
            precio_base: 0.0,
            precio_ajustado: 0.0,
            item_nombre: '',
            cantidad: 0,
            item_padre_nombre: '',
            Id: 0
        });
    }
    MostrarFormularioAjustado(row) {
        console.log(row);
        this.setState({
            visibleajustado: true,
            ComputoSeleccionado: row,
            precio_base: row.precio_base,
            precio_ajustado: row.precio_ajustado,
            item_nombre: row.item_nombre,
            cantidad: row.cantidad,
            item_padre_nombre: row.item_padre_nombre,
            Id: row.Id
        });
    }
    GenerarBotonesPresupuestos(cell, row) {
        return (
            <div>

                <button className="btn btn-outline-info btn-sm" onClick={() => { this.MostrarFormularioAjustado(row) }} style={{ float: 'left', marginRight: '0.3em' }}>Editar</button>

            </div>
        )
    }
    DescargarMatriz() {
        this.props.Loading();
        axios.get("/proyecto/Presupuesto/GenerarPrespuesto?OfertaId=" + this.props.OfertaId, { responseType: 'arraybuffer' })
            .then((response) => {
                console.log(response.headers["content-disposition"])
                var nombre=response.headers["content-disposition"].split('=');

                const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }));
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', nombre[1]);
               // link.setAttribute('download', 'Matriz Presupuesto.xlsx');
                document.body.appendChild(link);
                link.click();
                this.props.CancelarLoading();
                this.growl.show({ life: 5000, severity: 'success', summary: '', detail: "Matriz Descargada" });
            })
            .catch((error) => {
                console.log(error);
                this.props.CancelarLoading();
                this.growl.show({ life: 5000, severity: 'warn', summary: '', detail: "Ocurrio un Error" });
            });
    }

    GenerarPresupuesto() {
        this.props.Loading();

        axios.post("/Proyecto/OfertaPresupuesto/ActualizarCostos/" + this.props.OfertaId, {})
            .then((response) => {
                console.log(response.data);
                this.props.ConsultarComputos();
                this.props.CancelarLoading();
                this.props.ConsultarOferta();
                this.growl.show({ life: 5000, severity: 'success', summary: '', detail: "Presupuesto Generado" })
            })

            .catch((error) => {
                console.log(error);
                this.props.CancelarLoading();
                this.growl.show({ life: 5000, severity: 'warn', summary: '', detail: "Error" })

            });
    }

    EditarAjustado() {
        event.preventDefault();

        this.props.Loading();

        axios.post("/Proyecto/ComputoPresupuesto/EditPrecioAjustado/", {
            Id: this.state.Id,
            precio_ajustado: this.state.precio_ajustado,

        })
            .then((response) => {
                console.log(response.data);
                this.props.ConsultarComputos();
                this.props.CancelarLoading();
                this.growl.show({ life: 5000, severity: 'success', summary: '', detail: "Computo Modificado" })
                this.setState({
                    visibleajustado: false,
                    ComputoSeleccionado: null,
                    precio_base: 0.0,
                    precio_ajustado: 0.0,
                    item_nombre: '',
                    item_padre_nombre: '',
                    Id: 0,
                    cantidad: 0
                });
                this.props.ConsultarComputos();
            })

            .catch((error) => {
                console.log(error);
                this.props.CancelarLoading();
                this.growl.show({ life: 5000, severity: 'warn', summary: '', detail: "Error" })

            });
    }

    handleChange(event) {
        this.setState({ [event.target.name]: event.target.value });
    }
    Redireccionar(accion, id) {
        if (accion === "Presupuesto") {
            window.location.href = "/Proyecto/Presupuesto/Details/" + id;
        }
    }

    CodigoPadreFormato(cell, row) {
        return cell.item_padre
    }

    CodigoFormato(cell, row) {
        return cell.codigo
    }

    NombreFormato(cell, row) {
        return cell.nombre
    }

    PreciosFormato(cell, row) {
        return <CurrencyFormat value={cell} displayType={'text'} thousandSeparator={true} prefix={'$'} />
    }

    CantidadFormato(cell, row) {
        return <CurrencyFormat value={cell} displayType={'text'} thousandSeparator={true} />
    }
}