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
    console.log('on cell clicked: ' + x + ', ' + y);
  }

  componentWillUnmount() {
    if (this.authListener) {
      this.authListener();
    }
    this.authListener = undefined;
    this.ws.send("Closing...");
    this.ws.close();
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
            console.log(event.data);
          };

          fetch('http://localhost:5000/filler/getgamestate/' + this.state.id, {
            method: "GET",
            headers: { "auth": token }
          }).then(resp => resp.json())
            .then(data => {
              this.ws.send("Hello!");
              this.setState({ size: data["size"] });
              let boardArray = Array(data["size"]);
              for (let y = 0; y < data["size"]; ++y) {
                boardArray[y] = Array(data["size"]);
                for (let x = 0; x < data["size"]; ++x) {
                  boardArray[y][x] = <Cell x={x} y={y} color={data["board"][y * data["size"] + x]} onClick={this.onCellClick} key={x + ',' + y} team={0} />;
                }
              }
              this.setState({ board: boardArray });
            });
        });
      }
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