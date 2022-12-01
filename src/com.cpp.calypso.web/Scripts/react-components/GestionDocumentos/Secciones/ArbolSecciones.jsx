import React from "react"
import { Tree } from "primereact-v3.3/components/tree/Tree"
import { ContextMenu } from "primereact-v3.3/contextmenu"

export default class ArbolSecciones extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      nodes: null,
      selectedFile: null,
      selectedFile3: null,
      allkeys: {},
      menu: [
        {
          label: "Copiar a",
          icon: "pi pi-clone",
          command: () => {
            this.props.MostrarDialogo()
          },
        },
      ],
    }
  }

  render() {
    return (
      <div>
        <ContextMenu
          appendTo={document.body}
          model={this.state.menu}
          ref={(el) => (this.cm = el)}
        />
        <Tree
          style={{ with: "100%",scrollY:100 }}
          value={this.props.data}
          expandedKeys={this.props.expandedKeys}
          dragdropScope="demo"
          onDragDrop={this.props.onDragDrop}
          selectionMode="single"
          selectionKeys={this.state.selectedFile}
          onSelect={this.props.onSelect}
          onToggle={this.props.onToggle}
          onContextMenuSelectionChange={this.props.onContextMenuSelectionChange}
          // onContextMenu={event => this.cm.show(event.originalEvent)}
          contextMenuSelectionKey={(event) => console.log(event)}
        />
      </div>
    )
  }
}
