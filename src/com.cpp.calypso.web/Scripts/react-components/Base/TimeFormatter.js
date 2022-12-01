import React, { Component } from 'react';
import PropTypes from 'prop-types';

import config from '../Base/Config';
import moment from 'moment';
 
class TimeFormatter extends React.Component {

    constructor() {
        super();
    }


    render() {

        let fecha = (this.props.date !== undefined) ? moment(this.props.date).format(config.formatTime) : null;
        
        return (
            <span>{fecha}</span>
        );
    }
}

/*
DateFormatter.propTypes = {
    date: PropTypes.string.isRequired, 
};
*/

export default function timeFormatter(cell, row, props) {
 
    return <TimeFormatter date={cell}  {...props} />;

}