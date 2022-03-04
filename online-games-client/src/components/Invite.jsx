import React, { Component } from 'react';
import { useParams } from 'react-router-dom';
import { auth } from '../firebase';

export function withRouter(Children) {
  return (props) => {
    const match = { params: useParams() };
    return <Children {...props} match={match} />
  }
}

class Invite extends Component {
  componentWillUnmount() {
    if (this.authListener) {
      this.authListener();
    }
    this.authListener = undefined;
  }


  componentDidMount() {
    this.authListener = auth.onAuthStateChanged(user => {
      if (auth.currentUser != null) {
        const { code } = this.props.match.params;
        auth.currentUser.getIdToken().then((token) => {
          fetch('http://localhost:5000/invite/joininvite/' + code, {
            method: "POST",
            headers: { "auth": token } // token
          }).then(resp => resp.json()).then(data => {
            console.log(data);
          })
        });
      }
    });
  }
  render() {
    const { code } = this.props.match.params;

    return (
      <div>Invite {code}</div>
    )
  }
}

export default withRouter(Invite);

