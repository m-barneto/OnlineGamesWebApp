import React, { Component } from 'react';
import { auth } from '../firebase';

export default class InvitePage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      code: null
    };
  }


  createInvite = (size) => {
    console.log(size);
    auth.currentUser.getIdToken().then((token) => {
      fetch('http://localhost:5000/invite/createinvite/' + size, {
        method: "GET",
        headers: { "auth": token } // token
      }).then(resp => resp.json()).then(data => {
        this.setState({ code: data["InviteCode"] })
      })
    });
  }

  render() {
    return (
      <div>
        <button onClick={() => { this.createInvite(9) }}>Create Invite Link</button>
        {this.state.code &&
          <a href={'./invite/' + this.state.code}>Open Link</a>
        }
      </div>
    )
  }
}