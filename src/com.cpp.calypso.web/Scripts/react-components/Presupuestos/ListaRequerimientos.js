import React from 'react';
import CurrencyFormat from 'react-currency-format';

export default class ListaRequerimientos extends React.Component {
    constructor(props){
        super(props)
        this.state = {

        }

        this.GenerarBotones = this.GenerarBotones.bind(this);
    }

    MontosFormato(cell, row) {
        if(cell==null){

            return <CurrencyFormat value={0} displayType={'text'} thousandSeparator={true} prefix={'$'} />
        }else{
        
        return <CurrencyFormat value={cell} displayType={'text'} thousandSeparator={true} prefix={'$'} />
        }
    }
    
    render(){
        return(
            <BootstrapTable data={ this.props.data } hover={true} pagination={true}>
             
                <TableHeaderColumn 
                dataField='proyecto_codigo' 
                isKey
                tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                filter={ { type: 'TextFilter', delay: 500 } } 
                dataSort={true}>Proyecto</TableHeaderColumn>

                <TableHeaderColumn 
                dataField='codigo' 
                tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                filter={ { type: 'TextFilter', delay: 500 } } 
                dataSort={true}>Cod_Trab</TableHeaderColumn>

                <TableHeaderColumn 
                dataField='descripcion' 
                tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                filter={ { type: 'TextFilter', delay: 500 } } 
                width={"20%"} 
                dataSort={true}>Descripción</TableHeaderColumn>
                 <TableHeaderColumn 
                dataField='estado_presupuesto_actual' 
                tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                filter={ { type: 'TextFilter', delay: 500 } } 
                dataSort={true}>Estado</TableHeaderColumn>
                <TableHeaderColumn 
                dataField='ultima_version' 
                tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                filter={ { type: 'TextFilter', delay: 500 } } 
                dataSort={true}>Rev</TableHeaderColumn>
                   <TableHeaderColumn 
                dataField='ultimo_origen' 
                tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                filter={ { type: 'TextFilter', delay: 500 } } 
                dataSort={true}>Origen</TableHeaderColumn>
                   <TableHeaderColumn 
                dataField='ultima_clase' 
                tdStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                filter={ { type: 'TextFilter', delay: 500 } } 
                dataSort={true}>Clase</TableHeaderColumn>

                    <TableHeaderColumn 
                    dataField='monto_construccion' 
                    dataFormat={this.MontosFormato} 
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal' }} 
                    filter={ { type: 'TextFilter', delay: 500 } } 
                    tdStyle={{ whiteSpace: 'normal', fontSize: '11px' ,textAlign: 'right'}}
                    width={"10%"} 
                    thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    dataSort={true}>
                    M. Construcción</TableHeaderColumn>
                    <TableHeaderColumn 
                    dataField='monto_ingenieria' 
                    dataFormat={this.MontosFormato} 
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal' }} 
                    filter={ { type: 'TextFilter', delay: 500 } } 
                    tdStyle={{ whiteSpace: 'normal', fontSize: '11px' ,textAlign: 'right'}}
                    width={"10%"} 
                    thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    dataSort={true}>
                    M. Ingeniería</TableHeaderColumn>
                    <TableHeaderColumn 
                    dataField='monto_procura' 
                    dataFormat={this.MontosFormato} 
                    tdStyle={{ whiteSpace: 'normal' }} 
                    thStyle={{ whiteSpace: 'normal' }} 
                    filter={ { type: 'TextFilter', delay: 500 } }
                    tdStyle={{ whiteSpace: 'normal', fontSize: '11px' ,textAlign: 'right'}}
                    thStyle={{ whiteSpace: 'normal', fontSize: '11px' }}
                    width={"10%"} 
                    dataSort={true}>
                    M. Suministros</TableHeaderColumn>

                    <TableHeaderColumn 
                dataField='Operaciones'
                width={'8%'}
                dataFormat={this.GenerarBotones.bind(this)}></TableHeaderColumn>
            </BootstrapTable>
        )
    }

    GenerarBotones(cell, row){
        return(
            <div>
                <button onClick={() => this.props.SeleccionarSr(row)} className="btn btn-sm btn-outline-indigo">Ver</button>
            </div>
        )
    }
}