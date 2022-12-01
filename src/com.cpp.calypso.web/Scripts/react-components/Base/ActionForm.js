import React, { Component } from 'react';
import PropTypes from 'prop-types';

class ActionForm extends React.Component {

    constructor() {
        super();
    }


    render() {
    
        return (
				<div className="nav">

                    {!this.props.onAccept && !this.props.onSave &&
                    <button type="submit" className="btn btn-outline-primary"><i className="fa fa-save"></i>  {this.props.saveActionName}  </button>
                    }

                    {this.props.onSave &&
                    <button onClick={this.props.onSave} type="button" className="btn btn-outline-primary"><i className="fa fa-save"></i>  {this.props.saveActionName} </button>
                    }

                    {this.props.onCancel && !this.props.onAccept   &&
                         <button onClick={this.props.onCancel} type="button" className="btn btn-outline-danger"><i className="fa fa-ban"></i>  {this.props.cancelActionName}</button>
                    }

                    {this.props.onAccept  &&
                    <button onClick={this.props.onAccept} type="button" className="btn btn-outline-primary"><i className="fa fa-check"></i>  {this.props.acceptActionName}</button>
                    }

				</div>
        );
		 
    }
}


ActionForm.propTypes = {
    saveActionName: PropTypes.string,
    cancelActionName: PropTypes.string,
    acceptActionName: PropTypes.string,
    onSave: PropTypes.func
};

ActionForm.defaultProps = {
    saveActionName: "Guardar",
    cancelActionName: "Cancelar",
    acceptActionName: "Cerrar" 
}
 
 
export default ActionForm