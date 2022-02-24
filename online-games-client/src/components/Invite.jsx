import React, { Component } from 'react';
import { useParams } from 'react-router-dom';

export function withRouter(Children) {
  return (props) => {
    const match = { params: useParams() };
    return <Children {...props} match={match} />
  }
}

class Invite extends Component {
  render() {
    const { code } = this.props.match.params;

    return (
      <div>Invite {code}</div>
    )
  }
}

export default withRouter(Invite);

