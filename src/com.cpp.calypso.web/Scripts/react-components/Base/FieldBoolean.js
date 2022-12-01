//import React from 'react';
//import PropTypes from 'prop-types';

//const FieldBoolean = (props) => {

//    const {
//        name,
//        label,
//        labelOption,
//        onChange,
//        value,
//        error,
//        edit,
//        readOnly,
//        classLabel,
//        classControl,
//        classError,
//        ...rest
//    } = props;

//    let control = undefined;
    
//    if (edit) {
//        control = <input name={name} type="checkbox" checked={value} onChange={onChange} {...rest} />;
//    } 

//    if (readOnly) {
//        control = <input name={name} type="checkbox" checked={value} readOnly {...rest} />;
//    }
    

//    return (
//        <div className="form-group row">
//            <label htmlFor={name} className={classLabel}>{label}</label>
//            <div className={classControl}>
//                <div>
//                    {control}
//                    <label className="form-check-label" htmlFor={name}>
//                        {labelOption}
//                    </label>
//                </div>
//                {error && <div className={classError}>{error}</div>}
//            </div>
//        </div>
//    );
//};
 

//FieldBoolean.propTypes = {
//    classControl: PropTypes.string,
//    classError: PropTypes.string,
//    classLabel: PropTypes.string,
//    edit: PropTypes.bool,
//    label: PropTypes.string,
//	labelOption:PropTypes.string,
//	onChange:PropTypes.func,
//    readOnly: PropTypes.bool   
//};

//FieldBoolean.defaultProps = {
//    classControl: "col-sm-12",
//    classError: "alert alert-danger",
//    classLabel: "col-sm-12 col-form-label"
//};
 
//export default FieldBoolean;