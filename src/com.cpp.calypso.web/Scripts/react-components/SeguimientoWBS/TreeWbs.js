import React, { Component } from 'react';

import {Tree} from 'primereact/components/tree/Tree';

export default class TreeWbs extends React.Component{

    constructor(props){
        super(props);
        this.state = {
        }
    }

    render(){
        return(
            <div>
                <Tree value={this.props.data} selectionMode="single" ></Tree>
            </div>
        );
    }
}