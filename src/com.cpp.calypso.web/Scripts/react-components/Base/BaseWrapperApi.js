import React from 'react';
import http from './HttpService';
import BlockUi from 'react-block-ui';


export default function BaseWrapper(Component, url, bodyParams, autoUnlock = true){

    return class WrappedComponent extends React.Component{
        constructor(props){
            super(props)

            this.state = {
                blocking: true,
                displayMessage: '',
                data: [],
                bodyParams: bodyParams,
                url: url,

            }
        }

        componentDidMount() {
            this.LoadData(this.state.url, this.state.bodyParams);
        }

        render(){
            return(
                <BlockUi tag="div" blocking={this.state.blocking}>
                    <Component
                        {...this.state}
                        {...this.props}
                        onRefreshData={this.onRefreshData}
                        data={this.state.data}
                        blocking={this.state.blocking}
                        showSuccess={this.showSuccess}
                        showWarn={this.showWarn}
                        showValidation={this.showValidation}
                        blockScreen={this.blockScreen}
                        unlockScreen={this.unlockScreen}
                    />
                </BlockUi>
            )
        }

        LoadData = (url, bodyParams) => {
            http.get(url, {
                bodyParams
            })
            .then((response) => {
                let data = response.data;

                if (data.success === true) {
                    this.setState({ data: data.result });
                } else {
                    var message = $.fn.responseAjaxErrorToString(data);
                    this.setState({displayMessage: message }, this.showWarn);
                }

                if(autoUnlock){
                    this.setState({ blocking: false });
                }
                
            })
            .catch((error) => {
                this.setState({ blocking: false, error: error });
            });
        }

        warn = () => {
            abp.notify.error(this.state.displayMessage, 'Error');
        }

        success = () => {
            abp.notify.success(this.state.displayMessage, "Aviso");
        }

        validation = () => {
            abp.notify.error(this.state.displayMessage, 'Validación');
        }

        showSuccess = displayMessage => {
            this.setState({displayMessage}, this.success)
        }

        showWarn = displayMessage => {
            this.setState({displayMessage}, this.warn)
        }

        showValidation = displayMessage => {
            this.setState({displayMessage}, this.validation)
        }

        


        onRefreshData = (newParams) => {
            this.setState({ bodyParams: newParams });
            this.LoadData(this.state.url, newParams);
        }

        blockScreen = () => {
            this.setState({blocking: true})
        }

        unlockScreen = () => {
            this.setState({blocking: false})
        }
    }
}