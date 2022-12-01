import React, { Component } from 'react';
import PropTypes from 'prop-types';

import config from '../Base/Config';
import moment from 'moment';
 
class NumberFormatter extends React.Component {

    constructor() {
        super();
    }


    render() {

          
        return (
            <span>{this.props.number}</span>
        );
    }
}

/*
DateFormatter.propTypes = {
    date: PropTypes.string.isRequired, 
};
*/

export default function numberFormatter(cell, row, props) {
 
    return <NumberFormatter number={cell}  {...props} />;

}