////import React from 'react';
////import PropTypes from 'prop-types';

////export default function wrapInput(Component,name,label,error) {

////    return class WrappedComponent extends React.Component {

////        constructor(props) {
////            super(props);
////        }


////        render() {
////            return (

////                <div className="form-group row">
////                    <label htmlFor={name} className={this.props.classLabel}>{label}</label>
////                    <div className={this.props.classControl}>
////                        <Component />
////                        {error && <div className={this.props.classError}>{error}</div>}
////                    </div>
////                </div>

////            );
////        } 
////    };

////}


////WrappedComponent.propTypes = {
////    classControl: PropTypes.string,
////    classError: PropTypes.string,
////    classLabel: PropTypes.string,
////    label: PropTypes.string,
////    name: PropTypes.string
////};

////WrappedComponent.defaultProps = {
////    classLabel: "col-sm-12 col-form-label",
////    classControl: "col-sm-12",
////    classError: "alert alert-danger"

////};
 