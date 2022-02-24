
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

export default class App extends Component {
  render() {
    return (
      <Router>
        <div className='app'>
          <Routes>
            <Route path='filler/:id' element={<Filler />} />
            <Route path="invite/:code" element={<Invite />} />
            <Route path="invite" element={<InvitePage />} />
            <Route path="/" element={<Homepage />} />
          </Routes>
        </div>
      </Router>
    )
  }
}
