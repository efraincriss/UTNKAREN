import React from 'react'
import PropTypes from 'prop-types';

export default function wrapForm(Component) {

    return class WrappedComponent extends React.Component {

        constructor(props) {
            super(props);
        } 


        mapDropdown(data, nameField = 'name', valueField = 'Id') {
            if (data.success === true) {

                return data.result.map(i => {
                    return { label: i[nameField], value: i[valueField] }
                });
            } else if (data !== undefined) {

                return data.map(i => {
                    return { label: i[nameField], value: i[valueField] }
                });
            }

            return {};
        }

        getExtraSelect(lista) {
            return (
                lista.map((item) => {
                    return (
                        <option key={Math.random()} value={item.Id}>{item.nombre}</option>
                    );
                })
            );
        } 


        getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }
   

        render() {
            return (

                <Component
                    {...this.state}
                    {...this.props}

                    mapDropdown={this.mapDropdown}
                    getExtraSelect={this.getExtraSelect}
                    getParameterByName={this.getParameterByName}
                />

            );
        }
    }

}

/*
wrapForm.propTypes = {
    urlApiBase: PropTypes.string.isRequired
};

*/