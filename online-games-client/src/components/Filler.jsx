import './filler.css';

import React, { Component } from 'react';
import Board from './Board';

export default class Filler extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div>
        <div className='game'>
          <div className="gameboard"
            style={
              { gridTemplateRows: "repeat(" + this.props.boardHeight + ", " + (900 / this.props.boardHeight) + "px)" },
              { gridTemplateColumns: "repeat(" + this.props.boardWidth + ", " + (900 / this.props.boardWidth) + "px)" }
            }
          >
            <Board width={9} height={9} gameId={0} />
          </div>
        </div>
      </div>
    )
  }
}
