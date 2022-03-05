import './filler.css';

import React, { Component } from 'react';
import { useParams } from 'react-router-dom';
import Board from './Board';
import Cell from './Cell';
import { auth } from '../firebase';

export function withRouter(Children) {
  return (props) => {
    const match = { params: useParams() };
    return <Children {...props} match={match} />
  }
}

class Filler extends Component {
  constructor(props) {
    super(props);
    const { id } = this.props.match.params;
    this.state = {
      board: null,
      id: id,
      size: 0
    };
  }

  onCellClick = (x, y) => {
    console.log("Clicked (" + x + ", " + y + "): " + this.state.board[y][x].props.color);
    auth.currentUser.getIdToken().then((token) => {
      fetch('http://localhost:5000/filler/move/' + this.state.id + "/" + this.state.board[y][x].props.color, {
        method: "POST",
        headers: { "auth": token }
      }).then(resp => {
        if (resp.status === 200) {
          this.updateBoard();
        }
      })
    });
  }

  componentWillUnmount() {
    if (this.authListener) {
      this.authListener();
    }
    this.authListener = undefined;
  }

  componentDidMount() {
    this.authListener = auth.onAuthStateChanged(user => {
      if (auth.currentUser != null) {
        auth.currentUser.getIdToken().then((token) => {
          this.ws = new WebSocket('ws://localhost:5000/user/ws?auth=' + token);
          this.ws.onopen = (event) => {
            console.log('connected to ws');
          };
          this.ws.onclose = (event) => {
            console.log('disconnected from ws');
          };
          this.ws.onmessage = (event) => {
            if (event.data === this.state.id) {
              this.updateBoard();
            }
          };
        });
        this.updateBoard();
      }
    });
  }

  updateBoard = () => {
    auth.currentUser.getIdToken().then((token) => {
      fetch('http://localhost:5000/filler/getgamestate/' + this.state.id, {
        method: "GET",
        headers: { "auth": token }
      }).then(resp => resp.json())
        .then(data => {
          this.setState({ size: data["size"] });
          let boardArray = Array(this.state.size);
          for (let y = 0; y < this.state.size; ++y) {
            boardArray[y] = Array(this.state.size);
            for (let x = 0; x < this.state.size; ++x) {
              boardArray[y][x] = <Cell x={x} y={y} color={data["board"][y * this.state.size + x]["color"]} onClick={this.onCellClick} key={x + ',' + y} />;
            }
          }
          this.setState({ board: boardArray });
        });
    });
  }

  render() {
    return (
      <div className='body'>
        <div className='game'>
          <div className="gameboard"
            style={
              {
                gridTemplateRows: "repeat(" + this.state.size + ", " + (900 / this.state.size) + "px)",
                gridTemplateColumns: "repeat(" + this.state.size + ", " + (900 / this.state.size) + "px)"
              }
            }
          >
            <Board width={this.state.size} height={this.state.size} board={this.state.board} />
          </div>
        </div>
      </div>
    )
  }
}

export default withRouter(Filler);