import './filler.css';

import React, { Component } from 'react';
import Board from './Board';
import Cell from './Cell';
import { auth } from '../firebase';

export default class Filler extends Component {
  constructor(props) {
    super(props);
    this.state = {
      board: null
    };
  }

  updateBoardState = () => {
    auth.currentUser.getIdToken().then((token) => {
      fetch('http://localhost:5000/filler/getgamestate/1', {
        method: "GET",
        headers: { "auth": token } // token
      }).then(resp => resp.json())
        .then(data => {
          let boardArray = Array(data["size"]);
          for (let y = 0; y < data["size"]; ++y) {
            boardArray[y] = Array(data["size"]);
            for (let x = 0; x < data["size"]; ++x) {
              boardArray[y][x] = <Cell x={x} y={y} color={data["board"][y * data["size"] + x]} onClick={this.onClick} key={x + ',' + y} team={0} />;
            }
          }
          this.setState({ board: boardArray });
        });
    });
  }

  sendMove = () => {
    console.log('send board move');
  }

  createBoard = () => {
    auth.currentUser.getIdToken().then((token) => {
      fetch('http://localhost:5000/filler/creategame/9', {
        method: "POST",
        headers: { "auth": token } // token
      }).then(resp => {
        console.log(resp.status);
      })
    });
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
            <Board width={9} height={9} gameId={0} board={this.state.board} />

          </div>
          <button onClick={this.updateBoardState}>Update board state</button>
          <button onClick={this.sendMove}>Send move</button>
          <button onClick={this.createBoard}>Create Board</button>
        </div>
      </div>
    )
  }
}
