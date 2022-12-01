import React from "react";
import BlockUi from "react-block-ui";

import { Dialog } from "primereact/components/dialog/Dialog";

import http from "../Base/HttpService";
import ProveedorZonaList from "./ProveedorZonaList";
import ProveedorZonaForm from "./ProveedorZonaForm";

class ProveedorZonaContainer extends React.Component {
  constructor() {
    super();
    this.state = {
      visibleForm: false,
      entityId: 0,
      entityAction: "create",
      errors: {},
      urlApiBase: "/proveedor/Proveedor/",
    };

    this.onNewItem = this.onNewItem.bind(this);
    this.onEditItem = this.onEditItem.bind(this);
    this.onDeleteItem = this.onDeleteItem.bind(this);
    this.onDetailItem = this.onDetailItem.bind(this);

    this.handleAdded = this.handleAdded.bind(this);
    this.handleUpdated = this.handleUpdated.bind(this);

    this.onHide = this.onHide.bind(this);
  }

  componentDidMount() {}

  onNewItem(e) {
    console.log(e);
    console.log("this.props.data", this.props.data);
    if (this.props.data !== null && this.props.data.length == 0) {
      this.setState({ entityId: 0, visibleForm: true, entityAction: "create" });
    } else {
      abp.notify.error('Solo puede ingresar una zona por proveedor', "Error");
    }
  }

  onEditItem(entity) {
    console.log(entity);
    this.setState({
      entityId: entity.Id,
      visibleForm: true,
      entityAction: "edit",
    });
  }

  onDetailItem(entity) {
    console.log("DetailItem Item: " + entity);
    this.setState({
      entityId: entity.Id,
      visibleForm: true,
      entityAction: "show",
    });
  }

  onDeleteItem(entity) {
    console.log("onDeleteItem Item: " + entity);

    this.setState({ blocking: true });

    let url = "";
    url = `${this.state.urlApiBase}/DeleteProveedorZonaApi`;

    let data = {
      id: entity.Id,
    };

    http
      .post(url, data)
      .then((response) => {
        let data = response.data;

        if (data.success === true) {
          abp.notify.success("Proceso guardado exitosamente", "Aviso");

          var newParams = {};

          this.props.onRefreshData(newParams);
        } else {
          //TODO:
          //Presentar errores...
          var message = $.fn.responseAjaxErrorToString(data);
          abp.notify.error(message, "Error");
        }

        this.setState({ blocking: false });
      })
      .catch((error) => {
        console.log(error);

        this.setState({ blocking: false });
      });
  }

  handleAdded(entity) {
    console.log("add Item: ");
    console.log(entity);
    this.setState({ visibleForm: false, entityId: 0 });

    abp.notify.success("Proceso guardado exitosamente", "Aviso");

    var newParams = {};

    this.props.onRefreshData(newParams);
  }

  handleUpdated(entity) {
    console.log("update Item: ");
    console.log(entity);
    this.setState({ visibleForm: false, entityId: 0 });

    abp.notify.success("Proceso guardado exitosamente", "Aviso");

    var newParams = {};

    this.props.onRefreshData(newParams);
  }

  onHide() {
    console.log("onHide ");
    this.setState({ visibleForm: false, entityId: 0 });
  }

  render() {
    let dialogWith = "420px";

    let blocking = this.props.blocking || this.state.blocking;

    return (
      <BlockUi tag="div" blocking={blocking}>
        <div className="row">
          <div className="col">
            <div className="col nav justify-content-end">
              <button
                className="btn btn-outline-primary"
                onClick={this.onNewItem}
              >
                {" "}
                Nueva Zona{" "}
              </button>
            </div>
          </div>
        </div>

        <hr />
        <div>
          <ProveedorZonaList
            data={this.props.data}
            onEditAction={this.onEditItem}
            onDeleteAction={this.onDeleteItem}
          />
        </div>

        <Dialog
          header="Zona"
          visible={this.state.visibleForm}
          width="420px"
          modal
          minY={210}
          onHide={this.onHide}
          maximizable
        >
          <ProveedorZonaForm
            entityId={this.state.entityId}
            padreId={this.props.padreId}
            entityAction={this.state.entityAction}
            onUpdated={this.handleUpdated}
            onAdded={this.handleAdded}
            onHide={this.onHide}
            show={this.state.visibleForm}
            urlApiBase={this.state.urlApiBase}
          />
        </Dialog>
      </BlockUi>
    );
  }
}

export default ProveedorZonaContainer;
