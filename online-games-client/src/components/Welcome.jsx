import React, { Component } from 'react';
import { auth } from '../firebase';

export default class Welcome extends Component {
  constructor(props) {
    super(props);
    this.state = {
      code: null
    };
  }

  componentDidMount() {
    this.authListener = auth.onAuthStateChanged(user => {
      this.forceUpdate();
    });
  }

  componentWillUnmount() {
    if (this.authListener) {
      this.authListener();
    }
    this.authListener = undefined;
  }

  createInvite = (size) => {
    console.log("Create invite");
    auth.currentUser.getIdToken().then((token) => {
      fetch('http://localhost:5000/invite/createinvite/' + size, {
        method: "GET",
        headers: { "auth": token }
      }).then(resp => resp.json()).then(data => {
        this.setState({ code: data["InviteCode"] })
      }).then(() => {
        navigator.clipboard.writeText("http://localhost:3000/invite/" + this.state.code);
      })
    });
  }

  createAiGame = () => {
    console.log("ai game");
  }

  render() {
    return (
      <div className='body'>
        {auth.currentUser
          ? <div>
            {this.state.code
              ? <h1>(Invite link copied.)</h1>
              : <div>
                <button onClick={() => { this.createInvite(9) }}>Create Invite</button>
                <button onClick={this.createAiGame}>Play Against AI</button>
              </div>
            }
          </div>

          : <div>
            <h1>Not logged in</h1>
          </div>
        }
      </div>
    )
  }
}
