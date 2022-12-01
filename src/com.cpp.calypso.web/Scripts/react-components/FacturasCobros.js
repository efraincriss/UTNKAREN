import React from 'react';
import ReactDOM from 'react-dom';
import moment from 'moment';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import {Dialog} from 'primereact-v2/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';
import { Dropdown } from 'primereact/components/dropdown/Dropdown';
import {Button} from 'primereact/components/button/Button';
import {Calendar} from 'primereact/components/calendar/Calendar';
import {InputText} from 'primereact-v2/components/inputtext/InputText';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import CurrencyFormat from 'react-currency-format';

  class FacturaporCobro extends React.Component{
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
        this.showForm = this.showForm.bind(this);
        this.OnEdit = this.OnEdit.bind(this);
        this.ObtenerFacturas = this.ObtenerFacturas.bind(this);
        this.ObtenerEmpresas = this.ObtenerEmpresas.bind(this);
        this.ObtenerClientes = this.ObtenerClientes.bind(this);
        this.EmpresaNombreFormato = this.EmpresaNombreFormato.bind(this);
        this.ClienteNombreFormato = this.ClienteNombreFormato.bind(this);
        this.dateFormat = this.dateFormat.bind(this);
        this.OcultarFormularioEdicion = this.OcultarFormularioEdicion.bind(this);
        this.enumFormatter = this.enumFormatter.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.VerFactura=this.VerFactura.bind(this);
        this.CobroFormato=this.CobroFormato.bind(this);
        this.FacturaCodigo=this.FacturaCodigo.bind(this);
        this.CobroDato=this.CobroDato.bind(this);
        this.FacturaMonto=this.FacturaMonto.bind(this);
        this.VerFactura=this.VerFactura.bind(this);
        this.Eliminar=this.Eliminar.bind(this);
        

    }
    componentDidMount() {
        this.ObtenerFacturas();
        this.ObtenerEmpresas();
        this.ObtenerClientes();
    }
  
    ObtenerFacturas(){ 
        axios.get("/proyecto/Factura/ObtenerFacturaporCobro/"+ document.getElementById('FacturaId').className,{})
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
        window.location.href = "/proyecto/Factura/Details/"+id;
        
    }
    OcultarFormularioEdicion(){ 
        this.setState({visible_editar_factura: false})         
    }
    MostrarFormularioEdicion(e){ 
        console.log(e);    
        this.setState({visible_editar_factura: true});
        axios.get("/proyecto/Factura/ObtenerFacturasD/"+e,{})
        .then((response) => {
            console.log(response.data);
            this.setState({
                EmpresaId: response.data.EmpresaId,
                ClienteId:response.data.ClienteId, 
                numero_documento:response.data.numero_documento,
              // descripcion=response.data.descripcion,
               
                /* fecha_emision:response.data.fecha_emision,
                codigosap:response.data.codigosapId,
                 tipo_documento:""+reponse.data.tipo_documento,
                valor_iva:response.data.valor_iva,
                valor_total:response.data.valor_total,
                valor_a_cobrar:response.data.valor_a_cobrar,
                valor_importe:response.data.valor_importe,
                valor_cobrado:response.data.valor_cobrado
                */     

            });
            
        })
        .catch((error) => {
            this.growl.show({severity: 'warn', summary: 'Error', detail: 'No se Cargo la Factura'});
            
        });   
    }
    ObtenerEmpresas(){ 
        axios.get("/proyecto/Factura/ObtenerFacturasE/",{})
        .then((response) => {
            var empresas = response.data.map(i => {
                return {label: i.razon_social, value: i.Id }
            }) 
            this.setState({lista_empresas: empresas, blocking:false})
        })
        .catch((error) => {
            console.log(error);    
        });
    }
    
    ObtenerClientes(){ 
        axios.get("/proyecto/Factura/ObtenerFacturasC/",{})
        .then((response) => {
              var clientes = response.data.map(i => {
                return {label: i.razon_social, value: i.Id }
            })
            this.setState({lista_clientes: clientes, blocking:false})
        })
        .catch((error) => {
            console.log(error);    
        });
    }
    generateButton(cell, row){
        return(
            <div>
                <button className="btn btn-outline-success btn-sm" onClick={() => {this.VerFactura(row)}}   style={{float:'left', marginRight:'0.3em'}}>Ver</button>
                <button className="btn btn-outline-danger btn-sm" onClick={() => { if (window.confirm('Estás seguro de eliminar este Cobro')) this.Eliminar(row.CobroId)}} style={{ float: 'left', marginRight: '0.3em' }}>Eliminar</button>
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
            moment(cell).format('DD-MM-YYYY')
        )
    }
    FacturaMonto(cell, row) {
        return <CurrencyFormat value={cell.valor_total} displayType={'text'} thousandSeparator={true} prefix={'$'} />
        
    }
    FacturaCobrado(cell, row) {
        return <CurrencyFormat value={cell.valor_cobrado} displayType={'text'} thousandSeparator={true} prefix={'$'} />
        
    }
    
    handleSubmit(event){

    
            axios.post("/Proyecto/DetalleAvanceIngenieria/Create",{
                Id: this.state.Id,
                AvanceIngenieriaId: document.getElementById('AvanceIngenieriaId').className,
                tipo_registro: this.state.registro,
                ComputoId: this.state.computo,
                cantidad_horas: this.state.cantidad_horas,
                vigente: true,
                valor_real: 0,
                fecha_real: new Date()
            })
            .then((response) => {
                this.props.updateData();
                this.props.showSuccess("Ingresado correctamente")
                this.props.onHide();
            })
            .catch((error) => {
                console.log(error);
                this.props.showWarn("Intentalo mas tarde")
            });
                
    }

    render(){
        return(
         <div>
             <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
             <BlockUi tag="div" blocking={this.state.blocking}>
             <BootstrapTable data={this.state.lista_facturas} hover={true} pagination={ true } >
                    <TableHeaderColumn dataField="Factura" 
                                       filter={ { type: 'RegexFilter', delay: 500 } } 
                                    
                                        dataFormat={this.FacturaCodigo}      
                                       isKey={true} 
                                       width={"10%"}
                                       dataAlign="center" 
                                       dataSort={true}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        > #Factura</TableHeaderColumn>
                    <TableHeaderColumn  dataField="Cobro"  
                                         dataFormat={this.CobroDato} 
                                       filter={ { type: 'TextFilter', delay: 500 } } 
                                       dataSort={true}
                                       width={"10%"}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       >Doc. Compensación</TableHeaderColumn>
                    <TableHeaderColumn  dataField="Factura"  
                                         dataFormat={this.CobroDescripcion} 
                                       filter={ { type: 'TextFilter', delay: 500 } } 
                                       dataSort={true}
                                       width={"10%"}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       >Descripción</TableHeaderColumn>
                    <TableHeaderColumn dataField="fecha_emision" 
                                       filter={ { type: 'TextFilter', delay: 500 } } 
                                       dataFormat={this.dateFormat} 
                                       dataSort={true}
                                       width={"10%"}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px',textAlign: 'right' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px',textAlign: 'center' }}
                                       >Fecha</TableHeaderColumn>
                    
                    <TableHeaderColumn dataField="Factura"   
                                        dataFormat={this.FacturaMonto} 
                                       filter={ { type: 'TextFilter', delay: 500 } } 
                                       dataSort={true}
                                       width={"10%"}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px',textAlign: 'right' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px',textAlign: 'right' }}
                                       >Monto Total</TableHeaderColumn>
                    <TableHeaderColumn dataField="Factura"   
                                        dataFormat={this.FacturaCobrado} 
                                       filter={ { type: 'TextFilter', delay: 500 } } 
                                       dataSort={true}
                                       width={"10%"}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px',textAlign: 'right' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px',textAlign: 'right' }}
                                       >Monto Cobrado</TableHeaderColumn>
                     <TableHeaderColumn dataField='Operaciones'
                                       width={"15%"} 
                                       dataFormat={this.generateButton.bind(this)}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       >Operaciones</TableHeaderColumn>
      
                </BootstrapTable>

            </BlockUi>  
         </div>   
        )
    }
  
    VerFactura(row){ 
        
        window.location.href = "/proyecto/Factura/Details/"+row.FacturaId+"?id2="+row.CobroId;
    }
    CobroDescripcion(cell, row) {
        return cell.descripcion;
    }
    onHideVisibleActividadForm(event){
        this.setState({visible_actividades_form: false});
    }
    enumFormatter(cell, row, enumObject) {
        return enumObject[cell];
      }

    FacturaCodigo(cell, row) {
        return cell.numero_documento;
    }
    EmpresaNombreFormato(cell, row) {
        return cell.Empresa.razon_social;
    }
    ClienteNombreFormato(cell, row) {
        return cell.Cliente.razon_social;
    }
    CobroFormato(cell, row) {
        return <CurrencyFormat value={cell.monto} displayType={'text'} thousandSeparator={true} prefix={'$'} />
    }
    CobroDato(cell, row) {
        return cell.documento_compensacion;
    }
    OnEdit(id){
  
        axios.post("/proyecto/Wbs/Delete/"+id,{})
        .then((response) => {
            if(response.data == "Ok"){            
                this.growl.show({severity: 'info', summary: 'Correcto', detail: 'Se Elimino Correctamente'});
            } else {  
                this.props.showWarn("La actividad tiene computos registrados");
            }
        })
        .catch((error) => {
            this.props.showWarn("No se puedo eliminar el registro");
            
        });
    }
    
    Eliminar(id){
  
        axios.post("/proyecto/Factura/Eliminar/"+id,{})
        .then((response) => {
            if(response.data == "OK"){            
                this.growl.show({severity: 'info', summary: 'Correcto', detail: 'Se Elimino Correctamente'});
                this.ObtenerFacturas();
            } else {  
                this.growl.show({severity: 'warn', summary: 'Error', detail: 'No se Elimino Cobro'});
            }
        })
        .catch((error) => {
            this.growl.show({severity: 'warn', summary: 'Error', detail: 'Ocurrio un Error'});
            
        });
    }
    
    showForm(){
        this.setState({
            visible_editar_factura: true,
            key_form: Math.random()
        })
    }
}

ReactDOM.render(
    <FacturaporCobro />,
    document.getElementById('content-facturacobro')
  );
