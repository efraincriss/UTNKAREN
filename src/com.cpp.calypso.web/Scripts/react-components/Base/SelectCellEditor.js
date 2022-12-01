import React from 'react';
import PropTypes from 'prop-types';


class SelectCellEditor extends React.Component {

    constructor(props) {
        super(props);
        this.updateData = this.updateData.bind(this);
        this.state = {
            value: props.defaultValue 
        };
    }

    focus() {
        this.refs.inputRef.focus();
    }

    updateData(event) {

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
        }
    }

    
    render() {
        
        let options = this.props.data.map(item => (<option key={item[this.props.fieldValueName]} value={item[this.props.fieldTextName]}>{item[this.props.fieldTextName]}</option>));

        return (
            <span>
                 <select
                    ref='inputRef'
                    value={this.state.value}
                    onKeyDown={this.props.onKeyDown}
                    onChange={this.updateData}
                    className="form-control"
                 >
                    {this.props.textSelect && <option value=''>{this.props.textSelect}</option>}
                    {options}
                 </select>
            </span>
        );
}


}


SelectCellEditor.propTypes = {
    fieldTextName: PropTypes.string,
    fieldValueName: PropTypes.string,
    onUpdateData: PropTypes.func,
    textSelect: PropTypes.string    
};

SelectCellEditor.defaultProps = {
    fieldValueName: "Id",
    fieldTextName: "name"
};

export default SelectCellEditor;