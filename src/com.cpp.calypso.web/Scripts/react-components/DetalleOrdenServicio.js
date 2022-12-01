import React from 'react';
import ReactDOM from 'react-dom';
import Moment from 'moment';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';
import { Dropdown } from 'primereact/components/dropdown/Dropdown';
import {Button} from 'primereact/components/button/Button';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import CurrencyFormat from 'react-currency-format';

  class DetalleOrdenServicio extends React.Component{
    constructor(props){
        super(props);

        this.state = {
            blocking:true,
            lista_detalles:[],
            lista_items:[],
            visible: false,
            ItemId:0,
            valor_os:0.0
    
         }
       
        this.ObtenerDetalles=this.ObtenerDetalles.bind(this);
        this.ObtenerItem=this.ObtenerItem.bind(this);
             
                this.handleChange=this.handleChange.bind(this);
                this.onClick = this.onClick.bind(this);
                this.onHide = this.onHide.bind(this);
                this.OnEdit=this.OnEdit.bind(this);
                

    }
    componentDidMount() {
        this.ObtenerDetalles();
        this.ObtenerItem();
    }

    ObtenerDetalles(){ 
        axios.get("/proyecto/OfertaComercial/GetDetallesOrdenes/"+ document.getElementById('OrdenId').className,{})
        .then((response) => {
            console.log(response.data);
         this.setState({lista_detalles: response.data, blocking:false})
        })
        .catch((error) => {
            console.log(error);    
        });
        
    }
    ObtenerItem(){ 
        axios.get("/proyecto/OfertaComercial/GetItems/",{})
        .then((response) => {
            console.log(response.data);
         this.setState({lista_items: response.data, blocking:false})
        })
        .catch((error) => {
            console.log(error);    
        });
        
    }
    handleChange(event) {
        event.stopPropagation();

        this.setState({ [event.target.name]: event.target.value });
    }
    onClick(event) {
        this.setState({visible: true});
    }

    onHide(event) {
        this.setState({visible: false});
    }
    MontoFormato(cell, row) {
        return <CurrencyFormat value={cell} displayType={'text'} thousandSeparator={true} prefix={'$'} />
        
    }

    generateButton(cell, row){
        return(
            <div>
                <button className="btn btn-outline-success btn-sm" onClick={() => {this.VerFactura(row.Id)}}   style={{float:'left', marginRight:'0.3em'}}>Ver</button>
                <button className="btn btn-outline-info btn-sm"  onClick={() => {this.MostrarFormularioEdicion(row.Id)}} style={{float:'left', marginRight:'0.3em'}}>Editar</button>
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
            Moment(cell).format('DD-MM-YYYY')
        )
    }
    
    handleSubmit(event){

    
            axios.post("/Proyecto/DetalleOrdenServicio/Create",{
                Id: 0,
                OrdenServicioId: document.getElementById('OrdenId').className,
                GrupoItemId: this.state.itemid,

                vigente: true,
                valor_os: 0,
            })
            .then((response) => {
   
             
         
            })
            .catch((error) => {
                console.log(error);
           
            });
                
    }

    render(){
        return(
         <div>
             <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
             <BlockUi tag="div" blocking={this.state.blocking}>
             <BootstrapTable data={this.state.lista_detalles} hover={true} pagination={ true } >
                    <TableHeaderColumn dataField="GrupoItemId" 
                                       filter={ { type: 'TextFilter', delay: 500 } } 
                                       isKey={true} 
                                       width={"10%"}
                                       dataAlign="center" 
                                       dataSort={true}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        > Grupo Item</TableHeaderColumn>

                    
                    <TableHeaderColumn dataField="valor_os"  
                                       dataFormat={this.MontoFormato} 
                                       filter={ { type: 'TextFilter', delay: 500 } } 
                                       dataSort={true}
                                       width={"10%"}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px',textAlign: 'right'  }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px',textAlign: 'center'  }}
                                       >Valor OS</TableHeaderColumn>
                  
                    <TableHeaderColumn dataField='Operaciones'
                                       width={"15%"} 
                                       dataFormat={this.generateButton.bind(this)}
                                       tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                       >Operaciones</TableHeaderColumn>
                </BootstrapTable>
                <Dialog header="Detalles Orden" visible={this.state.visible} style={{width: '50vw'}} onHide={this.onHide} maximizable>
                      
                 
                            
                            <form onSubmit={this.handleSubmit}>
                            
                        <div className="row">
                                    <div className="form-group">
                                    <label htmlFor="label" style={{fontSize:'12px'}}>Item:</label>
                                    <div>
                                        <Dropdown 
                                        value={this.state.ItemId} 
                                        options={this.state.lista_items} 
                                        onChange={(e) => {this.setState({ItemId: e.value})}} 
                                        filter={true} filterPlaceholder="Selecciona un Item" 
                                        filterBy="label,value" placeholder="Selecciona unItem"
                                        style={{width: '100%'}}
                                        disabled
                                         />
                                    </div>
                                </div>
                                <div className="form-group">
                                            <label htmlFor="label">Valor Os</label>
                                            <input
                                            type="number"
                                            name="valor_os"
                                            className="form-control"
                                            onChange={this.handleChange}
                                            min="0" value="0" step="any"
                                            value={this.state.valor_os}
                                    />
                     </div>

                    <Button type="submit"  label="Guardar" icon="fa fa-fw fa-folder-open"/>
                    <Button type="button"  label="Cancelar" icon="fa fa-fw fa-ban" onClick={this.onHide}/>
                    </div>
                </form>
          
               </Dialog>
            </BlockUi>  
         </div>   
        )
    }
  

    OnEdit(id){
  
        axios.post("/proyecto/DetalleOrdenServicio/Edit/"+id,{})
        .then((response) => {
            if(response.data == "Ok"){            
                this.props.showSuccess("Se eliminó el registro");
                this.props.updateData();            
                this.props.reset();
            } else {  
                this.props.showWarn("La actividad tiene computos registrados");
            }
        })
        .catch((error) => {
            this.props.showWarn("No se puedo eliminar el registro");
            
        });
    }
    

    
}

ReactDOM.render(
    <DetalleOrdenServicio />,
    document.getElementById('content-viewdetalle')
  );
