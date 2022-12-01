import React from 'react';
import PropTypes from 'prop-types';
import { Dropdown } from 'primereact/components/dropdown/Dropdown';


///TODO: No funciona correctamente... en react table boostrap. no puede salir del limite de caja de la cell
class DropdownCellEditor extends React.Component {

    constructor(props) {
        super(props);
        this.updateData = this.updateData.bind(this);
        this.state = {
            value: props.defaultValue 
        };
    }

    focus() {
        //TODO: Analizar como establecer el focus
        //this.refs.inputRef.focus();
        this.refs.inputRef.onInputFocus();
    }

    updateData(event) {
        console.log(event);
        /*
        var selectedIndex = event.nativeEvent.target.selectedIndex;

        if (selectedIndex > 0) {

            var text = event.nativeEvent.target[selectedIndex].text;
            //Actualizar Cell
            this.props.onUpdate(text);

            //Si existe un callBack para actualizar datos
            if (this.props.onUpdateData) {

                //var value = event.currentTarget.value;
                var itemSelected = null;
                if (this.props.textSelect)
                    itemSelected = { ...this.props.data[selectedIndex - 1] };
                else
                    itemSelected = { ...this.props.data[selectedIndex] };

                //Retornar la fila que se esta editando, y el item seleccionado del Selecte
                this.props.onUpdateData(this.props.row, itemSelected);
            }
        }*/
    }

    
    render() {
        
        let options = this.props.data.map(item => (<option key={item[this.props.fieldValueName]} value={item[this.props.fieldTextName]}>{item[this.props.fieldTextName]}</option>));

        return (
            <span>

                <Dropdown
                    ref='inputRef'
                    value={this.state.value}
                    options={options}
                    onChange={this.updateData}
                    filter={true} filterPlaceholder={this.props.textSelect}
                    filterBy="label,value" placeholder={this.props.textSelect}
                    style={{ width: '100%' }}
                    required
                />
            </span>
        );
}


}


DropdownCellEditor.propTypes = {
    fieldNameValueUpdate: PropTypes.string.isRequired,
    fieldTextName: PropTypes.string,
    fieldValueName: PropTypes.string,
    onUpdateData: PropTypes.func,
    textSelect: PropTypes.string    
};

DropdownCellEditor.defaultProps = {
    fieldValueName: "Id",
    fieldTextName: "name"
};

export default DropdownCellEditor;