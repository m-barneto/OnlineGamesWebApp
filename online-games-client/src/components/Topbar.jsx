import React, { Component } from 'react';
import { signInWithPopup } from 'firebase/auth';
import { auth, provider } from '../firebase';

export default class Topbar extends Component {
  constructor(props) {
    super(props);
    this.state = { user: null };
  }

  handleLogin = () => {
    signInWithPopup(auth, provider);
  }

  handleLogout = () => {
    auth.signOut();
  }

  authListener() {
    auth.onAuthStateChanged((user) => {
      this.setState({ user: user });
      if (this.state.user != null) {
        user.getIdToken().then((token) => {
          fetch('http://localhost:5000/user/login', {
            method: "PUT",
            headers: { "auth": token }
          });
        });
      }
    });
  }

  componentWillMount() {
    this.authListener = this.authListener.bind(this);
    this.authListener();
  }

  componentWillUnmount() {
    this.authListener && this.authListener();
    this.authListener = undefined;
  }

  render() {
    return (
      /*
#topBar {
  width: 100%;
  display: flex;
  flex-direction: row;
  align-items: right;
  justify-content: space-between;
}
      */
      <div className='topbar' style={{
        width: '100%',
        height: '60px',
        display: 'flex',
        flexDirection: 'row',
        justifyContent: 'flex-end',
        alignItems: 'right'
      }
      }>
        <div id='end' style={{
          minWidth: '200px',
          alignItems: 'center',
          justifyContent: 'flex-end'
        }}>
          {
            this.state.user
            && <img alt={this.state.user.displayName} title={this.state.user.displayName} src={this.state.user.photoURL} style={{ width: '50px', height: '50px', borderRadius: '50px', padding: '0' }} />
          }

          < button onClick={this.state.user == null ? this.handleLogin : this.handleLogout} style={{
            height: '50px',
            margin: '3px',
            borderRadius: '.75em'
          }}> {!this.state.user ? 'Login' : 'Logout'}</button >
        </div>

      </div >
    )
  }
}
