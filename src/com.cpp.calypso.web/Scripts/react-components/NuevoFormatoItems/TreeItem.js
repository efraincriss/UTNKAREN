import React, { Component } from "react";
import { Tree } from "primereact-v2/tree";
import { ContextMenu } from "primereact-v2/contextmenu";
import axios from "axios";
export default class TreeItem extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      selectedNodeKey: null,
      Id: 0,
      menu: [],
      ItemSeleccionado: null,
    };
  }
  componentWillReceiveProps(nextProps) {
    console.log(nextProps);

    this.setState({
      Id: nextProps.Id,
      menu:
        nextProps.Seleccionado != null && nextProps.Seleccionado.para_oferta
          ? [
              {
                label: "Editar",
                icon: "pi pi-pencil",
                command: () => {
                  this.props.showEdit();
                },
              },
              {
                label: "Eliminar",
                icon: "pi pi-times",
                command: () => {
                  this.props.showDelete();
                },
              },
            ]
          : [
              {
                label: "Insertar Hijo",
                icon: "pi pi-plus",
                command: () => {
                  this.props.showNew();
                },
              },

              {
                label: "Editar",
                icon: "pi pi-pencil",
                command: () => {
                  this.props.showEdit();
                },
              },
              {
                label: "Eliminar",
                icon: "pi pi-times",
                command: () => {
                  this.props.showDelete();
                },
              },
            ],
      ItemSeleccionado: nextProps.Seleccionado,
    });
  }

  render() {
    return (
      <div>
        <br />
        <ContextMenu
          appendTo={document.body}
          model={this.state.menu}
          ref={(el) => (this.cm = el)}
        />
        <Tree
          value={this.props.data}
          dragdropScope="demo"
          onSelect={this.props.onSelect}
          selectionMode="single"
          selectionKeys={this.state.selectedNodeKey}
          onSelectionChange={(e) => this.setState({ selectedNodeKey: e.value })}
          onContextMenuSelectionChange={(event) => this.props.SelectNode(event)}
          onContextMenu={(event) => this.cm.show(event.originalEvent)}
        />
      </div>
    );
  }
}
