import React, { Component } from 'react';
import { signInWithPopup } from 'firebase/auth';
import { auth, provider } from '../firebase';
import './homepage.css';
import Filler from './Filler';

export default class Homepage extends Component {
  ws = null;

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
          this.ws = new WebSocket('ws://localhost:5000/user/ws?auth=' + token);
          this.ws.onopen = (event) => {
            console.log('connected to ws');
          };
          this.ws.onclose = (event) => {
            console.log('disconnected from ws');
          };
          this.ws.onmessage = (event) => {
            console.log(event.data);
          };
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
      <div>
        <div className='topbar' style={{ display: 'flex', justifyContent: 'flex-end', flexWrap: 'wrap' }}>
          <button onClick={this.state.user == null ? this.handleLogin : this.handleLogout}>{this.state.user == null ? 'Login' : 'Logout'}</button>
          <hr />
        </div>
        <div className='body'>
          {auth.currentUser
            ? <div>
              <h1>Currently logged in</h1>
            </div>

            : <div>
              <h1>Not logged in</h1>
            </div>
          }
          <h1 style={{ textAlign: 'center', fontSize: 50, color: '#fff' }}>Filler</h1>
          <Filler boardHeight={9} boardWidth={9} />
        </div>
      </div>
    )
  }
}
