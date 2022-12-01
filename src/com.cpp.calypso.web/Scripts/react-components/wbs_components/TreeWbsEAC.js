import React, { Component } from "react";
import { Tree } from "primereact-v3.3/tree";
import { Button } from "primereact-v3.3/components/button/Button";

export default class TreeWbs extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      selectedFile: null,
      selectedFile3: null,
      selectedNodeKey: null,
    };
  }

  render() {
    return (
      <div>
        <Tree
          filter={true}
          filterMode="strict"
          value={this.props.data}
          dragdropScope="demo"
          onSelect={this.props.onSelectionChange}
          selectionMode="single"
          selectionKeys={this.state.selectedNodeKey}
          onSelectionChange={(e) => this.setState({ selectedNodeKey: e.value })}
        ></Tree>
      </div>
    );
  }
}
