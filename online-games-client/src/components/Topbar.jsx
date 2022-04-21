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

  imgErr(ev) {
    ev.target.src = 'https://cdn.discordapp.com/attachments/590552347441102848/966706967362019328/unknown.png';
  }

  render() {
    return (
      <div className='topbar' style={{
        minWidth: 'auto',
        width: '100%',
        height: '50px',
        display: 'flex',
        justifyContent: 'space-between',
      }
      }>
        <h1>FILLER</h1>
        {
          this.state.user
            ? <div style={{
              display: 'flex',
              flexDirection: 'row',
              justifyContent: 'space-around',
              minWidth: 'auto',
              width: '200px',
              alignItems: 'center'
            }}>
              <span className='material-icons'>notifications_none</span>
              <span className='material-icons'>mail</span>
              <img alt='' onError={this.imgErr} src={this.state.user.photoURL} style={{ width: '40px', height: '40px', borderRadius: '40px', padding: '0', border: '1px solid' }} />
              <button className='material-icons' onClick={this.handleLogout} style={{
                height: '50px',
                margin: '3px',
                borderRadius: '.75em',
                background: 'rgba(0,0,0,0)',
                borderColor: 'transparent',
                color: 'white'
              }}>logout</button>
            </div>
            : <div style={{
              display: 'flex',
              flexDirection: 'row',
              alignContent: 'stretch',
              minWidth: 'auto'
            }}>
              <button className='material-icons' onClick={this.handleLogin} style={{
                height: '50px',
                margin: '3px',
                borderRadius: '.75em',
                border: '0px',
                background: 'rgba(0,0,0,0)',
                borderColor: 'transparent',
                color: 'white'
              }}>login</button>
            </div>
        }
      </div>
    )
  }
}
