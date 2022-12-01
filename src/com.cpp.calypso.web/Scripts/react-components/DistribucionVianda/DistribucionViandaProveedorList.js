import React, { Component } from 'react';



import { Tree } from 'primereact/components/tree/Tree';


class DistribucionViandaProveedorList extends React.Component {

    constructor() {
        super();
        this.state = {
            data: [],
            expandedKeys: {}
        };
    }

    componentDidUpdate(prevProps) {

        if (this.props.data !== prevProps.data ||
            this.props.dataAsignaciones !== prevProps.dataAsignaciones ) {
            this.mapProveedoresFlatToTree();
        }

          
    }

    mapProveedoresFlatToTree() {

        let data = [...this.props.data];
        let dataAsignaciones = [...this.props.dataAsignaciones];

        let groups = {};
        for (var i = 0; i < data.length; i++) {
            var groupName = data[i].zona_nombre;
            if (!groups[groupName]) {
                groups[groupName] = [];
            }

            const { Id, ProveedorId, razon_social } = data[i];
            let dataProveedor = dataAsignaciones.filter(item => item.ProveedorId === ProveedorId);
            const total = dataProveedor.reduce((prev, cur) => prev + cur.total_pedido, 0);
            let razon_social_total = razon_social + ' (' + total + ')';

            const item = Object.assign(data[i], { label: razon_social_total, nivel: 2, key: Id });
            groups[groupName].push(item);
        }

        console.log(groups);

        let tree = [];
        let expandedKeys = {}; 
        let j = -1;
        for (var zonaNombre in groups) {
            tree.push({ label: zonaNombre, nivel: 1, key: j, children: groups[zonaNombre] });
            expandedKeys[j]=true;
            j--;
        }

        this.setState({ data: tree, expandedKeys: expandedKeys});
    }
   

    render() {
        return (
            <div>
                <h6 className="p-3 mb-2 bg-primary">Proveedores</h6>

                <Tree value={this.state.data}
                    selectionMode="single"
                    selectionKeys={this.props.selectedNodeKey}
                    selectionChange={this.props.onSelectionChange}
                    onToggle={e => this.setState({ expandedKeys: e.value })}
                    expandedKeys={this.state.expandedKeys}
                    
                />

            </div>
        );
    }

}

export default DistribucionViandaProveedorList;