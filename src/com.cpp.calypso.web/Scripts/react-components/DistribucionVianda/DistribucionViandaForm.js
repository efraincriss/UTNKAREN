import React, { Component } from 'react';
import BlockUi from 'react-block-ui';
import moment from 'moment';
import { Calendar } from 'primereact/components/calendar/Calendar';

import http from '../Base/HttpService';
import { Dropdown } from 'primereact/components/dropdown/Dropdown';
 

import config from '../Base/Config';

class DistribucionViandaForm extends Component {

    constructor(props) {
        super(props);

        this.state = {
            data: {
                fecha: moment().format(config.formatDate),
                tipoComidaId: 0 
            },
            dataExtra: {
                tipo_comida: []
            },
            blocking: true,
            loadDataExtra: false,
            loading:false,
            errors: {} 
        };

        this.handleChange = this.handleChange.bind(this);
        this.setData = this.setData.bind(this);
         
        this.handleSubmit = this.handleSubmit.bind(this);
    }


    componentDidMount() {
        console.log('SolicitudViandaForm.componentDidMount');
    }


    componentDidUpdate(prevProps) {
        console.log('SolicitudViandaForm.componentDidUpdate');

        if (!this.state.loadDataExtra && this.props.show && !this.state.loading) {
            //Init
            this.loadDataExtra();
        }

        // Typical usage (don't forget to compare props):
        if (this.props.show  && (this.props.show !== prevProps.show)) {
            this.loadData();
        }
    }

    initData() {
        let dataInit = {
            fecha: moment().format(config.formatDate),
            tipoComidaId: 0 
        };
        return dataInit; 
    }

    loadData() {
     
        this.setState({
            data: this.initData(),
            blocking: false
        });
       
    }

     

    getTipoComida() {
       
    }
     



    loadDataExtra() {

        var self = this;
        self.setState({ loading:   true });

        let url = '';
        url = `/Proveedor/DistribucionVianda/SearchByCodeApiComida/?code=TIPOCOMIDA`;
         
        
        http.get(url, {})
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    self.setState({ blocking: false, loadDataExtra: true });

                    self.setState({
                        dataExtra: {
                            tiposComidas: self.mapDropdown(data, 'nombre', 'Id')
                        }
                    });

                    self.setState({ blocking: false, loading: false });

                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(data);
                    abp.notify.error(message, 'Error');
                }


                self.setState({ blocking: false });

            })
            .catch((error) => {
                //TODO: Gestion de errores
                self.setState({ blocking: false, loading: false });
                console.log(error);
            });
    }

    mapDropdown(data,nameField='name',valueField='Id') {
        if (data.success === true) {

            return data.result.map(i => {
                return { label: i[nameField], value: i[valueField] }
            });
        } else if (data !== undefined) {

            return data.map(i => {
                return { label: i[nameField], value: i[valueField] }
            });
        }

        return {};
    }

    getExtraSelect(lista) {
        return (
            lista.map((item) => {
                return (
                    <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                );
            })
        );
    }


    isValid() {
        const errors = {};

        if (this.state.data.fecha === undefined || this.state.data.fecha.length <= 0) {
            errors.fecha = 'Debe ingresar una fecha';
        }

        if (this.state.data.tipoComidaId === undefined || this.state.data.tipoComidaId <= 0) {
            errors.tipoComidaId = 'Debe seleccionar un tipo de Comida';
        }
  
        this.setState({ errors });
        return Object.keys(errors).length === 0;
    }

 

    handleSubmit(event) {
        event.preventDefault();

        if (!this.isValid()) {
            return;
        }
 
        this.setState({ blocking: true });

        let url = '';
        url = `/proveedor/DistribucionVianda/CreateDistribucionApi`;
         

         //creating copy of object
        let dataRequest = Object.assign({}, this.state.data);   
     
        http.post(url, dataRequest)
            .then((response) => {

                let dataResponse = response.data;

                if (dataResponse.success === true) {
 
                    this.setState({
                        data: this.initData()
                    });
               
                    this.props.onAdded(dataRequest);
     
                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(dataResponse);
                    abp.notify.error(message, 'Error');
                }


                this.setState({ blocking: false });

            })
            .catch((error) => {
                console.log(error);

                this.setState({ blocking: false });
            });

    }


    handleChange(event) {
        const target = event.target;
        const value = target.type === "checkbox" ? target.checked : target.value;
        const name = target.name;

        const updatedData = {
            ...this.state.data
        };

        updatedData[name] = value;

        this.setState({
            data: updatedData
        });
    }

    setData(name, value) {

        const updatedData = {
            ...this.state.data
        };

        updatedData[name] = value;

        this.setState({
            data: updatedData
        });
    }
 

    render() {
        /*
        const es = {
            firstDayOfWeek: 1,
            dayNames: ["domingo", "lunes", "martes", "miércoles", "jueves", "viernes", "sábado"],
            dayNamesShort: ["dom", "lun", "mar", "mié", "jue", "vie", "sáb"],
            dayNamesMin: ["D", "L", "M", "X", "J", "V", "S"],
            monthNames: ["enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"],
            monthNamesShort: ["ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic"]
        };*/

        return (

            <BlockUi tag="div" blocking={this.state.blocking}>

                <form onSubmit={this.handleSubmit}>

                    <div className="row">
                        <div className="col">

                           

                            <div className="form-group row">
                                <label htmlFor="fecha" className="col-sm-12 col-form-label">Fecha</label>

                                <div className="col-sm-12">
                                    <input type="date" id="fecha" className="form-control" value={this.state.data.fecha} onChange={this.handleChange} name="fecha" />

                                    {this.state.errors.fecha && <div className="alert alert-danger">{this.state.errors.fecha}</div>}
                                </div>

                                
                            </div>
 
 
                            <div className="form-group row">
                                <label htmlFor="tipoComidaId" className="col-sm-12 col-form-label" >Tipo Comida</label>
                                <div className="col-sm-12">

                                    <Dropdown
                                        id="tipoComidaId"
                                        value={this.state.data.tipoComidaId}
                                        options={this.state.dataExtra.tiposComidas}
                                        onChange={(e) => {
                                            this.setData("tipoComidaId", e.value);
                                        }} 
                                        style={{ width: '100%'}}
                                        required
                                        filter={true}
                                        placeholder="Seleccione.."
                                        filterPlaceholder="Seleccione.."
                                    />

                                    {this.state.errors.tipoComidaId && <div className="alert alert-danger">{this.state.errors.tipoComidaId}</div>}
                                </div>
                            </div>
                          </div>
                     </div>
                    <button type="submit" className="btn btn-outline-primary">Crear</button>
                </form>
            </BlockUi>
        );
    }
}

export default DistribucionViandaForm;