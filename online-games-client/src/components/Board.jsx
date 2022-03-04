import React, { Component } from 'react';

export default class Board extends Component {
  render() {
    if (this.props.board == null) {
      return (
        <div>No board data</div>
      )
    }
    return this.props.board.map((col, y) => col.map((item, x) => {
      return this.props.board[y][x];
    }));
  }
}
