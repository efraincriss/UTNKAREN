import React, { Component } from 'react';

import { Tree } from 'primereact/components/tree/Tree';
import { Button } from 'primereact/components/button/Button';

export default class ComputoItem extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            selectedFile: null,
            selectedFile3: null,
        }
    }

    render() {
        return (
            <div>

            <Tree value={this.props.data} selectionMode="single" selectionChange={this.props.onSelectionChange}></Tree>

        </div>
    );
}
}