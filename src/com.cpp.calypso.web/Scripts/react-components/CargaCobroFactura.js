import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import {Growl} from 'primereact-v2/components/growl/Growl';
import BlockUi from 'react-block-ui';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import {FileUpload} from 'primereact-v2/components/fileupload/FileUpload';;


const selectRowProp = {
    mode: 'checkbox'
  };
export class CargaCobroFactura extends React.Component{
    constructor(props){
        super(props);
                this.state = {
                visible: false,
                data:[],
                model:null,
                products:[],
                detallesseleccionados: [],
                AvanceProcuraId: 0,
                message: '',
                blocking: true,
                file:null
                    }
                    this.successMessage = this.successMessage.bind(this);
                    this.warnMessage = this.warnMessage.bind(this);
                    this.Submit = this.Submit.bind(this)
                    this.onHide = this.onHide.bind(this);
                    this.handleSubmit = this.handleSubmit.bind(this);
                    this.showSuccess = this.showSuccess.bind(this);
                    this.showWarn = this.showWarn.bind(this);
                    this.GetData=this.GetData.bind(this);
                    this.onUpload = this.onUpload.bind(this);
                    this.onSelect = this.onSelect.bind(this);
                    this.onBasicUpload = this.onBasicUpload.bind(this);
                    this.onBasicUploadAuto = this.onBasicUploadAuto.bind(this);
                    this.renderShowsTotal = this.renderShowsTotal.bind(this);
                    this.onSelectAll=this.onSelectAll.bind(this);
    }    
   
    componentDidMount(){
        this.setState({blocking: false})
    } 
        
    
    onUpload(event) {
        this.growl.show({severity: 'info', summary: 'Success', detail: 'File Uploaded'});
    
    }
    
    onBasicUpload(event) {
        this.growl.show({severity: 'info', summary: 'Correcto', detail: 'Archivo Subido Satisfactoriamente'});

     this.setState({file:event.files[0]});
     this.GetData(event);

    }
    
    onBasicUploadAuto(event) {   
        this.growl.show({severity: 'info', summary: 'Success', detail: 'File Uploaded with Auto Mode'});
    }
    onSelect(event) {
       this.GetData();
       console.log("termino");
       
    
    }
    onSelectAll(isSelected){
        if (isSelected) {
           return this.state.data.map(row => row.id);
         } else {
           return [];
         }
      }
    renderShowsTotal(start, to, total) {
        return (
          <p style={ { color: 'blue' } }>
            De { start } A { to }, Total  { total }&nbsp;&nbsp;
          </p>
        );
      }
    
