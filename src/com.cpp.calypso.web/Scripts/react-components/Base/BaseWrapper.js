import React from 'react';
import BlockUi from 'react-block-ui';


export default function BaseWrapper(Component, autoUnlock = false) {

    return class WrappedComponent extends React.Component {
        constructor(props) {
            super(props)

            this.state = {
                blocking: true,
                displayMessage: '',
            }
        }

        render() {
            return (
                <div className="row">
                    <div style={{ width: '100%' }}>
                        <div className="col">
                            <BlockUi tag="div" blocking={this.state.blocking}>
                                <Component
                                    {...this.state}
                                    {...this.props}
                                    showSuccess={this.showSuccess}
                                    showWarning={this.showWarning}
                                    showWarn={this.showWarn}
                                    blockScreen={this.blockScreen}
                                    unlockScreen={this.unlockScreen}
                                    buildDropdown={this.buildDropdown}
                                    getParameterByName={this.getParameterByName}
                                    showValidation={this.showValidation}
                                />
                            </BlockUi>
                        </div>
                    </div>
                </div>
            )
        }


        buildDropdown = (data, nameField = 'name', valueField = 'Id') => {
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

        getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }

        warn = () => {
            abp.notify.error(this.state.displayMessage, 'Error');
        }

        success = () => {
            abp.notify.success(this.state.displayMessage, "Aviso");
        }

        warning = () => {
            abp.notify.warn(this.state.displayMessage, 'Alerta');
        }

        validation = () => {
            abp.notify.error(this.state.displayMessage, 'Validación');
        }

        showSuccess = displayMessage => {
            this.setState({ displayMessage }, this.success)
        }

        showWarning = displayMessage => {
            this.setState({ displayMessage }, this.warning)
        }


        showWarn = displayMessage => {
            this.setState({ displayMessage }, this.warn)
        }

        showValidation = displayMessage => {
            this.setState({ displayMessage }, this.validation)
        }

        blockScreen = () => {
            this.setState({ blocking: true })
        }

        unlockScreen = () => {
            this.setState({ blocking: false })
        }
    }
}