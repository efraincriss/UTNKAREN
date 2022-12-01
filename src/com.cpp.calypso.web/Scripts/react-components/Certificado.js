import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';
import BlockUi from 'react-block-ui';
import {Button} from 'primereact/components/button/Button';
import {Dialog} from 'primereact/components/dialog/Dialog';
import {Growl} from 'primereact/components/growl/Growl';
import TreeWbs from './wbs_components/TreeWbs';
import ComputoForm from './forms/ComputoForm';
import {Sidebar} from 'primereact/components/sidebar/Sidebar';
import {DataTable} from 'primereact/components/datatable/DataTable';
import {Column} from 'primereact/components/column/Column';

export default class Certificados  extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            key: 7954,
            key2:200,
            key3:400,
            key4:309,
          blocking: true,
        //Construcción

         dataconstruccion:[],
          dataconstruccion2:[],
          construccionaprobados:[],
          seleccionadoscontruccion: [],

          //Ingenieria
          dataavanceingenieria:[],
          dataavanceingenieria2:[],
          seleccionadosingenieria: [],
          ingenieriaaprobados:[],

          // Procura
          dataprocura:[],
          dataprocura2:[],
          procuraprobados:[],
          seleccionadosprocura: [],

          //Dialog
          visibleconstruccion:false,
          visibleingenieria:false,
          visibleprocura:false,

        //Proyecto
        proyecto:[],
        //certificados
          datacertificados:[],
          datacertificadosi:[],
          datacertificadosp:[],

          message: '',

          //montos

          montopresupuestoconstruccion:0,
          montopresupuestoingenieria:0,
          montopresupuestoprocura:0,
          montototal:0,

          //montos certificados
          montocconstrucion:0,
          montocingenieria:0,
          montocprocura:0,
          montoctotal:0,

          //saldo

           
           montosc:0,
           montosi:0,
           montosp:0,
           montost:0,

           //porcentaje

           porcentajec:0,
           porcentajei:0,
           porcentajep:0,
           porcentajet:0,

           //aprobado
           aprobado:'No',
           numero:'',
           periodo:''

        }
        this.GetDataAvanceCertificados=this.GetDataAvanceCertificados.bind(this);
        this.GetDataAvanceCertificadosI=this.GetDataAvanceCertificadosI.bind(this);
        this.GetDataAvanceCertificadosP=this.GetDataAvanceCertificadosP.bind(this);
        this.GetDataAvance = this.GetDataAvance.bind(this);
        this.cargarconstruccion = this.cargarconstruccion.bind(this);
        this.onHideConstruccion = this.onHideConstruccion.bind(this);
        this.onHideIngenieria=this.onHideIngenieria.bind(this);
        this.GetDataIngenieria=this.GetDataIngenieria.bind(this);
        this.cargaringenieria=this.cargaringenieria.bind(this);
        this.GetDataProcura=this.GetDataProcura.bind(this);
        this.cargarprocura=this.cargarprocura.bind(this);
        this.onHideProcura=this.onHideProcura.bind(this);
        this.IngresarCertificadosObra=this.IngresarCertificadosObra.bind(this);
        this.IngresarCertificadosIngenieria=this.IngresarCertificadosIngenieria.bind(this);
        this.IngresarCertificadosProcura=this.IngresarCertificadosProcura.bind(this);
        this.showSuccess = this.showSuccess.bind(this);
        this.showWarn = this.showWarn.bind(this);
        this.GetMontoCertificados=this.GetMontoCertificados.bind(this);
        this.AprobarCertificado=this.AprobarCertificado.bind(this);
        this.CancelarCertificado=this.CancelarCertificado.bind(this);
        this.IngresarAllCertificados=this.IngresarAllCertificados.bind(this);
    }

    componentWillMount(){
        this.GetDataAvance(); 
        this.GetDataAvanceCertificados();
       // this.GetDataAvanceCertificadosP();
       // this.GetDataAvanceCertificadosI();
       // this.GetDataIngenieria();
        //this.GetDataProcura();
       this.GetMontoCertificados();
       console.log('fecha',document.getElementById('FechaCorteId').className);
    }


    render(){
        return(

            <BlockUi tag="div" blocking={this.state.blocking}>
                              <div className="row">
    <div style={{width:  '100%'}}>
        <div className="card">
            <div className="card-body">
                <div className="row">
                    <div  align="right">
                    <button onClick={this.IngresarAllCertificados} className="btn btn-outline-primary">Generar Certificados</button> 
                    
                    </div>


                 </div>
           <br/>
       
                <div className="row">
                    <div className="col">
                        <h6 style={{fontSize:'12px'}}><b>Presupuesto Construcción: </b>${this.state.montopresupuestoconstruccion}</h6>
                        <h6 style={{fontSize:'12px'}}><b>Monto Contrucción:</b>${this.state.montocconstrucion}</h6>
                        <h6 style={{fontSize:'12px'}}><b>Saldo Construcción: </b>${this.state.montosc}</h6>
                        <h6 style={{fontSize:'12px'}}><b>% Avance Construcción: </b>{(this.state.porcentajec)}%</h6>
                    </div>
                   
                </div>
        </div>
        </div>
        </div>
            <div style={{width:  '100%'}}  >
            <div>

                        <ul className="nav nav-tabs" id="empresa_tabs" role="tablist">
                            <li className="nav-item">
                                <a className="nav-link active" id="historicos-tab" data-toggle="tab" href="#historicos" role="tab" aria-controls="home" aria-expanded="true">Construcción</a>
                            </li>
                           
                          
                        </ul>

                        <div className="tab-content" id="myTabContent">

                            <div   className="tab-pane fade show active" id="historicos" role="tabpanel" aria-labelledby="historicos-tab">
                         
                           
                            <DataTable   value={this.state.dataconstruccion}  header="Avances de Obra sin Certificar" 
                                    selection={this.state.seleccionadoscontruccion}  paginator={true} rows={10} rowsPerPageOptions={[5,10,20]} onSelectionChange={(e) => this.setState({seleccionadoscontruccion: e.data})}>
                                    <Column selectionMode="multiple" style={{width:'2em'}}/>
                                    <Column field="AvanceObra.Oferta.codigo" header="Oferta" filter="Wbs.OfertaId" />
                                    <Column field="AvanceObraId" header="Avance Obra" filter="AvanceObraId" />
                                    <Column field="fechar" header="Fecha"   filter="fecha_registro"/>
                                    <Column field="item_codigo" header="Item"   filter="item_codigo"/>
                                    <Column field="nombre_item" header="Descripción" filter="nombre_item" />
                                    <Column field="cantidad_diaria" header="Cantidad Avance"  style={{textAlign: 'right'}} filter="cantidad_diaria" />
                                    <Column field="Computo.cantidad" header="Cant Presupuestada" style={{textAlign: 'right'}} filter="Computo.cantidad" />
                                    <Column field="total" header="Monto Total" style={{textAlign: 'right'}} filter="total" />
                                    
                                </DataTable>
                                <br/>
                               
                            </div>

                            
                             <Growl ref={(el) => { this.growl = el; }} position="bottomright" baseZIndex={1000}></Growl>
                        </div>
                    </div>
                </div>
           
                </div>
                <Dialog header="Aprobación de Certificados de Construcción" visible={this.state.visibleconstruccion} width="70%" modal={true} onHide={this.onHideConstruccion}>
                <button onClick={this.IngresarCertificadosObra} className="btn btn-outline-indigo" style={{marginBottom: '1em'}}>Registrar</button>
                 <DataTable   value={this.state.dataconstruccion2} header="Avances de Obra para Aprobación" 
                                    selection={this.state.construccionaprobados }  paginator={true} rows={10} rowsPerPageOptions={[5,10,20]} onSelectionChange={(e) => this.setState({construccionaprobados: e.data})}>
                                    <Column selectionMode="multiple" style={{width:'2em'}}/>
                                    <Column field="AvanceObra.Oferta.codigo" header="Oferta" filter="Wbs.OfertaId" />
                                    <Column field="AvanceObraId" header="Avance Obra" filter="AvanceObraId" />
                                    <Column field="fechar" header="Fecha"   filter="fecha_registro"/>
                                    <Column field="item_codigo" header="Item"   filter="item_codigo"/>
                                    <Column field="nombre_item" header="Descripción" filter="nombre_item" />
                                    <Column field="cantidad_diaria" header="Cantidad Avance"  style={{textAlign: 'right'}} filter="cantidad_diaria" />
                                    <Column field="Computo.cantidad" header="Cant Presupuestada" style={{textAlign: 'right'}} filter="Computo.cantidad" />
                                    
                                </DataTable>
                         
                        </Dialog>    
                        <Dialog header="Aprobación de Certificados de Ingeniería" visible={this.state.visibleingenieria} width="70%" modal={true} onHide={this.onHideIngenieria}>
                <button onClick={this.IngresarCertificadosIngenieria} className="btn btn-outline-indigo" style={{marginBottom: '1em'}}>Registrar</button>
                 <DataTable  value={this.state.dataavanceingenieria2} header="Avances de Ingenieria para Aprobación" 
                                    selection={this.state.ingenieriaaprobados }  paginator={true} rows={10} rowsPerPageOptions={[5,10,20]} onSelectionChange={(e) => this.setState({ingenieriaaprobados: e.data})}>
                                   <Column selectionMode="multiple" style={{width:'2em'}}/>
                                   <Column field="AvanceIngenieria.Oferta.codigo" header="Oferta" filter="AvanceIngenieria.Oferta.codigo" />
                                    <Column field="AvanceIngenieriaId" header="Avance de Ingenieria" filter="AvanceIngenieriaId" />
                                    <Column field="fechar" header="Fecha"   filter="fecha_registro"/>
                                    <Column field="codigo_item" header="Item"   filter="codigo_item"/>
                                    <Column field="descripcion_item" header="Descripción" filter="descripcion_item" />
                                    <Column field="cantidad_horas" header="Cant Horas"  style={{textAlign: 'right'}} filter="cantidad_horas" />
                                    <Column field="Computo.cantidad" header="Cant Presup" style={{textAlign: 'right'}} filter="Computo.cantidad" />
                                    
                                    
                                </DataTable>
                         
                        </Dialog>       
                        <Dialog header="Aprobación de Certificados de Procura" visible={this.state.visibleprocura} width="70%" modal={true} onHide={this.onHideIngenieria}>
                <button onClick={this.IngresarCertificadosProcura} className="btn btn-outline-indigo" style={{marginBottom: '1em'}}>Registrar</button>
                 <DataTable  value={this.state.dataprocura2} header="Avances de Procura para Aprobación" 
                                    selection={this.state.procuraprobados }  paginator={true} rows={10} rowsPerPageOptions={[5,10,20]} onSelectionChange={(e) => this.setState({procuraprobados: e.data})}>
                                   <Column selectionMode="multiple" style={{width:'2em'}}/>
                                   <Column field="AvanceProcura.Oferta.codigo" header="Oferta" filter="AvanceProcura.Oferta.codigo" />
                                    <Column field="Item.codigo" header="Código Item" filter="Item.codigo" />
                                    <Column field="Item.nombre" header="Item"   style={{width: '200px'}} filter="Item.nombre"/>
                                    <Column field="fechar" header="Fecha"   filter="fechar"/>
                                    <Column field="calculo_diario" header="Calculo Diario" filter="calculo_diario" />
                                    <Column field="cantidad" header="Cant Procura"  style={{textAlign: 'right'}} filter="cantidad" />
                                    <Column field="Computo.cantidad" header="Cant Presup" style={{textAlign: 'right'}} filter="Computo.cantidad" />
                                    
                                    
                                </DataTable>
                         
                        </Dialog>                   
            </BlockUi>
        );
    }
    showSuccess() {
        this.growl.show({ life: 5000, severity: 'success', summary: 'Proceso exitoso!', detail: this.state.message });
    }

    showWarn() {
        this.growl.show({ life: 5000, severity: 'error', summary: 'Error', detail: this.state.message });
    }

    onHideConstruccion(){
        this.setState({visibleconstruccion: false,visibleingenieria:false,visibleprocura:false})
    }
    onHideProcura(){
        this.setState({visibleprocura: false,visibleingenieria:false,visibleprocura:false})
    }
    onHideIngenieria(){
        this.setState({visibleingenieria: false,visibleingenieria:false,visibleprocura:false})
    }
    cargarconstruccion(){
        this.setState({dataconstruccion2:this.state.seleccionadoscontruccion, visibleconstruccion:false,visibleingenieria:false,visibleprocura:false,blocking:false        
        })
    } 
    cargaringenieria(){
        this.setState({dataavanceingenieria2:this.state.seleccionadosingenieria, visibleingenieria:false, visibleconstruccion:false,visibleprocura:false, blocking:false        
        })
    } 
    cargarprocura(){
        this.setState({dataprocura2:this.state.seleccionadosprocura, visibleprocura:false, visibleconstruccion:false, visibleingenieria:false, blocking:false        
        })
    } 
    GetDataAvance(){
        axios.post("/proyecto/Certificado/AvanceObraSinCertificar",{
            id: document.getElementById('ProyectoId').className,
            fechaCorte: new Date(document.getElementById('fechacorte').className)
            
        })
        .then((response) => {
            this.setState({dataconstruccion: response.data, blocking: false        
            })
        })
        .catch((error) => {
            console.log(error);    
        });
    }

    GetDataIngenieria(){
        axios.post("/proyecto/Certificado/AvanceIngenieriaSinCertificar",{
            id: document.getElementById('ProyectoId').className,
            
        })
        .then((response) => {
            this.setState({dataavanceingenieria: response.data, blocking: false        
            })
        })
        .catch((error) => {
            console.log(error);    
        });
    }
    GetDataProcura(){
        axios.post("/Proyecto/Certificado/AvanceProcuraSinCertificar",{
            id: document.getElementById('ProyectoId').className,
            
        })
        .then((response) => {
            this.setState({dataprocura: response.data, blocking: false  
            })
        })
        .catch((error) => {
            console.log(error);    
        });
    }
    GetDataAvanceCertificados(){
        axios.post("/proyecto/Certificado/AvancesCertificadosC",{
            id: document.getElementById('CertificadoId').className,
            
        })
        .then((response) => {
            this.setState({datacertificados: response.data, blocking: false          
            })
        })
        .catch((error) => {
            console.log(error);    
        });
    }
    GetDataAvanceCertificadosI(){
        axios.post("/proyecto/Certificado/AvancesCertificadosI",{
            id: document.getElementById('CertificadoId').className,
            
        })
        .then((response) => {
            this.setState({datacertificadosi: response.data, blocking: false          
            })
        })
        .catch((error) => {
            console.log(error);    
        });
    }  
     GetDataAvanceCertificadosP(){
        axios.post("/proyecto/Certificado/AvancesCertificadosP",{
            id: document.getElementById('CertificadoId').className,
            
        })
        .then((response) => {
            this.setState({datacertificadosp: response.data, blocking: false          
            })
        })
        .catch((error) => {
            console.log(error);    
        });
    }
   
    AprobarCertificado(){
        axios.post("/proyecto/Certificado/AprobarCertificado",{
            id: document.getElementById('CertificadoId').className,
            
        })
        .then((response) => {
            var r = response.data;
            console.log(r)
        if (r == "OK") {
            this.setState({
                message: 'Aprobado', aprobado:"Si"
              },
                this.showSuccess)
              
            } else if (r == "Error") {
                this.setState({
                    message: 'No Se Aprobo',
                              },
                    this.showWarn)
            }
            })
            .catch((error) => {
            console.log(error);    
        });
    }
    CancelarCertificado(){
        axios.post("/proyecto/Certificado/CarcelarCertificado",{
            id: document.getElementById('CertificadoId').className,
            
        })
        .then((response) => {
            var r = response.data;
            console.log(r)
        if (r == "OK") {
            this.setState({
                message: 'Cancelado', aprobado:"No"
              },
                this.showSuccess)
              
            } else if (r == "Error") {
                this.setState({
                    message: 'No Se Aprobo',
                              },
                    this.showWarn)
            }
            })
            .catch((error) => {
            console.log(error);    
        });
    }
    IngresarCertificadosObra(){
  
        if (this.state.construccionaprobados.length==0) {
     
                this.setState({
                    message: 'Debe seleccionar Al menos una Fila'},
                    this.showWarn)
            
            }else{
        axios.post("/proyecto/Certificado/IngresarDetallesCertificadosConstruccion",{
            
            data: this.state.construccionaprobados,
            CertificadoId:  document.getElementById('CertificadoId').className
            
        })
        .then((response) => {
            this.GetDataAvance();
            this.GetDataAvanceCertificados();
            this.GetMontoCertificados();
            var r = response.data;
            if (r == "OK") {
                    this.setState({
                    message: 'Guardado Correctamente', visibleconstruccion:false,seleccionadoscontruccion:[],construccionaprobados:[]
                  },this.showSuccess)
                   
                //this.props.updateData();

            } else if (r == "Error") {
                this.setState({
                    message: 'No Ingreso',
                              },
                    this.showWarn)
            }
            })
        .catch((error) => {
            console.log(error);    
        });
    }
    }

    IngresarCertificadosIngenieria(){

        
        if (this.state.ingenieriaaprobados.length==0) {
     
            this.setState({
                message: 'Debe seleccionar Al menos una Fila'},
                this.showWarn)
        
        }else{
        axios.post("/proyecto/Certificado/IngresarDetallesCertificadosIngenieria",{
            data: this.state.ingenieriaaprobados,
            CertificadoId:  document.getElementById('CertificadoId').className
        })
        .then((response) => {
            this.GetDataIngenieria();
            this.GetDataAvanceCertificadosI();
            this.GetMontoCertificados();
            var r = response.data;
            if (r == "OK") {
                    this.setState({
                    message: 'Guardado Correctamente', visibleingenieria:false,seleccionadosingenieria:[],ingenieriaaprobados:[]
                  },
                    this.showSuccess)
                  

            } else if (r == "Error") {
                this.setState({
                    message: 'No Ingreso',
                              },
                    this.showWarn)
            }
            })
        .catch((error) => {
            console.log(error);    
        });
    }
    }
    IngresarCertificadosProcura(){

            
        if (this.state.procuraprobados.length==0) {
     
            this.setState({
                message: 'Debe seleccionar Al menos una Fila'},
                this.showWarn)
        
        }else{
        axios.post("/proyecto/Certificado/IngresarDetallesCertificadosProcura",{
            data: this.state.procuraprobados,
            CertificadoId:  document.getElementById('CertificadoId').className
            
        })
        .then((response) => {
            this.GetDataProcura();
            this.GetDataAvanceCertificadosP();
            this.GetMontoCertificados();
            var r = response.data;
            if (r == "OK") {
                    this.setState({
                    message: 'Guardado Correctamente', visibleprocura:false,seleccionadosprocura:[],procuraprobados:[]
                  },
                    this.showSuccess)
                  


            } else if (r == "Error") {
                this.setState({
                    message: 'No Ingreso',
                              },
                    this.showWarn)
            }
            })
        .catch((error) => {
            console.log(error);    
        });
    }
        }


        
        GetMontoCertificados(){
            axios.post("/proyecto/Certificado/MontosCertificados",{
                id: document.getElementById('ProyectoId').className,
                
            })
            .then((response) => {
                
                this.setState({
                    montopresupuestoconstruccion: response.data[0],
                    montopresupuestoingenieria: response.data[1],
                    montopresupuestoprocura: response.data[2],
                    montototal:response.data[3],
                    montocconstrucion: response.data[4],
                    montocingenieria: response.data[5],
                    montocprocura: response.data[6],
                    montoctotal: response.data[7],
                    montosc: response.data[8],
                    montosi: response.data[9],
                    montosp: response.data[10],
                    montost: response.data[11],
                    porcentajec: response.data[12],
                    porcentajei: response.data[13],
                    porcentajep: response.data[14],
                    porcentajet: response.data[15],
                    blocking: false
                })
            })
            .catch((error) => {
                console.log(error);    
            });
        }

        IngresarAllCertificados(){
            this.setState({blocking:true});
            if(this.state.seleccionadoscontruccion.length>0){
            
            axios.post("/proyecto/Certificado/IngresarDetallesCertificados",{
                data: this.state.seleccionadoscontruccion.map((item) => {
                    return (item.Id)
                }),
                data1: this.state.seleccionadosingenieria.map((item) => {
                    return (item.Id)
                }),
                data2: this.state.seleccionadosprocura.map((item) => {
                    return (item.Id)
                }),
                proyectoId:  document.getElementById('ProyectoId').className,
                fechaCorte:  document.getElementById('FechaCorteId').className
                
            })
            .then((response) => {
                //this.GetDataProcura();
                this.GetDataAvance();
               // this.GetDataIngenieria();
                this.GetMontoCertificados();
                var r = response.data;
                if (r == "OK") {
                        this.setState({
                        message: 'Guardado Correctamente',seleccionadoscontruccion:[], seleccionadosingenieria:[], seleccionadosprocura:[] ,ingenieriaaprobados:[],procuraprobados:[],construccionaprobados:[]
                      },
                        this.showSuccess)
                      
    
    
                } else if (r == "Error") {
                    this.setState({
                        message: 'No Ingreso',
                                  },
                        this.showWarn)
                }
                })
            .catch((error) => {
                console.log(error);    
                this.setState({blocking:true});
            });
        
        }else{
            this.setState({
                message: 'Debe Seleccionar al menos un avance de obra',
                          },
                this.showWarn)
            
        }
            }

    
    
         

}
ReactDOM.render(
    <Certificados />,
    document.getElementById('content_certificado')
  );