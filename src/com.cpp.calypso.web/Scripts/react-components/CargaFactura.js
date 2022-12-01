import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import {Growl} from 'primereact/components/growl/Growl';
import BlockUi from 'react-block-ui';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import {FileUpload} from 'primereact/components/fileupload/FileUpload';
import moment from 'moment';


export class CargaFactura extends React.Component{
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
                file:null,
                minimodatos:0,
                blockminimo:true,
                 
                //TIPOS DE ARCHIVOS

                DB:[],
                DR:[],
                DE:[],
                DI:[],
                AB:[],
                DZ:[],
                DF:[],
                DG:[],

                

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
                    this.dateFormat=this.dateFormat.bind(this);
    }    
   
    componentDidMount(){
        this.setState({blocking: false})
    } 
        
    
    onUpload(event) {
        this.growl.show({severity: 'info', summary: 'Success', detail: 'File Uploaded'});
    
    }
    
    onBasicUpload(event) {
        this.growl.show({severity: 'info', summary: 'Correcto', detail: 'Cargado Correctamente'});

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

    render() {

    
          
        const selectRowProp = {
            mode: 'checkbox'
          };
          
              
        return (
            <div>
              
                <div className="content-section implementation">
                   
                    <h3>Seleccione Archivo Excel de Facturación</h3>

                    <FileUpload name="UploadedFile" 
                                chooseLabel="Seleccionar" 
                                cancelLabel="Cancelar" 
                                uploadLabel="Cargar" 
                                onUpload={this.onBasicUpload}
                                multiple={true} 
                                accept="file_extension|media_type"
                                maxFileSize={991000000} 
                    />
                    <hr/>
      
                   <ul className="nav nav-tabs" id="tipo_tabs" role="tablist">
 
                        <li className="nav-item">
                            <a className="nav-link active" id="DB-tab" data-toggle="tab" href="#DB" role="tab" aria-controls="profile">Tipo DB</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" id="DR-tab" data-toggle="tab" href="#DR" role="tab" aria-controls="home" aria-expanded="true">Tipo DR</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" id="DE-tab" data-toggle="tab" href="#DE" role="tab" aria-controls="profile">Tipo DE</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" id="DI-tab" data-toggle="tab" href="#DI" role="tab" aria-controls="home" aria-expanded="true">Tipo DI</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" id="DZ-tab" data-toggle="tab" href="#DZ" role="tab" aria-controls="profile">Tipo DZ</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" id="DF-tab" data-toggle="tab" href="#DF" role="tab" aria-controls="profile">Tipo DF</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" id="DG-tab" data-toggle="tab" href="#DG" role="tab" aria-controls="profile">Tipo DG</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" id="AB-tab" data-toggle="tab" href="#AB" role="tab" aria-controls="profile">Tipo AB</a>
                        </li>
                    </ul>
                    <div className="tab-content" id="myTabContent">
                        <div className="tab-pane fade show active" id="DB" role="tabpanel" aria-labelledby="DB-tab">

                    <BlockUi tag="div" blocking={this.state.blocking}>  
                  
                    <BlockUi tag="div" blocking={this.state.blockminimo}>
                    <button onClick={this.Submit} 
                                className="btn btn-outline-indigo" style={{marginBottom: '1em'}}>Guardar</button>
    
                     </BlockUi>  
                        <BootstrapTable data={this.state.DB } selectRow={ selectRowProp } 
                        pagination={ true }
                        hover={true}>
                        <TableHeaderColumn 
                                     tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     dataField='id' 
                                     isKey={true} 
                                     filter={ { type: 'TextFilter', delay: 1000 } }>Fila</TableHeaderColumn>
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
                    <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                    </BlockUi>

                     </div>
                  
                    <div className="tab-pane fade" id="DR" role="tabpanel" aria-labelledby="DR-tab">
                  
                    <BootstrapTable data={this.state.DR } selectRow={ selectRowProp } pagination={ true }>
                        <TableHeaderColumn 
                                     tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     dataField='id' 
                                     isKey={true} 
                                     filter={ { type: 'TextFilter', delay: 1000 } }>Fila</TableHeaderColumn>
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
                    <div className="tab-pane fade" id="DE" role="tabpanel" aria-labelledby="DE-tab">
                  
                    <BootstrapTable data={this.state.DE } selectRow={ selectRowProp } pagination={ true }>
                        <TableHeaderColumn 
                                     tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     dataField='id' 
                                     isKey={true} 
                                     filter={ { type: 'TextFilter', delay: 1000 } }>Fila</TableHeaderColumn>
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
                                    thStyle={{ whiteSpace           : 'normal', fontSize: '10px' }}
                        dataField='cliente' filter={ { type: 'TextFilter', delay: 1000 } }>Cliente</TableHeaderColumn>
                    </BootstrapTable>                      
                    </div>
                    <div className="tab-pane fade" id="DI" role="tabpanel" aria-labelledby="DI-tab">
                   
                    <BootstrapTable data={this.state.DI } selectRow={ selectRowProp } pagination={ true }>
                        <TableHeaderColumn 
                                     tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     dataField='id' 
                                     isKey={true} 
                                     filter={ { type: 'TextFilter', delay: 1000 } }>Fila</TableHeaderColumn>
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
                    <div className="tab-pane fade" id="AB" role="tabpanel" aria-labelledby="AB-tab">
                  
                    <BootstrapTable data={this.state.AB } selectRow={ selectRowProp } pagination={ true }>
                        <TableHeaderColumn 
                                     tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     dataField='id' 
                                     isKey={true} 
                                     filter={ { type: 'TextFilter', delay: 1000 } }>Fila</TableHeaderColumn>
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
                    
                            <div className="tab-pane fade" id="DZ" role="tabpanel" aria-labelledby="DZ-tab">
                    
                    <BootstrapTable data={this.state.DZ } selectRow={ selectRowProp } pagination={ true }>
                        <TableHeaderColumn 
                                     tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     dataField='id' 
                                     isKey={true} 
                                     filter={ { type: 'TextFilter', delay: 1000 } }>Fila</TableHeaderColumn>
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
                   
                    <div className="tab-pane fade" id="DF" role="tabpanel" aria-labelledby="DF-tab">
                 
                    <BootstrapTable data={this.state.DF } selectRow={ selectRowProp } pagination={ true }>
                        <TableHeaderColumn 
                                     tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     dataField='id' 
                                     isKey={true} 
                                     filter={ { type: 'TextFilter', delay: 1000 } }>Fila</TableHeaderColumn>
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

                    <div className="tab-pane fade" id="DG" role="tabpanel" aria-labelledby="DG-tab">
                   
                    <BootstrapTable data={this.state.DG } selectRow={ selectRowProp } pagination={ true }>
                        <TableHeaderColumn 
                                     tdStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     thStyle={{ whiteSpace: 'normal', fontSize: '10px' }}
                                     dataField='id' 
                                     isKey={true} 
                                     filter={ { type: 'TextFilter', delay: 1000 } }>Fila</TableHeaderColumn>
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

                    </div>
                                    
       
                </div>
            </div>
        )
    }


    
    Submit(){
        this.setState({blocking: true})
        console.log(this.state.model);
        axios.post("/Proyecto/Factura/CreateFacturas",{
            a:"s",
     
            Validas:this.state.model.DB,
            NoValidas:this.state.model.AB

            })
        .then((response) => {
            if(response.data=="Ok"){
            this.setState({blocking: false,data:[]})
            this.successMessage("Se insertaron los registros.")
            }else{
                this.warnMessage("Ocurrio un Error");
                
            }
        })
        .catch((error) => {
            console.log(error);
            this.setState({blocking: false})
            this.warnMessage("Vuelve a intentar más tarde");
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
    axios.post("/proyecto/Factura/CargaFactura/",formData,config)
    .then((response) => {
        console.log(response);
        this.setState({
            model:response.data,
            DB: response.data.DB,
            DR: response.data.DR,
            DE: response.data.DE,
            DI: response.data.DI, 
            AB: response.data.AB,
            DZ: response.data.DZ,
            DF: response.data.DF,
            DG: response.data.DG,
            
            blocking: false });  

            if(response.data.DB.lenght>15){
            
                Console.log("minimo"+response.data.DB.lenght)
            }else{

            this.setState({blockminimo: false });  
        
            }

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
    <CargaFactura />,
    document.getElementById('content_factura')
  );
