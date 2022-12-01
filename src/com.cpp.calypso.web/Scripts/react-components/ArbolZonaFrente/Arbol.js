import React, { Component } from 'react';
import { Tree } from 'primereact/components/tree/Tree';
import { Button } from 'primereact/components/button/Button';

export default class TreeFull extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            data2: [{
                label: 'Zonas',
                children: this.props.data,
                expanded: true,
                labelcompleto: 'X'
            }],
            selectedFile: null,
            selectedFile3: null,
        }
    }

    render() {
        return (
            <div>
                <Tree
                    value={this.state.data2}
                    selectionMode="single"
                    layout="horizontal" 
                    selectionChange={this.props.onSelectionChange}
                >
                </Tree>
            </div>
        );
    }
}