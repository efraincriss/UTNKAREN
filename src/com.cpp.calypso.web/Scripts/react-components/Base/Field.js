import React from 'react';
import PropTypes from 'prop-types';
import { Dropdown } from 'primereact/components/dropdown/Dropdown';
import config from './Config';
import moment from 'moment';

const Field = (props) => {

    const {
        name,
        label,
        labelOption,
        options,
        onChange,
        value,
        error,
        edit,
        readOnly,
        classLabel,
        classControl,
        classError,
        required,
        type,
        ...rest
    } = props;

     
    let control = undefined;

    let markRequerid = required ? "* " : "";

    let typeControl = type.toUpperCase();

    let valueSafe = (value === null) ? "" : value;
 

    let classLabelLocal = classLabel;

    if (typeControl === "SELECT") {
        let disabled = false;
        if (readOnly) {
            classLabelLocal = classLabelLocal + " label-read-only";
            disabled = true;
        }

        control = (
            <Dropdown
                id={name}
                value={valueSafe}
                options={options}
                style={{ width: '100%' }}
                disabled={disabled}
                onChange={(e) => {
                    onChange(name, e.value);
                }}
                {...rest}
            />
        );
    }

    if (typeControl === "TEXT" || typeControl === "DATE" || typeControl === "TIME" || typeControl==="NUMBER") {

        if (typeControl === "DATE") {
            valueSafe = valueSafe;
            if (valueSafe !== undefined && valueSafe !== null && moment(valueSafe).isValid()) {
                valueSafe = moment(valueSafe).format(config.formatDate);
            } else {
                valueSafe = '';
            }
        }

        if (typeControl === "TIME") {
            valueSafe = valueSafe;
            if (valueSafe !== undefined && valueSafe !== null && moment(valueSafe, config.formatTime).isValid()) {
                valueSafe = moment(valueSafe, config.formatTime).format(config.formatTime);
            } else {
                valueSafe = '';
            }
        }

        let classValueReadOnly = "form-control-plaintext";
        if (typeControl === "NUMBER") {
            classValueReadOnly = "form-control-plaintext text-right";
        }
    

        if (edit) {
            control = <input name={name} type={type} value={valueSafe} onChange={onChange} className="form-control" {...rest} />;
        }

        if (readOnly) {
            classLabelLocal = classLabelLocal + " label-read-only";
            control = <input name={name} type={type} value={valueSafe} readOnly className={classValueReadOnly} {...rest} />;
        }
    }

    if (typeControl === "TEXTAREA") {
        
        let classValueReadOnly = "form-control-plaintext";
        
        if (edit) {
            control = <textarea name={name}   value={valueSafe} onChange={onChange} className="form-control" {...rest} />;
        }

        if (readOnly) {
            classLabelLocal = classLabelLocal + " label-read-only";
            control = <textarea name={name} type={type} value={valueSafe} readOnly className={classValueReadOnly} {...rest} />;
        }
    }

    if (typeControl === "FILE") {

        let classValueReadOnly = "form-control-plaintext";

        if (edit) {
            control = <input name={name} type={type}  onChange={onChange} className="form-control" {...rest} />;
        }

        if (readOnly) {
            classLabelLocal = classLabelLocal + " label-read-only";
            control = <input name={name} type={type}  readOnly className={classValueReadOnly} {...rest} />;
        }
    }

    if (typeControl === "CHECKBOX") {
        if (edit) {
            control = (
                <div>
                    <input name={name} type="checkbox" checked={valueSafe} onChange={onChange} {...rest} />
                    <label className="form-check-label ml-2" htmlFor={name}>
                        {" "} {labelOption}
                    </label>
                </div>
                );
        }

        if (readOnly) {
            classLabelLocal = classLabelLocal + " label-read-only";

            control = (
                <div>
                    <input readOnly name={name} type="checkbox" checked={valueSafe} onChange={onChange} {...rest} />
                    <label className="form-check-label" htmlFor={name}>
                        {labelOption}
                    </label>
                </div>
            );
        }
    } 
  
    return (
        <div className="form-group row">
            <label htmlFor={name} className={classLabelLocal}>{markRequerid}{label}</label>
            <div className={classControl}>
                {control}
                {error && <div className={classError}>{error}</div>}
            </div>
        </div>
    );
};

Field.propTypes = {
    classControl: PropTypes.string,
    classError: PropTypes.string,
    classLabel: PropTypes.string,
    edit: PropTypes.bool,
    label: PropTypes.string,
    name: PropTypes.string.isRequired,
    onChange: PropTypes.func,
    options: PropTypes.array,
    readOnly: PropTypes.bool,
    required: PropTypes.bool,
    type: PropTypes.string.isRequired    
};

Field.defaultProps = {
    classLabel: "col-sm-12 col-form-label",
    classControl: "col-sm-12",
    classError: "alert alert-danger",
    required: false,
    type:"Text"

};

export default Field;