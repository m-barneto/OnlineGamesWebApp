import React, { Component } from 'react';

const colors = [
  "rgb(131, 89, 149)",
  "rgb(82, 167, 134)",
  "rgb(0, 144, 166)",
  "rgb(239, 145, 62)",
  "rgb(218, 82, 101)",
  "rgb(230, 217, 67)",
];

export default class Cell extends Component {
  render() {
    return (
      <div className="tile" style={{ backgroundColor: colors[this.props.color] }} onClick={() => this.props.onClick(this.props.x, this.props.y, this.props.team)} />
    );
  }
}
