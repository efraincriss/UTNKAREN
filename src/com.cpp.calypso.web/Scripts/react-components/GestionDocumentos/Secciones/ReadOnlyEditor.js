import React, { Component } from "react"
import {
  EditorState,
  ContentState,
  convertFromHTML,
  convertToRaw,
} from "draft-js"
import { Editor } from "react-draft-wysiwyg"
import draftToHtml from "draftjs-to-html"

export class ReadOnlyEditor extends Component {
  constructor(props) {
    super(props)
    this.state = {
      Contenido: props.Contenido ? props.Contenido : "",
      editorState: EditorState.createEmpty(),
    }
  }

  componentWillReceiveProps(prevProps) {
    this.setState({
      Contenido: prevProps.Contenido ? prevProps.Contenido : "",
    }, this.createState)
  }

  onEditorStateChange = (editorState) => {
    /*this.setState({
      editorState,
    });*/
  }

  componentWillUnmount() {
    this.createState()
  }

  createState = () => {
    this.setState({
      editorState: EditorState.createWithContent(
        ContentState.createFromBlockArray(convertFromHTML(this.state.Contenido))
      ),
    })
  }

  render() {
    const { editorState } = this.state
    return (
      <div>
        <Editor
          editorState={editorState}
          wrapperClassName="demo-wrapper"
          editorClassName="demo-editor"
          onEditorStateChange={this.onEditorStateChange}
          toolbarHidden={true}
        />
      </div>
    )
  }
}
