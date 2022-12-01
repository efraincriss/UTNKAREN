import React from 'react'
import PropTypes from 'prop-types';

export default function wrapForm(Component) {

    return class WrappedComponent extends React.Component {

        constructor(props) {
            super(props);

            this.state = {
                
                data: {},
                blocking: true,
                loadDataExtra: false,
                loading: false,
                errors: {},
                loadDataExtraHandler: null,
                initDataExtraHandler: null


            };

            this.refComponent = React.createRef();

            this.handleChange = this.handleChange.bind(this);
            this.setData = this.setData.bind(this);

            this.completedDataExtraHandler = this.completedDataExtraHandler.bind(this);
            this.registerLoadDataExtraHandler = this.registerLoadDataExtraHandler.bind(this);
            this.registerInitDataExtraHandler = this.registerInitDataExtraHandler.bind(this);

            this.handleSubmit = this.handleSubmit.bind(this);
      
        }

 
        componentDidMount() {
            console.log('WrappedComponent.componentDidMount');

            if (typeof this.state.initDataExtraHandler === "function") {
                let initData = this.state.initDataExtraHandler();
                this.setState({
                    data: initData
                });
            }
             
        }

        componentDidUpdate(prevProps) {
            console.log('WrappedComponent.componentDidUpdate');

            if (!this.state.loadDataExtra && this.props.show && !this.state.loading) {
                //Init

                if (typeof this.state.loadDataExtraHandler === "function") {

                    this.setState({ loading: true, blocking: true });
                    //this.state.loadDataExtraHandler();
                    this.refComponent.current.loadDataExtraHandler();
                }
            }

            // Typical usage (don't forget to compare props):
            if (this.props.show && (this.props.entityId !== prevProps.entityId ||
                this.props.show !== prevProps.show)) {
                this.loadData();
            }
        }

        loadData() {
            console.log('this.props.entityId : ' + this.props.entityId);

            if (this.props.entityId > 0) {

                let url = '';
                url = `${this.props.urlApiBase}/GetApi/${this.props.entityId}`;


                http.get(url, {})
                    .then((response) => {

                        let data = response.data;

                        if (data.success === true) {

                            //Normalizing
                            let dataEntity = data.result;
                              
                            if (this.props.NormalizingData) {

                                dataEntity = this.props.NormalizingData(dataEntity)
                            }

                            this.setState({
                                data: dataEntity
                            });

                        } else {
                            //TODO: 
                            //Presentar errores... 
                            var message = $.fn.responseAjaxErrorToString(data);
                            abp.notify.error(message, 'Error');
                        }


                        this.setState({ blocking: false, errors: {} });

                    })
                    .catch((error) => {
                        console.log(error);
                    });
            } else {

                this.setState({
                    data: this.props.initData(),
                    errors: {},
                    blocking: false
                });

            }
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

        handleSubmit(event) {
            event.preventDefault();

            if (!this.props.isValid()) {
                return;
            }

            console.log(this.state);

            this.setState({ blocking: true });

            let url = '';
            if (this.props.entityAction === 'edit')
                url = `${this.props.urlApiBase}/EditApi`;
            else
                url = `${this.props.urlApiBase}/CreateApi`;


            //creating copy of object
            let data = Object.assign({}, this.state.data);
            data.Id = this.props.entityId;                        //updating value


            http.post(url, data)
                .then((response) => {

                    let data = response.data;

                    if (data.success === true) {

                        this.setState({
                            data: this.initData()
                        });


                        if (this.props.entityId <= 0) {
                            this.props.onAdded(response.data.result.id);
                        }
                        else {
                            this.props.onUpdated(response.data.result.id);
                        }

                    } else {
                        //TODO: 
                        //Presentar errores... 
                        var message = $.fn.responseAjaxErrorToString(data);
                        abp.notify.error(message, 'Error');
                    }


                    this.setState({ blocking: false });

                })
                .catch((error) => {
                    console.log(error);

                    this.setState({ blocking: false });
                });

        }

        handleChange(event) {
            const target = event.target;
            const value = target.type === "checkbox" ? target.checked : target.value;
            const name = target.name;

            const updatedData = {
                ...this.state.data
            };

            updatedData[name] = value;


            this.setState({
                data: updatedData
            });
        }


        setData(name, value) {

            const updatedData = {
                ...this.state.data
            };

            updatedData[name] = value;

            this.setState({
                data: updatedData
            });
        }


        completedDataExtraHandler() {
            this.setState({ loading: false, loadDataExtra: true, blocking: false  });
        }

        registerLoadDataExtraHandler(handler) {
            this.setState({ loadDataExtraHandler: handler });
        }

        registerInitDataExtraHandler(handler) {
            this.setState({ initDataExtraHandler: handler }); 
        }

        render() {
            return (

                <Component
                    {...this.state}
                    {...this.props}

                    ref={this.refComponent}

                    data={this.state.data}

                    blocking={this.state.blocking}

                    registerLoadDataExtraHandler={this.registerLoadDataExtraHandler}
                    registerInitDataExtraHandler={this.registerInitDataExtraHandler}
                    completedDataExtraHandler={this.completedDataExtraHandler}

                    handleSubmit={this.handleSubmit}
                    handleChange={this.handleChange}
                    setData={this.setData}
                    mapDropdown={this.mapDropdown}
                    getExtraSelect={this.getExtraSelect}
                />

            );
        }
    }

}

wrapForm.propTypes = {
    urlApiBase: PropTypes.string.isRequired
};
