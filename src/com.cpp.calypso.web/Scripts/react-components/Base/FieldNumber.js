//import React from 'react';
//import PropTypes from 'prop-types';

//const FieldNumber = (props) => {

//    const {
//        name,
//        label,
//        onChange,
//        value,
//        error,
//        edit,
//        readOnly,
//        classLabel,
//        classControl,
//        classError,
//        required,
//        ...rest
//    } = props;

//    let control = undefined;

//    let markRequerid = required ? "* " : "";

//    if (edit) {
//        control = <input name={name} placeholder={placeholder} type="number" value={value} onChange={onChange} className="form-control" {...rest} />;
//    }

//    if (readOnly) {
//        control = <input name={name} type="number" value={value} readOnly className="form-control-plaintext" {...rest} />;
//    }

//    return (
//        <div className="form-group row">
//            <label htmlFor="{name}" className={classLabel}>{markRequerid}{label}</label>
//            <div className={classControl}>
//                {control} 
//                {error && <div className={classError}>{error}</div>}
//            </div>
//        </div>
//    );
//};

//FieldNumber.propTypes = {
//    classControl: PropTypes.string,
//    classError: PropTypes.string,
//    classLabel: PropTypes.string,
//    edit: PropTypes.bool,
//    label: PropTypes.string,
//    onChange: PropTypes.func,
//    readOnly: PropTypes.bool,
//    required: PropTypes.bool
//};

//FieldNumber.defaultProps = {
//    classLabel: "col-sm-12 col-form-label",
//    classControl: "col-sm-12",
//    classError: "alert alert-danger",
//    required:false

//};
 
//export default FieldText;