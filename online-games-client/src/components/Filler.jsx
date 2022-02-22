import './filler.css';

import React, { Component } from 'react';
import Board from './Board';
import { auth } from '../firebase';

export default class Filler extends Component {
  constructor(props) {
    super(props);
  }

  updateBoardState() {
    console.log('update board');
    auth.currentUser.getIdToken().then((token) => {
      fetch('http://localhost:5000/filler/getgamestate/1', {
        method: "GET",
        headers: { "auth": 'dev' } // token
      }).then(resp => resp.json())
        .then(data => {

        });
    });
  }

  sendMove() {
    console.log('send board move');
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
            <Board width={9} height={9} gameId={0} data={this.state.data} />
            <button onClick={this.updateBoardState}>Update board state</button>
            <button onClick={this.sendMove}>Send move</button>
          </div>
        </div>
      </div>
    )
  }
}
