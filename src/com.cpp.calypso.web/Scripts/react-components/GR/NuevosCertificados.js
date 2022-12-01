import React from 'react';
import axios from 'axios';
import moment from 'moment';
import CurrencyFormat from 'react-currency-format';

import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class NuevosCertificados extends React.Component {
    constructor(props) {
        super(props)
        
        this.state = {
            select: [],
        }
        this.CodigoFormat = this.CodigoFormat.bind(this);
        this.DescripcionFormat = this.DescripcionFormat.bind(this)
        this.onRowSelect = this.onRowSelect.bind(this)
        this.HandleSubmit = this.HandleSubmit.bind(this);
        this.FechaFormat = this.FechaFormat.bind(this);
        this.MontoFormat = this.MontoFormat.bind(this);
        this.onSelectAll = this.onSelectAll.bind(this);
    }

    onRowSelect(row, isSelected, e){;
        if(isSelected){
            this.setState({select: [...this.state.select, row.Id]})
        } else {
            var items = this.state.select.filter((item) => {
                return item != row.Id;
            })
            this.setState({select: items})
        }
        
    }

    onSelectAll(isSelected, rows) {
        if (isSelected) {
            var array = [];
            for (let i = 0; i < rows.length; i++) {
                array.push(rows[i].Id)
            }
            this.setState({select: array})
        } else {
          this.setState({select: []})
        }
    }

    render() {
        const selectRowProp = {
            mode: 'checkbox',
            clickToSelect: true,
            bgColor: '#81D4FA',
            showOnlySelected: true,
            onSelect: this.onRowSelect,
            onSelectAll: this.onSelectAll
          };
          const options = {
            noDataText: 'No hay datos para mostrar'
            // withoutNoDataText: true, // this will make the noDataText hidden, means only showing the table header
          };
        return (
            <div>
                <BootstrapTable data={ this.props.certificados } options={ options } hover={true} pagination={true} selectRow={ selectRowProp } >

                    <TableHeaderColumn 
                    dataField='Proyecto'
                    width={'12%'}
                    dataFormat={this.CodigoFormat}
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}  
                    filter={ { type: 'TextFilter', delay: 500 } } 
                    dataSort={true}>Proyecto</TableHeaderColumn>

                    <TableHeaderColumn 
                    dataField='Proyecto'
                    dataFormat={this.DescripcionFormat}
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal', textAlign: 'center' }}  
                    filter={ { type: 'TextFilter', delay: 500 } } 
                    dataSort={true}>Descripción</TableHeaderColumn>

                    <TableHeaderColumn 
                    dataField='numero_certificado'
                    width={'20%'}
                    thStyle={{ whiteSpace: 'normal', textAlign: 'center' }} 
                    filter={ { type: 'TextFilter', delay: 500 } } 
                    dataSort={true}># Certificado</TableHeaderColumn>

                    <TableHeaderColumn 
                    width={'12%'}
                    dataField='fecha_emision'
                    dataFormat={this.FechaFormat}
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal', textAlign: 'center' }} 
                    filter={ { type: 'TextFilter', delay: 500 } } 
                    dataSort={true}>Fecha</TableHeaderColumn>


                    <TableHeaderColumn 
                    dataField='monto_certificado'
                    isKey
                    width={'20%'}
                    thStyle={{ whiteSpace: 'normal', textAlign: 'center' }} 
                    dataFormat={this.MontoFormat} 
                    tdStyle={{ textAlign: 'right' }}
                    filter={ { type: 'TextFilter', delay: 500 } } 
                    dataSort={true}>Monto Certificado</TableHeaderColumn>

                </BootstrapTable>
                <hr/>
                <button onClick={this.HandleSubmit} className="btn btn-outline-primary">Guardar</button>
            </div>
        )
    }

    HandleSubmit(){
        if(this.state.select.length == 0){
            this.props.warnMessage("Selecciona al menos un certificado")
        } else {
            this.props.Block();

            var uniqueArray = this.state.select.filter((item, pos) => {
                return this.state.select.indexOf(item) == pos;
            })
            this.setState({select: uniqueArray})
            axios.post("/proyecto/DetalleGR/Create/",{
                ids: uniqueArray,
                GrId: document.getElementById('GRId').className
            })
            .then((response) => {
                this.props.GetDetalles();
                this.props.GetCertificados();
                this.props.GetMonto();
                this.props.onHide();
                this.props.successMessage("Se insertaron " + response.data + " registros.")
                this.props.ClearCertificados();
            })
            .catch((error) => {
                this.props.warnMessage("Intentalo más tarde")
                console.log(error);    
            });
        }
    }

    CodigoFormat(cell, row){
        return cell.codigo
    }

    DescripcionFormat(cell, row){
        return cell.descripcion_proyecto
    }

    FechaFormat(cell, row){
        return moment(cell).format("DD/MM/YYYY")
    }

    MontoFormat(cell, row){
        return <CurrencyFormat value={cell} displayType={'text'} thousandSeparator={true} prefix={'$'} />
    }
    
}