    render() {


        const selectRowProp = {
            mode: 'checkbox'
          };
          
              
        return (
            <div>
              
                <div className="content-section implementation">
                   
                    <h3>Seleccione Archivo Excel de Cobros</h3>
                    <FileUpload name="UploadedFile" 
                                chooseLabel="Seleccionar" 
                                cancelLabel="Cancelar" 
                                uploadLabel="Cargar" 
                                onUpload={this.onBasicUpload}
                                multiple={true} 
                                accept="file_extension|media_type"
                                maxFileSize={1000000} 
                    />
                    <hr/>

                    <ul className="nav nav-tabs" id="gestion_tabs" role="tablist">

                    <li className="nav-item">
                    <a className="nav-link active" id="jerarquia-tab" data-toggle="tab" href="#jerarquia" role="tab" aria-controls="profile">Cobros Válidas</a>
                    </li>
                    <li className="nav-item">
                    <a className="nav-link" id="gestion-tab" data-toggle="tab" href="#gestion" role="tab" aria-controls="home" aria-expanded="true">Cobros No Válidas</a>
                    </li>
                    </ul>
            <div className="tab-content" id="myTabContent">
             <div className="tab-pane fade show active" id="jerarquia" role="tabpanel" aria-labelledby="jerarquia-tab">

             <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                <BlockUi tag="div" blocking={this.state.blocking}>
                <button onClick={this.Submit} className="btn btn-outline-indigo" style={{marginBottom: '1em'}}>Guardar</button>
                        <div id="productList1">
                                        <BootstrapTable data={this.state.data } selectRow={ selectRowProp } pagination={ true }>
                                        <TableHeaderColumn 
                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='id' isKey={true} filter={ { type: 'TextFilter', delay: 1000 } }>Fila</TableHeaderColumn>
                                        <TableHeaderColumn 
                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='sociedad'  filter={ { type: 'TextFilter', delay: 1000 } }>Sociedad</TableHeaderColumn>
                                        <TableHeaderColumn 
                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='fecha_documento' filter={ { type: 'TextFilter', delay: 1000 } }>Fecha Documento</TableHeaderColumn>
                                        <TableHeaderColumn
                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='referencia' filter={ { type: 'TextFilter', delay: 1000 } }>Factura</TableHeaderColumn>
                                        <TableHeaderColumn 

                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='detalle'  filter={ { type: 'TextFilter', delay: 1000 } }>Detalle</TableHeaderColumn>
                                        <TableHeaderColumn 
                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='clase_documento' filter={ { type: 'TextFilter', delay: 1000 } }>Clase Documento</TableHeaderColumn>
                                        <TableHeaderColumn 
                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='documento_compensacion' filter={ { type: 'TextFilter', delay: 1000 } }>D. Compensación</TableHeaderColumn>
                                        <TableHeaderColumn 
                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='importe_moneda'  filter={ { type: 'TextFilter', delay: 1000 } }>Importe Moneda</TableHeaderColumn>
                                        <TableHeaderColumn 
                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='fecha_compensacion' filter={ { type: 'TextFilter', delay: 1000 } }>Fecha Compensacion</TableHeaderColumn>
                                        <TableHeaderColumn 
                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='fecha_pago' filter={ { type: 'TextFilter', delay: 1000 } }>Fecha Pago</TableHeaderColumn>
                                        <TableHeaderColumn 
                                        tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                        dataField='cliente' filter={ { type: 'TextFilter', delay: 1000 } }>Cliente</TableHeaderColumn>
                                        </BootstrapTable>
                </div>
                </BlockUi>
            </div>
                 
            <div className="tab-pane fade" id="gestion" role="tabpanel" aria-labelledby="gestion-tab">
                <div id="productList">
                    <BootstrapTable data={ this.state.products } selectRow={ selectRowProp } pagination={ true } >
                    <TableHeaderColumn dataField='id' isKey={true} filter={ { type: 'TextFilter', delay: 1000 } }>Fila</TableHeaderColumn>
                    <TableHeaderColumn dataField='sociedad'  filter={ { type: 'TextFilter', delay: 1000 } }>Sociedad</TableHeaderColumn>
                    <TableHeaderColumn dataField='fecha_documento' filter={ { type: 'TextFilter', delay: 1000 } }>Fecha Documento</TableHeaderColumn>
                    <TableHeaderColumn dataField='factura' filter={ { type: 'TextFilter', delay: 1000 } }>Factura</TableHeaderColumn>
                    <TableHeaderColumn dataField='detalle'  filter={ { type: 'TextFilter', delay: 1000 } }>Detalle</TableHeaderColumn>
                    <TableHeaderColumn dataField='clase_documento' filter={ { type: 'TextFilter', delay: 1000 } }>Clase Documento</TableHeaderColumn>
                    <TableHeaderColumn dataField='documento_compensacion' filter={ { type: 'TextFilter', delay: 1000 } }>D. Compensación</TableHeaderColumn>
                    <TableHeaderColumn dataField='importe_moneda'  filter={ { type: 'TextFilter', delay: 1000 } }>Importe Moneda</TableHeaderColumn>
                    <TableHeaderColumn dataField='fecha_compensacion' filter={ { type: 'TextFilter', delay: 1000 } }>Fecha Compensacion</TableHeaderColumn>
                    <TableHeaderColumn dataField='fecha_pago' filter={ { type: 'TextFilter', delay: 1000 } }>Fecha Pago</TableHeaderColumn>
                    <TableHeaderColumn dataField='cliente' filter={ { type: 'TextFilter', delay: 1000 } }>Cliente</TableHeaderColumn>
                    </BootstrapTable>
                </div>


            </div>
</div>
                   
       
                  
                </div>
            </div>
        )
    }


    
    Submit(){
        this.setState({blocking: true})
        console.log(this.state.model);
        axios.post("/Proyecto/Factura/CreateFacturasC",{
            a:"s",
        
            data:this.state.model
            })
        .then((response) => {

            if(response.data=="Ok"){
            this.setState({blocking: false,data:[]})
            this.successMessage("Se Insertaron los registros.")
                }else{
                    this.warnMessage("Ocurrio un Error");

                }
        })
        .catch((error) => {
            console.log(error);
            this.setState({blocking: false})
            this.warnMessage("Vuelve a Intentar más tarde");
        });
    }

    handleSubmit(event){
        event.preventDefault();
       
    }
    GetData(event){

    if(event.files[0]!=null){

    const formData = new FormData();
    formData.append('UploadedFile',event.files[0])
    this.setState({blocking: true})
    const config = {
    headers: {
    'content-type': 'multipart/form-data'
    }
    }
    axios.post("/proyecto/Factura/CargaFacturaC/",formData,config)
    .then((response) => {
        console.log(response);
        this.setState({data: response.data.Validas,products:response.data.NoValidas, model:response.data, blocking: false });  

    })
    .catch((error) => {
    console.log(error);    
    });
    }else{

    console.log("error llamada");   
    }

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
    <CargaCobroFactura />,
    document.getElementById('content_cobro')
  );
