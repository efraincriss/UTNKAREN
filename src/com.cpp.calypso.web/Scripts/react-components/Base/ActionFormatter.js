import React, { Component } from 'react';
import PropTypes from 'prop-types';

class ActionFormatter extends React.Component {

    constructor() {
        super();
        this.handleDelete = this.handleDelete.bind(this);
    }


    handleDelete(event) {
        event.preventDefault();

        var self = this;

        abp.message.confirm(
            this.props.actionDeleteMessage,
            "Confirmación",
            function (isConfirmed) {
                if (isConfirmed) {
                    self.props.onDeleteAction(self.props.rowData);
                }
            }
        );
    }
    
    render() {
        
        var buttons = this.props.buttons.map((button) => {
            const visible = (typeof button.onCondition === "function") ? button.onCondition(this.props.rowData) : true;
            return visible ?  <button key={Math.random()}
                    onClick={(e) => button.onClick(this.props.rowData)}
                    className={this.props.classActionDefault}
                >
                    {button.label}
            </button> : "";
        });
        
        return (
 
            <div>

                {this.props.onEditAction !== undefined && this.props.editCondition(this.props.rowData) &&
                    <button
                        onClick={(e) => this.props.onEditAction(this.props.rowData,e)}
                        className={this.props.classActionEdit}
                    >
                    {this.props.editActionName}
                    </button>
                }

                {this.props.onDetailAction !== undefined &&
                    <button
                        onClick={(e) => this.props.onDetailAction(this.props.rowData,e)}
                        className={this.props.classActionDetail}
                    >
                    {this.props.showActionName}
                    </button>
                }

                {this.props.onDeleteAction !== undefined && this.props.deleteCondition(this.props.rowData) &&
                    <button
                        onClick={this.handleDelete}
                        className={this.props.classActionDelete}
                    >
                        {this.props.deleteActionName}
                    </button>
                }

                {buttons}
            </div>
        );
    }
}

ActionFormatter.propTypes = {
    actionDeleteMessage: PropTypes.string,
    buttons: PropTypes.arrayOf(PropTypes.object),
    deleteActionName: PropTypes.string,
    editActionName: PropTypes.string,
    onDeleteAction: PropTypes.func,
    onDetailAction: PropTypes.func,
    onEditAction: PropTypes.func,
    rowData: PropTypes.object.isRequired,
    showActionName: PropTypes.string   
};


ActionFormatter.defaultProps = {
    actionDeleteMessage: "¿Está seguro de eliminar el registro, desea continuar?",
    showActionName: "Ver",
    editActionName: "Editar",
    editCondition: (row) => true,
    deleteActionName: "Eliminar",
    deleteCondition: (row) =>  true,
    classActionEdit: "btn btn-outline-primary btn-sm",
    classActionDownload: "btn btn-outline-indigo btn-sm",
    classActionDelete: "btn btn-outline-danger btn-sm",
    classActionDetail: "btn btn-outline-success btn-sm",
    classActionDefault: "btn btn-outline-primary btn-sm",
    buttons: []
};

 
export default function actionFormatter(cell, row, props) {
    return <ActionFormatter rowData={row}  {...props} />;
}