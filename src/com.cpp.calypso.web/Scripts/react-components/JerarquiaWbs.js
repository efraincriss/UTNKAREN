import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';

import {OrganizationChart} from 'primereact/components/organizationchart/OrganizationChart';
export default class JerarquiaWbs extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            data: [{
                label: 'CEO',
                type: 'person',
                className: 'ui-person',
                expanded: true,
                data: "Proyecto"
            }]
        }
        this.updateData = this.updateData.bind(this);
    }

    componentDidMount(){
        this.updateData();
    }



    nodeTemplate(node) {
        if(node.type === "area") {
            return (
                <div>
                    <div className="node-header ui-corner-top">{node.label}</div>
                    <div className="node-content">
                            
                        <div>{node.data}</div>
                    </div>
                </div>
            );
        }

        if(node.type === "disciplina") {    
            return (
                <div>
                    <div className="node-header-disciplina ui-corner-top">{node.label}</div>
                    <div className="node-content-disciplina">
                            
                        <div>{node.data}</div>
                    </div>
                </div>
            );
        }

        if(node.type === "elemento") {    
            return (
                <div>
                    <div className="node-header-elemento ui-corner-top">{node.label}</div>
                    <div className="node-content-elemento">
                            
                        <div>{node.data}</div>
                    </div>
                </div>
            );
        }

        if(node.type === "actividad") {    
            return (
                <div>
                    <div className="node-header-actividad ui-corner-top">{node.label}</div>
                    <div className="node-content-actividad">
                            
                        <div>{node.data}</div>
                    </div>
                </div>
            );
        }

        return (<div>
            <div className="node-header-top ui-corner-top">{node.label}</div>
            <div className="node-content-top">
                
                <div>{node.data}</div>
            </div>
        </div>);
    }


    render(){

        return(
            <div className="row">
                <div className=" col tbl-container">
                    <OrganizationChart value={this.state.data} nodeTemplate={this.nodeTemplate.bind(this)} selectionMode="multiple" className="company"></OrganizationChart>
                </div>
            </div>
        )
    }

    updateData(){
        
        axios.get("/proyecto/Wbs/GetDiagramaApi/" + document.getElementById('content-jerarquia').className,
        {})
        .then((response) => {
            var data = [response.data]
            this.setState({data: data})
            
        })
        .catch((error) => {
            console.log(error);
            
        });
    }
}

ReactDOM.render(
    <JerarquiaWbs />,
    document.getElementById('content-jerarquia')
  );