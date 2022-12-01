import React, { Component } from "react";
import ReactDOM from "react-dom";
import axios from "axios";
import BlockUi from "react-block-ui";
import { Dialog } from "primereact/components/dialog/Dialog";
import { Button } from "primereact/components/button/Button";
import { Growl } from "primereact/components/growl/Growl";
//import { Tree } from 'primereact-v2/components/tree/Tree';
import { Tree } from "primereact-v3.3/tree";

export default class WbsPresupuestoDragDrop extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      visible: false,
      nodes: null,
      blocking: true,
      expandedKeys: {},
      selectedNodeKey: null,
      selectedNodeKeys1: null,
      selectedNodeKeys2: null,
      selectedNodeKeys3: null
    };
    this.Refrescar = this.Refrescar.bind(this);
    this.onClick = this.onClick.bind(this);
    this.onHide = this.onHide.bind(this);
    this.updateData = this.updateData.bind(this);
    this.GuardarDrag = this.GuardarDrag.bind(this);
    this.Expander = this.Expander.bind(this);
  }
  onClick(event) {
    console.log(this.state.nodes);
    this.setState({ visible: true });
  }
  changedrag = event => {
    /*console.clear();
    console.log(event);*/
    this.setState({ nodes: event.value });
  };
  selectionchange = event => {
    console.log("sekection change", event);
  };
  Refrescar(event) {
    console.log("refrescar");
    this.updateData();
  }
  Expander() {
    axios
      .post(
        "/proyecto/WbsPresupuesto/ApiWbsK/" +
          document.getElementById("PresupuestoId").className,
        {
          data: this.state.data
        }
      )
      .then(response => {
        var llaves = response.data;
        let expandedKeys = { ...this.state.expandedKeys };
        llaves.forEach(product => {
          if (expandedKeys[product]) delete expandedKeys[product];
          else expandedKeys[product] = true;
        });
        this.setState({ expandedKeys: expandedKeys });
      })
      .catch(error => {
        console.log(error);
      });
  }

  onHide(event) {
    this.setState({ visible: false });
  }
  componentDidMount() {
    this.updateData();
  }
  updateData() {
    axios
      .get(
        "/proyecto/WbsPresupuesto/ApiWbsL/" +
          document.getElementById("content-presupuestos").className,
        {}
      )
      .then(response => {
        this.setState({ nodes: response.data, blocking: false });
      })
      .catch(error => {
        console.log(error);
      });
  }
  GuardarDrag() {
    console.log(this.state.nodes);
    this.setState({ blocking: true });
    axios
      .post("/proyecto/WbsPresupuesto/ApiWbsD/", {
        data: this.state.nodes
      })
      .then(response => {
        if (response.data == "OK") {
          this.growl.show({
            life: 5000,
            severity: "info",
            summary: "Proceso exitoso!",
            detail: "OK"
          });
          this.updateData();
          this.setState({ blocking: false, visible: false });
        } else {
        }
      })
      .catch(error => {
        console.log(error);
      });
  }

  render() {
    const footer = (
      <div>
        <Button label="Si" icon="pi pi-check" onClick={this.GuardarDrag} />
        <Button
          label="No"
          icon="pi pi-times"
          onClick={this.onHide}
          className="p-button-secondary"
        />
      </div>
    );
    return (
      <div>
        <BlockUi tag="div" blocking={this.state.blocking}>
          <Growl
            ref={el => {
              this.growl = el;
            }}
            position="bottomright"
            baseZIndex={1000}
          ></Growl>
          <Button
            label="Refrescar"
            icon="pi pi-refresh"
            onClick={this.Refrescar}
          />
          <Button
            label="Guardar"
            icon="pi pi-external-link"
            onClick={this.onClick}
          />
          <Button
            label="Exp/Contraer"
            icon="pi pi-external-link"
            onClick={this.Expander}
          />
          <div className="p-tree p-component">
            <Tree
              // key={Math.random()}
              value={this.state.nodes}
              //selectionMode="single"
                expandedKeys={this.state.expandedKeys}
               onToggle={e => this.setState({ expandedKeys: e.value })}
              //style={{ marginTop: ".5em" }}
              dragdropScope="drag"
              //filter
              onDragDrop={
                //event => this.setState({ nodes: event.value })
                event => this.changedrag(event)
              }
              onSelectionChange={event => this.selectionchange(event)}
              //filter={true}
              //filterMode="strict"
              //appendTo={document.body}
            />
          </div>
          <Dialog
            header="Reordenar"
            visible={this.state.visible}
            width="350px"
            footer={footer}
            minY={70}
            onHide={this.onHide}
            maximizable={true}
          >
            Esta Seguro de Guardar?
          </Dialog>
        </BlockUi>
      </div>
    );
  }
}

ReactDOM.render(
  <WbsPresupuestoDragDrop />,
  document.getElementById("content_reordenar")
);
