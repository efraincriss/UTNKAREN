import React from "react";
import { Tree } from 'primereact-v2/tree';

export default class HabitacionTree extends React.Component {

    constructor(props) {
        super(props)

    }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <Tree
                            style={{ with: '100%' }}
                            value={this.props.nodes}
                            selectionMode="single"
                            onSelect={this.props.onSelect}
                        />
                    </div>
                </div>
            </div>

        )
    }
}