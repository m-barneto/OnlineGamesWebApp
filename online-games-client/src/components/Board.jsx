import React, { Component } from 'react';
import Cell from './Cell';


const colors = [
  "rgb(131, 89, 149)",
  "rgb(82, 167, 134)",
  "rgb(0, 144, 166)",
  "rgb(239, 145, 62)",
  "rgb(218, 82, 101)",
  "rgb(230, 217, 67)",
];

export default class Board extends Component {
  constructor(props) {
    super(props);

    // let boardArray = Array(this.props.height);
    // for (let y = 0; y < this.props.height; ++y) {
    //   boardArray[y] = Array(this.props.width);
    //   for (let x = 0; x < this.props.width; ++x) {
    //     boardArray[y][x] = <Cell x={x} y={y} color={this.getRandomColor(colors.length)} onClick={this.onClick} key={x + ',' + y} team={0} />;
    //     //boardArray[y][x] = new Cell(y, x, this.getRandomColor(colors.length));
    //   }
    // }
  }

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
