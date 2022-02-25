import React, { Component } from 'react';


const colors = [
  "rgb(131, 89, 149)",
  "rgb(82, 167, 134)",
  "rgb(0, 144, 166)",
  "rgb(239, 145, 62)",
  "rgb(218, 82, 101)",
  "rgb(230, 217, 67)",
];

export default class Board extends Component {
  getRandomColor(max = colors.length) {
    return Math.floor((Math.random() * max));
  }

  onClick(x, y, team) {
    console.log(x, y, team);
  }

  render() {
    console.log('rendering');
    if (this.props.board == null) {
      console.log('null');
      return (
        <div>No board data</div>
      )
    }
    console.log('not null');
    return this.props.board.map((col, y) => col.map((item, x) => {
      return this.props.board[y][x];
    }));
  }
}
