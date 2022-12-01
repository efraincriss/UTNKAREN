import React from 'react';
import ReactDOM from 'react-dom';
import moment from 'moment';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import {Growl} from 'primereact/components/growl/Growl';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import CurrencyFormat from 'react-currency-format';


  class VerFacturas extends React.Component{
    constructor(props){
        super(props);

        this.state = {
            blocking:true,
            lista_facturas:[],
            lista_empresas:[],
            lista_clientes:[],
            visible_editar_factura: false,

            //EDITAR EMPRESA
            EmpresaId:0,
            ClienteId:0,
            numero_documento:0,
            tipo_documento:"",
            fecha_emision:null,
            fecha_vencimiento:null,
            descripcion:"",
            proyecto:"",
            codigosap:"",
            documento_compensación:"",
            codigoauxicial:"", //ov
            codigoprincipal:"", //OS
            valor_importe:0,
            valor_iva:0,
            valor_total:0,
            valor_a_cobrar:0,
            
            //Retenciones
            documento_retencion:"",
            total_retencion:"",
            retencion_iva:"",
            retencion_ir:"",
            //Cobros
            documento_cobro:"",
            valor_cobrado:0,



            key_form: 89247,
         }
        this.onHideVisibleActividadForm = this.onHideVisibleActividadForm.bind(this);
        this.ObtenerFacturas = this.ObtenerFacturas.bind(this);
        this.EmpresaNombreFormato = this.EmpresaNombreFormato.bind(this);
        this.ClienteNombreFormato = this.ClienteNombreFormato.bind(this);
        this.dateFormat = this.dateFormat.bind(this);
        this.enumFormatter = this.enumFormatter.bind(this);
        this.VerFactura=this.VerFactura.bind(this);
        this.CobroFormato=this.CobroFormato.bind(this);
        this.CobroDato=this.CobroDato.bind(this);
      

    }
    componentDidMount() {
        console.log("dsads");
        this.ObtenerFacturas();
       }

    ObtenerFacturas(){ 
        axios.get("/proyecto/Factura/ObtenerCobros/",{})
        .then((response) => {
            console.log(response.data); 
         this.setState({lista_facturas: response.data, blocking:false})
        })
        .catch((error) => {
            console.log(error);    
        });
        
    }
    VerFactura(id){ 
        console.log(id);
        window.location.href = "/proyecto/Factura/DetailsCobros/"+id;
            }
    OcultarFormularioEdicion(){ 
        this.setState({visible_editar_factura: false})         
    }
   
  
        generateButton(cell, row){
        return(
            <div>
                <button className="btn btn-outline-success btn-sm" onClick={() => {this.VerFactura(row.Id)}}   style={{float:'left', marginRight:'0.3em'}}>Ver</button>
                {/*<button className="btn btn-outline-info btn-sm"  onClick={() => {this.MostrarFormularioEdicion(row.Id)}} style={{float:'left', marginRight:'0.3em'}}>Editar</button>*/}
            </div>
        )
    }

    dateFormat(cell, row){
        if(cell === null){
            return(
                "dd/mm/yy"
            )
        }
        return(
            moment(cell).format('DD/MM/YYYY')
        )
    }

   

    render(){
        return(
         <div>
             <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
             <BlockUi tag="div" blocking={this.state.blocking}>
             <BootstrapTable data={this.state.lista_facturas} hover={true} pagination={ true } >
             <TableHeaderColumn dataField="Empresa" 
                                       dataFormat={this.EmpresaNombreFormato} 
                                       filter={ { type: 'TextFilter', delay: 500 } }  
                                       dataAlign="center" 
                                       dataSort={true}
                                       width={"10%"}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       >Empresa</TableHeaderColumn>

                    <TableHeaderColumn dataField="Cliente" 
                                       dataFormat={this.ClienteNombreFormato}  
                                       filter={ { type: 'TextFilter', delay: 500 } }  
                                       dataAlign="center" 
                                       dataSort={true}
                                       width={"10%"}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       >Cliente</TableHeaderColumn>
                        
                        <TableHeaderColumn  dataField="descripcion"  
                                         
                                         filter={ { type: 'TextFilter', delay: 500 } } 
                                         dataSort={true}
                                         width={"10%"}
                                         tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                         thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                         >Descripción</TableHeaderColumn>
                                          <TableHeaderColumn  dataField="documento_compensacion"  
                                          isKey={true}
                                       filter={ { type: 'TextFilter', delay: 500 } } 
                                       dataSort={true}
                                       width={"10%"}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       >Doc. Compensación</TableHeaderColumn>
                      <TableHeaderColumn dataField="fecha_documento" 
                                         filter={ { type: 'TextFilter', delay: 500 } } 
                                         dataFormat={this.dateFormat} 
                                         dataSort={true}
                                         width={"10%"}
                                         tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                         thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                         >Fecha Documento</TableHeaderColumn>
                      
                      <TableHeaderColumn dataField="monto"   
                                           dataFormat={this.CobroFormato} 
                                         filter={ { type: 'TextFilter', delay: 500 } } 
                                         dataSort={true}
                                         width={"10%"}
                                         tdStyle={{ whiteSpace: 'normal', fontSize: '10px',textAlign: 'right'  }}
                                         thStyle={{ whiteSpace: 'normal', fontSize: '10px' ,textAlign: 'center' }}
                                         >Monto Total</TableHeaderColumn>
                     
                      <TableHeaderColumn dataField='Operaciones'
                                         width={"15%"} 
                                         dataFormat={this.generateButton.bind(this)}
                                         tdStyle={{ whiteSpace: 'normal', fontSize: '10px',textAlign: 'right'  }}
                                         thStyle={{ whiteSpace: 'normal', fontSize: '10px' ,textAlign: 'center' }}
                                         >Operaciones</TableHeaderColumn>
                       
                </BootstrapTable>
                
            </BlockUi>  
         </div>   
        )
    }


    onHideVisibleActividadForm(event){
        this.setState({visible_actividades_form: false});
    }
    enumFormatter(cell, row, enumObject) {
        return enumObject[cell];
      }

        EmpresaNombreFormato(cell, row) {
        return cell.razon_social;
    }
    ClienteNombreFormato(cell, row) {
        return cell.razon_social;
    }
    CobroFormato(cell, row) {
        return <CurrencyFormat value={cell} displayType={'text'} thousandSeparator={true} prefix={'$'} />
    }
    CobroDato(cell, row) {
        return cell.documento_compensacion;
    }
    
  }

ReactDOM.render(
    <VerFacturas />,
    document.getElementById('content-cobro')
  );
