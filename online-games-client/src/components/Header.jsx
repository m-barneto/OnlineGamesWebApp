import React, { Component } from 'react';
import { signInWithPopup } from 'firebase/auth';
import { auth, provider } from '../firebase';

export default class Header extends Component {
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
      <div className='topbar' style={{ display: 'flex', justifyContent: 'flex-end', flexWrap: 'wrap' }}>
        {this.state.user
          && <img alt={this.state.user.displayName} title={this.state.user.displayName} src={this.state.user.photoURL} style={{ width: '50px', height: '50px', borderRadius: '50px' }} />
        }

        <button onClick={this.state.user == null ? this.handleLogin : this.handleLogout}>{!this.state.user ? 'Login' : 'Logout'}</button>
        <hr />
      </div>
    )
  }
}
