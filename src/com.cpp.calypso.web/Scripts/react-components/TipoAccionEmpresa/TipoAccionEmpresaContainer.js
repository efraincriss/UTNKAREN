import React from "react";
import ReactDOM from "react-dom";
import BlockUi from "react-block-ui";
import axios from "axios";

import { Dialog } from "primereact/components/dialog/Dialog";

import config from "../Base/Config";
import http from "../Base/HttpService";
import wrapContainer from "../Base/WrapContainer";
import moment from "moment";

import TipoAccionEmpresaList from "./TipoAccionEmpresaList";
import TipoAccionEmpresaForm from "./TipoAccionEmpresaForm";

class TipoAccionEmpresaContainer extends React.Component {
  constructor() {
    super();

    this.state = {
      selectIds: [],
      visibleForm: false,
      entityId: 0,
      entityAction: "create",
      errors: {},
      urlApiBase: "/proveedor/TipoAccion/"
    };

    this.onNewItem = this.onNewItem.bind(this);
    this.onEditItem = this.onEditItem.bind(this);
    this.onDeleteItem = this.onDeleteItem.bind(this);
    this.onDetailItem = this.onDetailItem.bind(this);

    this.handleAdded = this.handleAdded.bind(this);
    this.handleUpdated = this.handleUpdated.bind(this);

    //this.handleSelect = this.handleSelect.bind(this);
    this.onHide = this.onHide.bind(this);
    //this.handleChangeDate = this.handleChangeDate.bind(this);
  }

  onNewItem(e) {
    console.log(e);
    this.setState({ entityId: 0, visibleForm: true, entityAction: "create" });
  }

  isValid() {
    const errors = {};

    this.setState({ errors });
    return Object.keys(errors).length === 0;
  }

  onEditItem(entity) {
    console.log(entity);
    this.setState({
      entityId: entity.Id,
      visibleForm: true,
      entityAction: "edit"
    });
  }

  onDeleteItem(entity) {
    console.log("onDeleteItem Item: " + entity);

    this.setState({ blocking: true });

    let url = "";
    url = `${this.state.urlApiBase}/DeleteApi`;

    let data = {
      id: entity.Id
    };

    http
      .post(url, data)
      .then(response => {
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
      .catch(error => {
        console.log(error);

        this.setState({ blocking: false });
      });
  }

  onDetailItem(entity) {
    console.log("DetailItem Item: " + entity);
    this.setState({
      entityId: entity.Id,
      visibleForm: true,
      entityAction: "show"
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

  handleSelect(isSelected, selectIds) {
    var newSelectIds = [];
    this.setState({ selectIds: selectIds });
  }

  render() {
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
                Nuevo{" "}
              </button>
            </div>
          </div>
        </div>

        <hr />

        <div>
          <TipoAccionEmpresaList
            data={this.props.data}
            selectIds={this.state.selectIds}
            onEditAction={this.onEditItem}
            onDeleteAction={this.onDeleteItem}
            onDetailAction={this.onDetailItem}
          />
        </div>

        <Dialog
          header="Registro de Alimentación - Acción"
          visible={this.state.visibleForm}
          width="640px"
          modal
          minY={70}
          onHide={this.onHide}
          maximizable
        >
          <TipoAccionEmpresaForm
            entityId={this.state.entityId}
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

// HOC
const Container = wrapContainer(
  TipoAccionEmpresaContainer,
  `/proveedor/TipoAccion/GetAllApi/`
);

ReactDOM.render(
  <Container />,
  document.getElementById("nuc_tree_body_tipo_accion")
);
