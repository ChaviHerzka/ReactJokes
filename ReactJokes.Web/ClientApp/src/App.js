import React, { Component } from 'react';
import { Route } from 'react-router';
import Signup from './Pages/Signup.js';
import Login from './Pages/Login';
import Home from './Pages/Home';
import Logout from './Pages/Logout';
import Layout from './Layout';
import { AuthContextComponent } from './AuthContext.js';
import ViewAll from './Pages/ViewAll.js';


export default class App extends Component {
  render () {
    return (
      <AuthContextComponent>
       <Layout>
          <Route exact path='/' component={Home}/>
           <Route exact path='/signup' component={Signup} />
           <Route path='/login' component={Login} />
           <Route path='/logout' component={Logout} />
           <Route  path='/viewall' component={ViewAll}/>
        </Layout>
     </AuthContextComponent>
    );
  }
}
