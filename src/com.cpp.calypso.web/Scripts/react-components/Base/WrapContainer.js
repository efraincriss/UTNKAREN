import React from 'react';
import axios from 'axios';
import http from '../Base/HttpService';

import PropTypes from 'prop-types';

export default function wrapContainer(Component, url, params) {

    
    return class WrappedComponent extends React.Component {

        constructor(props) {
            super(props);

            this.state = {
                data: [],
                blocking: true,
                hasError: false,
                error: '',
                url: url,
                params: params
            };

            this.GetData = this.GetData.bind(this);
            this.onRefreshData = this.onRefreshData.bind(this);
        }

        componentDidMount() {
            this.GetData(this.state.url, this.state.params);
        }


        GetData(url,params) {

            console.log(url);
            console.log(params);

            this.setState({ blocking: true });

            http.get(url, {
                params
            })
                .then((response) => {
                    let data = response.data;

                    if (data.success === true) {

                        //Is pagination Server
                        if (data.result.TotalCount !== undefined) {

                            this.setState({ data: data.result.Items });

                        } else {
                            this.setState({ data: data.result });
                        }
                    } else {
                        var message = $.fn.responseAjaxErrorToString(data);
                        this.setState({ hasError: true, error: message });

                        //TODO: 
                        //Presentar errores... 
                        abp.notify.error(message, 'Error');
                    }

                    this.setState({ blocking: false });
                })
                .catch((error) => {
                    this.setState({ blocking: false, hasError: true, error: error });

                    //TODO: Pendiente (Gestion de errores)
                    console.log(error);
                });
        }

        onRefreshData(newParams) {

            this.setState({ params: newParams });

            this.GetData(this.state.url, newParams);
        }

        render() {
            return (

                <Component
                    {...this.state}
                    {...this.props}
                    onRefreshData={this.onRefreshData}
                    data={this.state.data}
                    blocking={this.state.blocking}
                />

            );
        }
    }

}

/*
wrapContainer.propTypes = {
    rowData: PropTypes.object.isRequired
};
*/