
import React, { Component } from 'react';
import {
  BrowserRouter as Router,
  Routes,
  Route
} from "react-router-dom";
import Homepage from './components/Homepage';
import Filler from './components/Filler';
import InvitePage from './components/InvitePage';
import Invite from './components/Invite';
import Welcome from './components/Welcome';
import Topbar from './components/Topbar';

export default class App extends Component {
  render() {
    return (
      <Router>

        <div className='app'>
          <Topbar />
          <Routes>
            <Route path='filler/:id' element={<Filler />} />
            <Route path="invite/:code" element={<Invite />} />
            <Route path="/" element={<Welcome />} />
          </Routes>
        </div>
      </Router>
    )
  }
}
