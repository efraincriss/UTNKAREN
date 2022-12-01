import React, { Component } from 'react';
import BlockUi from 'react-block-ui';
import axios from 'axios';
import moment from 'moment';

import http from '../Base/HttpService';
import { Dropdown } from 'primereact/components/dropdown/Dropdown';

import config from '../Base/Config';
import ActionForm from '../Base/ActionForm';
import wrapForm from '../Base/WrapForm';

import ConciliacionSolicitudInfo from './ConciliacionSolicitudInfo'
import ConciliacionConsumoList from './ConciliacionConsumoList'

class ConciliacionDetalleSolicitudInfo extends Component {
     
    render() {



        return ( 
            <div>
                <ConciliacionSolicitudInfo data={this.props.data} /> 

                <ConciliacionConsumoList data={this.props.data.consumo_viandas} />

                <hr />
                <ActionForm onAccept={this.props.onHide} />
                
            </div>
         
        );
    }
}

export default ConciliacionDetalleSolicitudInfo;