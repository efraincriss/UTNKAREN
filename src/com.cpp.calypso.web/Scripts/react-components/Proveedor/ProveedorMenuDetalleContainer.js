import React from 'react';
import ReactDOM from 'react-dom';
import BlockUi from 'react-block-ui';
import axios from 'axios';
import { TabView, TabPanel } from 'primereact/components/tabview/TabView';

import config from '../Base/Config';
import ProveedorInfo from './ProveedorInfo';
import MenuContainer from './MenuContainer';
import ProveedorNovedadContainer from './ProveedorNovedadContainer';
 

class ProveedorMenuDetalleContainer extends React.Component {

    constructor() {
        super();
        this.state = {
            data: {
                proveedor: '',
                menus: [],
                novedades: []
            },
            proveedorId:0,
            blocking: true
        };
      
        this.GetData = this.GetData.bind(this);
        this.onRefreshData = this.onRefreshData.bind(this);
        this.onReturn = this.onReturn.bind(this);
    }


    componentWillMount() {
        //TODO: Recuparar el ID, desde el path
        //console.log(this.props.match.params);
        console.log(this.props.match);
        let url = window.location.href;
        let proveedorId = url.substr(url.lastIndexOf('/') + 1);

        this.setState({ proveedorId: proveedorId });
    }

    componentDidMount() {
        console.log('componentDidMount');
        this.GetData(); 
    }


    GetData() {

      

        let url = '';
        url = `/proveedor/menu/GetProveedorMenuApi/${this.state.proveedorId}`;
         

        this.setState({ blocking: true });

        axios.get(url, {})
            .then((response) => {

                let data = response.data;

                if (data.success === true) {

                    this.setState({ data: data.result });

                } else {
                    //TODO: 
                    //Presentar errores... 
                    var message = $.fn.responseAjaxErrorToString(data);
                    abp.notify.error(message, 'Error');
                }

               
                this.setState({  blocking: false });
            })
            .catch((error) => {
                console.log(error);
            });
    }

    onRefreshData() {
        this.GetData();
    }

    onReturn() {
        return (
            window.location.href = `${config.appUrl}/proveedor/menu`
        );
    }

 

    render() {
        return (
            <BlockUi tag="div" blocking={this.state.blocking}>

                <div className="row">
                    <div className="col-md-12">
                        <div className="card">
                            <div className="card-header">
                                Gestión de Menús
                                <div className="float-right">
                                    <button className="btn btn-outline-primary" onClick={this.onReturn} > Regresar </button>

                                </div>
                            </div>

                            <div className="card-body">
                                <div className="card">
                                    <div className="card-body">
                                        <ProveedorInfo data={this.state.data.proveedor} />
                                    </div>
                                </div>

                                <div className="row">
                                   
                                        <TabView>

                                            <TabPanel header="Menús">
                                                <MenuContainer
                                                    data={this.state.data.menus}
                                                        blocking={this.state.blocking}
                                                    padreId={this.state.proveedorId}
                                                    onRefreshData={this.onRefreshData} 
                                                    />
                                            </TabPanel>
                                         
                                            <TabPanel header="Novedades">
                                                <ProveedorNovedadContainer
                                                data={this.state.data.novedades}
                                                    blocking={this.state.blocking}
                                                padreId={this.state.proveedorId}
                                                onRefreshData={this.onRefreshData} 
                                                />
                                            </TabPanel>
                                        
                                        </TabView>
                                    
                                </div>
                                
                               
                                  
                                
                            </div>
                        </div>
                    </div>
                </div>
 
            </BlockUi>
        );
    }

}

ReactDOM.render(
    <ProveedorMenuDetalleContainer />,
    document.getElementById('nuc_detail_body_proveedores_menus')
);

 