import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';

import { Growl } from 'primereact/components/growl/Growl';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Dialog } from 'primereact/components/dialog/Dialog';
import CurrencyFormat from 'react-currency-format';
import CrearComputo from './AvanceObra/CrearComputoEAC';

class AvanceObra extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            blocking: true,
            data_table: [],

            visibleComputo: false,
        }

        this.successMessage = this.successMessage.bind(this)
        this.warnMessage = this.warnMessage.bind(this)
        this.onHideComputo = this.onHideComputo.bind(this);
        this.PresupuestoFormat = this.PresupuestoFormat.bind(this);
        this.NumberFormat = this.NumberFormat.bind(this);
        this.PriceFormat = this.PriceFormat.bind(this);
        this.Delete = this.Delete.bind(this);
        this.DeleteButton = this.DeleteButton.bind(this);
        this.RenderButtons = this.RenderButtons.bind(this);
    }
    componentDidMount() {
        this.GetData();
    }

    PresupuestoFormat(cell, row) {

        if (row.presupuestado) {
            return "SI"
        }
        return "NO"
    }

    NumberFormat(cell, row) {
        return <CurrencyFormat value={cell} displayType={'text'} thousandSeparator={true} />
    }

    PriceFormat(cell, row) {
        return <CurrencyFormat value={cell} displayType={'text'} thousandSeparator={true} prefix={'$'} />
    }

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.blocking}>

                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={10000}></Growl>
                <div className="row" style={{ marginBottom: '1em' }}>
                    {this.RenderButtons()}
                </div>
                <div className="row" style={{ marginTop: '1em' }}>
                    <div className="col">

                        <BootstrapTable id="table-excel" data={this.state.data} hover={true} pagination={true}>
                            <TableHeaderColumn
                                dataField='nombre_padre'
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                isKey
                            >Nivel Superior</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='nombre_item'
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Actividad</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='item_codigo'
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Código Item</TableHeaderColumn>


                            <TableHeaderColumn
                                dataField='budget'
                                dataFormat={this.NumberFormat}
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Budget</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='cantidad_eac'
                                dataFormat={this.NumberFormat}
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>C.EAC</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='cantidad_acumulada_anterior'
                                dataFormat={this.NumberFormat}
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>C.Anterior</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='cantidad_diaria'
                                dataFormat={this.NumberFormat}
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>C.Diaria</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='cantidad_acumulada'
                                dataFormat={this.NumberFormat}
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>C.Acumulada</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='precio_unitario'
                                dataFormat={this.PriceFormat}
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>P.U</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='total'
                                dataFormat={this.PriceFormat}
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px', textAlign: 'right' }}
                                filter={{ type: 'TextFilter', delay: 500 }}
                                dataSort={true}>Total</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='presupuestado'
                                dataFormat={this.PresupuestoFormat}
                                tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                dataSort={true}>Presupuestado</TableHeaderColumn>

                            <TableHeaderColumn
                                dataField='Id'
                                width={'8%'}
                                dataFormat={this.DeleteButton}
                                thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}></TableHeaderColumn>
                        </BootstrapTable>
                    </div>
                </div>


                <Dialog header="Item" visible={this.state.visibleComputo} width="1100px" modal={true} onHide={this.onHideComputo}>
                    <CrearComputo successMessage={this.successMessage} warnMessage={this.warnMessage} />
                </Dialog>

            </BlockUi>
        )
    }

    RenderButtons(){
        if(document.getElementById('Aprobado').className === '0'){
            return(
                <div className="col">
                    <button
                        style={{ marginLeft: '0.3em' }}
                        className="btn btn-outline-primary"
                        onClick={() => window.location.href = "/Proyecto/DetalleAvanceObra/Create/" + document.getElementById('AvanceId').className}>Carga Masiva</button>

                    <button
                        style={{ marginLeft: '0.3em' }}
                        className="btn btn-outline-primary" onClick={() => window.location.href = "/Proyecto/DetalleAvanceObra/CreateDetalleAvance/" + document.getElementById('AvanceId').className}>Nuevo Avance</button>

                    <button
                        style={{ marginLeft: '0.3em' }}
                        className="btn btn-outline-primary" onClick={() => this.setState({ visibleComputo: true })}>Nuevo item</button>
                </div>
            )
        }
    }

    GetData() {
        axios.post("/proyecto/AvanceObra/GetDetallesAvanceObra/" + document.getElementById('AvanceId').className, {})
            .then((response) => {
                this.setState({ data: response.data, blocking: false })
            })
            .catch((error) => {
                console.log(error);
            });
    }

    DeleteButton(cell, row){
        if(document.getElementById('Aprobado').className === '0'){

            return(
                <>
              
                <button className="btn btn-outline-danger"
                onClick={() => { if (window.confirm('Estás segur@?')) this.Delete(cell) } }>
                    <i className="fa fa-trash"></i>
                </button>
                </>
            )  
        }else{

            return(
                <>
              
                <button className="btn btn-outline-danger" disabled
                onClick={() => { if (window.confirm('Estás segur@?')) this.Delete(cell) } }>
                    <i className="fa fa-trash"></i>
                </button>
                </>
            )  
        }
      
    }

    Delete(id){
        this.setState({blocking: true})
        axios.post("/proyecto/DetalleAvanceObra/Delete/" + id,{
        })
        .then((response) => {
            this.GetData();
            this.successMessage("Registro eliminado");
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
    }

    successMessage(msg) {
        this.setState({ message: msg }, this.showSuccess)
    }

    warnMessage(msg) {
        this.setState({ message: msg }, this.showWarn)
    }

    onHideComputo() {
        this.setState({ visibleComputo: false })
    }

}



ReactDOM.render(
    <AvanceObra />,
    document.getElementById('content')
);