import React, { Component } from "react";
import PropTypes from "prop-types";

class StateFormatter extends React.Component {
  constructor() {
    super();
  }

  render() {
    let editLabel = "Editar";
    if (this.props.editActionName !== undefined) {
      editLabel = this.props.editActionName;
    }

    let showLabel = "Visualizar";
    if (this.props.showActionName !== undefined) {
      showLabel = this.props.showActionName;
    }

    let deleteLabel = "Eliminar";
    if (this.props.deleteActionName !== undefined) {
      deleteLabel = this.props.deleteActionName;
    }

    return (
      <div>
        {this.props.onEditAction !== undefined && (
          <button
            onClick={() => this.props.onEditAction(this.props.rowData.Id)}
            className="btn btn-outline-primary btn-sm"
          >
            {editLabel}
          </button>
        )}

        {this.props.onDetailAction !== undefined && (
          <button
            onClick={() => this.props.onDetailAction(this.props.rowData.Id)}
            className="btn btn-outline-success btn-sm"
          >
            {showLabel}
          </button>
        )}

        {this.props.onDeleteAction !== undefined && (
          <button
            onClick={() => {
              if (window.confirm("Est�s seguro?"))
                this.props.onDeleteAction(this.props.rowData.Id);
            }}
            className="btn btn-outline-danger btn-sm"
          >
            {deleteLabel}
          </button>
        )}
      </div>
    );
  }
}

ActionFormatter.propTypes = {
  rowData: PropTypes.object.isRequired,
};

export default function actionFormatter(cell, row, props) {
  return <ActionFormatter rowData={row} {...props} />;
}
