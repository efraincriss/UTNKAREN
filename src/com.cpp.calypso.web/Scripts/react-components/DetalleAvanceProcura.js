import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';
import {DataTable} from 'primereact/components/datatable/DataTable';
import {Column} from 'primereact/components/column/Column';
import CrearComputo from './AvanceObra/CrearComputo';
 class DetalleAvanceProcura extends React.Component {
    constructor(props){
        super(props);
                this.state = {
                visible: false,
                data:[],
                detallesseleccionados: [],
                AvanceProcuraId: 0,
                message: '',
                blocking: true,
                visibleComputo: false,
                    }
                    this.successMessage = this.successMessage.bind(this)
      this.warnMessage = this.warnMessage.bind(this)
                    this.onHideComputo = this.onHideComputo.bind(this);
                this.showForm = this.showForm.bind(this);
                this.GetData = this.GetData.bind(this);
                this.onHide = this.onHide.bind(this);
                this.handleSubmit = this.handleSubmit.bind(this);
                this.showSuccess = this.showSuccess.bind(this);
                this.showWarn = this.showWarn.bind(this);
    }

    componentDidMount(){
        this.GetData(); 
    }
    render(){
        return(
            <div>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <input type="button" className="btn btn-outline-primary" onClick={this.showForm} value="Nuevo Detalle"/>
                <button
                        style={{marginLeft: '0.3em'}}
                        className="btn btn-outline-primary" onClick={ () => this.setState({visibleComputo: true}) }>Nuevo item</button>
                <Dialog header="Seleccionar Detalles de Ordenes de Compra" visible={this.state.visible} width="70%" modal={true} onHide={this.onHide} >
                <BlockUi tag="div" blocking={this.state.blocking}>
                <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <button onClick={this.handleSubmit} className="btn btn-outline-indigo" style={{marginBottom: '1em'}}>Guardar</button>
                
                <DataTable value={this.state.data} header="Ordenes de Compra" 
                        selection={this.state.detallesseleccionados}  paginator={true} rows={10} rowsPerPageOptions={[5,10,20]} onSelectionChange={(e) => this.setState({detallesseleccionados: e.data})}>
                        <Column selectionMode="multiple" style={{width:'2em'}}/>
                        <Column field="OrdenCompra.nro_pedido_compra" header="Orden de Compra" filter="OrdenCompra.nro_pedido_compra" />
                        <Column field="Computo.Item.codigo" header="Código Item" filter="Computo.Item.codigo" />
                        <Column field="Computo.Item.nombre" header="Item"   style={{width: '200px'}} filter="Computo.Item.nombre"/>
                        <Column field="fechas" header="Fecha"   filter="fechas"/>
                        <Column field="tiporegistro" header="Tipo Registro" filter="tiporegistro" />
                        <Column field="porcentajes" header="Porcentaje"  style={{textAlign: 'right'}} filter="porcentajes" />
                        <Column field="valores" header="Valor" style={{textAlign: 'right'}} filter="valores" />
                        
                    </DataTable>
                    </BlockUi>
              </Dialog>
                  <Dialog header="Item" visible={this.state.visibleComputo} width="1100px" modal={true} onHide={this.onHideComputo}>
                 
                    <CrearComputo successMessage={this.successMessage} warnMessage={this.warnMessage}/>
                   
                </Dialog>
      
                </div>
             )
    }

    GetData(){
        axios.post("/proyecto/DetalleAvanceProcura/GetDetallesOrdenes",{
            id: document.getElementById('OfertaId').className,
            
        })
        .then((response) => {
            this.setState({data: response.data, blocking: false,
                AvanceProcuraId: document.getElementById('AvanceProcuraId').className            
            })
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    handleSubmit(event){
        event.preventDefault();
       
        axios.post("/proyecto/DetalleAvanceProcura/CrearDetalles",{
            detallesseleccionados:  this.state.detallesseleccionados.map((number) => number.Id),
          
            AvanceProcuraId:this.state.AvanceProcuraId
            
        })
        .then((response) => {
            console.log(response)
            if(response.data === "OK"){
                this.setState({detallesseleccionados:[], blocking: false,visible:false,
                    AvanceProcuraId: document.getElementById('AvanceProcuraId').className   
                     
            })
            window.location.href = "/Proyecto/AvanceProcura/Details/"+document.getElementById('AvanceProcuraId').className ;
        }
           
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    showForm(){
        this.setState({visible: true})
    }
    onHideComputo(){
        this.setState({visibleComputo: false})
    }
    
    onHide(event){
        this.setState({visible: false, blockSubmit: false});
    }       
    showSuccess() {
        this.growl.show({life: 5000,  severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({life: 5000,  severity: 'error', summary: 'Error', detail: this.state.message });
    }
    successMessage(msg){
        this.setState({message: msg}, this.showSuccess)
    }

    warnMessage(msg){
        this.setState({message: msg}, this.showWarn)
    }
}

ReactDOM.render(
    <DetalleAvanceProcura />,
    document.getElementById('content-detalle-procura')
  );