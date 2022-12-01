import React, { Component } from 'react';
import axios from 'axios';
import {Tree} from 'primereact-v2/components/tree/Tree';
import {Button} from 'primereact/components/button/Button';
export default class TreeWbs extends React.Component{

    constructor(props){
        super(props);
        this.state = {
           
            nodes: null,
            selectedFile: null, 
            selectedFile3: null,
            allkeys:{}
        }
   
       
    }

    render(){
        return(
            <div>
              
                <Tree value={this.props.data}
                 expandedKeys={this.props.expandedKeys} 
              
                dragdropScope="demo" onDragDrop={this.props.onDragDrop } 
               
                selectionMode="single"  selectionKeys={this.state.selectedFile} onSelect={this.props.onSelect}></Tree>
                
            </div>
        );
    }
}